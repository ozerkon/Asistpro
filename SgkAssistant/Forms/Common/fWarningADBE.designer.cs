namespace SgkAssistant.Forms.Defs
{
    partial class fWarningADBE
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fWarningADBE));
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.lblAccessError = new Telerik.WinControls.UI.RadLabel();
            this.lblMessage = new Telerik.WinControls.UI.RadLabel();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.btnContinue = new Telerik.WinControls.UI.RadButton();
            this.roundRectShape1 = new Telerik.WinControls.RoundRectShape(this.components);
            this.btnTest = new Telerik.WinControls.UI.RadButton();
            this.btnCancel = new Telerik.WinControls.UI.RadButton();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAccessError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMessage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnContinue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnTest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel1
            // 
            this.radLabel1.AutoSize = false;
            this.radLabel1.Location = new System.Drawing.Point(3, 31);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(466, 118);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = resources.GetString("radLabel1.Text");
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.linkLabel1.Location = new System.Drawing.Point(240, 159);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(167, 19);
            this.linkLabel1.TabIndex = 1;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Access Database Engine";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // lblAccessError
            // 
            this.lblAccessError.AutoSize = false;
            this.lblAccessError.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAccessError.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblAccessError.ForeColor = System.Drawing.Color.Red;
            this.lblAccessError.Location = new System.Drawing.Point(0, 0);
            this.lblAccessError.Name = "lblAccessError";
            this.lblAccessError.Size = new System.Drawing.Size(472, 24);
            this.lblAccessError.TabIndex = 3;
            this.lblAccessError.Text = "<html>Bilgisayarınızda MS Access yüklü değil!</html>";
            this.lblAccessError.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMessage.ForeColor = System.Drawing.Color.Navy;
            this.lblMessage.Location = new System.Drawing.Point(3, 3);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(444, 19);
            this.lblMessage.TabIndex = 4;
            this.lblMessage.Text = "Access Database Engine bulunamadı, kurulumu kontrol edip tekrar deneyin";
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.lblMessage);
            this.radPanel1.Controls.Add(this.btnContinue);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.radPanel1.Location = new System.Drawing.Point(0, 224);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(472, 30);
            this.radPanel1.TabIndex = 5;
            this.radPanel1.Visible = false;
            ((Telerik.WinControls.UI.RadPanelElement)(this.radPanel1.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanel1.GetChildAt(0).GetChildAt(1))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // btnContinue
            // 
            this.btnContinue.Image = ((System.Drawing.Image)(resources.GetObject("btnContinue.Image")));
            this.btnContinue.Location = new System.Drawing.Point(379, 1);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Padding = new System.Windows.Forms.Padding(3, 0, 5, 0);
            this.btnContinue.Size = new System.Drawing.Size(89, 28);
            this.btnContinue.TabIndex = 5;
            this.btnContinue.Text = "Devam Et";
            this.btnContinue.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnContinue.GetChildAt(0))).Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnContinue.GetChildAt(0))).TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnContinue.GetChildAt(0))).Text = "Devam Et";
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnContinue.GetChildAt(0))).ToolTipText = "Devam Et";
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnContinue.GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnContinue.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(3, 0, 5, 0);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.btnContinue.GetChildAt(0).GetChildAt(0))).NumberOfColors = 1;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.btnContinue.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btnContinue.GetChildAt(0).GetChildAt(2))).Shape = this.roundRectShape1;
            // 
            // btnTest
            // 
            this.btnTest.BackColor = System.Drawing.Color.Transparent;
            this.btnTest.Image = ((System.Drawing.Image)(resources.GetObject("btnTest.Image")));
            this.btnTest.Location = new System.Drawing.Point(116, 193);
            this.btnTest.Name = "btnTest";
            this.btnTest.Padding = new System.Windows.Forms.Padding(3, 0, 5, 0);
            this.btnTest.Size = new System.Drawing.Size(76, 28);
            this.btnTest.TabIndex = 2;
            this.btnTest.Text = "Test Et";
            this.btnTest.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnTest.GetChildAt(0))).Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnTest.GetChildAt(0))).TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnTest.GetChildAt(0))).Text = "Test Et";
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnTest.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(3, 0, 5, 0);
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btnTest.GetChildAt(0).GetChildAt(2))).Shape = this.roundRectShape1;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(281, 193);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(3, 0, 5, 0);
            this.btnCancel.Size = new System.Drawing.Size(76, 28);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Vazgeç";
            this.btnCancel.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnCancel.GetChildAt(0))).Image = ((System.Drawing.Image)(resources.GetObject("resource.Image2")));
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnCancel.GetChildAt(0))).TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnCancel.GetChildAt(0))).Text = "Vazgeç";
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnCancel.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(3, 0, 5, 0);
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btnCancel.GetChildAt(0).GetChildAt(2))).Shape = this.roundRectShape1;
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.linkLabel2.Location = new System.Drawing.Point(66, 159);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(121, 19);
            this.linkLabel2.TabIndex = 6;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Microsoft Access";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // fWarningADBE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 254);
            this.ControlBox = false;
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.radPanel1);
            this.Controls.Add(this.lblAccessError);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.radLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(480, 284);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(480, 284);
            this.Name = "fWarningADBE";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(480, 284);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MS Access Hatası";
            this.Load += new System.EventHandler(this.fWarningADBE_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAccessError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMessage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.radPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnContinue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnTest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel radLabel1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private Telerik.WinControls.UI.RadButton btnCancel;
        private Telerik.WinControls.UI.RadButton btnTest;
        private Telerik.WinControls.UI.RadLabel lblAccessError;
        private Telerik.WinControls.RoundRectShape roundRectShape1;
        private Telerik.WinControls.UI.RadButton btnContinue;
        private Telerik.WinControls.UI.RadLabel lblMessage;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private System.Windows.Forms.LinkLabel linkLabel2;
    }
}
