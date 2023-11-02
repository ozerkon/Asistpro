namespace SgkAssistant.Forms.Defs
{
    partial class FTimeOut
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FTimeOut));
            this.btnOK = new Telerik.WinControls.UI.RadButton();
            this.rbUnpaid = new Telerik.WinControls.UI.RadRadioButton();
            this.rbAdvance = new Telerik.WinControls.UI.RadRadioButton();
            this.btnCancel = new Telerik.WinControls.UI.RadButton();
            this.radPictureBox1 = new Telerik.WinControls.UI.RadPictureBox();
            this.lblQuestion = new Telerik.WinControls.UI.RadLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblDesc = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.btnOK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbUnpaid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbAdvance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblQuestion)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lblDesc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnOK.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnOK.Image = global::SgkAssistant.Properties.Resources.approval;
            this.btnOK.Location = new System.Drawing.Point(155, 332);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(110, 36);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "Tamam";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // rbUnpaid
            // 
            this.rbUnpaid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rbUnpaid.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rbUnpaid.Location = new System.Drawing.Point(12, 278);
            this.rbUnpaid.Name = "rbUnpaid";
            this.rbUnpaid.Size = new System.Drawing.Size(149, 19);
            this.rbUnpaid.TabIndex = 1;
            this.rbUnpaid.Text = "Ücretsiz izin olarak ekle";
            this.rbUnpaid.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            this.rbUnpaid.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.Radio_ToggleStateChanged);
            // 
            // rbAdvance
            // 
            this.rbAdvance.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rbAdvance.Location = new System.Drawing.Point(12, 307);
            this.rbAdvance.Name = "rbAdvance";
            this.rbAdvance.Size = new System.Drawing.Size(138, 19);
            this.rbAdvance.TabIndex = 1;
            this.rbAdvance.TabStop = false;
            this.rbAdvance.Text = "Avans izin olarak ekle";
            this.rbAdvance.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.Radio_ToggleStateChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Image = global::SgkAssistant.Properties.Resources.cancel;
            this.btnCancel.Location = new System.Drawing.Point(297, 332);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 36);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "İptal";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // radPictureBox1
            // 
            this.radPictureBox1.DefaultSvgImageXml = resources.GetString("radPictureBox1.DefaultSvgImageXml");
            this.radPictureBox1.Image = global::SgkAssistant.Properties.Resources.question_mark_96;
            this.radPictureBox1.Location = new System.Drawing.Point(12, 12);
            this.radPictureBox1.Name = "radPictureBox1";
            this.radPictureBox1.Size = new System.Drawing.Size(96, 96);
            this.radPictureBox1.TabIndex = 2;
            // 
            // lblQuestion
            // 
            this.lblQuestion.AutoSize = false;
            this.lblQuestion.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblQuestion.Location = new System.Drawing.Point(114, 12);
            this.lblQuestion.Name = "lblQuestion";
            this.lblQuestion.Size = new System.Drawing.Size(439, 96);
            this.lblQuestion.TabIndex = 3;
            this.lblQuestion.Text = "Asistpro\'nun ne yapmasını istersiniz?";
            this.lblQuestion.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblDesc);
            this.groupBox1.Location = new System.Drawing.Point(13, 114);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(540, 144);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Açıklamalar";
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = false;
            this.lblDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDesc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDesc.Location = new System.Drawing.Point(3, 20);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.lblDesc.Size = new System.Drawing.Size(534, 121);
            this.lblDesc.TabIndex = 0;
            this.lblDesc.Text = "radLabel1";
            this.lblDesc.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            // 
            // FTimeOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(560, 375);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblQuestion);
            this.Controls.Add(this.radPictureBox1);
            this.Controls.Add(this.rbAdvance);
            this.Controls.Add(this.rbUnpaid);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(568, 408);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(568, 408);
            this.Name = "FTimeOut";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(568, 408);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "İzin günü aşımı!";
            ((System.ComponentModel.ISupportInitialize)(this.btnOK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbUnpaid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbAdvance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblQuestion)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lblDesc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadButton btnOK;
        private Telerik.WinControls.UI.RadRadioButton rbUnpaid;
        private Telerik.WinControls.UI.RadRadioButton rbAdvance;
        private Telerik.WinControls.UI.RadButton btnCancel;
        private Telerik.WinControls.UI.RadPictureBox radPictureBox1;
        private Telerik.WinControls.UI.RadLabel lblQuestion;
        private System.Windows.Forms.GroupBox groupBox1;
        private Telerik.WinControls.UI.RadLabel lblDesc;
    }
}
