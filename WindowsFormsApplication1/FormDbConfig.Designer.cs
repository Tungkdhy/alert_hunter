using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApplication1
{
    partial class FormDbConfig
    {
        private System.ComponentModel.IContainer components = null;

        private Panel topPanel;
        private TabControl tabControl;
        private TabPage tabInput;
        private TabPage tabJson;
        private Panel panelJson;
        private TextBox txtJson;
        private Button btnLoad;
        private Button btnSave;
        private Button btnChooseConfig;
        private Label lblPath;

        // Input fields for database config
        private GroupBox grpDatabaseConfig;
        private TableLayoutPanel tableLayoutMain;
        private Panel panelDatabasePath;
        private Label lblDatabasePath;
        private TextBox txtDatabasePath;
        private Button btnBrowseDatabasePath;
        private Panel panelRestartService;
        private Label lblRestartService;
        private TextBox txtRestartService;

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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabInput = new System.Windows.Forms.TabPage();
            this.grpDatabaseConfig = new System.Windows.Forms.GroupBox();
            this.tableLayoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelDatabasePath = new System.Windows.Forms.Panel();
            this.lblDatabasePath = new System.Windows.Forms.Label();
            this.txtDatabasePath = new System.Windows.Forms.TextBox();
            this.btnBrowseDatabasePath = new System.Windows.Forms.Button();
            this.panelRestartService = new System.Windows.Forms.Panel();
            this.lblRestartService = new System.Windows.Forms.Label();
            this.txtRestartService = new System.Windows.Forms.TextBox();
            this.tabJson = new System.Windows.Forms.TabPage();
            this.panelJson = new System.Windows.Forms.Panel();
            this.txtJson = new System.Windows.Forms.TextBox();
            this.topPanel.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabInput.SuspendLayout();
            this.grpDatabaseConfig.SuspendLayout();
            this.tableLayoutMain.SuspendLayout();
            this.panelDatabasePath.SuspendLayout();
            this.panelRestartService.SuspendLayout();
            this.tabJson.SuspendLayout();
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
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabInput);
            this.tabControl.Controls.Add(this.tabJson);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabControl.Location = new System.Drawing.Point(0, 88);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1080, 562);
            this.tabControl.TabIndex = 0;
            // 
            // tabInput
            // 
            this.tabInput.Controls.Add(this.grpDatabaseConfig);
            this.tabInput.Location = new System.Drawing.Point(4, 32);
            this.tabInput.Name = "tabInput";
            this.tabInput.Padding = new System.Windows.Forms.Padding(3);
            this.tabInput.Size = new System.Drawing.Size(1072, 526);
            this.tabInput.TabIndex = 0;
            this.tabInput.Text = "Cấu hình";
            this.tabInput.UseVisualStyleBackColor = true;
            // 
            // grpDatabaseConfig
            // 
            this.grpDatabaseConfig.Controls.Add(this.tableLayoutMain);
            this.grpDatabaseConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDatabaseConfig.Font = new System.Drawing.Font("Segoe UI Semibold", 11F);
            this.grpDatabaseConfig.Location = new System.Drawing.Point(3, 3);
            this.grpDatabaseConfig.Name = "grpDatabaseConfig";
            this.grpDatabaseConfig.Padding = new System.Windows.Forms.Padding(40, 30, 40, 30);
            this.grpDatabaseConfig.Size = new System.Drawing.Size(1066, 520);
            this.grpDatabaseConfig.TabIndex = 0;
            this.grpDatabaseConfig.TabStop = false;
            this.grpDatabaseConfig.Text = "Cấu hình Database";
            // 
            // tableLayoutMain
            // 
            this.tableLayoutMain.ColumnCount = 2;
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutMain.Controls.Add(this.panelDatabasePath, 0, 0);
            this.tableLayoutMain.Controls.Add(this.panelRestartService, 1, 0);
            this.tableLayoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutMain.Location = new System.Drawing.Point(40, 55);
            this.tableLayoutMain.Name = "tableLayoutMain";
            this.tableLayoutMain.Padding = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.tableLayoutMain.RowCount = 1;
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutMain.Size = new System.Drawing.Size(986, 435);
            this.tableLayoutMain.TabIndex = 0;
            // 
            // panelDatabasePath
            // 
            this.panelDatabasePath.Controls.Add(this.lblDatabasePath);
            this.panelDatabasePath.Controls.Add(this.txtDatabasePath);
            this.panelDatabasePath.Controls.Add(this.btnBrowseDatabasePath);
            this.panelDatabasePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDatabasePath.Location = new System.Drawing.Point(0, 20);
            this.panelDatabasePath.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.panelDatabasePath.Name = "panelDatabasePath";
            this.panelDatabasePath.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.panelDatabasePath.Size = new System.Drawing.Size(473, 415);
            this.panelDatabasePath.TabIndex = 0;
            // 
            // lblDatabasePath
            // 
            this.lblDatabasePath.AutoSize = true;
            this.lblDatabasePath.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            this.lblDatabasePath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblDatabasePath.Location = new System.Drawing.Point(0, 0);
            this.lblDatabasePath.Name = "lblDatabasePath";
            this.lblDatabasePath.Size = new System.Drawing.Size(138, 25);
            this.lblDatabasePath.TabIndex = 0;
            this.lblDatabasePath.Text = "Đường dẫn DB:";
            // 
            // txtDatabasePath
            // 
            this.txtDatabasePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDatabasePath.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtDatabasePath.Location = new System.Drawing.Point(0, 30);
            this.txtDatabasePath.Name = "txtDatabasePath";
            this.txtDatabasePath.Size = new System.Drawing.Size(343, 30);
            this.txtDatabasePath.TabIndex = 1;
            // 
            // btnBrowseDatabasePath
            // 
            this.btnBrowseDatabasePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseDatabasePath.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.btnBrowseDatabasePath.Location = new System.Drawing.Point(353, 30);
            this.btnBrowseDatabasePath.Name = "btnBrowseDatabasePath";
            this.btnBrowseDatabasePath.Size = new System.Drawing.Size(110, 30);
            this.btnBrowseDatabasePath.TabIndex = 2;
            this.btnBrowseDatabasePath.Text = "📁 Duyệt";
            this.btnBrowseDatabasePath.UseVisualStyleBackColor = true;
            // 
            // panelRestartService
            // 
            this.panelRestartService.Controls.Add(this.lblRestartService);
            this.panelRestartService.Controls.Add(this.txtRestartService);
            this.panelRestartService.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRestartService.Location = new System.Drawing.Point(493, 20);
            this.panelRestartService.Margin = new System.Windows.Forms.Padding(0);
            this.panelRestartService.Name = "panelRestartService";
            this.panelRestartService.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.panelRestartService.Size = new System.Drawing.Size(493, 415);
            this.panelRestartService.TabIndex = 1;
            // 
            // lblRestartService
            // 
            this.lblRestartService.AutoSize = true;
            this.lblRestartService.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            this.lblRestartService.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblRestartService.Location = new System.Drawing.Point(10, 0);
            this.lblRestartService.Name = "lblRestartService";
            this.lblRestartService.Size = new System.Drawing.Size(141, 25);
            this.lblRestartService.TabIndex = 0;
            this.lblRestartService.Text = "Restart Service:";
            // 
            // txtRestartService
            // 
            this.txtRestartService.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRestartService.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtRestartService.Location = new System.Drawing.Point(10, 30);
            this.txtRestartService.Name = "txtRestartService";
            this.txtRestartService.Size = new System.Drawing.Size(473, 30);
            this.txtRestartService.TabIndex = 1;
            // 
            // tabJson
            // 
            this.tabJson.Controls.Add(this.panelJson);
            this.tabJson.Location = new System.Drawing.Point(4, 32);
            this.tabJson.Name = "tabJson";
            this.tabJson.Padding = new System.Windows.Forms.Padding(3);
            this.tabJson.Size = new System.Drawing.Size(1072, 526);
            this.tabJson.TabIndex = 1;
            this.tabJson.Text = "Nâng cao";
            this.tabJson.UseVisualStyleBackColor = true;
            // 
            // panelJson
            // 
            this.panelJson.BackColor = System.Drawing.Color.White;
            this.panelJson.Controls.Add(this.txtJson);
            this.panelJson.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelJson.Location = new System.Drawing.Point(3, 3);
            this.panelJson.Name = "panelJson";
            this.panelJson.Padding = new System.Windows.Forms.Padding(20);
            this.panelJson.Size = new System.Drawing.Size(1066, 520);
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
            this.txtJson.Location = new System.Drawing.Point(20, 20);
            this.txtJson.Multiline = true;
            this.txtJson.Name = "txtJson";
            this.txtJson.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtJson.Size = new System.Drawing.Size(1026, 480);
            this.txtJson.TabIndex = 0;
            this.txtJson.WordWrap = false;
            // 
            // FormDbConfig
            // 
            this.ClientSize = new System.Drawing.Size(1080, 650);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.topPanel);
            this.Name = "FormDbConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cấu hình Database";
            this.topPanel.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabInput.ResumeLayout(false);
            this.grpDatabaseConfig.ResumeLayout(false);
            this.tableLayoutMain.ResumeLayout(false);
            this.panelDatabasePath.ResumeLayout(false);
            this.panelDatabasePath.PerformLayout();
            this.panelRestartService.ResumeLayout(false);
            this.panelRestartService.PerformLayout();
            this.tabJson.ResumeLayout(false);
            this.panelJson.ResumeLayout(false);
            this.panelJson.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}

