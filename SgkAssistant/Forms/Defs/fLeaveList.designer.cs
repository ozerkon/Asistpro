namespace SgkAssistant.Forms.Defs
{
    partial class FLeaveList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FLeaveList));
            this.commandBarLeaves = new Telerik.WinControls.UI.RadCommandBar();
            this.cbreAddUpdateDelete = new Telerik.WinControls.UI.CommandBarRowElement();
            this.cbseAddUpdateDelete = new Telerik.WinControls.UI.CommandBarStripElement();
            this.btnAdd = new Telerik.WinControls.UI.CommandBarButton();
            this.btnEdit = new Telerik.WinControls.UI.CommandBarButton();
            this.btnDelete = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarSeparator1 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.btnImport = new Telerik.WinControls.UI.CommandBarButton();
            this.btnExport = new Telerik.WinControls.UI.CommandBarButton();
            this.lblMessage = new Telerik.WinControls.UI.CommandBarLabel();
            this.btnOpenFile = new Telerik.WinControls.UI.CommandBarButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new Telerik.WinControls.UI.RadButton();
            this.rgvLeaveList = new Telerik.WinControls.UI.RadGridView();
            this.pnlMessage = new System.Windows.Forms.FlowLayoutPanel();
            this.commandBarMessage = new Telerik.WinControls.UI.RadCommandBar();
            this.cbreMessage = new Telerik.WinControls.UI.CommandBarRowElement();
            this.cbseMessage = new Telerik.WinControls.UI.CommandBarStripElement();
            this.cblMessage = new Telerik.WinControls.UI.CommandBarLabel();
            this.cbbUndoDelete = new Telerik.WinControls.UI.CommandBarButton();
            this.cbbCloseMessage = new Telerik.WinControls.UI.CommandBarButton();
            ((System.ComponentModel.ISupportInitialize)(this.commandBarLeaves)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvLeaveList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvLeaveList.MasterTemplate)).BeginInit();
            this.pnlMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.commandBarMessage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // commandBarLeaves
            // 
            this.commandBarLeaves.BackColor = System.Drawing.Color.Transparent;
            this.commandBarLeaves.Cursor = System.Windows.Forms.Cursors.Hand;
            this.commandBarLeaves.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandBarLeaves.Location = new System.Drawing.Point(3, 3);
            this.commandBarLeaves.Name = "commandBarLeaves";
            this.commandBarLeaves.Rows.AddRange(new Telerik.WinControls.UI.CommandBarRowElement[] {
            this.cbreAddUpdateDelete});
            this.commandBarLeaves.Size = new System.Drawing.Size(1229, 36);
            this.commandBarLeaves.TabIndex = 0;
            this.commandBarLeaves.ThemeName = "TelerikMetroBlue";
            // 
            // cbreAddUpdateDelete
            // 
            this.cbreAddUpdateDelete.BackColor = System.Drawing.Color.Transparent;
            this.cbreAddUpdateDelete.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.cbreAddUpdateDelete.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.cbreAddUpdateDelete.ForeColor = System.Drawing.Color.Black;
            this.cbreAddUpdateDelete.MinSize = new System.Drawing.Size(25, 25);
            this.cbreAddUpdateDelete.Name = "cbreAddUpdateDelete";
            this.cbreAddUpdateDelete.NumberOfColors = 1;
            this.cbreAddUpdateDelete.Strips.AddRange(new Telerik.WinControls.UI.CommandBarStripElement[] {
            this.cbseAddUpdateDelete});
            this.cbreAddUpdateDelete.Text = "";
            this.cbreAddUpdateDelete.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.cbreAddUpdateDelete.UseCompatibleTextRendering = false;
            this.cbreAddUpdateDelete.UseDefaultDisabledPaint = true;
            // 
            // cbseAddUpdateDelete
            // 
            this.cbseAddUpdateDelete.BackColor = System.Drawing.Color.Transparent;
            this.cbseAddUpdateDelete.DisplayName = "commandBarStripElement1";
            this.cbseAddUpdateDelete.DrawBorder = false;
            this.cbseAddUpdateDelete.DrawFill = true;
            this.cbseAddUpdateDelete.DrawImage = true;
            this.cbseAddUpdateDelete.EnableDragging = false;
            this.cbseAddUpdateDelete.GradientPercentage2 = 0.5F;
            this.cbseAddUpdateDelete.Items.AddRange(new Telerik.WinControls.UI.RadCommandBarBaseItem[] {
            this.btnAdd,
            this.btnEdit,
            this.btnDelete,
            this.commandBarSeparator1,
            this.btnImport,
            this.btnExport,
            this.lblMessage,
            this.btnOpenFile});
            this.cbseAddUpdateDelete.Name = "cbseAddUpdateDelete";
            this.cbseAddUpdateDelete.NumberOfColors = 1;
            // 
            // 
            // 
            this.cbseAddUpdateDelete.OverflowButton.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            ((Telerik.WinControls.UI.RadCommandBarOverflowButton)(this.cbseAddUpdateDelete.GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // btnAdd
            // 
            this.btnAdd.DisplayName = "commandBarButton1";
            this.btnAdd.DrawText = true;
            this.btnAdd.Image = global::SgkAssistant.Properties.Resources.leave_add32;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Text = "Yeni";
            this.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.DisplayName = "commandBarButton2";
            this.btnEdit.DrawText = true;
            this.btnEdit.Image = global::SgkAssistant.Properties.Resources.leave_edit32;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Text = "Detay";
            this.btnEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayName = "commandBarButton3";
            this.btnDelete.DrawText = true;
            this.btnDelete.Image = global::SgkAssistant.Properties.Resources.leave_delete32;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Text = "Sil";
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // commandBarSeparator1
            // 
            this.commandBarSeparator1.AutoSize = false;
            this.commandBarSeparator1.Bounds = new System.Drawing.Rectangle(0, 0, 2, 36);
            this.commandBarSeparator1.DisplayName = "commandBarSeparator1";
            this.commandBarSeparator1.Name = "commandBarSeparator1";
            this.commandBarSeparator1.VisibleInOverflowMenu = false;
            // 
            // btnImport
            // 
            this.btnImport.DisplayName = "commandBarButton4";
            this.btnImport.DrawText = true;
            this.btnImport.Image = global::SgkAssistant.Properties.Resources.import_from_excel_32;
            this.btnImport.Name = "btnImport";
            this.btnImport.Text = "Toplu Ekleme";
            this.btnImport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExport
            // 
            this.btnExport.DisplayName = "commandBarButton5";
            this.btnExport.DrawText = true;
            this.btnExport.Image = global::SgkAssistant.Properties.Resources.excel_32;
            this.btnExport.Name = "btnExport";
            this.btnExport.Text = "Excel\'e Aktar";
            this.btnExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = false;
            this.lblMessage.Bounds = new System.Drawing.Rectangle(0, 0, 630, 36);
            this.lblMessage.DisplayName = "commandBarLabel1";
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMessage.ForeColor = System.Drawing.Color.Red;
            this.lblMessage.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Text = "lbl mesaj";
            this.lblMessage.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMessage.TextWrap = true;
            this.lblMessage.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.AutoSize = false;
            this.btnOpenFile.Bounds = new System.Drawing.Rectangle(0, 0, 105, 36);
            this.btnOpenFile.DisplayName = "commandBarButton6";
            this.btnOpenFile.DrawText = true;
            this.btnOpenFile.Image = global::SgkAssistant.Properties.Resources.xls;
            this.btnOpenFile.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.btnOpenFile.Text = "Dosyayı Aç";
            this.btnOpenFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOpenFile.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.commandBarLeaves, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1285, 46);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Image = global::SgkAssistant.Properties.Resources.cancel_32;
            this.btnClose.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnClose.Location = new System.Drawing.Point(1238, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(38, 36);
            this.btnClose.TabIndex = 2;
            this.btnClose.ThemeName = "TelerikMetroBlue";
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnClose.GetChildAt(0))).Image = global::SgkAssistant.Properties.Resources.cancel_32;
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnClose.GetChildAt(0))).ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btnClose.GetChildAt(0).GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // rgvLeaveList
            // 
            this.rgvLeaveList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgvLeaveList.Location = new System.Drawing.Point(0, 92);
            // 
            // 
            // 
            this.rgvLeaveList.MasterTemplate.AllowAddNewRow = false;
            this.rgvLeaveList.MasterTemplate.AllowDeleteRow = false;
            this.rgvLeaveList.MasterTemplate.AllowEditRow = false;
            this.rgvLeaveList.MasterTemplate.AllowRowResize = false;
            this.rgvLeaveList.MasterTemplate.EnableAlternatingRowColor = true;
            this.rgvLeaveList.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rgvLeaveList.Name = "rgvLeaveList";
            this.rgvLeaveList.Size = new System.Drawing.Size(1285, 602);
            this.rgvLeaveList.TabIndex = 2;
            this.rgvLeaveList.CellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.rgvLeaveList_CellFormatting);
            this.rgvLeaveList.SelectionChanged += new System.EventHandler(this.rgvLeaveList_SelectionChanged);
            this.rgvLeaveList.RowsChanged += new Telerik.WinControls.UI.GridViewCollectionChangedEventHandler(this.rgvLeaveList_RowsChanged);
            this.rgvLeaveList.CommandCellClick += new Telerik.WinControls.UI.CommandCellClickEventHandler(this.rgvLeaveList_CommandCellClick);
            this.rgvLeaveList.FilterExpressionChanged += new Telerik.WinControls.UI.GridViewFilterExpressionChangedEventHandler(this.rgvLeaveList_FilterExpressionChanged);
            this.rgvLeaveList.MouseEnter += new System.EventHandler(this.rgvLeaveList_MouseEnter);
            this.rgvLeaveList.MouseLeave += new System.EventHandler(this.rgvLeaveList_MouseLeave);
            this.rgvLeaveList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.rgvLeaveList_MouseMove);
            // 
            // pnlMessage
            // 
            this.pnlMessage.Controls.Add(this.commandBarMessage);
            this.pnlMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMessage.Location = new System.Drawing.Point(0, 46);
            this.pnlMessage.Name = "pnlMessage";
            this.pnlMessage.Size = new System.Drawing.Size(1285, 46);
            this.pnlMessage.TabIndex = 3;
            // 
            // commandBarMessage
            // 
            this.commandBarMessage.BackColor = System.Drawing.Color.Transparent;
            this.commandBarMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandBarMessage.Location = new System.Drawing.Point(3, 3);
            this.commandBarMessage.Name = "commandBarMessage";
            this.commandBarMessage.Rows.AddRange(new Telerik.WinControls.UI.CommandBarRowElement[] {
            this.cbreMessage});
            this.commandBarMessage.Size = new System.Drawing.Size(1279, 71);
            this.commandBarMessage.TabIndex = 0;
            // 
            // cbreMessage
            // 
            this.cbreMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.cbreMessage.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.cbreMessage.MinSize = new System.Drawing.Size(25, 25);
            this.cbreMessage.Name = "cbreMessage";
            this.cbreMessage.Strips.AddRange(new Telerik.WinControls.UI.CommandBarStripElement[] {
            this.cbseMessage});
            this.cbreMessage.Text = "";
            this.cbreMessage.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.cbreMessage.UseCompatibleTextRendering = false;
            // 
            // cbseMessage
            // 
            this.cbseMessage.AutoSize = false;
            this.cbseMessage.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.cbseMessage.BackColor = System.Drawing.Color.Transparent;
            this.cbseMessage.Bounds = new System.Drawing.Rectangle(0, 0, 1280, 46);
            this.cbseMessage.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.cbseMessage.DisplayName = "commandBarStripElement3";
            this.cbseMessage.DrawBorder = true;
            this.cbseMessage.DrawFill = true;
            this.cbseMessage.DrawText = false;
            this.cbseMessage.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.cbseMessage.ForeColor = System.Drawing.Color.Black;
            // 
            // 
            // 
            this.cbseMessage.Grip.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.cbseMessage.Items.AddRange(new Telerik.WinControls.UI.RadCommandBarBaseItem[] {
            this.cblMessage,
            this.cbbUndoDelete,
            this.cbbCloseMessage});
            this.cbseMessage.Name = "cbseMessage";
            this.cbseMessage.NumberOfColors = 1;
            // 
            // 
            // 
            this.cbseMessage.OverflowButton.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.cbseMessage.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            this.cbseMessage.StretchHorizontally = true;
            this.cbseMessage.StretchVertically = true;
            this.cbseMessage.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbseMessage.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.cbseMessage.UseCompatibleTextRendering = true;
            this.cbseMessage.UseDefaultDisabledPaint = true;
            ((Telerik.WinControls.UI.RadCommandBarGrip)(this.cbseMessage.GetChildAt(0))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            ((Telerik.WinControls.UI.RadCommandBarOverflowButton)(this.cbseMessage.GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // cblMessage
            // 
            this.cblMessage.AutoSize = true;
            this.cblMessage.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.cblMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cblMessage.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.cblMessage.DisplayName = "commandBarLabel1";
            this.cblMessage.ForeColor = System.Drawing.Color.Red;
            this.cblMessage.Name = "cblMessage";
            this.cblMessage.StretchHorizontally = true;
            this.cblMessage.Text = "Silinme Mesajı";
            this.cblMessage.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cblMessage.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.cblMessage.UseCompatibleTextRendering = false;
            this.cblMessage.UseDefaultDisabledPaint = true;
            this.cblMessage.TextChanged += new System.EventHandler(this.cblMessage_TextChanged);
            // 
            // cbbUndoDelete
            // 
            this.cbbUndoDelete.DisplayName = "commandBarButton1";
            this.cbbUndoDelete.DrawText = true;
            this.cbbUndoDelete.Image = global::SgkAssistant.Properties.Resources.undo;
            this.cbbUndoDelete.Name = "cbbUndoDelete";
            this.cbbUndoDelete.Text = "Geri Al";
            this.cbbUndoDelete.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.cbbUndoDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.cbbUndoDelete.Click += new System.EventHandler(this.cbbUndoDelete_Click);
            // 
            // cbbCloseMessage
            // 
            this.cbbCloseMessage.DisplayName = "commandBarButton2";
            this.cbbCloseMessage.DrawText = true;
            this.cbbCloseMessage.Image = global::SgkAssistant.Properties.Resources.hide;
            this.cbbCloseMessage.Name = "cbbCloseMessage";
            this.cbbCloseMessage.Text = "Gizle";
            this.cbbCloseMessage.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.cbbCloseMessage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.cbbCloseMessage.Click += new System.EventHandler(this.cbbCloseMessage_Click);
            // 
            // FLeaveList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1285, 694);
            this.ControlBox = false;
            this.Controls.Add(this.rgvLeaveList);
            this.Controls.Add(this.pnlMessage);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FLeaveList";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "İZİN LİSTESİ";
            this.Load += new System.EventHandler(this.fLeaveList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.commandBarLeaves)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvLeaveList.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvLeaveList)).EndInit();
            this.pnlMessage.ResumeLayout(false);
            this.pnlMessage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.commandBarMessage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadCommandBar commandBarLeaves;
        private Telerik.WinControls.UI.CommandBarRowElement cbreAddUpdateDelete;
        private Telerik.WinControls.UI.CommandBarStripElement cbseAddUpdateDelete;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.CommandBarButton btnAdd;
        private Telerik.WinControls.UI.CommandBarButton btnEdit;
        private Telerik.WinControls.UI.CommandBarButton btnDelete;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator1;
        private Telerik.WinControls.UI.CommandBarButton btnImport;
        private Telerik.WinControls.UI.CommandBarButton btnExport;
        private Telerik.WinControls.UI.CommandBarLabel lblMessage;
        private Telerik.WinControls.UI.CommandBarButton btnOpenFile;
        private Telerik.WinControls.UI.RadButton btnClose;
        private Telerik.WinControls.UI.RadGridView rgvLeaveList;
        private System.Windows.Forms.FlowLayoutPanel pnlMessage;
        private Telerik.WinControls.UI.RadCommandBar commandBarMessage;
        private Telerik.WinControls.UI.CommandBarRowElement cbreMessage;
        private Telerik.WinControls.UI.CommandBarStripElement cbseMessage;
        private Telerik.WinControls.UI.CommandBarLabel cblMessage;
        private Telerik.WinControls.UI.CommandBarButton cbbUndoDelete;
        private Telerik.WinControls.UI.CommandBarButton cbbCloseMessage;
    }
}
