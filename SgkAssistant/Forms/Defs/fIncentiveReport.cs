using Models.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace SgkAssistant.Forms.Defs
{
    public partial class FIncentiveReport : Telerik.WinControls.UI.RadForm
    {
        List<Incentive> lstInc = new List<Incentive>();
        public FIncentiveReport(List<Incentive> lstInc)
        {
            InitializeComponent();
            if (lstInc != null)
            {
                this.lstInc = lstInc;
            }
        }

        private void fIncentiveReport_Load(object sender, EventArgs e)
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
            if (lstInc != null)
            {
                rgvTesvikSonucu.DataSource = lstInc;
                if (lstInc.Count == 0) rgvTesvikSonucu.TitleText = "GİRİLEN KİMLİK NUMARALARINA AİT 5510 HARİCİ TEŞVİK BULUNAMAMIŞTIR";
                rgvTesvikSonucu.Columns[0].HeaderText = "Firma Adı";                rgvTesvikSonucu.Columns[0].TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
                rgvTesvikSonucu.Columns[1].HeaderText = "Kimlik Numarası";          rgvTesvikSonucu.Columns[1].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                rgvTesvikSonucu.Columns[2].HeaderText = "Yararlanılabilcek Teşvik"; rgvTesvikSonucu.Columns[2].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                rgvTesvikSonucu.Columns[3].HeaderText = "Başlangıç - Bitiş Dönemi"; rgvTesvikSonucu.Columns[3].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                rgvTesvikSonucu.Columns[4].HeaderText = "Teşvik Süresi";            rgvTesvikSonucu.Columns[4].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                rgvTesvikSonucu.Columns[5].HeaderText = "Teşvik Kazancınız";        rgvTesvikSonucu.Columns[5].TextAlignment = System.Drawing.ContentAlignment.MiddleRight; 
                rgvTesvikSonucu.Columns[6].HeaderText = "İlave Olunacak Sayı";      rgvTesvikSonucu.Columns[6].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                rgvTesvikSonucu.Columns[7].IsVisible = false;
                rgvTesvikSonucu.Columns[7].VisibleInColumnChooser = false;
                rgvTesvikSonucu.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
                rgvTesvikSonucu.MasterTemplate.AutoExpandGroups = true;
                rgvTesvikSonucu.GroupDescriptors.Clear();
                rgvTesvikSonucu.GroupDescriptors.Add(new GridGroupByExpression("[Cn] Group By [Cn]")); 
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
            string title = rgvTesvikSonucu.TitleText;
            try
            {
                IOC.ExportService.CreateFile(out msg);
                if (lstInc != null && lstInc.Count > 0)
                {
                    result = IOC.ExportService.CreateFileForIncentives(title, lstInc, out msg);
                }
                lblMessage.Visible = true;
                if (result)
                {
                    IOC.ExportService.Save($"Potansiyel Teşvik Sorgulama {DateTime.Today:dd.MM.yyyy}", out msg);
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

        private void rgvTesvikSonucu_CellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            if (e.CellElement.ColumnIndex == 4)
            {
                e.CellElement.Padding = new Padding(0, 0, 5, 0);
            }
            
        }
    }
}
