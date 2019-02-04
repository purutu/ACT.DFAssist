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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rtxLogger = new System.Windows.Forms.RichTextBox();
            this.trvFates = new System.Windows.Forms.TreeView();
            this.splitBase = new System.Windows.Forms.SplitContainer();
            this.splitMenuFate = new System.Windows.Forms.SplitContainer();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.cboGameLanguage = new System.Windows.Forms.ComboBox();
            this.lblGameLanguage = new System.Windows.Forms.Label();
            this.cboUiLanguage = new System.Windows.Forms.ComboBox();
            this.lblUiLanguage = new System.Windows.Forms.Label();
            this.cboLogBackground = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitBase)).BeginInit();
            this.splitBase.Panel1.SuspendLayout();
            this.splitBase.Panel2.SuspendLayout();
            this.splitBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMenuFate)).BeginInit();
            this.splitMenuFate.Panel1.SuspendLayout();
            this.splitMenuFate.Panel2.SuspendLayout();
            this.splitMenuFate.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(105, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "This is test plugin";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(326, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "label2";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label2.Visible = false;
            // 
            // rtxLogger
            // 
            this.rtxLogger.BackColor = System.Drawing.Color.Linen;
            this.rtxLogger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxLogger.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtxLogger.Location = new System.Drawing.Point(0, 0);
            this.rtxLogger.Name = "rtxLogger";
            this.rtxLogger.ReadOnly = true;
            this.rtxLogger.Size = new System.Drawing.Size(536, 559);
            this.rtxLogger.TabIndex = 2;
            this.rtxLogger.Text = "";
            // 
            // trvFates
            // 
            this.trvFates.CheckBoxes = true;
            this.trvFates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvFates.Location = new System.Drawing.Point(0, 0);
            this.trvFates.Name = "trvFates";
            this.trvFates.Size = new System.Drawing.Size(372, 471);
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
            this.splitBase.Panel1.Controls.Add(this.splitMenuFate);
            // 
            // splitBase.Panel2
            // 
            this.splitBase.Panel2.Controls.Add(this.rtxLogger);
            this.splitBase.Size = new System.Drawing.Size(912, 559);
            this.splitBase.SplitterDistance = 372;
            this.splitBase.TabIndex = 4;
            // 
            // splitMenuFate
            // 
            this.splitMenuFate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMenuFate.Location = new System.Drawing.Point(0, 0);
            this.splitMenuFate.Name = "splitMenuFate";
            this.splitMenuFate.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitMenuFate.Panel1
            // 
            this.splitMenuFate.Panel1.Controls.Add(this.cboLogBackground);
            this.splitMenuFate.Panel1.Controls.Add(this.linkLabel1);
            this.splitMenuFate.Panel1.Controls.Add(this.btnClearLogs);
            this.splitMenuFate.Panel1.Controls.Add(this.cboGameLanguage);
            this.splitMenuFate.Panel1.Controls.Add(this.lblGameLanguage);
            this.splitMenuFate.Panel1.Controls.Add(this.cboUiLanguage);
            this.splitMenuFate.Panel1.Controls.Add(this.lblUiLanguage);
            this.splitMenuFate.Panel1.Controls.Add(this.label2);
            this.splitMenuFate.Panel1.Controls.Add(this.label1);
            // 
            // splitMenuFate.Panel2
            // 
            this.splitMenuFate.Panel2.Controls.Add(this.trvFates);
            this.splitMenuFate.Size = new System.Drawing.Size(372, 559);
            this.splitMenuFate.SplitterDistance = 84;
            this.splitMenuFate.TabIndex = 0;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(105, 68);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(92, 13);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Original by devunt";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1_LinkClicked);
            // 
            // btnClearLogs
            // 
            this.btnClearLogs.Location = new System.Drawing.Point(6, 32);
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.Size = new System.Drawing.Size(93, 49);
            this.btnClearLogs.TabIndex = 6;
            this.btnClearLogs.Text = "Clear Logs";
            this.btnClearLogs.UseVisualStyleBackColor = true;
            this.btnClearLogs.Click += new System.EventHandler(this.BtnClearLogs_Click);
            // 
            // cboGameLanguage
            // 
            this.cboGameLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGameLanguage.FormattingEnabled = true;
            this.cboGameLanguage.Location = new System.Drawing.Point(273, 3);
            this.cboGameLanguage.Name = "cboGameLanguage";
            this.cboGameLanguage.Size = new System.Drawing.Size(96, 21);
            this.cboGameLanguage.TabIndex = 5;
            this.cboGameLanguage.SelectedValueChanged += new System.EventHandler(this.CboGameLanguage_SelectedValueChanged);
            // 
            // lblGameLanguage
            // 
            this.lblGameLanguage.AutoSize = true;
            this.lblGameLanguage.Location = new System.Drawing.Point(198, 6);
            this.lblGameLanguage.Name = "lblGameLanguage";
            this.lblGameLanguage.Size = new System.Drawing.Size(47, 13);
            this.lblGameLanguage.TabIndex = 4;
            this.lblGameLanguage.Text = "In Game";
            // 
            // cboUiLanguage
            // 
            this.cboUiLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUiLanguage.FormattingEnabled = true;
            this.cboUiLanguage.Location = new System.Drawing.Point(78, 3);
            this.cboUiLanguage.Name = "cboUiLanguage";
            this.cboUiLanguage.Size = new System.Drawing.Size(96, 21);
            this.cboUiLanguage.TabIndex = 3;
            this.cboUiLanguage.SelectedValueChanged += new System.EventHandler(this.CboUiLanguage_SelectedValueChanged);
            // 
            // lblUiLanguage
            // 
            this.lblUiLanguage.AutoSize = true;
            this.lblUiLanguage.Location = new System.Drawing.Point(3, 6);
            this.lblUiLanguage.Name = "lblUiLanguage";
            this.lblUiLanguage.Size = new System.Drawing.Size(69, 13);
            this.lblUiLanguage.TabIndex = 2;
            this.lblUiLanguage.Text = "UI Language";
            // 
            // cboLogBackground
            // 
            this.cboLogBackground.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboLogBackground.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLogBackground.FormattingEnabled = true;
            this.cboLogBackground.Location = new System.Drawing.Point(233, 61);
            this.cboLogBackground.Name = "cboLogBackground";
            this.cboLogBackground.Size = new System.Drawing.Size(136, 21);
            this.cboLogBackground.TabIndex = 8;
            this.cboLogBackground.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CboLogBackground_DrawItem);
            this.cboLogBackground.SelectionChangeCommitted += new System.EventHandler(this.CboLogBackground_SelectionChangeCommitted);
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
            this.splitMenuFate.Panel1.ResumeLayout(false);
            this.splitMenuFate.Panel1.PerformLayout();
            this.splitMenuFate.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMenuFate)).EndInit();
            this.splitMenuFate.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox rtxLogger;
        private System.Windows.Forms.TreeView trvFates;
        private System.Windows.Forms.SplitContainer splitBase;
        private System.Windows.Forms.SplitContainer splitMenuFate;
        private System.Windows.Forms.ComboBox cboGameLanguage;
        private System.Windows.Forms.Label lblGameLanguage;
        private System.Windows.Forms.ComboBox cboUiLanguage;
        private System.Windows.Forms.Label lblUiLanguage;
        private System.Windows.Forms.Button btnClearLogs;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.ComboBox cboLogBackground;
    }
}
