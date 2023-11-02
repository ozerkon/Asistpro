using Models.Common;
using SgkAssistant.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Localization;

namespace SgkAssistant.Forms.Defs
{
    public partial class FConfirmDetails : Telerik.WinControls.UI.RadForm
    {
        public FConfirmDetails()
        {
            RadGridLocalizationProvider.CurrentProvider = new Localization();
            InitializeComponent();
        }

        private void fConfirmDetails_Load(object sender, EventArgs e)
        {
            List<RadButton> radButtons = new List<RadButton>() { btnExportExcel, btnOpenFile};
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
            rgvConfirmDetails.TitleText = $"{DateTime.Now:dd.MM.yyyy} TARİHLİ RAPOR ONAYLAMA İŞLEMLERİ";
            rgvConfirmDetails.DataSource = IOC.VisitReportService.ConfirmReports;
            rgvConfirmDetails.Columns[0].HeaderText = "Kimlik No"; rgvConfirmDetails.Columns[0].MinWidth = 80;
            rgvConfirmDetails.Columns[1].HeaderText = "Adı Soyadı"; rgvConfirmDetails.Columns[1].MinWidth = 150;
            rgvConfirmDetails.Columns[2].HeaderText = "Vaka Türü"; rgvConfirmDetails.Columns[2].MinWidth = 80;
            rgvConfirmDetails.Columns[3].HeaderText = "Başlama Tarihi"; rgvConfirmDetails.Columns[3].MinWidth = 80;
            rgvConfirmDetails.Columns[3].DataType = typeof(DateTime);
            rgvConfirmDetails.Columns[3].FormatString = "{0: dd.MM.yyyy}";
            rgvConfirmDetails.Columns[4].HeaderText = "Bitiş Tarihi"; rgvConfirmDetails.Columns[4].MinWidth = 80;
            rgvConfirmDetails.Columns[4].DataType = typeof(DateTime);
            rgvConfirmDetails.Columns[4].FormatString = "{0: dd.MM.yyyy}";
            rgvConfirmDetails.Columns[5].HeaderText = "Takip No"; rgvConfirmDetails.Columns[5].MinWidth = 120;
            rgvConfirmDetails.Columns[6].HeaderText = "Sıra No"; rgvConfirmDetails.Columns[6].MaxWidth = 50; rgvConfirmDetails.Columns[6].MinWidth = 50;
            rgvConfirmDetails.Columns[7].HeaderText = "İşlem Sonucu"; rgvConfirmDetails.Columns[7].MinWidth = 120;
            rgvConfirmDetails.Columns[8].HeaderText = "İşlem Tarihi"; rgvConfirmDetails.Columns[8].MinWidth = 80;
            rgvConfirmDetails.Columns[8].DataType = typeof(DateTime);
            rgvConfirmDetails.Columns[8].FormatString = "{0: dd.MM.yyyy}";
            rgvConfirmDetails.Columns[9].HeaderText = "Pdf Dosyası";
            rgvConfirmDetails.AutoSizeRows = false;
            rgvConfirmDetails.TableElement.RowHeight = 30;

            if (SearchReport.GetConfirmPdf == true)
            {
                rgvConfirmDetails.Columns.Add(Helpers.WinHelpers.AddCommandColumnToRgv("Onay Dökümü"));
            }
            rgvConfirmDetails.Columns[9].IsVisible = false;
            rgvConfirmDetails.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
        }
        private void rgvConfirmDetails_CommandCellClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            string location = rgvConfirmDetails.Rows[i].Cells[9].Value.ToString();
            Process.Start(location);
        }
        private void rgvConfirmDetails_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement.ColumnInfo is GridViewCommandColumn)
            {
                RadButtonElement button = (RadButtonElement)e.CellElement.Children[0];
                button.BorderElement.Shape = new RoundRectShape();
                button.ButtonFillElement.BackColor = Color.Transparent;
                button.ButtonFillElement.NumberOfColors = 1;
                if (e.CellElement.RowInfo.Cells[8].Value != null)
                {
                    string title = e.CellElement.RowInfo.Cells[9].Value.ToString();
                    if (title == "Dosya indirilemedi")
                    {
                        button.Enabled = false;
                    }
                    else
                    {
                        button.Enabled = true;
                    }
                }
            }
        }
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            btnOpenFile.Visible = false;
            lblMessage.Visible = false;
            bool result = false;
            string msg;
            List<ConfirmReport> lst = IOC.WinHelpers.GetConfirmReportsFromRgv(rgvConfirmDetails, out msg);
            string title = rgvConfirmDetails.TitleText;
            try
            {
                IOC.ExportService.CreateFile(out msg);
                if (lst != null && lst.Count > 0)
                {
                    result = IOC.ExportService.CreateFileForConfirm(title, lst, out msg);
                }
                lblMessage.Visible = true;
                if (result)
                {
                    IOC.ExportService.Save($"Rapor Onay Listesi {DateTime.Today:dd.MM.yyyy}", out msg);
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
                lblMessage.Text = $"Dosya oluşturulamadı! Hata: {ex.Message}"; btnOpenFile.Visible =false;
            }
        }
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            string location = IOC.ExportService.FileName;
            Process.Start(location);
        }
    }
}
