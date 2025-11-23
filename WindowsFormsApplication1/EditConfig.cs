using System;
using System.IO;
using System.Linq;
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
    public partial class EditConfig : Form
    {
        private string configPath = "";
        private CancellationTokenSource cts;

        Color Primary = Color.FromArgb(25, 55, 120);
        Color PrimaryLight = Color.FromArgb(230, 240, 255);
        Color SuccessColor = Color.FromArgb(40, 167, 69);
        Color DangerColor = Color.FromArgb(220, 53, 69);

        public EditConfig()
        {
            InitializeComponent();

            // Icon
            if (System.IO.File.Exists(Application.StartupPath + "\\webshell_detector_round2.ico"))
                this.Icon = new Icon(Application.StartupPath + "\\webshell_detector_round2.ico");

            StyleUI();

            btnChooseConfig.Click += BtnChooseConfig_Click;
            btnLoad.Click += BtnLoad_Click;
            btnSave.Click += BtnSave_Click;

            // btnStop.Click += (s, e) =>
            // {
            // cts?.Cancel();
            // lblScanStatus.Text = "Đã dừng quét!";
            // }//;

            // Tự động load file agent_config.json nếu tồn tại
            LoadDefaultConfig();
        }

        private void LoadDefaultConfig()
        {
            // Thử load agent_config.json trước
            string defaultConfigPath = Path.Combine(Application.StartupPath, "agent_config.json");
            if (File.Exists(defaultConfigPath))
            {
                configPath = defaultConfigPath;
                lblPath.Text = "Đường dẫn config: " + configPath;
                LoadConfigFile(defaultConfigPath);
                return;
            }

            // Thử load config.dat (file mã hóa)
            string configDatPath = Path.Combine(Application.StartupPath, "config.dat");
            if (File.Exists(configDatPath))
            {
                configPath = configDatPath;
                lblPath.Text = "Đường dẫn config: " + configPath;
                LoadConfigFile(configDatPath);
                return;
            }

            lblPath.Text = "Đường dẫn config: (Chưa chọn file)";
        }

        private void LoadConfigFile(string filePath)
        {
            try
            {
                // File .template và .example luôn là file text, không cần giải mã
                if (filePath.EndsWith(".template", StringComparison.OrdinalIgnoreCase) || 
                    filePath.EndsWith(".example", StringComparison.OrdinalIgnoreCase))
                {
                    string jsonContent = File.ReadAllText(filePath, Encoding.UTF8);
                    
                    // Loại bỏ BOM nếu có
                    if (jsonContent.Length > 0 && jsonContent[0] == '\uFEFF')
                    {
                        jsonContent = jsonContent.Substring(1);
                    }

                    jsonContent = jsonContent.Trim();
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
                        string content = DecryptConfig(base64Content);
                        txtJson.Text = FormatJson(content);
                        return;
                    }
                    catch
                    {
                        // Không phải file mã hóa, xử lý như text
                    }
                }

                // Xử lý như file text (JSON)
                string textContent = Encoding.UTF8.GetString(fileBytes);
                // Thử giải mã nếu là base64
                try
                {
                    textContent = DecryptConfig(textContent);
                }
                catch
                {
                    // File chưa được mã hóa, giữ nguyên
                }
                txtJson.Text = FormatJson(textContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đọc file: " + ex.Message, "Lỗi");
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
            StyleDataGridView();
            StyleTextBox();
            StyleProgressBar();
        }

        private void StyleButtons()
        {
            StyleButtonOutline(btnChooseConfig, Primary);
            StyleButtonOutline(btnLoad, Color.FromArgb(100, 100, 100));
            StyleButton(btnSave, Primary, Color.White);
            //StyleButton(btnStop, DangerColor, Color.White);
        }

        private void StyleButton(Button btn, Color back, Color text)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = back;
            btn.ForeColor = text;
            btn.Font = new Font("Segoe UI Semibold", 9.5f);
            btn.Cursor = Cursors.Hand;
            btn.TextAlign = ContentAlignment.MiddleCenter; // Căn giữa text

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
            btn.TextAlign = ContentAlignment.MiddleCenter; // Căn giữa text

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

        private void StyleDataGridView()
        {
            // dgvScanResult.BackgroundColor = Color.FromArgb(248, 249, 250);
            // dgvScanResult.BorderStyle = BorderStyle.None;
            // dgvScanResult.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            // dgvScanResult.EnableHeadersVisualStyles = false;

            // // Header
            // dgvScanResult.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            // dgvScanResult.ColumnHeadersDefaultCellStyle.BackColor = Primary;
            // dgvScanResult.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            // dgvScanResult.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5f);
            // dgvScanResult.ColumnHeadersDefaultCellStyle.Padding = new Padding(12, 10, 12, 10);
            // dgvScanResult.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            // dgvScanResult.ColumnHeadersHeight = 42;

            // // Cells
            // dgvScanResult.DefaultCellStyle.Font = new Font("Segoe UI", 10f);
            // dgvScanResult.DefaultCellStyle.Padding = new Padding(12, 8, 12, 8);
            // dgvScanResult.DefaultCellStyle.BackColor = Color.White;
            // dgvScanResult.DefaultCellStyle.ForeColor = Color.FromArgb(50, 50, 50);
            // dgvScanResult.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 240, 255);
            // dgvScanResult.DefaultCellStyle.SelectionForeColor = Color.Black;

            // // Alternating rows
            // dgvScanResult.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(252, 253, 254);
            // dgvScanResult.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(50, 50, 50);

            // // Grid lines
            // dgvScanResult.GridColor = Color.FromArgb(240, 242, 245);
            // dgvScanResult.RowTemplate.Height = 38;

            // // Selection
            // dgvScanResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // dgvScanResult.MultiSelect = false;
        }

        private void StyleTextBox()
        {
            txtJson.BackColor = Color.White;
            txtJson.ForeColor = Color.FromArgb(30, 30, 30);
            txtJson.BorderStyle = BorderStyle.FixedSingle;
        }

        private void StyleProgressBar()
        {
            progressScan.Style = ProgressBarStyle.Continuous;
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

        private bool IsSuspicious(string file, JArray arr)
        {
            string ext = Path.GetExtension(file).ToLower();
            return arr.Select(x => x.ToString().ToLower()).Contains(ext);
        }

        private string ScanFile(string file, JArray suspiciousExt)
        {
            return IsSuspicious(file, suspiciousExt) ? "Nghi Ngờ" : "An Toàn";
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(configPath))
            {
                MessageBox.Show("Chưa chọn file config!", "Lỗi");
                return;
            }

            // Mã hóa nội dung trước khi lưu
            string encryptedContent = EncryptConfig(txtJson.Text);

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
            

            JObject root;
            try
            {
                root = JObject.Parse(txtJson.Text);
            }
            catch
            {
                MessageBox.Show("JSON lỗi!", "Lỗi");
                return;
            }

            var dirs = root["monitor_dirs"]?.Select(x => x.ToString()).ToList();
            var suspiciousExt = (JArray)root["suspicious_extensions"];

            //dgvScanResult.Rows.Clear();
            lblScanStatus.Text = "Đang quét...";
            progressScan.Value = 0;

            cts = new CancellationTokenSource();
            var token = cts.Token;

            int total = dirs.Sum(dir =>
                Directory.Exists(dir) ?
                Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories).Length : 0);

            if (total == 0)
            {
                MessageBox.Show("Không có file nào để quét!");
                return;
            }

            progressScan.Maximum = total;

            int scanned = 0;

            try
            {
                foreach (var dir in dirs)
                {
                    if (!Directory.Exists(dir)) continue;

                    var files = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);

                    foreach (var file in files)
                    {
                        if (token.IsCancellationRequested)
                        {
                            lblScanStatus.Text = "Đã dừng quét!";
                            return;
                        }

                        scanned++;
                        progressScan.Value = scanned;

                        string status = ScanFile(file, suspiciousExt);

                        //int row = dgvScanResult.Rows.Add(
                        //Path.GetFileName(file),
                        //file,
                        //status
                        // );

                        if (status == "Nghi Ngờ")
                        {
                            //dgvScanResult.Rows[row].DefaultCellStyle.ForeColor = DangerColor;
                            // dgvScanResult.Rows[row].DefaultCellStyle.BackColor = Color.FromArgb(255, 245, 245);
                        }
                        else
                        {
                            //dgvScanResult.Rows[row].DefaultCellStyle.ForeColor = SuccessColor;
                            //dgvScanResult.Rows[row].DefaultCellStyle.BackColor = Color.FromArgb(245, 255, 245);
                        }

                        lblScanStatus.Text = $"({scanned}/{total}) {file}";
                        await Task.Delay(1);
                    }
                }

                lblScanStatus.Text = $"Hoàn tất! {scanned} file.";

                // Restart service nếu có cấu hình trong db_config.json
                string serviceName = GetServiceNameFromDbConfig();
                if (!string.IsNullOrWhiteSpace(serviceName))
                {
                    try
                    {
                        lblScanStatus.Text = $"Đang restart service: {serviceName}...";
                        RestartService(serviceName);
                        lblScanStatus.Text = $"Hoàn tất! {scanned} file. Đã restart service: {serviceName}";
                        MessageBox.Show($"Quét hoàn tất!\nĐã restart service: {serviceName}", "OK");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Quét hoàn tất!\nLỗi restart service: {ex.Message}", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        lblScanStatus.Text = $"Hoàn tất! {scanned} file. Lỗi restart service: {ex.Message}";
                    }
                }
                else
                {
                    MessageBox.Show("Quét hoàn tất!", "OK");
                }

            }
            catch (OperationCanceledException)
            {
                lblScanStatus.Text = "Đã dừng!";
            }
        }

        private string GetServiceNameFromDbConfig()
        {
            try
            {
                // Thử load db_config.json trước
                string configPath = Path.Combine(Application.StartupPath, "db_config.json");
                if (!File.Exists(configPath))
                {
                    // Thử load db_config.dat
                    configPath = Path.Combine(Application.StartupPath, "db_config.dat");
                    if (!File.Exists(configPath))
                    {
                        return null; // Không tìm thấy config
                    }
                }

                // Đọc file
                byte[] fileBytes = File.ReadAllBytes(configPath);

                string textContent = "";

                // Kiểm tra xem có phải file mã hóa không
                if (fileBytes.Length >= 28)
                {
                    try
                    {
                        // Thử giải mã như file mã hóa
                        string base64Content = Convert.ToBase64String(fileBytes);
                        textContent = DecryptDbConfig(base64Content);
                    }
                    catch
                    {
                        // Không phải file mã hóa, xử lý như text
                        textContent = Encoding.UTF8.GetString(fileBytes);
                    }
                }
                else
                {
                    // Xử lý như file text (JSON)
                    textContent = Encoding.UTF8.GetString(fileBytes);
                }

                // Loại bỏ BOM nếu có
                if (textContent.Length > 0 && textContent[0] == '\uFEFF')
                {
                    textContent = textContent.Substring(1);
                }

                textContent = textContent.Trim();

                // Thử giải mã nếu là base64 (chỉ thử nếu có vẻ như base64)
                if (!string.IsNullOrEmpty(textContent) && textContent.Length > 50 &&
                    !textContent.TrimStart().StartsWith("{"))
                {
                    try
                    {
                        textContent = DecryptDbConfig(textContent);
                    }
                    catch
                    {
                        // File chưa được mã hóa, giữ nguyên
                    }
                }

                // Parse JSON
                JObject config = JObject.Parse(textContent);
                string serviceName = config["restart_service"]?.ToString();

                return string.IsNullOrWhiteSpace(serviceName) ? null : serviceName;
            }
            catch
            {
                return null; // Lỗi đọc config, trả về null
            }
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

        private void dgvScanResult_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

