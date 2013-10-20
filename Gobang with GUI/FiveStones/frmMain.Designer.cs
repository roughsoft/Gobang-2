namespace FiveStones
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.picBoard = new System.Windows.Forms.PictureBox();
            this.grpBlack = new System.Windows.Forms.GroupBox();
            this.txtBlackNet = new System.Windows.Forms.TextBox();
            this.lblBlackTime = new System.Windows.Forms.Label();
            this.radBlackNet = new System.Windows.Forms.RadioButton();
            this.txtBlackHuman = new System.Windows.Forms.TextBox();
            this.radBlackHuman = new System.Windows.Forms.RadioButton();
            this.radBlackAI = new System.Windows.Forms.RadioButton();
            this.picBlack = new System.Windows.Forms.PictureBox();
            this.grpWhite = new System.Windows.Forms.GroupBox();
            this.txtWhiteNet = new System.Windows.Forms.TextBox();
            this.lblWhiteTime = new System.Windows.Forms.Label();
            this.radWhiteNet = new System.Windows.Forms.RadioButton();
            this.txtWhiteHuman = new System.Windows.Forms.TextBox();
            this.radWhiteHuman = new System.Windows.Forms.RadioButton();
            this.radWhiteAI = new System.Windows.Forms.RadioButton();
            this.picWhite = new System.Windows.Forms.PictureBox();
            this.lstHistory = new System.Windows.Forms.ListBox();
            this.btnGame = new System.Windows.Forms.Button();
            this.btnFile = new System.Windows.Forms.Button();
            this.txtTimeLimit = new System.Windows.Forms.TextBox();
            this.lblTimeLimit = new System.Windows.Forms.Label();
            this.chkTimeLimit = new System.Windows.Forms.CheckBox();
            this.dlgOpenGame = new System.Windows.Forms.OpenFileDialog();
            this.dlgSaveGame = new System.Windows.Forms.SaveFileDialog();
            this.timerLimit = new System.Windows.Forms.Timer(this.components);
            this.trkVideo = new System.Windows.Forms.TrackBar();
            this.lblVideo = new System.Windows.Forms.Label();
            this.timerVideo = new System.Windows.Forms.Timer(this.components);
            this.timerSynchronize = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picBoard)).BeginInit();
            this.grpBlack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBlack)).BeginInit();
            this.grpWhite.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWhite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkVideo)).BeginInit();
            this.SuspendLayout();
            // 
            // picBoard
            // 
            this.picBoard.Enabled = false;
            this.picBoard.Image = ((System.Drawing.Image)(resources.GetObject("picBoard.Image")));
            this.picBoard.Location = new System.Drawing.Point(12, 12);
            this.picBoard.Name = "picBoard";
            this.picBoard.Size = new System.Drawing.Size(600, 600);
            this.picBoard.TabIndex = 0;
            this.picBoard.TabStop = false;
            this.picBoard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picBoard_MouseDown);
            this.picBoard.Paint += new System.Windows.Forms.PaintEventHandler(this.picBoard_Paint);
            // 
            // grpBlack
            // 
            this.grpBlack.Controls.Add(this.txtBlackNet);
            this.grpBlack.Controls.Add(this.lblBlackTime);
            this.grpBlack.Controls.Add(this.radBlackNet);
            this.grpBlack.Controls.Add(this.txtBlackHuman);
            this.grpBlack.Controls.Add(this.radBlackHuman);
            this.grpBlack.Controls.Add(this.radBlackAI);
            this.grpBlack.Controls.Add(this.picBlack);
            this.grpBlack.Location = new System.Drawing.Point(629, 12);
            this.grpBlack.Name = "grpBlack";
            this.grpBlack.Size = new System.Drawing.Size(131, 134);
            this.grpBlack.TabIndex = 1;
            this.grpBlack.TabStop = false;
            this.grpBlack.Text = "黑子先手";
            // 
            // txtBlackNet
            // 
            this.txtBlackNet.Enabled = false;
            this.txtBlackNet.Location = new System.Drawing.Point(62, 110);
            this.txtBlackNet.Name = "txtBlackNet";
            this.txtBlackNet.Size = new System.Drawing.Size(63, 20);
            this.txtBlackNet.TabIndex = 6;
            this.txtBlackNet.Text = "127.0.0.1";
            // 
            // lblBlackTime
            // 
            this.lblBlackTime.AutoSize = true;
            this.lblBlackTime.Enabled = false;
            this.lblBlackTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBlackTime.Location = new System.Drawing.Point(52, 32);
            this.lblBlackTime.Name = "lblBlackTime";
            this.lblBlackTime.Size = new System.Drawing.Size(71, 29);
            this.lblBlackTime.TabIndex = 5;
            this.lblBlackTime.Text = "00:00";
            // 
            // radBlackNet
            // 
            this.radBlackNet.AutoSize = true;
            this.radBlackNet.Location = new System.Drawing.Point(6, 111);
            this.radBlackNet.Name = "radBlackNet";
            this.radBlackNet.Size = new System.Drawing.Size(61, 17);
            this.radBlackNet.TabIndex = 4;
            this.radBlackNet.TabStop = true;
            this.radBlackNet.Text = "网络：";
            this.radBlackNet.UseVisualStyleBackColor = true;
            this.radBlackNet.CheckedChanged += RadioChecks_CheckedChanged;
            // 
            // txtBlackHuman
            // 
            this.txtBlackHuman.Enabled = false;
            this.txtBlackHuman.Location = new System.Drawing.Point(61, 88);
            this.txtBlackHuman.Name = "txtBlackHuman";
            this.txtBlackHuman.Size = new System.Drawing.Size(64, 20);
            this.txtBlackHuman.TabIndex = 3;
            this.txtBlackHuman.Text = "Wang Cai";
            // 
            // radBlackHuman
            // 
            this.radBlackHuman.AutoSize = true;
            this.radBlackHuman.Location = new System.Drawing.Point(6, 88);
            this.radBlackHuman.Name = "radBlackHuman";
            this.radBlackHuman.Size = new System.Drawing.Size(61, 17);
            this.radBlackHuman.TabIndex = 2;
            this.radBlackHuman.Text = "玩家：";
            this.radBlackHuman.UseVisualStyleBackColor = true;
            this.radBlackHuman.CheckedChanged += RadioChecks_CheckedChanged;
            // 
            // radBlackAI
            // 
            this.radBlackAI.AutoSize = true;
            this.radBlackAI.Checked = true;
            this.radBlackAI.Location = new System.Drawing.Point(6, 65);
            this.radBlackAI.Name = "radBlackAI";
            this.radBlackAI.Size = new System.Drawing.Size(49, 17);
            this.radBlackAI.TabIndex = 1;
            this.radBlackAI.TabStop = true;
            this.radBlackAI.Text = "电脑";
            this.radBlackAI.UseVisualStyleBackColor = true;
            // 
            // picBlack
            // 
            this.picBlack.Image = ((System.Drawing.Image)(resources.GetObject("picBlack.Image")));
            this.picBlack.Location = new System.Drawing.Point(6, 19);
            this.picBlack.Name = "picBlack";
            this.picBlack.Size = new System.Drawing.Size(41, 42);
            this.picBlack.TabIndex = 0;
            this.picBlack.TabStop = false;
            // 
            // grpWhite
            // 
            this.grpWhite.Controls.Add(this.txtWhiteNet);
            this.grpWhite.Controls.Add(this.lblWhiteTime);
            this.grpWhite.Controls.Add(this.radWhiteNet);
            this.grpWhite.Controls.Add(this.txtWhiteHuman);
            this.grpWhite.Controls.Add(this.radWhiteHuman);
            this.grpWhite.Controls.Add(this.radWhiteAI);
            this.grpWhite.Controls.Add(this.picWhite);
            this.grpWhite.Location = new System.Drawing.Point(629, 148);
            this.grpWhite.Name = "grpWhite";
            this.grpWhite.Size = new System.Drawing.Size(131, 133);
            this.grpWhite.TabIndex = 2;
            this.grpWhite.TabStop = false;
            this.grpWhite.Text = "白子后手";
            // 
            // txtWhiteNet
            // 
            this.txtWhiteNet.Enabled = false;
            this.txtWhiteNet.Location = new System.Drawing.Point(62, 110);
            this.txtWhiteNet.Name = "txtWhiteNet";
            this.txtWhiteNet.Size = new System.Drawing.Size(63, 20);
            this.txtWhiteNet.TabIndex = 7;
            this.txtWhiteNet.Text = "127.0.0.1";
            // 
            // lblWhiteTime
            // 
            this.lblWhiteTime.AutoSize = true;
            this.lblWhiteTime.Enabled = false;
            this.lblWhiteTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWhiteTime.Location = new System.Drawing.Point(53, 30);
            this.lblWhiteTime.Name = "lblWhiteTime";
            this.lblWhiteTime.Size = new System.Drawing.Size(71, 29);
            this.lblWhiteTime.TabIndex = 6;
            this.lblWhiteTime.Text = "00:00";
            // 
            // radWhiteNet
            // 
            this.radWhiteNet.AutoSize = true;
            this.radWhiteNet.Location = new System.Drawing.Point(6, 111);
            this.radWhiteNet.Name = "radWhiteNet";
            this.radWhiteNet.Size = new System.Drawing.Size(61, 17);
            this.radWhiteNet.TabIndex = 4;
            this.radWhiteNet.TabStop = true;
            this.radWhiteNet.Text = "网络：";
            this.radWhiteNet.UseVisualStyleBackColor = true;
            this.radWhiteNet.CheckedChanged += RadioChecks_CheckedChanged;
            // 
            // txtWhiteHuman
            // 
            this.txtWhiteHuman.Location = new System.Drawing.Point(61, 88);
            this.txtWhiteHuman.Name = "txtWhiteHuman";
            this.txtWhiteHuman.Size = new System.Drawing.Size(64, 20);
            this.txtWhiteHuman.TabIndex = 3;
            this.txtWhiteHuman.Text = "Wang Cai";
            // 
            // radWhiteHuman
            // 
            this.radWhiteHuman.AutoSize = true;
            this.radWhiteHuman.Checked = true;
            this.radWhiteHuman.Location = new System.Drawing.Point(6, 88);
            this.radWhiteHuman.Name = "radWhiteHuman";
            this.radWhiteHuman.Size = new System.Drawing.Size(61, 17);
            this.radWhiteHuman.TabIndex = 2;
            this.radWhiteHuman.TabStop = true;
            this.radWhiteHuman.Text = "玩家：";
            this.radWhiteHuman.UseVisualStyleBackColor = true;
            this.radWhiteHuman.CheckedChanged += RadioChecks_CheckedChanged;
            // 
            // radWhiteAI
            // 
            this.radWhiteAI.AutoSize = true;
            this.radWhiteAI.Location = new System.Drawing.Point(6, 65);
            this.radWhiteAI.Name = "radWhiteAI";
            this.radWhiteAI.Size = new System.Drawing.Size(49, 17);
            this.radWhiteAI.TabIndex = 1;
            this.radWhiteAI.Text = "电脑";
            this.radWhiteAI.UseVisualStyleBackColor = true;
            // 
            // picWhite
            // 
            this.picWhite.Image = ((System.Drawing.Image)(resources.GetObject("picWhite.Image")));
            this.picWhite.Location = new System.Drawing.Point(6, 19);
            this.picWhite.Name = "picWhite";
            this.picWhite.Size = new System.Drawing.Size(41, 42);
            this.picWhite.TabIndex = 0;
            this.picWhite.TabStop = false;
            // 
            // lstHistory
            // 
            this.lstHistory.FormattingEnabled = true;
            this.lstHistory.Location = new System.Drawing.Point(629, 361);
            this.lstHistory.Name = "lstHistory";
            this.lstHistory.Size = new System.Drawing.Size(131, 212);
            this.lstHistory.TabIndex = 11;
            this.lstHistory.SelectedIndexChanged += new System.EventHandler(this.lstHistory_SelectedIndexChanged);
            // 
            // btnGame
            // 
            this.btnGame.Location = new System.Drawing.Point(629, 310);
            this.btnGame.Name = "btnGame";
            this.btnGame.Size = new System.Drawing.Size(131, 23);
            this.btnGame.TabIndex = 7;
            this.btnGame.Text = "开始新局";
            this.btnGame.UseVisualStyleBackColor = true;
            this.btnGame.Click += new System.EventHandler(this.btnGame_Click);
            // 
            // btnFile
            // 
            this.btnFile.Location = new System.Drawing.Point(629, 332);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(131, 23);
            this.btnFile.TabIndex = 8;
            this.btnFile.Text = "读取游戏";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // txtTimeLimit
            // 
            this.txtTimeLimit.Enabled = false;
            this.txtTimeLimit.Location = new System.Drawing.Point(687, 285);
            this.txtTimeLimit.Name = "txtTimeLimit";
            this.txtTimeLimit.Size = new System.Drawing.Size(36, 20);
            this.txtTimeLimit.TabIndex = 5;
            this.txtTimeLimit.Text = "1";
            this.txtTimeLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTimeLimit
            // 
            this.lblTimeLimit.AutoSize = true;
            this.lblTimeLimit.Location = new System.Drawing.Point(723, 288);
            this.lblTimeLimit.Name = "lblTimeLimit";
            this.lblTimeLimit.Size = new System.Drawing.Size(31, 13);
            this.lblTimeLimit.TabIndex = 6;
            this.lblTimeLimit.Text = "分钟";
            // 
            // chkTimeLimit
            // 
            this.chkTimeLimit.AutoSize = true;
            this.chkTimeLimit.Location = new System.Drawing.Point(635, 287);
            this.chkTimeLimit.Name = "chkTimeLimit";
            this.chkTimeLimit.Size = new System.Drawing.Size(62, 17);
            this.chkTimeLimit.TabIndex = 4;
            this.chkTimeLimit.Text = "限时：";
            this.chkTimeLimit.UseVisualStyleBackColor = true;
            this.chkTimeLimit.CheckedChanged += RadioChecks_CheckedChanged;
            // 
            // dlgOpenGame
            // 
            this.dlgOpenGame.Filter = "存档文件|*.ren|所有文件|*.*";
            this.dlgOpenGame.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgOpenGame_FileOk);
            // 
            // dlgSaveGame
            // 
            this.dlgSaveGame.FileName = "untitled.ren";
            this.dlgSaveGame.Filter = "存档文件|*.ren|所有文件|*.*";
            this.dlgSaveGame.FileOk += new System.ComponentModel.CancelEventHandler(this.dlgSaveGame_FileOk);
            // 
            // timerLimit
            // 
            this.timerLimit.Interval = 1000;
            this.timerLimit.Tick += new System.EventHandler(this.timerLimit_Tick);
            // 
            // trkVideo
            // 
            this.trkVideo.Location = new System.Drawing.Point(623, 592);
            this.trkVideo.Name = "trkVideo";
            this.trkVideo.Size = new System.Drawing.Size(145, 45);
            this.trkVideo.TabIndex = 12;
            this.trkVideo.Value = 3;
            this.trkVideo.Scroll += new System.EventHandler(this.trkVideo_Scroll);
            // 
            // lblVideo
            // 
            this.lblVideo.AutoSize = true;
            this.lblVideo.Location = new System.Drawing.Point(626, 576);
            this.lblVideo.Name = "lblVideo";
            this.lblVideo.Size = new System.Drawing.Size(126, 13);
            this.lblVideo.TabIndex = 13;
            this.lblVideo.Text = "录像播放速度：3秒/步";
            // 
            // timerVideo
            // 
            this.timerVideo.Interval = 3000;
            // 
            // timerGameSave
            // 
            this.timerSynchronize.Enabled = true;
            this.timerSynchronize.Tick += new System.EventHandler(this.timerSynchronize_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 624);
            this.Controls.Add(this.lblVideo);
            this.Controls.Add(this.trkVideo);
            this.Controls.Add(this.lblTimeLimit);
            this.Controls.Add(this.txtTimeLimit);
            this.Controls.Add(this.btnFile);
            this.Controls.Add(this.btnGame);
            this.Controls.Add(this.lstHistory);
            this.Controls.Add(this.grpWhite);
            this.Controls.Add(this.grpBlack);
            this.Controls.Add(this.picBoard);
            this.Controls.Add(this.chkTimeLimit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "五子棋";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.picBoard)).EndInit();
            this.grpBlack.ResumeLayout(false);
            this.grpBlack.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBlack)).EndInit();
            this.grpWhite.ResumeLayout(false);
            this.grpWhite.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWhite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkVideo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.PictureBox picBoard;
        private System.Windows.Forms.GroupBox grpBlack;
        private System.Windows.Forms.RadioButton radBlackAI;
        public System.Windows.Forms.PictureBox picBlack;
        private System.Windows.Forms.TextBox txtBlackHuman;
        private System.Windows.Forms.RadioButton radBlackHuman;
        private System.Windows.Forms.GroupBox grpWhite;
        private System.Windows.Forms.TextBox txtWhiteHuman;
        private System.Windows.Forms.RadioButton radWhiteHuman;
        private System.Windows.Forms.RadioButton radWhiteAI;
        public System.Windows.Forms.PictureBox picWhite;
        public System.Windows.Forms.ListBox lstHistory;
        private System.Windows.Forms.Button btnGame;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.RadioButton radBlackNet;
        private System.Windows.Forms.RadioButton radWhiteNet;
        private System.Windows.Forms.TextBox txtTimeLimit;
        private System.Windows.Forms.Label lblTimeLimit;
        private System.Windows.Forms.CheckBox chkTimeLimit;
        private System.Windows.Forms.Label lblBlackTime;
        private System.Windows.Forms.Label lblWhiteTime;
        private System.Windows.Forms.OpenFileDialog dlgOpenGame;
        private System.Windows.Forms.SaveFileDialog dlgSaveGame;
        private System.Windows.Forms.Timer timerLimit;
        private System.Windows.Forms.TrackBar trkVideo;
        private System.Windows.Forms.Label lblVideo;
        public System.Windows.Forms.Timer timerVideo;
        private System.Windows.Forms.TextBox txtBlackNet;
        private System.Windows.Forms.TextBox txtWhiteNet;
        private System.Windows.Forms.Timer timerSynchronize;
    }
}

