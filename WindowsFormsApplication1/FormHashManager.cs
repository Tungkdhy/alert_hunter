using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace WindowsFormsApplication1
{
    public partial class FormHashManager : Form
    {
        Color Primary = Color.FromArgb(25, 55, 120);
        Color PrimaryLight = Color.FromArgb(230, 240, 255);
        Color DangerColor = Color.FromArgb(220, 53, 69);

        private int currentPage = 1;
        private int totalPages = 0;
        private int totalRecords = 0;
        private const int pageSize = 20;
        private string filterMonitorDir = null;
        private string filterTag = null;
        private List<string> monitorDirs = new List<string>();
        private bool isUpdatingComboBox = false;
        private bool isUpdatingTagComboBox = false;

        public FormHashManager()
        {
            InitializeComponent();

            // Icon
            if (System.IO.File.Exists(Application.StartupPath + "\\webshell_detector_round2.ico"))
                this.Icon = new Icon(Application.StartupPath + "\\webshell_detector_round2.ico");

            this.Text = "Quản lý mã hash";
            this.Font = new Font("Segoe UI", 10);
            this.BackColor = Color.White;
            this.StartPosition = FormStartPosition.CenterScreen;

            StyleUI();

            // Khởi tạo ComboBox với "Tất cả"
            cmbWebserver.Items.Clear();
            cmbWebserver.Items.Add("Tất cả");
            cmbWebserver.SelectedIndex = 0;

            // Khởi tạo Tag ComboBox với "Tất cả"
            cmbTag.Items.Clear();
            cmbTag.Items.Add("Tất cả");
            cmbTag.SelectedIndex = 0;

            // Load monitor_dirs từ config
            LoadMonitorDirsFromConfig();

            btnBrowse.Click += BtnBrowse_Click;
            btnReload.Click += async delegate
            {
                currentPage = 1; // Reset về trang đầu khi nạp lại
                // Reload monitor_dirs khi reload database
                LoadMonitorDirsFromConfig();
                await LoadHashesFromDatabase();
            };

            // Tự động load database path từ config
            LoadDatabasePathFromConfig();
            btnPrevious.Click += async delegate { await GoToPreviousPage(); };
            btnNext.Click += async delegate { await GoToNextPage(); };
            cmbWebserver.SelectedIndexChanged += async (s, e) =>
            {
                if (!isUpdatingComboBox)
                {
                    await CmbMonitorDir_SelectedIndexChanged();
                }
            };
            cmbTag.SelectedIndexChanged += async (s, e) =>
            {
                if (!isUpdatingTagComboBox)
                {
                    await CmbTag_SelectedIndexChanged();
                }
            };
            btnClearFilter.Click += async delegate { await BtnClearFilter_Click(); };

            // Double click to show details
            dgvHashes.CellDoubleClick += DgvHashes_CellDoubleClick;
            dgvHashes.KeyDown += DgvHashes_KeyDown;
        }

        private void DgvHashes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ShowHashDetail(e.RowIndex);
            }
        }

        private void DgvHashes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dgvHashes.SelectedRows.Count > 0)
            {
                ShowHashDetail(dgvHashes.SelectedRows[0].Index);
            }
        }

        private void ShowHashDetail(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvHashes.Rows.Count) return;

            var hashEntry = dgvHashes.Rows[rowIndex].DataBoundItem as HashEntry;
            if (hashEntry != null)
            {
                string message = $"Hash: {hashEntry.Hash}\n\nĐường dẫn: {hashEntry.Path}";
                if (!string.IsNullOrEmpty(hashEntry.Tag))
                {
                    message += $"\n\nPhiên bản: {hashEntry.Tag}";
                }
                MessageBox.Show(message, "Chi tiết hash", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void StyleUI()
        {
            StyleTextBox();
            StyleButtons();
            StyleDataGridView();
            StyleFilterControls();
        }

        private void StyleTextBox()
        {
            txtDbPath.BorderStyle = BorderStyle.FixedSingle;
            txtDbPath.BackColor = Color.White;
            txtDbPath.ForeColor = Color.FromArgb(50, 50, 50);

            if (txtDbPath.Multiline)
            {
                txtDbPath.Multiline = true;
                txtDbPath.Height = 33;
                txtDbPath.Padding = new Padding(0, 10, 0, 0);
            }
            else
            {
                txtDbPath.Padding = new Padding(5, 3, 5, 3);
            }
        }

        private void StyleFilterControls()
        {
            // Style filter buttons
            StyleButton(btnClearFilter, DangerColor, Color.White);

            // Style labels
            lblFilterWebserver.Font = new Font("Segoe UI Semibold", 10f);
            lblFilterWebserver.ForeColor = Color.FromArgb(50, 50, 50);
            lblFilterWebserver.Text = "Monitor dir:";
            lblTag.Font = new Font("Segoe UI Semibold", 10f);
            lblTag.ForeColor = Color.FromArgb(50, 50, 50);

            // Style ComboBox - FlatCombo tự động vẽ border qua WndProc
            cmbWebserver.Font = new Font("Segoe UI", 11f);
            cmbWebserver.FlatStyle = FlatStyle.Flat;
            cmbWebserver.BackColor = Color.White;
            cmbWebserver.ForeColor = Color.FromArgb(50, 50, 50);
            cmbWebserver.BorderColor = Color.FromArgb(200, 200, 200);

            // Custom draw for better appearance
            cmbWebserver.DrawMode = DrawMode.OwnerDrawFixed;
            cmbWebserver.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;

                e.DrawBackground();
                string text = cmbWebserver.Items[e.Index].ToString();
                using (Brush brush = new SolidBrush(e.ForeColor))
                {
                    e.Graphics.DrawString(text, cmbWebserver.Font, brush, e.Bounds);
                }

                // Highlight selected item
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    using (Pen pen = new Pen(Primary, 2))
                    {
                        e.Graphics.DrawRectangle(pen, e.Bounds);
                    }
                }
            };

            cmbWebserver.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbWebserver.DropDownHeight = 200;

            // Style Tag ComboBox
            cmbTag.Font = new Font("Segoe UI", 11f);
            cmbTag.FlatStyle = FlatStyle.Flat;
            cmbTag.BackColor = Color.White;
            cmbTag.ForeColor = Color.FromArgb(50, 50, 50);
            cmbTag.BorderColor = Color.FromArgb(200, 200, 200);

            // Custom draw for better appearance
            cmbTag.DrawMode = DrawMode.OwnerDrawFixed;
            cmbTag.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;

                e.DrawBackground();
                string text = cmbTag.Items[e.Index].ToString();
                using (Brush brush = new SolidBrush(e.ForeColor))
                {
                    e.Graphics.DrawString(text, cmbTag.Font, brush, e.Bounds);
                }

                // Highlight selected item
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    using (Pen pen = new Pen(Primary, 2))
                    {
                        e.Graphics.DrawRectangle(pen, e.Bounds);
                    }
                }
            };

            cmbTag.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTag.DropDownHeight = 200;
        }

        private void StyleButtons()
        {
            StyleButton(btnBrowse, Primary, Color.White);
            StyleButton(btnReload, Color.FromArgb(20, 120, 220), Color.White);
            StyleButton(btnPrevious, Color.FromArgb(100, 100, 100), Color.White);
            StyleButton(btnNext, Color.FromArgb(100, 100, 100), Color.White);
        }

        private void StyleButton(Button btn, Color back, Color text)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = back;
            btn.ForeColor = text;
            btn.Font = new Font("Segoe UI Semibold", 10);
            btn.Cursor = Cursors.Hand;

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

        private void StyleDataGridView()
        {
            dgvHashes.BackgroundColor = Color.FromArgb(248, 249, 250);
            dgvHashes.BorderStyle = BorderStyle.None;
            dgvHashes.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvHashes.EnableHeadersVisualStyles = false;

            // Header
            dgvHashes.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvHashes.ColumnHeadersDefaultCellStyle.BackColor = Primary;
            dgvHashes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvHashes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5f);
            dgvHashes.ColumnHeadersDefaultCellStyle.Padding = new Padding(12, 10, 12, 10);
            dgvHashes.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvHashes.ColumnHeadersHeight = 42;

            // Cells
            dgvHashes.DefaultCellStyle.Font = new Font("Segoe UI", 10f);
            dgvHashes.DefaultCellStyle.Padding = new Padding(12, 8, 12, 8);
            dgvHashes.DefaultCellStyle.BackColor = Color.White;
            dgvHashes.DefaultCellStyle.ForeColor = Color.FromArgb(50, 50, 50);
            dgvHashes.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 240, 255);
            dgvHashes.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Alternating rows
            dgvHashes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(252, 253, 254);
            dgvHashes.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(50, 50, 50);

            // Grid lines
            dgvHashes.GridColor = Color.FromArgb(240, 242, 245);
            dgvHashes.RowTemplate.Height = 38;

            // Selection
            dgvHashes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHashes.MultiSelect = false;
        }

        private async System.Threading.Tasks.Task CmbMonitorDir_SelectedIndexChanged()
        {
            if (cmbWebserver.SelectedIndex > 0 && cmbWebserver.SelectedItem != null)
            {
                filterMonitorDir = cmbWebserver.SelectedItem.ToString();
            }
            else
            {
                filterMonitorDir = null;
            }

            currentPage = 1; // Reset về trang đầu
            await LoadHashesFromDatabase();
        }

        private async System.Threading.Tasks.Task CmbTag_SelectedIndexChanged()
        {
            if (cmbTag.SelectedIndex > 0 && cmbTag.SelectedItem != null)
            {
                filterTag = cmbTag.SelectedItem.ToString();
            }
            else
            {
                filterTag = null;
            }

            currentPage = 1; // Reset về trang đầu
            await LoadHashesFromDatabase();
        }

        private async System.Threading.Tasks.Task BtnClearFilter_Click()
        {
            cmbWebserver.SelectedIndex = 0;
            cmbTag.SelectedIndex = 0;
            filterMonitorDir = null;
            filterTag = null;

            currentPage = 1; // Reset về trang đầu
            await LoadHashesFromDatabase();
        }

        private void LoadDatabasePathFromConfig()
        {
            try
            {
                string dbPath = DbConfigHelper.GetDatabasePath();
                if (!string.IsNullOrEmpty(dbPath) && File.Exists(dbPath))
                {
                    txtDbPath.Text = dbPath;
                    // Reload monitor_dirs từ config.dat cùng thư mục với database
                    LoadMonitorDirsFromConfig();
                    // Tự động load hashes nếu có đường dẫn hợp lệ (async)
                    LoadHashesFromDatabase();
                }
            }
            catch
            {
                // Bỏ qua lỗi, để người dùng tự chọn file
            }
        }

        private void LoadMonitorDirsFromConfig()
        {
            try
            {
                // Tìm file config - ưu tiên cùng thư mục với database
                string configPath = null;
                string searchDirectory = Application.StartupPath;
                
                // Đọc đường dẫn database từ db_config
                string dbPath = DbConfigHelper.GetDatabasePath();
                if (!string.IsNullOrEmpty(dbPath) && File.Exists(dbPath))
                {
                    string dbDirectory = Path.GetDirectoryName(dbPath);
                    if (!string.IsNullOrEmpty(dbDirectory) && Directory.Exists(dbDirectory))
                    {
                        searchDirectory = dbDirectory;
                    }
                }

                // Thứ tự ưu tiên: config.dat cùng thư mục DB > agent_config.json > config.dat startup > config.template > config.example
                string[] configFiles = {
                    Path.Combine(searchDirectory, "config.dat"),
                    Path.Combine(Application.StartupPath, "agent_config.json"),
                    Path.Combine(Application.StartupPath, "config.dat"),
                    Path.Combine(Application.StartupPath, "config.template"),
                    Path.Combine(Application.StartupPath, "config.example")
                };

                foreach (string file in configFiles)
                {
                    if (File.Exists(file))
                    {
                        configPath = file;
                        break;
                    }
                }

                if (configPath == null)
                    return;

                // Đọc và parse config
                JObject config = ReadConfigFile(configPath);
                if (config != null)
                {
                    JArray monitorDirsArray = config["monitor_dirs"] as JArray;
                    if (monitorDirsArray != null)
                    {
                        monitorDirs.Clear();
                        foreach (var item in monitorDirsArray)
                        {
                            string dir = item?.ToString();
                            if (!string.IsNullOrEmpty(dir))
                            {
                                monitorDirs.Add(dir);
                            }
                        }

                        // Cập nhật ComboBox
                        isUpdatingComboBox = true;
                        cmbWebserver.Items.Clear();
                        cmbWebserver.Items.Add("Tất cả");
                        foreach (var dir in monitorDirs)
                        {
                            cmbWebserver.Items.Add(dir);
                        }
                        cmbWebserver.SelectedIndex = 0;
                        isUpdatingComboBox = false;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi load monitor_dirs từ config: {ex.Message}");
            }
        }

        private JObject ReadConfigFile(string filePath)
        {
            try
            {
                string jsonContent = "";

                // File .template và .example luôn là file text, không cần giải mã
                if (filePath.EndsWith(".template", StringComparison.OrdinalIgnoreCase) || 
                    filePath.EndsWith(".example", StringComparison.OrdinalIgnoreCase))
                {
                    jsonContent = File.ReadAllText(filePath, Encoding.UTF8);
                }
                else
                {
                    // Đọc file dưới dạng binary để xử lý cả file .dat (binary) và .json (text)
                    byte[] fileBytes = File.ReadAllBytes(filePath);

                    // Kiểm tra xem có phải file mã hóa không
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
                }

                // Loại bỏ BOM nếu có
                if (jsonContent.Length > 0 && jsonContent[0] == '\uFEFF')
                {
                    jsonContent = jsonContent.Substring(1);
                }

                jsonContent = jsonContent.Trim();

                return JObject.Parse(jsonContent);
            }
            catch
            {
                return null;
            }
        }

        private string DecryptConfig(string cipherText)
        {
            try
            {
                string keySuffix = "wsdetector V1.1.4 ";
                byte[] keyBytes = MacAddressHelper.GetConfigKey(keySuffix);

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
                int finalLen = gcm.DoFinal(decryptedData, len);

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

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "SQLite Database (*.db;*.sqlite;*.sqlite3)|*.db;*.sqlite;*.sqlite3|All files (*.*)|*.*";
            ofd.Title = "Chọn file SQLite database";
            ofd.CheckFileExists = true;
            ofd.Multiselect = false;

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                txtDbPath.Text = ofd.FileName;
                currentPage = 1; // Reset về trang đầu
                // Reload monitor_dirs từ config.dat cùng thư mục với database mới
                LoadMonitorDirsFromConfig();
                LoadHashesFromDatabase();
            }
        }

        private async System.Threading.Tasks.Task LoadHashesFromDatabase()
        {
            string dbPath = txtDbPath.Text.Trim();
            if (string.IsNullOrEmpty(dbPath) || !File.Exists(dbPath))
            {
                lblStatus.Text = "Chưa chọn database hoặc file không tồn tại.";
                dgvHashes.DataSource = null;
                UpdatePaginationControls();
                return;
            }

            try
            {
                lblStatus.Text = "Đang tải dữ liệu...";
                dgvHashes.DataSource = null;

                // Đọc tất cả hashes từ DB
                List<HashEntry> allHashes = await System.Threading.Tasks.Task.Run(() => ReadAllHashesFromSQLite(dbPath));

                // Cập nhật danh sách tag và ComboBox
                UpdateTagList(allHashes);

                // Lọc theo monitor_dir nếu có
                if (!string.IsNullOrEmpty(filterMonitorDir))
                {
                    allHashes = allHashes.Where(h =>
                        h.Path.StartsWith(filterMonitorDir, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                // Lọc theo tag nếu có
                if (!string.IsNullOrEmpty(filterTag))
                {
                    allHashes = allHashes.Where(h => h.Tag == filterTag).ToList();
                }

                // Đếm tổng số bản ghi sau khi lọc
                totalRecords = allHashes.Count;
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                if (totalPages == 0) totalPages = 1;

                // Đảm bảo currentPage hợp lệ
                if (currentPage > totalPages) currentPage = totalPages;
                if (currentPage < 1) currentPage = 1;

                // Áp dụng pagination
                var hashes = allHashes.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                if (hashes == null || hashes.Count == 0)
                {
                    lblStatus.Text = "Không có dữ liệu trong database.";
                    UpdatePaginationControls();
                    return;
                }

                // Thêm STT cho mỗi hash entry
                int startIndex = (currentPage - 1) * pageSize + 1;
                for (int i = 0; i < hashes.Count; i++)
                {
                    hashes[i].STT = startIndex + i;
                }

                // Bind data to DataGridView
                dgvHashes.DataSource = hashes;

                // Configure columns after binding
                if (dgvHashes.Columns.Count > 0)
                {
                    // Set column headers và thứ tự
                    if (dgvHashes.Columns["STT"] != null)
                    {
                        dgvHashes.Columns["STT"].HeaderText = "STT";
                        dgvHashes.Columns["STT"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        dgvHashes.Columns["STT"].Width = 60;
                        dgvHashes.Columns["STT"].DisplayIndex = 0;
                        dgvHashes.Columns["STT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    if (dgvHashes.Columns["Hash"] != null)
                    {
                        dgvHashes.Columns["Hash"].HeaderText = "Mã hash";
                        dgvHashes.Columns["Hash"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        dgvHashes.Columns["Hash"].Width = 300;
                        dgvHashes.Columns["Hash"].DisplayIndex = 1;
                    }
                    if (dgvHashes.Columns["Path"] != null)
                    {
                        dgvHashes.Columns["Path"].HeaderText = "Đường dẫn";
                        dgvHashes.Columns["Path"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dgvHashes.Columns["Path"].DisplayIndex = 2;
                    }
                    if (dgvHashes.Columns["Tag"] != null)
                    {
                        dgvHashes.Columns["Tag"].HeaderText = "Phiên bản";
                        dgvHashes.Columns["Tag"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        dgvHashes.Columns["Tag"].DisplayIndex = 3;
                    }
                }

                int startRecord = (currentPage - 1) * pageSize + 1;
                int endRecord = Math.Min(currentPage * pageSize, totalRecords);

                string filterInfo = "";
                if (!string.IsNullOrEmpty(filterMonitorDir))
                {
                    filterInfo = $" • Monitor dir: {filterMonitorDir}";
                }
                if (!string.IsNullOrEmpty(filterTag))
                {
                    if (!string.IsNullOrEmpty(filterInfo))
                        filterInfo += $" • Tag: {filterTag}";
                    else
                        filterInfo = $" • Tag: {filterTag}";
                }

                lblStatus.Text = string.Format("Hiển thị {0}-{1} / {2} bản ghi • Trang {3}/{4}{5} • {6:HH:mm:ss}",
                    startRecord, endRecord, totalRecords, currentPage, totalPages, filterInfo, DateTime.Now);

                UpdatePaginationControls();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Lỗi đọc database: " + ex.Message;
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdatePaginationControls();
            }
        }

        private void UpdatePaginationControls()
        {
            btnPrevious.Enabled = currentPage > 1;
            btnNext.Enabled = currentPage < totalPages;
            lblPageInfo.Text = string.Format("Trang {0} / {1}", currentPage, totalPages);
        }

        private async System.Threading.Tasks.Task GoToPreviousPage()
        {
            if (currentPage > 1)
            {
                currentPage--;
                await LoadHashesFromDatabase();
            }
        }

        private async System.Threading.Tasks.Task GoToNextPage()
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                await LoadHashesFromDatabase();
            }
        }

        private List<HashEntry> ReadAllHashesFromSQLite(string dbPath)
        {
            var hashes = new List<HashEntry>();

            try
            {
                string connectionString = $"Data Source={dbPath};Version=3;";

                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    // Kiểm tra xem bảng hashes có tồn tại không
                    using (SQLiteCommand checkCmd = new SQLiteCommand(
                        "SELECT name FROM sqlite_master WHERE type='table' AND name='hashes'", conn))
                    {
                        object result = checkCmd.ExecuteScalar();
                        if (result == null)
                        {
                            throw new Exception("Không tìm thấy bảng 'hashes' trong database.");
                        }
                    }

                    // Đọc tất cả records từ bảng hashes (thêm cột tag nếu có)
                    // Kiểm tra xem cột tag có tồn tại không
                    bool hasTagColumn = false;
                    using (SQLiteCommand checkTagCmd = new SQLiteCommand(
                        "PRAGMA table_info(hashes)", conn))
                    using (SQLiteDataReader pragmaReader = checkTagCmd.ExecuteReader())
                    {
                        while (pragmaReader.Read())
                        {
                            string columnName = pragmaReader.GetString(1);
                            if (columnName.Equals("tag", StringComparison.OrdinalIgnoreCase))
                            {
                                hasTagColumn = true;
                                break;
                            }
                        }
                    }

                    string selectQuery;
                    if (hasTagColumn)
                    {
                        selectQuery = "SELECT hash, path, tag FROM hashes ORDER BY path";
                    }
                    else
                    {
                        selectQuery = "SELECT hash, path FROM hashes ORDER BY path";
                    }

                    using (SQLiteCommand selectCmd = new SQLiteCommand(selectQuery, conn))
                    using (SQLiteDataReader dataReader = selectCmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            try
                            {
                                string hash = dataReader.IsDBNull(0) ? "" : dataReader.GetString(0);
                                string path = dataReader.IsDBNull(1) ? "" : dataReader.GetString(1);
                                string tag = "";
                                
                                // Đọc tag nếu có cột tag
                                if (dataReader.FieldCount > 2)
                                {
                                    tag = dataReader.IsDBNull(2) ? "" : dataReader.GetString(2);
                                }

                                if (!string.IsNullOrEmpty(hash) && !string.IsNullOrEmpty(path))
                                {
                                    hashes.Add(new HashEntry
                                    {
                                        Hash = hash,
                                        Path = path,
                                        Tag = tag
                                    });
                                }
                            }
                            catch (Exception ex)
                            {
                                // Bỏ qua lỗi parse, tiếp tục với record tiếp theo
                                System.Diagnostics.Debug.WriteLine($"Lỗi parse hash: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi đọc SQLite: {ex.Message}", ex);
            }

            return hashes;
        }


        private void UpdateTagList(List<HashEntry> hashes)
        {
            // Lấy danh sách tag duy nhất từ hashes
            var uniqueTags = hashes
                .Where(h => !string.IsNullOrWhiteSpace(h.Tag))
                .Select(h => h.Tag)
                .Distinct()
                .OrderBy(tag => tag)
                .ToList();

            // Cập nhật ComboBox (giữ nguyên selection nếu có)
            isUpdatingTagComboBox = true;

            string currentSelection = cmbTag.SelectedIndex > 0 ? cmbTag.SelectedItem?.ToString() : null;

            cmbTag.Items.Clear();
            cmbTag.Items.Add("Tất cả");

            foreach (var tag in uniqueTags)
            {
                cmbTag.Items.Add(tag);
            }

            // Khôi phục selection
            if (!string.IsNullOrEmpty(currentSelection) && uniqueTags.Contains(currentSelection))
            {
                cmbTag.SelectedIndex = uniqueTags.IndexOf(currentSelection) + 1;
            }
            else
            {
                cmbTag.SelectedIndex = 0;
            }

            // Cho phép event handler hoạt động lại
            isUpdatingTagComboBox = false;
        }

        public class HashEntry
        {
            public int STT { get; set; }
            public string Hash { get; set; }
            public string Path { get; set; }
            public string Tag { get; set; }

            public HashEntry()
            {
                STT = 0;
                Hash = "";
                Path = "";
                Tag = "";
            }
        }
    }
}

