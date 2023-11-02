using Models.Common;
using SgkAssistant.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Localization;

namespace SgkAssistant.Forms.Defs
{
    public partial class fUnconfirmDetails : Telerik.WinControls.UI.RadForm
    {
        public fUnconfirmDetails()
        {
            RadGridLocalizationProvider.CurrentProvider = new Localization();
            InitializeComponent();
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            string msg = "";
            btnOpenFile.Visible = false;
            lblMessage.Visible = false;
            bool result = false;
            string title = rgvConfirmDetails.TitleText;
            try
            {
                IOC.ExportService.CreateFile(out msg);
                if (IOC.VisitReportService.ProcessReports != null && IOC.VisitReportService.ProcessReports.Count > 0)
                {
                    result = IOC.ExportService.CreateFileForUnConfirm(title, IOC.VisitReportService.ProcessReports, out msg);
                }
                lblMessage.Visible = true;
                if (result)
                {
                    IOC.ExportService.Save($"Rapor Onay İptal Listesi {DateTime.Today:dd.MM.yyyy}", out msg);
                    btnOpenFile.Visible = msg.Contains("iptal") ? false : true;
                }

                if (msg.Contains("başka bir işlem tarafından"))
                {
                    lblMessage.Text = "Dosya oluşturulamadı! Kayıt konumunda aynı isimde bir dosya varsa ve bu dosya açıksa önce onu kapatıp sonra tekrar deneyin.";
                }
                else
                {
                    lblMessage.Text = msg;
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Dosya oluşturulamadı! Hata: {ex.Message}"; btnOpenFile.Visible = false;
            }
        }
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            string location = IOC.ExportService.FileName;
            Process.Start(location);
        }
        private void fUnconfirmDetails_Load(object sender, EventArgs e)
        {
            List<RadButton> radButtons = new List<RadButton>() { btnExportExcel, btnOpenFile };
            foreach (RadButton btn in radButtons)
            {
                btn.ButtonElement.BorderElement.Visibility = ElementVisibility.Visible;
                btn.ButtonElement.BorderElement.ForeColor = Color.FromArgb(120, 148, 186);
                btn.ButtonElement.ShowBorder = true;
                btn.ForeColor = Color.FromArgb(21, 66, 139);
                btn.ElementTree.Control.Cursor = Cursors.Hand;
            }
            lblMessage.Visible = false;
            btnOpenFile.Visible = false;
            rgvConfirmDetails.TitleText = $"{DateTime.Now:dd.MM.yyyy} TARİHLİ RAPOR ONAYI İPTAL İŞLEMLERİ";
            rgvConfirmDetails.DataSource = IOC.VisitReportService.ProcessReports;
            rgvConfirmDetails.Columns[0].HeaderText = "Kimlik No"; rgvConfirmDetails.Columns[0].MinWidth = 80;
            rgvConfirmDetails.Columns[1].HeaderText = "Adı Soyadı"; rgvConfirmDetails.Columns[1].MinWidth = 150;
            rgvConfirmDetails.Columns[2].HeaderText = "Vaka Türü"; rgvConfirmDetails.Columns[2].MinWidth = 80;
            rgvConfirmDetails.Columns[3].HeaderText = "Poliklinik Tarihi"; rgvConfirmDetails.Columns[3].MinWidth = 80;
            rgvConfirmDetails.Columns[3].DataType = typeof(DateTime);
            rgvConfirmDetails.Columns[3].FormatString = "{0: dd.MM.yyyy}";
            rgvConfirmDetails.Columns[3].TextAlignment = ContentAlignment.MiddleCenter;
            rgvConfirmDetails.Columns[4].HeaderText = "İşbaşı Tarihi"; rgvConfirmDetails.Columns[4].MinWidth = 80;
            rgvConfirmDetails.Columns[4].DataType = typeof(DateTime);
            rgvConfirmDetails.Columns[4].FormatString = "{0: dd.MM.yyyy}";
            rgvConfirmDetails.Columns[4].TextAlignment = ContentAlignment.MiddleCenter;
            rgvConfirmDetails.Columns[5].HeaderText = "Takip No"; rgvConfirmDetails.Columns[5].MinWidth = 120;
            rgvConfirmDetails.Columns[6].HeaderText = "Sıra No"; rgvConfirmDetails.Columns[6].MaxWidth = 50; rgvConfirmDetails.Columns[6].MinWidth = 50;
            rgvConfirmDetails.Columns[7].HeaderText = "İşlem Sonucu"; rgvConfirmDetails.Columns[7].MinWidth = 220;
            rgvConfirmDetails.Columns[8].HeaderText = "İşlem Tarihi"; rgvConfirmDetails.Columns[8].MinWidth = 80;
            rgvConfirmDetails.Columns[8].DataType = typeof(DateTime);
            rgvConfirmDetails.Columns[8].FormatString = "{0: dd.MM.yyyy}";
            rgvConfirmDetails.Columns[8].TextAlignment = ContentAlignment.MiddleCenter;
            rgvConfirmDetails.AutoSizeRows = false;
            rgvConfirmDetails.TableElement.RowHeight = 30;
            rgvConfirmDetails.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
        }
    }
}
