using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WindowsFormsApplication1
{
    public partial class FormLogViewer : Form
    {
        Color Primary = Color.FromArgb(25, 55, 120);
        Color PrimaryLight = Color.FromArgb(230, 240, 255);
        Color DangerColor = Color.FromArgb(220, 53, 69);

        private int currentPage = 1;
        private int totalPages = 0;
        private int totalRecords = 0;
        private const int pageSize = 20;
        private DateTime? filterFromDate = null;
        private DateTime? filterToDate = null;
        private string filterSeverity = null;
        private string filterTag = null;

        public FormLogViewer()
        {
            InitializeComponent();

            // Icon
            if (System.IO.File.Exists(Application.StartupPath + "\\webshell_detector_round2.ico"))
                this.Icon = new Icon(Application.StartupPath + "\\webshell_detector_round2.ico");

            this.Text = "Trình xem log từ SQLite";
            this.Font = new Font("Segoe UI", 10);
            this.BackColor = Color.White;
            this.StartPosition = FormStartPosition.CenterScreen;

            StyleUI();

            // Initialize date pickers
            dtpFromDate.Value = DateTime.Now.AddDays(-7); // Default: 7 days ago
            dtpToDate.Value = DateTime.Now.AddDays(1); // Default: tomorrow (to include today)
            dtpFromDate.ShowCheckBox = true;
            dtpToDate.ShowCheckBox = true;
            dtpFromDate.Checked = false;
            dtpToDate.Checked = false;

            // Initialize severity filter
            cmbSeverity.Items.Add("Tất cả");
            cmbSeverity.Items.Add("Nghiêm trọng");
            cmbSeverity.Items.Add("Cao");
            cmbSeverity.Items.Add("Trung bình");
            cmbSeverity.Items.Add("Thấp");
            cmbSeverity.Items.Add("Thông tin");
            cmbSeverity.SelectedIndex = 0; // Default: "Tất cả"

            // Initialize tag filter
            cmbTag.Items.Add("Tất cả");
            cmbTag.SelectedIndex = 0; // Default: "Tất cả"
            cmbTag.SelectedIndexChanged += CmbTag_SelectedIndexChanged;

            btnBrowse.Click += BtnBrowse_Click;
            btnReload.Click += async delegate
            {
                currentPage = 1; // Reset về trang đầu khi nạp lại
                await LoadLogsFromDatabase();
            };

            // Tự động load database path từ config
            LoadDatabasePathFromConfig();
            btnPrevious.Click += async delegate { await GoToPreviousPage(); };
            btnNext.Click += async delegate { await GoToNextPage(); };
            btnFilter.Click += async delegate { await BtnFilter_Click(); };
            btnClearFilter.Click += async delegate { await BtnClearFilter_Click(); };

            // Apply row colors after data binding
            dgvLogs.DataBindingComplete += (s, e) =>
            {
                ApplyRowColors();
            };

            // Double click to show details
            dgvLogs.CellDoubleClick += DgvLogs_CellDoubleClick;
            dgvLogs.KeyDown += DgvLogs_KeyDown;
        }

        private void DgvLogs_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                ShowLogDetail(e.RowIndex);
            }
        }

        private void DgvLogs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dgvLogs.SelectedRows.Count > 0)
            {
                ShowLogDetail(dgvLogs.SelectedRows[0].Index);
            }
        }

        private void ShowLogDetail(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvLogs.Rows.Count) return;

            var log = dgvLogs.Rows[rowIndex].DataBoundItem as AlertLogEntry;
            if (log != null && log.RawJson != null)
            {
                var detailForm = new FormLogDetail();
                detailForm.LoadLogData(log);
                detailForm.ShowDialog(this);
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

            // Căn giữa text theo chiều ngang và dọc
            //txtDbPath.TextAlign = HorizontalAlignment.Center;

            // Với Multiline TextBox, tính toán padding để căn giữa theo chiều dọc
            if (txtDbPath.Multiline)
            {
                txtDbPath.Multiline = true;
                txtDbPath.Height = 33;
                // Tính toán padding dựa trên chiều cao TextBox để text nằm giữa
                // Chiều cao 33px, font 10F (~14px line height), cần padding ~(33-14)/2 = ~9-10px
                txtDbPath.Padding = new Padding(0, 10, 0, 0);
            }
            else
            {
                // Với TextBox đơn dòng, thêm padding nhỏ
                txtDbPath.Padding = new Padding(5, 3, 5, 3);
            }
        }

        private void StyleFilterControls()
        {
            // Style DateTimePickers
            dtpFromDate.Font = new Font("Segoe UI", 11f);
            dtpFromDate.CalendarFont = new Font("Segoe UI", 11f);
            dtpFromDate.CalendarForeColor = Color.FromArgb(50, 50, 50);
            dtpFromDate.CalendarTitleBackColor = Primary;
            dtpFromDate.CalendarTitleForeColor = Color.White;
            dtpFromDate.CalendarTrailingForeColor = Color.FromArgb(180, 180, 180);

            dtpToDate.Font = new Font("Segoe UI", 11f);
            dtpToDate.CalendarFont = new Font("Segoe UI", 11f);
            dtpToDate.CalendarForeColor = Color.FromArgb(50, 50, 50);
            dtpToDate.CalendarTitleBackColor = Primary;
            dtpToDate.CalendarTitleForeColor = Color.White;
            dtpToDate.CalendarTrailingForeColor = Color.FromArgb(180, 180, 180);

            // Style filter buttons
            StyleButton(btnFilter, Primary, Color.White);
            StyleButton(btnClearFilter, DangerColor, Color.White);

            // Style labels
            lblFromDate.Font = new Font("Segoe UI Semibold", 10f);
            lblFromDate.ForeColor = Color.FromArgb(50, 50, 50);
            lblToDate.Font = new Font("Segoe UI Semibold", 10f);
            lblToDate.ForeColor = Color.FromArgb(50, 50, 50);
            lblSeverity.Font = new Font("Segoe UI Semibold", 10f);
            lblSeverity.ForeColor = Color.FromArgb(50, 50, 50);

            // Style ComboBox - FlatCombo tự động vẽ border qua WndProc
            cmbSeverity.Font = new Font("Segoe UI", 11f);
            cmbSeverity.FlatStyle = FlatStyle.Flat;
            cmbSeverity.BackColor = Color.White;
            cmbSeverity.ForeColor = Color.FromArgb(50, 50, 50);
            cmbSeverity.BorderColor = Color.FromArgb(200, 200, 200);

            // Custom draw for better appearance
            cmbSeverity.DrawMode = DrawMode.OwnerDrawFixed;
            cmbSeverity.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;

                e.DrawBackground();
                string text = cmbSeverity.Items[e.Index].ToString();
                using (Brush brush = new SolidBrush(e.ForeColor))
                {
                    e.Graphics.DrawString(text, cmbSeverity.Font, brush, e.Bounds);
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

            cmbSeverity.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSeverity.DropDownHeight = 200;

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

            // Style label
            lblTag.Font = new Font("Segoe UI Semibold", 10f);
            lblTag.ForeColor = Color.FromArgb(50, 50, 50);
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
            dgvLogs.BackgroundColor = Color.FromArgb(248, 249, 250);
            dgvLogs.BorderStyle = BorderStyle.None;
            dgvLogs.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvLogs.EnableHeadersVisualStyles = false;

            // Header
            dgvLogs.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvLogs.ColumnHeadersDefaultCellStyle.BackColor = Primary;
            dgvLogs.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLogs.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5f);
            dgvLogs.ColumnHeadersDefaultCellStyle.Padding = new Padding(12, 10, 12, 10);
            dgvLogs.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvLogs.ColumnHeadersHeight = 42;

            // Cells
            dgvLogs.DefaultCellStyle.Font = new Font("Segoe UI", 10f);
            dgvLogs.DefaultCellStyle.Padding = new Padding(12, 8, 12, 8);
            dgvLogs.DefaultCellStyle.BackColor = Color.White;
            dgvLogs.DefaultCellStyle.ForeColor = Color.FromArgb(50, 50, 50);
            dgvLogs.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 240, 255);
            dgvLogs.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Alternating rows
            dgvLogs.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(252, 253, 254);
            dgvLogs.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(50, 50, 50);

            // Grid lines
            dgvLogs.GridColor = Color.FromArgb(240, 242, 245);
            dgvLogs.RowTemplate.Height = 38;

            // Selection
            dgvLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLogs.MultiSelect = false;
        }

        private async System.Threading.Tasks.Task BtnFilter_Click()
        {
            // Validate dates
            if (dtpFromDate.Checked && dtpToDate.Checked)
            {
                if (dtpFromDate.Value > dtpToDate.Value)
                {
                    MessageBox.Show("Ngày bắt đầu không thể lớn hơn ngày kết thúc!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Set filter dates
            filterFromDate = dtpFromDate.Checked ? dtpFromDate.Value.Date : (DateTime?)null;
            filterToDate = dtpToDate.Checked ? dtpToDate.Value.Date.AddDays(1).AddTicks(-1) : (DateTime?)null; // End of day

            // Set filter severity
            if (cmbSeverity.SelectedIndex > 0 && cmbSeverity.SelectedItem != null)
            {
                filterSeverity = cmbSeverity.SelectedItem.ToString();
            }
            else
            {
                filterSeverity = null;
            }

            // Set filter tag
            if (cmbTag.SelectedIndex > 0 && cmbTag.SelectedItem != null)
            {
                filterTag = cmbTag.SelectedItem.ToString();
            }
            else
            {
                filterTag = null;
            }

            currentPage = 1; // Reset về trang đầu
            await LoadLogsFromDatabase();
        }

        private async System.Threading.Tasks.Task BtnClearFilter_Click()
        {
            dtpFromDate.Checked = false;
            dtpToDate.Checked = false;
            cmbSeverity.SelectedIndex = 0;
            cmbTag.SelectedIndex = 0;
            filterFromDate = null;
            filterToDate = null;
            filterSeverity = null;
            filterTag = null;

            currentPage = 1; // Reset về trang đầu
            await LoadLogsFromDatabase();
        }

        private void LoadDatabasePathFromConfig()
        {
            try
            {
                string dbPath = DbConfigHelper.GetDatabasePath();
                if (!string.IsNullOrEmpty(dbPath) && File.Exists(dbPath))
                {
                    txtDbPath.Text = dbPath;
                    // Tự động load logs nếu có đường dẫn hợp lệ (async)
                    LoadLogsFromDatabase();
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
                LoadLogsFromDatabase();
            }
        }

        private async System.Threading.Tasks.Task LoadLogsFromDatabase()
        {
            string dbPath = txtDbPath.Text.Trim();
            if (string.IsNullOrEmpty(dbPath) || !File.Exists(dbPath))
            {
                lblStatus.Text = "Chưa chọn database hoặc file không tồn tại.";
                dgvLogs.DataSource = null;
                UpdatePaginationControls();
                return;
            }

            try
            {
                lblStatus.Text = "Đang tải dữ liệu...";
                dgvLogs.DataSource = null;

                // Đọc tất cả logs từ DB (với date filter)
                List<AlertLogEntry> allLogs = await System.Threading.Tasks.Task.Run(() => ReadAllLogsFromSQLite(dbPath));

                // Load danh sách tag vào ComboBox
                await System.Threading.Tasks.Task.Run(() => LoadTagList(dbPath, allLogs));

                // Lọc theo severity nếu có
                if (!string.IsNullOrEmpty(filterSeverity))
                {
                    allLogs = allLogs.Where(log => log.Severity == filterSeverity).ToList();
                }

                // Lọc theo tag nếu có
                if (!string.IsNullOrEmpty(filterTag))
                {
                    allLogs = allLogs.Where(log => log.Tag == filterTag).ToList();
                }

                // Đếm tổng số bản ghi sau khi lọc
                totalRecords = allLogs.Count;
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                if (totalPages == 0) totalPages = 1;

                // Đảm bảo currentPage hợp lệ
                if (currentPage > totalPages) currentPage = totalPages;
                if (currentPage < 1) currentPage = 1;

                // Áp dụng pagination
                var logs = allLogs.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                if (logs == null || logs.Count == 0)
                {
                    lblStatus.Text = "Không có dữ liệu trong database.";
                    UpdatePaginationControls();
                    return;
                }

                // Bind data to DataGridView
                dgvLogs.DataSource = logs;

                // Configure columns after binding
                if (dgvLogs.Columns.Count > 0)
                {
                    // Hide columns that shouldn't be displayed
                    if (dgvLogs.Columns["RawJson"] != null)
                        dgvLogs.Columns["RawJson"].Visible = false;
                    if (dgvLogs.Columns["Timestamp"] != null)
                        dgvLogs.Columns["Timestamp"].Visible = false;
                    if (dgvLogs.Columns["HashBefore"] != null)
                        dgvLogs.Columns["HashBefore"].Visible = false;
                    if (dgvLogs.Columns["HashAfter"] != null)
                        dgvLogs.Columns["HashAfter"].Visible = false;
                    if (dgvLogs.Columns["Level"] != null)
                        dgvLogs.Columns["Level"].Visible = false;
                    if (dgvLogs.Columns["Op"] != null)
                        dgvLogs.Columns["Op"].Visible = false;

                    // Set column headers
                    if (dgvLogs.Columns["Path"] != null)
                    {
                        dgvLogs.Columns["Path"].HeaderText = "Đường dẫn";
                        dgvLogs.Columns["Path"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                    if (dgvLogs.Columns["Reason"] != null)
                    {
                        dgvLogs.Columns["Reason"].HeaderText = "Lý do";
                        dgvLogs.Columns["Reason"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }
                    if (dgvLogs.Columns["RuleNames"] != null)
                    {
                        dgvLogs.Columns["RuleNames"].HeaderText = "Rules";
                        dgvLogs.Columns["RuleNames"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    }
                    if (dgvLogs.Columns["Score"] != null)
                    {
                        dgvLogs.Columns["Score"].HeaderText = "Điểm";
                        dgvLogs.Columns["Score"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }
                    if (dgvLogs.Columns["Severity"] != null)
                    {
                        dgvLogs.Columns["Severity"].HeaderText = "Mức độ";
                        dgvLogs.Columns["Severity"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }
                    if (dgvLogs.Columns["TimestampFormatted"] != null)
                    {
                        dgvLogs.Columns["TimestampFormatted"].HeaderText = "Thời gian";
                        dgvLogs.Columns["TimestampFormatted"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }
                    if (dgvLogs.Columns["Tag"] != null)
                    {
                        dgvLogs.Columns["Tag"].HeaderText = "Phiên bản";
                        dgvLogs.Columns["Tag"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }
                }

                // Apply colors will be triggered by DataBindingComplete event
                int startRecord = (currentPage - 1) * pageSize + 1;
                int endRecord = Math.Min(currentPage * pageSize, totalRecords);

                string filterInfo = "";
                if (filterFromDate.HasValue || filterToDate.HasValue)
                {
                    string fromStr = filterFromDate.HasValue ? filterFromDate.Value.ToString("dd/MM/yyyy") : "...";
                    string toStr = filterToDate.HasValue ? filterToDate.Value.ToString("dd/MM/yyyy") : "...";
                    filterInfo = $" • Đã lọc: {fromStr} → {toStr}";
                }
                if (!string.IsNullOrEmpty(filterSeverity))
                {
                    if (!string.IsNullOrEmpty(filterInfo))
                        filterInfo += $" • Mức độ: {filterSeverity}";
                    else
                        filterInfo = $" • Mức độ: {filterSeverity}";
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
                await LoadLogsFromDatabase();
            }
        }

        private async System.Threading.Tasks.Task GoToNextPage()
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                await LoadLogsFromDatabase();
            }
        }

        private int CountTotalRecords(string dbPath)
        {
            try
            {
                string connectionString = $"Data Source={dbPath};Version=3;";

                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    // Tìm bảng
                    string tableName = FindTableName(conn);
                    if (string.IsNullOrEmpty(tableName))
                    {
                        return 0;
                    }

                    // Đếm trực tiếp từ database với filter date
                    string countQuery = $"SELECT COUNT(*) FROM {tableName}";

                    // Nếu có filter date, thêm điều kiện WHERE
                    if (filterFromDate.HasValue || filterToDate.HasValue)
                    {
                        var conditions = new List<string>();
                        if (filterFromDate.HasValue)
                        {
                            conditions.Add($"datetime(ts) >= datetime('{filterFromDate.Value:yyyy-MM-dd}')");
                        }
                        if (filterToDate.HasValue)
                        {
                            conditions.Add($"datetime(ts) <= datetime('{filterToDate.Value:yyyy-MM-dd} 23:59:59')");
                        }
                        if (conditions.Count > 0)
                        {
                            countQuery += " WHERE " + string.Join(" AND ", conditions);
                        }
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(countQuery, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            return Convert.ToInt32(result);
                        }
                    }

                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        private string FindTableName(SQLiteConnection conn)
        {
            // Thử các tên bảng phổ biến
            string[] possibleTables = { "logs", "log", "log_entries", "logentries", "alert_logs", "alerts" };

            foreach (var table in possibleTables)
            {
                using (SQLiteCommand cmd = new SQLiteCommand(
                    "SELECT name FROM sqlite_master WHERE type='table' AND name=@name", conn))
                {
                    cmd.Parameters.AddWithValue("@name", table);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return table;
                        }
                    }
                }
            }

            // Lấy tên bảng đầu tiên
            using (SQLiteCommand cmd = new SQLiteCommand(
                "SELECT name FROM sqlite_master WHERE type='table' LIMIT 1", conn))
            {
                object result = cmd.ExecuteScalar();
                if (result != null)
                    return result.ToString();
            }

            return null;
        }

        private List<AlertLogEntry> ReadAllLogsFromSQLite(string dbPath)
        {
            var logs = new List<AlertLogEntry>();

            try
            {
                string connectionString = $"Data Source={dbPath};Version=3;";

                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    // Tìm bảng
                    string tableName = FindTableName(conn);
                    if (string.IsNullOrEmpty(tableName))
                    {
                        return logs;
                    }

                    return ReadAllLogsFromSQLite(conn, tableName);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi đọc SQLite: {ex.Message}", ex);
            }
        }

        private List<AlertLogEntry> ReadAllLogsFromSQLite(SQLiteConnection conn, string tableName)
        {
            var logs = new List<AlertLogEntry>();

            try
            {
                // Đọc tất cả records từ các cột cụ thể với filter date (thêm cột tag)
                string selectQuery = $"SELECT id, ts, path, reason, hash_before, hash_after, level, op, sent_remote, yara_rules, tag FROM {tableName}";

                // Thêm điều kiện WHERE nếu có filter date
                if (filterFromDate.HasValue || filterToDate.HasValue)
                {
                    var conditions = new List<string>();
                    if (filterFromDate.HasValue)
                    {
                        conditions.Add($"datetime(ts) >= datetime('{filterFromDate.Value:yyyy-MM-dd}')");
                    }
                    if (filterToDate.HasValue)
                    {
                        conditions.Add($"datetime(ts) <= datetime('{filterToDate.Value:yyyy-MM-dd} 23:59:59')");
                    }
                    if (conditions.Count > 0)
                    {
                        selectQuery += " WHERE " + string.Join(" AND ", conditions);
                    }
                }

                selectQuery += " ORDER BY id DESC";

                using (SQLiteCommand selectCmd = new SQLiteCommand(selectQuery, conn))
                using (SQLiteDataReader dataReader = selectCmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        try
                        {
                            // Đọc các cột trực tiếp từ database
                            int id = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0);
                            string ts = dataReader.IsDBNull(1) ? "" : dataReader.GetString(1);
                            string path = dataReader.IsDBNull(2) ? "" : dataReader.GetString(2);
                            string reason = dataReader.IsDBNull(3) ? "" : dataReader.GetString(3);
                            string hashBefore = dataReader.IsDBNull(4) ? "" : dataReader.GetString(4);
                            string hashAfter = dataReader.IsDBNull(5) ? "" : dataReader.GetString(5);
                            int level = dataReader.IsDBNull(6) ? 0 : dataReader.GetInt32(6);
                            string op = dataReader.IsDBNull(7) ? "" : dataReader.GetString(7);
                            int sentRemote = dataReader.IsDBNull(8) ? 0 : dataReader.GetInt32(8);
                            string yaraRules = dataReader.IsDBNull(9) ? "" : dataReader.GetString(9);
                            string tag = dataReader.IsDBNull(10) ? "" : dataReader.GetString(10);

                            // Parse log entry
                            var logEntry = ParseLogEntry(id, ts, path, reason, hashBefore, hashAfter, level, op, sentRemote, yaraRules, tag);
                            if (logEntry != null)
                            {
                                logs.Add(logEntry);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Bỏ qua lỗi parse, tiếp tục với record tiếp theo
                            System.Diagnostics.Debug.WriteLine($"Lỗi parse log: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi đọc SQLite: {ex.Message}", ex);
            }

            return logs;
        }

        private List<AlertLogEntry> ReadLogsFromSQLite(string dbPath, int page, int pageSize)
        {
            var logs = new List<AlertLogEntry>();

            try
            {
                string connectionString = $"Data Source={dbPath};Version=3;";

                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    // Tìm bảng
                    string tableName = FindTableName(conn);
                    if (string.IsNullOrEmpty(tableName))
                    {
                        throw new Exception("Không tìm thấy bảng nào trong database.");
                    }

                    // Xây dựng query với filter và pagination
                    string selectQuery = $"SELECT id, ts, path, reason, hash_before, hash_after, level, op, sent_remote, yara_rules FROM {tableName}";

                    // Thêm điều kiện WHERE nếu có filter date
                    if (filterFromDate.HasValue || filterToDate.HasValue)
                    {
                        var conditions = new List<string>();
                        if (filterFromDate.HasValue)
                        {
                            conditions.Add($"datetime(ts) >= datetime('{filterFromDate.Value:yyyy-MM-dd}')");
                        }
                        if (filterToDate.HasValue)
                        {
                            conditions.Add($"datetime(ts) <= datetime('{filterToDate.Value:yyyy-MM-dd} 23:59:59')");
                        }
                        if (conditions.Count > 0)
                        {
                            selectQuery += " WHERE " + string.Join(" AND ", conditions);
                        }
                    }

                    // Sắp xếp và phân trang
                    selectQuery += " ORDER BY id DESC";
                    int offset = (page - 1) * pageSize;
                    selectQuery += $" LIMIT {pageSize} OFFSET {offset}";

                    using (SQLiteCommand selectCmd = new SQLiteCommand(selectQuery, conn))
                    using (SQLiteDataReader dataReader = selectCmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            try
                            {
                                // Đọc các cột trực tiếp từ database
                                int id = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0);
                                string ts = dataReader.IsDBNull(1) ? "" : dataReader.GetString(1);
                                string path = dataReader.IsDBNull(2) ? "" : dataReader.GetString(2);
                                string reason = dataReader.IsDBNull(3) ? "" : dataReader.GetString(3);
                                string hashBefore = dataReader.IsDBNull(4) ? "" : dataReader.GetString(4);
                                string hashAfter = dataReader.IsDBNull(5) ? "" : dataReader.GetString(5);
                                int level = dataReader.IsDBNull(6) ? 0 : dataReader.GetInt32(6);
                                string op = dataReader.IsDBNull(7) ? "" : dataReader.GetString(7);
                                int sentRemote = dataReader.IsDBNull(8) ? 0 : dataReader.GetInt32(8);
                                string yaraRules = dataReader.IsDBNull(9) ? "" : dataReader.GetString(9);
                                string tag = dataReader.IsDBNull(10) ? "" : dataReader.GetString(10);

                                // Parse log entry
                                var logEntry = ParseLogEntry(id, ts, path, reason, hashBefore, hashAfter, level, op, sentRemote, yaraRules, tag);
                                if (logEntry != null)
                                {
                                    logs.Add(logEntry);
                                }
                            }
                            catch (Exception ex)
                            {
                                // Bỏ qua lỗi parse, tiếp tục với record tiếp theo
                                System.Diagnostics.Debug.WriteLine($"Lỗi parse log: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi đọc SQLite: {ex.Message}", ex);
            }

            return logs;
        }

        private AlertLogEntry ParseLogEntry(int id, string ts, string path, string reason, string hashBefore, string hashAfter, int level, string op, int sentRemote, string yaraRules, string tag = "")
        {
            try
            {
                var entry = new AlertLogEntry();

                // Gán các trường từ database
                entry.Path = path ?? "";
                entry.Reason = reason ?? "";
                entry.HashBefore = hashBefore ?? "";
                entry.HashAfter = hashAfter ?? "";
                entry.Timestamp = ts ?? "";
                entry.Level = level;
                entry.Op = op ?? "";
                entry.Tag = tag ?? "";

                // Format timestamp
                if (!string.IsNullOrEmpty(entry.Timestamp))
                {
                    try
                    {
                        DateTime dt = DateTime.Parse(entry.Timestamp);
                        entry.TimestampFormatted = dt.ToLocalTime().ToString("HH:mm:ss dd/MM/yyyy");
                    }
                    catch
                    {
                        entry.TimestampFormatted = entry.Timestamp;
                    }
                }

                // Parse yara_rules (JSON string) để lấy rules và tính score
                double maxScore = 0;
                var ruleNames = new List<string>();

                if (!string.IsNullOrEmpty(yaraRules))
                {
                    try
                    {
                        var rules = JArray.Parse(yaraRules);
                        if (rules != null && rules.Count > 0)
                        {
                            foreach (var rule in rules)
                            {
                                var nameToken = rule["name"];
                                string ruleName = nameToken != null ? nameToken.ToString() : "";
                                if (!string.IsNullOrEmpty(ruleName))
                                    ruleNames.Add(ruleName);

                                var meta = rule["meta"];
                                if (meta != null)
                                {
                                    var scoreToken = meta["score"];
                                    if (scoreToken != null)
                                    {
                                        double score;
                                        if (double.TryParse(scoreToken.ToString(), out score))
                                        {
                                            if (score > maxScore)
                                                maxScore = score;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        // Nếu không parse được yara_rules, bỏ qua
                    }
                }

                entry.RuleNames = string.Join(", ", ruleNames);
                entry.Score = maxScore;

                // Tính severity từ score hoặc level (không có emoji để dễ filter)
                if (entry.Score >= 90 || entry.Level >= 2)
                    entry.Severity = "Nghiêm trọng";
                else if (entry.Score >= 75 || entry.Level == 1)
                    entry.Severity = "Cao";
                else if (entry.Score >= 50)
                    entry.Severity = "Trung bình";
                else if (entry.Score > 0 || entry.Level > 0)
                    entry.Severity = "Thấp";
                else
                    entry.Severity = "Thông tin";

                // Tạo RawJson từ dữ liệu
                var rawJsonObj = new JObject
                {
                    ["id"] = id,
                    ["ts"] = ts,
                    ["path"] = path,
                    ["reason"] = reason,
                    ["hash_before"] = hashBefore,
                    ["hash_after"] = hashAfter,
                    ["level"] = level,
                    ["op"] = op,
                    ["sent_remote"] = sentRemote,
                    ["yara_rules"] = yaraRules,
                    ["tag"] = tag
                };
                entry.RawJson = rawJsonObj.ToString();

                return entry;
            }
            catch
            {
                return null;
            }
        }

        private void ApplyRowColors()
        {
            foreach (DataGridViewRow row in dgvLogs.Rows)
            {
                var log = row.DataBoundItem as AlertLogEntry;
                if (log != null)
                {
                    // Highlight theo score
                    if (log.Score >= 90)
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230);
                        row.DefaultCellStyle.ForeColor = DangerColor;
                        row.DefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                    }
                    else if (log.Score >= 75)
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 240, 220);
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(200, 100, 0);
                        row.DefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                    }
                    else if (log.Score >= 50)
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 250, 235);
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(180, 100, 0);
                    }
                }
            }
        }

        public class AlertLogEntry
        {
            public string Path { get; set; }
            public string Reason { get; set; }
            public string HashBefore { get; set; }
            public string HashAfter { get; set; }
            public string RuleNames { get; set; }
            public double Score { get; set; }
            public string Severity { get; set; }
            public string Timestamp { get; set; }
            public string TimestampFormatted { get; set; }
            public string RawJson { get; set; }
            public int Level { get; set; }
            public string Op { get; set; }
            public string Tag { get; set; }

            public AlertLogEntry()
            {
                Path = "";
                Reason = "";
                HashBefore = "";
                HashAfter = "";
                RuleNames = "";
                Score = 0;
                Severity = "";
                Timestamp = "";
                TimestampFormatted = "";
                RawJson = "";
                Level = 0;
                Op = "";
                Tag = "";
            }
        }

        private void LoadTagList(string dbPath, List<AlertLogEntry> allLogs)
        {
            try
            {
                // Lấy danh sách tag duy nhất từ logs
                var uniqueTags = allLogs
                    .Where(log => !string.IsNullOrWhiteSpace(log.Tag))
                    .Select(log => log.Tag)
                    .Distinct()
                    .OrderBy(tag => tag)
                    .ToList();

                // Cập nhật UI thread-safe
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        string currentSelection = cmbTag.SelectedItem?.ToString();
                        cmbTag.SelectedIndexChanged -= CmbTag_SelectedIndexChanged; // Tạm thời tắt event
                        cmbTag.Items.Clear();
                        cmbTag.Items.Add("Tất cả");
                        foreach (var tag in uniqueTags)
                        {
                            cmbTag.Items.Add(tag);
                        }
                        // Khôi phục selection nếu có
                        if (!string.IsNullOrEmpty(currentSelection))
                        {
                            int index = cmbTag.Items.IndexOf(currentSelection);
                            if (index >= 0)
                                cmbTag.SelectedIndex = index;
                            else
                                cmbTag.SelectedIndex = 0;
                        }
                        else
                        {
                            cmbTag.SelectedIndex = 0;
                        }
                        cmbTag.SelectedIndexChanged += CmbTag_SelectedIndexChanged; // Bật lại event
                    }));
                }
                else
                {
                    string currentSelection = cmbTag.SelectedItem?.ToString();
                    cmbTag.SelectedIndexChanged -= CmbTag_SelectedIndexChanged; // Tạm thời tắt event
                    cmbTag.Items.Clear();
                    cmbTag.Items.Add("Tất cả");
                    foreach (var tag in uniqueTags)
                    {
                        cmbTag.Items.Add(tag);
                    }
                    // Khôi phục selection nếu có
                    if (!string.IsNullOrEmpty(currentSelection))
                    {
                        int index = cmbTag.Items.IndexOf(currentSelection);
                        if (index >= 0)
                            cmbTag.SelectedIndex = index;
                        else
                            cmbTag.SelectedIndex = 0;
                    }
                    else
                    {
                        cmbTag.SelectedIndex = 0;
                    }
                    cmbTag.SelectedIndexChanged += CmbTag_SelectedIndexChanged; // Bật lại event
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi load tag list: {ex.Message}");
            }
        }

        private async void CmbTag_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Chỉ filter khi người dùng chọn, không phải khi load
            if (cmbTag.SelectedIndex >= 0)
            {
                await BtnFilter_Click();
            }
        }

        private void lblFromDate_Click(object sender, EventArgs e)
        {

        }
    }
}
