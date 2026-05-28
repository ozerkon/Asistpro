using Models.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SgkAssistant.Helpers;
using SgkAssistant.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using WebDriverX;

namespace SgkAssistant.Forms.Defs
{
    public partial class FCompany : RadForm
    {
        bool allowStart, isSgkInfoOk, isSgscOk;  Company firma = null; int lastId; bool acError = false;
        Dictionary<string, int> dc;
       
        BackgroundWorker bgw;
        int browserno;
        string cid = "", cid2 = "";
        bool cancelled = false;
        bool isPageOpen = false;
        public FCompany(Company c = null)
        {
            firma = c;
            InitializeComponent();
            
            bgw = new BackgroundWorker();
            InitializeBgw();
        }
        private void InitializeBgw()
        {
            bgw.WorkerSupportsCancellation = true;
            bgw.DoWork += new DoWorkEventHandler(BgwDoWork);
            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BgwComplated);
            bgw.ProgressChanged += new ProgressChangedEventHandler(BgwChanged);
        }
        private void fCompany_Load(object sender, EventArgs e)
        {
            string msg ;

            List<RadButton> radButtons = new List<RadButton>() { btnAddCompany, btnCancelSgk, btnClear, btnExit, btnHideMessage };
            foreach (RadButton btn in radButtons)
            {
                btn.ButtonElement.BorderElement.Visibility = ElementVisibility.Visible;
                btn.ButtonElement.BorderElement.ForeColor = Color.FromArgb(120, 148, 186);
                btn.ButtonElement.ShowBorder = true;
                btn.ForeColor = Color.FromArgb(21, 66, 139);
                btn.ElementTree.Control.Cursor = Cursors.Hand;
            }
            Control.CheckForIllegalCrossThreadCalls = false;
            pnlWait.Visible = false;
            dtpKkc.Enabled = false;
            dtpKkc.Value = new DateTime(8923, 10, 29 , 0 , 0, 0);
            browserno = Settings.Default.browser;
            GetCompanyCenters(out msg);
            if (dc != null && dc.Count > 0)
            {
                ddlCenters.Enabled = true;
                rbBranch.Enabled = true;
                ddlCenters.DataSource = dc;
                ddlCenters.DisplayMember = "Key";
                ddlCenters.ValueMember = "Value";
            }
            else
            {
                rbBranch.Enabled = false;
                ddlCenters.Enabled = false;
            }
            if (firma != null)
            {
                texCompanyName.Text = firma.CompanyName ;
                texCompanyId.Text = firma.CompanyId; cid = firma.CompanyId;
                texCompanyId2.Text = firma.CompanyId2;  cid2 = firma.CompanyId2;
                texCompanyPassword.Text = firma.CompanyPassword;
                texSystemPassword.Text = firma.SystemPassword;
                rbCenter.CheckState = (firma.Fm == firma.Id)? CheckState.Checked: CheckState.Unchecked;
                rbBranch.CheckState = (firma.Fm == firma.Id) ? CheckState.Unchecked : CheckState.Checked;
                ddlCenters.SelectedValue = firma.Fm; if (firma.Fm == firma.Id) ddlCenters.Enabled = false;
                ddlCenters.ForeColor = (firma.Fm == firma.Id) ? Color.LightGray : Color.Black;
                Company c =  IOC.CompanyDataService.GetCompanyById(firma.Fm, out msg);
                if (firma.Gun == null)
                {
                    texGibUser.Text = "";
                    texGibParola.Text = "";
                    texGibSifre.Text = "";
                }
                else if (firma.Gun.Trim() != string.Empty && firma.Gun.Trim() != "")
                {
                    texGibUser.Text = firma.Gun;
                    texGibParola.Text = firma.Gp;
                    texGibSifre.Text = firma.Gs;
                }
                else if (c != null && c.Gun != null && c.Gun.Trim() != string.Empty && c.Gun.Trim() != "")
                {
                    texGibUser.Text = c.Gun.Trim();
                    texGibParola.Text = c.Gp.Trim();
                    texGibSifre.Text = c.Gs.Trim();
                }

                texCompanyRegNo.Text = firma.Sgsc;
                texCompanyTitle.Text = firma.Unvan;
                texCompanyAdress.Text = firma.Adres;
                texBOSGM.Text = firma.Sgm;
                dtpKka.Value = (firma.Kka.ToString("dd.MM.yyyy") != "01.01.1923") ? firma.Kka : DateTime.Today;
                dtpKkc.Value = (firma.Kkc <= DateTime.Now) ? firma.Kkc : new DateTime(8923, 10, 29, 0, 0, 0, 0);
                texSpecialCode1.Text = firma.Sc1;
                texSpecialCode2.Text = firma.Sc2;
                texSpecialCode3.Text = firma.Sc3;
                texSpecialCode4.Text = firma.Sc4;
                texSpecialCode5.Text = firma.Sc5;
            }
            else
            {
                lastId = IOC.CompanyDataService.GetLastId(out msg);
                ddlCenters.Enabled = false;
                ddlCenters.ForeColor =  Color.LightGray;
            }
        }
        private void BgwDoWork(object sender, DoWorkEventArgs e)
        {
            string msg = ""; isSgscOk = false;
            IOC.SgkLinksService.CaptchaCode = null;

            // UI elementlerinden güvenli şekilde değerleri alıyoruz (Cross-thread koruması)
            string compName = "", compId = "", compId2 = "", sysPass = "", compPass = "";
            Invoke((Action)(() =>
            {
                compName = texCompanyName.Text.Trim();
                compId = texCompanyId.Text.Trim();
                compId2 = texCompanyId2.Text.Trim();
                sysPass = texSystemPassword.Text.Trim();
                compPass = texCompanyPassword.Text.Trim();
            }));

            GlobalVars.ProcessReport = "<html><strong><span style=\"font-size: 10pt\"><ul>";

            Invoke((Action)(() =>
            {
                lblReport.Text = GlobalVars.ProcessReport;
                lblReport.Text += $"<li>{compName} için SGK İşveren Sistemi'nden veri alma işlemi başladı</li>";
            }));

            isSgkInfoOk = true;

            // Alanların doluluk kontrollerini UI thread üzerinde güvenli tetikliyoruz
            Invoke((Action)(() =>
            {
                if (compName.Length == 0) { SetTextBoxError(texCompanyName, null); isSgkInfoOk = false; }
                if (compId.Length == 0) { SetTextBoxError(texCompanyId, null); isSgkInfoOk = false; }
                if (compId2.Length == 0) { SetTextBoxError(texCompanyId2, null); isSgkInfoOk = false; }
                if (sysPass.Length == 0) { SetTextBoxError(texSystemPassword, null); isSgkInfoOk = false; }
                if (compPass.Length == 0) { SetTextBoxError(texCompanyPassword, null); isSgkInfoOk = false; }
            }));

            if (!isSgkInfoOk)
            {
                Invoke((Action)(() => lblMessage.Text = "Lütfen SGK bilgilerini eksiksiz olarak doldurup tekrar deneyin"));
            }
            else
            {
                isPageOpen = false;
                Invoke((Action)(() => lblMessage.Text = string.Empty));
                Thread.Sleep(100);

                Company login = new Company()
                {
                    CompanyName = compName,
                    CompanyId = compId,
                    CompanyId2 = compId2,
                    CompanyPassword = compPass,
                    SystemPassword = sysPass
                };

                IOC.SgkLinksService.SetSgkLoginCredentials(login);
                try
                {
                    int counter = 1;
                    while (LinkGlobals.LstLinks == null)
                    {
                        LinkGlobals.LstLinks = IOC.LinksDataService.GetAllLinks(out msg);
                        if (LinkGlobals.LstLinks != null) { break; }
                        counter++;
                        if (counter == 3)
                        {
                            Invoke((Action)(() => lblMessage.Text = "Veri tabanı hatası, lütfen daha sonra tekrar deneyin."));
                            return;
                        }
                    }

                    IOC.SgkLinksService.Command = (from x in LinkGlobals.LstLinks where x.Id == 20 select x.Cmd).FirstOrDefault();
                    IOC.SgkLinksService.Url = "https://uyg.sgk.gov.tr/IsverenSistemi";
                    List<Company> companies = new List<Company>() { login };

                    IOC.LinkOps.StartProcessForLinks(companies, ref acError, out msg, ref lblReport);

                    if (msg == "Oturum başlatıldı")
                    {
                        isPageOpen = true;
                        if (bgw.CancellationPending) { e.Cancel = true; return; }
                        WebDriverWait wait = new WebDriverWait(Surucu.Driver, TimeSpan.FromSeconds(LinkGlobals.MaxWait));

                        Invoke((Action)(() => texCompanyRegNo.Text = ""));
                        counter = 1;
                        if (bgw.CancellationPending) { e.Cancel = true; }

                        Invoke((Action)(() => lblReport.Text += $"<li>{compName} adlı firmanın bilgileri alınıyor</li>"));

                        // Döngü içi kontrolü lokal değişken üzerinden kontrol edilecek şekilde optimize edildi
                        string currentRegNo = "";
                        while (currentRegNo == "")
                        {
                            if (counter == 5) { e.Cancel = true; isSgscOk = false; return; }

                            // Native Selenium 4 Lambda bekleme yapıları entegre edildi
                            IWebElement table = wait.Until(d => d.FindElement(By.XPath("//*[@id='isyeriBilgileri']/td/table/tbody/tr[2]/td[2]/table")));
                            List<IWebElement> trs = table.FindElements(By.TagName("tr")).ToList();

                            IWebElement weSgsc = wait.Until(d => d.FindElement(By.XPath("//*[@id='isyeriBilgileri']/td/table/tbody/tr[2]/td[2]/table/tbody/tr[1]/td[3]")));
                            IWebElement weUnvan = wait.Until(d => d.FindElement(By.XPath("//*[@id='isyeriBilgileri']/td/table/tbody/tr[2]/td[2]/table/tbody/tr[2]/td[3]")));

                            if (bgw.CancellationPending) { e.Cancel = true; }
                            int rowCount = 7; string weAraciText = "";
                            if (trs.Count == 8)
                            {
                                rowCount = 8;
                                IWebElement weAraci = wait.Until(d => d.FindElement(By.XPath("//*[@id='isyeriBilgileri']/td/table/tbody/tr[2]/td[2]/table/tbody/tr[3]/td[3]")));
                                weAraciText = weUnvan.Text.Trim();
                            }

                            IWebElement weAdres = wait.Until(d => d.FindElement(By.XPath($"//*[@id='isyeriBilgileri']/td/table/tbody/tr[2]/td[2]/table/tbody/tr[{rowCount - 4}]/td[3]")));
                            if (bgw.CancellationPending) { e.Cancel = true; }
                            IWebElement weBosgm = wait.Until(d => d.FindElement(By.XPath($"//*[@id='isyeriBilgileri']/td/table/tbody/tr[2]/td[2]/table/tbody/tr[{rowCount - 3}]/td[3]")));
                            if (bgw.CancellationPending) { e.Cancel = true; }
                            IWebElement weKka = wait.Until(d => d.FindElement(By.XPath($"//*[@id='isyeriBilgileri']/td/table/tbody/tr[2]/td[2]/table/tbody/tr[{rowCount - 2}]/td[3]")));
                            if (bgw.CancellationPending) { e.Cancel = true; }
                            IWebElement weKkc = wait.Until(d => d.FindElement(By.XPath($"//*[@id='isyeriBilgileri']/td/table/tbody/tr[2]/td[2]/table/tbody/tr[{rowCount - 1}]/td[3]")));
                            if (bgw.CancellationPending) { e.Cancel = true; }

                            string cleanedRegNo = IOC.LinkOps.GetElementText(weSgsc).Trim().Replace(" ", "").Replace("-", "");
                            string cleanedTitle = IOC.LinkOps.GetElementText(weUnvan).Trim();
                            string cleanedAdres = IOC.LinkOps.GetElementText(weAdres).Trim();
                            string cleanedBosgm = IOC.LinkOps.GetElementText(weBosgm).Trim();
                            string kkaText = IOC.LinkOps.GetElementText(weKka).Trim();
                            string kkcText = IOC.LinkOps.GetElementText(weKkc).Trim();

                            if (cleanedRegNo != "") { isSgscOk = true; }
                            if (rowCount == 8)
                            {
                                cleanedTitle += $" (Aracı Unvan: {weAraciText}) ";
                            }

                            int gunKka = Convert.ToInt32(kkaText.Substring(0, 2));
                            int ayKka = Convert.ToInt32(kkaText.Substring(3, 2));
                            int yilKka = Convert.ToInt32(kkaText.Substring(6, 4));

                            // UI Güncellemeleri tamamen Invoke bloğuna taşındı
                            Invoke((Action)(() =>
                            {
                                texCompanyRegNo.Text = cleanedRegNo;
                                texCompanyTitle.Text = cleanedTitle;
                                texCompanyAdress.Text = cleanedAdres;
                                texBOSGM.Text = cleanedBosgm;
                                dtpKka.Value = new DateTime(yilKka, ayKka, gunKka);

                                if (!string.IsNullOrEmpty(kkcText))
                                {
                                    int gunKkc = Convert.ToInt32(kkcText.Substring(0, 2));
                                    int ayKkc = Convert.ToInt32(kkcText.Substring(3, 2));
                                    int yilKkc = Convert.ToInt32(kkcText.Substring(6, 4));
                                    dtpKkc.Value = new DateTime(yilKkc, ayKkc, gunKkc);
                                }
                                else
                                {
                                    dtpKkc.Value = new DateTime(8923, 10, 29, 0, 0, 0);
                                }
                            }));

                            if (bgw.CancellationPending) { e.Cancel = true; }
                            currentRegNo = cleanedRegNo;
                            counter++;
                        }
                        if (bgw.CancellationPending) { e.Cancel = true; }
                    }
                    else if (msg.Contains("iptal")) { e.Cancel = true; return; }
                    else if (msg == "continue" || msg.Contains("Hata") || msg == "continueGoAhead")
                    {
                        Invoke((Action)(() =>
                        {
                            lblMessage.Text = $"{login.CompanyName} adlı firmanın bilgilerine ulaşılamadı {IOC.SgkLinksService.Message}";
                        }));
                        return;
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    Invoke((Action)(() =>
                    {
                        lblMessage.Text = "SGK İşveren Sisteminden bilgiler alınamadı! Tekrar deneyiniz";
                    }));
                    e.Cancel = true;
                }
                finally
                {
                    Invoke((Action)(() => lblReport.Text += "</span></strong></ul></html>"));
                }
                if (bgw.CancellationPending) { e.Cancel = true; }
                if (Settings.Default.disposeDriver && Surucu.Driver != null) { Surucu.Driver.Dispose(); Surucu.Driver = null; }
            }
        }
        private void BgwComplated(object sender, RunWorkerCompletedEventArgs e)
        {
            string msg;
            if (e.Error != null)
            {
                Invoke((Action)(() =>
                {
                    lblMessage.Text = $"Hata: ! {e.Error.Message}";
                }));
            }
            else if (e.Cancelled)
            {
                Invoke((Action)(() =>
                {
                    lblMessage.Text = isSgscOk ? $"İşlem iptal edildi!" : "Firma bilgileri okunamadı, Lütfen tekrar deneyin";
                }));
                GlobalVars.CancelProcess = false; 
            }
            else if(isPageOpen)
            {
                CheckGibAndRegNoTextBoxes();
                if (allowStart == true)
                {
                    
                    AddUpdateCompany(out msg);
                }
            }
            bgw.Dispose();
            ProcessFinished(); 
        }
        private void BgwChanged(object sender, ProgressChangedEventArgs e)
        {

        }
        #region buttons
        private void CompanyType_Click(object sender, EventArgs e)
        {
            if (sender == rbCenter)
            {
                ddlCenters.Enabled = false;
                ddlCenters.ForeColor = Color.LightGray;
            }
            else if (sender == rbBranch)
            {
                ddlCenters.Enabled = true;
                ddlCenters.ForeColor = Color.Black;
                ddlCenters_SelectedIndexChanged(null, null);
            }
        }
        private void btnAddCompany_Click(object sender, EventArgs e)
        {
            
            if (cid != "" && cid2 != "" && cid == texCompanyId.Text && cid2 == texCompanyId2.Text)
            {
                string msg = ""; isSgkInfoOk = true; allowStart = true;
                try
                {

                    CheckTextBoxes();
                    if (!isSgkInfoOk)
                    {
                        lblMessage.Text = "Lütfen SGK bilgilerini eksiksiz olarak doldurup tekrar deneyin";
                    }
                    else
                    {
                        CheckGibAndRegNoTextBoxes();
                        if (allowStart == true)
                        {
                            AddUpdateCompany(out msg);
                        }
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message.ToString();
                    lblMessage.Text = $"İşlem tamamlanamadı! Hata: {msg}";
                }
            }
            else
            {
                allowStart = false; isSgkInfoOk = true;
                CheckTextBoxes();
                if (!isSgkInfoOk)
                {
                    lblMessage.Text = "Lütfen SGK bilgilerini eksiksiz olarak doldurup tekrar deneyin";
                }
                else
                {
                    ProcessStarted();
                    if (!bgw.IsBusy)
                    {
                        bgw.RunWorkerAsync();
                    }
                }
                
            }
        }
        private void CheckTextBoxes()
        {
            if (texCompanyName.Text.Trim().Length == 0) { SetTextBoxError(texCompanyName, null); isSgkInfoOk = false; }
            if (texCompanyId.Text.Trim().Length == 0) { SetTextBoxError(texCompanyId, null); isSgkInfoOk = false; }
            if (texCompanyId2.Text.Trim().Length == 0) { SetTextBoxError(texCompanyId2, null); isSgkInfoOk = false; }
            if (texSystemPassword.Text.Trim().Length == 0) { SetTextBoxError(texSystemPassword, null); isSgkInfoOk = false; }
            if (texCompanyPassword.Text.Trim().Length == 0) { SetTextBoxError(texCompanyPassword, null); isSgkInfoOk = false; }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            texCompanyName.Clear(); texCompanyId.Clear(); texCompanyId2.Clear();  texCompanyPassword.Clear(); texSystemPassword.Clear(); texGibUser.Clear(); texGibParola.Clear(); texGibSifre.Clear(); texCompanyRegNo.Clear(); texCompanyTitle.Clear(); texCompanyAdress.Clear(); texBOSGM.Clear(); dtpKka.Value = DateTime.Today; dtpKkc.Value = DateTime.Today; texSpecialCode1.Clear(); texSpecialCode2.Clear(); texSpecialCode3.Clear(); texSpecialCode4.Clear(); texSpecialCode5.Clear();
            texCompanyName.Focus();
        }
        private void btnHideMessage_Click(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
        }
        #endregion

        #region thread
       
        private void ProcessStarted()
        {
            rwbGetSgkInfo.Text = "SKG İşveren Sisteminden Firma Bilgileri Alınıyor";
            btnCancelSgk.Text = "SGK'dan veri almayı iptal et";
            IOC.SgkLinksService.LoginBtnClicked = false;
            btnAddCompany.Enabled = false;
            lblReport.Text = string.Empty;
            btnClear.Enabled= false;
            btnExit.Enabled = false;
            pnlWait.Visible = true;
            rwbGetSgkInfo.StartWaiting();
        }
        private void ProcessFinished()
        {
            btnAddCompany.Enabled = true;
            btnClear.Enabled = true;
            btnExit.Enabled = true;
            pnlWait.Visible = false;
            rwbGetSgkInfo.StopWaiting();
            if (cancelled) { lblMessage.Text = isSgscOk ? $"İşlem iptal edildi!" : "Firma bilgileri okunamadı, Lütfen tekrar deneyin"; }
            GlobalVars.CancelProcess = false;
            LinkGlobals.LinkCancel = false;
            IOC.LinkOps.DisposeProcess();
        }
        private void btnGetSgkInfo_Click(object sender, EventArgs e)
        {
            allowStart = false;
            ProcessStarted();
            if (!bgw.IsBusy)
            {
                bgw.RunWorkerAsync();
            }
        }

        private void btnCancelSgk_Click(object sender, EventArgs e)
        {
            btnCancelSgk.Text = "İşlem iptal ediliyor...";
            lblReport.Text = "Lütfen işlem iptal edilene kadar bekleyiniz";
            rwbGetSgkInfo.Text = "İşveren Sistemi'nden bilgi alma iptal ediliyor";
            btnCancelSgk.Enabled = false;
            bgw.CancelAsync(); Thread.Sleep(1000);
            if (Surucu.Driver != null)
            {
                Surucu.Driver.Dispose(); Surucu.Driver = null;
            }
        }

        #endregion

        #region controls

        private void ddlCenters_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            string msg = "";
            try
            {
                int id = Convert.ToInt32(ddlCenters.SelectedValue);
                Company c = IOC.CompanyDataService.GetCompanyById(id, out msg);
                if (c != null)
                {
                    texGibUser.Text = c.Gun;
                    texGibParola.Text = c.Gp;
                    texGibSifre.Text = c.Gs;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }
        private void cbCaptcha_ToolTipTextNeeded(object sender, Telerik.WinControls.ToolTipTextNeededEventArgs e)
        {
            e.ToolTipText = "Demo sürümünde Oto Captcha üçüncü parti OCR programı ile sağlanmaktadır,\r\n bütün güvenlik kodlarının çözülmesini garanti etmez, özellikle karmaşık arka planlı\r\n ve eğik yazılarla yazılmış kodların çözülebilmesi düşük bir ihtimaldir.\r\n Üç deneme sonunda kod çözülemezse kodu manuel olarak girmeniz istenecektir";
        }

         #endregion

        #region textBoxEvents
        private void texCompanyName_KeyUp(object sender, KeyEventArgs e)
        {
            if (texCompanyName.Text.Trim().Length < 2)
            {
                lblCompanyName.Visible = true; allowStart = false;
            }
            else
            {
                lblCompanyName.Visible = false; allowStart = true;
            }
        }
        
        private void texCompanyId_KeyUp(object sender, KeyEventArgs e)
        {
            Regex rx = new Regex(@"^[0-9]{1}[0-9]{9}[02468]{1}$");
            if (!rx.IsMatch(texCompanyId.Text.Trim()))
            {
                lblCompanyId.Visible = true; allowStart = false;
            }
            else
            {
                lblCompanyId.Visible = false; allowStart = true;
            }
        }

        private void texCompanyId2_KeyUp(object sender, KeyEventArgs e)
        {
            Regex rx = new Regex(@"^[0-9]{1,4}$");
            if (!rx.IsMatch(texCompanyId2.Text.Trim()))
            {
                lblCompanyId2.Visible = true; allowStart = false;
            }
            else
            {
                lblCompanyId2.Visible = false; allowStart = true;
            }
        }

        private void texSystemPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (texSystemPassword.Text.Trim().Length < 1)
            {
                lblSystemPassword.Visible = true; allowStart = false;
            }
            else
            {
                lblSystemPassword.Visible = false; allowStart = true;
            }
        }

        private void texCompanyPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (texCompanyPassword.Text.Trim().Length < 2)
            {
                lblCompanyPassword.Visible = true; allowStart = false;
            }
            else
            {
                lblCompanyPassword.Visible = false; allowStart = true;
            }
        }

        private void OnlyDigit_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if (Char.IsDigit(e.KeyChar) || e.KeyChar == '\b' || e.KeyChar == '\u0016' || Char.IsControl(e.KeyChar))  
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void SetTextBoxError(object sender, EventArgs e)
        {
            RadTextBoxControl t = sender as RadTextBoxControl;
            t.TextBoxElement.BorderColor = Color.MediumVioletRed;
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            string msg = "";
            try
            {
                RadTextBoxControl t = sender as RadTextBoxControl;
                t.TextBoxElement.BorderColor = Color.FromArgb(156, 189, 232);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }

        private void lblMessage_TextChanged(object sender, EventArgs e)
        {
            if (lblMessage.Text == string.Empty)
            {
                lblMessage.Visible = false;
            }
            else
            {
                lblMessage.Visible = true;
            }
            if (lblMessage.Text.Contains("ChromeDriver only supports"))
            {
                lblMessage.Text = "Chrome sürümü güncel değil. Lütfen Chrome'u güncelleyip tekrar deneyin."; return;
            }
        }
        #endregion

        #region methods
        private void CheckGibAndRegNoTextBoxes()
        {
            allowStart = true;
            if (texGibParola.Text.Trim().Length > 0)
            {
                if (texGibUser.Text.Trim().Length == 0)
                {
                    lblMessage.Text = "Gib bilgilerini eksiksiz doldurun ya da sonra ayarlamak için hepsini boş bırakın"; allowStart = false;
                    SetTextBoxError(texGibUser, null);
                }
                if (texGibSifre.Text.Trim().Length == 0)
                {
                    lblMessage.Text = "Gib bilgilerini eksiksiz doldurun ya da sonra ayarlamak için hepsini boş bırakın"; allowStart = false;
                    SetTextBoxError(texGibSifre, null);
                }
            }
            if (texGibSifre.Text.Trim().Length > 0)
            {
                if (texGibUser.Text.Trim().Length == 0)
                {
                    lblMessage.Text = "Gib bilgilerini eksiksiz doldurun ya da sonra ayarlamak için hepsini boş bırakın"; allowStart = false;
                    SetTextBoxError(texGibUser, null);
                }
                if (texGibParola.Text.Trim().Length == 0)
                {
                    lblMessage.Text = "Gib bilgilerini eksiksiz doldurun ya da sonra ayarlamak için hepsini boş bırakın"; allowStart = false;
                    SetTextBoxError(texGibParola, null);
                }
            }
            if (texGibUser.Text.Trim().Length > 0)
            {
                if (texGibParola.Text.Trim().Length == 0)
                {
                    lblMessage.Text = "Gib bilgilerini eksiksiz doldurun ya da sonra ayarlamak için hepsini boş bırakın"; allowStart = false;
                    SetTextBoxError(texGibParola, null);
                }
                if (texGibSifre.Text.Trim().Length == 0)
                {
                    lblMessage.Text = "Gib bilgilerini eksiksiz doldurun ya da sonra ayarlamak için hepsini boş bırakın"; allowStart = false;
                    SetTextBoxError(texGibSifre, null);
                }
                if (texGibSifre.Text.Trim().Length > 0 && texGibParola.Text.Trim().Length > 0)
                {
                    Regex rx = new Regex(@"^[0-9]{8}$");
                    if (!rx.IsMatch(texGibUser.Text.Trim()))
                    {
                        lblMessage.Text = "GİB Kullanıcı Adı,8 haneli bir sayı olmalıdır"; allowStart = false;
                        SetTextBoxError(texGibUser, null);
                    }
                    else
                    {
                        CheckCompanyRegNo();
                    }
                }
            }
        }
        
        private void CheckCompanyRegNo()
        {
            if (texCompanyRegNo.Text.Trim().Length > 0)
            {
                Regex rx = new Regex(@"^[0-9]{29}$");
                if (!rx.IsMatch(texCompanyRegNo.Text.Trim()))
                {
                    lblMessage.Text = "Firma Sicil Numarası, 26 haneli bir sayı olmalıdır"; allowStart = false;
                }
                else
                {
                    allowStart = true;
                }
            }
        }

        private void GetCompanyCenters(out string msg)
        {
            msg = "";
            try
            {
                dc = IOC.CompanyDataService.GetCompanyCenters(out msg);
            }
            catch (Exception ex)
            {
                dc = null;
                msg = ex.Message.ToString();
            }
        }

        private void AddUpdateCompany(out string msg)
        {
            List<Company>  list=  IOC.CompanyDataService.GetCompanies(out msg);
            int lc = list == null ? 0 : list.Count;
            int mcc = PackageHelper.Mcc;
            Company company = new Company()
            {
                CompanyName = texCompanyName.Text.Trim().ToUpper(),
                CompanyId = texCompanyId.Text.Trim(),
                CompanyId2 = texCompanyId2.Text.Trim(),
                SystemPassword = texSystemPassword.Text.Trim(),
                CompanyPassword = texCompanyPassword.Text.Trim(),
                Fm = 1, // duruma göre değişecek. bkz: #region firma_merkezi
                Gun = texGibUser.Text.Trim(),
                Gp = texGibParola.Text.Trim(),
                Gs = texGibSifre.Text.Trim(),
                Sgsc = texCompanyRegNo.Text.Trim(),
                Unvan = texCompanyTitle.Text.Trim(),
                Adres = texCompanyAdress.Text.Trim(),
                Sgm = texBOSGM.Text.Trim(),
                Kka = dtpKka.Value,
                Kkc = dtpKkc.Value,
                Sc1 = texSpecialCode1.Text.Trim(),
                Sc2 = texSpecialCode2.Text.Trim(),
                Sc3 = texSpecialCode3.Text.Trim(),
                Sc4 = texSpecialCode4.Text.Trim(),
                Sc5 = texSpecialCode5.Text.Trim(),
                Cu = Users.ActiveUser.Id,
            };
            if(firma == null) // var olan bir firmayı eklemeye çalışıyorsa
            {
                firma = (from x in list where (x.CompanyId == company.CompanyId && x.CompanyId2 == company.CompanyId2) select x).FirstOrDefault();
            }
            #region firma_merkezi
            if (lc == 0) // hiç firma yok ilk oluşturulan firmayı merkez olarak belirle
            {
                company.Fm = 1;
                IOC.CompanyDataService.ResetCompanyId(out msg);
            }
            else if (rbBranch.IsChecked == true) // şube olarak belirlenmiş, ddlCenter açılır listesinde seçili firmanın id değerini ver
            {
                company.Fm = Convert.ToInt32(ddlCenters.SelectedValue);
            }
            else if (ddlCenters.Enabled == false && firma == null) // "Yeni" butonuna basılmış, girilen bilgiler yeni bir firmaya ait ve merkez olarak belirlenmiş
            {
                company.Fm = lastId + 1; // listedeki son id değerinin bi fazlasını ver. oluşacak id değeri ile aynı olur
            }
            else if (ddlCenters.Enabled != true && firma != null)// "Detay" butonuna basılmış, firma bilgileri güncellenecek ve merkez olarak belirlenmiş
            {
                company.Fm = firma.Id; // kendi id değerini ver
            }
            
            #endregion
            try
            {
                if (company.Fm == 0)
                {
                    lblMessage.Text = $"Lütfen {company.CompanyName} adlı firmanın bağlı olduğu merkezi seçiniz!";
                }
                else
                {
                    int result = IOC.CompanyDataService.AddCompany(company, PackageHelper.Mcc, out msg);
                    if (result == 1)
                    {
                        lblMessage.Text = $"{company.CompanyName} adlı firma eklendi"; firma = company; firma.Id = lastId + 1;
                        IOC.WinHelpers.AddSgscEnc(company.Sgsc, out msg);
                        
                        Changed.HasChanged = true; Changed.HasChangedForDialogBox = true;
                        texCompanyName.Focus(); Thread.Sleep(500); GetCompanyCenters(out msg); Thread.Sleep(500); ddlCenters.Refresh();
                        dtpKkc.Enabled = false; 
                        lastId = IOC.CompanyDataService.GetLastId(out msg);
                        cid = Encrypt.DecryptString( company.CompanyId, GlobalVars.PassPhrase); 
                        cid2 = Encrypt.DecryptString(company.CompanyId2, GlobalVars.PassPhrase);
                    }
                    else if (result == 2 || result == 3)
                    {
                        lblMessage.Text = $"{company.Sgsc} sicil numaralı firmanın bilgileri değiştirildi";
                        Changed.HasChanged = true; Changed.HasChangedForDialogBox = true; GetCompanyCenters(out msg); Thread.Sleep(1000); ddlCenters.Refresh();
                        dtpKkc.Enabled = false; dtpKkc.Value = DateTime.Today;
                        cid = Encrypt.DecryptString(company.CompanyId, GlobalVars.PassPhrase);
                        cid2 = Encrypt.DecryptString(company.CompanyId2, GlobalVars.PassPhrase);
                    }
                    else
                    {
                        lblMessage.Text = $"Veritabanı hatası, lütfen tekrar deneyiniz";
                    }
                    if(result == 3)
                    {
                        // firma merkez iken şube konumuna düşürüldüyse, kullanıcıya durumu bildir ve bu firmalar için merkez seçtir
                        if ((from x in GlobalVars.Companies where x.Id != x.Fm && x.Fm == company.Id select x).ToList().Count == 0) return;
                        FCompanyCenter f = new FCompanyCenter(company);
                        f.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }

        #endregion

        private void fCompany_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bgw.IsBusy)
            {
                bgw.CancelAsync();
            }
            Settings.Default.browser = browserno;
        }
    } 
}
