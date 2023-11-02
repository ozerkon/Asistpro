namespace SgkAssistant.Forms.Defs
{
    partial class FUserList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FUserList));
            this.rgvUserList = new Telerik.WinControls.UI.RadGridView();
            this.cbreButtons = new Telerik.WinControls.UI.CommandBarRowElement();
            this.cbseMain = new Telerik.WinControls.UI.CommandBarStripElement();
            this.cbbAdd = new Telerik.WinControls.UI.CommandBarButton();
            this.cbbEdit = new Telerik.WinControls.UI.CommandBarButton();
            this.cbbDelete = new Telerik.WinControls.UI.CommandBarButton();
            this.cbbChangeUser = new Telerik.WinControls.UI.CommandBarButton();
            this.cbseClose = new Telerik.WinControls.UI.CommandBarStripElement();
            this.cblExit = new Telerik.WinControls.UI.CommandBarLabel();
            this.cbbExit = new Telerik.WinControls.UI.CommandBarButton();
            this.radCommandBar1 = new Telerik.WinControls.UI.RadCommandBar();
            this.cbreMessage = new Telerik.WinControls.UI.CommandBarRowElement();
            this.cbseMessage = new Telerik.WinControls.UI.CommandBarStripElement();
            this.cblMessage = new Telerik.WinControls.UI.CommandBarLabel();
            this.cbbCloseMessage = new Telerik.WinControls.UI.CommandBarButton();
            ((System.ComponentModel.ISupportInitialize)(this.rgvUserList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvUserList.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // rgvUserList
            // 
            this.rgvUserList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgvUserList.EnableGestures = false;
            this.rgvUserList.EnableHotTracking = false;
            this.rgvUserList.Location = new System.Drawing.Point(0, 51);
            // 
            // 
            // 
            this.rgvUserList.MasterTemplate.AllowAddNewRow = false;
            this.rgvUserList.MasterTemplate.AllowColumnChooser = false;
            this.rgvUserList.MasterTemplate.AllowColumnReorder = false;
            this.rgvUserList.MasterTemplate.AllowDeleteRow = false;
            this.rgvUserList.MasterTemplate.AllowDragToGroup = false;
            this.rgvUserList.MasterTemplate.AllowEditRow = false;
            this.rgvUserList.MasterTemplate.AllowRowHeaderContextMenu = false;
            this.rgvUserList.MasterTemplate.EnableGrouping = false;
            this.rgvUserList.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rgvUserList.Name = "rgvUserList";
            this.rgvUserList.Size = new System.Drawing.Size(662, 216);
            this.rgvUserList.TabIndex = 1;
            this.rgvUserList.SelectionChanged += new System.EventHandler(this.rgvUserList_SelectionChanged);
            this.rgvUserList.MouseEnter += new System.EventHandler(this.rgvUserList_MouseEnter);
            // 
            // cbreButtons
            // 
            this.cbreButtons.BackColor = System.Drawing.Color.Transparent;
            this.cbreButtons.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.cbreButtons.MinSize = new System.Drawing.Size(25, 25);
            this.cbreButtons.Name = "cbreButtons";
            this.cbreButtons.NumberOfColors = 1;
            this.cbreButtons.Strips.AddRange(new Telerik.WinControls.UI.CommandBarStripElement[] {
            this.cbseMain,
            this.cbseClose});
            this.cbreButtons.Text = "";
            this.cbreButtons.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.cbreButtons.UseCompatibleTextRendering = false;
            // 
            // cbseMain
            // 
            this.cbseMain.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.cbseMain.DisplayName = "commandBarStripElement1";
            this.cbseMain.EnableDragging = false;
            this.cbseMain.Items.AddRange(new Telerik.WinControls.UI.RadCommandBarBaseItem[] {
            this.cbbAdd,
            this.cbbEdit,
            this.cbbDelete,
            this.cbbChangeUser});
            this.cbseMain.Name = "cbseMain";
            // 
            // 
            // 
            this.cbseMain.OverflowButton.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.cbseMain.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.cbseMain.UseCompatibleTextRendering = false;
            ((Telerik.WinControls.UI.RadCommandBarOverflowButton)(this.cbseMain.GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // cbbAdd
            // 
            this.cbbAdd.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.cbbAdd.DisplayName = "commandBarButton1";
            this.cbbAdd.DrawText = true;
            this.cbbAdd.Image = ((System.Drawing.Image)(resources.GetObject("cbbAdd.Image")));
            this.cbbAdd.Name = "cbbAdd";
            this.cbbAdd.Text = "Yeni";
            this.cbbAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.cbbAdd.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.cbbAdd.UseCompatibleTextRendering = false;
            this.cbbAdd.Click += new System.EventHandler(this.cbbAdd_Click);
            // 
            // cbbEdit
            // 
            this.cbbEdit.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.cbbEdit.DisplayName = "commandBarButton2";
            this.cbbEdit.DrawText = true;
            this.cbbEdit.Image = ((System.Drawing.Image)(resources.GetObject("cbbEdit.Image")));
            this.cbbEdit.Name = "cbbEdit";
            this.cbbEdit.Text = "Detay";
            this.cbbEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.cbbEdit.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.cbbEdit.UseCompatibleTextRendering = false;
            this.cbbEdit.Click += new System.EventHandler(this.cbbEdit_Click);
            // 
            // cbbDelete
            // 
            this.cbbDelete.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.cbbDelete.DisplayName = "commandBarButton3";
            this.cbbDelete.DrawText = true;
            this.cbbDelete.Image = ((System.Drawing.Image)(resources.GetObject("cbbDelete.Image")));
            this.cbbDelete.Name = "cbbDelete";
            this.cbbDelete.Text = "Sil";
            this.cbbDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.cbbDelete.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.cbbDelete.UseCompatibleTextRendering = false;
            this.cbbDelete.Click += new System.EventHandler(this.cbbDelete_Click);
            // 
            // cbbChangeUser
            // 
            this.cbbChangeUser.DisplayName = "commandBarButton1";
            this.cbbChangeUser.DrawText = true;
            this.cbbChangeUser.Image = ((System.Drawing.Image)(resources.GetObject("cbbChangeUser.Image")));
            this.cbbChangeUser.Name = "cbbChangeUser";
            this.cbbChangeUser.Text = "Kullanıcı Değiştir";
            this.cbbChangeUser.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.cbbChangeUser.Click += new System.EventHandler(this.cbbChangeUser_Click);
            // 
            // cbseClose
            // 
            this.cbseClose.DisplayName = "commandBarStripElement2";
            this.cbseClose.EnableDragging = false;
            this.cbseClose.Items.AddRange(new Telerik.WinControls.UI.RadCommandBarBaseItem[] {
            this.cblExit,
            this.cbbExit});
            this.cbseClose.Name = "cbseClose";
            // 
            // 
            // 
            this.cbseClose.OverflowButton.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.cbseClose.StretchHorizontally = true;
            ((Telerik.WinControls.UI.RadCommandBarOverflowButton)(this.cbseClose.GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // cblExit
            // 
            this.cblExit.DisplayName = "commandBarLabel1";
            this.cblExit.Name = "cblExit";
            this.cblExit.StretchHorizontally = true;
            this.cblExit.Text = " ";
            this.cblExit.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            // 
            // cbbExit
            // 
            this.cbbExit.DisplayName = "commandBarButton4";
            this.cbbExit.DrawText = true;
            this.cbbExit.Image = ((System.Drawing.Image)(resources.GetObject("cbbExit.Image")));
            this.cbbExit.Name = "cbbExit";
            this.cbbExit.Text = "Kapat";
            this.cbbExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.cbbExit.Click += new System.EventHandler(this.cbbExit_Click);
            // 
            // radCommandBar1
            // 
            this.radCommandBar1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radCommandBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radCommandBar1.Location = new System.Drawing.Point(0, 0);
            this.radCommandBar1.Name = "radCommandBar1";
            this.radCommandBar1.Rows.AddRange(new Telerik.WinControls.UI.CommandBarRowElement[] {
            this.cbreButtons,
            this.cbreMessage});
            this.radCommandBar1.Size = new System.Drawing.Size(662, 51);
            this.radCommandBar1.TabIndex = 0;
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
            this.cbseMessage.DisplayName = "commandBarStripElement3";
            this.cbseMessage.Items.AddRange(new Telerik.WinControls.UI.RadCommandBarBaseItem[] {
            this.cblMessage,
            this.cbbCloseMessage});
            this.cbseMessage.Name = "cbseMessage";
            // 
            // 
            // 
            this.cbseMessage.OverflowButton.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.cbseMessage.StretchHorizontally = true;
            ((Telerik.WinControls.UI.RadCommandBarOverflowButton)(this.cbseMessage.GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // cblMessage
            // 
            this.cblMessage.DisplayName = "commandBarLabel1";
            this.cblMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.cblMessage.Name = "cblMessage";
            this.cblMessage.StretchHorizontally = true;
            this.cblMessage.Text = "Silinme Mesajı";
            this.cblMessage.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cblMessage.TextChanged += new System.EventHandler(this.cblMessage_TextChanged);
            // 
            // cbbCloseMessage
            // 
            this.cbbCloseMessage.DisplayName = "commandBarButton2";
            this.cbbCloseMessage.DrawText = true;
            this.cbbCloseMessage.Image = ((System.Drawing.Image)(resources.GetObject("cbbCloseMessage.Image")));
            this.cbbCloseMessage.Name = "cbbCloseMessage";
            this.cbbCloseMessage.Text = "Gizle";
            this.cbbCloseMessage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.cbbCloseMessage.Click += new System.EventHandler(this.cbbCloseMessage_Click);
            // 
            // fUserList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 267);
            this.ControlBox = false;
            this.Controls.Add(this.rgvUserList);
            this.Controls.Add(this.radCommandBar1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(670, 300);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(670, 300);
            this.Name = "FUserList";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(670, 300);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KULLANICILAR";
            this.Load += new System.EventHandler(this.fUserList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rgvUserList.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvUserList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.UI.RadGridView rgvUserList;
        private Telerik.WinControls.UI.CommandBarRowElement cbreButtons;
        private Telerik.WinControls.UI.CommandBarStripElement cbseMain;
        private Telerik.WinControls.UI.CommandBarButton cbbAdd;
        private Telerik.WinControls.UI.CommandBarButton cbbEdit;
        private Telerik.WinControls.UI.CommandBarButton cbbDelete;
        private Telerik.WinControls.UI.CommandBarStripElement cbseClose;
        private Telerik.WinControls.UI.RadCommandBar radCommandBar1;
        private Telerik.WinControls.UI.CommandBarLabel cblExit;
        private Telerik.WinControls.UI.CommandBarButton cbbExit;
        private Telerik.WinControls.UI.CommandBarRowElement cbreMessage;
        private Telerik.WinControls.UI.CommandBarStripElement cbseMessage;
        private Telerik.WinControls.UI.CommandBarLabel cblMessage;
        private Telerik.WinControls.UI.CommandBarButton cbbCloseMessage;
        private Telerik.WinControls.UI.CommandBarButton cbbChangeUser;
    }
}
