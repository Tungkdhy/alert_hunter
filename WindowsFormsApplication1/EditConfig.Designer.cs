using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WindowsFormsApplication1
{
    partial class EditConfig
    {
        private System.ComponentModel.IContainer components = null;

        private Panel topPanel;
        private Panel panelJson;
        private TextBox txtJson;
        private Button btnLoad;
        private Button btnSave;
        private Button btnChooseConfig;
        private Label lblPath;
        private ProgressBar progressScan;
        private Label lblScanStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.topPanel = new System.Windows.Forms.Panel();
            this.btnChooseConfig = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblPath = new System.Windows.Forms.Label();
            this.panelJson = new System.Windows.Forms.Panel();
            this.txtJson = new System.Windows.Forms.TextBox();
            this.progressScan = new System.Windows.Forms.ProgressBar();
            this.lblScanStatus = new System.Windows.Forms.Label();
            this.topPanel.SuspendLayout();
            this.panelJson.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.topPanel.Controls.Add(this.btnChooseConfig);
            this.topPanel.Controls.Add(this.btnLoad);
            this.topPanel.Controls.Add(this.btnSave);
            this.topPanel.Controls.Add(this.lblPath);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.topPanel.Size = new System.Drawing.Size(1080, 88);
            this.topPanel.TabIndex = 4;
            // 
            // btnChooseConfig
            // 
            this.btnChooseConfig.BackColor = System.Drawing.Color.White;
            this.btnChooseConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChooseConfig.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.btnChooseConfig.Location = new System.Drawing.Point(20, 40);
            this.btnChooseConfig.Name = "btnChooseConfig";
            this.btnChooseConfig.Size = new System.Drawing.Size(160, 38);
            this.btnChooseConfig.TabIndex = 0;
            this.btnChooseConfig.Text = "📁 Chọn File Config";
            this.btnChooseConfig.UseVisualStyleBackColor = false;
            // 
            // btnLoad
            // 
            this.btnLoad.BackColor = System.Drawing.Color.White;
            this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoad.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.btnLoad.Location = new System.Drawing.Point(190, 40);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(110, 38);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = "📄 Load";
            this.btnLoad.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(55)))), ((int)(((byte)(120)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(310, 40);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(130, 38);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "💾 Lưu";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // lblPath
            // 
            this.lblPath.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            this.lblPath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(55)))), ((int)(((byte)(120)))));
            this.lblPath.Location = new System.Drawing.Point(20, 5);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(1000, 25);
            this.lblPath.TabIndex = 4;
            this.lblPath.Text = "Đường dẫn config:";
            // 
            // panelJson
            // 
            this.panelJson.BackColor = System.Drawing.Color.White;
            this.panelJson.Controls.Add(this.txtJson);
            this.panelJson.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelJson.Location = new System.Drawing.Point(0, 88);
            this.panelJson.Name = "panelJson";
            this.panelJson.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.panelJson.Size = new System.Drawing.Size(1080, 526);
            this.panelJson.TabIndex = 0;
            // 
            // txtJson
            // 
            this.txtJson.AcceptsReturn = true;
            this.txtJson.AcceptsTab = true;
            this.txtJson.BackColor = System.Drawing.Color.White;
            this.txtJson.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtJson.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtJson.Font = new System.Drawing.Font("Consolas", 10.5F);
            this.txtJson.Location = new System.Drawing.Point(20, 0);
            this.txtJson.Multiline = true;
            this.txtJson.Name = "txtJson";
            this.txtJson.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtJson.Size = new System.Drawing.Size(1040, 526);
            this.txtJson.TabIndex = 0;
            this.txtJson.WordWrap = false;
            // 
            // progressScan
            // 
            this.progressScan.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressScan.Location = new System.Drawing.Point(0, 644);
            this.progressScan.Name = "progressScan";
            this.progressScan.Size = new System.Drawing.Size(1080, 6);
            this.progressScan.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressScan.TabIndex = 2;
            // 
            // lblScanStatus
            // 
            this.lblScanStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.lblScanStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblScanStatus.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblScanStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblScanStatus.Location = new System.Drawing.Point(0, 614);
            this.lblScanStatus.Name = "lblScanStatus";
            this.lblScanStatus.Padding = new System.Windows.Forms.Padding(15, 5, 15, 5);
            this.lblScanStatus.Size = new System.Drawing.Size(1080, 30);
            this.lblScanStatus.TabIndex = 1;
            this.lblScanStatus.Text = "Trạng thái: Chưa quét";
            // 
            // EditConfig
            // 
            this.ClientSize = new System.Drawing.Size(1080, 650);
            this.Controls.Add(this.panelJson);
            this.Controls.Add(this.lblScanStatus);
            this.Controls.Add(this.progressScan);
            this.Controls.Add(this.topPanel);
            this.Name = "EditConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chỉnh sửa Config WebShell Agent";
            this.topPanel.ResumeLayout(false);
            this.panelJson.ResumeLayout(false);
            this.panelJson.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
