namespace StreamAudioPlayer
{
    partial class FrmPrincipal
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrincipal));
            trackVolume = new TrackBar();
            textBox1 = new TextBox();
            lstBox = new ListBox();
            timerCheck = new System.Windows.Forms.Timer(components);
            btnPlay = new PictureBox();
            btnStop = new PictureBox();
            lblUptime = new Label();
            btnRecolher = new Button();
            btnLog = new Button();
            bntVolume = new PictureBox();
            label1 = new Label();
            btnTestFalha = new Button();
            btnTestOffline = new Button();
            btnTestReconexao = new Button();
            btnTestOnline = new Button();
            ((System.ComponentModel.ISupportInitialize)trackVolume).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnPlay).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnStop).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bntVolume).BeginInit();
            SuspendLayout();
            // 
            // trackVolume
            // 
            trackVolume.AutoSize = false;
            trackVolume.BackColor = SystemColors.ControlDarkDark;
            trackVolume.Location = new Point(525, 115);
            trackVolume.Maximum = 100;
            trackVolume.Name = "trackVolume";
            trackVolume.Size = new Size(195, 30);
            trackVolume.TabIndex = 0;
            trackVolume.TickStyle = TickStyle.None;
            trackVolume.Value = 100;
            trackVolume.Scroll += trackVolume_Scroll;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(14, 1);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(460, 35);
            textBox1.TabIndex = 0;
            // 
            // lstBox
            // 
            lstBox.ForeColor = SystemColors.ActiveCaptionText;
            lstBox.FormattingEnabled = true;
            lstBox.Location = new Point(14, 214);
            lstBox.Name = "lstBox";
            lstBox.Size = new Size(707, 264);
            lstBox.TabIndex = 2;
            // 
            // btnPlay
            // 
            btnPlay.BackColor = Color.Transparent;
            btnPlay.Image = Properties.Resources.playBranco;
            btnPlay.Location = new Point(541, 21);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(75, 73);
            btnPlay.SizeMode = PictureBoxSizeMode.StretchImage;
            btnPlay.TabIndex = 3;
            btnPlay.TabStop = false;
            btnPlay.Click += btnPlayPict_Click;
            // 
            // btnStop
            // 
            btnStop.BackColor = Color.Transparent;
            btnStop.Image = Properties.Resources.stopBranco;
            btnStop.Location = new Point(622, 21);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(77, 73);
            btnStop.SizeMode = PictureBoxSizeMode.StretchImage;
            btnStop.TabIndex = 5;
            btnStop.TabStop = false;
            btnStop.Click += btnStop_Click_1;
            // 
            // lblUptime
            // 
            lblUptime.BackColor = Color.Transparent;
            lblUptime.Font = new Font("Segoe UI", 48F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblUptime.ForeColor = Color.White;
            lblUptime.Location = new Point(12, 1);
            lblUptime.Name = "lblUptime";
            lblUptime.Size = new Size(475, 105);
            lblUptime.TabIndex = 6;
            // 
            // btnRecolher
            // 
            btnRecolher.BackColor = Color.Transparent;
            btnRecolher.FlatAppearance.BorderColor = Color.White;
            btnRecolher.FlatAppearance.MouseOverBackColor = Color.Red;
            btnRecolher.FlatStyle = FlatStyle.Flat;
            btnRecolher.Font = new Font("Segoe UI Semibold", 7.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRecolher.ForeColor = SystemColors.ButtonHighlight;
            btnRecolher.Location = new Point(626, 446);
            btnRecolher.Name = "btnRecolher";
            btnRecolher.Size = new Size(92, 29);
            btnRecolher.TabIndex = 10;
            btnRecolher.Text = "Limpar Log";
            btnRecolher.UseVisualStyleBackColor = false;
            btnRecolher.Click += btnRecolher_Click;
            // 
            // btnLog
            // 
            btnLog.BackColor = Color.Transparent;
            btnLog.FlatAppearance.MouseDownBackColor = SystemColors.MenuText;
            btnLog.FlatAppearance.MouseOverBackColor = Color.Black;
            btnLog.FlatStyle = FlatStyle.Flat;
            btnLog.Font = new Font("Segoe UI Semibold", 7.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLog.ForeColor = SystemColors.ButtonHighlight;
            btnLog.Location = new Point(628, 151);
            btnLog.Name = "btnLog";
            btnLog.Size = new Size(92, 28);
            btnLog.TabIndex = 11;
            btnLog.Text = "Exibir Log";
            btnLog.UseVisualStyleBackColor = false;
            btnLog.Click += btnFechaAbreLog_Click;
            // 
            // bntVolume
            // 
            bntVolume.BackColor = Color.Transparent;
            bntVolume.Image = Properties.Resources.volume;
            bntVolume.Location = new Point(475, 110);
            bntVolume.Name = "bntVolume";
            bntVolume.Size = new Size(37, 36);
            bntVolume.SizeMode = PictureBoxSizeMode.StretchImage;
            bntVolume.TabIndex = 12;
            bntVolume.TabStop = false;
            bntVolume.Click += bntVolume_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ButtonHighlight;
            label1.Location = new Point(35, 121);
            label1.Name = "label1";
            label1.Size = new Size(267, 20);
            label1.TabIndex = 13;
            label1.Text = "https://stm19.srvstm.com:7080/stream";
            // 
            // btnTestFalha
            // 
            btnTestFalha.Location = new Point(74, 174);
            btnTestFalha.Name = "btnTestFalha";
            btnTestFalha.Size = new Size(94, 29);
            btnTestFalha.TabIndex = 14;
            btnTestFalha.Text = "FALHA";
            btnTestFalha.UseVisualStyleBackColor = true;
            btnTestFalha.Visible = false;
            btnTestFalha.Click += btnTestFalha_Click_1;
            // 
            // btnTestOffline
            // 
            btnTestOffline.Location = new Point(308, 174);
            btnTestOffline.Name = "btnTestOffline";
            btnTestOffline.Size = new Size(109, 29);
            btnTestOffline.TabIndex = 14;
            btnTestOffline.Text = "TEST OFFLINE";
            btnTestOffline.UseVisualStyleBackColor = true;
            btnTestOffline.Visible = false;
            btnTestOffline.Click += btnTestOffline_Click_1;
            // 
            // btnTestReconexao
            // 
            btnTestReconexao.Location = new Point(174, 174);
            btnTestReconexao.Name = "btnTestReconexao";
            btnTestReconexao.Size = new Size(128, 29);
            btnTestReconexao.TabIndex = 14;
            btnTestReconexao.Text = "TESTE RECONEC";
            btnTestReconexao.UseVisualStyleBackColor = true;
            btnTestReconexao.Visible = false;
            btnTestReconexao.Click += btnTestReconexao_Click_1;
            // 
            // btnTestOnline
            // 
            btnTestOnline.Location = new Point(423, 174);
            btnTestOnline.Name = "btnTestOnline";
            btnTestOnline.Size = new Size(133, 29);
            btnTestOnline.TabIndex = 15;
            btnTestOnline.Text = "TESTE ONLIN";
            btnTestOnline.UseVisualStyleBackColor = true;
            btnTestOnline.Visible = false;
            btnTestOnline.Click += btnTestOnline_Click_1;
            // 
            // FrmPrincipal
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            BackgroundImage = Properties.Resources.fundoAzul2;
            ClientSize = new Size(736, 494);
            Controls.Add(btnTestOnline);
            Controls.Add(btnTestReconexao);
            Controls.Add(btnTestOffline);
            Controls.Add(btnTestFalha);
            Controls.Add(label1);
            Controls.Add(btnRecolher);
            Controls.Add(btnLog);
            Controls.Add(lstBox);
            Controls.Add(bntVolume);
            Controls.Add(lblUptime);
            Controls.Add(trackVolume);
            Controls.Add(btnStop);
            Controls.Add(btnPlay);
            Controls.Add(textBox1);
            ForeColor = SystemColors.ControlText;
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FrmPrincipal";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Player de Áudio";
            FormClosing += FrmPrincipal_FormClosing;
            Load += FrmPrincipal_Load;
            ((System.ComponentModel.ISupportInitialize)trackVolume).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnPlay).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnStop).EndInit();
            ((System.ComponentModel.ISupportInitialize)bntVolume).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private ListBox lstBox;
        private System.Windows.Forms.Timer timerCheck;
        private PictureBox btnPlay;
        private PictureBox btnStop;
        internal TrackBar trackVolume;
        private Button btnRecolher;
        private Button btnTestOnline;
        private Button btnLog;
        private PictureBox bntVolume;
        private Label label1;
        private Button btnTestFalha;
        private Button btnTestOffline;
        private Button btnTestReconexao;
    }
}
