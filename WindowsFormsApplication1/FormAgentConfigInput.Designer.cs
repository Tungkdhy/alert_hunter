using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApplication1
{
    partial class FormAgentConfigInput
    {
        private System.ComponentModel.IContainer components = null;

        private TabControl tabControl;
        private TabPage tabInput;
        private TabPage tabJson;
        private Panel topPanel;
        private Button btnChooseConfig;
        private Button btnLoad;
        private Button btnSave;
        private Button btnRunBat;
        private Label lblPath;
        private TextBox txtJson;
        private Panel panelJson;

        private GroupBox grpServerConnection;
        private Label lblServerIP;
        private TextBox txtServerIP;
        private Label lblServerPort;
        private TextBox txtServerPort;
        private GroupBox grpAgentConfig;
        private Label lblAgentID;
        private TextBox txtAgentID;
        private Label lblApiKey;
        private TextBox txtApiKey;
        private Label lblVersion;
        private TextBox txtVersion;
        private GroupBox grpConnectionSettings;
        private Label lblHttpTimeout;
        private TextBox txtHttpTimeout;
        private Label lblDebounce;
        private TextBox txtDebounce;

        // Single path fields
        private Label lblYaraRulesDir;
        private TextBox txtYaraRulesDir;
        private Button btnBrowseYaraRulesDir;

        private Label lblSqlitePath;
        private TextBox txtSqlitePath;
        private Button btnBrowseSqlitePath;

        private Label lblProgramDir;
        private TextBox txtProgramDir;
        private Button btnBrowseProgramDir;

        // GroupBox for File Paths
        private GroupBox grpFilePaths;
        private TableLayoutPanel tableLayoutInput;

        // Array path fields
        private GroupBox grpMonitorDirs;
        private TextBox txtInputMonitorDir;
        private ListBox lstMonitorDirs;
        private Button btnAddMonitorDir;
        private Button btnRemoveMonitorDir;
        private Button btnBrowseMonitorDir;
        private CheckBox chkInitHashTaken;

        private GroupBox grpIgnoreHash;
        private TextBox txtInputIgnoreHash;
        private ListBox lstIgnoreHash;
        private Button btnAddIgnoreHash;
        private Button btnRemoveIgnoreHash;
        private Button btnBrowseIgnoreHash;

        private GroupBox grpIgnoreScanYara;
        private TextBox txtInputIgnoreScanYara;
        private ListBox lstIgnoreScanYara;
        private Button btnAddIgnoreScanYara;
        private Button btnRemoveIgnoreScanYara;
        private Button btnBrowseIgnoreScanYara;

        private GroupBox grpIgnoreSendAlert;
        private TextBox txtInputIgnoreSendAlert;
        private ListBox lstIgnoreSendAlert;
        private Button btnAddIgnoreSendAlert;
        private Button btnRemoveIgnoreSendAlert;
        private Button btnBrowseIgnoreSendAlert;

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
            this.btnRunBat = new System.Windows.Forms.Button();
            this.lblPath = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabInput = new System.Windows.Forms.TabPage();
            this.scrollPanel = new System.Windows.Forms.Panel();
            this.grpConnectionSettings = new System.Windows.Forms.GroupBox();
            this.lblHttpTimeout = new System.Windows.Forms.Label();
            this.txtHttpTimeout = new System.Windows.Forms.TextBox();
            this.lblDebounce = new System.Windows.Forms.Label();
            this.txtDebounce = new System.Windows.Forms.TextBox();
            this.grpAgentConfig = new System.Windows.Forms.GroupBox();
            this.lblAgentID = new System.Windows.Forms.Label();
            this.txtAgentID = new System.Windows.Forms.TextBox();
            this.lblApiKey = new System.Windows.Forms.Label();
            this.txtApiKey = new System.Windows.Forms.TextBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.grpServerConnection = new System.Windows.Forms.GroupBox();
            this.lblServerIP = new System.Windows.Forms.Label();
            this.txtServerIP = new System.Windows.Forms.TextBox();
            this.lblServerPort = new System.Windows.Forms.Label();
            this.txtServerPort = new System.Windows.Forms.TextBox();
            this.grpFilePaths = new System.Windows.Forms.GroupBox();
            this.lblProgramDir = new System.Windows.Forms.Label();
            this.txtProgramDir = new System.Windows.Forms.TextBox();
            this.btnBrowseProgramDir = new System.Windows.Forms.Button();
            this.lblSqlitePath = new System.Windows.Forms.Label();
            this.txtSqlitePath = new System.Windows.Forms.TextBox();
            this.btnBrowseSqlitePath = new System.Windows.Forms.Button();
            this.lblYaraRulesDir = new System.Windows.Forms.Label();
            this.txtYaraRulesDir = new System.Windows.Forms.TextBox();
            this.btnBrowseYaraRulesDir = new System.Windows.Forms.Button();
            this.tableLayoutInput = new System.Windows.Forms.TableLayoutPanel();
            this.grpMonitorDirs = new System.Windows.Forms.GroupBox();
            this.chkInitHashTaken = new System.Windows.Forms.CheckBox();
            this.txtInputMonitorDir = new System.Windows.Forms.TextBox();
            this.lstMonitorDirs = new System.Windows.Forms.ListBox();
            this.btnAddMonitorDir = new System.Windows.Forms.Button();
            this.btnRemoveMonitorDir = new System.Windows.Forms.Button();
            this.btnBrowseMonitorDir = new System.Windows.Forms.Button();
            this.grpIgnoreHash = new System.Windows.Forms.GroupBox();
            this.txtInputIgnoreHash = new System.Windows.Forms.TextBox();
            this.lstIgnoreHash = new System.Windows.Forms.ListBox();
            this.btnAddIgnoreHash = new System.Windows.Forms.Button();
            this.btnRemoveIgnoreHash = new System.Windows.Forms.Button();
            this.btnBrowseIgnoreHash = new System.Windows.Forms.Button();
            this.grpIgnoreScanYara = new System.Windows.Forms.GroupBox();
            this.txtInputIgnoreScanYara = new System.Windows.Forms.TextBox();
            this.lstIgnoreScanYara = new System.Windows.Forms.ListBox();
            this.btnAddIgnoreScanYara = new System.Windows.Forms.Button();
            this.btnRemoveIgnoreScanYara = new System.Windows.Forms.Button();
            this.btnBrowseIgnoreScanYara = new System.Windows.Forms.Button();
            this.grpIgnoreSendAlert = new System.Windows.Forms.GroupBox();
            this.txtInputIgnoreSendAlert = new System.Windows.Forms.TextBox();
            this.lstIgnoreSendAlert = new System.Windows.Forms.ListBox();
            this.btnAddIgnoreSendAlert = new System.Windows.Forms.Button();
            this.btnRemoveIgnoreSendAlert = new System.Windows.Forms.Button();
            this.btnBrowseIgnoreSendAlert = new System.Windows.Forms.Button();
            this.tabJson = new System.Windows.Forms.TabPage();
            this.panelJson = new System.Windows.Forms.Panel();
            this.txtJson = new System.Windows.Forms.TextBox();
            this.topPanel.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabInput.SuspendLayout();
            this.scrollPanel.SuspendLayout();
            this.grpConnectionSettings.SuspendLayout();
            this.grpAgentConfig.SuspendLayout();
            this.grpServerConnection.SuspendLayout();
            this.grpFilePaths.SuspendLayout();
            this.tableLayoutInput.SuspendLayout();
            this.grpMonitorDirs.SuspendLayout();
            this.grpIgnoreHash.SuspendLayout();
            this.grpIgnoreScanYara.SuspendLayout();
            this.grpIgnoreSendAlert.SuspendLayout();
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
            this.topPanel.Controls.Add(this.btnRunBat);
            this.topPanel.Controls.Add(this.lblPath);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.topPanel.Size = new System.Drawing.Size(1359, 88);
            this.topPanel.TabIndex = 0;
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
            this.btnChooseConfig.Text = "📁 Chọn cấu hình";
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
            // btnRunBat
            // 
            this.btnRunBat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnRunBat.FlatAppearance.BorderSize = 0;
            this.btnRunBat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRunBat.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.btnRunBat.ForeColor = System.Drawing.Color.White;
            this.btnRunBat.Location = new System.Drawing.Point(450, 40);
            this.btnRunBat.Name = "btnRunBat";
            this.btnRunBat.Size = new System.Drawing.Size(140, 38);
            this.btnRunBat.TabIndex = 3;
            this.btnRunBat.Text = "▶ Chạy Agent";
            this.btnRunBat.UseVisualStyleBackColor = false;
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
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.tabControl.Location = new System.Drawing.Point(0, 88);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1359, 540);
            this.tabControl.TabIndex = 1;
            // 
            // tabInput
            // 
            this.tabInput.Controls.Add(this.scrollPanel);
            this.tabInput.Location = new System.Drawing.Point(4, 30);
            this.tabInput.Name = "tabInput";
            this.tabInput.Padding = new System.Windows.Forms.Padding(3);
            this.tabInput.Size = new System.Drawing.Size(1351, 506);
            this.tabInput.TabIndex = 0;
            this.tabInput.Text = "Cơ bản";
            this.tabInput.UseVisualStyleBackColor = true;
            // 
            // scrollPanel
            // 
            this.scrollPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scrollPanel.AutoScroll = true;
            this.scrollPanel.BackColor = System.Drawing.Color.White;
            this.scrollPanel.Controls.Add(this.grpConnectionSettings);
            this.scrollPanel.Controls.Add(this.grpAgentConfig);
            this.scrollPanel.Controls.Add(this.grpServerConnection);
            this.scrollPanel.Controls.Add(this.grpFilePaths);
            this.scrollPanel.Controls.Add(this.tableLayoutInput);
            this.scrollPanel.Location = new System.Drawing.Point(3, 3);
            this.scrollPanel.Name = "scrollPanel";
            this.scrollPanel.Padding = new System.Windows.Forms.Padding(20);
            this.scrollPanel.Size = new System.Drawing.Size(1345, 494);
            this.scrollPanel.TabIndex = 0;
            this.scrollPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.scrollPanel_Paint);
            // 
            // grpConnectionSettings
            // 
            this.grpConnectionSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpConnectionSettings.Controls.Add(this.lblHttpTimeout);
            this.grpConnectionSettings.Controls.Add(this.txtHttpTimeout);
            this.grpConnectionSettings.Controls.Add(this.lblDebounce);
            this.grpConnectionSettings.Controls.Add(this.txtDebounce);
            this.grpConnectionSettings.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.grpConnectionSettings.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(55)))), ((int)(((byte)(120)))));
            this.grpConnectionSettings.Location = new System.Drawing.Point(23, 982);
            this.grpConnectionSettings.Name = "grpConnectionSettings";
            this.grpConnectionSettings.Padding = new System.Windows.Forms.Padding(15);
            this.grpConnectionSettings.Size = new System.Drawing.Size(1292, 101);
            this.grpConnectionSettings.TabIndex = 4;
            this.grpConnectionSettings.TabStop = false;
            this.grpConnectionSettings.Text = "Cài đặt Kết nối";
            // 
            // lblHttpTimeout
            // 
            this.lblHttpTimeout.AutoSize = true;
            this.lblHttpTimeout.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblHttpTimeout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblHttpTimeout.Location = new System.Drawing.Point(18, 28);
            this.lblHttpTimeout.Name = "lblHttpTimeout";
            this.lblHttpTimeout.Size = new System.Drawing.Size(199, 21);
            this.lblHttpTimeout.TabIndex = 0;
            this.lblHttpTimeout.Text = "Thời gian chờ HTTP (giây):";
            // 
            // txtHttpTimeout
            // 
            this.txtHttpTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHttpTimeout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txtHttpTimeout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHttpTimeout.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtHttpTimeout.Location = new System.Drawing.Point(18, 48);
            this.txtHttpTimeout.Margin = new System.Windows.Forms.Padding(0, 0, 100, 0);
            this.txtHttpTimeout.Name = "txtHttpTimeout";
            this.txtHttpTimeout.Size = new System.Drawing.Size(575, 29);
            this.txtHttpTimeout.TabIndex = 1;
            // 
            // lblDebounce
            // 
            this.lblDebounce.AutoSize = true;
            this.lblDebounce.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblDebounce.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblDebounce.Location = new System.Drawing.Point(638, 28);
            this.lblDebounce.Name = "lblDebounce";
            this.lblDebounce.Size = new System.Drawing.Size(163, 21);
            this.lblDebounce.TabIndex = 2;
            this.lblDebounce.Text = "Debounce (mili giây):";
            // 
            // txtDebounce
            // 
            this.txtDebounce.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDebounce.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txtDebounce.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDebounce.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtDebounce.Location = new System.Drawing.Point(652, 48);
            this.txtDebounce.Name = "txtDebounce";
            this.txtDebounce.Size = new System.Drawing.Size(594, 29);
            this.txtDebounce.TabIndex = 3;
            // 
            // grpAgentConfig
            // 
            this.grpAgentConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAgentConfig.Controls.Add(this.lblAgentID);
            this.grpAgentConfig.Controls.Add(this.txtAgentID);
            this.grpAgentConfig.Controls.Add(this.lblApiKey);
            this.grpAgentConfig.Controls.Add(this.txtApiKey);
            this.grpAgentConfig.Controls.Add(this.lblVersion);
            this.grpAgentConfig.Controls.Add(this.txtVersion);
            this.grpAgentConfig.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.grpAgentConfig.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(55)))), ((int)(((byte)(120)))));
            this.grpAgentConfig.Location = new System.Drawing.Point(23, 823);
            this.grpAgentConfig.Name = "grpAgentConfig";
            this.grpAgentConfig.Padding = new System.Windows.Forms.Padding(15);
            this.grpAgentConfig.Size = new System.Drawing.Size(1292, 145);
            this.grpAgentConfig.TabIndex = 3;
            this.grpAgentConfig.TabStop = false;
            this.grpAgentConfig.Text = "Cấu hình Agent";
            // 
            // lblAgentID
            // 
            this.lblAgentID.AutoSize = true;
            this.lblAgentID.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblAgentID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblAgentID.Location = new System.Drawing.Point(18, 28);
            this.lblAgentID.Name = "lblAgentID";
            this.lblAgentID.Size = new System.Drawing.Size(79, 21);
            this.lblAgentID.TabIndex = 0;
            this.lblAgentID.Text = "ID Agent:";
            // 
            // txtAgentID
            // 
            this.txtAgentID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAgentID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txtAgentID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAgentID.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtAgentID.Location = new System.Drawing.Point(18, 48);
            this.txtAgentID.Margin = new System.Windows.Forms.Padding(0, 0, 100, 0);
            this.txtAgentID.Name = "txtAgentID";
            this.txtAgentID.Size = new System.Drawing.Size(575, 29);
            this.txtAgentID.TabIndex = 1;
            // 
            // lblApiKey
            // 
            this.lblApiKey.AutoSize = true;
            this.lblApiKey.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblApiKey.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblApiKey.Location = new System.Drawing.Point(638, 28);
            this.lblApiKey.Name = "lblApiKey";
            this.lblApiKey.Size = new System.Drawing.Size(80, 21);
            this.lblApiKey.TabIndex = 2;
            this.lblApiKey.Text = "Khóa API:";
            // 
            // txtApiKey
            // 
            this.txtApiKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtApiKey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txtApiKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtApiKey.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtApiKey.Location = new System.Drawing.Point(652, 48);
            this.txtApiKey.Name = "txtApiKey";
            this.txtApiKey.Size = new System.Drawing.Size(594, 29);
            this.txtApiKey.TabIndex = 3;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblVersion.Location = new System.Drawing.Point(18, 88);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(85, 21);
            this.lblVersion.TabIndex = 4;
            this.lblVersion.Text = "Phiên bản:";
            // 
            // txtVersion
            // 
            this.txtVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVersion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txtVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVersion.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtVersion.Location = new System.Drawing.Point(18, 108);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(1228, 29);
            this.txtVersion.TabIndex = 5;
            // 
            // grpServerConnection
            // 
            this.grpServerConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpServerConnection.Controls.Add(this.lblServerIP);
            this.grpServerConnection.Controls.Add(this.txtServerIP);
            this.grpServerConnection.Controls.Add(this.lblServerPort);
            this.grpServerConnection.Controls.Add(this.txtServerPort);
            this.grpServerConnection.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.grpServerConnection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(55)))), ((int)(((byte)(120)))));
            this.grpServerConnection.Location = new System.Drawing.Point(23, 710);
            this.grpServerConnection.Name = "grpServerConnection";
            this.grpServerConnection.Padding = new System.Windows.Forms.Padding(15);
            this.grpServerConnection.Size = new System.Drawing.Size(1292, 101);
            this.grpServerConnection.TabIndex = 2;
            this.grpServerConnection.TabStop = false;
            this.grpServerConnection.Text = "Kết nối Server";
            // 
            // lblServerIP
            // 
            this.lblServerIP.AutoSize = true;
            this.lblServerIP.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblServerIP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblServerIP.Location = new System.Drawing.Point(18, 28);
            this.lblServerIP.Name = "lblServerIP";
            this.lblServerIP.Size = new System.Drawing.Size(81, 21);
            this.lblServerIP.TabIndex = 0;
            this.lblServerIP.Text = "IP Server:";
            // 
            // txtServerIP
            // 
            this.txtServerIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServerIP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txtServerIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtServerIP.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtServerIP.Location = new System.Drawing.Point(18, 48);
            this.txtServerIP.Margin = new System.Windows.Forms.Padding(0, 0, 100, 0);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(575, 29);
            this.txtServerIP.TabIndex = 1;
            // 
            // lblServerPort
            // 
            this.lblServerPort.AutoSize = true;
            this.lblServerPort.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblServerPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblServerPort.Location = new System.Drawing.Point(638, 28);
            this.lblServerPort.Name = "lblServerPort";
            this.lblServerPort.Size = new System.Drawing.Size(106, 21);
            this.lblServerPort.TabIndex = 2;
            this.lblServerPort.Text = "Cổng Server:";
            // 
            // txtServerPort
            // 
            this.txtServerPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServerPort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txtServerPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtServerPort.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtServerPort.Location = new System.Drawing.Point(652, 48);
            this.txtServerPort.Name = "txtServerPort";
            this.txtServerPort.Size = new System.Drawing.Size(594, 29);
            this.txtServerPort.TabIndex = 3;
            // 
            // grpFilePaths
            // 
            this.grpFilePaths.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpFilePaths.Controls.Add(this.lblProgramDir);
            this.grpFilePaths.Controls.Add(this.txtProgramDir);
            this.grpFilePaths.Controls.Add(this.btnBrowseProgramDir);
            this.grpFilePaths.Controls.Add(this.lblSqlitePath);
            this.grpFilePaths.Controls.Add(this.txtSqlitePath);
            this.grpFilePaths.Controls.Add(this.btnBrowseSqlitePath);
            this.grpFilePaths.Controls.Add(this.lblYaraRulesDir);
            this.grpFilePaths.Controls.Add(this.txtYaraRulesDir);
            this.grpFilePaths.Controls.Add(this.btnBrowseYaraRulesDir);
            this.grpFilePaths.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.grpFilePaths.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(55)))), ((int)(((byte)(120)))));
            this.grpFilePaths.Location = new System.Drawing.Point(23, 495);
            this.grpFilePaths.Name = "grpFilePaths";
            this.grpFilePaths.Padding = new System.Windows.Forms.Padding(15);
            this.grpFilePaths.Size = new System.Drawing.Size(1292, 209);
            this.grpFilePaths.TabIndex = 1;
            this.grpFilePaths.TabStop = false;
            this.grpFilePaths.Text = "Đường dẫn File";
            // 
            // lblProgramDir
            // 
            this.lblProgramDir.AutoSize = true;
            this.lblProgramDir.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblProgramDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblProgramDir.Location = new System.Drawing.Point(18, 136);
            this.lblProgramDir.Name = "lblProgramDir";
            this.lblProgramDir.Size = new System.Drawing.Size(71, 21);
            this.lblProgramDir.TabIndex = 6;
            this.lblProgramDir.Text = "Log File:";
            // 
            // txtProgramDir
            // 
            this.txtProgramDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProgramDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txtProgramDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProgramDir.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtProgramDir.Location = new System.Drawing.Point(18, 156);
            this.txtProgramDir.Name = "txtProgramDir";
            this.txtProgramDir.Size = new System.Drawing.Size(1154, 29);
            this.txtProgramDir.TabIndex = 7;
            // 
            // btnBrowseProgramDir
            // 
            this.btnBrowseProgramDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseProgramDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(55)))), ((int)(((byte)(120)))));
            this.btnBrowseProgramDir.FlatAppearance.BorderSize = 0;
            this.btnBrowseProgramDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseProgramDir.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnBrowseProgramDir.ForeColor = System.Drawing.Color.White;
            this.btnBrowseProgramDir.Location = new System.Drawing.Point(1183, 155);
            this.btnBrowseProgramDir.Name = "btnBrowseProgramDir";
            this.btnBrowseProgramDir.Size = new System.Drawing.Size(80, 30);
            this.btnBrowseProgramDir.TabIndex = 8;
            this.btnBrowseProgramDir.Text = "Chọn";
            this.btnBrowseProgramDir.UseVisualStyleBackColor = false;
            // 
            // lblSqlitePath
            // 
            this.lblSqlitePath.AutoSize = true;
            this.lblSqlitePath.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblSqlitePath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblSqlitePath.Location = new System.Drawing.Point(18, 82);
            this.lblSqlitePath.Name = "lblSqlitePath";
            this.lblSqlitePath.Size = new System.Drawing.Size(75, 21);
            this.lblSqlitePath.TabIndex = 3;
            this.lblSqlitePath.Text = "Hash DB:";
            // 
            // txtSqlitePath
            // 
            this.txtSqlitePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSqlitePath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txtSqlitePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSqlitePath.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtSqlitePath.Location = new System.Drawing.Point(18, 102);
            this.txtSqlitePath.Name = "txtSqlitePath";
            this.txtSqlitePath.Size = new System.Drawing.Size(1154, 29);
            this.txtSqlitePath.TabIndex = 4;
            // 
            // btnBrowseSqlitePath
            // 
            this.btnBrowseSqlitePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseSqlitePath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(55)))), ((int)(((byte)(120)))));
            this.btnBrowseSqlitePath.FlatAppearance.BorderSize = 0;
            this.btnBrowseSqlitePath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseSqlitePath.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnBrowseSqlitePath.ForeColor = System.Drawing.Color.White;
            this.btnBrowseSqlitePath.Location = new System.Drawing.Point(1183, 101);
            this.btnBrowseSqlitePath.Name = "btnBrowseSqlitePath";
            this.btnBrowseSqlitePath.Size = new System.Drawing.Size(80, 30);
            this.btnBrowseSqlitePath.TabIndex = 5;
            this.btnBrowseSqlitePath.Text = "Chọn";
            this.btnBrowseSqlitePath.UseVisualStyleBackColor = false;
            // 
            // lblYaraRulesDir
            // 
            this.lblYaraRulesDir.AutoSize = true;
            this.lblYaraRulesDir.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lblYaraRulesDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblYaraRulesDir.Location = new System.Drawing.Point(18, 28);
            this.lblYaraRulesDir.Name = "lblYaraRulesDir";
            this.lblYaraRulesDir.Size = new System.Drawing.Size(97, 21);
            this.lblYaraRulesDir.TabIndex = 0;
            this.lblYaraRulesDir.Text = "YARA Rules:";
            // 
            // txtYaraRulesDir
            // 
            this.txtYaraRulesDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtYaraRulesDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txtYaraRulesDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtYaraRulesDir.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtYaraRulesDir.Location = new System.Drawing.Point(18, 48);
            this.txtYaraRulesDir.Name = "txtYaraRulesDir";
            this.txtYaraRulesDir.Size = new System.Drawing.Size(1154, 29);
            this.txtYaraRulesDir.TabIndex = 1;
            // 
            // btnBrowseYaraRulesDir
            // 
            this.btnBrowseYaraRulesDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseYaraRulesDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(55)))), ((int)(((byte)(120)))));
            this.btnBrowseYaraRulesDir.FlatAppearance.BorderSize = 0;
            this.btnBrowseYaraRulesDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseYaraRulesDir.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnBrowseYaraRulesDir.ForeColor = System.Drawing.Color.White;
            this.btnBrowseYaraRulesDir.Location = new System.Drawing.Point(1183, 47);
            this.btnBrowseYaraRulesDir.Name = "btnBrowseYaraRulesDir";
            this.btnBrowseYaraRulesDir.Size = new System.Drawing.Size(80, 30);
            this.btnBrowseYaraRulesDir.TabIndex = 2;
            this.btnBrowseYaraRulesDir.Text = "Chọn";
            this.btnBrowseYaraRulesDir.UseVisualStyleBackColor = false;
            // 
            // tableLayoutInput
            // 
            this.tableLayoutInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutInput.AutoSize = true;
            this.tableLayoutInput.ColumnCount = 2;
            this.tableLayoutInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutInput.Controls.Add(this.grpMonitorDirs, 0, 0);
            this.tableLayoutInput.Controls.Add(this.grpIgnoreHash, 1, 0);
            this.tableLayoutInput.Controls.Add(this.grpIgnoreScanYara, 0, 1);
            this.tableLayoutInput.Controls.Add(this.grpIgnoreSendAlert, 1, 1);
            this.tableLayoutInput.Location = new System.Drawing.Point(23, 23);
            this.tableLayoutInput.Name = "tableLayoutInput";
            this.tableLayoutInput.Padding = new System.Windows.Forms.Padding(3);
            this.tableLayoutInput.RowCount = 2;
            this.tableLayoutInput.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutInput.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutInput.Size = new System.Drawing.Size(1292, 452);
            this.tableLayoutInput.TabIndex = 0;
            // 
            // grpMonitorDirs
            // 
            this.grpMonitorDirs.Controls.Add(this.chkInitHashTaken);
            this.grpMonitorDirs.Controls.Add(this.txtInputMonitorDir);
            this.grpMonitorDirs.Controls.Add(this.lstMonitorDirs);
            this.grpMonitorDirs.Controls.Add(this.btnAddMonitorDir);
            this.grpMonitorDirs.Controls.Add(this.btnRemoveMonitorDir);
            this.grpMonitorDirs.Controls.Add(this.btnBrowseMonitorDir);
            this.grpMonitorDirs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpMonitorDirs.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.grpMonitorDirs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(55)))), ((int)(((byte)(120)))));
            this.grpMonitorDirs.Location = new System.Drawing.Point(6, 6);
            this.grpMonitorDirs.Margin = new System.Windows.Forms.Padding(3, 3, 10, 10);
            this.grpMonitorDirs.Name = "grpMonitorDirs";
            this.grpMonitorDirs.Padding = new System.Windows.Forms.Padding(15);
            this.grpMonitorDirs.Size = new System.Drawing.Size(630, 220);
            this.grpMonitorDirs.TabIndex = 9;
            this.grpMonitorDirs.TabStop = false;
            this.grpMonitorDirs.Text = "Thư mục Giám sát";
            this.grpMonitorDirs.Enter += new System.EventHandler(this.grpMonitorDirs_Enter);
            // 
            // chkInitHashTaken
            // 
            this.chkInitHashTaken.AutoSize = true;
            this.chkInitHashTaken.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.chkInitHashTaken.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.chkInitHashTaken.Location = new System.Drawing.Point(18, 188);
            this.chkInitHashTaken.Name = "chkInitHashTaken";
            this.chkInitHashTaken.Size = new System.Drawing.Size(129, 25);
            this.chkInitHashTaken.TabIndex = 5;
            this.chkInitHashTaken.Text = "Khởi tạo Hash";
            this.chkInitHashTaken.UseVisualStyleBackColor = true;
            // 
            // txtInputMonitorDir
            // 
            this.txtInputMonitorDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInputMonitorDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txtInputMonitorDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInputMonitorDir.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtInputMonitorDir.Location = new System.Drawing.Point(18, 28);
            this.txtInputMonitorDir.Name = "txtInputMonitorDir";
            this.txtInputMonitorDir.Size = new System.Drawing.Size(497, 29);
            this.txtInputMonitorDir.TabIndex = 0;
            // 
            // lstMonitorDirs
            // 
            this.lstMonitorDirs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstMonitorDirs.BackColor = System.Drawing.Color.White;
            this.lstMonitorDirs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstMonitorDirs.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lstMonitorDirs.FormattingEnabled = true;
            this.lstMonitorDirs.ItemHeight = 21;
            this.lstMonitorDirs.Location = new System.Drawing.Point(18, 58);
            this.lstMonitorDirs.Name = "lstMonitorDirs";
            this.lstMonitorDirs.Size = new System.Drawing.Size(497, 107);
            this.lstMonitorDirs.TabIndex = 1;
            // 
            // btnAddMonitorDir
            // 
            this.btnAddMonitorDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddMonitorDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnAddMonitorDir.FlatAppearance.BorderSize = 0;
            this.btnAddMonitorDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddMonitorDir.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnAddMonitorDir.ForeColor = System.Drawing.Color.White;
            this.btnAddMonitorDir.Location = new System.Drawing.Point(525, 66);
            this.btnAddMonitorDir.Name = "btnAddMonitorDir";
            this.btnAddMonitorDir.Size = new System.Drawing.Size(80, 30);
            this.btnAddMonitorDir.TabIndex = 2;
            this.btnAddMonitorDir.Text = "Thêm";
            this.btnAddMonitorDir.UseVisualStyleBackColor = false;
            // 
            // btnRemoveMonitorDir
            // 
            this.btnRemoveMonitorDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveMonitorDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnRemoveMonitorDir.FlatAppearance.BorderSize = 0;
            this.btnRemoveMonitorDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveMonitorDir.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnRemoveMonitorDir.ForeColor = System.Drawing.Color.White;
            this.btnRemoveMonitorDir.Location = new System.Drawing.Point(525, 104);
            this.btnRemoveMonitorDir.Name = "btnRemoveMonitorDir";
            this.btnRemoveMonitorDir.Size = new System.Drawing.Size(80, 30);
            this.btnRemoveMonitorDir.TabIndex = 3;
            this.btnRemoveMonitorDir.Text = "Xóa";
            this.btnRemoveMonitorDir.UseVisualStyleBackColor = false;
            // 
            // btnBrowseMonitorDir
            // 
            this.btnBrowseMonitorDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseMonitorDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(55)))), ((int)(((byte)(120)))));
            this.btnBrowseMonitorDir.FlatAppearance.BorderSize = 0;
            this.btnBrowseMonitorDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseMonitorDir.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnBrowseMonitorDir.ForeColor = System.Drawing.Color.White;
            this.btnBrowseMonitorDir.Location = new System.Drawing.Point(525, 28);
            this.btnBrowseMonitorDir.Name = "btnBrowseMonitorDir";
            this.btnBrowseMonitorDir.Size = new System.Drawing.Size(80, 30);
            this.btnBrowseMonitorDir.TabIndex = 4;
            this.btnBrowseMonitorDir.Text = "Chọn";
            this.btnBrowseMonitorDir.UseVisualStyleBackColor = false;
            // 
            // grpIgnoreHash
            // 
            this.grpIgnoreHash.Controls.Add(this.txtInputIgnoreHash);
            this.grpIgnoreHash.Controls.Add(this.lstIgnoreHash);
            this.grpIgnoreHash.Controls.Add(this.btnAddIgnoreHash);
            this.grpIgnoreHash.Controls.Add(this.btnRemoveIgnoreHash);
            this.grpIgnoreHash.Controls.Add(this.btnBrowseIgnoreHash);
            this.grpIgnoreHash.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpIgnoreHash.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.grpIgnoreHash.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(55)))), ((int)(((byte)(120)))));
            this.grpIgnoreHash.Location = new System.Drawing.Point(656, 6);
            this.grpIgnoreHash.Margin = new System.Windows.Forms.Padding(10, 3, 3, 10);
            this.grpIgnoreHash.Name = "grpIgnoreHash";
            this.grpIgnoreHash.Padding = new System.Windows.Forms.Padding(15);
            this.grpIgnoreHash.Size = new System.Drawing.Size(630, 220);
            this.grpIgnoreHash.TabIndex = 10;
            this.grpIgnoreHash.TabStop = false;
            this.grpIgnoreHash.Text = "Đường dẫn Bỏ qua Hash";
            // 
            // txtInputIgnoreHash
            // 
            this.txtInputIgnoreHash.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInputIgnoreHash.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txtInputIgnoreHash.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInputIgnoreHash.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtInputIgnoreHash.Location = new System.Drawing.Point(18, 28);
            this.txtInputIgnoreHash.Name = "txtInputIgnoreHash";
            this.txtInputIgnoreHash.Size = new System.Drawing.Size(492, 29);
            this.txtInputIgnoreHash.TabIndex = 0;
            // 
            // lstIgnoreHash
            // 
            this.lstIgnoreHash.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstIgnoreHash.BackColor = System.Drawing.Color.White;
            this.lstIgnoreHash.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstIgnoreHash.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lstIgnoreHash.FormattingEnabled = true;
            this.lstIgnoreHash.ItemHeight = 21;
            this.lstIgnoreHash.Location = new System.Drawing.Point(18, 58);
            this.lstIgnoreHash.Name = "lstIgnoreHash";
            this.lstIgnoreHash.Size = new System.Drawing.Size(492, 107);
            this.lstIgnoreHash.TabIndex = 1;
            // 
            // btnAddIgnoreHash
            // 
            this.btnAddIgnoreHash.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddIgnoreHash.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnAddIgnoreHash.FlatAppearance.BorderSize = 0;
            this.btnAddIgnoreHash.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddIgnoreHash.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnAddIgnoreHash.ForeColor = System.Drawing.Color.White;
            this.btnAddIgnoreHash.Location = new System.Drawing.Point(519, 67);
            this.btnAddIgnoreHash.Name = "btnAddIgnoreHash";
            this.btnAddIgnoreHash.Size = new System.Drawing.Size(80, 30);
            this.btnAddIgnoreHash.TabIndex = 2;
            this.btnAddIgnoreHash.Text = "Thêm";
            this.btnAddIgnoreHash.UseVisualStyleBackColor = false;
            // 
            // btnRemoveIgnoreHash
            // 
            this.btnRemoveIgnoreHash.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveIgnoreHash.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnRemoveIgnoreHash.FlatAppearance.BorderSize = 0;
            this.btnRemoveIgnoreHash.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveIgnoreHash.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnRemoveIgnoreHash.ForeColor = System.Drawing.Color.White;
            this.btnRemoveIgnoreHash.Location = new System.Drawing.Point(519, 106);
            this.btnRemoveIgnoreHash.Name = "btnRemoveIgnoreHash";
            this.btnRemoveIgnoreHash.Size = new System.Drawing.Size(80, 30);
            this.btnRemoveIgnoreHash.TabIndex = 3;
            this.btnRemoveIgnoreHash.Text = "Xóa";
            this.btnRemoveIgnoreHash.UseVisualStyleBackColor = false;
            // 
            // btnBrowseIgnoreHash
            // 
            this.btnBrowseIgnoreHash.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseIgnoreHash.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(55)))), ((int)(((byte)(120)))));
            this.btnBrowseIgnoreHash.FlatAppearance.BorderSize = 0;
            this.btnBrowseIgnoreHash.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseIgnoreHash.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnBrowseIgnoreHash.ForeColor = System.Drawing.Color.White;
            this.btnBrowseIgnoreHash.Location = new System.Drawing.Point(519, 28);
            this.btnBrowseIgnoreHash.Name = "btnBrowseIgnoreHash";
            this.btnBrowseIgnoreHash.Size = new System.Drawing.Size(80, 30);
            this.btnBrowseIgnoreHash.TabIndex = 4;
            this.btnBrowseIgnoreHash.Text = "Chọn";
            this.btnBrowseIgnoreHash.UseVisualStyleBackColor = false;
            // 
            // grpIgnoreScanYara
            // 
            this.grpIgnoreScanYara.Controls.Add(this.txtInputIgnoreScanYara);
            this.grpIgnoreScanYara.Controls.Add(this.lstIgnoreScanYara);
            this.grpIgnoreScanYara.Controls.Add(this.btnAddIgnoreScanYara);
            this.grpIgnoreScanYara.Controls.Add(this.btnRemoveIgnoreScanYara);
            this.grpIgnoreScanYara.Controls.Add(this.btnBrowseIgnoreScanYara);
            this.grpIgnoreScanYara.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpIgnoreScanYara.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.grpIgnoreScanYara.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(55)))), ((int)(((byte)(120)))));
            this.grpIgnoreScanYara.Location = new System.Drawing.Point(6, 246);
            this.grpIgnoreScanYara.Margin = new System.Windows.Forms.Padding(3, 10, 10, 3);
            this.grpIgnoreScanYara.Name = "grpIgnoreScanYara";
            this.grpIgnoreScanYara.Padding = new System.Windows.Forms.Padding(15);
            this.grpIgnoreScanYara.Size = new System.Drawing.Size(630, 200);
            this.grpIgnoreScanYara.TabIndex = 11;
            this.grpIgnoreScanYara.TabStop = false;
            this.grpIgnoreScanYara.Text = "Bỏ qua Quét YARA";
            // 
            // txtInputIgnoreScanYara
            // 
            this.txtInputIgnoreScanYara.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInputIgnoreScanYara.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txtInputIgnoreScanYara.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInputIgnoreScanYara.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtInputIgnoreScanYara.Location = new System.Drawing.Point(18, 28);
            this.txtInputIgnoreScanYara.Name = "txtInputIgnoreScanYara";
            this.txtInputIgnoreScanYara.Size = new System.Drawing.Size(497, 29);
            this.txtInputIgnoreScanYara.TabIndex = 0;
            // 
            // lstIgnoreScanYara
            // 
            this.lstIgnoreScanYara.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstIgnoreScanYara.BackColor = System.Drawing.Color.White;
            this.lstIgnoreScanYara.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstIgnoreScanYara.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lstIgnoreScanYara.FormattingEnabled = true;
            this.lstIgnoreScanYara.ItemHeight = 21;
            this.lstIgnoreScanYara.Location = new System.Drawing.Point(18, 58);
            this.lstIgnoreScanYara.Name = "lstIgnoreScanYara";
            this.lstIgnoreScanYara.Size = new System.Drawing.Size(497, 107);
            this.lstIgnoreScanYara.TabIndex = 1;
            // 
            // btnAddIgnoreScanYara
            // 
            this.btnAddIgnoreScanYara.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddIgnoreScanYara.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnAddIgnoreScanYara.FlatAppearance.BorderSize = 0;
            this.btnAddIgnoreScanYara.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddIgnoreScanYara.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnAddIgnoreScanYara.ForeColor = System.Drawing.Color.White;
            this.btnAddIgnoreScanYara.Location = new System.Drawing.Point(525, 66);
            this.btnAddIgnoreScanYara.Name = "btnAddIgnoreScanYara";
            this.btnAddIgnoreScanYara.Size = new System.Drawing.Size(80, 30);
            this.btnAddIgnoreScanYara.TabIndex = 2;
            this.btnAddIgnoreScanYara.Text = "Thêm";
            this.btnAddIgnoreScanYara.UseVisualStyleBackColor = false;
            // 
            // btnRemoveIgnoreScanYara
            // 
            this.btnRemoveIgnoreScanYara.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveIgnoreScanYara.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnRemoveIgnoreScanYara.FlatAppearance.BorderSize = 0;
            this.btnRemoveIgnoreScanYara.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveIgnoreScanYara.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnRemoveIgnoreScanYara.ForeColor = System.Drawing.Color.White;
            this.btnRemoveIgnoreScanYara.Location = new System.Drawing.Point(525, 104);
            this.btnRemoveIgnoreScanYara.Name = "btnRemoveIgnoreScanYara";
            this.btnRemoveIgnoreScanYara.Size = new System.Drawing.Size(80, 30);
            this.btnRemoveIgnoreScanYara.TabIndex = 3;
            this.btnRemoveIgnoreScanYara.Text = "Xóa";
            this.btnRemoveIgnoreScanYara.UseVisualStyleBackColor = false;
            // 
            // btnBrowseIgnoreScanYara
            // 
            this.btnBrowseIgnoreScanYara.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseIgnoreScanYara.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(55)))), ((int)(((byte)(120)))));
            this.btnBrowseIgnoreScanYara.FlatAppearance.BorderSize = 0;
            this.btnBrowseIgnoreScanYara.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseIgnoreScanYara.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnBrowseIgnoreScanYara.ForeColor = System.Drawing.Color.White;
            this.btnBrowseIgnoreScanYara.Location = new System.Drawing.Point(525, 28);
            this.btnBrowseIgnoreScanYara.Name = "btnBrowseIgnoreScanYara";
            this.btnBrowseIgnoreScanYara.Size = new System.Drawing.Size(80, 30);
            this.btnBrowseIgnoreScanYara.TabIndex = 4;
            this.btnBrowseIgnoreScanYara.Text = "Chọn";
            this.btnBrowseIgnoreScanYara.UseVisualStyleBackColor = false;
            // 
            // grpIgnoreSendAlert
            // 
            this.grpIgnoreSendAlert.Controls.Add(this.txtInputIgnoreSendAlert);
            this.grpIgnoreSendAlert.Controls.Add(this.lstIgnoreSendAlert);
            this.grpIgnoreSendAlert.Controls.Add(this.btnAddIgnoreSendAlert);
            this.grpIgnoreSendAlert.Controls.Add(this.btnRemoveIgnoreSendAlert);
            this.grpIgnoreSendAlert.Controls.Add(this.btnBrowseIgnoreSendAlert);
            this.grpIgnoreSendAlert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpIgnoreSendAlert.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.grpIgnoreSendAlert.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(55)))), ((int)(((byte)(120)))));
            this.grpIgnoreSendAlert.Location = new System.Drawing.Point(656, 246);
            this.grpIgnoreSendAlert.Margin = new System.Windows.Forms.Padding(10, 10, 3, 3);
            this.grpIgnoreSendAlert.Name = "grpIgnoreSendAlert";
            this.grpIgnoreSendAlert.Padding = new System.Windows.Forms.Padding(15);
            this.grpIgnoreSendAlert.Size = new System.Drawing.Size(630, 200);
            this.grpIgnoreSendAlert.TabIndex = 12;
            this.grpIgnoreSendAlert.TabStop = false;
            this.grpIgnoreSendAlert.Text = "Bỏ qua Gửi Cảnh báo";
            // 
            // txtInputIgnoreSendAlert
            // 
            this.txtInputIgnoreSendAlert.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInputIgnoreSendAlert.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txtInputIgnoreSendAlert.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInputIgnoreSendAlert.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtInputIgnoreSendAlert.Location = new System.Drawing.Point(18, 28);
            this.txtInputIgnoreSendAlert.Name = "txtInputIgnoreSendAlert";
            this.txtInputIgnoreSendAlert.Size = new System.Drawing.Size(492, 29);
            this.txtInputIgnoreSendAlert.TabIndex = 0;
            // 
            // lstIgnoreSendAlert
            // 
            this.lstIgnoreSendAlert.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstIgnoreSendAlert.BackColor = System.Drawing.Color.White;
            this.lstIgnoreSendAlert.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstIgnoreSendAlert.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lstIgnoreSendAlert.FormattingEnabled = true;
            this.lstIgnoreSendAlert.ItemHeight = 21;
            this.lstIgnoreSendAlert.Location = new System.Drawing.Point(18, 58);
            this.lstIgnoreSendAlert.Name = "lstIgnoreSendAlert";
            this.lstIgnoreSendAlert.Size = new System.Drawing.Size(492, 107);
            this.lstIgnoreSendAlert.TabIndex = 1;
            // 
            // btnAddIgnoreSendAlert
            // 
            this.btnAddIgnoreSendAlert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddIgnoreSendAlert.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnAddIgnoreSendAlert.FlatAppearance.BorderSize = 0;
            this.btnAddIgnoreSendAlert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddIgnoreSendAlert.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnAddIgnoreSendAlert.ForeColor = System.Drawing.Color.White;
            this.btnAddIgnoreSendAlert.Location = new System.Drawing.Point(520, 65);
            this.btnAddIgnoreSendAlert.Name = "btnAddIgnoreSendAlert";
            this.btnAddIgnoreSendAlert.Size = new System.Drawing.Size(80, 30);
            this.btnAddIgnoreSendAlert.TabIndex = 2;
            this.btnAddIgnoreSendAlert.Text = "Thêm";
            this.btnAddIgnoreSendAlert.UseVisualStyleBackColor = false;
            // 
            // btnRemoveIgnoreSendAlert
            // 
            this.btnRemoveIgnoreSendAlert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveIgnoreSendAlert.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnRemoveIgnoreSendAlert.FlatAppearance.BorderSize = 0;
            this.btnRemoveIgnoreSendAlert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveIgnoreSendAlert.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnRemoveIgnoreSendAlert.ForeColor = System.Drawing.Color.White;
            this.btnRemoveIgnoreSendAlert.Location = new System.Drawing.Point(520, 102);
            this.btnRemoveIgnoreSendAlert.Name = "btnRemoveIgnoreSendAlert";
            this.btnRemoveIgnoreSendAlert.Size = new System.Drawing.Size(80, 30);
            this.btnRemoveIgnoreSendAlert.TabIndex = 3;
            this.btnRemoveIgnoreSendAlert.Text = "Xóa";
            this.btnRemoveIgnoreSendAlert.UseVisualStyleBackColor = false;
            // 
            // btnBrowseIgnoreSendAlert
            // 
            this.btnBrowseIgnoreSendAlert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseIgnoreSendAlert.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(55)))), ((int)(((byte)(120)))));
            this.btnBrowseIgnoreSendAlert.FlatAppearance.BorderSize = 0;
            this.btnBrowseIgnoreSendAlert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseIgnoreSendAlert.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnBrowseIgnoreSendAlert.ForeColor = System.Drawing.Color.White;
            this.btnBrowseIgnoreSendAlert.Location = new System.Drawing.Point(520, 28);
            this.btnBrowseIgnoreSendAlert.Name = "btnBrowseIgnoreSendAlert";
            this.btnBrowseIgnoreSendAlert.Size = new System.Drawing.Size(80, 30);
            this.btnBrowseIgnoreSendAlert.TabIndex = 4;
            this.btnBrowseIgnoreSendAlert.Text = "Chọn";
            this.btnBrowseIgnoreSendAlert.UseVisualStyleBackColor = false;
            // 
            // tabJson
            // 
            this.tabJson.Controls.Add(this.panelJson);
            this.tabJson.Location = new System.Drawing.Point(4, 30);
            this.tabJson.Name = "tabJson";
            this.tabJson.Padding = new System.Windows.Forms.Padding(3);
            this.tabJson.Size = new System.Drawing.Size(1351, 506);
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
            this.panelJson.Size = new System.Drawing.Size(1345, 500);
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
            this.txtJson.Size = new System.Drawing.Size(1305, 460);
            this.txtJson.TabIndex = 0;
            this.txtJson.WordWrap = false;
            // 
            // FormAgentConfigInput
            // 
            this.ClientSize = new System.Drawing.Size(1359, 628);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.topPanel);
            this.Name = "FormAgentConfigInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cấu hình Agent - Nhập liệu";
            this.topPanel.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabInput.ResumeLayout(false);
            this.scrollPanel.ResumeLayout(false);
            this.scrollPanel.PerformLayout();
            this.grpConnectionSettings.ResumeLayout(false);
            this.grpConnectionSettings.PerformLayout();
            this.grpAgentConfig.ResumeLayout(false);
            this.grpAgentConfig.PerformLayout();
            this.grpServerConnection.ResumeLayout(false);
            this.grpServerConnection.PerformLayout();
            this.grpFilePaths.ResumeLayout(false);
            this.grpFilePaths.PerformLayout();
            this.tableLayoutInput.ResumeLayout(false);
            this.grpMonitorDirs.ResumeLayout(false);
            this.grpMonitorDirs.PerformLayout();
            this.grpIgnoreHash.ResumeLayout(false);
            this.grpIgnoreHash.PerformLayout();
            this.grpIgnoreScanYara.ResumeLayout(false);
            this.grpIgnoreScanYara.PerformLayout();
            this.grpIgnoreSendAlert.ResumeLayout(false);
            this.grpIgnoreSendAlert.PerformLayout();
            this.tabJson.ResumeLayout(false);
            this.panelJson.ResumeLayout(false);
            this.panelJson.PerformLayout();
            this.ResumeLayout(false);

        }

        private Panel scrollPanel;
    }
}


