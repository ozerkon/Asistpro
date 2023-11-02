using Models.Common;
using Models.Domain;
using SgkAssistant.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Localization;

namespace SgkAssistant.Forms.Defs
{
    public partial class FLeavesAdd : RadForm
    {
        private string msg = "";
        private Personal personal;
        private List<Leaves> lstLeaves;
        public FLeavesAdd(Personal personal = null)
        {
            this.personal = personal;
            RadGridLocalizationProvider.CurrentProvider = new LocalizationForImport();
            InitializeComponent();
        }
        private void fLeavesAdd_Load(object sender, EventArgs e)
        {
            List<RadButton> radButtons = new List<RadButton>() { btnCreateTemplate, btnPasteFromClipboard, btnClearRgv, btnOpenFile, btnSave };
            foreach (RadButton btn in radButtons)
            {
                btn.ButtonElement.BorderElement.Visibility = ElementVisibility.Visible;
                btn.ButtonElement.BorderElement.ForeColor = Color.FromArgb(120, 148, 186);
                btn.ButtonElement.ShowBorder = true;
                btn.ForeColor = Color.FromArgb(21, 66, 139);
                btn.ElementTree.Control.Cursor = Cursors.Hand;
            }
            lblAlert.Visible = false;
            rgvLeaves.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            if (personal != null) Text = $"{personal.Tcno} KİMLİK NUMARALI {personal.Ads} İÇİN TOPLU İZİN EKLEME";
            else Text = $"{GlobalVars.IzinComp.CompanyName} İÇİN TOPLU İZİN EKLEME";
        }
        private void SetRgvLeaves()
        {
            int i = 0;

            rgvLeaves.Columns[i].Name = "Tcno";
            rgvLeaves.Columns[i++].HeaderText = "TC KİMLİK NO";
            rgvLeaves.Columns[i].Name = "Ads";
            rgvLeaves.Columns[i++].HeaderText = "ADI SOYADI";
            rgvLeaves.Columns[i].Name = "Startdate"; rgvLeaves.Columns[i].FormatInfo = new System.Globalization.CultureInfo("tr-TR"); rgvLeaves.Columns[i].FormatString = "{0:dd.MM.yyyy}";
            rgvLeaves.Columns[i++].HeaderText = "BAŞLANGIÇ TARİHİ";
            rgvLeaves.Columns[i].Name = "Enddate"; rgvLeaves.Columns[i].FormatInfo = new System.Globalization.CultureInfo("tr-TR"); rgvLeaves.Columns[i].FormatString = "{0:dd.MM.yyyy}";
            rgvLeaves.Columns[i++].HeaderText = "BİTİŞ TARİHİ";
            rgvLeaves.Columns[i].Name = "Paid";
            rgvLeaves.Columns[i++].HeaderText = "ÜCRETLİ İZİN";
            rgvLeaves.Columns[i].Name = "Notes";
            rgvLeaves.Columns[i++].HeaderText = "AÇIKLAMALAR";
            rgvLeaves.Columns[i].Name = "Ph"; rgvLeaves.Columns[i].DataType = typeof(bool);
            rgvLeaves.Columns[i++].HeaderText = "RESMİ TATİLLER\r\nİŞ GÜNÜ SAYILSIN";
        }
        private void btnCreateTemplate_Click(object sender, EventArgs e)
        {
            msg = "";
            try
            {
                bool result = false, saveResult = false;
                IOC.ExportService.CreateFile(out msg);
                result = IOC.ExportService.CreateTemplateForExcelLeaves( out msg);

                if (result)
                {
                    saveResult = IOC.ExportService.Save("İzin Listesi", out msg);
                    btnOpenFile.Visible = true;
                }
                lblAlert.Text = msg;
                btnOpenFile.Visible = saveResult;
            }
            catch (Exception ex)
            {
                lblAlert.Text = $"Dosya oluşturulamadı! Hata: {ex.Message}"; btnOpenFile.Visible = false;
            }
        }

