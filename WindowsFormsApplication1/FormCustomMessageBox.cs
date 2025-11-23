using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class FormCustomMessageBox : Form
    {
        private Label lblTitle;
        private Label lblMessage;
        private Button btnOK;
        private Button btnCancel;
        private PictureBox picIcon;

        private static DialogResult _result = DialogResult.None;

        Color Header1 = Color.FromArgb(121, 64, 205);
        Color Header2 = Color.FromArgb(162, 93, 245);

        public FormCustomMessageBox(string message, string title, MessageBoxIcon iconType, bool showCancel)
        {
            InitializeComponent();  // Designer form
            BuildUI(message, title, iconType, showCancel);
        }

        private void BuildUI(string message, string title, MessageBoxIcon iconType, bool showCancel)
        {
            Panel container = new Panel();
            container.BackColor = Color.White;
            container.Location = new Point(0, 0);
            container.Size = this.ClientSize;
            container.Paint += (s, e) => DrawRoundedBorder(e.Graphics, container);
            this.Controls.Add(container);

            // ===== Header Gradient =====
            Panel header = new Panel();
            header.Location = new Point(0, 0);
            header.Size = new Size(480, 55);
            header.Paint += DrawHeaderGradient;
            container.Controls.Add(header);

            lblTitle = new Label()
            {
                Text = title,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 12),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(20, 10),
                Size = new Size(420, 35)
            };
            lblTitle.BackColor = Color.Transparent;

            header.Controls.Add(lblTitle);

            // ===== Icon =====
            picIcon = new PictureBox()
            {
                Size = new Size(40, 40),
                Location = new Point(20, 75),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = GetIcon(iconType)
            };
            container.Controls.Add(picIcon);

            // ===== Message =====
            lblMessage = new Label()
            {
                Text = message,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Black,
                Location = new Point(70, 80),
                Size = new Size(380, 80)
            };
            container.Controls.Add(lblMessage);

            // ===== Buttons =====
            btnOK = new Button()
            {
                Text = "OK",
                BackColor = Color.FromArgb(25, 55, 120),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(90, 35),
                Location = new Point(250, 170),
                Font = new Font("Segoe UI Semibold", 10)
            };
            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.Click += (s, e) => { _result = DialogResult.OK; Close(); };
            container.Controls.Add(btnOK);

            btnCancel = new Button()
            {
                Text = "Hủy",
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(90, 35),
                Location = new Point(350, 170),
                Font = new Font("Segoe UI Semibold", 10),
                Visible = showCancel
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => { _result = DialogResult.Cancel; Close(); };
            container.Controls.Add(btnCancel);
        }

        // =======================
        //  STATIC CALLS
        // =======================
        public static DialogResult Show(IWin32Window owner, string msg, string title, MessageBoxIcon icon)
        {
            _result = DialogResult.None;
            FormCustomMessageBox frm = new FormCustomMessageBox(msg, title, icon, true); // ⭐ ENABLE Cancel
            frm.ShowDialog(owner);
            return _result;
        }

        public static DialogResult ShowWithCancel(IWin32Window owner, string msg, string title, MessageBoxIcon icon)
        {
            _result = DialogResult.None;
            FormCustomMessageBox frm = new FormCustomMessageBox(msg, title, icon, true);
            frm.ShowDialog(owner);
            return _result;
        }

        // =======================
        //  PAINT FUNCTIONS
        // =======================
        private void DrawHeaderGradient(object sender, PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, this.Width, 55);
            using (LinearGradientBrush br = new LinearGradientBrush(rect, Header1, Header2, 90f))
            {
                e.Graphics.FillRectangle(br, rect);
            }
        }

        private Image GetIcon(MessageBoxIcon icon)
        {
            switch (icon)
            {
                case MessageBoxIcon.Warning: return SystemIcons.Warning.ToBitmap();
                case MessageBoxIcon.Error: return SystemIcons.Error.ToBitmap();
                case MessageBoxIcon.Question: return SystemIcons.Question.ToBitmap();
                default: return SystemIcons.Information.ToBitmap();
            }
        }

        private void DrawRoundedBorder(Graphics g, Control c)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, c.Width - 1, c.Height - 1);
            using (GraphicsPath path = RoundedRect(rect, 20))
            using (Pen pen = new Pen(Color.LightGray, 1))
                g.DrawPath(pen, path);
        }

        private GraphicsPath RoundedRect(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(rect);   // ⭐ Vẽ hình chữ nhật, KHÔNG BO GÓC
            return path;
        }
    }
}
