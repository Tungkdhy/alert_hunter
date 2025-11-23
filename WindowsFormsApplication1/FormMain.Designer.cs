using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace WindowsFormsApplication1
{
    partial class FormMain
    {
        private Panel sidebar;
        private Panel panelLogo;
        private PictureBox logo;
        private Label appName;

        //private Button btnDashboard;
        private Button btnAlerts;
        private Button btnAgents;
        private Button btnHashManager;
        private Button btnDbConfig;

        private Panel header;
        private Label lblTitle;
        private Panel panelContent;

        private void InitializeComponent()
        {
            this.sidebar = new Panel();
            this.panelLogo = new Panel();
            this.logo = new PictureBox();
            this.appName = new Label();

            //this.btnDashboard = new Button();
            this.btnAlerts = new Button();
            this.btnAgents = new Button();
            this.btnHashManager = new Button();
            this.btnDbConfig = new Button();

            this.header = new Panel();
            this.lblTitle = new Label();
            this.panelContent = new Panel();

            // ---- FORM ----
            this.Text = "WebShell Detector";
            this.WindowState = FormWindowState.Maximized;
            this.Font = new Font("Segoe UI", 10);
            this.BackColor = Color.White;

            // ---- SIDEBAR ----
            sidebar.Dock = DockStyle.Left;
            sidebar.Width = 260;
            sidebar.Paint += Sidebar_Paint;

            // ---- LOGO PANEL ----
            panelLogo.Height = 70;
            panelLogo.Dock = DockStyle.Top;
            panelLogo.BackColor = Color.FromArgb(87, 74, 255);

            logo.Size = new Size(40, 40);
            logo.Location = new Point(20, 18);

            // == LOAD PNG ==
            if (System.IO.File.Exists(Application.StartupPath + "\\logo.png"))
                logo.Image = Image.FromFile(Application.StartupPath + "\\logo.png");

            logo.SizeMode = PictureBoxSizeMode.Zoom;
            logo.BackColor = Color.Transparent;

            appName.Text = "WebShell Detector";
            appName.ForeColor = Color.White;
            appName.Font = new Font("Segoe UI Semibold", 13);
            appName.Location = new Point(60, 20);
            appName.AutoSize = true;

            panelLogo.Controls.Add(logo);
            panelLogo.Controls.Add(appName);

            // ---- MENU BUTTONS ----
            //MakeMenuButton(btnDashboard, "   Xem cảnh báo", 100);
            MakeMenuButton(btnAlerts, "Cấu hình Agent", 130, GetIconImage("settings"));
            MakeMenuButton(btnAgents, "Xem cảnh báo", 180, GetIconImage("alert"));
            MakeMenuButton(btnHashManager, "Quản lý hash", 230, GetIconImage("document"));
            MakeMenuButton(btnDbConfig, "Cấu hình DB", 80, GetIconImage("database"));

            //sidebar.Controls.Add(btnDashboard);
            sidebar.Controls.Add(btnAlerts);
            sidebar.Controls.Add(btnAgents);
            sidebar.Controls.Add(btnHashManager);
            sidebar.Controls.Add(btnDbConfig);
            sidebar.Controls.Add(panelLogo);

            // ---- HEADER ----
            header.Dock = DockStyle.Top;
            header.Height = 60;
            header.BackColor = Color.White;

            lblTitle.Font = new Font("Segoe UI Semibold", 14);
            lblTitle.Location = new Point(20, 18);
            lblTitle.AutoSize = true;

            header.Controls.Add(lblTitle);

            // ---- CONTENT ----
            panelContent.Dock = DockStyle.Fill;
            panelContent.BackColor = Color.White;

            // ---- ADD TO FORM ----
            this.Controls.Add(panelContent);
            this.Controls.Add(header);
            this.Controls.Add(sidebar);
        }

        // ---- DRAW SIDEBAR GRADIENT ----
        private void Sidebar_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = sidebar.ClientRectangle;

            using (LinearGradientBrush brush = new LinearGradientBrush(
                rect,
                Color.FromArgb(91, 61, 255),
                Color.FromArgb(48, 34, 176),
                90f))
            {
                e.Graphics.FillRectangle(brush, rect);
            }
        }

        // ---- MENU BUTTON CREATOR ----
        private void MakeMenuButton(Button btn, string text, int top, Image icon = null)
        {
            btn.Text = "  " + text; // Thêm khoảng trống cho icon
            btn.Width = 220;
            btn.Height = 48;
            btn.Left = 20;
            btn.Top = top;
            btn.Padding = new Padding(8, 0, 0, 0); // Padding trái để icon lệch sang phải

            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI Semibold", 10);

            btn.ForeColor = Color.White;
            btn.BackColor = Color.Transparent;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.ImageAlign = ContentAlignment.MiddleLeft;

            // Set icon nếu có
            if (icon != null)
            {
                btn.Image = icon;
                btn.ImageAlign = ContentAlignment.MiddleLeft;
                btn.TextImageRelation = TextImageRelation.ImageBeforeText;
            }

            btn.Region = new Region(RoundedRect(btn.ClientRectangle, 12));
        }

        // ---- GET ICON IMAGE ----
        private Image GetIconImage(string iconType, Color iconColor = default(Color))
        {
            if (iconColor == default(Color))
                iconColor = Color.White;

            // Tạo icon chuyên nghiệp với kích thước lớn hơn
            Bitmap bmp = new Bitmap(24, 24);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                using (SolidBrush brush = new SolidBrush(iconColor))
                {
                    // Sử dụng Segoe MDL2 Assets - icon chuyên nghiệp của Microsoft
                    try
                    {
                        using (Font font = new Font("Segoe MDL2 Assets", 16, FontStyle.Regular))
                        {
                            string symbol = "";
                            if (iconType == "settings")
                            {
                                symbol = "\uE713"; // Settings icon (bánh răng)
                            }
                            else if (iconType == "alert")
                            {
                                symbol = "\uE7BA"; // Alert/Notification icon
                            }
                            else if (iconType == "document")
                            {
                                symbol = "\uE8A5"; // Document/File icon
                            }
                            else if (iconType == "hash")
                            {
                                symbol = "\uE72C"; // Database/Server icon - phù hợp hơn cho hash management
                            }
                            else if (iconType == "database")
                            {
                                symbol = "\uE7C3"; // Database icon (cylinder with lines)
                            }

                            if (!string.IsNullOrEmpty(symbol))
                            {
                                // Căn giữa icon
                                SizeF textSize = g.MeasureString(symbol, font);
                                float x = (bmp.Width - textSize.Width) / 2;
                                float y = (bmp.Height - textSize.Height) / 2;
                                g.DrawString(symbol, font, brush, x, y);
                            }
                        }
                    }
                    catch
                    {
                        // Fallback: Vẽ icon vector chuyên nghiệp bằng graphics
                        DrawVectorIcon(g, iconType, iconColor);
                    }
                }
            }
            return bmp;
        }

        // ---- DRAW VECTOR ICON ----
        private void DrawVectorIcon(Graphics g, string iconType, Color iconColor)
        {
            using (SolidBrush brush = new SolidBrush(iconColor))
            using (Pen pen = new Pen(iconColor, 1.5f))
            {
                pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;

                if (iconType == "settings")
                {
                    // Vẽ icon Settings (bánh răng) chuyên nghiệp
                    using (GraphicsPath gearPath = new GraphicsPath())
                    {
                        gearPath.FillMode = FillMode.Alternate;

                        int centerX = 12, centerY = 12;
                        int outerRadius = 9;
                        int innerRadius = 5;

                        // Vẽ bánh răng với 8 răng
                        PointF[] points = new PointF[16];

                        for (int i = 0; i < 8; i++)
                        {
                            double angle1 = i * Math.PI / 4 - Math.PI / 16;
                            double angle2 = i * Math.PI / 4 + Math.PI / 16;

                            // Điểm ngoài (răng)
                            points[i * 2] = new PointF(
                                (float)(centerX + outerRadius * Math.Cos(angle1)),
                                (float)(centerY + outerRadius * Math.Sin(angle1))
                            );
                            // Điểm trong (khe)
                            points[i * 2 + 1] = new PointF(
                                (float)(centerX + innerRadius * Math.Cos(angle2)),
                                (float)(centerY + innerRadius * Math.Sin(angle2))
                            );
                        }

                        gearPath.AddPolygon(points);

                        // Vẽ vòng tròn bên trong (lỗ) - sử dụng Alternate fill mode để tạo lỗ
                        gearPath.AddEllipse(centerX - 3, centerY - 3, 6, 6);

                        g.FillPath(brush, gearPath);
                    }
                }
                else if (iconType == "document")
                {
                    // Vẽ icon Document chuyên nghiệp
                    using (GraphicsPath docPath = new GraphicsPath())
                    {
                        // Hình chữ nhật với góc trên bên phải bị cắt (giống file document)
                        docPath.AddLines(new PointF[] {
                            new PointF(6, 4),
                            new PointF(14, 4),
                            new PointF(18, 8),
                            new PointF(18, 20),
                            new PointF(6, 20)
                        });
                        docPath.CloseFigure();

                        g.FillPath(brush, docPath);
                    }

                    // Vẽ góc cắt (góc trên bên phải)
                    using (Pen foldPen = new Pen(iconColor, 1.2f))
                    {
                        g.DrawLine(foldPen, 14, 4, 14, 8);
                        g.DrawLine(foldPen, 14, 8, 18, 8);
                    }

                    // Vẽ các đường ngang (nội dung document)
                    using (Pen linePen = new Pen(iconColor, 1.2f))
                    {
                        g.DrawLine(linePen, 8, 11, 16, 11);
                        g.DrawLine(linePen, 8, 14, 14, 14);
                        g.DrawLine(linePen, 8, 17, 15, 17);
                    }
                }
                else if (iconType == "hash")
                {
                    // Vẽ icon Database/Server - phù hợp với hash management
                    using (GraphicsPath dbPath = new GraphicsPath())
                    {
                        // Vẽ hình database (hình trụ với đường ngang)
                        // Vòng tròn trên
                        dbPath.AddEllipse(6, 4, 12, 4);
                        // Vòng tròn dưới
                        dbPath.AddEllipse(6, 16, 12, 4);
                        // Hình chữ nhật giữa
                        dbPath.AddRectangle(new RectangleF(6, 6, 12, 12));

                        g.FillPath(brush, dbPath);
                    }

                    // Vẽ các đường ngang (biểu thị dữ liệu)
                    using (Pen linePen = new Pen(iconColor, 1.2f))
                    {
                        g.DrawLine(linePen, 8, 9, 16, 9);
                        g.DrawLine(linePen, 8, 12, 16, 12);
                        g.DrawLine(linePen, 8, 15, 16, 15);
                    }
                }
                else if (iconType == "alert")
                {
                    // Vẽ icon Alert/Notification (tam giác cảnh báo với dấu chấm than)
                    using (GraphicsPath alertPath = new GraphicsPath())
                    {
                        // Vẽ tam giác cảnh báo
                        alertPath.AddPolygon(new PointF[] {
                            new PointF(12, 5),
                            new PointF(7, 18),
                            new PointF(17, 18)
                        });

                        g.FillPath(brush, alertPath);
                    }

                    // Vẽ dấu chấm than bên trong
                    using (Pen exclamationPen = new Pen(iconColor, 2f))
                    {
                        // Vẽ đường thẳng
                        g.DrawLine(exclamationPen, 12, 9, 12, 14);
                        // Vẽ chấm tròn
                        g.FillEllipse(brush, 11, 15, 2, 2);
                    }
                }
                else if (iconType == "database")
                {
                    // Vẽ icon Database (hình trụ với các đường ngang)
                    using (GraphicsPath dbPath = new GraphicsPath())
                    {
                        // Vòng tròn trên
                        dbPath.AddEllipse(5, 3, 14, 5);
                        // Vòng tròn dưới
                        dbPath.AddEllipse(5, 16, 14, 5);
                        // Hình chữ nhật giữa
                        dbPath.AddRectangle(new RectangleF(5, 5.5f, 14, 13));

                        g.FillPath(brush, dbPath);
                    }

                    // Vẽ các đường ngang (biểu thị dữ liệu)
                    using (Pen linePen = new Pen(iconColor, 1.5f))
                    {
                        g.DrawLine(linePen, 7, 8, 17, 8);
                        g.DrawLine(linePen, 7, 11, 17, 11);
                        g.DrawLine(linePen, 7, 14, 17, 14);
                        g.DrawLine(linePen, 7, 17, 17, 17);
                    }
                }
            }
        }

        // ---- ROUNDED CORNER ----
        private GraphicsPath RoundedRect(Rectangle rect, int radius)
        {
            int d = radius * 2;
            GraphicsPath path = new GraphicsPath();

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}
