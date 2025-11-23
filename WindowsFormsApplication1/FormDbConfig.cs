using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.ServiceProcess;

namespace WindowsFormsApplication1
{
    public partial class FormDbConfig : Form
    {
        private string configPath = "";
        private CancellationTokenSource cts;
        private int previousTabIndex = 0;
        private bool isChangingTab = false;

        Color Primary = Color.FromArgb(25, 55, 120);
        Color PrimaryLight = Color.FromArgb(230, 240, 255);
        Color SuccessColor = Color.FromArgb(40, 167, 69);
        Color DangerColor = Color.FromArgb(220, 53, 69);

        public FormDbConfig()
        {
            InitializeComponent();

            // Icon
            if (System.IO.File.Exists(Application.StartupPath + "\\webshell_detector_round2.ico"))
                this.Icon = new Icon(Application.StartupPath + "\\webshell_detector_round2.ico");

            StyleUI();
            AttachEventHandlers();

            // Tự động load file db_config.json nếu tồn tại
            LoadDefaultConfig();
        }

        private void AttachEventHandlers()
        {
            btnChooseConfig.Click += BtnChooseConfig_Click;
            btnLoad.Click += BtnLoad_Click;
            btnSave.Click += BtnSave_Click;
            btnBrowseDatabasePath.Click += (s, e) => BrowseFile(txtDatabasePath, "SQLite Database (*.db)|*.db|All Files (*.*)|*.*");
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
        }

        private void LoadDefaultConfig()
        {
            // Thử load db_config.json trước
            string defaultConfigPath = Path.Combine(Application.StartupPath, "db_config.json");
            if (File.Exists(defaultConfigPath))
            {
                configPath = defaultConfigPath;
                lblPath.Text = "Đường dẫn config: " + configPath;
                LoadConfigFile(defaultConfigPath);
                return;
            }

            // Thử load db_config.dat (file mã hóa)
            string configDatPath = Path.Combine(Application.StartupPath, "db_config.dat");
            if (File.Exists(configDatPath))
            {
                configPath = configDatPath;
                lblPath.Text = "Đường dẫn config: " + configPath;
                LoadConfigFile(configDatPath);
                return;
            }

            // Thử load db_config.template
            string configTemplatePath = Path.Combine(Application.StartupPath, "db_config.template");
            if (File.Exists(configTemplatePath))
            {
                configPath = configTemplatePath;
                lblPath.Text = "Đường dẫn config: " + configPath;
                LoadConfigFile(configTemplatePath);
                return;
            }

            // Thử load db_config.example (tương thích)
            string configExamplePath = Path.Combine(Application.StartupPath, "db_config.example");
            if (File.Exists(configExamplePath))
            {
                configPath = configExamplePath;
                lblPath.Text = "Đường dẫn config: " + configPath;
                LoadConfigFile(configExamplePath);
                return;
            }

            // Tạo config mặc định nếu chưa có
            CreateDefaultConfig();
            lblPath.Text = "Đường dẫn config: " + configPath;
        }

        private void CreateDefaultConfig()
        {
            // Tạo config mặc định
            var defaultConfig = new
            {
                database_path = "",
                restart_service = ""
            };

            string defaultJson = Newtonsoft.Json.JsonConvert.SerializeObject(defaultConfig, Newtonsoft.Json.Formatting.Indented);
            configPath = Path.Combine(Application.StartupPath, "db_config.json");
            txtJson.Text = defaultJson;
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

                // Loại bỏ BOM nếu có
                if (jsonContent.Length > 0 && jsonContent[0] == '\uFEFF')
                {
                    jsonContent = jsonContent.Substring(1);
                }

                jsonContent = jsonContent.Trim();

                // Thử giải mã nếu là base64 (chỉ thử nếu có vẻ như base64)
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
            }
            catch (Exception ex)
            {
                ShowCustomMessageBox("Lỗi đọc file: " + ex.Message, "Lỗi", MessageBoxIcon.Error);
            }
        }

        private void LoadConfigToForm(JObject config)
        {
            txtDatabasePath.Text = config["database_path"]?.ToString() ?? "";
            txtRestartService.Text = config["restart_service"]?.ToString() ?? "";
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

            // Update fields from form
            if (!string.IsNullOrEmpty(txtDatabasePath.Text))
                config["database_path"] = txtDatabasePath.Text;
            if (!string.IsNullOrEmpty(txtRestartService.Text))
                config["restart_service"] = txtRestartService.Text;

            return config;
        }

        private void BrowseFile(TextBox textBox, string filter)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = filter;
                ofd.CheckFileExists = false;
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
                return;
            }
            else if (currentTabIndex == 0) // Input tab
            {
                // Sync from JSON to form
                try
                {
                    JObject config = JObject.Parse(txtJson.Text);
                    LoadConfigToForm(config);
                }
                catch
                {
                    // Ignore JSON parse errors when switching tabs
                }
            }

            // Cập nhật previousTabIndex sau khi xử lý xong
            if (tabControl.SelectedIndex == currentTabIndex)
            {
                previousTabIndex = currentTabIndex;
            }
        }

        private string Version()
        {
            // Chỉ dùng key suffix như Python code, không dùng hostname
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

                // Sử dụng BouncyCastle AES-GCM mode (giống Python pycryptodome)
                AesEngine aesEngine = new AesEngine();
                GcmBlockCipher gcm = new GcmBlockCipher(aesEngine);
                KeyParameter keyParam = new KeyParameter(keyBytes);
                AeadParameters parameters = new AeadParameters(keyParam, 128, nonce); // 128-bit tag

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

                if (fullCipher.Length < 28) // Tối thiểu: 12 (nonce) + 0 (ciphertext) + 16 (tag)
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
                AeadParameters parameters = new AeadParameters(keyParam, 128, nonce); // 128-bit tag

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
            StyleTextBox();
        }

        private void StyleButtons()
        {
            StyleButtonOutline(btnChooseConfig, Primary);
            StyleButtonOutline(btnLoad, Color.FromArgb(100, 100, 100));
            StyleButton(btnSave, Primary, Color.White);
            //StyleButton(btnTestConnection, SuccessColor, Color.White);
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

            // Hover effect
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

            // Hover effect
            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(250, 250, 250);
            };
            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = Color.White;
            };
        }

        private void StyleTextBox()
        {
            txtJson.BackColor = Color.White;
            txtJson.ForeColor = Color.FromArgb(30, 30, 30);
            txtJson.BorderStyle = BorderStyle.FixedSingle;
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
                MessageBox.Show("Chưa chọn file config!", "Lỗi");
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

            ShowCustomMessageBox("Đã lưu cấu hình database thành công!", "Thành công", MessageBoxIcon.Information);
        }

        private void BtnTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                JObject root = JObject.Parse(txtJson.Text);
                string dbPath = root["database_path"]?.ToString();

                if (string.IsNullOrEmpty(dbPath))
                {
                    MessageBox.Show("Chưa cấu hình đường dẫn database!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!File.Exists(dbPath))
                {
                    MessageBox.Show($"File database không tồn tại:\n{dbPath}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Test connection
                using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection($"Data Source={dbPath};Version=3;"))
                {
                    conn.Open();
                    conn.Close();
                }

                MessageBox.Show("Kết nối database thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối database: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private DialogResult ShowCustomMessageBoxWithCancel(string message, string title, MessageBoxIcon iconType = MessageBoxIcon.Information)
        {
            return FormCustomMessageBox.ShowWithCancel(this, message, title, iconType);
        }
    }
}

