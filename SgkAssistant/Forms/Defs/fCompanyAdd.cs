using Models.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SgkAssistant.Forms.Sgk;
using SgkAssistant.Helpers;
using SgkAssistant.LinkOperations;
using SgkAssistant.Properties;
using SGKServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Localization;
using WebDriverX;

namespace SgkAssistant.Forms.Defs
{
    public partial class FCompanyAdd : Telerik.WinControls.UI.RadForm
    {
        Company login;
        bool hasUpdate, mccReach;
        bool hasError, isSgscOk = false;
        int lastId;
        bool acError;
        int added , updated;
        string addCompaniesReport = "";
        Dictionary<string, int> dc;
        //int BROWSERNO;
        BackgroundWorker bgw;

        public FCompanyAdd()
        {
            RadGridLocalizationProvider.CurrentProvider = new LocalizationForImport();
            bgw = new BackgroundWorker();
            InitializeComponent();
            InitializeBgw();
        }

        private void fCompanyAdd_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            List<RadButton> radButtons = new List<RadButton>() { btnCreateTemplate, btnPasteFromClipboard, btnClearRgv, btnCancelSgk, btnOpenFile,  btnGetSgkInfo, btnReport };
            foreach (RadButton btn in radButtons)
            {
                btn.ButtonElement.BorderElement.Visibility = ElementVisibility.Visible;
                btn.ButtonElement.BorderElement.ForeColor = Color.FromArgb(120, 148, 186);
                btn.ButtonElement.ShowBorder = true;
                btn.ForeColor = Color.FromArgb(21, 66, 139);
                btn.ElementTree.Control.Cursor = Cursors.Hand;
            }
            lblAlert.Visible = false;
            pnlWait.Visible = false;
            rwbGetSgkInfo.StopWaiting();
            Changed.HasChangedForDialogBox = false;
            rgvCompanyFromExcel.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            btnReport.Enabled = false;
           // BROWSERNO = Settings.Default.browser;
            //Properties.Settings.Default.browser = 0;
        }

