namespace WindowsFormsApplication1
{
    partial class FormHashManager
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtDbPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.DataGridView dgvHashes;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblPageInfo;
        private System.Windows.Forms.Label lblFilterWebserver;
        private FlatCombo cmbWebserver;
        private System.Windows.Forms.Button btnClearFilter;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtDbPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.dgvHashes = new System.Windows.Forms.DataGridView();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.lblPageInfo = new System.Windows.Forms.Label();
            this.lblFilterWebserver = new System.Windows.Forms.Label();
            this.cmbWebserver = new FlatCombo();
            this.btnClearFilter = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHashes)).BeginInit();
            this.SuspendLayout();
            // 
            // txtDbPath
            // 
            this.txtDbPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDbPath.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtDbPath.Location = new System.Drawing.Point(20, 20);
            this.txtDbPath.Multiline = true;
            this.txtDbPath.Name = "txtDbPath";
            this.txtDbPath.Size = new System.Drawing.Size(700, 33);
            this.txtDbPath.TabIndex = 0;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnBrowse.Location = new System.Drawing.Point(730, 18);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(120, 35);
            this.btnBrowse.TabIndex = 5;
            this.btnBrowse.Text = "📁 Chọn DB";
            this.btnBrowse.UseVisualStyleBackColor = true;
            // 
            // btnReload
            // 
            this.btnReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReload.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnReload.Location = new System.Drawing.Point(860, 18);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(120, 35);
            this.btnReload.TabIndex = 4;
            this.btnReload.Text = "🔄 Nạp lại";
            this.btnReload.UseVisualStyleBackColor = true;
            // 
            // lblFilterWebserver
            // 
            this.lblFilterWebserver.AutoSize = true;
            this.lblFilterWebserver.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblFilterWebserver.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lblFilterWebserver.Location = new System.Drawing.Point(20, 58);
            this.lblFilterWebserver.Name = "lblFilterWebserver";
            this.lblFilterWebserver.Size = new System.Drawing.Size(108, 23);
            this.lblFilterWebserver.TabIndex = 6;
            this.lblFilterWebserver.Text = "Folder webserver:";
            // 
            // cmbWebserver
            // 
            this.cmbWebserver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWebserver.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbWebserver.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbWebserver.FormattingEnabled = true;
            this.cmbWebserver.ItemHeight = 23;
            this.cmbWebserver.Location = new System.Drawing.Point(23, 84);
            this.cmbWebserver.Name = "cmbWebserver";
            this.cmbWebserver.Size = new System.Drawing.Size(300, 31);
            this.cmbWebserver.TabIndex = 7;
            // 
            // btnClearFilter
            // 
            this.btnClearFilter.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnClearFilter.Location = new System.Drawing.Point(340, 84);
            this.btnClearFilter.Name = "btnClearFilter";
            this.btnClearFilter.Size = new System.Drawing.Size(110, 30);
            this.btnClearFilter.TabIndex = 11;
            this.btnClearFilter.Text = "Xóa lọc";
            this.btnClearFilter.UseVisualStyleBackColor = true;
            // 
            // dgvHashes
            // 
            this.dgvHashes.AllowUserToAddRows = false;
            this.dgvHashes.AllowUserToDeleteRows = false;
            this.dgvHashes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvHashes.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.dgvHashes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvHashes.Location = new System.Drawing.Point(20, 125);
            this.dgvHashes.MultiSelect = false;
            this.dgvHashes.Name = "dgvHashes";
            this.dgvHashes.ReadOnly = true;
            this.dgvHashes.RowHeadersVisible = false;
            this.dgvHashes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHashes.Size = new System.Drawing.Size(960, 412);
            this.dgvHashes.TabIndex = 3;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblStatus.Location = new System.Drawing.Point(20, 590);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.lblStatus.Size = new System.Drawing.Size(960, 33);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Sẵn sàng";
            // 
            // btnPrevious
            // 
            this.btnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrevious.Enabled = false;
            this.btnPrevious.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnPrevious.Location = new System.Drawing.Point(20, 547);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(100, 35);
            this.btnPrevious.TabIndex = 3;
            this.btnPrevious.Text = "◀ Trước";
            this.btnPrevious.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Enabled = false;
            this.btnNext.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnNext.Location = new System.Drawing.Point(880, 547);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(100, 35);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "Sau ▶";
            this.btnNext.UseVisualStyleBackColor = true;
            // 
            // lblPageInfo
            // 
            this.lblPageInfo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblPageInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblPageInfo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPageInfo.Location = new System.Drawing.Point(400, 547);
            this.lblPageInfo.Name = "lblPageInfo";
            this.lblPageInfo.Size = new System.Drawing.Size(200, 35);
            this.lblPageInfo.TabIndex = 2;
            this.lblPageInfo.Text = "Trang 0 / 0";
            this.lblPageInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormHashManager
            // 
            this.ClientSize = new System.Drawing.Size(1000, 640);
            this.Controls.Add(this.btnClearFilter);
            this.Controls.Add(this.cmbWebserver);
            this.Controls.Add(this.lblFilterWebserver);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.lblPageInfo);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.dgvHashes);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtDbPath);
            this.Name = "FormHashManager";
            ((System.ComponentModel.ISupportInitialize)(this.dgvHashes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

