namespace SgkAssistant.Forms.Common
{
    partial class FLicence
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FLicence));
            this.btnCancel = new Telerik.WinControls.UI.RadButton();
            this.btnChangeLicence = new Telerik.WinControls.UI.RadButton();
            this.lblLicenceMessage = new System.Windows.Forms.Label();
            this.texLicencePass = new Telerik.WinControls.UI.RadTextBoxControl();
            this.texEmail = new Telerik.WinControls.UI.RadTextBoxControl();
            this.texCode6 = new Telerik.WinControls.UI.RadTextBox();
            this.texCode5 = new Telerik.WinControls.UI.RadTextBox();
            this.texCode4 = new Telerik.WinControls.UI.RadTextBox();
            this.texCode3 = new Telerik.WinControls.UI.RadTextBox();
            this.texCode2 = new Telerik.WinControls.UI.RadTextBox();
            this.texCode1 = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnChangeLicence)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.texLicencePass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.texEmail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.texCode6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.texCode5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.texCode4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.texCode3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.texCode2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.texCode1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Location = new System.Drawing.Point(240, 216);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 24);
            this.btnCancel.TabIndex = 47;
            this.btnCancel.Text = "Vazgeç";
            this.btnCancel.ThemeName = "VisualStudio2012Light";
            // 
            // btnChangeLicence
            // 
            this.btnChangeLicence.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnChangeLicence.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnChangeLicence.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnChangeLicence.Location = new System.Drawing.Point(88, 216);
            this.btnChangeLicence.Name = "btnChangeLicence";
            this.btnChangeLicence.Size = new System.Drawing.Size(110, 24);
            this.btnChangeLicence.TabIndex = 48;
            this.btnChangeLicence.Text = "Değiştir";
            this.btnChangeLicence.ThemeName = "VisualStudio2012Light";
            this.btnChangeLicence.Click += new System.EventHandler(this.btnChangeLicence_Click);
            // 
            // lblLicenceMessage
            // 
            this.lblLicenceMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblLicenceMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblLicenceMessage.ForeColor = System.Drawing.Color.Red;
            this.lblLicenceMessage.Location = new System.Drawing.Point(1, 247);
            this.lblLicenceMessage.Name = "lblLicenceMessage";
            this.lblLicenceMessage.Size = new System.Drawing.Size(450, 39);
            this.lblLicenceMessage.TabIndex = 46;
            this.lblLicenceMessage.Text = "Mesaj";
            this.lblLicenceMessage.Visible = false;
            this.lblLicenceMessage.TextChanged += new System.EventHandler(this.lblLicenceMessage_TextChanged);
            // 
            // texLicencePass
            // 
            this.texLicencePass.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.texLicencePass.Location = new System.Drawing.Point(12, 172);
            this.texLicencePass.Name = "texLicencePass";
            this.texLicencePass.NullText = "Şifreniz";
            this.texLicencePass.ShowClearButton = true;
            this.texLicencePass.ShowNullText = true;
            this.texLicencePass.Size = new System.Drawing.Size(420, 27);
            this.texLicencePass.TabIndex = 44;
            this.texLicencePass.WordWrap = false;
            this.texLicencePass.Enter += new System.EventHandler(this.text_Enter);
            ((Telerik.WinControls.UI.RadTextBoxControlElement)(this.texLicencePass.GetChildAt(0))).BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(189)))), ((int)(((byte)(232)))));
            // 
            // texEmail
            // 
            this.texEmail.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.texEmail.Location = new System.Drawing.Point(12, 127);
            this.texEmail.Name = "texEmail";
            this.texEmail.NullText = "E-posta adresiniz";
            this.texEmail.ShowClearButton = true;
            this.texEmail.ShowNullText = true;
            this.texEmail.Size = new System.Drawing.Size(420, 27);
            this.texEmail.TabIndex = 43;
            this.texEmail.WordWrap = false;
            this.texEmail.Enter += new System.EventHandler(this.text_Enter);
            ((Telerik.WinControls.UI.RadTextBoxControlElement)(this.texEmail.GetChildAt(0))).BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(189)))), ((int)(((byte)(232)))));
            // 
            // texCode6
            // 
            this.texCode6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.texCode6.Location = new System.Drawing.Point(392, 84);
            this.texCode6.MaxLength = 2;
            this.texCode6.Name = "texCode6";
            this.texCode6.Size = new System.Drawing.Size(40, 31);
            this.texCode6.TabIndex = 42;
            this.texCode6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.texCode6.Enter += new System.EventHandler(this.texCode_Enter);
            this.texCode6.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            this.texCode6.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.texCode_KeyPress);
            this.texCode6.KeyUp += new System.Windows.Forms.KeyEventHandler(this.texCode_KeyUp);
            this.texCode6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox_MouseDown);
            // 
            // texCode5
            // 
            this.texCode5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.texCode5.Location = new System.Drawing.Point(316, 84);
            this.texCode5.MaxLength = 4;
            this.texCode5.Name = "texCode5";
            this.texCode5.Size = new System.Drawing.Size(70, 31);
            this.texCode5.TabIndex = 41;
            this.texCode5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.texCode5.Enter += new System.EventHandler(this.texCode_Enter);
            this.texCode5.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            this.texCode5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.texCode_KeyPress);
            this.texCode5.KeyUp += new System.Windows.Forms.KeyEventHandler(this.texCode_KeyUp);
            this.texCode5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox_MouseDown);
            // 
            // texCode4
            // 
            this.texCode4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.texCode4.Location = new System.Drawing.Point(240, 84);
            this.texCode4.MaxLength = 4;
            this.texCode4.Name = "texCode4";
            this.texCode4.Size = new System.Drawing.Size(70, 31);
            this.texCode4.TabIndex = 40;
            this.texCode4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.texCode4.Enter += new System.EventHandler(this.texCode_Enter);
            this.texCode4.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            this.texCode4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.texCode_KeyPress);
            this.texCode4.KeyUp += new System.Windows.Forms.KeyEventHandler(this.texCode_KeyUp);
            this.texCode4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox_MouseDown);
            // 
            // texCode3
            // 
            this.texCode3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.texCode3.Location = new System.Drawing.Point(164, 84);
            this.texCode3.MaxLength = 4;
            this.texCode3.Name = "texCode3";
            this.texCode3.Size = new System.Drawing.Size(70, 31);
            this.texCode3.TabIndex = 39;
            this.texCode3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.texCode3.Enter += new System.EventHandler(this.texCode_Enter);
            this.texCode3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            this.texCode3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.texCode_KeyPress);
            this.texCode3.KeyUp += new System.Windows.Forms.KeyEventHandler(this.texCode_KeyUp);
            this.texCode3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox_MouseDown);
            // 
            // texCode2
            // 
            this.texCode2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.texCode2.Location = new System.Drawing.Point(88, 84);
            this.texCode2.MaxLength = 4;
            this.texCode2.Name = "texCode2";
            this.texCode2.Size = new System.Drawing.Size(70, 31);
            this.texCode2.TabIndex = 38;
            this.texCode2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.texCode2.Enter += new System.EventHandler(this.texCode_Enter);
            this.texCode2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            this.texCode2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.texCode_KeyPress);
            this.texCode2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.texCode_KeyUp);
            this.texCode2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox_MouseDown);
            // 
            // texCode1
            // 
            this.texCode1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.texCode1.Location = new System.Drawing.Point(12, 84);
            this.texCode1.MaxLength = 4;
            this.texCode1.Name = "texCode1";
            this.texCode1.Size = new System.Drawing.Size(70, 31);
            this.texCode1.TabIndex = 37;
            this.texCode1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.texCode1.Enter += new System.EventHandler(this.texCode_Enter);
            this.texCode1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyDown);
            this.texCode1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.texCode_KeyPress);
            this.texCode1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.texCode_KeyUp);
            this.texCode1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox_MouseDown);
            // 
            // radLabel1
            // 
            this.radLabel1.AutoSize = false;
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.radLabel1.Location = new System.Drawing.Point(12, 6);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(420, 64);
            this.radLabel1.TabIndex = 45;
            this.radLabel1.Text = resources.GetString("radLabel1.Text");
            // 
            // FLicence
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.ClientSize = new System.Drawing.Size(452, 287);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnChangeLicence);
            this.Controls.Add(this.lblLicenceMessage);
            this.Controls.Add(this.texLicencePass);
            this.Controls.Add(this.texEmail);
            this.Controls.Add(this.texCode6);
            this.Controls.Add(this.texCode5);
            this.Controls.Add(this.texCode4);
            this.Controls.Add(this.texCode3);
            this.Controls.Add(this.texCode2);
            this.Controls.Add(this.texCode1);
            this.Controls.Add(this.radLabel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(460, 320);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(460, 320);
            this.Name = "FLicence";
            this.Padding = new System.Windows.Forms.Padding(1);
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(460, 320);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LİSANS TÜRÜNÜ DEĞİŞTİR";
            this.Load += new System.EventHandler(this.fLicence_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnChangeLicence)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.texLicencePass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.texEmail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.texCode6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.texCode5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.texCode4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.texCode3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.texCode2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.texCode1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadButton btnCancel;
        private Telerik.WinControls.UI.RadButton btnChangeLicence;
        private System.Windows.Forms.Label lblLicenceMessage;
        private Telerik.WinControls.UI.RadTextBoxControl texLicencePass;
        private Telerik.WinControls.UI.RadTextBoxControl texEmail;
        private Telerik.WinControls.UI.RadTextBox texCode6;
        private Telerik.WinControls.UI.RadTextBox texCode5;
        private Telerik.WinControls.UI.RadTextBox texCode4;
        private Telerik.WinControls.UI.RadTextBox texCode3;
        private Telerik.WinControls.UI.RadTextBox texCode2;
        private Telerik.WinControls.UI.RadTextBox texCode1;
        private Telerik.WinControls.UI.RadLabel radLabel1;
    }
}
