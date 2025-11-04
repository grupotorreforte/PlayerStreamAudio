using NAudio.Wave;
using StreamAudioPlayer.Services;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading;

namespace StreamAudioPlayer
{
    public partial class FrmPrincipal : Form
    {
        private readonly string defaultStreamUrl = "https://stm19.srvstm.com:7080/stream";

        private IWavePlayer? waveOut;
        private MediaFoundationReader? mediaReader;
        private string streamUrl = "";
        private bool isPlaying = false;
        private bool isReconnecting = false;


        private readonly int maxRetryDelaySeconds = 30;

        private readonly System.Windows.Forms.Timer networkTimer;

        private System.Windows.Forms.Timer playbackTimer;
        private Label lblUptime;
        private DateTime playbackStartTime;
        private Label lblStream;
        private bool logsVisiveis = true;

        private bool playerCompacto = false;
        private int alturaExpandida;
        private int alturaSemLogs = 250;

        private bool isMuted = false;
        private float lastVolume = 1.0f;

        private readonly HttpClient _httpClient;
        private CancellationTokenSource? _monitorCts;
        private Task? _monitorTarefa;
        private readonly object _monitorLock = new object();
        private bool _monitorRodando = false;
        private int _consecutiveFailuresThreshold = 3;
        private TimeSpan _monitorIntervalo = TimeSpan.FromSeconds(8);
        private TimeSpan _reconnectIntervalBase = TimeSpan.FromSeconds(5);

        public FrmPrincipal()
        {
            InitializeComponent();


            _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };

            timerCheck.Interval = 5000;
            timerCheck.Tick += TimerCheck_Tick;

            btnPlay.MouseEnter += BtnPlay_MouseEnter;
            btnPlay.MouseLeave += BtnPlay_MouseLeave;

            btnStop.MouseEnter += BtnStop_MouseEnter;
            btnStop.MouseLeave += BtnStop_MouseLeave;

            networkTimer = new System.Windows.Forms.Timer();
            networkTimer.Interval = 10000;
            networkTimer.Tick += NetworkTimer_Tick;
            networkTimer.Start();

            NetworkChange.NetworkAvailabilityChanged += NetworkAvailabilityChangedHandler;

            playbackTimer = new System.Windows.Forms.Timer();
            playbackTimer.Interval = 1000;
            playbackTimer.Tick += PlaybackTimer_Tick;
        }

