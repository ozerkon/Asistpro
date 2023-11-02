namespace SgkAssistant.Forms.Defs
{
    partial class FConfirmDetails
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
            this.components = new System.ComponentModel.Container();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FConfirmDetails));
            this.rgvConfirmDetails = new Telerik.WinControls.UI.RadGridView();
            this.pnlControls = new Telerik.WinControls.UI.RadPanel();
            this.pnlMessage = new Telerik.WinControls.UI.RadPanel();
            this.pnlLabel = new Telerik.WinControls.UI.RadPanel();
            this.lblMessage = new Telerik.WinControls.UI.RadLabel();
            this.pnlBtnOpenFile = new Telerik.WinControls.UI.RadPanel();
            this.pnlBtnExit = new Telerik.WinControls.UI.RadPanel();
            this.pnlBtnExport = new Telerik.WinControls.UI.RadPanel();
            this.btnExit = new Telerik.WinControls.UI.RadButton();
            this.btnOpenFile = new Telerik.WinControls.UI.RadButton();
            this.roundRectShape1 = new Telerik.WinControls.RoundRectShape(this.components);
            this.btnExportExcel = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.rgvConfirmDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvConfirmDetails.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlControls)).BeginInit();
            this.pnlControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMessage)).BeginInit();
            this.pnlMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlLabel)).BeginInit();
            this.pnlLabel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblMessage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBtnOpenFile)).BeginInit();
            this.pnlBtnOpenFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBtnExit)).BeginInit();
            this.pnlBtnExit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBtnExport)).BeginInit();
            this.pnlBtnExport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnExit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOpenFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnExportExcel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // rgvConfirmDetails
            // 
            this.rgvConfirmDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgvConfirmDetails.EnableHotTracking = false;
            this.rgvConfirmDetails.Location = new System.Drawing.Point(0, 42);
            // 
            // 
            // 
            this.rgvConfirmDetails.MasterTemplate.AllowAddNewRow = false;
            this.rgvConfirmDetails.MasterTemplate.AllowColumnChooser = false;
            this.rgvConfirmDetails.MasterTemplate.AllowColumnReorder = false;
            this.rgvConfirmDetails.MasterTemplate.AllowDeleteRow = false;
            this.rgvConfirmDetails.MasterTemplate.AllowEditRow = false;
            this.rgvConfirmDetails.MasterTemplate.AllowRowReorder = true;
            this.rgvConfirmDetails.MasterTemplate.AllowRowResize = false;
            this.rgvConfirmDetails.MasterTemplate.EnableAlternatingRowColor = true;
            this.rgvConfirmDetails.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rgvConfirmDetails.Name = "rgvConfirmDetails";
            this.rgvConfirmDetails.ReadOnly = true;
            this.rgvConfirmDetails.Size = new System.Drawing.Size(1097, 470);
            this.rgvConfirmDetails.TabIndex = 0;
            this.rgvConfirmDetails.CellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.rgvConfirmDetails_CellFormatting);
            this.rgvConfirmDetails.CommandCellClick += new Telerik.WinControls.UI.CommandCellClickEventHandler(this.rgvConfirmDetails_CommandCellClick);
            // 
            // pnlControls
            // 
            this.pnlControls.Controls.Add(this.pnlMessage);
            this.pnlControls.Controls.Add(this.pnlBtnExit);
            this.pnlControls.Controls.Add(this.pnlBtnExport);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlControls.Location = new System.Drawing.Point(0, 0);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(1097, 42);
            this.pnlControls.TabIndex = 2;
            ((Telerik.WinControls.UI.RadPanelElement)(this.pnlControls.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.pnlControls.GetChildAt(0).GetChildAt(1))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.pnlControls.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // pnlMessage
            // 
            this.pnlMessage.Controls.Add(this.pnlLabel);
            this.pnlMessage.Controls.Add(this.pnlBtnOpenFile);
            this.pnlMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMessage.Location = new System.Drawing.Point(127, 0);
            this.pnlMessage.Name = "pnlMessage";
            this.pnlMessage.Size = new System.Drawing.Size(943, 42);
            this.pnlMessage.TabIndex = 4;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.pnlMessage.GetChildAt(0).GetChildAt(1))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.pnlMessage.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // pnlLabel
            // 
            this.pnlLabel.Controls.Add(this.lblMessage);
            this.pnlLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLabel.Location = new System.Drawing.Point(0, 0);
            this.pnlLabel.Name = "pnlLabel";
            this.pnlLabel.Size = new System.Drawing.Size(816, 42);
            this.pnlLabel.TabIndex = 1;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.pnlLabel.GetChildAt(0).GetChildAt(1))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.pnlLabel.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMessage.ForeColor = System.Drawing.Color.Red;
            this.lblMessage.Location = new System.Drawing.Point(14, 12);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(39, 19);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.Text = "Mesaj";
            // 
            // pnlBtnOpenFile
            // 
            this.pnlBtnOpenFile.Controls.Add(this.btnOpenFile);
            this.pnlBtnOpenFile.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlBtnOpenFile.Location = new System.Drawing.Point(816, 0);
            this.pnlBtnOpenFile.Name = "pnlBtnOpenFile";
            this.pnlBtnOpenFile.Size = new System.Drawing.Size(127, 42);
            this.pnlBtnOpenFile.TabIndex = 5;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.pnlBtnOpenFile.GetChildAt(0).GetChildAt(1))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.pnlBtnOpenFile.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // pnlBtnExit
            // 
            this.pnlBtnExit.Controls.Add(this.btnExit);
            this.pnlBtnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlBtnExit.Location = new System.Drawing.Point(1070, 0);
            this.pnlBtnExit.Name = "pnlBtnExit";
            this.pnlBtnExit.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.pnlBtnExit.Size = new System.Drawing.Size(27, 42);
            this.pnlBtnExit.TabIndex = 6;
            ((Telerik.WinControls.UI.RadPanelElement)(this.pnlBtnExit.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.pnlBtnExit.GetChildAt(0).GetChildAt(1))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.pnlBtnExit.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // pnlBtnExport
            // 
            this.pnlBtnExport.Controls.Add(this.btnExportExcel);
            this.pnlBtnExport.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlBtnExport.Location = new System.Drawing.Point(0, 0);
            this.pnlBtnExport.Name = "pnlBtnExport";
            this.pnlBtnExport.Size = new System.Drawing.Size(127, 42);
            this.pnlBtnExport.TabIndex = 3;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.pnlBtnExport.GetChildAt(0).GetChildAt(1))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.pnlBtnExport.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnExit.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.Image")));
            this.btnExit.Location = new System.Drawing.Point(3, 9);
            this.btnExit.Name = "btnExit";
            this.btnExit.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.btnExit.Size = new System.Drawing.Size(24, 24);
            this.btnExit.TabIndex = 0;
            this.btnExit.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnExit.GetChildAt(0))).Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnExit.GetChildAt(0))).TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnExit.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnExit.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btnExit.GetChildAt(0).GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenFile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnOpenFile.ForeColor = System.Drawing.Color.Black;
            this.btnOpenFile.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenFile.Image")));
            this.btnOpenFile.Location = new System.Drawing.Point(3, 9);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.btnOpenFile.Size = new System.Drawing.Size(110, 24);
            this.btnOpenFile.TabIndex = 0;
            this.btnOpenFile.Text = "Dosyayı Aç";
            this.btnOpenFile.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnOpenFile.GetChildAt(0))).Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnOpenFile.GetChildAt(0))).TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnOpenFile.GetChildAt(0))).Text = "Dosyayı Aç";
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnOpenFile.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btnOpenFile.GetChildAt(0).GetChildAt(2))).Shape = this.roundRectShape1;
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.BackColor = System.Drawing.Color.Transparent;
            this.btnExportExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportExcel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnExportExcel.ForeColor = System.Drawing.Color.Black;
            this.btnExportExcel.Image = ((System.Drawing.Image)(resources.GetObject("btnExportExcel.Image")));
            this.btnExportExcel.Location = new System.Drawing.Point(3, 6);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.btnExportExcel.Size = new System.Drawing.Size(110, 24);
            this.btnExportExcel.TabIndex = 0;
            this.btnExportExcel.Text = "Excel\'e Aktar";
            this.btnExportExcel.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnExportExcel.GetChildAt(0))).Image = ((System.Drawing.Image)(resources.GetObject("resource.Image2")));
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnExportExcel.GetChildAt(0))).TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnExportExcel.GetChildAt(0))).Text = "Excel\'e Aktar";
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnExportExcel.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btnExportExcel.GetChildAt(0).GetChildAt(2))).Shape = this.roundRectShape1;
            // 
            // FConfirmDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(1097, 512);
            this.ControlBox = false;
            this.Controls.Add(this.rgvConfirmDetails);
            this.Controls.Add(this.pnlControls);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1100, 200);
            this.Name = "FConfirmDetails";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "RAPOR ONAYLAMA SONUÇLARI";
            this.Load += new System.EventHandler(this.fConfirmDetails_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rgvConfirmDetails.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvConfirmDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlControls)).EndInit();
            this.pnlControls.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlMessage)).EndInit();
            this.pnlMessage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlLabel)).EndInit();
            this.pnlLabel.ResumeLayout(false);
            this.pnlLabel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblMessage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBtnOpenFile)).EndInit();
            this.pnlBtnOpenFile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlBtnExit)).EndInit();
            this.pnlBtnExit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlBtnExport)).EndInit();
            this.pnlBtnExport.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnExit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOpenFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnExportExcel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGridView rgvConfirmDetails;
        private Telerik.WinControls.UI.RadPanel pnlControls;
        private Telerik.WinControls.UI.RadPanel pnlBtnExport;
        private Telerik.WinControls.UI.RadButton btnExportExcel;
        private Telerik.WinControls.UI.RadPanel pnlMessage;
        private Telerik.WinControls.UI.RadLabel lblMessage;
        private Telerik.WinControls.UI.RadPanel pnlBtnExit;
        private Telerik.WinControls.UI.RadButton btnExit;
        private Telerik.WinControls.UI.RadPanel pnlBtnOpenFile;
        private Telerik.WinControls.UI.RadButton btnOpenFile;
        private Telerik.WinControls.UI.RadPanel pnlLabel;
        private Telerik.WinControls.RoundRectShape roundRectShape1;
    }
}
