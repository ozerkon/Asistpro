namespace SgkAssistant.Forms.Defs
{
    partial class FCompanyCenter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FCompanyCenter));
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.radPictureBox1 = new Telerik.WinControls.UI.RadPictureBox();
            this.lblQuestion = new Telerik.WinControls.UI.RadLabel();
            this.rgvCompanyList = new Telerik.WinControls.UI.RadGridView();
            this.ddlCenters = new Telerik.WinControls.UI.RadDropDownList();
            this.btnCancel = new Telerik.WinControls.UI.RadButton();
            this.btnSetFm = new Telerik.WinControls.UI.RadButton();
            this.lblMessage = new Telerik.WinControls.UI.RadLabel();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.radPictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblQuestion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvCompanyList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvCompanyList.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlCenters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSetFm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMessage)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPictureBox1
            // 
            this.radPictureBox1.DefaultSvgImageXml = resources.GetString("radPictureBox1.DefaultSvgImageXml");
            this.radPictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.radPictureBox1.Image = global::SgkAssistant.Properties.Resources.info_squared_96;
            this.radPictureBox1.Location = new System.Drawing.Point(0, 0);
            this.radPictureBox1.Name = "radPictureBox1";
            this.radPictureBox1.Size = new System.Drawing.Size(96, 101);
            this.radPictureBox1.TabIndex = 0;
            // 
            // lblQuestion
            // 
            this.lblQuestion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblQuestion.AutoSize = false;
            this.lblQuestion.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblQuestion.Location = new System.Drawing.Point(102, 20);
            this.lblQuestion.Name = "lblQuestion";
            this.lblQuestion.Size = new System.Drawing.Size(748, 69);
            this.lblQuestion.TabIndex = 4;
            this.lblQuestion.Text = "Asistpro\'nun ne yapmasını istersiniz?";
            this.lblQuestion.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            // 
            // rgvCompanyList
            // 
            this.rgvCompanyList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgvCompanyList.Location = new System.Drawing.Point(3, 21);
            // 
            // 
            // 
            this.rgvCompanyList.MasterTemplate.AllowAddNewRow = false;
            this.rgvCompanyList.MasterTemplate.AllowCellContextMenu = false;
            this.rgvCompanyList.MasterTemplate.AllowColumnChooser = false;
            this.rgvCompanyList.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.rgvCompanyList.MasterTemplate.AllowColumnReorder = false;
            this.rgvCompanyList.MasterTemplate.AllowColumnResize = false;
            this.rgvCompanyList.MasterTemplate.AllowDeleteRow = false;
            this.rgvCompanyList.MasterTemplate.AllowDragToGroup = false;
            this.rgvCompanyList.MasterTemplate.AllowEditRow = false;
            this.rgvCompanyList.MasterTemplate.AllowRowHeaderContextMenu = false;
            this.rgvCompanyList.MasterTemplate.AllowRowResize = false;
            this.rgvCompanyList.MasterTemplate.EnableAlternatingRowColor = true;
            this.rgvCompanyList.MasterTemplate.MultiSelect = true;
            this.rgvCompanyList.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rgvCompanyList.Name = "rgvCompanyList";
            this.rgvCompanyList.Size = new System.Drawing.Size(856, 304);
            this.rgvCompanyList.TabIndex = 5;
            // 
            // ddlCenters
            // 
            this.ddlCenters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlCenters.DropDownAnimationEnabled = true;
            this.ddlCenters.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.ddlCenters.Location = new System.Drawing.Point(0, 5);
            this.ddlCenters.Name = "ddlCenters";
            this.ddlCenters.Size = new System.Drawing.Size(856, 24);
            this.ddlCenters.TabIndex = 6;
            this.ddlCenters.Text = "firma merkezleri";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Image = global::SgkAssistant.Properties.Resources.cancel;
            this.btnCancel.Location = new System.Drawing.Point(447, 32);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(148, 36);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Kapat";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSetFm
            // 
            this.btnSetFm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetFm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSetFm.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnSetFm.Image = global::SgkAssistant.Properties.Resources.approval;
            this.btnSetFm.Location = new System.Drawing.Point(259, 32);
            this.btnSetFm.Name = "btnSetFm";
            this.btnSetFm.Size = new System.Drawing.Size(148, 36);
            this.btnSetFm.TabIndex = 8;
            this.btnSetFm.Text = "Ayarla";
            this.btnSetFm.Click += new System.EventHandler(this.btnSetFm_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = false;
            this.tableLayoutPanel1.SetColumnSpan(this.lblMessage, 5);
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblMessage.ForeColor = System.Drawing.Color.Red;
            this.lblMessage.Location = new System.Drawing.Point(3, 3);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(850, 18);
            this.lblMessage.TabIndex = 10;
            this.lblMessage.Text = "Mesaj";
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.lblQuestion);
            this.pnlTop.Controls.Add(this.radPictureBox1);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(862, 101);
            this.pnlTop.TabIndex = 11;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rgvCompanyList);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.groupBox1.Location = new System.Drawing.Point(0, 101);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(862, 328);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Merkezi belirli olmayan firmalar";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel1);
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.groupBox2.Location = new System.Drawing.Point(0, 429);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(862, 138);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Seçilen firmalar için merkez firmayı belirle";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnSetFm, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblMessage, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 58);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(856, 77);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ddlCenters);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 21);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.panel1.Size = new System.Drawing.Size(856, 34);
            this.panel1.TabIndex = 11;
            // 
            // FCompanyCenter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 567);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1000, 1200);
            this.MinimumSize = new System.Drawing.Size(850, 600);
            this.Name = "FCompanyCenter";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(1000, 1200);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FİRMA MERKEZLERİNİ AYARLA";
            this.Load += new System.EventHandler(this.FCompanyCenter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radPictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblQuestion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvCompanyList.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvCompanyList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlCenters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSetFm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMessage)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPictureBox radPictureBox1;
        private Telerik.WinControls.UI.RadLabel lblQuestion;
        private Telerik.WinControls.UI.RadGridView rgvCompanyList;
        private Telerik.WinControls.UI.RadDropDownList ddlCenters;
        private Telerik.WinControls.UI.RadButton btnCancel;
        private Telerik.WinControls.UI.RadButton btnSetFm;
        private Telerik.WinControls.UI.RadLabel lblMessage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel1;
    }
}
