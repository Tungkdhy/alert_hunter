using System;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Color Primary = Color.FromArgb(25, 55, 120);
        Color PrimaryLight = Color.FromArgb(230, 240, 255);
        Color BorderColor = Color.FromArgb(220, 220, 220);

        private List<AlertItem> _currentAlerts = new List<AlertItem>();
        private Dictionary<int, Color[]> _rowOriginalColors = new Dictionary<int, Color[]>();

        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;

            // Icon
            if (System.IO.File.Exists(Application.StartupPath + "\\webshell_detector_round2.ico"))
                this.Icon = new Icon(Application.StartupPath + "\\webshell_detector_round2.ico");
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10);
            this.WindowState = FormWindowState.Maximized;

            // Style UI
            StyleHeader();
            StyleButtons();
            StyleDatePickers();
            StyleDataGridView();

            lblMac.Text = "MAC: " + GetMacAddress();

            dtFrom.Value = DateTime.Now.AddHours(-24);
            dtTo.Value = DateTime.Now;

            await LoadAlerts();

            btnRefresh.Click += async delegate { await LoadAlerts(); };
            btnFilterTime.Click += async delegate { await LoadAlerts(); };
            btnOpenLog.Click += delegate
            {
                //FormLogViewer log = new FormLogViewer();
                //log.Show();
            };

            // Event handler để hiển thị chi tiết khi click vào hàng
            dgvAlerts.CellDoubleClick += DgvAlerts_CellDoubleClick;
            dgvAlerts.KeyDown += DgvAlerts_KeyDown;
            dgvAlerts.DataBindingComplete += DgvAlerts_DataBindingComplete;

            // Thêm hover effect
            dgvAlerts.CellMouseEnter += DgvAlerts_CellMouseEnter;
            dgvAlerts.CellMouseLeave += DgvAlerts_CellMouseLeave;
        }

        // ============================================================
        //  GET MAC
        // ============================================================
        private string GetMacAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    string raw = nic.GetPhysicalAddress().ToString();
                    if (raw.Length >= 12)
                        return string.Join(":", Enumerable.Range(0, 6).Select(i => raw.Substring(i * 2, 2))).ToUpper();
                }
            }
            return "Unknown";
        }

        // ============================================================
        //  LOAD ALERTS
        // ============================================================
        private async Task LoadAlerts()
        {
            dgvAlerts.DataSource = null;

            var alerts = await Task.Run(() => MockAlerts());

            if (alerts == null || alerts.Count == 0) return;

            DateTime from = dtFrom.Value;
            DateTime to = dtTo.Value;

            var filteredAlerts = alerts.Where(a =>
            {
                DateTime t;
                if (!DateTime.TryParse(a.timestamp, out t))
                    return false;
                t = t.ToLocalTime();
                return t >= from && t <= to;
            }).ToList();

            var data = filteredAlerts.Select(a =>
            {
                string sev = (a.score >= 90) ? "🚨 Nghiêm trọng"
                    : (a.score >= 75) ? "🔥 Cao"
                    : (a.score >= 50) ? "⚠ Trung bình"
                    : "🟡 Thấp";

                return new
                {
                    Agent = a.agent_id,
                    Máy = a.hostname,
                    MAC = a.mac,
                    IP = a.ip,
                    Tên_tệp = a.file,
                    Đường_dẫn = a.path,
                    Lý_do = a.reason,
                    Thời_gian = FormatTime(a.timestamp),
                    Rule = a.rule,
                    Mức_cảnh_báo = sev
                };
            }).ToList();

            dgvAlerts.DataSource = data;

            // Lưu danh sách alerts đã lọc để có thể truy cập sau
            _currentAlerts = filteredAlerts;

            ApplyRowColors();
        }

        private string FormatTime(object rawTime)
        {
            DateTime dt;
            if (DateTime.TryParse(rawTime.ToString(), out dt))
                return dt.ToLocalTime().ToString("HH:mm dd/MM/yyyy");
            return rawTime.ToString();
        }

        private void ApplyRowColors()
        {
            _rowOriginalColors.Clear();

            foreach (DataGridViewRow row in dgvAlerts.Rows)
            {
                string sev = Convert.ToString(row.Cells["Mức_cảnh_báo"].Value);
                bool isAlternating = row.Index % 2 == 1;
                Color backColor, foreColor;

                // Màu nền theo mức độ nghiêm trọng, nhưng vẫn giữ alternating pattern
                if (sev.Contains("Nghiêm") || sev.Contains("Cao"))
                {
                    backColor = isAlternating
                        ? Color.FromArgb(255, 240, 240)
                        : Color.FromArgb(255, 245, 245);
                    foreColor = Color.FromArgb(180, 0, 0);
                }
                else if (sev.Contains("Trung"))
                {
                    backColor = isAlternating
                        ? Color.FromArgb(255, 250, 235)
                        : Color.FromArgb(255, 252, 242);
                    foreColor = Color.FromArgb(180, 100, 0);
                }
                else
                {
                    backColor = isAlternating
                        ? Color.FromArgb(255, 255, 240)
                        : Color.FromArgb(255, 255, 248);
                    foreColor = Color.FromArgb(150, 120, 0);
                }

                // Lưu màu gốc để khôi phục khi hover
                Color[] originalColors = new Color[row.Cells.Count];
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    originalColors[i] = backColor;
                }
                _rowOriginalColors[row.Index] = originalColors;

                // Áp dụng màu
                row.DefaultCellStyle.BackColor = backColor;
                row.DefaultCellStyle.ForeColor = foreColor;

                // Selection colors vẫn giữ nguyên
                row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 235, 255);
                row.DefaultCellStyle.SelectionForeColor = Color.FromArgb(25, 55, 120);
            }
        }

        // ============================================================
        //  STYLING UI
        // ============================================================

        private void StyleHeader()
        {
            lblMac.Font = new Font("Segoe UI Semibold", 11);
            lblMac.ForeColor = Primary;
        }

        private void StyleButtons()
        {
            StyleButton(btnOpenLog, Primary, Color.White);
            StyleButton(btnRefresh, Color.FromArgb(20, 120, 220), Color.White);
            StyleButtonOutline(btnFilterTime, Primary);
        }

        private void StyleButton(Button btn, Color back, Color text)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = back;
            btn.ForeColor = text;
            btn.Font = new Font("Segoe UI Semibold", 10);

            RoundButton(btn, 8);
        }

        private void StyleButtonOutline(Button btn, Color borderColor)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 2;
            btn.FlatAppearance.BorderColor = borderColor;
            btn.BackColor = Color.White;
            btn.ForeColor = borderColor;
            btn.Font = new Font("Segoe UI Semibold", 10);

            RoundButton(btn, 8);
        }

        private void RoundButton(Button btn, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            btn.Region = new Region(path);
        }

        private void StyleDatePickers()
        {
            dtFrom.CalendarMonthBackground = PrimaryLight;
            dtFrom.Font = new Font("Segoe UI", 10);

            dtTo.CalendarMonthBackground = PrimaryLight;
            dtTo.Font = new Font("Segoe UI", 10);
        }

        private void StyleDataGridView()
        {
            // Background và Border
            dgvAlerts.BackgroundColor = Color.FromArgb(248, 249, 250);
            dgvAlerts.BorderStyle = BorderStyle.None;
            dgvAlerts.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvAlerts.EnableHeadersVisualStyles = false;

            // Header Styling - Chuyên nghiệp hơn
            dgvAlerts.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvAlerts.ColumnHeadersDefaultCellStyle.BackColor = Primary;
            dgvAlerts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvAlerts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10.5f);
            dgvAlerts.ColumnHeadersDefaultCellStyle.Padding = new Padding(12, 10, 12, 10);
            dgvAlerts.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvAlerts.ColumnHeadersHeight = 45;
            dgvAlerts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Default Cell Styling
            dgvAlerts.DefaultCellStyle.Font = new Font("Segoe UI", 10f);
            dgvAlerts.DefaultCellStyle.Padding = new Padding(12, 10, 12, 10);
            dgvAlerts.DefaultCellStyle.BackColor = Color.White;
            dgvAlerts.DefaultCellStyle.ForeColor = Color.FromArgb(50, 50, 50);
            dgvAlerts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 240, 255);
            dgvAlerts.DefaultCellStyle.SelectionForeColor = Color.FromArgb(25, 55, 120);
            dgvAlerts.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvAlerts.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // Alternating Rows - Dễ đọc hơn
            dgvAlerts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(252, 253, 254);
            dgvAlerts.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(50, 50, 50);
            dgvAlerts.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 235, 255);
            dgvAlerts.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(25, 55, 120);

            // Grid Lines - Subtle horizontal lines only
            dgvAlerts.GridColor = Color.FromArgb(240, 242, 245);
            dgvAlerts.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            // dgvAlerts.AdvancedCellBorderStyle.All = DataGridViewAdvancedBorderStyle.None;
            //dgvAlerts.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedBorderStyle.Single;

            // Row Styling
            dgvAlerts.RowTemplate.Height = 42;
            dgvAlerts.RowTemplate.DefaultCellStyle.Padding = new Padding(12, 10, 12, 10);

            // Selection Mode
            dgvAlerts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAlerts.MultiSelect = false;

            // Other Settings
            dgvAlerts.ReadOnly = true;
            dgvAlerts.RowHeadersVisible = false;
            dgvAlerts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAlerts.AllowUserToResizeRows = false;
            dgvAlerts.AllowUserToResizeColumns = true;
            dgvAlerts.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            // Scrollbar Styling
            dgvAlerts.ScrollBars = ScrollBars.Both;

            // Enable Double Buffering for smoother scrolling
            SetDoubleBuffered(dgvAlerts);

            // Custom Paint Event cho Header
            dgvAlerts.Paint += DgvAlerts_Paint;
        }

        private void DgvAlerts_Paint(object sender, PaintEventArgs e)
        {
            // Vẽ border cho toàn bộ grid
            Rectangle rect = dgvAlerts.ClientRectangle;
            using (Pen pen = new Pen(Color.FromArgb(220, 220, 220), 1))
            {
                e.Graphics.DrawRectangle(pen, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
            }
        }

        // Helper method để enable double buffering
        private void SetDoubleBuffered(DataGridView dgv)
        {
            System.Reflection.PropertyInfo prop = typeof(Control).GetProperty(
                "DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            prop.SetValue(dgv, true, null);
        }

        // ============================================================
        //  EVENT HANDLERS - CHI TIẾT CẢNH BÁO
        // ============================================================

        private void DgvAlerts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            ShowAlertDetail(e.RowIndex);
        }

        private void DgvAlerts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && dgvAlerts.SelectedRows.Count > 0)
            {
                int rowIndex = dgvAlerts.SelectedRows[0].Index;
                ShowAlertDetail(rowIndex);
            }
        }

        private void DgvAlerts_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Gán Tag cho mỗi row sau khi binding hoàn tất
            if (_currentAlerts != null && _currentAlerts.Count > 0)
            {
                for (int i = 0; i < dgvAlerts.Rows.Count && i < _currentAlerts.Count; i++)
                {
                    dgvAlerts.Rows[i].Tag = _currentAlerts[i];
                }
            }

            // Apply colors sau khi binding
            ApplyRowColors();
        }

        private void DgvAlerts_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = dgvAlerts.Rows[e.RowIndex];
                if (!row.Selected)
                {
                    // Highlight row khi hover (subtle effect)
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        Color originalColor = cell.Style.BackColor;
                        if (originalColor.A > 0 && originalColor != Color.Transparent)
                        {
                            // Làm sáng màu nền một chút để tạo hover effect
                            cell.Style.BackColor = Color.FromArgb(
                                Math.Min(255, originalColor.R + 8),
                                Math.Min(255, originalColor.G + 8),
                                Math.Min(255, originalColor.B + 8)
                            );
                        }
                    }
                }
            }
        }

        private void DgvAlerts_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = dgvAlerts.Rows[e.RowIndex];
                if (!row.Selected && _rowOriginalColors.ContainsKey(e.RowIndex))
                {
                    // Khôi phục màu gốc từ dictionary
                    Color[] originalColors = _rowOriginalColors[e.RowIndex];
                    for (int i = 0; i < row.Cells.Count && i < originalColors.Length; i++)
                    {
                        row.Cells[i].Style.BackColor = originalColors[i];
                    }
                }
            }
        }

        private void ShowAlertDetail(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvAlerts.Rows.Count) return;

            DataGridViewRow row = dgvAlerts.Rows[rowIndex];
            AlertItem alert = row.Tag as AlertItem;

            if (alert == null) return;

            FormAlertDetail detailForm = new FormAlertDetail();
            detailForm.LoadAlert(alert);
            detailForm.ShowDialog(this);
        }

        // ============================================================
        //  MOCK DATA
        // ============================================================

        private List<AlertItem> MockAlerts()
        {
            return new List<AlertItem>
            {
                new AlertItem{ agent_id="AG01", hostname="Server-1", mac="00:11:22:33:44:55",
                    ip="192.168.1.10", file="webshell.php", path="C:\\inetpub\\wwwroot\\webshell.php",
                    reason="Phát hiện chuỗi mã độc PHP", timestamp=DateTime.Now.AddHours(-2).ToString(),
                    rule="PHP_Webshell_Generic", score=85 },

                new AlertItem{ agent_id="AG02", hostname="Server-2", mac="00:11:22:33:66:99",
                    ip="192.168.1.11", file="test.php", path="C:\\web\\test.php",
                    reason="Tệp nghi ngờ upload", timestamp=DateTime.Now.AddHours(-1).ToString(),
                    rule="PHP_Upload_Suspicious", score=55 }
            };
        }

        public class AlertItem
        {
            public string agent_id;
            public string hostname;
            public string mac;
            public string ip;
            public string file;
            public string path;
            public string reason;
            public string timestamp;
            public string rule;
            public double score;
        }
    }
}
