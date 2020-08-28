namespace ACT.DFAssist
{
    partial class MainControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainControl));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rtxLogger = new System.Windows.Forms.RichTextBox();
            this.trvFates = new System.Windows.Forms.TreeView();
            this.splitBase = new System.Windows.Forms.SplitContainer();
            this.tabLeft = new System.Windows.Forms.TabControl();
            this.tabPageFates = new System.Windows.Forms.TabPage();
            this.tabPageSetting = new System.Windows.Forms.TabPage();
            this.btnShowLogSetting = new System.Windows.Forms.Button();
            this.txtUpdateSkip = new System.Windows.Forms.TextBox();
            this.txtClientVersion = new System.Windows.Forms.TextBox();
            this.cboClientVersion = new System.Windows.Forms.ComboBox();
            this.lblClientVersion = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cboUiLanguage = new System.Windows.Forms.ComboBox();
            this.cboGameLanguage = new System.Windows.Forms.ComboBox();
            this.lblUiLanguage = new System.Windows.Forms.Label();
            this.lblGameLanguage = new System.Windows.Forms.Label();
            this.txtLogFont = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSoundStop = new System.Windows.Forms.Button();
            this.btnSoundPlay = new System.Windows.Forms.Button();
            this.txtSoundFile = new System.Windows.Forms.TextBox();
            this.btnSelectSound = new System.Windows.Forms.Button();
            this.chkUseSound = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnBlinkOverlay = new System.Windows.Forms.Button();
            this.chkUseOverlay = new System.Windows.Forms.CheckBox();
            this.chkWholeFates = new System.Windows.Forms.CheckBox();
            this.pnlLogSetting = new System.Windows.Forms.Panel();
            this.lblDisplayFont = new System.Windows.Forms.Label();
            this.btnLogFont = new System.Windows.Forms.Button();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.lblBackColor = new System.Windows.Forms.Label();
            this.cboLogBackground = new System.Windows.Forms.ComboBox();
            this.txtOverayLocation = new System.Windows.Forms.TextBox();
            this.txtSelectedFates = new System.Windows.Forms.TextBox();
            this.btnReconnect = new System.Windows.Forms.Button();
            this.tabPageNotify = new System.Windows.Forms.TabPage();
            this.panel6 = new System.Windows.Forms.Panel();
            this.lblNtfTelegramToken = new System.Windows.Forms.Label();
            this.lblNtfTelegramId = new System.Windows.Forms.Label();
            this.txtNtfTelegramId = new System.Windows.Forms.TextBox();
            this.txtNtfTelegramToken = new System.Windows.Forms.TextBox();
            this.chkNtfUseTelegram = new System.Windows.Forms.CheckBox();
            this.btnTestNotify = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lnklblLineNotify = new System.Windows.Forms.LinkLabel();
            this.lblNtfLineToken = new System.Windows.Forms.Label();
            this.txtNtfLineToken = new System.Windows.Forms.TextBox();
            this.chkNtfUseLine = new System.Windows.Forms.CheckBox();
            this.ilTab = new System.Windows.Forms.ImageList(this.components);
            this.ttCtrls = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitBase)).BeginInit();
            this.splitBase.Panel1.SuspendLayout();
            this.splitBase.Panel2.SuspendLayout();
            this.splitBase.SuspendLayout();
            this.tabLeft.SuspendLayout();
            this.tabPageFates.SuspendLayout();
            this.tabPageSetting.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlLogSetting.SuspendLayout();
            this.tabPageNotify.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 411);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "This is test plugin";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(148, 411);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "label2";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label2.Visible = false;
            // 
            // rtxLogger
            // 
            this.rtxLogger.BackColor = System.Drawing.Color.Linen;
            this.rtxLogger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxLogger.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtxLogger.Location = new System.Drawing.Point(0, 0);
            this.rtxLogger.Name = "rtxLogger";
            this.rtxLogger.ReadOnly = true;
            this.rtxLogger.Size = new System.Drawing.Size(559, 559);
            this.rtxLogger.TabIndex = 2;
            this.rtxLogger.Text = "";
            // 
            // trvFates
            // 
            this.trvFates.CheckBoxes = true;
            this.trvFates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvFates.Location = new System.Drawing.Point(3, 3);
            this.trvFates.Name = "trvFates";
            this.trvFates.Size = new System.Drawing.Size(335, 510);
            this.trvFates.TabIndex = 3;
            this.trvFates.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TrvFates_AfterCheck);
            // 
            // splitBase
            // 
            this.splitBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitBase.Location = new System.Drawing.Point(0, 0);
            this.splitBase.Name = "splitBase";
            // 
            // splitBase.Panel1
            // 
            this.splitBase.Panel1.Controls.Add(this.rtxLogger);
            // 
            // splitBase.Panel2
            // 
            this.splitBase.Panel2.Controls.Add(this.tabLeft);
            this.splitBase.Size = new System.Drawing.Size(912, 559);
            this.splitBase.SplitterDistance = 559;
            this.splitBase.TabIndex = 4;
            // 
            // tabLeft
            // 
            this.tabLeft.Controls.Add(this.tabPageFates);
            this.tabLeft.Controls.Add(this.tabPageSetting);
            this.tabLeft.Controls.Add(this.tabPageNotify);
            this.tabLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabLeft.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabLeft.ImageList = this.ilTab;
            this.tabLeft.Location = new System.Drawing.Point(0, 0);
            this.tabLeft.Name = "tabLeft";
            this.tabLeft.SelectedIndex = 0;
            this.tabLeft.Size = new System.Drawing.Size(349, 559);
            this.tabLeft.TabIndex = 5;
            // 
            // tabPageFates
            // 
            this.tabPageFates.Controls.Add(this.trvFates);
            this.tabPageFates.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageFates.ImageIndex = 0;
            this.tabPageFates.Location = new System.Drawing.Point(4, 39);
            this.tabPageFates.Name = "tabPageFates";
            this.tabPageFates.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFates.Size = new System.Drawing.Size(341, 516);
            this.tabPageFates.TabIndex = 0;
            this.tabPageFates.Text = "FATEs";
            this.tabPageFates.UseVisualStyleBackColor = true;
            // 
            // tabPageSetting
            // 
            this.tabPageSetting.AutoScroll = true;
            this.tabPageSetting.Controls.Add(this.btnShowLogSetting);
            this.tabPageSetting.Controls.Add(this.txtUpdateSkip);
            this.tabPageSetting.Controls.Add(this.txtClientVersion);
            this.tabPageSetting.Controls.Add(this.cboClientVersion);
            this.tabPageSetting.Controls.Add(this.lblClientVersion);
            this.tabPageSetting.Controls.Add(this.panel4);
            this.tabPageSetting.Controls.Add(this.txtLogFont);
            this.tabPageSetting.Controls.Add(this.btnTest);
            this.tabPageSetting.Controls.Add(this.panel3);
            this.tabPageSetting.Controls.Add(this.panel2);
            this.tabPageSetting.Controls.Add(this.pnlLogSetting);
            this.tabPageSetting.Controls.Add(this.txtOverayLocation);
            this.tabPageSetting.Controls.Add(this.txtSelectedFates);
            this.tabPageSetting.Controls.Add(this.btnReconnect);
            this.tabPageSetting.ImageIndex = 1;
            this.tabPageSetting.Location = new System.Drawing.Point(4, 39);
            this.tabPageSetting.Name = "tabPageSetting";
            this.tabPageSetting.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSetting.Size = new System.Drawing.Size(341, 516);
            this.tabPageSetting.TabIndex = 1;
            this.tabPageSetting.Text = "Setting";
            this.tabPageSetting.UseVisualStyleBackColor = true;
            // 
            // btnShowLogSetting
            // 
            this.btnShowLogSetting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowLogSetting.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnShowLogSetting.Location = new System.Drawing.Point(3, 285);
            this.btnShowLogSetting.Name = "btnShowLogSetting";
            this.btnShowLogSetting.Size = new System.Drawing.Size(334, 23);
            this.btnShowLogSetting.TabIndex = 24;
            this.btnShowLogSetting.Text = "Log setting";
            this.btnShowLogSetting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShowLogSetting.UseVisualStyleBackColor = false;
            this.btnShowLogSetting.Click += new System.EventHandler(this.btnShowLogSetting_Click);
            // 
            // txtUpdateSkip
            // 
            this.txtUpdateSkip.Location = new System.Drawing.Point(84, 486);
            this.txtUpdateSkip.Name = "txtUpdateSkip";
            this.txtUpdateSkip.Size = new System.Drawing.Size(20, 25);
            this.txtUpdateSkip.TabIndex = 23;
            this.txtUpdateSkip.Visible = false;
            // 
            // txtClientVersion
            // 
            this.txtClientVersion.Location = new System.Drawing.Point(58, 486);
            this.txtClientVersion.Name = "txtClientVersion";
            this.txtClientVersion.Size = new System.Drawing.Size(20, 25);
            this.txtClientVersion.TabIndex = 22;
            this.txtClientVersion.Visible = false;
            // 
            // cboClientVersion
            // 
            this.cboClientVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboClientVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboClientVersion.FormattingEnabled = true;
            this.cboClientVersion.Location = new System.Drawing.Point(96, 6);
            this.cboClientVersion.Name = "cboClientVersion";
            this.cboClientVersion.Size = new System.Drawing.Size(239, 25);
            this.cboClientVersion.TabIndex = 7;
            this.cboClientVersion.SelectedIndexChanged += new System.EventHandler(this.CboClientVersion_SelectedIndexChanged);
            this.cboClientVersion.SelectedValueChanged += new System.EventHandler(this.CboClientVersion_SelectedValueChanged);
            // 
            // lblClientVersion
            // 
            this.lblClientVersion.AutoSize = true;
            this.lblClientVersion.Location = new System.Drawing.Point(8, 9);
            this.lblClientVersion.Name = "lblClientVersion";
            this.lblClientVersion.Size = new System.Drawing.Size(51, 17);
            this.lblClientVersion.TabIndex = 6;
            this.lblClientVersion.Text = "Version";
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.cboUiLanguage);
            this.panel4.Controls.Add(this.cboGameLanguage);
            this.panel4.Controls.Add(this.lblUiLanguage);
            this.panel4.Controls.Add(this.lblGameLanguage);
            this.panel4.Location = new System.Drawing.Point(3, 37);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(335, 65);
            this.panel4.TabIndex = 21;
            // 
            // cboUiLanguage
            // 
            this.cboUiLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboUiLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUiLanguage.FormattingEnabled = true;
            this.cboUiLanguage.Location = new System.Drawing.Point(91, 3);
            this.cboUiLanguage.Name = "cboUiLanguage";
            this.cboUiLanguage.Size = new System.Drawing.Size(239, 25);
            this.cboUiLanguage.TabIndex = 3;
            this.cboUiLanguage.SelectedValueChanged += new System.EventHandler(this.CboUiLanguage_SelectedValueChanged);
            // 
            // cboGameLanguage
            // 
            this.cboGameLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboGameLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGameLanguage.FormattingEnabled = true;
            this.cboGameLanguage.Location = new System.Drawing.Point(91, 34);
            this.cboGameLanguage.Name = "cboGameLanguage";
            this.cboGameLanguage.Size = new System.Drawing.Size(239, 25);
            this.cboGameLanguage.TabIndex = 5;
            this.cboGameLanguage.SelectedValueChanged += new System.EventHandler(this.CboGameLanguage_SelectedValueChanged);
            // 
            // lblUiLanguage
            // 
            this.lblUiLanguage.AutoSize = true;
            this.lblUiLanguage.Location = new System.Drawing.Point(4, 6);
            this.lblUiLanguage.Name = "lblUiLanguage";
            this.lblUiLanguage.Size = new System.Drawing.Size(81, 17);
            this.lblUiLanguage.TabIndex = 2;
            this.lblUiLanguage.Text = "UI Language";
            // 
            // lblGameLanguage
            // 
            this.lblGameLanguage.AutoSize = true;
            this.lblGameLanguage.Location = new System.Drawing.Point(4, 37);
            this.lblGameLanguage.Name = "lblGameLanguage";
            this.lblGameLanguage.Size = new System.Drawing.Size(56, 17);
            this.lblGameLanguage.TabIndex = 4;
            this.lblGameLanguage.Text = "In Game";
            // 
            // txtLogFont
            // 
            this.txtLogFont.Location = new System.Drawing.Point(110, 486);
            this.txtLogFont.Name = "txtLogFont";
            this.txtLogFont.Size = new System.Drawing.Size(20, 25);
            this.txtLogFont.TabIndex = 20;
            this.txtLogFont.Visible = false;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(6, 485);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(46, 26);
            this.btnTest.TabIndex = 19;
            this.btnTest.Text = "TEST!!!";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Visible = false;
            this.btnTest.Click += new System.EventHandler(this.BtnTest_Click);
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.btnSoundStop);
            this.panel3.Controls.Add(this.btnSoundPlay);
            this.panel3.Controls.Add(this.txtSoundFile);
            this.panel3.Controls.Add(this.btnSelectSound);
            this.panel3.Controls.Add(this.chkUseSound);
            this.panel3.Location = new System.Drawing.Point(3, 193);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(335, 65);
            this.panel3.TabIndex = 18;
            // 
            // btnSoundStop
            // 
            this.btnSoundStop.Image = global::ACT.DFAssist.Properties.Resources.stop;
            this.btnSoundStop.Location = new System.Drawing.Point(82, 25);
            this.btnSoundStop.Name = "btnSoundStop";
            this.btnSoundStop.Size = new System.Drawing.Size(32, 32);
            this.btnSoundStop.TabIndex = 18;
            this.btnSoundStop.UseVisualStyleBackColor = true;
            this.btnSoundStop.Click += new System.EventHandler(this.BtnSoundStop_Click);
            // 
            // btnSoundPlay
            // 
            this.btnSoundPlay.Image = global::ACT.DFAssist.Properties.Resources.play;
            this.btnSoundPlay.Location = new System.Drawing.Point(45, 25);
            this.btnSoundPlay.Name = "btnSoundPlay";
            this.btnSoundPlay.Size = new System.Drawing.Size(32, 32);
            this.btnSoundPlay.TabIndex = 17;
            this.btnSoundPlay.UseVisualStyleBackColor = true;
            this.btnSoundPlay.Click += new System.EventHandler(this.BtnSoundPlay_Click);
            // 
            // txtSoundFile
            // 
            this.txtSoundFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSoundFile.Location = new System.Drawing.Point(120, 30);
            this.txtSoundFile.Name = "txtSoundFile";
            this.txtSoundFile.Size = new System.Drawing.Size(207, 25);
            this.txtSoundFile.TabIndex = 16;
            this.txtSoundFile.TextChanged += new System.EventHandler(this.txtSoundFile_TextChanged);
            // 
            // btnSelectSound
            // 
            this.btnSelectSound.Image = global::ACT.DFAssist.Properties.Resources.magnifyingglassicon_114513_22;
            this.btnSelectSound.Location = new System.Drawing.Point(7, 25);
            this.btnSelectSound.Name = "btnSelectSound";
            this.btnSelectSound.Size = new System.Drawing.Size(32, 32);
            this.btnSelectSound.TabIndex = 15;
            this.btnSelectSound.UseVisualStyleBackColor = true;
            this.btnSelectSound.Click += new System.EventHandler(this.BtnSelectSound_Click);
            // 
            // chkUseSound
            // 
            this.chkUseSound.AutoSize = true;
            this.chkUseSound.Location = new System.Drawing.Point(2, 3);
            this.chkUseSound.Name = "chkUseSound";
            this.chkUseSound.Size = new System.Drawing.Size(89, 21);
            this.chkUseSound.TabIndex = 14;
            this.chkUseSound.Text = "Use sound";
            this.chkUseSound.UseVisualStyleBackColor = true;
            this.chkUseSound.CheckedChanged += new System.EventHandler(this.ChkUseSound_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnBlinkOverlay);
            this.panel2.Controls.Add(this.chkUseOverlay);
            this.panel2.Controls.Add(this.chkWholeFates);
            this.panel2.Location = new System.Drawing.Point(3, 119);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(335, 58);
            this.panel2.TabIndex = 17;
            // 
            // btnBlinkOverlay
            // 
            this.btnBlinkOverlay.Image = global::ACT.DFAssist.Properties.Resources.annotation;
            this.btnBlinkOverlay.Location = new System.Drawing.Point(3, 3);
            this.btnBlinkOverlay.Name = "btnBlinkOverlay";
            this.btnBlinkOverlay.Size = new System.Drawing.Size(43, 38);
            this.btnBlinkOverlay.TabIndex = 23;
            this.btnBlinkOverlay.UseVisualStyleBackColor = true;
            this.btnBlinkOverlay.Click += new System.EventHandler(this.BtnBlinkOverlay_Click);
            // 
            // chkUseOverlay
            // 
            this.chkUseOverlay.AutoSize = true;
            this.chkUseOverlay.Location = new System.Drawing.Point(52, 5);
            this.chkUseOverlay.Name = "chkUseOverlay";
            this.chkUseOverlay.Size = new System.Drawing.Size(95, 21);
            this.chkUseOverlay.TabIndex = 14;
            this.chkUseOverlay.Text = "Use overlay";
            this.chkUseOverlay.UseVisualStyleBackColor = true;
            this.chkUseOverlay.CheckedChanged += new System.EventHandler(this.ChkUseOverlay_CheckedChanged);
            // 
            // chkWholeFates
            // 
            this.chkWholeFates.AutoSize = true;
            this.chkWholeFates.Location = new System.Drawing.Point(52, 32);
            this.chkWholeFates.Name = "chkWholeFates";
            this.chkWholeFates.Size = new System.Drawing.Size(149, 21);
            this.chkWholeFates.TabIndex = 11;
            this.chkWholeFates.Text = "Logging whole FATEs";
            this.chkWholeFates.UseVisualStyleBackColor = true;
            this.chkWholeFates.CheckedChanged += new System.EventHandler(this.ChkWholeFates_CheckedChanged);
            // 
            // pnlLogSetting
            // 
            this.pnlLogSetting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlLogSetting.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLogSetting.Controls.Add(this.lblDisplayFont);
            this.pnlLogSetting.Controls.Add(this.btnLogFont);
            this.pnlLogSetting.Controls.Add(this.btnClearLogs);
            this.pnlLogSetting.Controls.Add(this.lblBackColor);
            this.pnlLogSetting.Controls.Add(this.cboLogBackground);
            this.pnlLogSetting.Location = new System.Drawing.Point(3, 309);
            this.pnlLogSetting.Name = "pnlLogSetting";
            this.pnlLogSetting.Size = new System.Drawing.Size(335, 115);
            this.pnlLogSetting.TabIndex = 16;
            this.pnlLogSetting.Visible = false;
            // 
            // lblDisplayFont
            // 
            this.lblDisplayFont.AutoSize = true;
            this.lblDisplayFont.Location = new System.Drawing.Point(2, 38);
            this.lblDisplayFont.Name = "lblDisplayFont";
            this.lblDisplayFont.Size = new System.Drawing.Size(77, 17);
            this.lblDisplayFont.TabIndex = 13;
            this.lblDisplayFont.Text = "Display font";
            // 
            // btnLogFont
            // 
            this.btnLogFont.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogFont.Location = new System.Drawing.Point(91, 35);
            this.btnLogFont.Name = "btnLogFont";
            this.btnLogFont.Size = new System.Drawing.Size(240, 23);
            this.btnLogFont.TabIndex = 12;
            this.btnLogFont.Text = "Font";
            this.btnLogFont.UseVisualStyleBackColor = true;
            this.btnLogFont.Click += new System.EventHandler(this.BtnLogFont_Click);
            // 
            // btnClearLogs
            // 
            this.btnClearLogs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearLogs.Image = global::ACT.DFAssist.Properties.Resources.btn_del;
            this.btnClearLogs.Location = new System.Drawing.Point(91, 64);
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.Size = new System.Drawing.Size(240, 44);
            this.btnClearLogs.TabIndex = 6;
            this.btnClearLogs.Text = "Clear Logs";
            this.btnClearLogs.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClearLogs.UseVisualStyleBackColor = true;
            this.btnClearLogs.Click += new System.EventHandler(this.BtnClearLogs_Click);
            // 
            // lblBackColor
            // 
            this.lblBackColor.AutoSize = true;
            this.lblBackColor.Location = new System.Drawing.Point(2, 6);
            this.lblBackColor.Name = "lblBackColor";
            this.lblBackColor.Size = new System.Drawing.Size(70, 17);
            this.lblBackColor.TabIndex = 9;
            this.lblBackColor.Text = "Back Color";
            // 
            // cboLogBackground
            // 
            this.cboLogBackground.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboLogBackground.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboLogBackground.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLogBackground.FormattingEnabled = true;
            this.cboLogBackground.Location = new System.Drawing.Point(91, 3);
            this.cboLogBackground.Name = "cboLogBackground";
            this.cboLogBackground.Size = new System.Drawing.Size(239, 26);
            this.cboLogBackground.TabIndex = 8;
            this.cboLogBackground.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CboLogBackground_DrawItem);
            this.cboLogBackground.SelectedValueChanged += new System.EventHandler(this.CboLogBackground_SelectedValueChanged);
            // 
            // txtOverayLocation
            // 
            this.txtOverayLocation.Location = new System.Drawing.Point(173, 486);
            this.txtOverayLocation.Name = "txtOverayLocation";
            this.txtOverayLocation.Size = new System.Drawing.Size(34, 25);
            this.txtOverayLocation.TabIndex = 15;
            this.txtOverayLocation.Text = "0,0";
            this.txtOverayLocation.Visible = false;
            // 
            // txtSelectedFates
            // 
            this.txtSelectedFates.Location = new System.Drawing.Point(136, 486);
            this.txtSelectedFates.Name = "txtSelectedFates";
            this.txtSelectedFates.Size = new System.Drawing.Size(31, 25);
            this.txtSelectedFates.TabIndex = 12;
            this.txtSelectedFates.Visible = false;
            // 
            // btnReconnect
            // 
            this.btnReconnect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReconnect.Image = global::ACT.DFAssist.Properties.Resources.Player2_Icon;
            this.btnReconnect.Location = new System.Drawing.Point(211, 485);
            this.btnReconnect.Name = "btnReconnect";
            this.btnReconnect.Size = new System.Drawing.Size(124, 25);
            this.btnReconnect.TabIndex = 10;
            this.btnReconnect.Text = "Reconnect";
            this.btnReconnect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReconnect.UseVisualStyleBackColor = true;
            this.btnReconnect.Visible = false;
            this.btnReconnect.Click += new System.EventHandler(this.BtnReconnect_Click);
            // 
            // tabPageNotify
            // 
            this.tabPageNotify.AutoScroll = true;
            this.tabPageNotify.Controls.Add(this.panel6);
            this.tabPageNotify.Controls.Add(this.btnTestNotify);
            this.tabPageNotify.Controls.Add(this.panel5);
            this.tabPageNotify.Controls.Add(this.label2);
            this.tabPageNotify.Controls.Add(this.label1);
            this.tabPageNotify.ImageIndex = 4;
            this.tabPageNotify.Location = new System.Drawing.Point(4, 39);
            this.tabPageNotify.Name = "tabPageNotify";
            this.tabPageNotify.Size = new System.Drawing.Size(341, 516);
            this.tabPageNotify.TabIndex = 2;
            this.tabPageNotify.Text = "Notify";
            this.tabPageNotify.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.lblNtfTelegramToken);
            this.panel6.Controls.Add(this.lblNtfTelegramId);
            this.panel6.Controls.Add(this.txtNtfTelegramId);
            this.panel6.Controls.Add(this.txtNtfTelegramToken);
            this.panel6.Controls.Add(this.chkNtfUseTelegram);
            this.panel6.Location = new System.Drawing.Point(3, 92);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(334, 97);
            this.panel6.TabIndex = 4;
            // 
            // lblNtfTelegramToken
            // 
            this.lblNtfTelegramToken.AutoSize = true;
            this.lblNtfTelegramToken.Location = new System.Drawing.Point(3, 34);
            this.lblNtfTelegramToken.Name = "lblNtfTelegramToken";
            this.lblNtfTelegramToken.Size = new System.Drawing.Size(42, 17);
            this.lblNtfTelegramToken.TabIndex = 5;
            this.lblNtfTelegramToken.Text = "Token";
            // 
            // lblNtfTelegramId
            // 
            this.lblNtfTelegramId.AutoSize = true;
            this.lblNtfTelegramId.Location = new System.Drawing.Point(3, 65);
            this.lblNtfTelegramId.Name = "lblNtfTelegramId";
            this.lblNtfTelegramId.Size = new System.Drawing.Size(20, 17);
            this.lblNtfTelegramId.TabIndex = 2;
            this.lblNtfTelegramId.Text = "ID";
            // 
            // txtNtfTelegramId
            // 
            this.txtNtfTelegramId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNtfTelegramId.Location = new System.Drawing.Point(61, 62);
            this.txtNtfTelegramId.Name = "txtNtfTelegramId";
            this.txtNtfTelegramId.Size = new System.Drawing.Size(268, 25);
            this.txtNtfTelegramId.TabIndex = 1;
            this.txtNtfTelegramId.TextChanged += new System.EventHandler(this.TxtNtfLineToken_TextChanged);
            // 
            // txtNtfTelegramToken
            // 
            this.txtNtfTelegramToken.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNtfTelegramToken.Location = new System.Drawing.Point(61, 31);
            this.txtNtfTelegramToken.Name = "txtNtfTelegramToken";
            this.txtNtfTelegramToken.Size = new System.Drawing.Size(268, 25);
            this.txtNtfTelegramToken.TabIndex = 4;
            this.txtNtfTelegramToken.TextChanged += new System.EventHandler(this.TxtNtfLineToken_TextChanged);
            // 
            // chkNtfUseTelegram
            // 
            this.chkNtfUseTelegram.AutoSize = true;
            this.chkNtfUseTelegram.Location = new System.Drawing.Point(3, 3);
            this.chkNtfUseTelegram.Name = "chkNtfUseTelegram";
            this.chkNtfUseTelegram.Size = new System.Drawing.Size(143, 21);
            this.chkNtfUseTelegram.TabIndex = 0;
            this.chkNtfUseTelegram.Text = "Use Telegram notify";
            this.chkNtfUseTelegram.UseVisualStyleBackColor = true;
            this.chkNtfUseTelegram.CheckedChanged += new System.EventHandler(this.ChkNtfUseTelegram_CheckedChanged);
            // 
            // btnTestNotify
            // 
            this.btnTestNotify.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestNotify.Location = new System.Drawing.Point(3, 195);
            this.btnTestNotify.Name = "btnTestNotify";
            this.btnTestNotify.Size = new System.Drawing.Size(334, 43);
            this.btnTestNotify.TabIndex = 3;
            this.btnTestNotify.Text = "Test Notify";
            this.btnTestNotify.UseVisualStyleBackColor = true;
            this.btnTestNotify.Click += new System.EventHandler(this.BtnTestNotify_Click);
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.lnklblLineNotify);
            this.panel5.Controls.Add(this.lblNtfLineToken);
            this.panel5.Controls.Add(this.txtNtfLineToken);
            this.panel5.Controls.Add(this.chkNtfUseLine);
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(334, 83);
            this.panel5.TabIndex = 2;
            // 
            // lnklblLineNotify
            // 
            this.lnklblLineNotify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnklblLineNotify.AutoSize = true;
            this.lnklblLineNotify.Location = new System.Drawing.Point(173, 55);
            this.lnklblLineNotify.Name = "lnklblLineNotify";
            this.lnklblLineNotify.Size = new System.Drawing.Size(156, 17);
            this.lnklblLineNotify.TabIndex = 3;
            this.lnklblLineNotify.TabStop = true;
            this.lnklblLineNotify.Text = "https://notify-bot.line.me/";
            this.lnklblLineNotify.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnklblLineNotify_LinkClicked);
            // 
            // lblNtfLineToken
            // 
            this.lblNtfLineToken.AutoSize = true;
            this.lblNtfLineToken.Location = new System.Drawing.Point(3, 30);
            this.lblNtfLineToken.Name = "lblNtfLineToken";
            this.lblNtfLineToken.Size = new System.Drawing.Size(42, 17);
            this.lblNtfLineToken.TabIndex = 2;
            this.lblNtfLineToken.Text = "Token";
            // 
            // txtNtfLineToken
            // 
            this.txtNtfLineToken.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNtfLineToken.Location = new System.Drawing.Point(61, 27);
            this.txtNtfLineToken.Name = "txtNtfLineToken";
            this.txtNtfLineToken.Size = new System.Drawing.Size(268, 25);
            this.txtNtfLineToken.TabIndex = 1;
            this.txtNtfLineToken.TextChanged += new System.EventHandler(this.TxtNtfLineToken_TextChanged);
            // 
            // chkNtfUseLine
            // 
            this.chkNtfUseLine.AutoSize = true;
            this.chkNtfUseLine.Location = new System.Drawing.Point(3, 3);
            this.chkNtfUseLine.Name = "chkNtfUseLine";
            this.chkNtfUseLine.Size = new System.Drawing.Size(115, 21);
            this.chkNtfUseLine.TabIndex = 0;
            this.chkNtfUseLine.Text = "Use LINE notify";
            this.chkNtfUseLine.UseVisualStyleBackColor = true;
            this.chkNtfUseLine.CheckedChanged += new System.EventHandler(this.ChkNtfUseLine_CheckedChanged);
            // 
            // ilTab
            // 
            this.ilTab.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilTab.ImageStream")));
            this.ilTab.TransparentColor = System.Drawing.Color.Transparent;
            this.ilTab.Images.SetKeyName(0, "061809.png");
            this.ilTab.Images.SetKeyName(1, "Main_Command_52_Icon.png");
            this.ilTab.Images.SetKeyName(2, "Map64_Icon.png");
            this.ilTab.Images.SetKeyName(3, "Player2_Icon.png");
            this.ilTab.Images.SetKeyName(4, "Player12_Icon.png");
            // 
            // ttCtrls
            // 
            this.ttCtrls.IsBalloon = true;
            this.ttCtrls.ShowAlways = true;
            // 
            // MainControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitBase);
            this.Name = "MainControl";
            this.Size = new System.Drawing.Size(912, 559);
            this.splitBase.Panel1.ResumeLayout(false);
            this.splitBase.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitBase)).EndInit();
            this.splitBase.ResumeLayout(false);
            this.tabLeft.ResumeLayout(false);
            this.tabPageFates.ResumeLayout(false);
            this.tabPageSetting.ResumeLayout(false);
            this.tabPageSetting.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnlLogSetting.ResumeLayout(false);
            this.pnlLogSetting.PerformLayout();
            this.tabPageNotify.ResumeLayout(false);
            this.tabPageNotify.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox rtxLogger;
        private System.Windows.Forms.TreeView trvFates;
        private System.Windows.Forms.SplitContainer splitBase;
        private System.Windows.Forms.ComboBox cboGameLanguage;
        private System.Windows.Forms.Label lblGameLanguage;
        private System.Windows.Forms.ComboBox cboUiLanguage;
        private System.Windows.Forms.Label lblUiLanguage;
        private System.Windows.Forms.Button btnClearLogs;
        private System.Windows.Forms.ComboBox cboLogBackground;
        private System.Windows.Forms.TabControl tabLeft;
        private System.Windows.Forms.TabPage tabPageFates;
        private System.Windows.Forms.TabPage tabPageSetting;
        private System.Windows.Forms.TabPage tabPageNotify;
        private System.Windows.Forms.Button btnReconnect;
        private System.Windows.Forms.Label lblBackColor;
        private System.Windows.Forms.ImageList ilTab;
        private System.Windows.Forms.CheckBox chkWholeFates;
        private System.Windows.Forms.TextBox txtSelectedFates;
        private System.Windows.Forms.CheckBox chkUseOverlay;
        private System.Windows.Forms.TextBox txtOverayLocation;
        private System.Windows.Forms.Panel pnlLogSetting;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtSoundFile;
        private System.Windows.Forms.Button btnSelectSound;
        private System.Windows.Forms.CheckBox chkUseSound;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnSoundPlay;
        private System.Windows.Forms.Button btnLogFont;
        private System.Windows.Forms.TextBox txtLogFont;
        private System.Windows.Forms.Label lblDisplayFont;
        private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.ComboBox cboClientVersion;
		private System.Windows.Forms.Label lblClientVersion;
		private System.Windows.Forms.TextBox txtClientVersion;
		private System.Windows.Forms.Button btnBlinkOverlay;
		private System.Windows.Forms.ToolTip ttCtrls;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Label lblNtfLineToken;
		private System.Windows.Forms.TextBox txtNtfLineToken;
		private System.Windows.Forms.CheckBox chkNtfUseLine;
		private System.Windows.Forms.Button btnTestNotify;
		private System.Windows.Forms.LinkLabel lnklblLineNotify;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Label lblNtfTelegramId;
		private System.Windows.Forms.TextBox txtNtfTelegramId;
		private System.Windows.Forms.CheckBox chkNtfUseTelegram;
		private System.Windows.Forms.Label lblNtfTelegramToken;
		private System.Windows.Forms.TextBox txtNtfTelegramToken;
		private System.Windows.Forms.TextBox txtUpdateSkip;
		private System.Windows.Forms.Button btnShowLogSetting;
        private System.Windows.Forms.Button btnSoundStop;
    }
}
