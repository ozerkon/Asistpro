namespace SgkAssistant.Forms.Common
{
    partial class FSplash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FSplash));
            this.radWaitingBar1 = new Telerik.WinControls.UI.RadWaitingBar();
            this.dotsSpinnerWaitingBarIndicatorElement1 = new Telerik.WinControls.UI.DotsSpinnerWaitingBarIndicatorElement();
            this.lblMessage = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.btnCancel = new Telerik.WinControls.UI.RadButton();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.radWaitingBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMessage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radWaitingBar1
            // 
            this.radWaitingBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.radWaitingBar1.EnableTheming = false;
            this.radWaitingBar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radWaitingBar1.Location = new System.Drawing.Point(256, 250);
            this.radWaitingBar1.Name = "radWaitingBar1";
            this.radWaitingBar1.Size = new System.Drawing.Size(70, 70);
            this.radWaitingBar1.TabIndex = 1;
            this.radWaitingBar1.Text = "YÜKLENİYOR";
            this.radWaitingBar1.WaitingIndicators.Add(this.dotsSpinnerWaitingBarIndicatorElement1);
            this.radWaitingBar1.WaitingIndicatorSize = new System.Drawing.Size(100, 14);
            this.radWaitingBar1.WaitingSpeed = 100;
            this.radWaitingBar1.WaitingStyle = Telerik.WinControls.Enumerations.WaitingBarStyles.DotsSpinner;
            ((Telerik.WinControls.UI.RadWaitingBarElement)(this.radWaitingBar1.GetChildAt(0))).WaitingIndicatorSize = new System.Drawing.Size(100, 14);
            ((Telerik.WinControls.UI.RadWaitingBarElement)(this.radWaitingBar1.GetChildAt(0))).WaitingSpeed = 100;
            ((Telerik.WinControls.UI.RadWaitingBarElement)(this.radWaitingBar1.GetChildAt(0))).DrawBackgroundImage = false;
            ((Telerik.WinControls.UI.RadWaitingBarElement)(this.radWaitingBar1.GetChildAt(0))).BorderColor = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.UI.RadWaitingBarElement)(this.radWaitingBar1.GetChildAt(0))).BackColor2 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.UI.RadWaitingBarElement)(this.radWaitingBar1.GetChildAt(0))).BackColor3 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.UI.RadWaitingBarElement)(this.radWaitingBar1.GetChildAt(0))).BackColor4 = System.Drawing.Color.Transparent;
            ((Telerik.WinControls.UI.RadWaitingBarElement)(this.radWaitingBar1.GetChildAt(0))).NumberOfColors = 1;
            ((Telerik.WinControls.UI.RadWaitingBarElement)(this.radWaitingBar1.GetChildAt(0))).ImageTransparentColor = System.Drawing.Color.Magenta;
            ((Telerik.WinControls.UI.WaitingBarContentElement)(this.radWaitingBar1.GetChildAt(0).GetChildAt(0))).WaitingStyle = Telerik.WinControls.Enumerations.WaitingBarStyles.DotsSpinner;
            ((Telerik.WinControls.UI.WaitingBarSeparatorElement)(this.radWaitingBar1.GetChildAt(0).GetChildAt(0).GetChildAt(0))).Dash = false;
            // 
            // dotsSpinnerWaitingBarIndicatorElement1
            // 
            this.dotsSpinnerWaitingBarIndicatorElement1.BackColor = System.Drawing.Color.Transparent;
            this.dotsSpinnerWaitingBarIndicatorElement1.BackColor2 = System.Drawing.Color.Transparent;
            this.dotsSpinnerWaitingBarIndicatorElement1.BackColor3 = System.Drawing.Color.Transparent;
            this.dotsSpinnerWaitingBarIndicatorElement1.BackColor4 = System.Drawing.Color.Transparent;
            this.dotsSpinnerWaitingBarIndicatorElement1.Name = "dotsSpinnerWaitingBarIndicatorElement1";
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMessage.ForeColor = System.Drawing.Color.Red;
            this.lblMessage.Location = new System.Drawing.Point(44, 299);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(79, 21);
            this.lblMessage.TabIndex = 2;
            this.lblMessage.Text = "lblMessage";
            ((Telerik.WinControls.UI.RadLabelElement)(this.lblMessage.GetChildAt(0))).Text = "lblMessage";
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.lblMessage.GetChildAt(0).GetChildAt(0))).NumberOfColors = 1;
            // 
            // radLabel1
            // 
            this.radLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(105)))), ((int)(((byte)(144)))));
            this.radLabel1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.radLabel1.ForeColor = System.Drawing.Color.White;
            this.radLabel1.Location = new System.Drawing.Point(367, 330);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(177, 24);
            this.radLabel1.TabIndex = 20;
            this.radLabel1.Text = "<html>© 2022 www.sinerjia.net</html>";
            this.radLabel1.Click += new System.EventHandler(this.radLabel1_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::SgkAssistant.Properties.Resources.cancel_32;
            this.btnCancel.Location = new System.Drawing.Point(512, 25);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(39, 32);
            this.btnCancel.TabIndex = 3;
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnCancel.GetChildAt(0))).Image = global::SgkAssistant.Properties.Resources.cancel_32;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btnCancel.GetChildAt(0).GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(105)))), ((int)(((byte)(144)))));
            this.pictureBox4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pictureBox4.Location = new System.Drawing.Point(0, 322);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(576, 50);
            this.pictureBox4.TabIndex = 14;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SgkAssistant.Properties.Resources.splash2;
            this.pictureBox1.Location = new System.Drawing.Point(23, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(533, 304);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // FSplash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(576, 372);
            this.ControlBox = false;
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.radWaitingBar1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(576, 372);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(576, 372);
            this.Name = "FSplash";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(576, 372);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Asistpro";
            this.Load += new System.EventHandler(this.fSplash_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radWaitingBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblMessage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.UI.RadWaitingBar radWaitingBar1;
        private Telerik.WinControls.UI.DotsSpinnerWaitingBarIndicatorElement dotsSpinnerWaitingBarIndicatorElement1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Telerik.WinControls.UI.RadLabel lblMessage;
        private Telerik.WinControls.UI.RadButton btnCancel;
        private System.Windows.Forms.PictureBox pictureBox4;
        private Telerik.WinControls.UI.RadLabel radLabel1;
    }
}
