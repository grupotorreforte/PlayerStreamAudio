using Prometheus;
using Serilog;

namespace StreamAudioPlayer.Services
{
    public static class Observability
    {
        private static readonly Counter StreamReconexoes = Metrics.CreateCounter("radio_stream_reconnections_total", "Número total de reconexões realizadas");
        private static readonly Counter StreamFalhas = Metrics.CreateCounter("radio_stream_failures_total", "Número total de falhas detectadas");
        private static readonly Gauge StreamOnline = Metrics.CreateGauge("radio_stream_online", "Indica se o stream está ativo (1) ou offline (0)");
        private static readonly Gauge UptimeSegundos = Metrics.CreateGauge("radio_stream_uptime_seconds", "Tempo total de reprodução (segundos)");

        private static DateTime _startTime;
        public static void Iniciar()
        {
            Log.Information("Observabilidade inicializada");
            _startTime = DateTime.Now;
            _ = Task.Run(() =>
            {
                var server = new MetricServer(port: 9091);
                // servidor do Prometheus para vermos os logs http://localhost:9090/metrics
                server.Start();
                Log.Information("Prometheus Metrics expostas em http://localhost:9090/metrics");
            });
        }
        public static void RegistrarFalha(string motivo)
        {
            StreamFalhas.Inc();
            Log.Warning("Falha detectada: {motivo}", motivo);
        }
        public static void RegistrarReconexao()
        {
            StreamReconexoes.Inc();
            Log.Information("Reconexão realizada com sucesso.");
        }
        public static void AtualizarStatus(bool online)
        {
            StreamOnline.Set(online ? 1 : 0);
            Log.Information("Status do stream alterado para: {status}", online ? "ONLINE" : "OFFLINE");
        }
        public static void AtualizarUptime()
        {
            var uptime = (DateTime.Now - _startTime).TotalSeconds;
            UptimeSegundos.Set(uptime);
        }
    }
}