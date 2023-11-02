using Models.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace SgkAssistant.Forms.Defs
{
    public partial class FLastNamesReport : Telerik.WinControls.UI.RadForm
    {
        List<LastNames> lastNames = new List<LastNames>();
        public FLastNamesReport(List<LastNames> lastNames)
        {
            InitializeComponent();
            if (lastNames != null)
            {
                this.lastNames = lastNames;
            }
        }

        private void fLastNamesReport_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            btnOpenFile.Visible = false;
            List<RadButton> radButtons = new List<RadButton>() { btnOpenFile, btnExport };
            foreach (RadButton btn in radButtons)
            {
                btn.ButtonElement.BorderElement.Visibility = ElementVisibility.Visible;
                btn.ButtonElement.BorderElement.ForeColor = System.Drawing.Color.FromArgb(120, 148, 186);
                btn.ButtonElement.ShowBorder = true;
                btn.ForeColor = System.Drawing.Color.FromArgb(21, 66, 139);
                btn.ElementTree.Control.Cursor = Cursors.Hand;
            }
            if (lastNames != null)
            {
                rgvTcnoName.DataSource = lastNames;
                rgvTcnoName.Columns[0].HeaderText = "TC Kimlik Numarası";
                rgvTcnoName.Columns[1].HeaderText = "Güncelleme Öncesi Adı Soyadı";
                rgvTcnoName.Columns[2].HeaderText = "Güncelleme Sonrası Adı Soyadı";
                rgvTcnoName.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            string msg = "";
            btnOpenFile.Visible = false;
            lblMessage.Visible = false;
            bool result = false;
            string title = rgvTcnoName.TitleText;
            try
            {
                IOC.ExportService.CreateFile(out msg);
                if (lastNames != null && lastNames.Count > 0)
                {
                    result = IOC.ExportService.CreateFileForLastNames(title, lastNames, out msg);
                }
                lblMessage.Visible = true;
                if (result)
                {
                    IOC.ExportService.Save($"Soyad Güncelleme Listesi {DateTime.Today:dd.MM.yyyy}", out msg);
                    btnOpenFile.Visible = true;
                    lblMessage.Visible = true;
                }
                if (msg.Contains("başka bir işlem tarafından"))
                {
                    lblMessage.Text = "Dosya oluşturulamadı! Aynı adlı bir dosya açıksa kapatıp tekrar deneyin.";
                }
                else if (msg.Contains("konumuna kaydedildi."))
                {
                    string path = IOC.ExportService.FileName.Remove(IOC.ExportService.FileName.LastIndexOf("\\") + 1);
                    string name = IOC.ExportService.FileName.Replace(path, "");
                    lblMessage.Text = $"{name} dosyası oluşturuldu";
                }
                else
                {
                    lblMessage.Text = msg;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Dosya oluşturulamadı! Hata: {ex.Message.ToString()}"; btnOpenFile.Visible = false;
            }
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            string location = IOC.ExportService.FileName;
            Process.Start(location);
        }
    }
}