        private void InitializeBgw()
        {
            bgw.WorkerSupportsCancellation = true;
            bgw.DoWork += new DoWorkEventHandler(BgwDoWork);
            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BgwComplated);
            bgw.ProgressChanged += new ProgressChangedEventHandler(BgwChanged);
        }

        private void BgwDoWork(object sender, DoWorkEventArgs e)
        {
            string msg = "";
            mccReach = false;
            hasUpdate = false;
            hasError = false;
            addCompaniesReport = "";

            // Ana UI elementlerini güvenli bir şekilde sıfırlıyoruz
            Invoke((Action)(() =>
            {
                lblMessage.Text = string.Empty;
                btnReport.Enabled = false;
            }));

            // Cross-thread hatası almamak için Excel grid verilerini güvenli bir listeye kopyalıyoruz
            List<Company> ExcelCompaniesList = new List<Company>();
            Invoke((Action)(() =>
            {
                foreach (GridViewRowInfo row in rgvCompanyFromExcel.Rows)
                {
                    if (row.Cells[0].Value == null) continue;

                    Company c = new Company
                    {
                        CompanyName = row.Cells[0].Value?.ToString().Trim() ?? "",
                        CompanyId = row.Cells[1].Value?.ToString().Trim() ?? "",
                        CompanyId2 = row.Cells[2].Value?.ToString().Trim() ?? "",
                        SystemPassword = row.Cells[3].Value?.ToString().Trim() ?? "",
                        CompanyPassword = row.Cells[4].Value?.ToString().Trim() ?? "",
                        Gun = row.Cells[5].Value?.ToString().Trim() ?? "",
                        Gp = row.Cells[6].Value?.ToString().Trim() ?? "",
                        Gs = row.Cells[7].Value?.ToString().Trim() ?? "",
                        Sc1 = row.Cells[8].Value?.ToString().Trim() ?? "",
                        Sc2 = row.Cells[9].Value?.ToString().Trim() ?? "",
                        Sc3 = row.Cells[10].Value?.ToString().Trim() ?? "",
                        Sc4 = row.Cells[11].Value?.ToString().Trim() ?? "",
                        Sc5 = row.Cells[12].Value?.ToString().Trim() ?? ""
                    };
                    ExcelCompaniesList.Add(c);
                }
            }));

            // Artık tamamen bellek üzerinden (UI'dan bağımsız) döngüyü güvenle çalıştırabiliriz
            foreach (Company currentLogin in ExcelCompaniesList)
            {
                login = currentLogin;
                IOC.SgkLinksService.SetSgkLoginCredentials(login);

                int counter = 1;
                while (LinkGlobals.LstLinks == null)
                {
                    LinkGlobals.LstLinks = IOC.LinksDataService.GetAllLinks(out msg);
                    if (LinkGlobals.LstLinks != null) { break; }
                    counter++;
                    if (counter == 3)
                    {
                        Invoke((Action)(() => { lblMessage.Text = "Veri tabanı hatası, lütfen daha sonra tekrar deneyin."; }));
                        return;
                    }
                }

                IOC.LinkOps.CmdUrlVurl = LinkGlobals.LstLinks.Where(x => x.Id == 20).Select(x => new CmdUrlVurl { Cmd = x.Cmd, Url = x.Url, Vurl = x.Vurl }).FirstOrDefault();
                IOC.SgkLinksService.Command = IOC.LinkOps.CmdUrlVurl?.Cmd;
                IOC.SgkLinksService.Vurl = IOC.LinkOps.CmdUrlVurl?.Vurl;
                IOC.SgkLinksService.Url = IOC.LinkOps.CmdUrlVurl?.Url;
                List<Company> companies = new List<Company>() { login };
                IOC.SgkLinksService.LoginBtnClicked = false;

                #region getInfoForCompany
                if (bgw.CancellationPending) { e.Cancel = true; return; }

                IOC.SgkLinksService.CompanyName = login.CompanyName;
                try
                {
                    GlobalVars.ProcessReport = "<html><ul>";
                    IOC.LinkOps.StartProcessForLinks(companies, ref acError, out msg, ref lblReport);

                    if (msg.Contains("Oturum başlatıldı"))
                    {
                        WebDriverWait wait = IOC.CommonFuncs.GetWait();
                        System.Threading.Thread.CurrentThread.CurrentCulture = GlobalVars.SetCulture();
                        isSgscOk = true;

                        Invoke((Action)(() =>
                        {
                            GlobalVars.ProcessReport += $"<li><span style=\"font-size: 10pt\"><strong>{login.CompanyName} adlı firmanın bilgileri alınıyor</strong></span></li>";
                            lblReport.Text = GlobalVars.ProcessReport;
                        }));

                        login.Sgsc = "";
                        counter = 1;
                        while (login.Sgsc == "")
                        {
                            // Selenium 4 Modern Lambda Bekleme Yapısı
                            IWebElement table = wait.Until(d => d.FindElement(By.XPath("//*[@id='isyeriBilgileri']/td/table/tbody/tr[2]/td[2]/table")));
                            List<IWebElement> trs = table.FindElements(By.TagName("tr")).ToList();

                            IWebElement weSgsc = wait.Until(d => d.FindElement(By.XPath("//*[@id='isyeriBilgileri']/td/table/tbody/tr[2]/td[2]/table/tbody/tr[1]/td[3]")));
                            if (bgw.CancellationPending) { e.Cancel = true; return; }
                            IWebElement weUnvan = wait.Until(d => d.FindElement(By.XPath("//*[@id='isyeriBilgileri']/td/table/tbody/tr[2]/td[2]/table/tbody/tr[2]/td[3]")));
                            if (bgw.CancellationPending) { e.Cancel = true; return; }

                            int rowCount = 7; string weAraciText = "";
                            if (trs.Count == 8)
                            {
                                rowCount = 8;
                                IWebElement weAraci = wait.Until(d => d.FindElement(By.XPath("//*[@id='isyeriBilgileri']/td/table/tbody/tr[2]/td[2]/table/tbody/tr[3]/td[3]")));
                                weAraciText = weUnvan.Text.Trim();
                            }
                            IWebElement weAdres = wait.Until(d => d.FindElement(By.XPath($"//*[@id='isyeriBilgileri']/td/table/tbody/tr[2]/td[2]/table/tbody/tr[{rowCount - 4}]/td[3]")));
                            if (bgw.CancellationPending) { e.Cancel = true; return; }
                            IWebElement weBosgm = wait.Until(d => d.FindElement(By.XPath($"//*[@id='isyeriBilgileri']/td/table/tbody/tr[2]/td[2]/table/tbody/tr[{rowCount - 3}]/td[3]")));
                            if (bgw.CancellationPending) { e.Cancel = true; return; }
                            IWebElement weKka = wait.Until(d => d.FindElement(By.XPath($"//*[@id='isyeriBilgileri']/td/table/tbody/tr[2]/td[2]/table/tbody/tr[{rowCount - 2}]/td[3]")));
                            if (bgw.CancellationPending) { e.Cancel = true; return; }
                            IWebElement weKkc = wait.Until(d => d.FindElement(By.XPath($"//*[@id='isyeriBilgileri']/td/table/tbody/tr[2]/td[2]/table/tbody/tr[{rowCount - 1}]/td[3]")));
                            if (bgw.CancellationPending) { e.Cancel = true; return; }

                            login.Sgsc = IOC.LinkOps.GetElementText(weSgsc).Trim().Replace(" ", "").Replace("-", "");
                            if (login.Sgsc != "") { isSgscOk = true; }

                            login.Unvan = IOC.LinkOps.GetElementText(weUnvan).Trim();
                            if (rowCount == 8)
                            {
                                login.Unvan += $" (Aracı Unvan: {weAraciText}) ";
                            }
                            login.Adres = IOC.LinkOps.GetElementText(weAdres).Trim();
                            login.Sgm = IOC.LinkOps.GetElementText(weBosgm).Trim();

                            string kkaText = IOC.LinkOps.GetElementText(weKka).Trim();
                            int gun = Convert.ToInt32(kkaText.Substring(0, 2));
                            int ay = Convert.ToInt32(kkaText.Substring(3, 2));
                            int yil = Convert.ToInt32(kkaText.Substring(6, 4));
                            login.Kka = new DateTime(yil, ay, gun);

                            string kkcText = IOC.LinkOps.GetElementText(weKkc).Trim();
                            if (!string.IsNullOrEmpty(kkcText))
                            {
                                login.Kkc = DateTime.Parse(kkcText);
                            }
                            else
                            {
                                login.Kkc = new DateTime(8923, 10, 29, 0, 0, 0);
                            }

                            counter++;
                            Thread.Sleep(100);
                            if (counter == 60) { e.Cancel = true; isSgscOk = false; return; }
                        }

                        #region setFm
                        dc = IOC.CompanyDataService.GetCompanyCenters(out msg);
                        Company tempC = IOC.CompanyDataService.GetCompanyBySgkIds(login.CompanyId, login.CompanyId2, out msg);
                        if (tempC != null && !string.IsNullOrEmpty(tempC.CompanyName))
                        {
                            login.Fm = tempC.Fm;
                        }
                        else
                        {
                            if (dc != null && dc.Count > 0)
                            {
                                foreach (var item in dc)
                                {
                                    if (bgw.CancellationPending) { e.Cancel = true; return; }
                                    Company compFm = new Company();
                                    int id = item.Value;
                                    lastId = IOC.CompanyDataService.GetLastId(out msg);
                                    compFm = IOC.CompanyDataService.GetCompanyById(id, out msg);
                                    if (login.Gun == compFm.Gun)
                                    {
                                        login.Fm = compFm.Id;
                                        break;
                                    }
                                    else
                                    {
                                        login.Fm = lastId + 1;
                                    }
                                }
                            }
                        }
                        #endregion

                        int sonuc = IOC.CompanyDataService.AddCompany(login, PackageHelper.Mcc, out msg);

                        Invoke((Action)(() =>
                        {
                            if (sonuc == 0)
                            {
                                GlobalVars.ProcessReport += $"<li><span style=\"font-size: 10pt; color: red\"><strong>{login.CompanyName}, eklenemedi, lütfen bilgileri kontrol edip tekrar deneyin</strong></span></li></ul></html>";
                                lblReport.Text = GlobalVars.ProcessReport;
                                addCompaniesReport += GlobalVars.ProcessReport;
                                lblReport.Text = "<html><ul>";
                                GlobalVars.ProcessReport = lblReport.Text;
                                hasError = true;
                            }
                            else if (sonuc == 1)
                            {
                                GlobalVars.ProcessReport += $"<li><span style=\"font-size: 10pt; color: DarkGreen\"><strong>{login.CompanyName}, eklendi</strong></span></li></ul></html>";
                                added++;
                                IOC.WinHelpers.AddSgscEnc(login.Sgsc, out msg);
                                lblReport.Text = GlobalVars.ProcessReport;
                                addCompaniesReport += GlobalVars.ProcessReport;
                                lblReport.Text = "<html><ul>";
                                GlobalVars.ProcessReport = lblReport.Text;
                            }
                            else if (sonuc == 2)
                            {
                                GlobalVars.ProcessReport += $"<li><span style=\"font-size: 10pt; color: DarkGreen\"><strong>{login.Sgsc}, sicil numaralı firmanın bilgileri güncellendi</strong></span></li></ul></html>";
                                hasUpdate = true;
                                updated++;
                                lblReport.Text = GlobalVars.ProcessReport;
                                addCompaniesReport += GlobalVars.ProcessReport;
                                lblReport.Text = "<html><ul>";
                                GlobalVars.ProcessReport = lblReport.Text;
                            }
                            else if (sonuc == 3)
                            {
                                GlobalVars.ProcessReport += $"<li><span style=\"font-size: 10pt; color: red\"><strong>Satın aldığınız paketteki firma ekleme limitine ({PackageHelper.Mcc}) ulaştınız.</strong></span></li></ul></html>";
                                mccReach = true;
                                lblReport.Text = GlobalVars.ProcessReport;
                                addCompaniesReport += GlobalVars.ProcessReport;
                                lblReport.Text = "<html><ul>";
                                GlobalVars.ProcessReport = lblReport.Text;
                            }
                        }));

                        if (sonuc == 3) break; // Limite ulaşıldıysa döngüden çık

                        IOC.LinkOps.LogOut("x", "//*[@id='navigation']/li[5]", "İŞVEREN SİSTEMİ", out msg);
                        if (bgw.CancellationPending) { e.Cancel = true; return; }
                    }
                    else if (msg.Contains("iptal"))
                    {
                        Invoke((Action)(() =>
                        {
                            GlobalVars.ProcessReport += $"<li><span style=\"font-size: 10pt; color: red\"><strong>Firma ekleme işlemi iptal edildi</strong></span></li></ul></html>";
                        }));
                        e.Cancel = true;
                        return;
                    }
                    else if (msg == "continue" || msg.Contains("Hata"))
                    {
                        GlobalVars.ProcessReport += $"<li><strong><span style=\"color: red; font-size: 10pt\"><strong>{login.CompanyName} için oturum açılamadı! {IOC.SgkLinksService.Message}</span></strong></li></ul></html>";
                        Invoke((Action)(() =>
                        {
                            lblReport.Text = GlobalVars.ProcessReport;
                            addCompaniesReport += GlobalVars.ProcessReport;
                            lblReport.Text = "<html><ul>";
                            GlobalVars.ProcessReport = lblReport.Text;
                        }));
                        continue;
                    }
                    else if (msg == "continueGoAhead")
                    {
                        GlobalVars.ProcessReport += $"<li><strong><span style=\"color: red; font-size: 10pt\"><strong>Oturum açıldıktan sonra devam edilemiyor. Hata: {IOC.SgkLinksService.Message}</span></strong></li></ul></html>";
                        Invoke((Action)(() =>
                        {
                            lblReport.Text = GlobalVars.ProcessReport;
                            addCompaniesReport += GlobalVars.ProcessReport;
                            lblReport.Text = "<html><ul>";
                            GlobalVars.ProcessReport = lblReport.Text;
                        }));
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }

                if (bgw.CancellationPending) { e.Cancel = true; return; }
                #endregion
            }
        }

        private void BgwComplated(object sender, RunWorkerCompletedEventArgs e)
        {
            Changed.HasChangedForDialogBox = true; btnReport.Enabled = true; Changed.HasChanged = true;
            if (e.Error != null)
            {
                lblAlert.Text = $"Hata: ! {e.Error.Message}";
            }
            else if (e.Cancelled)
            {
                lblAlert.Text = isSgscOk ? $"İşlem iptal edildi!" : "Firma bilgileri okunamadı, Lütfen tekrar deneyin";
                btnReport.Enabled = false;
                GlobalVars.CancelProcess = false;
            }
            else
            {
                if (hasError == false)
                {
                    if (mccReach)
                    {
                        if (hasUpdate)
                        {
                            lblMessage.Text = $"{added} adet firma eklendi {updated} adet firmanın bilgileri güncellendi. Satın aldığınız paketteki firma ekleme limitine ({PackageHelper.Mcc} adet) ulaştığınız için bazı firmalar eklenemedi. Ayrıntılar için yandaki butona tıklayın. Gelirler idaresi başkanlığı'nın web sayfalarına girmek için gerekli olan bilgileri eklemediyseniz, Firma Listesi pencersinde, ilgili firmayı seçtikten sonra Detay butonuna basarak ekleyebilirsiniz. "; btnReport.Enabled = true;
                        }
                        else
                        {
                            lblMessage.Text = $"{added} adet firma eklendi. Satın aldığınız paketteki firma ekleme limitine ({PackageHelper.Mcc} adet) ulaştığınız için bazı firmalar eklenemedi. Ayrıntılar için yandaki butona tıklayın. Gelirler idaresi başkanlığı'nın web sayfalarına girmek için gerekli olan bilgileri eklemediyseniz, Firma Listesi pencersinde, ilgili firmayı seçtikten sonra Detay butonuna basarak ekleyebilirsiniz. ";
                        }
                    }
                    else
                    {
                        if (hasUpdate)
                        {
                            lblMessage.Text = $"{added} adet firma eklendi {updated} adet firmanın bilgileri güncellendi. Ayrıntılar için yandaki butona tıklayın.Gelirler idaresi başkanlığı'nın web sayfalarına girmek için gerekli olan bilgileri eklemediyseniz, Firma Listesi pencersinde ilgili firmayı seçtikten sonra Detay butonuna basarak ekleyebilirsiniz."; btnReport.Enabled = true;
                        }
                        else
                        {
                            lblMessage.Text = $"{added} adet firma eklendi. Gelirler idaresi başkanlığı'nın web sayfalarına girmek için gerekli olan bilgileri eklemediyseniz, Firma Listesi pencersinde, ilgili firmayı seçtikten sonra Detay butonuna basarak ekleyebilirsiniz.";
                        }
                    }
                    
                }
                else
                {
                    lblMessage.Text = "Veritabanına kayıt işleminde bazı hatalara rastlandı. Ayrıntılar için \"Kayıt Sonucu\" butona tıklayın.";
                }
            }
            bgw.Dispose();
            ProcessFinished();
        }

        private void BgwChanged(object sender, ProgressChangedEventArgs e)
        {

        }
        #region buttons
        private void btnPasteFromClipboard_Click(object sender, EventArgs e)
        {
            pnlBottom.Visible = false; ;
            string msg = "";
            lblAlert.Text = string.Empty;
            acError = false;
            List<ExcelComp> comp = PasteIntoGrid(out msg);
            if (comp != null && comp.Count > 0)
            {
                List<ExcelComp> temp = IOC.WinHelpers.GetAllExcelCompaniesFromRgv(rgvCompanyFromExcel, out msg);
                rgvCompanyFromExcel.Columns.Clear();

                List<ExcelComp> unionList = temp; bool exists = false;

                if (temp != null && temp.Count > 0)
                {
                    foreach (var c in comp)
                    {
                        foreach (var t in temp)
                        {
                            if (t.CompanyId == c.CompanyId && t.CompanyId2 == c.CompanyId2 && t.CompanyName == c.CompanyName)
                            {
                                exists = true; break;
                            }
                            exists = false;
                        }
                        if (exists == false)
                        {
                            unionList.Add(c);
                        }
                    }
                }
                else
                {
                    unionList = comp;
                }

                rgvCompanyFromExcel.DataSource = unionList;

                pnlBottom.Visible = true;
                int i = 0;
                rgvCompanyFromExcel.Columns[i++].HeaderText = "FİRMA ADI"; 
                rgvCompanyFromExcel.Columns[i++].HeaderText = "KULLANICI ADI";
                rgvCompanyFromExcel.Columns[i++].HeaderText = "KULLANICI KODU";
                rgvCompanyFromExcel.Columns[i++].HeaderText = "SİSTEM ŞİFRESİ";
                rgvCompanyFromExcel.Columns[i++].HeaderText = "İŞYERİ ŞİFRESİ";
                rgvCompanyFromExcel.Columns[i++].HeaderText = "GİB KULLANICI ADI";
                rgvCompanyFromExcel.Columns[i++].HeaderText = "GİB PAROLA";
                rgvCompanyFromExcel.Columns[i++].HeaderText = "GİB ŞİFRE";
                rgvCompanyFromExcel.Columns[i++].HeaderText = "ÖZEL KOD 1";
                rgvCompanyFromExcel.Columns[i++].HeaderText = "ÖZEL KOD 2";
                rgvCompanyFromExcel.Columns[i++].HeaderText = "ÖZEL KOD 3";
                rgvCompanyFromExcel.Columns[i++].HeaderText = "ÖZEL KOD 4";
                rgvCompanyFromExcel.Columns[i++].HeaderText = "ÖZEL KOD 5";

                lblAlert.Text = "";
            }
            else
            {
                lblAlert.Text = "Kopyalama işlemi başarısız! Lütfen Excel'deki verilerinizi kontrol edip tekrar deneyiniz.";
            }
        }

        private void btnGetSgkInfo_Click(object sender, EventArgs e)
        {
            ProcessStarted();
            if (!bgw.IsBusy)
            {
                bgw.RunWorkerAsync();
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            try
            {
                lblReport.Text += $"</html>";
                addCompaniesReport += lblReport.Text.Replace(@"</html><html>", " ");
                addCompaniesReport = addCompaniesReport.Replace(@"</html><html>", " ").Replace("<ul></html>", "</html>").Replace("12pt", "18pt").Replace("10pt", "14pt"); ;
                IOC.ExportService.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"firma_ekle.html");
                using (StreamWriter writer = new StreamWriter(IOC.ExportService.FileName))
                {
                    StringBuilder stringBuilder = new StringBuilder(); stringBuilder.Append(addCompaniesReport);
                    writer.WriteLine(stringBuilder);
                }
                string location = IOC.ExportService.FileName;
                Process.Start(location);
            }
            catch (Exception)
            {
                MessageBox.Show("Hata: sonuç dosyası görüntülenemiyor");
            }
            
        }

        private void btnCreateTemplate_Click(object sender, EventArgs e)
        {
            string msg = "";
            bool result = false;
            try
            {
                IOC.ExportService.CreateFile(out msg);
                result = IOC.ExportService.CreateTemplateForCompanies(out msg);

                if (result)
                {
                    IOC.ExportService.Save("Firma Listesi", out msg);
                    btnOpenFile.Visible = true;
                }
                lblAlert.Text = msg;
            }
            catch (Exception ex)
            {
                lblAlert.Text = $"Dosya oluşturulamadı! Hata: {ex.Message.ToString()}"; btnOpenFile.Visible = false;
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
            rgvCompanyFromExcel.Columns.Clear();
            rgvCompanyFromExcel.DataSource = new List<ExcelComp>(); pnlBottom.Visible = false;
            int i = 0;
            rgvCompanyFromExcel.Columns[i++].HeaderText = "FİRMA ADI";
            rgvCompanyFromExcel.Columns[i++].HeaderText = "KULLANICI ADI";
            rgvCompanyFromExcel.Columns[i++].HeaderText = "KULLANICI KODU";
            rgvCompanyFromExcel.Columns[i++].HeaderText = "SİSTEM ŞİFRESİ";
            rgvCompanyFromExcel.Columns[i++].HeaderText = "İŞYERİ ŞİFRESİ";
            rgvCompanyFromExcel.Columns[i++].HeaderText = "GİB KULLANICI ADI";
            rgvCompanyFromExcel.Columns[i++].HeaderText = "GİB PAROLA";
            rgvCompanyFromExcel.Columns[i++].HeaderText = "GİB ŞİFRE";
            rgvCompanyFromExcel.Columns[i++].HeaderText = "ÖZEL KOD 1";
            rgvCompanyFromExcel.Columns[i++].HeaderText = "ÖZEL KOD 2";
            rgvCompanyFromExcel.Columns[i++].HeaderText = "ÖZEL KOD 3";
            rgvCompanyFromExcel.Columns[i++].HeaderText = "ÖZEL KOD 4";
            rgvCompanyFromExcel.Columns[i++].HeaderText = "ÖZEL KOD 5";
        }
        #endregion

        #region controls
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
        private void rbAutoCaptcha_ToolTipTextNeeded(object sender, Telerik.WinControls.ToolTipTextNeededEventArgs e)
        {
            e.ToolTipText = "Demo sürümünde Oto Captcha üçüncü parti OCR programı ile sağlanmaktadır,\r\n bütün güvenlik kodlarının çözülmesini garanti etmez, özellikle karmaşık arka planlı\r\n ve eğik yazılarla yazılmış kodların çözülebilmesi düşük bir ihtimaldir.\r\n Üç deneme sonunda kod çözülemezse kodu manuel olarak girmeniz istenecektir";
        }
        
        #endregion

        private List<ExcelComp> PasteIntoGrid(out string msg)
        {
            msg = "";
            DataObject o = (DataObject)Clipboard.GetDataObject();
            List<ExcelComp> liste = new List<ExcelComp>();
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
                        ExcelComp c = new ExcelComp();
                        List<string> datas = Regex.Split(row, "\t").ToList();
                        int i = 0;
                        if (datas.Count < 5)
                        {
                            return null;
                        }
                        else if (datas.Count >= 5)
                        {
                            c.CompanyName = datas[i++];
                            c.CompanyId = datas[i++];
                            c.CompanyId2 = datas[i++];
                            c.SystemPassword = datas[i++];
                            c.CompanyPassword = datas[i++];
                        }
                        if (datas.Count >= 6)
                        {
                            c.Gun = (datas[i++] != null) ? datas[5] : string.Empty;
                        }
                        if (datas.Count >= 7)
                        {
                            c.Gp = (datas[i++] != null) ? datas[6] : string.Empty;
                        }
                        if (datas.Count >= 8)
                        {
                            c.Gs = (datas[i++] != null) ? datas[7] : string.Empty;
                        }
                        if (datas.Count >= 9)
                        {
                            c.Sc1 = (datas[i++] != null) ? datas[8] : string.Empty;
                        }
                        if (datas.Count >= 10)
                        {
                            c.Sc2 = (datas[i++] != null) ? datas[9] : string.Empty;
                        }
                        if (datas.Count >= 11)
                        {
                            c.Sc3 = (datas[i++] != null) ? datas[10] : string.Empty;
                        }
                        if (datas.Count >= 12)
                        {
                            c.Sc4 = (datas[i++] != null) ? datas[11] : string.Empty;
                        }
                        if (datas.Count >= 13)
                        {
                            c.Sc5 = (datas[i++] != null) ? datas[12] : string.Empty;
                        }
                        liste.Add(c);
                    }
                }
                return liste;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }

        #region thread

        private void ProcessStarted()
        {
            IOC.SgkLinksService.LoginBtnClicked = false;
            btnCreateTemplate.Enabled = false;
            btnPasteFromClipboard.Enabled = false;
            btnClearRgv.Enabled = false;
            btnOpenFile.Enabled = false;
            btnGetSgkInfo.Enabled = false;
            btnReport.Enabled = false;
            btnCancelSgk.Enabled = true;
            lblReport.Text = string.Empty;
            pnlWait.Visible = true;
            rwbGetSgkInfo.StartWaiting();
        }

        private void lblMessage_TextChanged(object sender, EventArgs e)
        {
            if (lblMessage.Text.Contains("ChromeDriver only supports"))
            {
                lblMessage.Text = "Chrome sürümü güncel değil. Lütfen Chrome'u güncelleyip tekrar deneyin."; return;
            }
        }

        private void ProcessFinished()
        {
            btnCreateTemplate.Enabled = true;
            btnPasteFromClipboard.Enabled = true;
            btnClearRgv.Enabled = true;
            btnOpenFile.Enabled = true;
            btnGetSgkInfo.Enabled = true;
            btnReport.Enabled = true;
            btnCancelSgk.Enabled = true;
            pnlWait.Visible = false;
            rwbGetSgkInfo.StopWaiting();
            GlobalVars.CancelProcess = false;
            LinkGlobals.LinkCancel = false;
            IOC.LinkOps.DisposeProcess();
            if (Settings.Default.disposeDriver && Surucu.Driver != null) { Surucu.Driver.Dispose(); Surucu.Driver = null; }
        }

        #endregion

        private void fCompanyAdd_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bgw.IsBusy)
            {
                bgw.CancelAsync();
            }
            //Properties.Settings.Default.browser = BROWSERNO;
        }

        private void btnCancelSgk_Click(object sender, EventArgs e)
        {
            btnCancelSgk.Text = "İşlem iptal ediliyor...";
            btnCancelSgk.Enabled = false;
            bgw.CancelAsync();
            Thread.Sleep(1000);
        }
    }
}
