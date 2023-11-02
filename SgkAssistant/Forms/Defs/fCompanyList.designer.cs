namespace SgkAssistant.Forms.Defs
{
    partial class FCompanyList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FCompanyList));
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.commandBarCompanyList = new Telerik.WinControls.UI.RadCommandBar();
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
            this.cbreMessage = new Telerik.WinControls.UI.CommandBarRowElement();
            this.cbseMessage = new Telerik.WinControls.UI.CommandBarStripElement();
            this.cblMessage = new Telerik.WinControls.UI.CommandBarLabel();
            this.cbbUndoDelete = new Telerik.WinControls.UI.CommandBarButton();
            this.cbbCloseMessage = new Telerik.WinControls.UI.CommandBarButton();
            this.rgvCompanyList = new Telerik.WinControls.UI.RadGridView();
            this.commandBarRowElement1 = new Telerik.WinControls.UI.CommandBarRowElement();
            this.commandBarRowElement2 = new Telerik.WinControls.UI.CommandBarRowElement();
            this.roundRectShape1 = new Telerik.WinControls.RoundRectShape(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.commandBarCompanyList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvCompanyList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvCompanyList.MasterTemplate)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // commandBarCompanyList
            // 
            this.commandBarCompanyList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.commandBarCompanyList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandBarCompanyList.Location = new System.Drawing.Point(3, 3);
            this.commandBarCompanyList.Name = "commandBarCompanyList";
            this.commandBarCompanyList.Rows.AddRange(new Telerik.WinControls.UI.CommandBarRowElement[] {
            this.cbreAddUpdateDelete,
            this.cbreMessage});
            this.commandBarCompanyList.Size = new System.Drawing.Size(1226, 36);
            this.commandBarCompanyList.TabIndex = 0;
            this.commandBarCompanyList.ToolTipTextNeeded += new Telerik.WinControls.ToolTipTextNeededEventHandler(this.commandBarCompanyList_ToolTipTextNeeded);
            // 
            // cbreAddUpdateDelete
            // 
            this.cbreAddUpdateDelete.MinSize = new System.Drawing.Size(25, 25);
            this.cbreAddUpdateDelete.Name = "cbreAddUpdateDelete";
            this.cbreAddUpdateDelete.Strips.AddRange(new Telerik.WinControls.UI.CommandBarStripElement[] {
            this.cbseAddUpdateDelete});
            // 
            // cbseAddUpdateDelete
            // 
            this.cbseAddUpdateDelete.AutoSize = true;
            this.cbseAddUpdateDelete.BackColor = System.Drawing.Color.Transparent;
            this.cbseAddUpdateDelete.DisplayName = "commandBarStripElement1";
            this.cbseAddUpdateDelete.DrawBorder = false;
            this.cbseAddUpdateDelete.EnableDragging = false;
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
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.MinSize = new System.Drawing.Size(40, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Text = "Yeni";
            this.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.DisplayName = "commandBarButton2";
            this.btnEdit.DrawText = true;
            this.btnEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnEdit.Image")));
            this.btnEdit.MinSize = new System.Drawing.Size(40, 0);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Text = "Detay";
            this.btnEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayName = "commandBarButton3";
            this.btnDelete.DrawText = true;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.MinSize = new System.Drawing.Size(40, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Text = "Sil";
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // commandBarSeparator1
            // 
            this.commandBarSeparator1.DisplayName = "commandBarSeparator1";
            this.commandBarSeparator1.Name = "commandBarSeparator1";
            this.commandBarSeparator1.VisibleInOverflowMenu = false;
            // 
            // btnImport
            // 
            this.btnImport.DisplayName = "commandBarButton1";
            this.btnImport.DrawText = true;
            this.btnImport.Image = ((System.Drawing.Image)(resources.GetObject("btnImport.Image")));
            this.btnImport.Name = "btnImport";
            this.btnImport.Shape = null;
            this.btnImport.Text = "Toplu Ekleme";
            this.btnImport.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.btnImport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExport
            // 
            this.btnExport.DisplayName = "commandBarButton2";
            this.btnExport.DrawText = true;
            this.btnExport.Image = ((System.Drawing.Image)(resources.GetObject("btnExport.Image")));
            this.btnExport.Name = "btnExport";
            this.btnExport.Text = "Excel\'e Aktar";
            this.btnExport.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.DisplayName = "commandBarLabel1";
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMessage.ForeColor = System.Drawing.Color.Red;
            this.lblMessage.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.StretchHorizontally = true;
            this.lblMessage.Text = "commandBarLabel1";
            this.lblMessage.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.DisplayName = "commandBarButton1";
            this.btnOpenFile.DrawText = true;
            this.btnOpenFile.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenFile.Image")));
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Text = "Dosyayı Aç";
            this.btnOpenFile.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOpenFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOpenFile.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // cbreMessage
            // 
            this.cbreMessage.MinSize = new System.Drawing.Size(25, 25);
            this.cbreMessage.Name = "cbreMessage";
            this.cbreMessage.Strips.AddRange(new Telerik.WinControls.UI.CommandBarStripElement[] {
            this.cbseMessage});
            this.cbreMessage.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // cbseMessage
            // 
            this.cbseMessage.BackColor = System.Drawing.Color.Transparent;
            this.cbseMessage.DisplayName = "commandBarStripElement3";
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
            this.cbseMessage.StretchHorizontally = true;
            this.cbseMessage.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            ((Telerik.WinControls.UI.RadCommandBarOverflowButton)(this.cbseMessage.GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // cblMessage
            // 
            this.cblMessage.DisplayName = "commandBarLabel2";
            this.cblMessage.ForeColor = System.Drawing.Color.Red;
            this.cblMessage.Name = "cblMessage";
            this.cblMessage.StretchHorizontally = true;
            this.cblMessage.Text = "Silinme mesajı";
            this.cblMessage.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cblMessage.TextChanged += new System.EventHandler(this.cblMessage_TextChanged);
            // 
            // cbbUndoDelete
            // 
            this.cbbUndoDelete.DisplayName = "commandBarButton1";
            this.cbbUndoDelete.DrawText = true;
            this.cbbUndoDelete.Image = ((System.Drawing.Image)(resources.GetObject("cbbUndoDelete.Image")));
            this.cbbUndoDelete.Name = "cbbUndoDelete";
            this.cbbUndoDelete.Text = "Geri Al";
            this.cbbUndoDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.cbbUndoDelete.Click += new System.EventHandler(this.cbbUndoDelete_Click);
            // 
            // cbbCloseMessage
            // 
            this.cbbCloseMessage.DisplayName = "commandBarButton1";
            this.cbbCloseMessage.DrawText = true;
            this.cbbCloseMessage.Image = ((System.Drawing.Image)(resources.GetObject("cbbCloseMessage.Image")));
            this.cbbCloseMessage.MinSize = new System.Drawing.Size(42, 0);
            this.cbbCloseMessage.Name = "cbbCloseMessage";
            this.cbbCloseMessage.Text = "Gizle";
            this.cbbCloseMessage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.cbbCloseMessage.Click += new System.EventHandler(this.cbbCloseMessage_Click);
            // 
            // rgvCompanyList
            // 
            this.rgvCompanyList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgvCompanyList.EnableGestures = false;
            this.rgvCompanyList.EnableHotTracking = false;
            this.rgvCompanyList.Location = new System.Drawing.Point(0, 42);
            // 
            // 
            // 
            this.rgvCompanyList.MasterTemplate.AllowAddNewRow = false;
            this.rgvCompanyList.MasterTemplate.AllowDeleteRow = false;
            this.rgvCompanyList.MasterTemplate.AllowDragToGroup = false;
            this.rgvCompanyList.MasterTemplate.AllowEditRow = false;
            this.rgvCompanyList.MasterTemplate.AllowRowResize = false;
            this.rgvCompanyList.MasterTemplate.EnableAlternatingRowColor = true;
            this.rgvCompanyList.MasterTemplate.EnableFiltering = true;
            this.rgvCompanyList.MasterTemplate.EnableGrouping = false;
            this.rgvCompanyList.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rgvCompanyList.Name = "rgvCompanyList";
            this.rgvCompanyList.Size = new System.Drawing.Size(1276, 479);
            this.rgvCompanyList.TabIndex = 1;
            this.rgvCompanyList.SelectionChanged += new System.EventHandler(this.rgvCompanyList_SelectionChanged);
            this.rgvCompanyList.RowsChanged += new Telerik.WinControls.UI.GridViewCollectionChangedEventHandler(this.rgvCompanyList_RowsChanged);
            this.rgvCompanyList.FilterExpressionChanged += new Telerik.WinControls.UI.GridViewFilterExpressionChangedEventHandler(this.rgvCompanyList_FilterExpressionChanged);
            this.rgvCompanyList.MouseEnter += new System.EventHandler(this.rgvCompanyList_MouseEnter);
            // 
            // commandBarRowElement1
            // 
            this.commandBarRowElement1.MinSize = new System.Drawing.Size(25, 25);
            this.commandBarRowElement1.Name = "commandBarRowElement1";
            // 
            // commandBarRowElement2
            // 
            this.commandBarRowElement2.MinSize = new System.Drawing.Size(25, 25);
            this.commandBarRowElement2.Name = "commandBarRowElement2";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.commandBarCompanyList, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1276, 42);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Image = global::SgkAssistant.Properties.Resources.cancel_32;
            this.btnClose.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnClose.Location = new System.Drawing.Point(1235, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(38, 36);
            this.btnClose.TabIndex = 2;
            this.btnClose.ThemeName = "TelerikMetroBlue";
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnClose.GetChildAt(0))).Image = global::SgkAssistant.Properties.Resources.cancel_32;
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnClose.GetChildAt(0))).ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btnClose.GetChildAt(0).GetChildAt(2))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btnClose.GetChildAt(0).GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // FCompanyList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1276, 521);
            this.ControlBox = false;
            this.Controls.Add(this.rgvCompanyList);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1280, 500);
            this.Name = "FCompanyList";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FİRMA LİSTESİ";
            this.Load += new System.EventHandler(this.fCompanyList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.commandBarCompanyList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvCompanyList.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvCompanyList)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadCommandBar commandBarCompanyList;
        private Telerik.WinControls.UI.CommandBarRowElement cbreAddUpdateDelete;
        private Telerik.WinControls.UI.CommandBarStripElement cbseAddUpdateDelete;
        private Telerik.WinControls.UI.CommandBarButton btnAdd;
        private Telerik.WinControls.UI.CommandBarButton btnEdit;
        private Telerik.WinControls.UI.CommandBarButton btnDelete;
        private Telerik.WinControls.UI.RadGridView rgvCompanyList;
        private Telerik.WinControls.UI.CommandBarRowElement cbreMessage;
        private Telerik.WinControls.UI.CommandBarStripElement cbseMessage;
        private Telerik.WinControls.UI.CommandBarLabel cblMessage;
        private Telerik.WinControls.UI.CommandBarButton cbbUndoDelete;
        private Telerik.WinControls.UI.CommandBarButton cbbCloseMessage;
        private Telerik.WinControls.UI.CommandBarRowElement commandBarRowElement1;
        private Telerik.WinControls.UI.CommandBarRowElement commandBarRowElement2;
        private Telerik.WinControls.UI.CommandBarButton btnImport;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator1;
        private Telerik.WinControls.UI.CommandBarButton btnExport;
        private Telerik.WinControls.RoundRectShape roundRectShape1;
        private Telerik.WinControls.UI.CommandBarLabel lblMessage;
        private Telerik.WinControls.UI.CommandBarButton btnOpenFile;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadButton btnClose;
    }
}
