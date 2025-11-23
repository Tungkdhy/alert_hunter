namespace WindowsFormsApplication1
{
    partial class FormAlertDetail
    {
        private System.ComponentModel.IContainer components = null;
        
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelSeverity;
        private System.Windows.Forms.Label lblSeverityValue;
        private System.Windows.Forms.Label lblAgent;
        private System.Windows.Forms.Label lblAgentValue;
        private System.Windows.Forms.Label lblHostname;
        private System.Windows.Forms.Label lblHostnameValue;
        private System.Windows.Forms.Label lblMac;
        private System.Windows.Forms.Label lblMacValue;
        private System.Windows.Forms.Label lblIp;
        private System.Windows.Forms.Label lblIpValue;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.Label lblFileNameValue;
        private System.Windows.Forms.Label lblFilePath;
        private System.Windows.Forms.Label lblFilePathValue;
        private System.Windows.Forms.Label lblReason;
        private System.Windows.Forms.Label lblReasonValue;
        private System.Windows.Forms.Label lblRule;
        private System.Windows.Forms.Label lblRuleValue;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblTimeValue;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.Label lblScoreValue;
        private System.Windows.Forms.Button btnClose;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelMain = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelSeverity = new System.Windows.Forms.Panel();
            this.lblSeverityValue = new System.Windows.Forms.Label();
            this.lblAgent = new System.Windows.Forms.Label();
            this.lblAgentValue = new System.Windows.Forms.Label();
            this.lblHostname = new System.Windows.Forms.Label();
            this.lblHostnameValue = new System.Windows.Forms.Label();
            this.lblMac = new System.Windows.Forms.Label();
            this.lblMacValue = new System.Windows.Forms.Label();
            this.lblIp = new System.Windows.Forms.Label();
            this.lblIpValue = new System.Windows.Forms.Label();
            this.lblFileName = new System.Windows.Forms.Label();
            this.lblFileNameValue = new System.Windows.Forms.Label();
            this.lblFilePath = new System.Windows.Forms.Label();
            this.lblFilePathValue = new System.Windows.Forms.Label();
            this.lblReason = new System.Windows.Forms.Label();
            this.lblReasonValue = new System.Windows.Forms.Label();
            this.lblRule = new System.Windows.Forms.Label();
            this.lblRuleValue = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblTimeValue = new System.Windows.Forms.Label();
            this.lblScore = new System.Windows.Forms.Label();
            this.lblScoreValue = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.panelMain.SuspendLayout();
            this.panelSeverity.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.AutoScroll = true;
            this.panelMain.BackColor = System.Drawing.Color.White;
            this.panelMain.Controls.Add(this.btnClose);
            this.panelMain.Controls.Add(this.lblScoreValue);
            this.panelMain.Controls.Add(this.lblScore);
            this.panelMain.Controls.Add(this.lblTimeValue);
            this.panelMain.Controls.Add(this.lblTime);
            this.panelMain.Controls.Add(this.lblRuleValue);
            this.panelMain.Controls.Add(this.lblRule);
            this.panelMain.Controls.Add(this.lblReasonValue);
            this.panelMain.Controls.Add(this.lblReason);
            this.panelMain.Controls.Add(this.lblFilePathValue);
            this.panelMain.Controls.Add(this.lblFilePath);
            this.panelMain.Controls.Add(this.lblFileNameValue);
            this.panelMain.Controls.Add(this.lblFileName);
            this.panelMain.Controls.Add(this.lblIpValue);
            this.panelMain.Controls.Add(this.lblIp);
            this.panelMain.Controls.Add(this.lblMacValue);
            this.panelMain.Controls.Add(this.lblMac);
            this.panelMain.Controls.Add(this.lblHostnameValue);
            this.panelMain.Controls.Add(this.lblHostname);
            this.panelMain.Controls.Add(this.lblAgentValue);
            this.panelMain.Controls.Add(this.lblAgent);
            this.panelMain.Controls.Add(this.panelSeverity);
            this.panelMain.Controls.Add(this.lblTitle);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(30);
            this.panelMain.Size = new System.Drawing.Size(700, 600);
            this.panelMain.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(25, 55, 120);
            this.lblTitle.Location = new System.Drawing.Point(30, 30);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Chi tiết cảnh báo";
            // 
            // panelSeverity
            // 
            this.panelSeverity.Controls.Add(this.lblSeverityValue);
            this.panelSeverity.Location = new System.Drawing.Point(30, 80);
            this.panelSeverity.Name = "panelSeverity";
            this.panelSeverity.Padding = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.panelSeverity.Size = new System.Drawing.Size(640, 50);
            this.panelSeverity.TabIndex = 1;
            // 
            // lblSeverityValue
            // 
            this.lblSeverityValue.AutoSize = true;
            this.lblSeverityValue.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblSeverityValue.Location = new System.Drawing.Point(15, 10);
            this.lblSeverityValue.Name = "lblSeverityValue";
            this.lblSeverityValue.Size = new System.Drawing.Size(0, 32);
            this.lblSeverityValue.TabIndex = 0;
            // 
            // lblAgent
            // 
            this.lblAgent.AutoSize = true;
            this.lblAgent.Font = new System.Drawing.Font("Segoe UI Semibold", 11F);
            this.lblAgent.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.lblAgent.Location = new System.Drawing.Point(30, 160);
            this.lblAgent.Name = "lblAgent";
            this.lblAgent.Size = new System.Drawing.Size(60, 25);
            this.lblAgent.TabIndex = 2;
            this.lblAgent.Text = "Agent:";
            // 
            // lblAgentValue
            // 
            this.lblAgentValue.AutoSize = true;
            this.lblAgentValue.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblAgentValue.Location = new System.Drawing.Point(200, 160);
            this.lblAgentValue.Name = "lblAgentValue";
            this.lblAgentValue.Size = new System.Drawing.Size(0, 25);
            this.lblAgentValue.TabIndex = 3;
            // 
            // lblHostname
            // 
            this.lblHostname.AutoSize = true;
            this.lblHostname.Font = new System.Drawing.Font("Segoe UI Semibold", 11F);
            this.lblHostname.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.lblHostname.Location = new System.Drawing.Point(30, 195);
            this.lblHostname.Name = "lblHostname";
            this.lblHostname.Size = new System.Drawing.Size(95, 25);
            this.lblHostname.TabIndex = 4;
            this.lblHostname.Text = "Tên máy:";
            // 
            // lblHostnameValue
            // 
            this.lblHostnameValue.AutoSize = true;
            this.lblHostnameValue.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblHostnameValue.Location = new System.Drawing.Point(200, 195);
            this.lblHostnameValue.Name = "lblHostnameValue";
            this.lblHostnameValue.Size = new System.Drawing.Size(0, 25);
            this.lblHostnameValue.TabIndex = 5;
            // 
            // lblMac
            // 
            this.lblMac.AutoSize = true;
            this.lblMac.Font = new System.Drawing.Font("Segoe UI Semibold", 11F);
            this.lblMac.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.lblMac.Location = new System.Drawing.Point(30, 230);
            this.lblMac.Name = "lblMac";
            this.lblMac.Size = new System.Drawing.Size(50, 25);
            this.lblMac.TabIndex = 6;
            this.lblMac.Text = "MAC:";
            // 
            // lblMacValue
            // 
            this.lblMacValue.AutoSize = true;
            this.lblMacValue.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblMacValue.Location = new System.Drawing.Point(200, 230);
            this.lblMacValue.Name = "lblMacValue";
            this.lblMacValue.Size = new System.Drawing.Size(0, 25);
            this.lblMacValue.TabIndex = 7;
            // 
            // lblIp
            // 
            this.lblIp.AutoSize = true;
            this.lblIp.Font = new System.Drawing.Font("Segoe UI Semibold", 11F);
            this.lblIp.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.lblIp.Location = new System.Drawing.Point(30, 265);
            this.lblIp.Name = "lblIp";
            this.lblIp.Size = new System.Drawing.Size(35, 25);
            this.lblIp.TabIndex = 8;
            this.lblIp.Text = "IP:";
            // 
            // lblIpValue
            // 
            this.lblIpValue.AutoSize = true;
            this.lblIpValue.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblIpValue.Location = new System.Drawing.Point(200, 265);
            this.lblIpValue.Name = "lblIpValue";
            this.lblIpValue.Size = new System.Drawing.Size(0, 25);
            this.lblIpValue.TabIndex = 9;
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Font = new System.Drawing.Font("Segoe UI Semibold", 11F);
            this.lblFileName.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.lblFileName.Location = new System.Drawing.Point(30, 300);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(85, 25);
            this.lblFileName.TabIndex = 10;
            this.lblFileName.Text = "Tên tệp:";
            // 
            // lblFileNameValue
            // 
            this.lblFileNameValue.AutoSize = true;
            this.lblFileNameValue.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblFileNameValue.Location = new System.Drawing.Point(200, 300);
            this.lblFileNameValue.Name = "lblFileNameValue";
            this.lblFileNameValue.Size = new System.Drawing.Size(0, 25);
            this.lblFileNameValue.TabIndex = 11;
            // 
            // lblFilePath
            // 
            this.lblFilePath.AutoSize = true;
            this.lblFilePath.Font = new System.Drawing.Font("Segoe UI Semibold", 11F);
            this.lblFilePath.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.lblFilePath.Location = new System.Drawing.Point(30, 335);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(105, 25);
            this.lblFilePath.TabIndex = 12;
            this.lblFilePath.Text = "Đường dẫn:";
            // 
            // lblFilePathValue
            // 
            this.lblFilePathValue.AutoSize = true;
            this.lblFilePathValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblFilePathValue.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.lblFilePathValue.Location = new System.Drawing.Point(200, 335);
            this.lblFilePathValue.MaximumSize = new System.Drawing.Size(470, 0);
            this.lblFilePathValue.Name = "lblFilePathValue";
            this.lblFilePathValue.Size = new System.Drawing.Size(0, 23);
            this.lblFilePathValue.TabIndex = 13;
            // 
            // lblReason
            // 
            this.lblReason.AutoSize = true;
            this.lblReason.Font = new System.Drawing.Font("Segoe UI Semibold", 11F);
            this.lblReason.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.lblReason.Location = new System.Drawing.Point(30, 370);
            this.lblReason.Name = "lblReason";
            this.lblReason.Size = new System.Drawing.Size(75, 25);
            this.lblReason.TabIndex = 14;
            this.lblReason.Text = "Lý do:";
            // 
            // lblReasonValue
            // 
            this.lblReasonValue.AutoSize = true;
            this.lblReasonValue.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblReasonValue.ForeColor = System.Drawing.Color.FromArgb(200, 0, 0);
            this.lblReasonValue.Location = new System.Drawing.Point(200, 370);
            this.lblReasonValue.MaximumSize = new System.Drawing.Size(470, 0);
            this.lblReasonValue.Name = "lblReasonValue";
            this.lblReasonValue.Size = new System.Drawing.Size(0, 25);
            this.lblReasonValue.TabIndex = 15;
            // 
            // lblRule
            // 
            this.lblRule.AutoSize = true;
            this.lblRule.Font = new System.Drawing.Font("Segoe UI Semibold", 11F);
            this.lblRule.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.lblRule.Location = new System.Drawing.Point(30, 405);
            this.lblRule.Name = "lblRule";
            this.lblRule.Size = new System.Drawing.Size(50, 25);
            this.lblRule.TabIndex = 16;
            this.lblRule.Text = "Rule:";
            // 
            // lblRuleValue
            // 
            this.lblRuleValue.AutoSize = true;
            this.lblRuleValue.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblRuleValue.Location = new System.Drawing.Point(200, 405);
            this.lblRuleValue.Name = "lblRuleValue";
            this.lblRuleValue.Size = new System.Drawing.Size(0, 25);
            this.lblRuleValue.TabIndex = 17;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Segoe UI Semibold", 11F);
            this.lblTime.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.lblTime.Location = new System.Drawing.Point(30, 440);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(105, 25);
            this.lblTime.TabIndex = 18;
            this.lblTime.Text = "Thời gian:";
            // 
            // lblTimeValue
            // 
            this.lblTimeValue.AutoSize = true;
            this.lblTimeValue.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblTimeValue.Location = new System.Drawing.Point(200, 440);
            this.lblTimeValue.Name = "lblTimeValue";
            this.lblTimeValue.Size = new System.Drawing.Size(0, 25);
            this.lblTimeValue.TabIndex = 19;
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.Font = new System.Drawing.Font("Segoe UI Semibold", 11F);
            this.lblScore.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
            this.lblScore.Location = new System.Drawing.Point(30, 475);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(60, 25);
            this.lblScore.TabIndex = 20;
            this.lblScore.Text = "Điểm:";
            // 
            // lblScoreValue
            // 
            this.lblScoreValue.AutoSize = true;
            this.lblScoreValue.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lblScoreValue.ForeColor = System.Drawing.Color.FromArgb(25, 55, 120);
            this.lblScoreValue.Location = new System.Drawing.Point(200, 475);
            this.lblScoreValue.Name = "lblScoreValue";
            this.lblScoreValue.Size = new System.Drawing.Size(0, 28);
            this.lblScoreValue.TabIndex = 21;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(25, 55, 120);
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(500, 520);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(170, 45);
            this.btnClose.TabIndex = 22;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FormAlertDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(700, 600);
            this.Controls.Add(this.panelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAlertDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chi tiết cảnh báo";
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.panelSeverity.ResumeLayout(false);
            this.panelSeverity.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}

