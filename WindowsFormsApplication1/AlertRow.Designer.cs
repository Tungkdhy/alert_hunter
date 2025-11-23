
using System;
using System.Drawing;
using System.Windows.Forms;
namespace WindowsFormsApplication1
{
    partial class AlertRow
    {
        private Label lblFile;
        private Label lblPath;
        private Label lblAgent;
        private Label lblRule;
        private Label lblTime;

        private Panel panelSeverity;
        private Label lblSeverity;

        private Panel panelStatus;
        private Label lblStatus;

        private Button btnAction;

        private void InitializeComponent()
        {
            this.table = new System.Windows.Forms.TableLayoutPanel();
            this.pnlFile = new System.Windows.Forms.Panel();
            this.lblPath = new System.Windows.Forms.Label();
            this.lblFile = new System.Windows.Forms.Label();
            this.lblAgent = new System.Windows.Forms.Label();
            this.lblRule = new System.Windows.Forms.Label();
            this.panelSeverity = new System.Windows.Forms.Panel();
            this.lblSeverity = new System.Windows.Forms.Label();
            this.panelStatus = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.btnAction = new System.Windows.Forms.Button();
            this.table.SuspendLayout();
            this.pnlFile.SuspendLayout();
            this.panelSeverity.SuspendLayout();
            this.panelStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // table
            // 
            this.table.BackColor = System.Drawing.Color.White;
            this.table.ColumnCount = 7;
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 260F));
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.table.Controls.Add(this.pnlFile, 0, 0);
            this.table.Controls.Add(this.lblAgent, 1, 0);
            this.table.Controls.Add(this.lblRule, 2, 0);
            this.table.Controls.Add(this.panelSeverity, 3, 0);
            this.table.Controls.Add(this.panelStatus, 4, 0);
            this.table.Controls.Add(this.lblTime, 5, 0);
            this.table.Controls.Add(this.btnAction, 6, 0);
            this.table.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table.Location = new System.Drawing.Point(0, 0);
            this.table.Name = "table";
            this.table.RowCount = 1;
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.table.Size = new System.Drawing.Size(150, 65);
            this.table.TabIndex = 0;
            // 
            // pnlFile
            // 
            this.pnlFile.Controls.Add(this.lblPath);
            this.pnlFile.Controls.Add(this.lblFile);
            this.pnlFile.Location = new System.Drawing.Point(3, 3);
            this.pnlFile.Name = "pnlFile";
            this.pnlFile.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.pnlFile.Size = new System.Drawing.Size(200, 59);
            this.pnlFile.TabIndex = 0;
            // 
            // lblPath
            // 
            this.lblPath.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPath.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblPath.ForeColor = System.Drawing.Color.Gray;
            this.lblPath.Location = new System.Drawing.Point(0, 35);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(200, 23);
            this.lblPath.TabIndex = 0;
            // 
            // lblFile
            // 
            this.lblFile.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFile.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.lblFile.Location = new System.Drawing.Point(0, 12);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(200, 23);
            this.lblFile.TabIndex = 1;
            // 
            // lblAgent
            // 
            this.lblAgent.Location = new System.Drawing.Point(263, 0);
            this.lblAgent.Name = "lblAgent";
            this.lblAgent.Size = new System.Drawing.Size(100, 23);
            this.lblAgent.TabIndex = 1;
            // 
            // lblRule
            // 
            this.lblRule.Location = new System.Drawing.Point(413, 0);
            this.lblRule.Name = "lblRule";
            this.lblRule.Size = new System.Drawing.Size(100, 23);
            this.lblRule.TabIndex = 2;
            // 
            // panelSeverity
            // 
            this.panelSeverity.Controls.Add(this.lblSeverity);
            this.panelSeverity.Location = new System.Drawing.Point(643, 3);
            this.panelSeverity.Name = "panelSeverity";
            this.panelSeverity.Size = new System.Drawing.Size(94, 59);
            this.panelSeverity.TabIndex = 3;
            // 
            // lblSeverity
            // 
            this.lblSeverity.Location = new System.Drawing.Point(0, 0);
            this.lblSeverity.Name = "lblSeverity";
            this.lblSeverity.Size = new System.Drawing.Size(100, 23);
            this.lblSeverity.TabIndex = 0;
            // 
            // panelStatus
            // 
            this.panelStatus.Controls.Add(this.lblStatus);
            this.panelStatus.Location = new System.Drawing.Point(743, 3);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(124, 59);
            this.panelStatus.TabIndex = 4;
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(0, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(100, 23);
            this.lblStatus.TabIndex = 0;
            // 
            // lblTime
            // 
            this.lblTime.Location = new System.Drawing.Point(873, 0);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(100, 23);
            this.lblTime.TabIndex = 5;
            // 
            // btnAction
            // 
            this.btnAction.FlatAppearance.BorderSize = 0;
            this.btnAction.Location = new System.Drawing.Point(1023, 3);
            this.btnAction.Name = "btnAction";
            this.btnAction.Size = new System.Drawing.Size(75, 23);
            this.btnAction.TabIndex = 6;
            // 
            // AlertRow
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.table);
            this.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.Name = "AlertRow";
            this.Size = new System.Drawing.Size(150, 65);
            this.table.ResumeLayout(false);
            this.pnlFile.ResumeLayout(false);
            this.panelSeverity.ResumeLayout(false);
            this.panelStatus.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private TableLayoutPanel table;
        private Panel pnlFile;
    }
}
