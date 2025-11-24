using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace WindowsFormsApplication1
{
    public partial class FormAgentConfigInput : Form
    {
        private string configPath = "";
        private int previousTabIndex = 0;
        private bool isChangingTab = false; // Flag để tránh vòng lặp khi quay lại tab
        private bool previousInitHashTaken = false; // Lưu giá trị init_hash_taken trước đó

        Color Primary = Color.FromArgb(25, 55, 120);
        Color PrimaryLight = Color.FromArgb(230, 240, 255);
        Color SuccessColor = Color.FromArgb(40, 167, 69);
        Color DangerColor = Color.FromArgb(220, 53, 69);

        public FormAgentConfigInput()
        {
            InitializeComponent();

            // Icon
            if (System.IO.File.Exists(Application.StartupPath + "\\webshell_detector_round2.ico"))
                this.Icon = new Icon(Application.StartupPath + "\\webshell_detector_round2.ico");

            StyleUI();
            AttachEventHandlers();

            // Tự động load file agent_config.json nếu tồn tại
            LoadDefaultConfig();

            // Cập nhật trạng thái nút dựa trên service status
            UpdateServiceButtonStatus();
        }

        private void AttachEventHandlers()
        {
            btnChooseConfig.Click += BtnChooseConfig_Click;
            btnLoad.Click += BtnLoad_Click;
            btnSave.Click += BtnSave_Click;
            btnRunBat.Click += BtnRunBat_Click;

            // Single path browse buttons
            btnBrowseYaraRulesDir.Click += (s, e) => BrowseFolder(txtYaraRulesDir);
            btnBrowseSqlitePath.Click += (s, e) => BrowseFile(txtSqlitePath, "SQLite Database (*.db)|*.db|All Files (*.*)|*.*");
            btnBrowseProgramDir.Click += (s, e) => BrowseFolder(txtProgramDir);

            // Array path buttons - Monitor Dirs
            btnBrowseMonitorDir.Click += (s, e) => BrowseAndAddToList(txtInputMonitorDir, lstMonitorDirs);
            btnAddMonitorDir.Click += (s, e) => AddPathFromTextBox(txtInputMonitorDir, lstMonitorDirs);
            btnRemoveMonitorDir.Click += (s, e) => RemoveSelectedFromList(lstMonitorDirs);

            // Array path buttons - Ignore Hash
            btnBrowseIgnoreHash.Click += (s, e) => BrowseAndAddToList(txtInputIgnoreHash, lstIgnoreHash);
            btnAddIgnoreHash.Click += (s, e) => AddPathFromTextBox(txtInputIgnoreHash, lstIgnoreHash);
            btnRemoveIgnoreHash.Click += (s, e) => RemoveSelectedFromList(lstIgnoreHash);

            // Array path buttons - Ignore Scan YARA
            btnBrowseIgnoreScanYara.Click += (s, e) => BrowseAndAddToList(txtInputIgnoreScanYara, lstIgnoreScanYara);
            btnAddIgnoreScanYara.Click += (s, e) => AddPathFromTextBox(txtInputIgnoreScanYara, lstIgnoreScanYara);
            btnRemoveIgnoreScanYara.Click += (s, e) => RemoveSelectedFromList(lstIgnoreScanYara);

            // Array path buttons - Ignore Send Alert
            btnBrowseIgnoreSendAlert.Click += (s, e) => BrowseAndAddToList(txtInputIgnoreSendAlert, lstIgnoreSendAlert);
            btnAddIgnoreSendAlert.Click += (s, e) => AddPathFromTextBox(txtInputIgnoreSendAlert, lstIgnoreSendAlert);
            btnRemoveIgnoreSendAlert.Click += (s, e) => RemoveSelectedFromList(lstIgnoreSendAlert);

            // Sync between tabs
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;

            // Fix TextBox sizing to prevent overlap in Server sections
            grpServerConnection.Resize += GrpServerConnection_Resize;
            grpAgentConfig.Resize += GrpAgentConfig_Resize;
            grpConnectionSettings.Resize += GrpConnectionSettings_Resize;

            // Also handle form resize
            this.Resize += (s, e) => AdjustServerTextBoxes();
            this.Load += (s, e) => AdjustServerTextBoxes();
            this.Shown += (s, e) => AdjustServerTextBoxes();
        }

        private void GrpServerConnection_Resize(object sender, EventArgs e)
        {
            AdjustServerTextBoxes();
        }

        private void GrpAgentConfig_Resize(object sender, EventArgs e)
        {
            AdjustServerTextBoxes();
        }

        private void GrpConnectionSettings_Resize(object sender, EventArgs e)
        {
            AdjustServerTextBoxes();
        }

        private void AdjustServerTextBoxes()
        {
            // Server Connection GroupBox
            if (grpServerConnection != null && grpServerConnection.Width > 0)
            {
                int availableWidth = grpServerConnection.Width - 36 - 100; // 36 = padding (18*2), 100 = margin between
                int halfWidth = availableWidth / 2;

                if (txtServerIP != null && txtServerPort != null)
                {
                    txtServerIP.Size = new Size(halfWidth, txtServerIP.Height);
                    txtServerPort.Location = new Point(18 + halfWidth + 100, txtServerPort.Location.Y);
                    txtServerPort.Size = new Size(grpServerConnection.Width - (18 + halfWidth + 100) - 18, txtServerPort.Height);
                    lblServerPort.Location = new Point(18 + halfWidth + 100, lblServerPort.Location.Y);
                }
            }

            // Agent Config GroupBox
            if (grpAgentConfig != null && grpAgentConfig.Width > 0)
            {
                int availableWidth = grpAgentConfig.Width - 36 - 100;
                int halfWidth = availableWidth / 2;

                if (txtAgentID != null && txtApiKey != null)
                {
                    txtAgentID.Size = new Size(halfWidth, txtAgentID.Height);
                    txtApiKey.Location = new Point(18 + halfWidth + 100, txtApiKey.Location.Y);
                    txtApiKey.Size = new Size(grpAgentConfig.Width - (18 + halfWidth + 100) - 18, txtApiKey.Height);
                    lblApiKey.Location = new Point(18 + halfWidth + 100, lblApiKey.Location.Y);
                }

                // txtVersion (Tag) - full width, responsive
                if (txtVersion != null)
                {
                    txtVersion.Size = new Size(grpAgentConfig.Width - 36, txtVersion.Height);
                }
            }

            // Connection Settings GroupBox
            if (grpConnectionSettings != null && grpConnectionSettings.Width > 0)
            {
                int availableWidth = grpConnectionSettings.Width - 36 - 100;
                int halfWidth = availableWidth / 2;

                if (txtHttpTimeout != null && txtDebounce != null)
                {
                    txtHttpTimeout.Size = new Size(halfWidth, txtHttpTimeout.Height);
                    txtDebounce.Location = new Point(18 + halfWidth + 100, txtDebounce.Location.Y);
                    txtDebounce.Size = new Size(grpConnectionSettings.Width - (18 + halfWidth + 100) - 18, txtDebounce.Height);
                    lblDebounce.Location = new Point(18 + halfWidth + 100, lblDebounce.Location.Y);
                }
            }
        }

        private void BrowseFolder(TextBox textBox)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Chọn thư mục";
                fbd.ShowNewFolderButton = true;
                if (!string.IsNullOrEmpty(textBox.Text) && Directory.Exists(textBox.Text))
                {
                    fbd.SelectedPath = textBox.Text;
                }
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = fbd.SelectedPath;
                }
            }
        }

        private void BrowseFile(TextBox textBox, string filter)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = filter;
                ofd.CheckFileExists = false; // Allow creating new file
                if (!string.IsNullOrEmpty(textBox.Text))
                {
                    string dir = Path.GetDirectoryName(textBox.Text);
                    if (Directory.Exists(dir))
                    {
                        ofd.InitialDirectory = dir;
                    }
                    ofd.FileName = Path.GetFileName(textBox.Text);
                }
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = ofd.FileName;
                }
            }
        }

        private void BrowseAndAddToList(TextBox textBox, ListBox listBox)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Chọn thư mục để thêm vào danh sách";
                fbd.ShowNewFolderButton = true;

                // Nếu TextBox có giá trị và là thư mục hợp lệ, mở dialog tại đó
                if (!string.IsNullOrEmpty(textBox.Text) && Directory.Exists(textBox.Text))
                {
                    fbd.SelectedPath = textBox.Text;
                }

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string path = fbd.SelectedPath;

                    // Cập nhật TextBox với đường dẫn đã chọn
                    textBox.Text = path;

                    // Thêm vào danh sách nếu chưa có
                    if (!listBox.Items.Cast<string>().Contains(path))
                    {
                        listBox.Items.Add(path);
                    }
                    else
                    {
                        ShowCustomMessageBox("Đường dẫn này đã có trong danh sách!", "Thông báo", MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void AddPathFromTextBox(TextBox textBox, ListBox listBox)
        {
            string input = textBox.Text.Trim();
            if (!string.IsNullOrEmpty(input))
            {
                if (!listBox.Items.Cast<string>().Contains(input))
                {
                    listBox.Items.Add(input);
                    textBox.Clear();
                }
                else
                {
                    ShowCustomMessageBox("Đường dẫn này đã có trong danh sách!", "Thông báo", MessageBoxIcon.Information);
                }
            }
        }

        private void AddPathToList(ListBox listBox)
        {
            using (Form inputForm = new Form())
            {
                inputForm.Text = "Thêm đường dẫn";
                inputForm.Size = new Size(500, 120);
                inputForm.StartPosition = FormStartPosition.CenterParent;
                inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                inputForm.MaximizeBox = false;
                inputForm.MinimizeBox = false;

                Label lbl = new Label() { Text = "Nhập đường dẫn:", Left = 10, Top = 10, Width = 460 };
                TextBox txt = new TextBox() { Left = 10, Top = 35, Width = 460 };
                Button btnOk = new Button() { Text = "OK", Left = 300, Top = 70, Width = 80, DialogResult = DialogResult.OK };
                Button btnCancel = new Button() { Text = "Hủy", Left = 390, Top = 70, Width = 80, DialogResult = DialogResult.Cancel };

                inputForm.Controls.Add(lbl);
                inputForm.Controls.Add(txt);
                inputForm.Controls.Add(btnOk);
                inputForm.Controls.Add(btnCancel);
                inputForm.AcceptButton = btnOk;
                inputForm.CancelButton = btnCancel;

                if (inputForm.ShowDialog(this) == DialogResult.OK)
                {
                    string input = txt.Text.Trim();
                    if (!string.IsNullOrEmpty(input))
                    {
                        if (!listBox.Items.Cast<string>().Contains(input))
                        {
                            listBox.Items.Add(input);
                        }
                        else
                        {
                            ShowCustomMessageBox("Đường dẫn này đã có trong danh sách!", "Thông báo", MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        private void RemoveSelectedFromList(ListBox listBox)
        {
            if (listBox.SelectedIndex >= 0)
            {
                listBox.Items.RemoveAt(listBox.SelectedIndex);
            }
            else
            {
                ShowCustomMessageBox("Vui lòng chọn một mục để xóa!", "Thông báo", MessageBoxIcon.Information);
            }
        }

        private void LoadDefaultConfig()
        {
            // Kiểm tra và copy config.dat từ template nếu chưa có (ưu tiên làm việc này trước)
            string configDatPath = Path.Combine(Application.StartupPath, "config.dat");
            string configTemplatePath = Path.Combine(Application.StartupPath, "config.template");
            
            // Nếu chưa có config.dat và có config.template, copy từ template
            if (!File.Exists(configDatPath) && File.Exists(configTemplatePath))
            {
                try
                {
                    // Copy file template thành config.dat
                    File.Copy(configTemplatePath, configDatPath, false);
                    System.Diagnostics.Debug.WriteLine($"Đã copy {configTemplatePath} thành {configDatPath}");
                }
                catch (UnauthorizedAccessException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Không có quyền copy file: {ex.Message}");
                }
                catch (IOException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi I/O khi copy file: {ex.Message}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi khi copy config.template: {ex.Message}");
                }
            }

            // Thử load agent_config.json trước
            string defaultConfigPath = Path.Combine(Application.StartupPath, "agent_config.json");
            if (File.Exists(defaultConfigPath))
            {
                configPath = defaultConfigPath;
                lblPath.Text = "Đường dẫn config: " + configPath;
                LoadConfigFile(defaultConfigPath);
                return;
            }

            // Load config.dat nếu đã tồn tại hoặc vừa được tạo
            if (File.Exists(configDatPath))
            {
                configPath = configDatPath;
                lblPath.Text = "Đường dẫn config: " + configPath;
                LoadConfigFile(configDatPath);
                return;
            }

            // Thử load agent_config.template
            string agentConfigTemplatePath = Path.Combine(Application.StartupPath, "agent_config.template");
            if (File.Exists(agentConfigTemplatePath))
            {
                configPath = agentConfigTemplatePath;
                lblPath.Text = "Đường dẫn config: " + configPath;
                LoadConfigFile(agentConfigTemplatePath);
                return;
            }

            // Thử load agent_config.example hoặc config.example (tương thích)
            string configExamplePath = Path.Combine(Application.StartupPath, "agent_config.example");
            if (File.Exists(configExamplePath))
            {
                configPath = configExamplePath;
                lblPath.Text = "Đường dẫn config: " + configPath;
                LoadConfigFile(configExamplePath);
                return;
            }

            configExamplePath = Path.Combine(Application.StartupPath, "config.example");
            if (File.Exists(configExamplePath))
            {
                configPath = configExamplePath;
                lblPath.Text = "Đường dẫn config: " + configPath;
                LoadConfigFile(configExamplePath);
                return;
            }

            lblPath.Text = "Đường dẫn config: (Chưa chọn file)";
        }

        private void LoadConfigFile(string filePath)
        {
            try
            {
                string jsonContent = "";
                JObject config = null;

                // File .template và .example luôn là file text, không cần giải mã
                if (filePath.EndsWith(".template", StringComparison.OrdinalIgnoreCase) || 
                    filePath.EndsWith(".example", StringComparison.OrdinalIgnoreCase))
                {
                    jsonContent = File.ReadAllText(filePath, Encoding.UTF8);

                    // Loại bỏ BOM nếu có
                    if (jsonContent.Length > 0 && jsonContent[0] == '\uFEFF')
                    {
                        jsonContent = jsonContent.Substring(1);
                    }

                    jsonContent = jsonContent.Trim();

                    // Parse JSON và load vào form
                    config = JObject.Parse(jsonContent);
                    LoadConfigToForm(config);

                    // Cập nhật JSON editor
                    txtJson.Text = FormatJson(jsonContent);

                    // Cập nhật trạng thái nút service
                    UpdateServiceButtonStatus();
                    return;
                }

                // Đọc file dưới dạng binary để xử lý cả file .dat (binary) và .json (text)
                byte[] fileBytes = File.ReadAllBytes(filePath);

                // Kiểm tra xem có phải file mã hóa không (có ít nhất 28 bytes: 12 nonce + data + 16 tag)
                if (fileBytes.Length >= 28)
                {
                    try
                    {
                        // Thử giải mã như file mã hóa
                        string base64Content = Convert.ToBase64String(fileBytes);
                        jsonContent = DecryptConfig(base64Content);
                    }
                    catch
                    {
                        // Không phải file mã hóa, xử lý như text
                        jsonContent = Encoding.UTF8.GetString(fileBytes);
                    }
                }
                else
                {
                    // Xử lý như file text (JSON)
                    jsonContent = Encoding.UTF8.GetString(fileBytes);
                }

                // Thử giải mã nếu là base64
                if (!string.IsNullOrEmpty(jsonContent) && jsonContent.Length > 50 &&
                    !jsonContent.TrimStart().StartsWith("{"))
                {
                    try
                    {
                        jsonContent = DecryptConfig(jsonContent);
                    }
                    catch
                    {
                        // File chưa được mã hóa, giữ nguyên
                    }
                }

                // Parse JSON và load vào form
                config = JObject.Parse(jsonContent);
                LoadConfigToForm(config);

                // Cập nhật JSON editor
                txtJson.Text = FormatJson(jsonContent);

                // Cập nhật trạng thái nút service
                UpdateServiceButtonStatus();
            }
            catch (Exception ex)
            {
                ShowCustomMessageBox("Lỗi đọc file: " + ex.Message, "Lỗi", MessageBoxIcon.Error);
            }
        }

        private void LoadConfigToForm(JObject config)
        {
            // Single paths
            txtYaraRulesDir.Text = config["yara_rules_dir"]?.ToString() ?? "";
            txtSqlitePath.Text = config["sqlite_path"]?.ToString() ?? "";
            txtProgramDir.Text = config["program_dir"]?.ToString() ?? "";

            // Array paths
            LoadArrayToList(config["monitor_dirs"] as JArray, lstMonitorDirs);
            LoadArrayToList(config["ignore_hash"] as JArray, lstIgnoreHash);
            LoadArrayToList(config["ignore_scan_yara"] as JArray, lstIgnoreScanYara);
            LoadArrayToList(config["ignore_send_alert"] as JArray, lstIgnoreSendAlert);

            // Init hash taken
            chkInitHashTaken.Checked = config["init_hash_taken"]?.ToObject<bool>() ?? false;
            previousInitHashTaken = chkInitHashTaken.Checked; // Lưu giá trị ban đầu

            // Server configuration
            txtServerIP.Text = config["server_ip"]?.ToString() ?? "127.0.0.1";
            txtServerPort.Text = config["server_port"]?.ToString() ?? "8000";
            txtAgentID.Text = config["agent_id"]?.ToString() ?? "";
            txtApiKey.Text = config["api_key"]?.ToString() ?? "";
            txtVersion.Text = config["tag"]?.ToString() ?? "";
            txtHttpTimeout.Text = config["http_timeout_sec"]?.ToString() ?? "15";
            txtDebounce.Text = config["debounce_ms"]?.ToString() ?? "350";
        }

        private void LoadArrayToList(JArray array, ListBox listBox)
        {
            listBox.Items.Clear();
            if (array != null)
            {
                foreach (var item in array)
                {
                    listBox.Items.Add(item.ToString());
                }
            }
        }

        private JObject GetConfigFromForm()
        {
            // Load existing config if available
            JObject config = new JObject();
            try
            {
                if (!string.IsNullOrEmpty(configPath) && File.Exists(configPath))
                {
                    byte[] fileBytes = File.ReadAllBytes(configPath);
                    string jsonContent = "";

                    if (fileBytes.Length >= 28)
                    {
                        try
                        {
                            string base64Content = Convert.ToBase64String(fileBytes);
                            jsonContent = DecryptConfig(base64Content);
                        }
                        catch
                        {
                            jsonContent = Encoding.UTF8.GetString(fileBytes);
                        }
                    }
                    else
                    {
                        jsonContent = Encoding.UTF8.GetString(fileBytes);
                    }

                    if (!string.IsNullOrEmpty(jsonContent) && jsonContent.Length > 50 &&
                        !jsonContent.TrimStart().StartsWith("{"))
                    {
                        try
                        {
                            jsonContent = DecryptConfig(jsonContent);
                        }
                        catch { }
                    }

                    config = JObject.Parse(jsonContent);
                }
            }
            catch { }

            // Update path fields
            if (!string.IsNullOrEmpty(txtYaraRulesDir.Text))
                config["yara_rules_dir"] = txtYaraRulesDir.Text;
            if (!string.IsNullOrEmpty(txtSqlitePath.Text))
                config["sqlite_path"] = txtSqlitePath.Text;
            if (!string.IsNullOrEmpty(txtProgramDir.Text))
                config["program_dir"] = txtProgramDir.Text;

            // Update array fields
            config["monitor_dirs"] = new JArray(lstMonitorDirs.Items.Cast<string>().ToArray());
            config["ignore_hash"] = new JArray(lstIgnoreHash.Items.Cast<string>().ToArray());
            config["ignore_scan_yara"] = new JArray(lstIgnoreScanYara.Items.Cast<string>().ToArray());
            config["ignore_send_alert"] = new JArray(lstIgnoreSendAlert.Items.Cast<string>().ToArray());

            // Update init hash taken
            config["init_hash_taken"] = chkInitHashTaken.Checked;

            // Update server configuration
            config["server_ip"] = txtServerIP.Text;
            int port;
            if (int.TryParse(txtServerPort.Text, out port))
                config["server_port"] = port;
            config["agent_id"] = txtAgentID.Text;
            config["api_key"] = txtApiKey.Text;
            if (!string.IsNullOrEmpty(txtVersion.Text))
                config["tag"] = txtVersion.Text;
            int timeout;
            if (int.TryParse(txtHttpTimeout.Text, out timeout))
                config["http_timeout_sec"] = timeout;
            int debounce;
            if (int.TryParse(txtDebounce.Text, out debounce))
                config["debounce_ms"] = debounce;

            return config;
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Bỏ qua nếu đang trong quá trình thay đổi tab (tránh vòng lặp)
            if (isChangingTab)
            {
                return;
            }

            // Lưu lại tab index hiện tại trước khi xử lý
            int currentTabIndex = tabControl.SelectedIndex;

            // Lưu lại tab index trước đó (chỉ khi không phải tab 1, để dùng khi quay lại)
            // Nếu đang chuyển sang tab 1, previousTabIndex đã được lưu từ lần trước
            if (currentTabIndex != 1)
            {
                previousTabIndex = currentTabIndex;
            }

            if (currentTabIndex == 1) // JSON tab (Nâng cao)
            {
                // Ngay lập tức quay lại tab trước để chặn việc chuyển tab
                isChangingTab = true;
                tabControl.SelectedIndex = previousTabIndex;
                isChangingTab = false;

                // Hiển thị cảnh báo khi truy cập tab Nâng cao với cả nút OK và Hủy
                DialogResult result = ShowCustomMessageBoxWithCancel(
                    "Cấu hình nâng cao chỉ phù hợp cho người đã được tập huấn cách cấu hình.\n\n" +
                    "Việc chỉnh sửa trực tiếp JSON có thể gây lỗi hệ thống nếu không hiểu rõ cấu trúc.\n\n" +
                    "Bạn có chắc chắn muốn tiếp tục?",
                    "Cảnh báo - Cấu hình Nâng cao",
                    MessageBoxIcon.Warning
                );

                // Chỉ khi người dùng chọn OK mới chuyển sang tab Nâng cao
                if (result == DialogResult.OK)
                {
                    // Đặt flag để tránh vòng lặp
                    isChangingTab = true;
                    tabControl.SelectedIndex = 1;
                    isChangingTab = false;

                    // Sync from form to JSON
                    try
                    {
                        JObject config = GetConfigFromForm();
                        txtJson.Text = FormatJson(config.ToString());
                    }
                    catch (Exception ex)
                    {
                        ShowCustomMessageBox("Lỗi khi chuyển đổi sang JSON: " + ex.Message, "Lỗi", MessageBoxIcon.Error);
                    }
                }
                // Nếu người dùng chọn Hủy, không làm gì (đã quay lại tab trước rồi)
                return;
            }
            else if (currentTabIndex == 0) // Input tab
            {
                // Sync from JSON to form
                try
                {
                    JObject config = JObject.Parse(txtJson.Text);
                    LoadConfigToForm(config);
                    // Cập nhật previousInitHashTaken sau khi load từ JSON
                    previousInitHashTaken = chkInitHashTaken.Checked;
                }
                catch
                {
                    // Ignore JSON parse errors when switching tabs
                }
            }

            // Cập nhật previousTabIndex sau khi xử lý xong
            // Chỉ cập nhật nếu tab đã được chấp nhận (không bị quay lại)
            if (tabControl.SelectedIndex == currentTabIndex)
            {
                previousTabIndex = currentTabIndex;
            }
        }

        private void BtnChooseConfig_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            f.Filter = "Config Files (*.json;*.dat;*.template;*.example)|*.json;*.dat;*.template;*.example|All Files (*.*)|*.*";

            if (f.ShowDialog() == DialogResult.OK)
            {
                configPath = f.FileName;
                lblPath.Text = "Đường dẫn config: " + configPath;
                LoadConfigFile(configPath);
            }
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            if (!File.Exists(configPath))
            {
                ShowCustomMessageBox("Chưa chọn file config!", "Lỗi", MessageBoxIcon.Error);
                return;
            }

            LoadConfigFile(configPath);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(configPath))
            {
                ShowCustomMessageBox("Chưa chọn file config!", "Lỗi", MessageBoxIcon.Error);
                return;
            }

            // Get config from current tab
            JObject config;
            if (tabControl.SelectedIndex == 0) // Input tab
            {
                config = GetConfigFromForm();
            }
            else // JSON tab (tab 1)
            {
                try
                {
                    config = JObject.Parse(txtJson.Text);
                }
                catch (Exception ex)
                {
                    ShowCustomMessageBox("JSON không hợp lệ: " + ex.Message, "Lỗi", MessageBoxIcon.Error);
                    return;
                }
            }

            // Kiểm tra nếu init_hash_taken chuyển từ true sang false
            bool currentInitHashTaken = config["init_hash_taken"]?.ToObject<bool>() ?? false;
            if (previousInitHashTaken && !currentInitHashTaken)
            {
                // Yêu cầu người dùng lưu database trước
                if (!SaveDatabaseBeforeDisablingInitHash())
                {
                    // Người dùng hủy, không lưu config
                    return;
                }
            }

            string jsonText = config.ToString(Newtonsoft.Json.Formatting.Indented);

            // File .template và .example luôn lưu dưới dạng text thuần, không mã hóa
            if (configPath.EndsWith(".template", StringComparison.OrdinalIgnoreCase) || 
                configPath.EndsWith(".example", StringComparison.OrdinalIgnoreCase))
            {
                File.WriteAllText(configPath, jsonText, Encoding.UTF8);
            }
            else
            {
                // Mã hóa nội dung trước khi lưu cho các file khác
                string encryptedContent = EncryptConfig(jsonText);

                // Nếu file là .dat, lưu dưới dạng binary
                if (configPath.EndsWith(".dat", StringComparison.OrdinalIgnoreCase))
                {
                    byte[] encryptedBytes = Convert.FromBase64String(encryptedContent);
                    File.WriteAllBytes(configPath, encryptedBytes);
                }
                else
                {
                    // Lưu dưới dạng base64 string cho file .json
                    File.WriteAllText(configPath, encryptedContent);
                }
            }

            // Update both tabs
            txtJson.Text = FormatJson(jsonText);
            LoadConfigToForm(config);

            // Cập nhật giá trị previousInitHashTaken sau khi lưu thành công
            previousInitHashTaken = currentInitHashTaken;

            // Cập nhật trạng thái nút service
            UpdateServiceButtonStatus();

            // Restart service nếu có cấu hình trong db_config.json và service đang chạy
            string serviceName = GetServiceNameFromDbConfig();
            if (!string.IsNullOrWhiteSpace(serviceName))
            {
                try
                {
                    // Kiểm tra service status trước khi restart
                    bool isServiceRunning = false;
                    using (ServiceController service = new ServiceController(serviceName))
                    {
                        try
                        {
                            service.Refresh();
                            isServiceRunning = (service.Status == ServiceControllerStatus.Running);
                        }
                        catch
                        {
                            // Service không tồn tại hoặc không thể truy cập
                            isServiceRunning = false;
                        }
                    }

                    if (isServiceRunning)
                    {
                        // Chỉ restart nếu service đang chạy
                        RestartService(serviceName);
                        ShowCustomMessageBox($"Đã lưu cấu hình thành công!\nĐã restart service: {serviceName}", "Thành công", MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Service không chạy, không cần restart
                        ShowCustomMessageBox("Đã lưu cấu hình thành công!", "Thành công", MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    ShowCustomMessageBox($"Đã lưu cấu hình thành công!\nLỗi khi kiểm tra/restart service: {ex.Message}", "Cảnh báo", MessageBoxIcon.Warning);
                }
            }
            else
            {
                // Debug: Kiểm tra xem có file db_config không
                string dbConfigJson = Path.Combine(Application.StartupPath, "db_config.json");
                string dbConfigDat = Path.Combine(Application.StartupPath, "db_config.dat");
                string debugMsg = "Đã lưu cấu hình thành công!";
                if (!File.Exists(dbConfigJson) && !File.Exists(dbConfigDat))
                {
                    debugMsg += "\n(Không tìm thấy db_config.json hoặc db_config.dat)";
                }
                else
                {
                    debugMsg += "\n(Không có field 'restart_service' trong db_config hoặc service name rỗng)";
                }
                ShowCustomMessageBox(debugMsg, "Thành công", MessageBoxIcon.Information);
            }
        }

        private void BtnRunBat_Click(object sender, EventArgs e)
        {
            try
            {
                string serviceName = GetServiceNameFromDbConfig();
                bool isServiceRunning = false;

                // Kiểm tra service status nếu có tên service
                if (!string.IsNullOrWhiteSpace(serviceName))
                {
                    try
                    {
                        using (ServiceController service = new ServiceController(serviceName))
                        {
                            service.Refresh();
                            isServiceRunning = (service.Status == ServiceControllerStatus.Running);
                        }
                    }
                    catch
                    {
                        // Service không tồn tại hoặc không thể truy cập
                        isServiceRunning = false;
                    }
                }

                string currentDirectory = Application.StartupPath;
                string batFile = null;

                if (isServiceRunning)
                {
                    // Service đang chạy -> chạy uninstall.bat để tắt service
                    batFile = Path.Combine(currentDirectory, "uninstall.bat");
                    if (!File.Exists(batFile))
                    {
                        ShowCustomMessageBox($"Không tìm thấy file uninstall.bat trong thư mục:\n{currentDirectory}", "Lỗi", MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    // Service chưa chạy -> tìm install.bat hoặc file .bat khác để cài
                    string installBat = Path.Combine(currentDirectory, "install.bat");
                    if (File.Exists(installBat))
                    {
                        batFile = installBat;
                    }
                    else
                    {
                        // Tìm file .bat đầu tiên (không phải uninstall.bat)
                        string[] batFiles = Directory.GetFiles(currentDirectory, "*.bat", SearchOption.TopDirectoryOnly)
                            .Where(f => !Path.GetFileName(f).Equals("uninstall.bat", StringComparison.OrdinalIgnoreCase))
                            .ToArray();

                        if (batFiles.Length == 0)
                        {
                            ShowCustomMessageBox($"Không tìm thấy file .bat để cài đặt trong thư mục:\n{currentDirectory}", "Thông báo", MessageBoxIcon.Information);
                            return;
                        }

                        if (batFiles.Length == 1)
                        {
                            batFile = batFiles[0];
                        }
                        else
                        {
                            // Hiển thị dialog để chọn file
                            using (Form selectForm = new Form())
                            {
                                selectForm.Text = "Chọn file .bat để cài đặt";
                                selectForm.Size = new Size(500, 350);
                                selectForm.StartPosition = FormStartPosition.CenterParent;
                                selectForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                                selectForm.MaximizeBox = false;
                                selectForm.MinimizeBox = false;

                                Label lbl = new Label()
                                {
                                    Text = "Chọn file .bat:",
                                    Location = new Point(15, 15),
                                    Size = new Size(460, 25),
                                    Font = new Font("Segoe UI", 10F)
                                };

                                ListBox lstBats = new ListBox()
                                {
                                    Location = new Point(15, 45),
                                    Size = new Size(460, 220),
                                    Font = new Font("Segoe UI", 9.5F)
                                };

                                foreach (string bat in batFiles)
                                {
                                    lstBats.Items.Add(Path.GetFileName(bat));
                                }

                                lstBats.SelectedIndex = 0;

                                Button btnOk = new Button()
                                {
                                    Text = "Chạy",
                                    Location = new Point(300, 275),
                                    Size = new Size(80, 35),
                                    DialogResult = DialogResult.OK,
                                    Font = new Font("Segoe UI Semibold", 9.5F)
                                };

                                Button btnCancel = new Button()
                                {
                                    Text = "Hủy",
                                    Location = new Point(390, 275),
                                    Size = new Size(80, 35),
                                    DialogResult = DialogResult.Cancel,
                                    Font = new Font("Segoe UI Semibold", 9.5F)
                                };

                                selectForm.Controls.Add(lbl);
                                selectForm.Controls.Add(lstBats);
                                selectForm.Controls.Add(btnOk);
                                selectForm.Controls.Add(btnCancel);
                                selectForm.AcceptButton = btnOk;
                                selectForm.CancelButton = btnCancel;

                                if (selectForm.ShowDialog(this) == DialogResult.OK && lstBats.SelectedIndex >= 0)
                                {
                                    string fileName = lstBats.SelectedItem.ToString();
                                    batFile = Path.Combine(currentDirectory, fileName);
                                }
                                else
                                {
                                    return; // User cancelled
                                }
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(batFile) || !File.Exists(batFile))
                {
                    ShowCustomMessageBox("File .bat không tồn tại!", "Lỗi", MessageBoxIcon.Error);
                    return;
                }

                // Chạy file .bat
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = batFile,
                    WorkingDirectory = currentDirectory,
                    UseShellExecute = true,
                    CreateNoWindow = false
                };

                Process.Start(psi);

                string action = isServiceRunning ? "tắt service" : "cài đặt agent";
                ShowCustomMessageBox($"Đã chạy file để {action}:\n{Path.GetFileName(batFile)}", "Thành công", MessageBoxIcon.Information);

                // Cập nhật lại trạng thái nút sau khi chạy
                System.Threading.Thread.Sleep(1000); // Đợi một chút để service status cập nhật
                UpdateServiceButtonStatus();
            }
            catch (Exception ex)
            {
                ShowCustomMessageBox($"Lỗi khi chạy file .bat:\n{ex.Message}", "Lỗi", MessageBoxIcon.Error);
            }
        }

        private void UpdateServiceButtonStatus()
        {
            try
            {
                string serviceName = GetServiceNameFromDbConfig();
                bool isServiceRunning = false;

                if (!string.IsNullOrWhiteSpace(serviceName))
                {
                    try
                    {
                        // Kiểm tra xem service có tồn tại không
                        ServiceController[] services = ServiceController.GetServices();
                        bool serviceExists = services.Any(s => s.ServiceName.Equals(serviceName, StringComparison.OrdinalIgnoreCase));
                        
                        if (!serviceExists)
                        {
                            System.Diagnostics.Debug.WriteLine($"Service '{serviceName}' không tồn tại trong danh sách services");
                            isServiceRunning = false;
                        }
                        else
                        {
                            using (ServiceController service = new ServiceController(serviceName))
                            {
                                service.Refresh();
                                isServiceRunning = (service.Status == ServiceControllerStatus.Running);
                                System.Diagnostics.Debug.WriteLine($"Service '{serviceName}' status: {service.Status}");
                            }
                        }
                    }
                    catch (InvalidOperationException ex)
                    {
                        // Service không tồn tại hoặc không thể truy cập
                        // Có thể cần quyền Administrator
                        System.Diagnostics.Debug.WriteLine($"InvalidOperationException khi check service '{serviceName}': {ex.Message}");
                        isServiceRunning = false;
                    }
                    catch (System.ServiceProcess.TimeoutException ex)
                    {
                        // Timeout khi check service
                        System.Diagnostics.Debug.WriteLine($"TimeoutException khi check service '{serviceName}': {ex.Message}");
                        isServiceRunning = false;
                    }
                    catch (System.ComponentModel.Win32Exception ex)
                    {
                        // Lỗi Windows API - có thể do quyền truy cập
                        System.Diagnostics.Debug.WriteLine($"Win32Exception khi check service '{serviceName}': {ex.Message} (Error code: {ex.NativeErrorCode})");
                        isServiceRunning = false;
                    }
                    catch (Exception ex)
                    {
                        // Lỗi khác
                        System.Diagnostics.Debug.WriteLine($"Exception khi check service '{serviceName}': {ex.GetType().Name} - {ex.Message}");
                        isServiceRunning = false;
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Service name là null hoặc rỗng, không thể check status");
                }

                // Cập nhật text và màu nút
                if (isServiceRunning)
                {
                    btnRunBat.Text = "⏹ Tắt Service";
                    btnRunBat.BackColor = DangerColor;
                }
                else
                {
                    btnRunBat.Text = "▶ Chạy Agent";
                    btnRunBat.BackColor = SuccessColor;
                }
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, mặc định hiển thị "Chạy Agent"
                System.Diagnostics.Debug.WriteLine($"Lỗi trong UpdateServiceButtonStatus: {ex.GetType().Name} - {ex.Message}\nStack trace: {ex.StackTrace}");
                btnRunBat.Text = "▶ Chạy Agent";
                btnRunBat.BackColor = SuccessColor;
            }
        }

        private string Version()
        {
            return "wsdetector V1.1.4 ";
        }

        private string EncryptConfig(string plainText)
        {
            try
            {
                // Key derivation giống Python: SHA256(key_suffix + mac_part)
                string keySuffix = Version();
                byte[] keyBytes = MacAddressHelper.GetConfigKey(keySuffix);

                byte[] plaintextBytes = Encoding.UTF8.GetBytes(plainText);

                // Generate 12-byte nonce (như Python get_random_bytes(12))
                byte[] nonce = new byte[12];
                using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(nonce);
                }

                AesEngine aesEngine = new AesEngine();
                GcmBlockCipher gcm = new GcmBlockCipher(aesEngine);
                KeyParameter keyParam = new KeyParameter(keyBytes);
                AeadParameters parameters = new AeadParameters(keyParam, 128, nonce);

                gcm.Init(true, parameters);

                byte[] ciphertext = new byte[gcm.GetOutputSize(plaintextBytes.Length)];
                int len = gcm.ProcessBytes(plaintextBytes, 0, plaintextBytes.Length, ciphertext, 0);
                gcm.DoFinal(ciphertext, len);

                // Extract tag (16 bytes) từ cuối ciphertext
                byte[] tag = new byte[16];
                byte[] encryptedData = new byte[ciphertext.Length - 16];
                Array.Copy(ciphertext, 0, encryptedData, 0, encryptedData.Length);
                Array.Copy(ciphertext, encryptedData.Length, tag, 0, 16);

                // Format giống Python: nonce (12 bytes) + ciphertext + tag (16 bytes)
                byte[] result = new byte[12 + encryptedData.Length + 16];
                Array.Copy(nonce, 0, result, 0, 12);
                Array.Copy(encryptedData, 0, result, 12, encryptedData.Length);
                Array.Copy(tag, 0, result, 12 + encryptedData.Length, 16);

                return Convert.ToBase64String(result);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi mã hóa: " + ex.Message);
            }
        }

        private string DecryptConfig(string cipherText)
        {
            try
            {
                // Key derivation giống Python: SHA256(key_suffix + mac_part)
                string keySuffix = Version();
                byte[] keyBytes = MacAddressHelper.GetConfigKey(keySuffix);

                byte[] fullCipher = Convert.FromBase64String(cipherText);

                if (fullCipher.Length < 28)
                {
                    throw new Exception("Dữ liệu mã hóa không hợp lệ");
                }

                // Format: nonce (12 bytes) + ciphertext + tag (16 bytes)
                byte[] nonce = new byte[12];
                Array.Copy(fullCipher, 0, nonce, 0, 12);

                byte[] tag = new byte[16];
                Array.Copy(fullCipher, fullCipher.Length - 16, tag, 0, 16);

                byte[] ciphertext = new byte[fullCipher.Length - 12 - 16];
                Array.Copy(fullCipher, 12, ciphertext, 0, ciphertext.Length);

                // Combine ciphertext + tag cho GCM
                byte[] ciphertextWithTag = new byte[ciphertext.Length + 16];
                Array.Copy(ciphertext, 0, ciphertextWithTag, 0, ciphertext.Length);
                Array.Copy(tag, 0, ciphertextWithTag, ciphertext.Length, 16);

                AesEngine aesEngine = new AesEngine();
                GcmBlockCipher gcm = new GcmBlockCipher(aesEngine);
                KeyParameter keyParam = new KeyParameter(keyBytes);
                AeadParameters parameters = new AeadParameters(keyParam, 128, nonce);

                gcm.Init(false, parameters);

                byte[] decryptedData = new byte[gcm.GetOutputSize(ciphertextWithTag.Length)];
                int len = gcm.ProcessBytes(ciphertextWithTag, 0, ciphertextWithTag.Length, decryptedData, 0);
                int finalLen = gcm.DoFinal(decryptedData, len);

                // Chỉ lấy dữ liệu thực tế (loại bỏ null bytes ở cuối)
                int totalLen = len + finalLen;
                if (totalLen < decryptedData.Length)
                {
                    byte[] actualData = new byte[totalLen];
                    Array.Copy(decryptedData, 0, actualData, 0, totalLen);
                    decryptedData = actualData;
                }

                return Encoding.UTF8.GetString(decryptedData);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi giải mã: " + ex.Message);
            }
        }

        private void StyleUI()
        {
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10);

            StyleButtons();
        }

        private void StyleButtons()
        {
            StyleButtonOutline(btnChooseConfig, Primary);
            StyleButtonOutline(btnLoad, Color.FromArgb(100, 100, 100));
            StyleButton(btnSave, Primary, Color.White);
            StyleButton(btnRunBat, SuccessColor, Color.White);
        }

        private void StyleButton(Button btn, Color back, Color text)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = back;
            btn.ForeColor = text;
            btn.Font = new Font("Segoe UI Semibold", 9.5f);
            btn.Cursor = Cursors.Hand;
            btn.TextAlign = ContentAlignment.MiddleCenter;

            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(
                    Math.Min(255, back.R + 10),
                    Math.Min(255, back.G + 10),
                    Math.Min(255, back.B + 10)
                );
            };
            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = back;
            };
        }

        private void StyleButtonOutline(Button btn, Color borderColor)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = borderColor;
            btn.BackColor = Color.White;
            btn.ForeColor = borderColor;
            btn.Font = new Font("Segoe UI Semibold", 9.5f);
            btn.Cursor = Cursors.Hand;
            btn.TextAlign = ContentAlignment.MiddleCenter;

            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(250, 250, 250);
            };
            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = Color.White;
            };
        }

        private string FormatJson(string json)
        {
            try
            {
                return JToken.Parse(json).ToString(Newtonsoft.Json.Formatting.Indented);
            }
            catch
            {
                return json;
            }
        }

        private DialogResult ShowCustomMessageBox(string message, string title, MessageBoxIcon iconType = MessageBoxIcon.Information)
        {
            return FormCustomMessageBox.Show(this, message, title, iconType);
        }

        private Color GetIconBackColor(MessageBoxIcon iconType)
        {
            switch (iconType)
            {
                case MessageBoxIcon.Information:
                    return Color.FromArgb(59, 130, 246); // Beautiful blue
                case MessageBoxIcon.Warning:
                    return Color.FromArgb(245, 158, 11); // Amber
                case MessageBoxIcon.Error:
                    return Color.FromArgb(239, 68, 68); // Red
                case MessageBoxIcon.Question:
                    return Color.FromArgb(34, 197, 94); // Green
                default:
                    return Color.FromArgb(59, 130, 246);
            }
        }

        private DialogResult ShowCustomMessageBoxWithCancel(string message, string title, MessageBoxIcon iconType = MessageBoxIcon.Information)
        {
            return FormCustomMessageBox.ShowWithCancel(this, message, title, iconType);
        }


        private string GetServiceNameFromDbConfig()
        {
            // Fix cứng tên service là wsdetector
            return "wsdetector";
        }

        private string DecryptDbConfig(string cipherText)
        {
            try
            {
                // Key derivation giống như trong DbConfigHelper
                string keySuffix = Version();
                byte[] keyMaterial = Encoding.UTF8.GetBytes(keySuffix);
                byte[] keyBytes;
                using (SHA256 sha256 = SHA256.Create())
                {
                    keyBytes = sha256.ComputeHash(keyMaterial);
                }

                byte[] fullCipher = Convert.FromBase64String(cipherText);

                if (fullCipher.Length < 28)
                {
                    throw new Exception("Dữ liệu mã hóa không hợp lệ");
                }

                byte[] nonce = new byte[12];
                Array.Copy(fullCipher, 0, nonce, 0, 12);

                byte[] tag = new byte[16];
                Array.Copy(fullCipher, fullCipher.Length - 16, tag, 0, 16);

                byte[] ciphertext = new byte[fullCipher.Length - 12 - 16];
                Array.Copy(fullCipher, 12, ciphertext, 0, ciphertext.Length);

                byte[] ciphertextWithTag = new byte[ciphertext.Length + 16];
                Array.Copy(ciphertext, 0, ciphertextWithTag, 0, ciphertext.Length);
                Array.Copy(tag, 0, ciphertextWithTag, ciphertext.Length, 16);

                AesEngine aesEngine = new AesEngine();
                GcmBlockCipher gcm = new GcmBlockCipher(aesEngine);
                KeyParameter keyParam = new KeyParameter(keyBytes);
                AeadParameters parameters = new AeadParameters(keyParam, 128, nonce);

                gcm.Init(false, parameters);

                byte[] decryptedData = new byte[gcm.GetOutputSize(ciphertextWithTag.Length)];
                int len = gcm.ProcessBytes(ciphertextWithTag, 0, ciphertextWithTag.Length, decryptedData, 0);
                gcm.DoFinal(decryptedData, len);

                return Encoding.UTF8.GetString(decryptedData);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi giải mã: " + ex.Message);
            }
        }

        private void RestartService(string serviceName)
        {
            try
            {
                using (ServiceController service = new ServiceController(serviceName))
                {
                    // Kiểm tra service có tồn tại không
                    try
                    {
                        string displayName = service.DisplayName;
                    }
                    catch
                    {
                        throw new Exception($"Service '{serviceName}' không tồn tại!");
                    }

                    // Refresh status
                    service.Refresh();

                    // Nếu service đang chạy, dừng nó
                    if (service.Status == ServiceControllerStatus.Running)
                    {
                        service.Stop();
                        service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                    }
                    // Nếu service đang dừng, đợi nó dừng hoàn toàn
                    else if (service.Status == ServiceControllerStatus.StopPending)
                    {
                        service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                    }

                    // Khởi động lại service
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception($"Không thể truy cập service '{serviceName}'. Cần quyền Administrator! Chi tiết: {ex.Message}");
            }
            catch (System.ServiceProcess.TimeoutException)
            {
                throw new Exception($"Timeout khi restart service '{serviceName}'. Service có thể đang bận.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi restart service '{serviceName}': {ex.Message}");
            }
        }

        private void grpMonitorDirs_Enter(object sender, EventArgs e)
        {

        }

        private void scrollPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private bool SaveDatabaseBeforeDisablingInitHash()
        {
            try
            {
                string sourceDbPath = txtSqlitePath.Text.Trim();
                
                // Kiểm tra xem có đường dẫn database không
                if (string.IsNullOrEmpty(sourceDbPath))
                {
                    ShowCustomMessageBox("Chưa có đường dẫn Hash DB! Vui lòng cấu hình đường dẫn Hash DB trước.", "Cảnh báo", MessageBoxIcon.Warning);
                    return false;
                }

                // Kiểm tra file database có tồn tại không
                if (!File.Exists(sourceDbPath))
                {
                    ShowCustomMessageBox($"File database không tồn tại:\n{sourceDbPath}\n\nVui lòng kiểm tra lại đường dẫn Hash DB.", "Lỗi", MessageBoxIcon.Error);
                    return false;
                }

                // Hiển thị dialog để chọn nơi lưu database
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "SQLite Database (*.db)|*.db|All Files (*.*)|*.*";
                    sfd.Title = "Lưu file Hash DB trước khi tắt Init Hash Taken";
                    sfd.FileName = Path.GetFileNameWithoutExtension(sourceDbPath) + "_backup_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".db";
                    
                    // Đặt thư mục mặc định là thư mục chứa file database gốc hoặc thư mục hiện tại
                    if (!string.IsNullOrEmpty(Path.GetDirectoryName(sourceDbPath)))
                    {
                        sfd.InitialDirectory = Path.GetDirectoryName(sourceDbPath);
                    }
                    else
                    {
                        sfd.InitialDirectory = Application.StartupPath;
                    }

                    if (sfd.ShowDialog(this) == DialogResult.OK)
                    {
                        string targetDbPath = sfd.FileName;

                        try
                        {
                            // Copy file database
                            File.Copy(sourceDbPath, targetDbPath, true);
                            ShowCustomMessageBox($"Đã lưu file Hash DB thành công!\n\nĐường dẫn: {targetDbPath}", "Thành công", MessageBoxIcon.Information);
                            return true;
                        }
                        catch (Exception ex)
                        {
                            ShowCustomMessageBox($"Lỗi khi lưu file Hash DB:\n{ex.Message}", "Lỗi", MessageBoxIcon.Error);
                            return false;
                        }
                    }
                    else
                    {
                        // Người dùng hủy, hỏi lại xem có muốn tiếp tục không
                        DialogResult result = ShowCustomMessageBoxWithCancel(
                            "Bạn đã hủy việc lưu file Hash DB.\n\n" +
                            "Nếu tiếp tục lưu cấu hình (tắt Init Hash Taken), dữ liệu hash có thể bị mất.\n\n" +
                            "Bạn có muốn tiếp tục lưu cấu hình không?",
                            "Xác nhận",
                            MessageBoxIcon.Warning
                        );

                        return (result == DialogResult.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowCustomMessageBox($"Lỗi khi xử lý lưu database: {ex.Message}", "Lỗi", MessageBoxIcon.Error);
                return false;
            }
        }


    }
}

