using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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
        private string filterWebserver = null;
        private List<string> webserverFolders = new List<string>();
        private bool isUpdatingComboBox = false;

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

            btnBrowse.Click += BtnBrowse_Click;
            btnReload.Click += async delegate
            {
                currentPage = 1; // Reset về trang đầu khi nạp lại
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
                    await CmbWebserver_SelectedIndexChanged();
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

        private async System.Threading.Tasks.Task CmbWebserver_SelectedIndexChanged()
        {
            if (cmbWebserver.SelectedIndex > 0 && cmbWebserver.SelectedItem != null)
            {
                filterWebserver = cmbWebserver.SelectedItem.ToString();
            }
            else
            {
                filterWebserver = null;
            }

            currentPage = 1; // Reset về trang đầu
            await LoadHashesFromDatabase();
        }

        private async System.Threading.Tasks.Task BtnClearFilter_Click()
        {
            cmbWebserver.SelectedIndex = 0;
            filterWebserver = null;

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
                    // Tự động load hashes nếu có đường dẫn hợp lệ (async)
                    LoadHashesFromDatabase();
                }
            }
            catch
            {
                // Bỏ qua lỗi, để người dùng tự chọn file
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

                // Cập nhật danh sách webserver folders và ComboBox
                UpdateWebserverFolders(allHashes);

                // Lọc theo webserver folder nếu có
                if (!string.IsNullOrEmpty(filterWebserver))
                {
                    allHashes = allHashes.Where(h =>
                        ExtractWebserverFolder(h.Path) == filterWebserver).ToList();
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
                }

                int startRecord = (currentPage - 1) * pageSize + 1;
                int endRecord = Math.Min(currentPage * pageSize, totalRecords);

                string filterInfo = "";
                if (!string.IsNullOrEmpty(filterWebserver))
                {
                    filterInfo = $" • Folder webserver: {filterWebserver}";
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

                    // Đọc tất cả records từ bảng hashes
                    string selectQuery = "SELECT hash, path FROM hashes ORDER BY path";

                    using (SQLiteCommand selectCmd = new SQLiteCommand(selectQuery, conn))
                    using (SQLiteDataReader dataReader = selectCmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            try
                            {
                                string hash = dataReader.IsDBNull(0) ? "" : dataReader.GetString(0);
                                string path = dataReader.IsDBNull(1) ? "" : dataReader.GetString(1);

                                if (!string.IsNullOrEmpty(hash) && !string.IsNullOrEmpty(path))
                                {
                                    hashes.Add(new HashEntry
                                    {
                                        Hash = hash,
                                        Path = path
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

        private void UpdateWebserverFolders(List<HashEntry> hashes)
        {
            // Extract unique webserver folders từ paths
            // Sử dụng tất cả paths để tìm folder webserver chính xác hơn
            var folders = new HashSet<string>();

            if (hashes.Count > 0)
            {
                // Tìm tất cả folder webserver khác nhau từ nhiều paths
                var webserverFoldersList = FindAllWebserverFolders(hashes.Select(h => h.Path).ToList());

                if (webserverFoldersList != null && webserverFoldersList.Count > 0)
                {
                    foreach (var folder in webserverFoldersList)
                    {
                        if (!string.IsNullOrEmpty(folder))
                        {
                            folders.Add(folder);
                        }
                    }
                }
                else
                {
                    // Fallback: extract từng path riêng lẻ
                    foreach (var hash in hashes)
                    {
                        string folder = ExtractWebserverFolder(hash.Path);
                        if (!string.IsNullOrEmpty(folder))
                        {
                            folders.Add(folder);
                        }
                    }
                }
            }

            // Cập nhật danh sách
            webserverFolders = folders.OrderBy(f => f).ToList();

            // Cập nhật ComboBox (giữ nguyên selection nếu có)
            // Đánh dấu đang update để tránh trigger event
            isUpdatingComboBox = true;

            string currentSelection = cmbWebserver.SelectedIndex > 0 ? cmbWebserver.SelectedItem?.ToString() : null;

            cmbWebserver.Items.Clear();
            cmbWebserver.Items.Add("Tất cả");

            foreach (var folder in webserverFolders)
            {
                cmbWebserver.Items.Add(folder);
            }

            // Khôi phục selection
            if (!string.IsNullOrEmpty(currentSelection) && webserverFolders.Contains(currentSelection))
            {
                cmbWebserver.SelectedIndex = webserverFolders.IndexOf(currentSelection) + 1;
            }
            else
            {
                cmbWebserver.SelectedIndex = 0;
            }

            // Cho phép event handler hoạt động lại
            isUpdatingComboBox = false;
        }

        private List<string> FindAllWebserverFolders(List<string> paths)
        {
            if (paths == null || paths.Count == 0)
                return null;

            try
            {
                // Chuyển đổi tất cả paths thành mảng parts
                var pathParts = new List<string[]>();
                foreach (var path in paths)
                {
                    if (!string.IsNullOrEmpty(path))
                    {
                        string normalizedPath = path.Replace('/', '\\');
                        string[] parts = normalizedPath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length > 0)
                        {
                            pathParts.Add(parts);
                        }
                    }
                }

                if (pathParts.Count == 0)
                    return null;

                // Tìm level đầu tiên có nhiều folder khác nhau (từ cuối lên)
                // Ví dụ:
                // Path 1: C:\Users\Admin\Desktop\Monitor-webshell\Agent\file1.txt
                // Path 2: C:\Users\Admin\Desktop\Monitor-webshell\Agent\file2.txt
                // Path 3: C:\Users\Admin\Desktop\Monitor-webshell\Server1\file3.txt
                // => Level của folder webserver là level có "Agent" và "Server1" (folder đầu tiên khác nhau)

                int maxLevel = pathParts.Max(p => p.Length);

                // Tìm từ cuối lên (bỏ qua file cuối cùng)
                for (int level = maxLevel - 2; level >= 0; level--)
                {
                    var foldersAtLevel = new HashSet<string>();

                    // Thu thập tất cả folder ở level này
                    foreach (var parts in pathParts)
                    {
                        if (level < parts.Length)
                        {
                            string folder = parts[level];
                            // Kiểm tra folder hợp lệ (không có extension, không phải drive letter)
                            if (!string.IsNullOrEmpty(folder) && !folder.Contains('.'))
                            {
                                foldersAtLevel.Add(folder);
                            }
                        }
                    }

                    // Nếu có nhiều folder khác nhau ở level này, đây là level của webserver folders
                    if (foldersAtLevel.Count > 1)
                    {
                        return foldersAtLevel.ToList();
                    }
                    // Nếu chỉ có 1 folder nhưng xuất hiện ở nhiều paths, có thể là folder webserver chung
                    else if (foldersAtLevel.Count == 1)
                    {
                        // Kiểm tra xem folder này có xuất hiện ở tất cả paths không
                        string folder = foldersAtLevel.First();
                        bool allPathsHaveThisFolder = true;
                        foreach (var parts in pathParts)
                        {
                            if (level >= parts.Length || parts[level] != folder)
                            {
                                allPathsHaveThisFolder = false;
                                break;
                            }
                        }

                        // Nếu không phải tất cả paths đều có folder này, thì level trước đó có thể là webserver level
                        if (!allPathsHaveThisFolder)
                        {
                            continue; // Tiếp tục tìm level khác
                        }
                    }
                }

                // Fallback: lấy folder cha của file từ tất cả paths
                var fallbackFolders = new HashSet<string>();
                foreach (var parts in pathParts)
                {
                    if (parts.Length > 1)
                    {
                        string folder = parts[parts.Length - 2];
                        if (!string.IsNullOrEmpty(folder) && !folder.Contains('.'))
                        {
                            fallbackFolders.Add(folder);
                        }
                    }
                }

                if (fallbackFolders.Count > 0)
                {
                    return fallbackFolders.ToList();
                }
            }
            catch
            {
                // Nếu có lỗi, trả về null
            }

            return null;
        }

        private string ExtractWebserverFolder(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            try
            {
                // Tìm folder webserver trong path
                // Logic: Tìm folder cha của file (thường là folder webserver)

                string normalizedPath = path.Replace('/', '\\');
                string[] parts = normalizedPath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

                // Lấy folder cha của file (folder thứ 2 từ cuối)
                if (parts.Length >= 2)
                {
                    string folder = parts[parts.Length - 2];
                    // Kiểm tra folder hợp lệ (không có extension, không phải drive letter)
                    if (!string.IsNullOrEmpty(folder) && !folder.Contains('.'))
                    {
                        return folder;
                    }
                }
            }
            catch
            {
                // Nếu có lỗi, trả về null
            }

            return null;
        }

        public class HashEntry
        {
            public int STT { get; set; }
            public string Hash { get; set; }
            public string Path { get; set; }

            public HashEntry()
            {
                STT = 0;
                Hash = "";
                Path = "";
            }
        }
    }
}