        private void btnPasteFromClipboard_Click(object sender, EventArgs e)
        {
            List<ExcelLeaves> excelLeaves = PasteIntoGrid(out msg);
            if (excelLeaves != null && excelLeaves.Count > 0)
            {
                rgvLeaves.Columns.Clear();
                rgvLeaves.DataSource = excelLeaves;
                SetRgvLeaves();
                lstLeaves = new List<Leaves>();
                foreach (ExcelLeaves elv in excelLeaves)
                {
                    Leaves alv = new Leaves()
                    {
                        Trxtype = "1",
                        Pid = (from x in GlobalVars.Personals where x.Tcno == elv.Tcno select x.Id).FirstOrDefault(),
                        Cid = (from x in GlobalVars.Personals where x.Tcno == elv.Tcno select x.Cid).FirstOrDefault(),
                        Trxdate = DateTime.Now,
                        Startdate = elv.Startdate.Date,
                        Enddate = elv.Enddate.Date,
                        Paid = elv.Paid,
                        Timeval = IOC.PersonalService.SetTimeVal(elv.Ph, new DateRange() { Sd = elv.Startdate, Ed = elv.Enddate }),
                        Timeunit = 1,
                        Cd = DateTime.Now,
                        Docref = "",
                        Notes = elv.Notes,
                        Ph = elv.Ph,
                        Cu = GlobalVars.ActiveUser
                    };
                    lstLeaves.Add(alv);
                }
                List<string> tcnos = (from x in excelLeaves select x.Tcno).Distinct().ToList();
                if (tcnos.Count > 1) Text = $"TOPLU İZİN EKLEME (Birden fazla firma için)";
                else if (tcnos.Count == 1) {   Text =$"{tcnos[0]} KİMLİK NUMARALI {(from x in GlobalVars.Personals where x.Tcno == tcnos[0] select x.Ads).FirstOrDefault()} İÇİN TOPLU İZİN EKLEME"; }
                else if (personal != null) Text = $"{personal.Tcno} KİMLİK NUMARALI {personal.Ads} İÇİN TOPLU İZİN EKLEME";
                else Text = $"{GlobalVars.IzinComp.CompanyName} İÇİN TOPLU İZİN EKLEME";
                lblAlert.Text = "";
            }
            else
            {
                lblAlert.Text = "Kopyalama işlemi başarısız! Lütfen Excel'deki verilerinizi kontrol edip tekrar deneyiniz.";
            }
        }

        private void btnClearRgv_Click(object sender, EventArgs e)
        {
            if (personal != null) Text = $"{personal.Tcno} KİMLİK NUMARALI {personal.Ads} İÇİN TOPLU İZİN EKLEME";
            else Text = $"{GlobalVars.IzinComp.CompanyName} İÇİN TOPLU İZİN EKLEME";
            lblAlert.Text = string.Empty;
            rgvLeaves.Columns.Clear();
            rgvLeaves.DataSource = new List<ExcelLeaves>();
            SetRgvLeaves();
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            //bool showed = false;
            if (rgvLeaves.RowCount < 1) { lblAlert.Text = "Kaydedecek veri yok!"; return; }
            msg = ""; string mesaj = ""; int addRes = 0, updRes = 0;
            try
            {
                foreach (Leaves leave in lstLeaves)
                {
                    personal = (from q in GlobalVars.Personals where q.Id == leave.Pid select q).FirstOrDefault();
                    IOC.PersonalService.AddOrUpdateLeaves(leave, out msg);
                    if (msg.Contains("Hata") || msg.Contains("hata")) { mesaj += $"{msg} \r\n"; continue; }
                    else if (msg.Contains("iptal")) { mesaj += $"{personal.Tcno} kimlik numaralı, {personal.Ads} adlı personele {leave.Startdate:dd.MM.yyyy} - {leave.Startdate:dd.MM.yyyy} tarihleri arasında izin ekleme işlemi iptal edildi\r\n"; continue; }
                    else if (msg.Contains("eklendi")) { mesaj += $"{personal.Tcno} kimlik numaralı, {personal.Ads} adlı personele {leave.Startdate:dd.MM.yyyy} - {leave.Startdate:dd.MM.yyyy} tarihleri arasında izin eklendi\r\n"; addRes++; }
                    else if (msg.Contains("güncellendi")) { mesaj += $"{personal.Tcno} kimlik numaralı, {personal.Ads} adlı personele {leave.Startdate:dd.MM.yyyy} - {leave.Startdate:dd.MM.yyyy} tarihleri arasında izin eklendi (bu tarih aralığına denk gelen başka bir izin iptal edildi)\r\n"; updRes++; }
                    
                }
                if(addRes == 0 && updRes == 0) { LeavesChanged.HasChanged = false; lblAlert.Text = msg; return; }
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"İZİN EKLEME SONUCU");
                sb.AppendLine();
                sb.AppendLine(mesaj);
                sb.AppendLine();
                sb.AppendLine($"{addRes} adet izin eklendi, {updRes} adet izin güncellendi");
                IOC.ExportService.FileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $@"/izin.txt";
                File.WriteAllText(IOC.ExportService.FileName, sb.ToString());
                btnOpenFile.Visible = true;
                lblAlert.Text = $"{addRes} adet izin eklendi, {updRes} adet izin güncellendi, ayrıntılar için 'Dosyayı Aç' butonun basın"; LeavesChanged.HasChanged = true;
            }
            catch (Exception ex)
            {
                lblAlert.Text = $"Hata: {ex.Message}";
            }
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            Process.Start(IOC.ExportService.FileName);
        }

