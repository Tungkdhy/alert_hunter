using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class AlertRow : UserControl
    {
        public AlertRow()
        {
            InitializeComponent();
            this.Dock = DockStyle.Top;
        }

        public void Bind(
            string fileName,
            string filePath,
            string agentName,
            string rule,
            string severity,
            string status,
            string time)
        {
            lblFile.Text = fileName;
            lblPath.Text = filePath;

            lblAgent.Text = agentName;
            lblRule.Text = rule;

            lblTime.Text = time;
            lblStatus.Text = status;

            // ==== Severity badge ====
            switch (severity)
            {
                case "Cao":
                    panelSeverity.BackColor = Color.FromArgb(255, 226, 200);
                    lblSeverity.ForeColor = Color.FromArgb(204, 85, 0);
                    lblSeverity.Text = "⚠ Cao";
                    break;

                case "Cực cao":
                    panelSeverity.BackColor = Color.FromArgb(255, 205, 210);
                    lblSeverity.ForeColor = Color.DarkRed;
                    lblSeverity.Text = "🚨 Cực cao";
                    break;

                case "Trung bình":
                    panelSeverity.BackColor = Color.FromArgb(255, 240, 200);
                    lblSeverity.ForeColor = Color.FromArgb(180, 130, 0);
                    lblSeverity.Text = "🟡 TBình";
                    break;

                default:
                    panelSeverity.BackColor = Color.FromArgb(230, 240, 255);
                    lblSeverity.ForeColor = Color.FromArgb(30, 90, 160);
                    lblSeverity.Text = "Thấp";
                    break;
            }

            // ==== Status badge ====
            panelStatus.BackColor = Color.FromArgb(205, 170, 255);
            lblStatus.Text = status;

        }
    }
}
