using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace WindowsFormsApplication1
{
    public partial class FormLogDetail : Form
    {
        Color Primary = Color.FromArgb(25, 55, 120);
        Color PrimaryLight = Color.FromArgb(230, 240, 255);
        Color BorderColor = Color.FromArgb(220, 220, 220);

        public FormLogDetail()
        {
            InitializeComponent();

            // Icon
            if (System.IO.File.Exists(Application.StartupPath + "\\webshell_detector_round2.ico"))
                this.Icon = new Icon(Application.StartupPath + "\\webshell_detector_round2.ico");
        }

        public void LoadLogData(FormLogViewer.AlertLogEntry log)
        {
            if (log == null) return;

            // Ẩn các trường không cần thiết
            lblAgent.Visible = false;
            lblAgentValue.Visible = false;
            //lblHostname.Visible = false;
            lblHostnameValue.Visible = false;
            lblMac.Visible = false;
            lblMacValue.Visible = false;
            lblIp.Visible = false;
            lblIpValue.Visible = false;
            lblOS.Visible = false;
            lblOSValue.Visible = false;
            //lblArch.Visible = false;
            lblArchValue.Visible = false;
            //lblKernel.Visible = false;
            lblKernelValue.Visible = false;

            // Hiển thị thông tin
            lblPathValue.Text = log.Path ?? "";
            lblReasonValue.Text = log.Reason ?? "";
            lblHashBeforeValue.Text = log.HashBefore ?? "";
            lblHashAfterValue.Text = log.HashAfter ?? "";
            lblRuleValue.Text = log.RuleNames ?? "";
            lblSeverityValue.Text = log.Severity ?? "";
            lblScoreValue.Text = log.Score.ToString("F1");
            lblTimeValue.Text = log.TimestampFormatted ?? "";

            // Tính toán lại chiều cao cho các label có text dài
            AdjustLabelHeight(lblPathValue);
            AdjustLabelHeight(lblReasonValue);
            AdjustLabelHeight(lblHashBeforeValue);
            AdjustLabelHeight(lblHashAfterValue);
            AdjustLabelHeight(lblRuleValue);

            // Điều chỉnh vị trí các control dựa trên chiều cao thực tế
            int currentY = 160; // Vị trí bắt đầu của Path

            currentY = SetControlPosition(lblPath, lblPathValue, currentY);
            currentY = SetControlPosition(lblReason, lblReasonValue, currentY);
            currentY = SetControlPosition(lblHashBefore, lblHashBeforeValue, currentY);
            currentY = SetControlPosition(lblHashAfter, lblHashAfterValue, currentY);
            currentY = SetControlPosition(lblRule, lblRuleValue, currentY);
            currentY = SetControlPosition(lblTime, lblTimeValue, currentY);
            currentY = SetControlPosition(lblScore, lblScoreValue, currentY);

            // Tính toán chiều cao cần thiết cho form
            int requiredHeight = currentY + 40; // 40px padding dưới cùng
            int minHeight = 400; // Chiều cao tối thiểu
            int maxHeight = Screen.PrimaryScreen.WorkingArea.Height - 100; // Không vượt quá màn hình
            
            // Điều chỉnh chiều cao form
            int formHeight = Math.Max(minHeight, Math.Min(requiredHeight, maxHeight));
            this.ClientSize = new Size(this.ClientSize.Width, formHeight);
            
            // Điều chỉnh chiều cao panelMain
            panelMain.Size = new Size(panelMain.Size.Width, formHeight);
            
            // Nếu nội dung dài hơn chiều cao tối đa, bật scroll
            if (requiredHeight > maxHeight)
            {
                panelMain.AutoScroll = true;
            }

            // Màu nền theo mức độ
            if (log.Score >= 90)
                panelSeverity.BackColor = Color.FromArgb(255, 230, 230);
            else if (log.Score >= 75)
                panelSeverity.BackColor = Color.FromArgb(255, 240, 220);
            else if (log.Score >= 50)
                panelSeverity.BackColor = Color.FromArgb(255, 245, 220);
            else
                panelSeverity.BackColor = Color.FromArgb(255, 255, 230);

            // Parse và hiển thị JSON đẹp hơn
            if (!string.IsNullOrEmpty(log.RawJson))
            {
                try
                {
                    var formatted = JToken.Parse(log.RawJson).ToString(Newtonsoft.Json.Formatting.Indented);
                    //txtJson.Text = formatted;
                }
                catch
                {
                    //txtJson.Text = log.RawJson;
                }
            }
        }

        private void AdjustLabelHeight(Label label)
        {
            if (string.IsNullOrEmpty(label.Text))
            {
                label.Height = label.Font.Height + 4;
                return;
            }

            using (Graphics g = label.CreateGraphics())
            {
                SizeF size = g.MeasureString(label.Text, label.Font, label.Width);
                int height = (int)Math.Ceiling(size.Height) + 4;
                label.Height = Math.Max(height, label.Font.Height + 4);
            }
        }

        private int SetControlPosition(Label label, Label valueLabel, int startY)
        {
            int labelHeight = Math.Max(label.Height, valueLabel.Height);
            label.Location = new Point(label.Location.X, startY);
            valueLabel.Location = new Point(valueLabel.Location.X, startY);
            return startY + labelHeight + 10; // 10px spacing
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblHostname_Click(object sender, EventArgs e)
        {

        }
    }
}

