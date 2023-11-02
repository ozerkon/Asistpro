using System;
using System.Collections.Generic;
using System.Web;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace SgkAssistant.Forms.Defs
{
    public partial class FHlReport : Telerik.WinControls.UI.RadForm
    {
        private string Rapor { get; set; }
        public FHlReport(string rapor)
        {
            this.Rapor = rapor;
            InitializeComponent();
        }

        private void fHlReport_Load(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            List<RadButton> radButtons = new List<RadButton>() { btnCopyText, btnClose };
            foreach (RadButton btn in radButtons)
            {
                btn.ButtonElement.BorderElement.Visibility = ElementVisibility.Visible;
                btn.ButtonElement.BorderElement.ForeColor = System.Drawing.Color.FromArgb(120, 148, 186);
                btn.ButtonElement.ShowBorder = true;
                btn.ForeColor = System.Drawing.Color.FromArgb(21, 66, 139);
                btn.ElementTree.Control.Cursor = Cursors.Hand;
            }
            this.textReport.Text = Rapor;
            Telerik.WinControls.RichTextEditor.UI.FontFamily fontFamily = new Telerik.WinControls.RichTextEditor.UI.FontFamily("Segoe UI");
            textReport.ChangeFontFamily(fontFamily);
            this.textReport.IsReadOnly = true;
            this.textReport.RichTextBoxElement.BorderThickness = new Padding(0);
            this.textReport.BackColor = this.BackColor;
            this.textReport.TabStop = false;
        }

        private void btnCopyText_Click(object sender, EventArgs e)
        {
            RichTextBox rtbTemp = new RichTextBox();
            WebBrowser wb = new WebBrowser();
            try
            {
                wb.Navigate("about:blank");

                wb.Document.Write(textReport.Text);
                wb.Document.ExecCommand("SelectAll", false, null);
                wb.Document.ExecCommand("Copy", false, null);

                rtbTemp.SelectAll();
                rtbTemp.Paste();
                Clipboard.SetText(rtbTemp.Text);
                lblMessage.Text = "Hizmet listesi analiz sonuçları panoya kopyalandı. ";
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Kopyalama işlemi başarısız oldu. Lütfen tekrar deneyin. Hata: {ex.Message}";
            }
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }

        private void lblMessage_TextChanged(object sender, EventArgs e)
        {
            if (lblMessage.Text == "")
            {
                lblMessage.Visible = false;
            }
            else
            {
                lblMessage.Visible = true;
            }
        }
    }
}
