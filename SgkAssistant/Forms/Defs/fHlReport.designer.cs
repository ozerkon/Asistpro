namespace SgkAssistant.Forms.Defs
{
    partial class FHlReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FHlReport));
            this.textReport = new Telerik.WinControls.UI.RadRichTextEditor();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCopyText = new Telerik.WinControls.UI.RadButton();
            this.btnClose = new Telerik.WinControls.UI.RadButton();
            this.lblMessage = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.textReport)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnCopyText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // textReport
            // 
            this.textReport.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(189)))), ((int)(((byte)(232)))));
            this.tableLayoutPanel1.SetColumnSpan(this.textReport, 5);
            this.textReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textReport.Location = new System.Drawing.Point(3, 3);
            this.textReport.Name = "textReport";
            this.textReport.SelectionFill = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(78)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.textReport.SelectionStroke = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(236)))), ((int)(((byte)(248)))));
            this.textReport.Size = new System.Drawing.Size(1186, 496);
            this.textReport.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnCopyText, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.textReport, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblMessage, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1192, 567);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // btnCopyText
            // 
            this.btnCopyText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCopyText.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnCopyText.Image = global::SgkAssistant.Properties.Resources.copy_24;
            this.btnCopyText.Location = new System.Drawing.Point(424, 505);
            this.btnCopyText.Name = "btnCopyText";
            this.btnCopyText.Size = new System.Drawing.Size(144, 28);
            this.btnCopyText.TabIndex = 1;
            this.btnCopyText.Text = "KOPYALA";
            this.btnCopyText.Click += new System.EventHandler(this.btnCopyText_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnClose.Image = global::SgkAssistant.Properties.Resources.cancel_24;
            this.btnClose.Location = new System.Drawing.Point(624, 505);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(144, 28);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "KAPAT";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.ForeColor = System.Drawing.Color.Red;
            this.lblMessage.Location = new System.Drawing.Point(3, 536);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(44, 17);
            this.lblMessage.TabIndex = 3;
            this.lblMessage.Text = "Mesaj";
            this.lblMessage.TextChanged += new System.EventHandler(this.lblMessage_TextChanged);
            // 
            // FHlReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1192, 567);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1200, 600);
            this.MinimumSize = new System.Drawing.Size(1200, 600);
            this.Name = "FHlReport";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(1200, 600);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HİZMET LİSTESİ ÖNCEKİ DÖNEM - SONRAKİ DÖNEM TUTARSIZLIKLARI";
            this.Load += new System.EventHandler(this.fHlReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.textReport)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnCopyText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Telerik.WinControls.UI.RadButton btnClose;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadButton btnCopyText;
        private Telerik.WinControls.UI.RadRichTextEditor textReport;
        private System.Windows.Forms.Label lblMessage;
    }
}
