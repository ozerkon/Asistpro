using Models.Common;
using OpenQA.Selenium;
using SgkAssistant.Forms.Sgk;
using SgkAssistant.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using WebDriverX;

namespace SgkAssistant.LinkOperations
{
    public enum SgrpEnum { ebildirge, ebildirgev2, isveren, isegiris, evizite, ivd };
    public class CmdUrlVurl
    {
        public string Cmd { get; set; }
        public string Url { get; set; }
        public string Vurl { get; set; }
    }
    public class LinkOps
    {
        public CmdUrlVurl CmdUrlVurl { get; set; }
        private Process Process { get; set; }
        private SgrpEnum sgrpEnum { get; set; }

        public void SetSizesForLinkListRgv(RadGridView rgvLinkList, RadGridView rgvCompanyList, SplitPanel spCompanyList)
        {
            rgvLinkList.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            rgvLinkList.GridViewElement.TitleLabelElement.Padding = new Padding(0, 2, 0, 2);
            rgvLinkList.Columns[0].MinWidth = 80;   rgvLinkList.Columns[0].MaxWidth = 80; 
            rgvLinkList.Columns[1].MinWidth = 100;  rgvLinkList.Columns[1].MaxWidth = 100;
            rgvLinkList.Columns[2].MinWidth = 140;  rgvLinkList.Columns[2].MaxWidth = 140;
            rgvCompanyList.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            rgvCompanyList.Width = spCompanyList.Width - 2;
            rgvCompanyList.Columns[0].BestFit();
            rgvCompanyList.Columns[4].FormatString = "{0:s}";
            rgvCompanyList.Columns[4].FormatInfo = CultureInfo.CreateSpecificCulture("tr-TR");
        }
        public void FavColumnSet(RadGridView rgvLinkList,  out string msg)
        {
            msg = "";
            try
            {
                foreach (GridViewRowInfo rowInfo in rgvLinkList.Rows)
                {
                    rowInfo.Cells["AddRemoveFavs"].Value = rowInfo.Cells["Fav"].Value;
                }
            }
            catch (Exception ex)
            {
                msg = $"Favoriler sütunu ayarlanamıyor! Hata: {ex.Message}";
            }
        }
        public void StartProcessForLinks(List<Company> lst, ref bool acError,  out string msg, ref RadLabel lbl)
        {
            msg = "";
            if (IOC.SgkLinksService.Url.Contains("vizite/welcome.do")) lbl.Text = GlobalVars.ProcessReport;
            IOC.SgkLinksService.LoginError = false;
            LinkGlobals.HasRefreshButton = false;
            LinkGlobals.BrowserType = Settings.Default.browser;
            try
            {
                foreach (Company login in lst)
                {
                    IOC.SgkLinksService.LoginBtnClicked = false;
                    IOC.SgkLinksService.SetSgkLoginCredentials(login);
                    
                    if (IOC.SgkLinksService.Command.Remove(15).Contains(@"grl https://ivd"))
                    {
                        LinkGlobals.Ivd = true;
                    }
                    else
                    {
                        LinkGlobals.Ivd = false;
                    }
                    if (LinkGlobals.Ivd == true)
                    {
                        Dictionary<string, int> dic = IOC.CompanyDataService.GetCompanyCenters(out msg);
                        if (login.Gun != string.Empty)
                        {
                            IOC.SgkLinksService.SetGibCridentals(login.Gun, login.Gp, login.Gs);
                                
                            VisitLink(login, IOC.SgkLinksService.Url, ref acError,  out msg, ref lbl);
                            if (LinkGlobals.IsLink) DisposeProcess();
                            if (msg == "continue" || msg == "continueGoAhead" || msg.Contains("Hata")) continue;
                            else if (msg == "iptal") return;
                        }
                        else if (dic.Count > 0)
                        {
                            int id = 1; Company c = new Company();
                            foreach (var item in dic)
                            {
                                id = item.Value;
                                c = IOC.CompanyDataService.GetCompanyById(id, out msg);
                                break;
                            }
                            if (c.Gun != string.Empty)
                            {
                                IOC.SgkLinksService.SetGibCridentals(c.Gun, c.Gp, c.Gs);
                                VisitLink(login, IOC.SgkLinksService.Url, ref acError,  out msg, ref lbl);
                                if (LinkGlobals.IsLink) DisposeProcess();
                                if (msg == "continue" || msg == "continueGoAhead" || msg.Contains("Hata")) continue;
                                else if (msg == "iptal") return;
                            }
                            else
                            {
                                msg = "GİB için kullanıcı bilgileri bulunamadı! Lütfen İşyerlerim butonuna basarak GİB bilgilerini tanımlayın."; 
                            }
                        }
                    }
                    else
                    {
                        VisitLink(login, IOC.SgkLinksService.Url, ref acError, out msg, ref lbl);
                        if (LinkGlobals.IsLink) DisposeProcess();
                        if (msg == "continue" || msg == "continueGoAhead" || msg.Contains("Hata")) continue;
                        else if (msg == "iptal") return;
                    }
                    msg = "Oturum başlatıldı";
                } 
            }
            catch (Exception ex)
            {
                msg = $"İşlem başlatılamıyor! Hata: {ex.Message}";
            }
        }
        public void VisitLink(Company login, string url, ref bool acError,  out string msg, ref RadLabel lbl)
        {
            string komut = IOC.SgkLinksService.Command;
            int cprLoc = komut.IndexOf("cpr t:");
            int ftxLastLoc = komut.LastIndexOf("ftx t:") - 2;
            string cprCommand = komut.Substring(cprLoc, ftxLastLoc - cprLoc);
            string[] parts = cprCommand.Split(' ');
            IOC.SgkLinksService.CaptchaType = parts[1].Remove(0, 2);
            IOC.SgkLinksService.CaptchaValue = parts[2].Remove(0, 3);

            if (Surucu.Driver == null || !IsBrowserOpen()) { Surucu.Driver = IOC.WinHelpers.GetWebDriver(Settings.Default.hideBrowser, out msg); }
            IOC.SgkLinksService.CompanyName = login.CompanyName;
            try
            {
                string sayfa = "";
                if (url.Contains("EBildirgeV2")) { sayfa = "E-BİLDİRGE V2"; sgrpEnum = SgrpEnum.ebildirgev2; }
                else if (url.Contains("ebildirge")) { sayfa = "E-BİLDİRGE"; sgrpEnum = SgrpEnum.ebildirge; }
                else if (url.Contains("IsverenSistemi")) { sayfa = "İŞVEREN SİSTEMİ"; sgrpEnum = SgrpEnum.isveren; }
                else if (url.Contains("SigortaliTescil")) { sayfa = "SİGORTALI İŞE GİRİŞ-AYRILIŞ BİLDİRGELERİ"; sgrpEnum = SgrpEnum.isegiris; }
                else if (url.Contains("EBorcuYoktur5510")) { sayfa = "E-BORCU YOKTUR SORGULAMA";  sgrpEnum = SgrpEnum.isveren; }
                else if (url.Contains("vizite/welcome")) { sayfa = "E-VİZİTE"; sgrpEnum = SgrpEnum.evizite;  }
                else if (url.Contains("ivd.gib")) { sayfa = "İNTERNET VERGİ DAİRESİ"; sgrpEnum = SgrpEnum.ivd; }
                GlobalVars.ProcessReport += $"<li><strong><span style=\"font-size: 10pt\">{sayfa} SAYFASI AÇILIYOR...</span></strong></li>";
                lbl.Text = GlobalVars.ProcessReport;
                IOC.TrmBase.GotoUrl(url, out msg);
                if (LinkGlobals.LinkCancel == true || GlobalVars.CancelProcess == true) { msg = "iptal"; return; }
                if (Settings.Default.autoCaptcha)
                {
                    int tryCount = 0;
                    GlobalVars.ProcessReport += $"<li><strong><span style=\"font-size: 10pt\">{login.CompanyName} için oturum açma bilgileri giriliyor </span></strong></li>";
                    lbl.Text = GlobalVars.ProcessReport;
                    while (tryCount < 3)
                    {
                        tryCount++;
                        GlobalVars.ProcessReport += $"<li><strong><span style=\"font-size: 10pt\">Otomatik Captcha hizmeti güvenlik kodunu çözmeyi deniyor ({tryCount}. deneme )</span></strong></li>";
                        lbl.Text = GlobalVars.ProcessReport;
                        IOC.SgkLinksService.FillPageLoginFields(out msg);
                        if (LinkGlobals.LinkCancel == true || GlobalVars.CancelProcess == true) { msg = "iptal"; return; }
                        if (IOC.SgkLinksService.WeCaptchaImg == null)
                        {
                            GlobalVars.ProcessReport += $"<li><strong><span style=\"font-size: 10pt; color: red\">{login.CompanyName} için oturum açılamadı (Sayfadaki güvenlik kodu yüklenemedi) </span></strong></li>";
                            lbl.Text = GlobalVars.ProcessReport;
                            msg = "continue"; return;
                        }
                        if (IOC.SgkLinksService.CaptchaCode == null || IOC.SgkLinksService.CaptchaCode == "")
                        {
                            IOC.CaptchaService.TakeScreenshot(IOC.SgkLinksService.WeCaptchaImg, out msg);
                            GlobalVars.OcrEngine = 1;
                            IOC.SgkLinksService.CaptchaCode = IOC.Solver.Solve(out msg);
                        }
                        if (IOC.SgkLinksService.CaptchaCode == null || IOC.SgkLinksService.CaptchaCode == "")
                        {
                            tryCount = 4;
                        }
                            
                        if (tryCount == 4)
                        {
                            acError = true;
                            GlobalVars.ProcessReport += $"<li><strong><span style=\"font-size: 10pt; color: red\">Güvenlik kodu çözülemiyor; lütfen kodu manuel olarak girin </span></strong></li>";
                            lbl.Text = GlobalVars.ProcessReport;
                            Settings.Default.autoCaptcha = false; GlobalVars.AutoCaptcha = Settings.Default.autoCaptcha;
                            break;
                        }
                        if (IOC.SgkLinksService.IsLoginSuccessful(out msg))
                        {
                            GlobalVars.ProcessReport += "<li><strong><span style=\"font-size: 10pt\">Oturum açıldı, ilgili linklere gidiliyor </span></strong></li>";
                            lbl.Text = GlobalVars.ProcessReport;
                            IOC.SgkLinksService.GoAhead(out msg);
                            if (msg == "iptal")
                            {
                                GlobalVars.ProcessReport += $"<li><strong><span style=\"font-size: 10pt; color: red\">Oturum açıldıktan sonra devam edilemiyor. {IOC.SgkLinksService.Message}</span></strong></li>";
                                lbl.Text = GlobalVars.ProcessReport;
                                msg = "continueGoAhead";
                                return;
                            }else if (msg.Contains("yeniden"))
                            {
                                string browser = LinkGlobals.BrowserType == 0 ? "firefox" : "chrome";
                                GlobalVars.ProcessReport += $"<li><strong><span style=\"font-size: 10pt; color: red\">Oturum açıldıktan sonra sayfa yüklenemedi, {browser} yenileniyor</span></strong></li>";
                                lbl.Text = GlobalVars.ProcessReport;
                                RefreshAfterLogin(out msg);
                                if (msg == "tamam") break;
                                continue;
                            }
                            //RefreshAfterLogin(out msg); // silmeyi unutma
                            break;

                        }
                        else if (msg.Contains("kodu hatalı")) continue;
                        else if (msg.Contains("yeniden")) { 
                            string browser = LinkGlobals.BrowserType == 0 ? "firefox" : "chrome";
                            GlobalVars.ProcessReport += $"<li><strong><span style=\"font-size: 10pt; color: red\">Sayfa yüklenemedi, {browser} yenileniyor</span></strong></li>";
                            lbl.Text = GlobalVars.ProcessReport;
                            Surucu.Driver.Url = url;
                            //Surucu.Driver.Navigate().Refresh();
                            continue; 
                        }
                        else
                        {
                            GlobalVars.ProcessReport += "<li><strong><span style=\"font-size: 10pt; color: red\">";
                            if (msg.Contains("Kullanıcı")) GlobalVars.ProcessReport += "Kullanıcı adı ya da şifreler hatalı!";
                            else if (msg.Contains("iz olmuş")) GlobalVars.ProcessReport += "İş yeri iz olmuş, işlem yapılamaz!";
                            else if (msg.Contains("sunucularında")) GlobalVars.ProcessReport += "SGK sunucularında hata oluştu!";
                            else if (msg.Contains("iptal")) GlobalVars.ProcessReport += $"Hata: {IOC.SgkLinksService.Message}";
                            GlobalVars.ProcessReport += "</span></strong></li>";
                            lbl.Text = GlobalVars.ProcessReport;
                            msg = "continue";
                            return;
                        }
                        
                    }
                }
                if (!Settings.Default.autoCaptcha)
                {
                    if (LinkGlobals.LinkCancel == true || GlobalVars.CancelProcess == true) { msg = "iptal"; return; }
                    
                    if(!acError)
                    {
                        GlobalVars.ProcessReport += $"<li><strong><span style=\"font-size: 10pt\">{login.CompanyName} için oturum açma bilgileri giriliyor </span></strong></li>" +
                            $"<li><strong><span style=\"font-size: 10pt\">Güvenlik kodu alınıyor </span></strong></li>";
                        lbl.Text = GlobalVars.ProcessReport;
                    }
                    IOC.SgkLinksService.GetCaptchaPicture(IOC.SgkLinksService.CaptchaType, IOC.SgkLinksService.CaptchaValue, out msg);
                    if(IOC.SgkLinksService.Command.Contains("brc t:"))
                    {
                        int brcLoc = komut.IndexOf("brc t:");
                        int cprLastLoc = komut.LastIndexOf("cpr t:") - 2;
                        string brcCommand = komut.Substring(brcLoc, cprLastLoc - brcLoc);
                        parts = brcCommand.Split(' ');
                        LinkGlobals.HasRefreshButton = true;
                        LinkGlobals.RcbType = parts[1].Remove(0, 2);
                        LinkGlobals.RcbValue = parts[2].Remove(0, 3);
                    }

                    for (int i = 1; i < 5; i++)
                    {
                        IOC.SgkLinksService.BitmapCaptcha = IOC.CaptchaService.TakeScreenshot(IOC.SgkLinksService.WeCaptchaImg, out msg);
                        if (IOC.SgkLinksService.BitmapCaptcha != null) { break; } else { Thread.Sleep( i * 100); }
                    }

                    if (IOC.SgkLinksService.BitmapCaptcha == null) {
                        GlobalVars.ProcessReport += $"<li><strong><span style=\"font-size: 10pt; color: red\">Hata: Güvenlik kodu alınamadı, lütfen daha sonra tekrar deneyin</span></strong></li></ul></html>"; 
                        lbl.Text = GlobalVars.ProcessReport;
                        msg = "continue";  return; 
                    }
                    FCaptcha f = new FCaptcha(IOC.SgkLinksService.BitmapCaptcha, "links");

                    if (LinkGlobals.HasRefreshButton)
                    {
                        f = new FCaptcha(IOC.SgkLinksService.BitmapCaptcha, "links", true);
                    }
                    if (LinkGlobals.LinkCancel == true || GlobalVars.CancelProcess == true) { msg = "iptal"; return; }
                    Surucu.Driver.Manage().Window.Minimize();
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        lbl.Text += $"<li><strong><span style=\"font-size: 10pt\">Oturum açıldı, ilgili linklere gidiliyor </span></strong></li>";
                        IOC.SgkLinksService.GoAhead(out msg);
                        if (msg == "iptal")
                        {
                            GlobalVars.ProcessReport += $"<li><strong><span style=\"font-size: 10pt; color: red\">Oturum açıldıktan sonra devam edilemiyor. {IOC.SgkLinksService.Message}</span></strong></li>";
                            lbl.Text = GlobalVars.ProcessReport;
                            msg = "continueGoAhead"; 
                            return;
                        }
                        else if (msg.Contains("yeniden"))
                        {
                            string browser = LinkGlobals.BrowserType == 0 ? "firefox" : "chrome";
                            GlobalVars.ProcessReport += $"<li><strong><span style=\"font-size: 10pt; color: red\">Oturum açıldıktan sonra sayfa yüklenemedi, {browser} yenileniyor</span></strong></li>";
                            lbl.Text = GlobalVars.ProcessReport;
                            RefreshAfterLogin(out msg);
                            if (msg != "tamam") {msg = "continueGoAhead"; return; }
                        }
                        Surucu.Driver.Manage().Window.Maximize(); 
                    }
                    else { 
                        Application.OpenForms["fBaslangic"].WindowState = FormWindowState.Maximized;
                        //GlobalVars.ProcessReport += $"<li></strong><span style=\"font-size: 10pt; color: red\">İşlem kullanıcı tarafından iptal edildi</span></strong></li></ul>"; 
                        //lbl.Text = GlobalVars.ProcessReport;
                        msg = "continue";
                        return; 
                    }
                    
                }
                msg = IOC.SgkLinksService.Message;
            }
            catch (Exception ex)
            {
                msg = (!IsBrowserOpen()) ? "Hata: Web browser kullanıcı tarafından kapatıldı" : $"Hata: {ex.Message}"; 
            }
            finally
            {
                if (acError == true)
                {
                    acError = false; Settings.Default.autoCaptcha = true; GlobalVars.AutoCaptcha = true; Settings.Default.Save();
                    IOC.CommonFuncs.WaitForPageLoad(out msg);
                }
            }
        }
        public bool IsBrowserOpen()
        {
            if (Settings.Default.hideBrowser) return true;
            else if (Process == null) { 
                return false; }
           return !Process.HasExited;
        }
        public void DisposeProcess()
        {
            Process = null;
            Surucu.Driver = null;
        }
        public void SetWebBrowserProcess()
        {
            // ---- FİREFOX İÇİN KESİN ÇÖZÜM (PID YÖNTEMİ) ----
            if (Surucu.Driver is OpenQA.Selenium.IHasCapabilities hasCaps && hasCaps.Capabilities.HasCapability("moz:processID"))
            {
                int firefoxPid = Convert.ToInt32(hasCaps.Capabilities.GetCapability("moz:processID"));
                Process = System.Diagnostics.Process.GetProcessById(firefoxPid);
                return; // Firefox ise işlemi bulduk, metottan çıkabiliriz.
            }

            // ---- CHROME İÇİN BAŞLIK DEĞİŞTİRME YÖNTEMİ ----
            int tryCount = 0;
            string oldTitle = Surucu.Driver.Title ?? "";
            string newTitle = $"{Guid.NewGuid():n}";

            IJavaScriptExecutor js = (IJavaScriptExecutor)Surucu.Driver;

            try
            {
                js.ExecuteScript($"document.title = '{newTitle}'");
            }
            catch (Exception)
            {
                return;
            }

            while (Process == null && tryCount < 100)
            {
                Process = System.Diagnostics.Process.GetProcesses().FirstOrDefault(p => p.MainWindowTitle.Contains(newTitle));
                if (Process == null)
                {
                    tryCount++;
                    Thread.Sleep(100);
                }
            }

            try
            {
                js.ExecuteScript($"document.title = '{oldTitle}'");
            }
            catch { }
        }
        public string GetElementText(IWebElement element)
        {
            string text = "";
            int tryCount = 10;
            while (text == "")
            {
                Thread.Sleep(500);
                if (text == "")
                {
                    text = element.GetAttribute("innerText").Trim();
                }
                if (text == "")
                {
                    text = element.GetAttribute("textContent").Trim();
                }
                if (text == "")
                {
                    text = element.Text.Trim();
                }
                tryCount--;
                if (tryCount == 0) break;
            }
            
            return text;

        }
        public void RefreshAfterLogin(out string msg)
        {
            msg = "";
            int lckLoc = IOC.SgkLinksService.Command.LastIndexOf("lck");
            int clkLoc = IOC.SgkLinksService.Command.LastIndexOf("clk ");
            string lckLast = IOC.SgkLinksService.Command.Substring(lckLoc + 4).Replace("\r\n","").Trim(), clkLast = "";
            int count = Surucu.Driver.WindowHandles.Count;
            switch (sgrpEnum)
            {
                case SgrpEnum.ebildirge:
                    
                    if((IOC.SgkLinksService.Command.Contains("hnd t") || IOC.SgkLinksService.Command.Contains("hnd l")) && count == 2)
                    {
                        Surucu.Driver.SwitchTo().Window(Surucu.Driver.WindowHandles[0]);
                        Surucu.Driver.SwitchTo().NewWindow(WindowType.Tab);
                        Surucu.Driver.Url = "https://ebildirge.sgk.gov.tr/WPEB/amp/borcSorgu";
                        if(IOC.SgkLinksService.Vurl.Contains("icra.action")) Surucu.Driver.Url = "https://uyg.sgk.gov.tr/IsverenBorcSorgu/borc/icra.action";
                        if(IOC.SgkLinksService.Vurl.Contains("bankaEmanet.action")) Surucu.Driver.Url = "https://uyg.sgk.gov.tr/IsverenBorcSorgu/borc/bankaEmanet.action";
                        if(IOC.SgkLinksService.Vurl.Contains("araver")) Surucu.Driver.Url = "https://ebildirge.sgk.gov.tr/WPEB/amp/araver";
                    }
                    else if ( ( (IOC.SgkLinksService.Command.Contains("hnd t") || IOC.SgkLinksService.Command.Contains("hnd l")) && count == 3 ) || count == 2)
                        Surucu.Driver.Url = IOC.SgkLinksService.Vurl;
                    IOC.SgkLinksService.WaitForPageLoaded(out msg);
                    if (Surucu.Driver.PageSource.Replace("  "," ").Contains(lckLast)) msg = "tamam";
                    break;
                case SgrpEnum.ebildirgev2:
                    clkLast = IOC.SgkLinksService.Command.Substring(clkLoc + 11, lckLoc - clkLoc - 11).Replace("\r\n", "").Trim();
                    Surucu.Driver.Navigate().Refresh(); 
                    IOC.SgkLinksService.WaitForPageLoaded(out msg);
                    IOC.SgkLinksService.WaitForPageLoaded(out msg);
                    IWebElement linkText = IOC.SgkLinksService.GetElementBy("t", clkLast, out msg);
                    if(linkText != null)
                    {
                        linkText.Click(); IOC.SgkLinksService.WaitForPageLoaded(out msg);
                        if (Surucu.Driver.PageSource.Replace("  "," ").Contains(lckLast)) msg = "tamam";
                    }
                    break;
                case SgrpEnum.isveren:
                    Surucu.Driver.SwitchTo().DefaultContent();
                    Surucu.Driver.Url = "https://uyg.sgk.gov.tr/IsverenSistemi/pages/baslangic.jsf";
                    List<IWebElement> menus = IOC.SgkLinksService.GetElementsBy("cl", "ui-panelmenu-content", out msg); 
                    ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript($"let menus = document.getElementsByClassName(\"ui-panelmenu-content\");for(let t=0;t<menus.length;t++)menus[t].setAttribute(\"style\",\"display:none\");");

                    List<IWebElement> h3s = IOC.SgkLinksService.GetElementsBy("cl", "ui-panelmenu-header", out msg);
                    ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript($"let h3s = document.getElementsByClassName(\"ui-panelmenu-header\");for(let e=0;e<h3s.length;e++)h3s[e].removeAttribute(\"aria-expanded\"),h3s[e].setAttribute(\"class\",\"ui-panelmenu-header ui-state-default ui-corner-all\");");

                    IOC.SgkLinksService.GoAhead(out msg);
                    break;
                case SgrpEnum.isegiris:
                case SgrpEnum.evizite:
                    Surucu.Driver.Url = IOC.SgkLinksService.Vurl;
                    IOC.SgkLinksService.WaitForPageLoaded(out msg);
                    if (Surucu.Driver.PageSource.Replace("  "," ").Contains(lckLast)) msg = "tamam";
                    break;
                case SgrpEnum.ivd:
                    break;
            }
        }
        public void LogOut(string t, string tv, string type, out string msg)
        {
            Surucu.Driver.SwitchTo().ParentFrame();
            IWebElement BtnLogOut = IOC.SgkLinksService.GetButtonElementBy(t, tv, out msg);
            BtnLogOut.Click();

            switch (type)
            {
                case "E-BİLDİRGE":
                    IReadOnlyCollection<string> handles = Surucu.Driver.WindowHandles;
                    for (int i = handles.Count - 1; i > 0; i--)
                    {
                        Surucu.Driver.SwitchTo().Window(Surucu.Driver.WindowHandles[i]);
                        Surucu.Driver.Close();
                    }
                    break;
                case "E-BİLDİRGE V2":
                    break;
                case "İŞVEREN SİSTEMİ":

                    break;
                case "İŞE GİRİŞ-AYRILIŞ":
                    break;
                case "E-VİZİTE":
                    break;
                case "İVD":
                    break;
                
            }
            Surucu.Driver.SwitchTo().Window(Surucu.Driver.WindowHandles[0]);
        }
    }
}
