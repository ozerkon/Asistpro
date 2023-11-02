namespace SgkAssistant.Forms.Defs
{
    partial class FPersonalList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FPersonalList));
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.commandBarPersonalList = new Telerik.WinControls.UI.RadCommandBar();
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
            this.commandBarSeparator2 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.cbreMessage = new Telerik.WinControls.UI.CommandBarRowElement();
            this.cbseMessage = new Telerik.WinControls.UI.CommandBarStripElement();
            this.cblMessage = new Telerik.WinControls.UI.CommandBarLabel();
            this.cbbUndoDelete = new Telerik.WinControls.UI.CommandBarButton();
            this.cbbCloseMessage = new Telerik.WinControls.UI.CommandBarButton();
            this.rgvPersonalList = new Telerik.WinControls.UI.RadGridView();
            this.roundRectShape1 = new Telerik.WinControls.RoundRectShape(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new Telerik.WinControls.UI.RadButton();
            this.ddlCompanies = new Telerik.WinControls.UI.RadDropDownList();
            this.commandBarMessage = new Telerik.WinControls.UI.RadCommandBar();
            this.pnlMessage = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.commandBarPersonalList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvPersonalList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvPersonalList.MasterTemplate)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlCompanies)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.commandBarMessage)).BeginInit();
            this.pnlMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // commandBarPersonalList
            // 
            this.commandBarPersonalList.BackColor = System.Drawing.Color.Transparent;
            this.commandBarPersonalList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.commandBarPersonalList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandBarPersonalList.Location = new System.Drawing.Point(253, 3);
            this.commandBarPersonalList.Name = "commandBarPersonalList";
            this.commandBarPersonalList.Rows.AddRange(new Telerik.WinControls.UI.CommandBarRowElement[] {
            this.cbreAddUpdateDelete});
            this.commandBarPersonalList.Size = new System.Drawing.Size(1052, 40);
            this.commandBarPersonalList.TabIndex = 0;
            this.commandBarPersonalList.ThemeName = "TelerikMetroBlue";
            // 
            // cbreAddUpdateDelete
            // 
            this.cbreAddUpdateDelete.BackColor = System.Drawing.Color.Transparent;
            this.cbreAddUpdateDelete.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.cbreAddUpdateDelete.MinSize = new System.Drawing.Size(25, 25);
            this.cbreAddUpdateDelete.Name = "cbreAddUpdateDelete";
            this.cbreAddUpdateDelete.NumberOfColors = 1;
            this.cbreAddUpdateDelete.Strips.AddRange(new Telerik.WinControls.UI.CommandBarStripElement[] {
            this.cbseAddUpdateDelete});
            this.cbreAddUpdateDelete.Text = "";
            this.cbreAddUpdateDelete.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.cbreAddUpdateDelete.UseCompatibleTextRendering = false;
            this.cbreAddUpdateDelete.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // cbseAddUpdateDelete
            // 
            this.cbseAddUpdateDelete.AutoSize = true;
            this.cbseAddUpdateDelete.BackColor = System.Drawing.Color.Transparent;
            this.cbseAddUpdateDelete.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
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
            this.btnOpenFile,
            this.commandBarSeparator2});
            this.cbseAddUpdateDelete.Name = "cbseAddUpdateDelete";
            this.cbseAddUpdateDelete.NumberOfColors = 1;
            // 
            // 
            // 
            this.cbseAddUpdateDelete.OverflowButton.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.cbseAddUpdateDelete.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.cbseAddUpdateDelete.UseCompatibleTextRendering = false;
            ((Telerik.WinControls.UI.RadCommandBarOverflowButton)(this.cbseAddUpdateDelete.GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // btnAdd
            // 
            this.btnAdd.DisplayName = "commandBarButton1";
            this.btnAdd.DrawText = true;
            this.btnAdd.Image = global::SgkAssistant.Properties.Resources.personalAdd_321;
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
            this.btnEdit.Image = global::SgkAssistant.Properties.Resources.personalEdit_32;
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
            this.btnDelete.Image = global::SgkAssistant.Properties.Resources.personalDelet_32;
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
            this.lblMessage.AutoSize = false;
            this.lblMessage.Bounds = new System.Drawing.Rectangle(0, 0, 420, 36);
            this.lblMessage.DisplayName = "commandBarLabel1";
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMessage.ForeColor = System.Drawing.Color.Red;
            this.lblMessage.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.StretchHorizontally = false;
            this.lblMessage.Text = "lbl mesaj";
            this.lblMessage.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMessage.TextWrap = true;
            this.lblMessage.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.AutoSize = false;
            this.btnOpenFile.Bounds = new System.Drawing.Rectangle(0, 0, 110, 36);
            this.btnOpenFile.DisplayName = "commandBarButton1";
            this.btnOpenFile.DrawText = true;
            this.btnOpenFile.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenFile.Image")));
            this.btnOpenFile.ImageAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenFile.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.btnOpenFile.Text = "Dosyayı Aç";
            this.btnOpenFile.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnOpenFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOpenFile.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // commandBarSeparator2
            // 
            this.commandBarSeparator2.DisplayName = "commandBarSeparator2";
            this.commandBarSeparator2.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
            this.commandBarSeparator2.Name = "commandBarSeparator2";
            this.commandBarSeparator2.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.commandBarSeparator2.VisibleInOverflowMenu = false;
            // 
            // cbreMessage
            // 
            this.cbreMessage.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.cbreMessage.MinSize = new System.Drawing.Size(25, 25);
            this.cbreMessage.Name = "cbreMessage";
            this.cbreMessage.Strips.AddRange(new Telerik.WinControls.UI.CommandBarStripElement[] {
            this.cbseMessage});
            this.cbreMessage.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // cbseMessage
            // 
            this.cbseMessage.AutoSize = false;
            this.cbseMessage.BackColor = System.Drawing.Color.Transparent;
            this.cbseMessage.Bounds = new System.Drawing.Rectangle(0, 0, 1350, 44);
            this.cbseMessage.DisplayName = "commandBarStripElement3";
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
            this.cbseMessage.StretchHorizontally = true;
            this.cbseMessage.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            ((Telerik.WinControls.UI.RadCommandBarGrip)(this.cbseMessage.GetChildAt(0))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
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
            this.cbbCloseMessage.NumberOfColors = 1;
            this.cbbCloseMessage.Text = "Gizle";
            this.cbbCloseMessage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.cbbCloseMessage.Click += new System.EventHandler(this.cbbCloseMessage_Click);
            // 
            // rgvPersonalList
            // 
            this.rgvPersonalList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgvPersonalList.EnableGestures = false;
            this.rgvPersonalList.EnableHotTracking = false;
            this.rgvPersonalList.Location = new System.Drawing.Point(0, 92);
            // 
            // 
            // 
            this.rgvPersonalList.MasterTemplate.AllowAddNewRow = false;
            this.rgvPersonalList.MasterTemplate.AllowDeleteRow = false;
            this.rgvPersonalList.MasterTemplate.AllowEditRow = false;
            this.rgvPersonalList.MasterTemplate.AllowRowResize = false;
            this.rgvPersonalList.MasterTemplate.EnableAlternatingRowColor = true;
            this.rgvPersonalList.MasterTemplate.EnableFiltering = true;
            this.rgvPersonalList.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rgvPersonalList.Name = "rgvPersonalList";
            this.rgvPersonalList.Size = new System.Drawing.Size(1352, 675);
            this.rgvPersonalList.TabIndex = 1;
            this.rgvPersonalList.SelectionChanged += new System.EventHandler(this.rgvPersonalList_SelectionChanged);
            this.rgvPersonalList.RowsChanged += new Telerik.WinControls.UI.GridViewCollectionChangedEventHandler(this.rgvPersonalList_RowsChanged);
            this.rgvPersonalList.ContextMenuOpening += new Telerik.WinControls.UI.ContextMenuOpeningEventHandler(this.rgvPersonalList_ContextMenuOpening);
            this.rgvPersonalList.FilterExpressionChanged += new Telerik.WinControls.UI.GridViewFilterExpressionChangedEventHandler(this.rgvPersonalList_FilterExpressionChanged);
            this.rgvPersonalList.MouseEnter += new System.EventHandler(this.rgvPersonalList_MouseEnter);
            this.rgvPersonalList.MouseLeave += new System.EventHandler(this.rgvPersonalList_MouseLeave);
            this.rgvPersonalList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.rgvPersonalList_MouseMove);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel1.Controls.Add(this.commandBarPersonalList, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.ddlCompanies, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1352, 46);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Image = global::SgkAssistant.Properties.Resources.cancel_32;
            this.btnClose.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnClose.Location = new System.Drawing.Point(1311, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(38, 36);
            this.btnClose.TabIndex = 2;
            this.btnClose.ThemeName = "TelerikMetroBlue";
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnClose.GetChildAt(0))).Image = global::SgkAssistant.Properties.Resources.cancel_32;
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnClose.GetChildAt(0))).ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btnClose.GetChildAt(0).GetChildAt(2))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btnClose.GetChildAt(0).GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // ddlCompanies
            // 
            this.ddlCompanies.AutoSize = false;
            this.ddlCompanies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlCompanies.DropDownAnimationEnabled = true;
            this.ddlCompanies.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.ddlCompanies.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.ddlCompanies.Location = new System.Drawing.Point(3, 3);
            this.ddlCompanies.Name = "ddlCompanies";
            this.ddlCompanies.Size = new System.Drawing.Size(244, 40);
            this.ddlCompanies.TabIndex = 3;
            this.ddlCompanies.Text = "radDropDownList1";
            this.ddlCompanies.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler(this.ddlCompanies_SelectedIndexChanged);
            // 
            // commandBarMessage
            // 
            this.commandBarMessage.BackColor = System.Drawing.Color.Transparent;
            this.commandBarMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandBarMessage.Location = new System.Drawing.Point(3, 3);
            this.commandBarMessage.Name = "commandBarMessage";
            this.commandBarMessage.Rows.AddRange(new Telerik.WinControls.UI.CommandBarRowElement[] {
            this.cbreMessage});
            this.commandBarMessage.Size = new System.Drawing.Size(1346, 44);
            this.commandBarMessage.TabIndex = 4;
            this.commandBarMessage.ThemeName = "TelerikMetroBlue";
            // 
            // pnlMessage
            // 
            this.pnlMessage.Controls.Add(this.commandBarMessage);
            this.pnlMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMessage.Location = new System.Drawing.Point(0, 46);
            this.pnlMessage.Name = "pnlMessage";
            this.pnlMessage.Size = new System.Drawing.Size(1352, 46);
            this.pnlMessage.TabIndex = 3;
            // 
            // FPersonalList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1352, 767);
            this.ControlBox = false;
            this.Controls.Add(this.rgvPersonalList);
            this.Controls.Add(this.pnlMessage);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1360, 1200);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1360, 400);
            this.Name = "FPersonalList";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(1360, 1200);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PERSONEL LİSTESİ";
            this.Load += new System.EventHandler(this.fPersonalList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.commandBarPersonalList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvPersonalList.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvPersonalList)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlCompanies)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.commandBarMessage)).EndInit();
            this.pnlMessage.ResumeLayout(false);
            this.pnlMessage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadCommandBar commandBarPersonalList;
        private Telerik.WinControls.UI.CommandBarRowElement cbreAddUpdateDelete;
        private Telerik.WinControls.UI.CommandBarStripElement cbseAddUpdateDelete;
        private Telerik.WinControls.UI.CommandBarButton btnAdd;
        private Telerik.WinControls.UI.CommandBarButton btnEdit;
        private Telerik.WinControls.UI.CommandBarButton btnDelete;
        private Telerik.WinControls.UI.RadGridView rgvPersonalList;
        private Telerik.WinControls.UI.CommandBarRowElement cbreMessage;
        private Telerik.WinControls.UI.CommandBarStripElement cbseMessage;
        private Telerik.WinControls.UI.CommandBarLabel cblMessage;
        private Telerik.WinControls.UI.CommandBarButton cbbUndoDelete;
        private Telerik.WinControls.UI.CommandBarButton cbbCloseMessage;
        private Telerik.WinControls.UI.CommandBarButton btnImport;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator1;
        private Telerik.WinControls.UI.CommandBarButton btnExport;
        private Telerik.WinControls.RoundRectShape roundRectShape1;
        private Telerik.WinControls.UI.CommandBarLabel lblMessage;
        private Telerik.WinControls.UI.CommandBarButton btnOpenFile;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadButton btnClose;
        private Telerik.WinControls.UI.RadDropDownList ddlCompanies;
        private Telerik.WinControls.UI.RadCommandBar commandBarMessage;
        private System.Windows.Forms.FlowLayoutPanel pnlMessage;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator2;
    }
}
