namespace SgkAssistant.Forms.Defs
{
    partial class FHlp
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FHlp));
            this.rgvMossip = new Telerik.WinControls.UI.RadGridView();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.pnlMessage = new System.Windows.Forms.Panel();
            this.pnlLblMessage = new System.Windows.Forms.Panel();
            this.lblMessage = new Telerik.WinControls.UI.RadLabel();
            this.pnlBtnOpenFile = new System.Windows.Forms.Panel();
            this.btnOpenFile = new Telerik.WinControls.UI.RadButton();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnExcelHLP = new Telerik.WinControls.UI.RadButton();
            this.btnAnalyzeHL = new Telerik.WinControls.UI.RadButton();
            this.btnPrintHLP = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.rgvMossip)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvMossip.MasterTemplate)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.pnlMessage.SuspendLayout();
            this.pnlLblMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblMessage)).BeginInit();
            this.pnlBtnOpenFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnOpenFile)).BeginInit();
            this.pnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnExcelHLP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAnalyzeHL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPrintHLP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // rgvMossip
            // 
            this.rgvMossip.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rgvMossip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgvMossip.Location = new System.Drawing.Point(0, 62);
            // 
            // 
            // 
            this.rgvMossip.MasterTemplate.AllowAddNewRow = false;
            this.rgvMossip.MasterTemplate.AllowDeleteRow = false;
            this.rgvMossip.MasterTemplate.AllowEditRow = false;
            this.rgvMossip.MasterTemplate.AllowRowResize = false;
            this.rgvMossip.MasterTemplate.AllowSearchRow = true;
            this.rgvMossip.MasterTemplate.EnableFiltering = true;
            this.rgvMossip.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rgvMossip.Name = "rgvMossip";
            this.rgvMossip.Size = new System.Drawing.Size(1761, 708);
            this.rgvMossip.TabIndex = 3;
            this.rgvMossip.ViewRowFormatting += new Telerik.WinControls.UI.RowFormattingEventHandler(this.rgvMossip_ViewRowFormatting);
            this.rgvMossip.CellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.rgvMossip_CellFormatting);
            this.rgvMossip.ViewCellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.rgvMossip_ViewCellFormatting);
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.pnlMessage);
            this.pnlTop.Controls.Add(this.pnlButtons);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1761, 62);
            this.pnlTop.TabIndex = 4;
            // 
            // pnlMessage
            // 
            this.pnlMessage.Controls.Add(this.pnlLblMessage);
            this.pnlMessage.Controls.Add(this.pnlBtnOpenFile);
            this.pnlMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMessage.Location = new System.Drawing.Point(621, 0);
            this.pnlMessage.Name = "pnlMessage";
            this.pnlMessage.Size = new System.Drawing.Size(1140, 62);
            this.pnlMessage.TabIndex = 3;
            // 
            // pnlLblMessage
            // 
            this.pnlLblMessage.AutoSize = true;
            this.pnlLblMessage.Controls.Add(this.lblMessage);
            this.pnlLblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLblMessage.Location = new System.Drawing.Point(0, 0);
            this.pnlLblMessage.Name = "pnlLblMessage";
            this.pnlLblMessage.Size = new System.Drawing.Size(977, 62);
            this.pnlLblMessage.TabIndex = 3;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = false;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMessage.ForeColor = System.Drawing.Color.Red;
            this.lblMessage.Location = new System.Drawing.Point(0, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(977, 62);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.Text = "radLabel1";
            // 
            // pnlBtnOpenFile
            // 
            this.pnlBtnOpenFile.Controls.Add(this.btnOpenFile);
            this.pnlBtnOpenFile.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlBtnOpenFile.Location = new System.Drawing.Point(977, 0);
            this.pnlBtnOpenFile.Name = "pnlBtnOpenFile";
            this.pnlBtnOpenFile.Size = new System.Drawing.Size(163, 62);
            this.pnlBtnOpenFile.TabIndex = 3;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenFile.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnOpenFile.Image = global::SgkAssistant.Properties.Resources.microsoft_excel_641;
            this.btnOpenFile.Location = new System.Drawing.Point(7, 3);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(137, 52);
            this.btnOpenFile.TabIndex = 2;
            this.btnOpenFile.Text = "Dosyayı Aç";
            this.btnOpenFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnExcelHLP);
            this.pnlButtons.Controls.Add(this.btnAnalyzeHL);
            this.pnlButtons.Controls.Add(this.btnPrintHLP);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlButtons.Location = new System.Drawing.Point(0, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(621, 62);
            this.pnlButtons.TabIndex = 4;
            // 
            // btnExcelHLP
            // 
            this.btnExcelHLP.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExcelHLP.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnExcelHLP.Image = global::SgkAssistant.Properties.Resources.export_xls_44;
            this.btnExcelHLP.Location = new System.Drawing.Point(415, 3);
            this.btnExcelHLP.Name = "btnExcelHLP";
            this.btnExcelHLP.Size = new System.Drawing.Size(200, 52);
            this.btnExcelHLP.TabIndex = 0;
            this.btnExcelHLP.Text = "Listeyi Excel\'e Aktar";
            this.btnExcelHLP.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnExcelHLP.Click += new System.EventHandler(this.btnExcelHLP_Click);
            // 
            // btnAnalyzeHL
            // 
            this.btnAnalyzeHL.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAnalyzeHL.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnAnalyzeHL.Image = global::SgkAssistant.Properties.Resources.process_50;
            this.btnAnalyzeHL.Location = new System.Drawing.Point(3, 3);
            this.btnAnalyzeHL.Name = "btnAnalyzeHL";
            this.btnAnalyzeHL.Size = new System.Drawing.Size(200, 52);
            this.btnAnalyzeHL.TabIndex = 0;
            this.btnAnalyzeHL.Text = "Hizmet Lstesini Analiz Et";
            this.btnAnalyzeHL.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAnalyzeHL.Click += new System.EventHandler(this.btnAnalyzeHL_Click);
            // 
            // btnPrintHLP
            // 
            this.btnPrintHLP.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrintHLP.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnPrintHLP.Image = global::SgkAssistant.Properties.Resources.print_641;
            this.btnPrintHLP.Location = new System.Drawing.Point(209, 3);
            this.btnPrintHLP.Name = "btnPrintHLP";
            this.btnPrintHLP.Size = new System.Drawing.Size(200, 52);
            this.btnPrintHLP.TabIndex = 0;
            this.btnPrintHLP.Text = "Listeyi Yazdır";
            this.btnPrintHLP.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPrintHLP.Click += new System.EventHandler(this.btnPrintHLP_Click);
            // 
            // FHlp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1761, 770);
            this.Controls.Add(this.rgvMossip);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1200, 500);
            this.Name = "FHlp";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(0, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HİZMET LİSTESİ";
            this.Load += new System.EventHandler(this.fHl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rgvMossip.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvMossip)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlMessage.ResumeLayout(false);
            this.pnlMessage.PerformLayout();
            this.pnlLblMessage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lblMessage)).EndInit();
            this.pnlBtnOpenFile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnOpenFile)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnExcelHLP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAnalyzeHL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPrintHLP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGridView rgvMossip;
        private System.Windows.Forms.Panel pnlTop;
        private Telerik.WinControls.UI.RadButton btnExcelHLP;
        private Telerik.WinControls.UI.RadButton btnPrintHLP;
        private Telerik.WinControls.UI.RadButton btnAnalyzeHL;
        private Telerik.WinControls.UI.RadLabel lblMessage;
        private Telerik.WinControls.UI.RadButton btnOpenFile;
        private System.Windows.Forms.Panel pnlMessage;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Panel pnlLblMessage;
        private System.Windows.Forms.Panel pnlBtnOpenFile;
    }
}
