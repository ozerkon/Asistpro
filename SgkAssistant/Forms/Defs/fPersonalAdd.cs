using Models.Common;
using SgkAssistant.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Localization;


namespace SgkAssistant.Forms.Defs
{
    public partial class FPersonalAdd : RadForm
    {
        Dictionary<string, int> dc = new Dictionary<string, int>();
        string msg = "";
        private int companyId = 0;
        public FPersonalAdd(int companyId)
        {
            this.companyId = companyId;
            RadGridLocalizationProvider.CurrentProvider = new LocalizationForImport();
            InitializeComponent();
        }

        private void fPersonalAdd_Load(object sender, EventArgs e)
        {
            List<RadButton> radButtons = new List<RadButton>() { btnCreateTemplate, btnPasteFromClipboard, btnClearRgv, btnOpenFile,  btnSave };
            foreach (RadButton btn in radButtons)
            {
                btn.ButtonElement.BorderElement.Visibility = ElementVisibility.Visible;
                btn.ButtonElement.BorderElement.ForeColor = Color.FromArgb(120, 148, 186);
                btn.ButtonElement.ShowBorder = true;
                btn.ForeColor = Color.FromArgb(21, 66, 139);
                btn.ElementTree.Control.Cursor = Cursors.Hand;
            }
            lblAlert.Visible = false;
            rgvPersonal.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            foreach (Company company in GlobalVars.Companies)
            {
                dc.Add(company.CompanyName, company.Id);
            }
        }
        private void btnPasteFromClipboard_Click(object sender, EventArgs e)
        {
            msg = "";
            lblAlert.Text = string.Empty;
            List<ExcelPersonal> personals = PasteIntoGrid(out msg);
            //List<ExcelPersonal> exclude = new List<ExcelPersonal>();
            //foreach (ExcelPersonal ep in personals)
            //{
            //    DateTime zeroTime = new DateTime(1, 1, 1);
            //    TimeSpan span = ep.igt.Subtract(ep.dtr.AddDays(1));
            //    int years = (zeroTime + span).Year - 1;

            //    if (ep.dtr > DateTime.Now.AddYears(-14).AddDays(-1))
            //    {
            //        DialogResult confirmResult = RadMessageBox.Show($"Eklemeye çalıştığınız ({ep.ads}) adlı personel 15 yaşından küçük, devam edilsin mi?", "15 yaşından küçük personel", MessageBoxButtons.YesNo, RadMessageIcon.Question);
            //        if (confirmResult == DialogResult.No)
            //        {
            //            exclude.Add(ep); continue;
            //        }
            //    }
            //    else if (years < 15)
            //    {
            //        DialogResult confirmResult = RadMessageBox.Show($"Eklemeye çalıştığınız ({ep.ads}) adlı personel 15 yaşından küçükken işe başlamış görünüyor, devam edilsin mi?", "15 yaşından küçük personel", MessageBoxButtons.YesNo, RadMessageIcon.Question);
            //        if (confirmResult == DialogResult.No)
            //        {
            //            exclude.Add(ep);  continue;
            //        }
            //    }
            //}
            //if (exclude.Count > 0) personals = personals.Except(exclude).ToList();
            if (personals != null && personals.Count > 0)
            {
                List<ExcelPersonal> temp = IOC.WinHelpers.GetAllPersonalsFromRgv(rgvPersonal, out msg);
                rgvPersonal.Columns.Clear();

                List<ExcelPersonal> unionList = temp; bool exists = false;

                if (temp != null && temp.Count > 0)
                {
                    foreach (ExcelPersonal ep in personals)
                    {
                        foreach (var t in temp)
                        {
                            if (t.Tcno == ep.Tcno)
                            {
                                exists = true; break;
                            }
                            exists = false;
                        }
                        if (exists == false)
                        {
                            unionList.Add(ep);
                        }
                    }
                }
                else
                {
                    unionList = personals;
                }

                rgvPersonal.DataSource = unionList;
                SetRgvPersonal();
                lblAlert.Text = "";
            }
            else
            {
                lblAlert.Text = "Kopyalama işlemi başarısız! Lütfen Excel'deki verilerinizi kontrol edip tekrar deneyiniz.";
            }
        }
        private void SetRgvPersonal()
        {
            int i = 0;
            
            rgvPersonal.Columns[i].Name = "Tcno";
            rgvPersonal.Columns[i++].HeaderText = "TC KİMLİK NO";
            rgvPersonal.Columns[i].Name = "Ads";
            rgvPersonal.Columns[i++].HeaderText = "ADI SOYADI";
            rgvPersonal.Columns[i].Name = "Dtr"; rgvPersonal.Columns[i].FormatInfo = new System.Globalization.CultureInfo("tr-TR"); rgvPersonal.Columns[i].FormatString = "{0:dd.MM.yyyy}";
            rgvPersonal.Columns[i++].HeaderText = "DOĞUM TARİHİ";
            rgvPersonal.Columns[i].Name = "Igt"; rgvPersonal.Columns[i].FormatInfo = new System.Globalization.CultureInfo("tr-TR"); rgvPersonal.Columns[i].FormatString = "{0:dd.MM.yyyy}";
            rgvPersonal.Columns[i++].HeaderText = "İŞE GİRİŞ TARİHİ";
            rgvPersonal.Columns[i].Name = "Tih"; rgvPersonal.Columns[i].FormatString = "{0:0.0}"; 
            rgvPersonal.Columns[i++].HeaderText = "BİRİKMİŞ İZİN GÜNÜ";
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if(rgvPersonal.RowCount < 1) { lblAlert.Text = "Kaydedecek veri yok!"; return; }
            msg = ""; int sonuc = 0;
            List<Personal> lst = new List<Personal>();
            List<ExcelPersonal> excelPersonals = IOC.WinHelpers.GetAllPersonalsFromRgv(rgvPersonal, out msg);
            foreach (ExcelPersonal ep in excelPersonals)
            {
                Personal personal = new Personal() { Tcno = ep.Tcno, Ads = ep.Ads, Dtr = ep.Dtr, Igt = ep.Igt, Cid = companyId, Active = true, Tih = ep.Tih };
                
                lst.Add(personal);
            }
            try
            {
                foreach (Personal prs in lst)
                {
                    IOC.PersonalService.AddPersonal(prs, out msg);
                    if (msg == "") sonuc++;
                }
                //sonuc = IOC.personalDataService.AddPersonals(lst, out msg);
                lblAlert.Text = $"{sonuc} adet personel eklendi";
            }
            catch (Exception ex)
            {
                lblAlert.Text = $"Hata: {ex.Message}";
            }
            
        }
        private void btnCreateTemplate_Click(object sender, EventArgs e)
        {
            msg = "";
            try
            {   
                bool result = false;
                IOC.ExportService.CreateFile(out msg);
                result = IOC.ExportService.CreateTemplateForPersonals(out msg);

                if (result)
                {
                    IOC.ExportService.Save("Personel Listesi", out msg);
                    btnOpenFile.Visible = true;
                }
                lblAlert.Text = msg;
            }
            catch (Exception ex)
            {
                lblAlert.Text = $"Dosya oluşturulamadı! Hata: {ex.Message}"; btnOpenFile.Visible = false;
            }
        }
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            string location = IOC.ExportService.FileName;
            Process.Start(location);
        }
        private void btnClearRgv_Click(object sender, EventArgs e)
        {
            lblAlert.Text = string.Empty;
            rgvPersonal.Columns.Clear();
            rgvPersonal.DataSource = new List<ExcelPersonal>();
            SetRgvPersonal();
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
        private List<ExcelPersonal> PasteIntoGrid(out string msg)
        {
            msg = ""; string errors = "";
            DataObject o = (DataObject)Clipboard.GetDataObject();
            List<ExcelPersonal> liste = new List<ExcelPersonal>();
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
                        ExcelPersonal p = new ExcelPersonal();
                        List<string> datas = Regex.Split(row, "\t").ToList();
                        if (datas.Count < 5)
                        {
                            return null;
                        }
                        else if (datas.Count >= 5)
                        {
                            if (!IOC.WinHelpers.IsTcnoValid(datas[0].Trim(), out msg)) { errors += $"{datas[1]} adlı personelin kimlik numarası geçersiz\r\n"; continue; }
                            else p.Tcno = datas[0].Trim();
                            if (!IOC.WinHelpers.IsNameValid(datas[1].Trim(), out msg)) { errors += $"{p.Tcno} kimlik numaralı adında geçersiz karakterler var\r\n"; continue; }
                            else p.Ads = datas[1].Trim().ToUpper();
                            try {Convert.ToDateTime(datas[2]);  }
                            catch {errors += $"{p.Tcno} kimlik numaralı personelin doğum tarihi hatalı\r\n"; continue; }
                            p.Dtr = Convert.ToDateTime(datas[2]);
                            try { Convert.ToDateTime(datas[3]); }
                            catch { errors += $"{p.Tcno} kimlik numaralı personelin işe giriş tarihi hatalı\r\n"; continue; }
                            p.Igt = Convert.ToDateTime(datas[3]);
                            try { IOC.AssistantBase.SetPoints(datas[4].ToString().Replace(".",","));  } 
                            catch { errors += $"{p.Tcno} kimlik numaralı personelin işe Önceki dönemlerden devreden izin gün sayısı bilgisi hatalı\r\n"; continue; }
                            p.Tih = IOC.AssistantBase.SetPoints(datas[4].ToString().Replace(".", ",")); 
                        }
                        if((from x in liste where p.Tcno == x.Tcno select x).FirstOrDefault() != null) { errors += $"{p.Tcno} kimlik numaralı personelin zaten eklenmiş\r\n"; continue; }
                        if (p.Igt > DateTime.Now) { errors += $"{p.Tcno} kimlik numaralı personelin işe giriş tarihi hatalı (Bugünden daha ileri bir tarih girilmiş)\r\n"; continue; }
                        if (p.Igt < p.Dtr) { errors += $"{p.Tcno} kimlik numaralı personelin doğum tarihi ya da işe giriş tarihi hatalı!\r\n"; continue; }
                        Personal personal = new Personal() { Tcno = p.Tcno, Ads = p.Ads, Igt = p.Igt, Dtr = p.Dtr, Tih = p.Tih };
                        decimal maxLeaveDays = IOC.PersonalService.GetMaxLeaveDay(personal);
                        if(p.Tih > maxLeaveDays) { errors += $"{p.Tcno} kimlik numaralı personelin işe Önceki dönemlerden devreden izin gün sayısı hak edilenden ({maxLeaveDays} gün) fazla girildi\r\n"; continue; }
                        liste.Add(p);
                    }
                }
                if (errors != "") { RadMessageBox.Show(errors + "\r\n Not: Hata mesajları panoya kopyalandı, herhangi bir metin editöründe yapıştırabilirsiniz", "Hata!"); Clipboard.SetText(errors); }
                return liste;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
    }
}
