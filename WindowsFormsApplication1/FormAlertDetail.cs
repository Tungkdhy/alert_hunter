using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class FormAlertDetail : Form
    {
        Color Primary = Color.FromArgb(25, 55, 120);
        Color PrimaryLight = Color.FromArgb(230, 240, 255);
        Color BorderColor = Color.FromArgb(220, 220, 220);

        public FormAlertDetail()
        {
            InitializeComponent();
            
            // Icon
            if (System.IO.File.Exists(Application.StartupPath + "\\webshell_detector_round2.ico"))
                this.Icon = new Icon(Application.StartupPath + "\\webshell_detector_round2.ico");
        }

        public void LoadAlert(Form1.AlertItem alert)
        {
            if (alert == null) return;

            // Tính toán mức cảnh báo
            string sev = (alert.score >= 90) ? "🚨 Nghiêm trọng"
                : (alert.score >= 75) ? "🔥 Cao"
                : (alert.score >= 50) ? "⚠ Trung bình"
                : "🟡 Thấp";

            // Hiển thị thông tin
            lblAgentValue.Text = alert.agent_id;
            lblHostnameValue.Text = alert.hostname;
            lblMacValue.Text = alert.mac;
            lblIpValue.Text = alert.ip;
            lblFileNameValue.Text = alert.file;
            lblFilePathValue.Text = alert.path;
            lblReasonValue.Text = alert.reason;
            lblRuleValue.Text = alert.rule;
            lblSeverityValue.Text = sev;
            lblScoreValue.Text = alert.score.ToString("F1");
            
            DateTime dt;
            if (DateTime.TryParse(alert.timestamp, out dt))
                lblTimeValue.Text = dt.ToLocalTime().ToString("HH:mm:ss dd/MM/yyyy");
            else
                lblTimeValue.Text = alert.timestamp;

            // Màu nền theo mức độ
            if (alert.score >= 90)
                panelSeverity.BackColor = Color.FromArgb(255, 230, 230);
            else if (alert.score >= 75)
                panelSeverity.BackColor = Color.FromArgb(255, 240, 220);
            else if (alert.score >= 50)
                panelSeverity.BackColor = Color.FromArgb(255, 245, 220);
            else
                panelSeverity.BackColor = Color.FromArgb(255, 255, 230);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

