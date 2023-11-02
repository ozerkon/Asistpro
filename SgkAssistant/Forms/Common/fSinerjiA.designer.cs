namespace SgkAssistant.Forms.Defs
{
    partial class FSinerjiA
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FSinerjiA));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClose = new Telerik.WinControls.UI.RadButton();
            this.roundRectShape1 = new Telerik.WinControls.RoundRectShape(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(8, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(460, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Personel teşviklerinizi en verimli şekilde takip etmek ister misiniz?";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(8, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(479, 59);
            this.label2.TabIndex = 4;
            this.label2.Text = "Sinerjia Yazılım tarafından geliştirilen Personel Teşvik yazılımı ile kendi bünye" +
    "nizde personel teşviklerini kolayca takip edip hesaplatabilirsiniz.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label3.Location = new System.Drawing.Point(8, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(148, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Daha fazla bilgi için ";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.linkLabel1.Location = new System.Drawing.Point(149, 114);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(170, 20);
            this.linkLabel1.TabIndex = 6;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "https://www.sinerjia.net";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label4.Location = new System.Drawing.Point(318, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "adresini ziyaret edin";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnClose.Image = global::SgkAssistant.Properties.Resources.cancel_24;
            this.btnClose.Location = new System.Drawing.Point(194, 177);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(110, 35);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "KAPAT";
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnClose.GetChildAt(0))).Image = global::SgkAssistant.Properties.Resources.cancel_24;
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnClose.GetChildAt(0))).Text = "KAPAT";
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btnClose.GetChildAt(0).GetChildAt(2))).Shape = this.roundRectShape1;
            // 
            // FSinerjiA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(499, 243);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FSinerjiA";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TEŞVİKLERDEN EN VERİMLİ ŞEKİLDE YARARLANIN";
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label4;
        private Telerik.WinControls.UI.RadButton btnClose;
        private Telerik.WinControls.RoundRectShape roundRectShape1;
    }
}
