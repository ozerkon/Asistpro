namespace SgkAssistant.Forms.Defs
{
    partial class FWarningAuthority
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FWarningAuthority));
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.btnOpenFileProperties = new Telerik.WinControls.UI.RadButton();
            this.roundRectShape1 = new Telerik.WinControls.RoundRectShape(this.components);
            this.btnCancel = new Telerik.WinControls.UI.RadButton();
            this.btnReTry = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOpenFileProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnReTry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel2
            // 
            this.radLabel2.ForeColor = System.Drawing.Color.Red;
            this.radLabel2.Location = new System.Drawing.Point(153, 4);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(167, 24);
            this.radLabel2.TabIndex = 5;
            this.radLabel2.Text = "<html><span style=\"font-size: 12pt\"><span style=\"font-size: 12pt\"><strong>Yetkile" +
    "ndirme Hatası!</strong></span></span></html>";
            // 
            // radLabel1
            // 
            this.radLabel1.AutoSize = false;
            this.radLabel1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.radLabel1.Location = new System.Drawing.Point(3, 32);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(467, 211);
            this.radLabel1.TabIndex = 4;
            this.radLabel1.Text = resources.GetString("radLabel1.Text");
            this.radLabel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOpenFileProperties
            // 
            this.btnOpenFileProperties.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenFileProperties.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnOpenFileProperties.ForeColor = System.Drawing.Color.Black;
            this.btnOpenFileProperties.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenFileProperties.Image")));
            this.btnOpenFileProperties.Location = new System.Drawing.Point(12, 254);
            this.btnOpenFileProperties.Name = "btnOpenFileProperties";
            this.btnOpenFileProperties.Padding = new System.Windows.Forms.Padding(3, 0, 5, 0);
            this.btnOpenFileProperties.Size = new System.Drawing.Size(143, 28);
            this.btnOpenFileProperties.TabIndex = 8;
            this.btnOpenFileProperties.Text = "Dosya özelliklerini aç";
            this.btnOpenFileProperties.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOpenFileProperties.Click += new System.EventHandler(this.btnOpenFileProperties_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnOpenFileProperties.GetChildAt(0))).Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnOpenFileProperties.GetChildAt(0))).TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnOpenFileProperties.GetChildAt(0))).Text = "Dosya özelliklerini aç";
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnOpenFileProperties.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(3, 0, 5, 0);
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btnOpenFileProperties.GetChildAt(0).GetChildAt(2))).Shape = this.roundRectShape1;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(343, 254);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(3, 0, 5, 0);
            this.btnCancel.Size = new System.Drawing.Size(116, 28);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Programı Kapat";
            this.btnCancel.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnCancel.GetChildAt(0))).Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnCancel.GetChildAt(0))).TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnCancel.GetChildAt(0))).Text = "Programı Kapat";
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnCancel.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(3, 0, 5, 0);
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btnCancel.GetChildAt(0).GetChildAt(2))).Shape = this.roundRectShape1;
            // 
            // btnReTry
            // 
            this.btnReTry.BackColor = System.Drawing.Color.Transparent;
            this.btnReTry.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnReTry.ForeColor = System.Drawing.Color.Black;
            this.btnReTry.Image = global::SgkAssistant.Properties.Resources.refresh;
            this.btnReTry.Location = new System.Drawing.Point(197, 254);
            this.btnReTry.Name = "btnReTry";
            this.btnReTry.Padding = new System.Windows.Forms.Padding(3, 0, 5, 0);
            this.btnReTry.Size = new System.Drawing.Size(104, 28);
            this.btnReTry.TabIndex = 10;
            this.btnReTry.Text = "Yeniden Dene";
            this.btnReTry.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.btnReTry.ToolTipTextNeeded += new Telerik.WinControls.ToolTipTextNeededEventHandler(this.btnReTry_ToolTipTextNeeded);
            this.btnReTry.Click += new System.EventHandler(this.btnReTry_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnReTry.GetChildAt(0))).Image = global::SgkAssistant.Properties.Resources.refresh;
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnReTry.GetChildAt(0))).TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnReTry.GetChildAt(0))).Text = "Yeniden Dene";
            ((Telerik.WinControls.UI.RadButtonElement)(this.btnReTry.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(3, 0, 5, 0);
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btnReTry.GetChildAt(0).GetChildAt(2))).Shape = this.roundRectShape1;
            // 
            // FWarningAuthority
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 290);
            this.ControlBox = false;
            this.Controls.Add(this.btnReTry);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOpenFileProperties);
            this.Controls.Add(this.radLabel2);
            this.Controls.Add(this.radLabel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FWarningAuthority";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(0, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "YETKİLENDİRME HATASI!";
            this.Load += new System.EventHandler(this.fWarningAuthority_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOpenFileProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnReTry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadButton btnOpenFileProperties;
        private Telerik.WinControls.RoundRectShape roundRectShape1;
        private Telerik.WinControls.UI.RadButton btnCancel;
        private Telerik.WinControls.UI.RadButton btnReTry;
    }
}
