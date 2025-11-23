using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace WindowsFormsApplication1
{
    public partial class FormMain : Form
    {
        private Button activeButton = null;

        public FormMain()
        {
            InitializeComponent();

            if (System.IO.File.Exists(Application.StartupPath + "\\webshell_detector_round2.ico"))
                this.Icon = new Icon(Application.StartupPath + "\\webshell_detector_round2.ico");

            AttachMenu(btnAlerts);
            AttachMenu(btnAgents);
            AttachMenu(btnHashManager);
            AttachMenu(btnDbConfig);

            SetActive(btnDbConfig);
            LoadModule(new FormDbConfig(), "Cấu hình DB");

            // Xử lý khi form được restore (thu nhỏ)
            //this.WindowStateChanged += FormMain_WindowStateChanged;
        }

        private void FormMain_WindowStateChanged(object sender, EventArgs e)
        {
            // Khi form được restore từ maximized, set kích thước = 80% màn hình
            if (this.WindowState == FormWindowState.Normal)
            {
                Rectangle screenBounds = Screen.PrimaryScreen.WorkingArea;
                int targetWidth = (int)(screenBounds.Width * 0.8);
                int targetHeight = (int)(screenBounds.Height * 0.8);

                this.Size = new Size(targetWidth, targetHeight);
                this.StartPosition = FormStartPosition.CenterScreen;
            }
        }

        private void AttachMenu(Button btn)
        {
            btn.Click += (s, e) =>
            {
                SetActive(btn);

                if (btn == btnAlerts)
                    LoadModule(new FormAgentConfigInput(), "Cấu hình Agent");

                if (btn == btnAgents)
                    LoadModule(new FormLogViewer(), "Xem cảnh báo");

                if (btn == btnHashManager)
                    LoadModule(new FormHashManager(), "Quản lý mã hash");

                if (btn == btnDbConfig)
                    LoadModule(new FormDbConfig(), "Cấu hình Database");
            };

            // Hover effect đơn giản
            btn.MouseEnter += (s, e) =>
            {
                if (btn != activeButton)
                {
                    btn.BackColor = Color.FromArgb(67, 45, 215); // #432dd7
                }
            };
            btn.MouseLeave += (s, e) =>
            {
                if (btn != activeButton)
                {
                    btn.BackColor = Color.Transparent;
                }
            };
        }

        private void SetActive(Button btn)
        {
            // Reset button cũ về trạng thái không active
            if (activeButton != null)
            {
                activeButton.BackColor = Color.Transparent;
                activeButton.ForeColor = Color.White;
                // Cập nhật icon màu trắng cho button không active
                UpdateButtonIcon(activeButton, Color.White);
            }

            // Set button mới thành active
            activeButton = btn;
            btn.BackColor = Color.White;
            btn.ForeColor = Color.FromArgb(48, 34, 176);
            // Cập nhật icon màu xanh cho button active
            UpdateButtonIcon(btn, Color.FromArgb(48, 34, 176));
        }

        private void UpdateButtonIcon(Button btn, Color iconColor)
        {
            // Lấy icon type từ button (dựa vào text hoặc tag)
            string iconType = "";
            if (btn == btnAlerts)
                iconType = "settings";
            else if (btn == btnAgents)
                iconType = "alert";
            else if (btn == btnHashManager)
                iconType = "hash";
            else if (btn == btnDbConfig)
                iconType = "database";

            if (!string.IsNullOrEmpty(iconType) && btn.Image != null)
            {
                // Tạo lại icon với màu mới
                btn.Image.Dispose();
                btn.Image = GetIconImage(iconType, iconColor);
            }
        }

        private void LoadModule(Form frm, string title)
        {
            panelContent.Controls.Clear();

            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;

            panelContent.Controls.Add(frm);
            frm.Show();

            lblTitle.Text = title;
        }
    }
}
