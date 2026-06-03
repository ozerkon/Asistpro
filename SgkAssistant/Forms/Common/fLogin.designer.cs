namespace SgkAssistant.Forms.Sgk
{
    partial class FLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FLogin));
            this.label1 = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.radPanel2 = new Telerik.WinControls.UI.RadPanel();
            this.btnRefresh = new Telerik.WinControls.UI.RadButton();
            this.roundRectShape1 = new Telerik.WinControls.RoundRectShape(this.components);
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.texUname = new Telerik.WinControls.UI.RadTextBoxControl();
            this.texUpass = new Telerik.WinControls.UI.RadTextBoxControl();
            this.btnCancel = new Telerik.WinControls.UI.RadButton();
            this.btnCreateFirstUser = new Telerik.WinControls.UI.RadButton();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.btnLogin = new Telerik.WinControls.UI.RadButton();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel2)).BeginInit();
            this.radPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnRefresh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.texUname)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.texUpass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCreateFirstUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLogin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(105)))), ((int)(((byte)(144)))));
            this.label1.Font = new System.Drawing.Font("Segoe UI", 24F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(186, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 45);
            this.label1.TabIndex = 1;
            this.label1.Text = "ASİSTPRO";
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblMessage.ForeColor = System.Drawing.Color.Red;
            this.lblMessage.Location = new System.Drawing.Point(0, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(56, 21);
            this.lblMessage.TabIndex = 9;
            this.lblMessage.Text = "Label2";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // radPanel1
            // 
            this.radPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.radPanel1.Controls.Add(this.radPanel2);
            this.radPanel1.Controls.Add(this.lblMessage);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.radPanel1.Location = new System.Drawing.Point(0, 276);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(560, 34);
            this.radPanel1.TabIndex = 18;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanel1.GetChildAt(0).GetChildAt(1))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanel1.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radPanel2
            // 
            this.radPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.radPanel2.Controls.Add(this.btnRefresh);
            this.radPanel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.radPanel2.Location = new System.Drawing.Point(457, 0);
            this.radPanel2.Name = "radPanel2";
            this.radPanel2.Size = new System.Drawing.Size(103, 34);
            this.radPanel2.TabIndex = 19;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanel2.GetChildAt(0).GetChildAt(1))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanel2.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnRefresh.BackColor = System.Drawing.Color.Transparent;
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.Image = global::SgkAssistant.Properties.Resources.refresh;
            this.btnRefresh.Location = new System.Drawing.Point(3, 6);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.btnRefresh.Size = new System.Drawing.Size(94, 24);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "Tekrar Dene";
            this.btnRefresh.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnRefresh.GetChildAt(0))).Image = global::SgkAssistant.Properties.Resources.refresh;
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnRefresh.GetChildAt(0))).TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnRefresh.GetChildAt(0))).Text = "Tekrar Dene";
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnRefresh.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnRefresh.GetChildAt(0))).Shape = this.roundRectShape1;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btnRefresh.GetChildAt(0).GetChildAt(2))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(148)))), ((int)(((byte)(186)))));
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btnRefresh.GetChildAt(0).GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // radLabel1
            // 
            this.radLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(105)))), ((int)(((byte)(144)))));
            this.radLabel1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.radLabel1.ForeColor = System.Drawing.Color.White;
            this.radLabel1.Location = new System.Drawing.Point(376, 325);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(177, 24);
            this.radLabel1.TabIndex = 19;
            this.radLabel1.Text = "<html>© 2022 www.sinerjia.net</html>";
            this.radLabel1.Click += new System.EventHandler(this.radLabel1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.label2.Location = new System.Drawing.Point(68, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 30);
            this.label2.TabIndex = 20;
            this.label2.Text = "Kullanıcı Adı";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.label3.Location = new System.Drawing.Point(67, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 30);
            this.label3.TabIndex = 20;
            this.label3.Text = "Şifre";
            // 
            // texUname
            // 
            this.texUname.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.texUname.BackColor = System.Drawing.Color.Transparent;
            this.texUname.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.texUname.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.texUname.ForeColor = System.Drawing.Color.Black;
            this.texUname.Location = new System.Drawing.Point(203, 80);
            this.texUname.Name = "texUname";
            this.texUname.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            // 
            // 
            // 
            this.texUname.RootElement.Shape = this.roundRectShape1;
            this.texUname.ShowClearButton = true;
            this.texUname.ShowNullText = true;
            this.texUname.Size = new System.Drawing.Size(250, 40);
            this.texUname.TabIndex = 21;
            this.texUname.WordWrap = false;
            ((Telerik.WinControls.UI.RadTextBoxControlElement)(this.texUname.GetChildAt(0))).DrawBorder = true;
            ((Telerik.WinControls.UI.RadTextBoxControlElement)(this.texUname.GetChildAt(0))).DrawBackgroundImage = true;
            ((Telerik.WinControls.UI.RadTextBoxControlElement)(this.texUname.GetChildAt(0))).BorderWidth = 1F;
            ((Telerik.WinControls.UI.RadTextBoxControlElement)(this.texUname.GetChildAt(0))).BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(148)))), ((int)(((byte)(186)))));
            ((Telerik.WinControls.UI.RadTextBoxControlElement)(this.texUname.GetChildAt(0))).BackgroundImage = ((System.Drawing.Image)(resources.GetObject("resource.BackgroundImage")));
            ((Telerik.WinControls.UI.RadTextBoxControlElement)(this.texUname.GetChildAt(0))).FocusBorderWidth = 2;
            ((Telerik.WinControls.UI.RadTextBoxControlElement)(this.texUname.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            ((Telerik.WinControls.UI.RadTextBoxControlElement)(this.texUname.GetChildAt(0))).Shape = this.roundRectShape1;
            ((Telerik.WinControls.UI.RadScrollBarElement)(this.texUname.GetChildAt(0).GetChildAt(3))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.texUname.GetChildAt(0).GetChildAt(3).GetChildAt(5))).BoxStyle = Telerik.WinControls.BorderBoxStyle.SingleBorder;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.texUname.GetChildAt(0).GetChildAt(3).GetChildAt(5))).BackColor = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.texUname.GetChildAt(0).GetChildAt(3).GetChildAt(5))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.texUname.GetChildAt(0).GetChildAt(3).GetChildAt(5))).CanFocus = false;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.texUname.GetChildAt(0).GetChildAt(3).GetChildAt(5))).AngleTransform = -6030F;
            // 
            // texUpass
            // 
            this.texUpass.BackColor = System.Drawing.Color.Transparent;
            this.texUpass.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.texUpass.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.texUpass.ForeColor = System.Drawing.Color.Black;
            this.texUpass.Location = new System.Drawing.Point(202, 135);
            this.texUpass.Name = "texUpass";
            this.texUpass.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.texUpass.PasswordChar = '*';
            this.texUpass.ShowClearButton = true;
            this.texUpass.ShowNullText = true;
            this.texUpass.Size = new System.Drawing.Size(250, 40);
            this.texUpass.TabIndex = 2;
            this.texUpass.WordWrap = false;
            this.texUpass.KeyUp += new System.Windows.Forms.KeyEventHandler(this.texUpass_KeyUp);
            ((Telerik.WinControls.UI.RadTextBoxControlElement)(this.texUpass.GetChildAt(0))).DrawBorder = true;
            ((Telerik.WinControls.UI.RadTextBoxControlElement)(this.texUpass.GetChildAt(0))).BorderWidth = 1F;
            ((Telerik.WinControls.UI.RadTextBoxControlElement)(this.texUpass.GetChildAt(0))).BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(148)))), ((int)(((byte)(186)))));
            ((Telerik.WinControls.UI.RadTextBoxControlElement)(this.texUpass.GetChildAt(0))).BackgroundImage = ((System.Drawing.Image)(resources.GetObject("resource.BackgroundImage1")));
            ((Telerik.WinControls.UI.RadTextBoxControlElement)(this.texUpass.GetChildAt(0))).FocusBorderWidth = 2;
            ((Telerik.WinControls.UI.RadTextBoxControlElement)(this.texUpass.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            ((Telerik.WinControls.UI.RadTextBoxControlElement)(this.texUpass.GetChildAt(0))).Shape = this.roundRectShape1;
            ((Telerik.WinControls.UI.RadScrollBarElement)(this.texUpass.GetChildAt(0).GetChildAt(3))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.texUpass.GetChildAt(0).GetChildAt(3).GetChildAt(5))).BackColor = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.texUpass.GetChildAt(0).GetChildAt(3).GetChildAt(5))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.texUpass.GetChildAt(0).GetChildAt(3).GetChildAt(5))).AngleTransform = -5940F;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Image = global::SgkAssistant.Properties.Resources.cancel_32;
            this.btnCancel.Location = new System.Drawing.Point(352, 195);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(1, 1, 5, 1);
            this.btnCancel.Size = new System.Drawing.Size(100, 40);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Vazgeç";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnCancel.GetChildAt(0))).Image = global::SgkAssistant.Properties.Resources.cancel_32;
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnCancel.GetChildAt(0))).TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnCancel.GetChildAt(0))).TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnCancel.GetChildAt(0))).Text = "Vazgeç";
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnCancel.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(1, 1, 5, 1);
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnCancel.GetChildAt(0))).Shape = this.roundRectShape1;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btnCancel.GetChildAt(0).GetChildAt(2))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(148)))), ((int)(((byte)(186)))));
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btnCancel.GetChildAt(0).GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // btnCreateFirstUser
            // 
            this.btnCreateFirstUser.BackColor = System.Drawing.Color.Transparent;
            this.btnCreateFirstUser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCreateFirstUser.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.btnCreateFirstUser.ForeColor = System.Drawing.Color.Red;
            this.btnCreateFirstUser.Image = global::SgkAssistant.Properties.Resources.addPerson;
            this.btnCreateFirstUser.Location = new System.Drawing.Point(202, 242);
            this.btnCreateFirstUser.Name = "btnCreateFirstUser";
            this.btnCreateFirstUser.Size = new System.Drawing.Size(249, 28);
            this.btnCreateFirstUser.TabIndex = 16;
            this.btnCreateFirstUser.Text = "Yeni Kullanıcı Oluştur";
            this.btnCreateFirstUser.Click += new System.EventHandler(this.btnCreateFirstUser_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnCreateFirstUser.GetChildAt(0))).Image = global::SgkAssistant.Properties.Resources.addPerson;
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnCreateFirstUser.GetChildAt(0))).Text = "Yeni Kullanıcı Oluştur";
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btnCreateFirstUser.GetChildAt(0).GetChildAt(2))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(148)))), ((int)(((byte)(186)))));
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btnCreateFirstUser.GetChildAt(0).GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Visible;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btnCreateFirstUser.GetChildAt(0).GetChildAt(2))).Shape = this.roundRectShape1;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(105)))), ((int)(((byte)(144)))));
            this.pictureBox4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pictureBox4.Location = new System.Drawing.Point(0, 310);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(560, 50);
            this.pictureBox4.TabIndex = 13;
            this.pictureBox4.TabStop = false;
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.Transparent;
            this.btnLogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnLogin.ForeColor = System.Drawing.Color.Black;
            this.btnLogin.Image = global::SgkAssistant.Properties.Resources.login_32;
            this.btnLogin.Location = new System.Drawing.Point(202, 195);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Padding = new System.Windows.Forms.Padding(1, 1, 5, 1);
            this.btnLogin.Size = new System.Drawing.Size(100, 40);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "Giriş Yap";
            this.btnLogin.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnLogin.GetChildAt(0))).Image = global::SgkAssistant.Properties.Resources.login_32;
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnLogin.GetChildAt(0))).TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnLogin.GetChildAt(0))).TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnLogin.GetChildAt(0))).Text = "Giriş Yap";
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnLogin.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(1, 1, 5, 1);
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnLogin.GetChildAt(0))).Shape = this.roundRectShape1;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btnLogin.GetChildAt(0).GetChildAt(2))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(148)))), ((int)(((byte)(186)))));
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btnLogin.GetChildAt(0).GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(105)))), ((int)(((byte)(144)))));
            this.pictureBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox3.Location = new System.Drawing.Point(0, 0);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(560, 50);
            this.pictureBox3.TabIndex = 3;
            this.pictureBox3.TabStop = false;
            // 
            // FLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(560, 360);
            this.ControlBox = false;
            this.Controls.Add(this.texUpass);
            this.Controls.Add(this.texUname);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.radPanel1);
            this.Controls.Add(this.btnCreateFirstUser);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FLogin";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kullanıcı Girişi";
            this.ThemeName = "ControlDefault";
            this.Load += new System.EventHandler(this.fLogin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.radPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel2)).EndInit();
            this.radPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnRefresh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.texUname)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.texUpass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCreateFirstUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLogin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox3;
        private Telerik.WinControls.UI.RadButton btnLogin;
        private Telerik.WinControls.UI.RadButton btnCancel;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.PictureBox pictureBox4;
        private Telerik.WinControls.UI.RadButton btnCreateFirstUser;
        private Telerik.WinControls.UI.RadButton btnRefresh;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadPanel radPanel2;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Telerik.WinControls.UI.RadTextBoxControl texUname;
        private Telerik.WinControls.UI.RadTextBoxControl texUpass;
        private Telerik.WinControls.RoundRectShape roundRectShape1;
    }
}