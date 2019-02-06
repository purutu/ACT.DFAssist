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
            this.chkWholeFates = new System.Windows.Forms.CheckBox();
            this.btnReconnect = new System.Windows.Forms.Button();
            this.lblBackColor = new System.Windows.Forms.Label();
            this.cboLogBackground = new System.Windows.Forms.ComboBox();
            this.lblUiLanguage = new System.Windows.Forms.Label();
            this.cboUiLanguage = new System.Windows.Forms.ComboBox();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.cboGameLanguage = new System.Windows.Forms.ComboBox();
            this.lblGameLanguage = new System.Windows.Forms.Label();
            this.tabPageInformation = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.ilTab = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitBase)).BeginInit();
            this.splitBase.Panel1.SuspendLayout();
            this.splitBase.Panel2.SuspendLayout();
            this.splitBase.SuspendLayout();
            this.tabLeft.SuspendLayout();
            this.tabPageFates.SuspendLayout();
            this.tabPageSetting.SuspendLayout();
            this.tabPageInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "This is test plugin";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 474);
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
            this.rtxLogger.Size = new System.Drawing.Size(658, 559);
            this.rtxLogger.TabIndex = 2;
            this.rtxLogger.Text = "";
            // 
            // trvFates
            // 
            this.trvFates.CheckBoxes = true;
            this.trvFates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvFates.Location = new System.Drawing.Point(3, 3);
            this.trvFates.Name = "trvFates";
            this.trvFates.Size = new System.Drawing.Size(236, 510);
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
            this.splitBase.Panel1.Controls.Add(this.tabLeft);
            // 
            // splitBase.Panel2
            // 
            this.splitBase.Panel2.Controls.Add(this.rtxLogger);
            this.splitBase.Size = new System.Drawing.Size(912, 559);
            this.splitBase.SplitterDistance = 250;
            this.splitBase.TabIndex = 4;
            // 
            // tabLeft
            // 
            this.tabLeft.Controls.Add(this.tabPageFates);
            this.tabLeft.Controls.Add(this.tabPageSetting);
            this.tabLeft.Controls.Add(this.tabPageInformation);
            this.tabLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabLeft.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabLeft.ImageList = this.ilTab;
            this.tabLeft.Location = new System.Drawing.Point(0, 0);
            this.tabLeft.Name = "tabLeft";
            this.tabLeft.SelectedIndex = 0;
            this.tabLeft.Size = new System.Drawing.Size(250, 559);
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
            this.tabPageFates.Size = new System.Drawing.Size(242, 516);
            this.tabPageFates.TabIndex = 0;
            this.tabPageFates.Text = "FATEs";
            this.tabPageFates.UseVisualStyleBackColor = true;
            // 
            // tabPageSetting
            // 
            this.tabPageSetting.Controls.Add(this.chkWholeFates);
            this.tabPageSetting.Controls.Add(this.btnReconnect);
            this.tabPageSetting.Controls.Add(this.lblBackColor);
            this.tabPageSetting.Controls.Add(this.cboLogBackground);
            this.tabPageSetting.Controls.Add(this.lblUiLanguage);
            this.tabPageSetting.Controls.Add(this.cboUiLanguage);
            this.tabPageSetting.Controls.Add(this.btnClearLogs);
            this.tabPageSetting.Controls.Add(this.cboGameLanguage);
            this.tabPageSetting.Controls.Add(this.lblGameLanguage);
            this.tabPageSetting.ImageIndex = 1;
            this.tabPageSetting.Location = new System.Drawing.Point(4, 39);
            this.tabPageSetting.Name = "tabPageSetting";
            this.tabPageSetting.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSetting.Size = new System.Drawing.Size(242, 516);
            this.tabPageSetting.TabIndex = 1;
            this.tabPageSetting.Text = "Setting";
            this.tabPageSetting.UseVisualStyleBackColor = true;
            // 
            // chkWholeFates
            // 
            this.chkWholeFates.AutoSize = true;
            this.chkWholeFates.Location = new System.Drawing.Point(9, 92);
            this.chkWholeFates.Name = "chkWholeFates";
            this.chkWholeFates.Size = new System.Drawing.Size(149, 21);
            this.chkWholeFates.TabIndex = 11;
            this.chkWholeFates.Text = "Logging whole FATEs";
            this.chkWholeFates.UseVisualStyleBackColor = true;
            this.chkWholeFates.CheckedChanged += new System.EventHandler(this.ChkWholeFates_CheckedChanged);
            // 
            // btnReconnect
            // 
            this.btnReconnect.Image = global::ACT.DFAssist.Properties.Resources.Player2_Icon;
            this.btnReconnect.Location = new System.Drawing.Point(6, 253);
            this.btnReconnect.Name = "btnReconnect";
            this.btnReconnect.Size = new System.Drawing.Size(230, 46);
            this.btnReconnect.TabIndex = 10;
            this.btnReconnect.Text = "Reconnect";
            this.btnReconnect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReconnect.UseVisualStyleBackColor = true;
            this.btnReconnect.Click += new System.EventHandler(this.BtnReconnect_Click);
            // 
            // lblBackColor
            // 
            this.lblBackColor.AutoSize = true;
            this.lblBackColor.Location = new System.Drawing.Point(6, 63);
            this.lblBackColor.Name = "lblBackColor";
            this.lblBackColor.Size = new System.Drawing.Size(70, 17);
            this.lblBackColor.TabIndex = 9;
            this.lblBackColor.Text = "Back Color";
            // 
            // cboLogBackground
            // 
            this.cboLogBackground.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboLogBackground.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLogBackground.FormattingEnabled = true;
            this.cboLogBackground.Location = new System.Drawing.Point(95, 60);
            this.cboLogBackground.Name = "cboLogBackground";
            this.cboLogBackground.Size = new System.Drawing.Size(141, 26);
            this.cboLogBackground.TabIndex = 8;
            this.cboLogBackground.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CboLogBackground_DrawItem);
            this.cboLogBackground.SelectedValueChanged += new System.EventHandler(this.CboLogBackground_SelectedValueChanged);
            // 
            // lblUiLanguage
            // 
            this.lblUiLanguage.AutoSize = true;
            this.lblUiLanguage.Location = new System.Drawing.Point(6, 9);
            this.lblUiLanguage.Name = "lblUiLanguage";
            this.lblUiLanguage.Size = new System.Drawing.Size(81, 17);
            this.lblUiLanguage.TabIndex = 2;
            this.lblUiLanguage.Text = "UI Language";
            // 
            // cboUiLanguage
            // 
            this.cboUiLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUiLanguage.FormattingEnabled = true;
            this.cboUiLanguage.Location = new System.Drawing.Point(95, 6);
            this.cboUiLanguage.Name = "cboUiLanguage";
            this.cboUiLanguage.Size = new System.Drawing.Size(141, 25);
            this.cboUiLanguage.TabIndex = 3;
            this.cboUiLanguage.SelectedValueChanged += new System.EventHandler(this.CboUiLanguage_SelectedValueChanged);
            // 
            // btnClearLogs
            // 
            this.btnClearLogs.Image = global::ACT.DFAssist.Properties.Resources.btn_del;
            this.btnClearLogs.Location = new System.Drawing.Point(6, 190);
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.Size = new System.Drawing.Size(230, 44);
            this.btnClearLogs.TabIndex = 6;
            this.btnClearLogs.Text = "Clear Logs";
            this.btnClearLogs.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClearLogs.UseVisualStyleBackColor = true;
            this.btnClearLogs.Click += new System.EventHandler(this.BtnClearLogs_Click);
            // 
            // cboGameLanguage
            // 
            this.cboGameLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGameLanguage.FormattingEnabled = true;
            this.cboGameLanguage.Location = new System.Drawing.Point(95, 33);
            this.cboGameLanguage.Name = "cboGameLanguage";
            this.cboGameLanguage.Size = new System.Drawing.Size(141, 25);
            this.cboGameLanguage.TabIndex = 5;
            this.cboGameLanguage.SelectedValueChanged += new System.EventHandler(this.CboGameLanguage_SelectedValueChanged);
            // 
            // lblGameLanguage
            // 
            this.lblGameLanguage.AutoSize = true;
            this.lblGameLanguage.Location = new System.Drawing.Point(6, 36);
            this.lblGameLanguage.Name = "lblGameLanguage";
            this.lblGameLanguage.Size = new System.Drawing.Size(56, 17);
            this.lblGameLanguage.TabIndex = 4;
            this.lblGameLanguage.Text = "In Game";
            // 
            // tabPageInformation
            // 
            this.tabPageInformation.Controls.Add(this.label3);
            this.tabPageInformation.Controls.Add(this.linkLabel2);
            this.tabPageInformation.Controls.Add(this.linkLabel1);
            this.tabPageInformation.Controls.Add(this.label2);
            this.tabPageInformation.Controls.Add(this.label1);
            this.tabPageInformation.ImageIndex = 4;
            this.tabPageInformation.Location = new System.Drawing.Point(4, 39);
            this.tabPageInformation.Name = "tabPageInformation";
            this.tabPageInformation.Size = new System.Drawing.Size(242, 516);
            this.tabPageInformation.TabIndex = 2;
            this.tabPageInformation.Text = "Info";
            this.tabPageInformation.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(191, 51);
            this.label3.TabIndex = 9;
            this.label3.Text = "Thank you for using this plugin!\r\n\r\nHave good games!\r\n";
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(3, 58);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(137, 17);
            this.linkLabel2.TabIndex = 8;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Idea from lalafellsleep";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(3, 41);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(115, 17);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Original by devunt";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1_LinkClicked);
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
            this.tabPageInformation.ResumeLayout(false);
            this.tabPageInformation.PerformLayout();
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
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.ComboBox cboLogBackground;
        private System.Windows.Forms.TabControl tabLeft;
        private System.Windows.Forms.TabPage tabPageFates;
        private System.Windows.Forms.TabPage tabPageSetting;
        private System.Windows.Forms.TabPage tabPageInformation;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Button btnReconnect;
        private System.Windows.Forms.Label lblBackColor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ImageList ilTab;
        private System.Windows.Forms.CheckBox chkWholeFates;
    }
}