        private List<ExcelLeaves> PasteIntoGrid(out string msg)
        {
            msg = ""; string errors = "";
            DataObject o = (DataObject)Clipboard.GetDataObject();
            List<ExcelLeaves> liste = new List<ExcelLeaves>();
            try
            {
                if (o.GetDataPresent(DataFormats.Text))
                {
                    string[] pastedRows = Regex.Split(o.GetData(DataFormats.Text).ToString(), "\r\n");
                    List<string> list = new List<string>(pastedRows);
                    list.RemoveAt(pastedRows.Length - 1);
                    pastedRows = list.ToArray();
                    foreach (string row in pastedRows)
                    {
                        ExcelLeaves alv = new ExcelLeaves();
                        List<string> datas = Regex.Split(row, "\t").ToList();
                        Personal prs = new Personal();
                        if (datas.Count < 5)
                        {
                            return null;
                        }
                        else if (datas.Count >= 5)
                        {
                            if (!IOC.WinHelpers.IsTcnoValid(datas[0].Trim(), out msg)) { errors += $"{datas[1]} adlı personelin kimlik numarası ({datas[0]}) geçersiz\r\n"; continue; }
                            else alv.Tcno = datas[0].Trim();
                            prs = (from x in GlobalVars.Personals where x.Tcno == alv.Tcno select x).FirstOrDefault();
                            if(prs == null || prs.Id == 0) { errors += $"{alv.Tcno} kimlik numarası personel veri tabanında yok!\r\n"; continue; }
                            if (!IOC.WinHelpers.IsNameValid(datas[1].Trim(), out msg)) { errors += $"{alv.Tcno} kimlik numaralı personelin adı ({datas[1]}) çok kısa ya da geçersiz karakterler var\r\n"; continue; }
                            else alv.Ads = datas[1].Trim().ToUpper();
                            try { Convert.ToDateTime(datas[2]); }
                            catch { errors += $"{alv.Tcno} kimlik numaralı personelin izin başlangıç tarihi ({datas[2]:dd.MM.yyyy}) hatalı\r\n"; continue; }
                            alv.Startdate = Convert.ToDateTime(datas[2]);
                            try { Convert.ToDateTime(datas[3]); }
                            catch { errors += $"{alv.Tcno} kimlik numaralı personelin izin bitiş tarihi ({datas[3]:dd.MM.yyyy}) hatalı\r\n"; continue; }
                            alv.Enddate = Convert.ToDateTime(datas[3]);
                            if (datas[4].Trim() == "Evet" || datas[4].Trim() == "Hayır") { alv.Paid = datas[4].ToString().Trim() == "Evet" ? true : false; }
                            else { errors += $"{alv.Tcno} kimlik numaralı personelin 'ücretli izin' bilgisi ({datas[4]}) hatalı\r\n"; continue; }

                            alv.Notes = datas[5].ToString().Trim();
                            if(datas[6].Trim() == "Evet" || datas[6].Trim() == "Hayır") { alv.Ph = datas[6].ToString().Trim() == "Evet" ? true : false; alv.Ph = alv.Paid? alv.Ph : true; }
                            else { errors += $"{alv.Tcno} kimlik numaralı personelin 'resmi tatiller iş günü sayılsın mı' bilgisi ({datas[6]}) hatalı\r\n"; continue; }
                            
                        }
                        ExcelLeaves isExist = (from x in liste where alv.Tcno == x.Tcno && alv.Startdate.Date == x.Startdate.Date && alv.Enddate.Date == x.Enddate.Date && alv.Ph == x.Ph select x).FirstOrDefault();
                        if (isExist != null) { errors += $"{alv.Tcno} kimlik numaralı personel için {alv.Startdate.Date:dd.MM.yyyy} - {alv.Enddate.Date:dd.MM.yyyy} tarihleri arasında izin zaten eklenmiş\r\n"; continue; }
                        if (alv.Startdate.Date > alv.Enddate.Date) { errors += $"İzin başlangıç tarihi ({alv.Startdate.Date:dd.MM.yyyy}) bitiş tarihinden ({alv.Enddate.Date:dd.MM.yyyy}) daha ileride\r\n"; continue; }
                        if (prs.Igt > alv.Startdate.Date ) { errors += $"{alv.Tcno} kimlik numaralı personelin izin başlangıç tarihi ({alv.Startdate.Date:dd.MM.yyyy}) işe giriş tarihinden ({prs.Igt:dd.MM.yyyy}) önce !\r\n"; continue; }
                        
                        liste.Add(alv);
                    }
                }
                if (errors != "") { RadMessageBox.Show(errors + "\r\n Not: Hata mesajları panoya kopyalandı, herhangi bir metin editöründe yapıştırabilirsiniz", "Hata!"); Clipboard.Clear(); Clipboard.SetText(errors); }
                return liste;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }

        private void lblAlert_TextChanged(object sender, EventArgs e)
        {
            if (lblAlert.Text == string.Empty)
            {
                lblAlert.Visible = false;
            }
            else
            {
                lblAlert.Visible = true;
            }
        }
    }
}