        private void FrmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Tem certeza que deseja sair?",
                "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                PararStream();
                ParaMonitoramento();
                _httpClient.Dispose();
            }
        }
        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
                textBox1.Text = defaultStreamUrl;

            streamUrl = textBox1.Text.Trim();
            IniciaStream();

            alturaExpandida = this.Height;

            btnLog.Visible = true;
            btnLog.Text = "Fechar";
        }

        //Aqui estou mapeando o tempo de reprodução que ficou
        private void PlaybackTimer_Tick(object? sender, EventArgs e)
        {
            var uptime = DateTime.Now - playbackStartTime;
            lblUptime.Text = $"{uptime:hh\\:mm\\:ss}";
        }
        //captura dos logs de desconexo~~ao do audio
        private void Log(string message)
        {
            if (lstBox.InvokeRequired)
            {
                lstBox.Invoke(() => Log(message));
                return;
            }
            lstBox.Items.Add($"[{DateTime.Now:HH:mm:ss}] {message}");
            lstBox.TopIndex = lstBox.Items.Count - 1;
        }

        //Aqui fiz a captura do volume do audio com o trackBar e Scroll
        private void trackVolume_Scroll(object sender, EventArgs e)
        {
            if (waveOut != null)
            {
                float volume = trackVolume.Value / 100f;
                waveOut.Volume = volume;
            }

            if (trackVolume.Value == 0)
            {
                bntVolume.Image = Properties.Resources.volumeMudo;
                isMuted = true;
                Log("🔇 Áudio mutado.");
            }
            else
            {
                bntVolume.Image = Properties.Resources.volume;
                isMuted = false;
                lastVolume = trackVolume.Value / 100f;
            }
        }

        #region BOTÕES DO PLAYER
        private void btnPlayPict_Click(object sender, EventArgs e)
        {
            streamUrl = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(streamUrl))
            {
                MessageBox.Show("Informe a URL do stream.");
                return;
            }

            IniciaStream();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (waveOut != null && waveOut.PlaybackState == PlaybackState.Playing)
            {
                waveOut.Pause();
                playbackTimer.Stop();
                Log("⏸️ Reprodução pausada.");
            }
            else if (waveOut != null && waveOut.PlaybackState == PlaybackState.Paused)
            {
                waveOut.Play();
                playbackTimer.Start();
                Log("▶️ Reprodução retomada.");
            }
        }

        private void btnStop_Click_1(object sender, EventArgs e)
        {
            PararStream();
            Log("⏹️ Reprodução parada.");
        }

        private void bntVolume_Click(object sender, EventArgs e)
        {
            if (waveOut == null)
                return;

            if (!isMuted)
            {
                lastVolume = trackVolume.Value / 100f;
                waveOut.Volume = 0f;
                trackVolume.Value = 0;
                bntVolume.Image = Properties.Resources.volumeMudo;
                isMuted = true;
                Log("🔇 Áudio mutado.");
            }
            else
            {
                float restoreVolume = lastVolume > 0 ? lastVolume : 1.0f;
                int restoreTrackValue = (int)(restoreVolume * 100);

                waveOut.Volume = restoreVolume;
                trackVolume.Value = restoreTrackValue;
                bntVolume.Image = Properties.Resources.volume;
                isMuted = false;
                Log($"🔊 Áudio reativado ({restoreTrackValue}%).");
            }
        }

        private void btnRecolher_Click(object sender, EventArgs e)
        {
            lstBox.Items.Clear();
        }

        private void btnFechaAbreLog_Click(object sender, EventArgs e)
        {
            if (logsVisiveis)
            {
                logsVisiveis = false;
                playerCompacto = true;

                var timer = new System.Windows.Forms.Timer();
                timer.Interval = 10;
                timer.Tick += (s, _) =>
                {
                    if (this.Height > alturaSemLogs)
                        this.Height -= 15;
                    else
                    {
                        this.Height = alturaSemLogs;
                        timer.Stop();
                    }
                };
                timer.Start();

                lstBox.Visible = false;
                btnRecolher.Visible = false;
                btnLog.Text = "Exibir Log";
            }
            else
            {

                logsVisiveis = true;
                playerCompacto = false;
                btnLog.Visible = false;
                btnRecolher.Visible = true;
                lstBox.Visible = true;

                int alturaFinal = alturaExpandida;

                var timer = new System.Windows.Forms.Timer();
                timer.Interval = 10;
                timer.Tick += (s, _) =>
                {
                    if (this.Height < alturaFinal)
                        this.Height += 15;
                    else
                    {
                        this.Height = alturaFinal;
                        timer.Stop();
                    }
                };
                timer.Start();
                btnLog.Text = "Ocultar Log";
                btnLog.Visible = true;

            }
        }
        #endregion

        #region BOTOES + HOVERS
        //BOTÕES DE HOVER
        private void BtnPlay_MouseEnter(object? sender, EventArgs e)
        {
            btnPlay.Image = Properties.Resources.playHover;
        }
        private void BtnPlay_MouseLeave(object? sender, EventArgs e)
        {
            btnPlay.Image = Properties.Resources.playBranco;
        }
        private void BtnStop_MouseEnter(object? sender, EventArgs e)
        {
            btnStop.Image = Properties.Resources.stopHover;
        }
        private void BtnStop_MouseLeave(object? sender, EventArgs e)
        {
            btnStop.Image = Properties.Resources.stopBranco;
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            PararStream();
            Log("⏹️ Reprodução parada.");
        }

        #endregion

        #region INICIO E PARADA DE STREAM DO AUDIO
        private void IniciaStream()
        {
            PararStream();
            try
            {
                Log("Iniciando Retransmissão de cabeça de Rede - 89.1 Maravilha FM...");
                Observability.AtualizarStatus(true);

                mediaReader = new MediaFoundationReader(streamUrl);
                waveOut = new WaveOutEvent();
                waveOut.Init(mediaReader);
                waveOut.Play();

                playbackStartTime = DateTime.Now;
                playbackTimer.Start();

                Observability.AtualizarUptime();
            }
            catch (Exception ex)
            {
                Log($"Erro ao iniciar: {ex.Message}");
                Observability.RegistrarFalha(ex.Message);
            }
        }

        private void PararStream()
        {
            timerCheck.Stop();
            playbackTimer.Stop();

            try
            {
                waveOut?.Stop();
                waveOut?.Dispose();
                waveOut = null;

                mediaReader?.Dispose();
                mediaReader = null;
            }
            catch { }

            lblUptime.Text = "00:00:00";
            isPlaying = false;
        }
        #endregion

        private async void TimerCheck_Tick(object? sender, EventArgs e)
        {
            if (!isPlaying && !isReconnecting)
                return;

            try
            {
                if (mediaReader == null || waveOut == null || waveOut.PlaybackState != PlaybackState.Playing)
                {
                    Log("⚠️ Stream parada. Acionando monitor para reconexão...");
                    IniciandoMonitoramento();
                    return;
                }


                bool alcance = await IsStreamReachableAsync(streamUrl).ConfigureAwait(false);
                if (!alcance)
                {
                    Log("🚫 Sem resposta. Iniciando reconexão controlada...");
                    IniciandoMonitoramento();
                }
            }
            catch (Exception ex)
            {
                Log($"Erro no monitor: {ex.Message}");
                IniciandoMonitoramento();
            }
        }

        #region INICIANDO E PARANDO MONITORAMENTO
        private void IniciandoMonitoramento()
        {
            lock (_monitorLock)
            {
                if (_monitorRodando) return;
                _monitorRodando = true;
                _monitorCts = new CancellationTokenSource();
                _monitorTarefa = Task.Run(() => MonitoramentoDoLoopAsync(_monitorCts.Token));
            }
        }

        private void ParaMonitoramento()
        {
            lock (_monitorLock)
            {
                try
                {
                    _monitorCts?.Cancel();
                }
                catch { }
                _monitorCts?.Dispose();
                _monitorCts = null;
                _monitorTarefa = null;
                _monitorRodando = false;
            }
        }
        #endregion

        private async Task MonitoramentoDoLoopAsync(CancellationToken ct)
        {
            int falhasConsecutivas = 0;
            try
            {
                while (!ct.IsCancellationRequested)
                {
                    bool alcance = await IsStreamReachableAsync(streamUrl).ConfigureAwait(false);

                    if (alcance)
                    {
                        falhasConsecutivas = 0;
                    }
                    else
                    {
                        falhasConsecutivas++;
                    }


                    if (falhasConsecutivas >= _consecutiveFailuresThreshold)
                    {
                        this.Invoke(() =>
                        {
                            if (isPlaying)
                            {
                                Log("🚫 Queda confirmada: stream fora do ar. Parando player e iniciando reconexão...");
                                PararStream();
                            }
                        });

                        await IniciarReconexaoControladaAsync(ct);
                        falhasConsecutivas = 0;
                    }
                    else
                    {
                        await Task.Delay(_monitorIntervalo, ct).ConfigureAwait(false);
                    }
                    if (falhasConsecutivas >= _consecutiveFailuresThreshold)
                    {

                        this.Invoke(() =>
                        {
                            if (isPlaying)
                            {
                                Log("🚫 Queda detectada. Parando player e iniciando rotina de reconexão...");
                                PararStream();
                            }
                        });

                        int tentando = 0;
                        int delaySeconds = (int)_reconnectIntervalBase.TotalSeconds;
                        while (!ct.IsCancellationRequested)
                        {
                            tentando++;
                            Log($"🔁 Tentativa de reconexão #{tentando} em {delaySeconds}s...");
                            try
                            {
                                await Task.Delay(TimeSpan.FromSeconds(delaySeconds), ct).ConfigureAwait(false);
                            }
                            catch (TaskCanceledException) { break; }

                            bool atual = await IsStreamReachableAsync(streamUrl).ConfigureAwait(false);
                            if (atual)
                            {
                                this.Invoke(() =>
                                {
                                    Log("✅ Reconectado. Reiniciando stream...");
                                    IniciaStream();
                                });
                                falhasConsecutivas = 0;
                                break;
                            }
                            else
                            {
                                delaySeconds = Math.Min(delaySeconds * 2, maxRetryDelaySeconds);
                                Log($"⏳ Ainda indisponível. Próxima tentativa em {delaySeconds}s...");
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            await Task.Delay(_monitorIntervalo, ct).ConfigureAwait(false);
                        }
                        catch (TaskCanceledException) { break; }
                    }
                }
            }
            catch (OperationCanceledException) { /* cancelado - silencioso */ }
            catch (Exception ex)
            {
                Log($"Erro no monitor: {ex.Message}");
            }
            finally
            {
                lock (_monitorLock)
                {
                    _monitorRodando = false;
                    _monitorCts?.Dispose();
                    _monitorCts = null;
                    _monitorTarefa = null;
                }
            }
        }
        private async Task IniciarReconexaoControladaAsync(CancellationToken ct)
        {
            int tentativa = 0;
            int delay = (int)_reconnectIntervalBase.TotalSeconds;

            while (!ct.IsCancellationRequested)
            {
                tentativa++;
                Log($"🔁 Tentativa de reconexão #{tentativa} em {delay}s...");

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(delay), ct).ConfigureAwait(false);
                }
                catch (TaskCanceledException)
                {
                    break;
                }

                bool ok = await IsStreamReachableAsync(streamUrl).ConfigureAwait(false);
                if (ok)
                {
                    Observability.RegistrarReconexao();
                    Observability.AtualizarStatus(true);
                    this.Invoke(() =>
                    {
                        Log("✅ Reconectado. Reiniciando stream...");
                        IniciaStream();
                    });
                    break;
                }

                delay = Math.Min(delay * 2, maxRetryDelaySeconds);
            }
        }
        private async Task<bool> IsStreamReachableAsync(string url)
        {
            try
            {
                using var req = new HttpRequestMessage(HttpMethod.Head, url);
                var resp = await _httpClient.SendAsync(req, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                return resp.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Observability.RegistrarFalha(ex.Message);
                return false;
            }
        }
        private void NetworkAvailabilityChangedHandler(object? sender, NetworkAvailabilityEventArgs e)
        {
            if (e.IsAvailable)
            {
                this.Invoke(() =>
                {
                    Log("🌐 Conexão de rede detectada novamente. Solicitando reconexão controlada...");
                    IniciandoMonitoramento();
                });
            }
            else
            {
                this.Invoke(() =>
                {
                    Log("📴 Conexão de rede perdida.");
                });
            }
        }
        private async void NetworkTimer_Tick(object? sender, EventArgs e)
        {
            if (_monitorRodando)
                return;

            bool alcance = await IsStreamReachableAsync(streamUrl);
            if (!alcance && isPlaying)
            {
                Log("❌ Conexão perdida. Iniciando reconexão controlada...");
                IniciandoMonitoramento();
            }
        }

        #region TESTES EM MASSA PARA PROMETHEUS LOGS
        private void btnTestFalha_Click_1(object sender, EventArgs e)
        {
            Log("🔴 Simulando falha manual...");
            Observability.RegistrarFalha("Teste manual de falha");
        }

        private void btnTestReconexao_Click_1(object sender, EventArgs e)
        {
            Log("🟢 Simulando reconexão manual...");
            Observability.RegistrarReconexao();
        }

        private void btnTestOffline_Click_1(object sender, EventArgs e)
        {
            Log("🟡 Simulando status OFFLINE...");
            Observability.AtualizarStatus(false);
        }

        private void btnTestOnline_Click_1(object sender, EventArgs e)
        {
            Log("🟢 Simulando status ONLINE...");
            Observability.AtualizarStatus(true);
        }
        #endregion
    }
}
