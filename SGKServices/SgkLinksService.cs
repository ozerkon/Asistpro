using Models.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SGKServices.Captcha;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using WebDriverX;
namespace SGKServices
{
    public class SgkLinksService : TrmBase
    {
        public IWebElement Element { get; set; }
        public IWebElement WeCaptchaImg { get; set; }   // captcha resminin olduğu img nesnesi
        public IWebElement WeInputCaptcha { get; set; } // captcha metninin girileceği input nesnesi
        public Bitmap BitmapCaptcha { get; set; }       // takescreenshot metodu ile oluşturulan bitmap nesnesi
        public string CaptchaCode { get; set; }         // manuel (fCaptcha formu ile) ya da otomatik elde edilen captcha metni 
        public string CompanyName { get; set; }

        public string Command { get; set; }             // veritabanından (rgvLinkList son sütun) okunan komut stringi 
        public string Vurl { get; set; }                // validation url 
        public string Url { get; set; }                // go to url 
        public string CaptchaType { get; set; } = "x";  // captcha resmine ulaşma yöntemi (x = XPath, i= ID, n= Name, c= CssSelector)
        public string CaptchaValue { get; set; }        // üstteki tipe göre elde edilen string değer
        public string TextBoxType { get; set; } = "x";  // captcha metin kutusuna ulaşma yöntemi (x = XPath, i= ID, n= Name, c= CssSelector)
        public string TextBoxValue { get; set; }        // üstteki tipe göre elde edilen string değer
        public string Message { get; set; }

        public string Gunx { get; set; }
        public string Gpx { get; set; }
        public string Gsx { get; set; }

        public bool LoginError { get; set; }
        public bool LoginBtnClicked { get; set; }
        public string Script { get; set; }
        public SgkLinksService()
        {

        }

        public void SetGibCridentals(string gunx, string gpx, string gsx)
        {
            Gunx = gunx;
            Gpx = gpx;
            Gsx = gsx;
        }
        public bool FillPageLoginFields(out string msg)
        {
            msg = "";
            try
            {
                if(Surucu.Driver.WindowHandles.Count > 1)
                {
                    Surucu.Driver.SwitchTo().Window(Surucu.Driver.CurrentWindowHandle);
                }
                if(GlobalVars.CancelProcess || LinkGlobals.LinkCancel) { msg = "iptal"; return false; }
                WebDriverWait wait = GetWait();
                int algLoc = Command.IndexOf("alg");
                string komut = algLoc > 0 ? Command.Substring(0, Command.IndexOf("alg") - 1).Trim() : Command.Trim();
                StringReader sr = new StringReader(komut);
                string line;
                while((line = sr.ReadLine()) != null)
                {
                    if(GlobalVars.CancelProcess || LinkGlobals.LinkCancel) { msg = "iptal"; return false; }

                    string[] parts = line.Split(' ');
                    switch (parts[0])
                    {
                        case "pass":
                            continue;
                        case "rva":
                            if(LoginError == false)
                            {
                                wait.Until(e => ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("return document.readyState").Equals("complete"));
                                IWebElement surveyBox = IsElementEXISTS(By.XPath(parts[2].Remove(0, 3)), 5);
                                if(surveyBox != null)
                                {
                                    surveyBox.Click(); LinkGlobals.IvdSurveyBox = false;
                                }
                            }
                            break;
                        case "iph":
                            WebDriverWait bekle = GetWait();
                            bekle.Until(e => ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("return document.readyState").Equals("complete"));
                            
                            if(!Surucu.Driver.Url.Contains(parts[1]))
                            {
                                Message = "Sayfa yüklenemedi";
                                return false;
                            }
                            break;
                        case "pgc":
                            
                            wait.Until(e => ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("return document.readyState").Equals("complete"));
                            if(!Surucu.Driver.PageSource.Replace("  "," ").Contains(line.Remove(0, 4))) {
                                Message = "Sayfa yüklenemedi"; 
                                return false;
                            }
                            break;
                        case "wfe":
                            WaitForElement(parts[1].Remove(0, 2), parts[2].Remove(0, 3), out msg);
                            break;
                        case "sdp":
                            IWebElement element = GetElementBy(parts[1].Remove(0, 2), parts[2].Remove(0, 3), out msg);
                            if(GlobalVars.CancelProcess || LinkGlobals.LinkCancel) { msg = "iptal"; return false; }
                            Actions actions = new Actions(Surucu.Driver);
                            actions.MoveToElement(element);
                            actions.Perform();
                            Thread.Sleep(500);
                            break;
                        case "cpr":
                            CaptchaType = parts[1].Remove(0, 2);
                            CaptchaValue = parts[2].Remove(0, 3);
                            
                            if(!GetCaptchaPicture(CaptchaType, CaptchaValue, out msg))
                            {
                                Message = "Güvenlik resmi bulunamadı, lütfen bağlantınızı kontrol edip tekrar deneyin.";
                                return false;
                            }
                            else
                            {
                                CaptchaCode = CaptchaProcess(CaptchaType, CaptchaValue, out msg);
                                if(CaptchaCode == "hata") { Message = "Sayfadaki Güvenlik Kodu Yüklenmedi"; return false; }
                            }
                            break;
                        case "hnd":
                            if(Surucu.Driver.WindowHandles.Count == 1) continue;
                            if(parts[1] == "2" || parts[1] == "t")
                            {
                                Surucu.Driver.SwitchTo().Window(Surucu.Driver.WindowHandles[1]);
                            }
                            else if(parts[1] == "l")
                            {
                                Surucu.Driver.SwitchTo().Window(Surucu.Driver.WindowHandles.Last());
                            }
                            else if(parts[1] == "c")
                            {
                                Surucu.Driver.SwitchTo().Window(Surucu.Driver.CurrentWindowHandle);
                            }
                            if(Command.Contains("EBorcuYoktur"))
                            {
                                SetAttirbute();
                            }
                            break;
                        case "clk":
                            if(Command.Contains("EBorcuYoktur") ){ 
                                element = GetElementBy(parts[1].Remove(0, 2), parts[2].Remove(0, 3), out msg);
                                if(GlobalVars.CancelProcess || LinkGlobals.LinkCancel) { msg = "iptal"; return false; }
                                element.Click();
                            }
                            break;
                        case "ftx":

                            FillTextBox(parts[1].Remove(0, 2), parts[2].Remove(0, 3), parts[3].Remove(0, 2), out msg);
                            if(GlobalVars.CancelProcess || LinkGlobals.LinkCancel) { msg = "iptal"; return false; }
                            if(parts[3].Remove(0, 2) == "captchaText")
                            {
                                TextBoxType = parts[1].Remove(0, 2);
                                TextBoxValue = parts[2].Remove(0, 3);
                            }
                            break;
                        case "brc":
                            LinkGlobals.HasRefreshButton = true;
                            LinkGlobals.RcbType = parts[1].Remove(0, 2);
                            LinkGlobals.RcbValue = parts[2].Remove(0, 3);
                            break;
                        case "ols":
                            element = GetElementBy(parts[1].Remove(0, 2), parts[2].Remove(0, 3), out msg);
                            if(GlobalVars.CancelProcess || LinkGlobals.LinkCancel) { msg = "iptal"; return false; }
                            element.Click();
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "WebDriver başlatılamıyor, lütfen bağlantınızı kontrol edip tekrar deneyin.";
                msg = $"Bağlantı hatası : {ex.Message}";
                return false;
            }
            Message = "Oturum açma bilgileri girildi";
            return true;
        }
        public bool FillTextBox(string t, string tv, string v , out string msg)
        {
            msg = "";
            WebDriverWait wait = GetWait();
            try
            {
                wait.Until(e => ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("return document.readyState").Equals("complete"));
                if(GlobalVars.CancelProcess || LinkGlobals.LinkCancel) { msg = "iptal"; return false; }
                Element = GetElementBy(t, tv, out msg);
                if(msg == "iptal") { msg = "iptal"; return false; }
                if(Element == null) { Message = "Metin kutusu bulunamadı"; return false; } 
                switch (v)
                {
                    case "DetailsNameTC.tcno":
                        if(Element.GetAttribute("value") != DetailsNameTc.Tcno)
                            Element.SendKeys(DetailsNameTc.Tcno);  
                        break;
                    case "login.CompanyID":
                        if(Element.GetAttribute("value") != SgkLoginCredentials.CompanyId)
                            Element.SendKeys(SgkLoginCredentials.CompanyId);
                        break;
                    case "login.CompanyID2":
                        if(Element.GetAttribute("value") != SgkLoginCredentials.CompanyId2)
                            Element.SendKeys(SgkLoginCredentials.CompanyId2);
                        break;
                    case "login.CompanyPassword":
                        if(Element.GetAttribute("value") != SgkLoginCredentials.CompanyPassword)
                            Element.SendKeys(SgkLoginCredentials.CompanyPassword);
                        break;
                    case "login.SystemPassword":
                        if(Element.GetAttribute("value") != SgkLoginCredentials.SystemPassword)
                            Element.SendKeys(SgkLoginCredentials.SystemPassword);
                        break;
                    case "gun":
                        if(Element.GetAttribute("value") != Gunx)
                            Element.SendKeys(Gunx);
                        break;
                    case "gs":
                        if(Element.GetAttribute("value") != Gsx)
                            Element.SendKeys(Gsx);
                        break;
                    case "gp":
                        if(Element.GetAttribute("value") != Gpx)
                            Element.SendKeys(Gpx);
                        break;
                    case "captchaText":
                        if(CaptchaCode != null && CaptchaCode != "")
                        {
                            Element.SendKeys(CaptchaCode);
                        }
                        break;
                    default:
                        Element.SendKeys(v);
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                Message = $"Bağlantı hatası! Hata: {msg}";
                msg = ex.Message.ToString();
                return false;
            }
        }
        public bool IsAlertPresent(out string msg)
        {
            msg = "";
            try
            {
                WebDriverWait wait = new WebDriverWait(Surucu.Driver, TimeSpan.FromSeconds(10));

                // ExpectedConditions yerine modern lambda kullanımı
                IAlert alert = wait.Until(d =>
                {
                    try
                    {
                        // Alerte geçiş yapmayı dene. Başarılı olursa alert nesnesini döndür (ve beklemeyi bitir).
                        return d.SwitchTo().Alert();
                    }
                    catch (NoAlertPresentException)
                    {
                        // Henüz alert yoksa null döndür ki süre (10sn) dolana kadar denemeye devam etsin.
                        return null;
                    }
                });

                if (alert != null)
                {
                    alert.Accept(); // Alerti onayla (Tamam'a bas)
                    return true;
                }
                return false;
            }
            catch (WebDriverTimeoutException ex) // 10 saniye içinde alert çıkmazsa bu hata fırlar
            {
                msg = $"Alert bulunamadı (Zaman aşımı): {ex.Message}";
                return false;
            }
            catch (Exception ex)
            {
                msg = $"Beklenmeyen hata: {ex.Message}";
                return false;
            }
        }
        public bool GetCaptchaPicture(string t, string tv, out string msg)
        {
            tryCount = 0;
            try
            {
                WeCaptchaImg = GetElementBy(t, tv, out msg);
                while (WeCaptchaImg == null)
                {
                    if (GlobalVars.CancelProcess || LinkGlobals.LinkCancel) { Message = "İşlem kullanıcı tarafından iptal edildi."; msg = "iptal"; return false; }
                    WeCaptchaImg = GetElementBy(t, tv, out msg);
                    if(tryCount < 50) { tryCount++; Thread.Sleep(100); }
                    else break;
                }
                if(WeCaptchaImg != null)
                {
                    tryCount = 0;
                    return true;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
            tryCount = 0;
            return false;
        }
        public string CaptchaProcess(string t, string tv, out string msg) 
        {
            msg = "";
            try
            {
                CaptchaService cs = new CaptchaService();
                ICaptchaSolver solver = SolverFactory.CreateSolver(GlobalVars.SolverType);
                
                BitmapCaptcha = null;
                int tryCount = 1;
                while(BitmapCaptcha == null)
                {
                    if(tryCount != 1) Surucu.Driver.Navigate().Refresh();
                    CommonFuncs cf = new CommonFuncs();
                    cf.SetDriver(Surucu.Driver);
                    cf.WaitForPageLoad(out msg); 
                    if(WeCaptchaImg == null) { WaitForImageLoad(tv, out msg, t);GetCaptchaPicture(CaptchaType, CaptchaValue, out msg);}
                    BitmapCaptcha = cs.TakeScreenshot(WeCaptchaImg, out msg);
                    if(tryCount == 5) 
                        break;
                }
                if(BitmapCaptcha == null) { return "hata"; }
                if(GlobalVars.AutoCaptcha == true)
                {
                    GlobalVars.OcrEngine = 1;
                    return solver.Solve(out msg); // cs.Solve(weCaptchaImg, out msg);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
            return String.Empty;
        }
        public IWebElement GetElementBy(string t, string tv, out string msg)
        {
            msg = "";
            try
            {
                tryCount = 0;
                WebDriverWait wait = base.GetWait();
                if (!ExecuteScript(t, tv)) { wait.Until(e => ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("return document.readyState").Equals("complete")); }
                if (GlobalVars.CancelProcess || LinkGlobals.LinkCancel) { msg = "iptal"; return null; }
                switch (t)
                {
                    case "n":
                        Element = wait.Until(e => e.FindElement(By.Name(tv)));
                        break;
                    case "i":
                        Element = wait.Until(e => e.FindElement(By.Id(tv)));
                        break;
                    case "x":
                        Element = wait.Until(e => e.FindElement(By.XPath(tv)));
                        break;
                    case "c":
                        Element = wait.Until(e => e.FindElement(By.CssSelector(tv)));
                        break;
                    case "t":
                        Element = wait.Until(e => e.FindElement(By.LinkText(tv)));
                        break;
                    case "cl":
                        Element = wait.Until(e => e.FindElement(By.ClassName(tv)));
                        break;
                    case "tn":
                        Element = wait.Until(e => e.FindElement(By.TagName(tv)));
                        break;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
            return Element;
        }
        public List<IWebElement> GetElementsBy(string t, string tv, out string msg)
        {
            msg = "";
            List<IWebElement> element = new List<IWebElement>();
            try
            {
                WebDriverWait wait = base.GetWait();
                if(!ExecuteScript(t, tv)) { wait.Until(e => ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("return document.readyState").Equals("complete")); }

                if(GlobalVars.CancelProcess || LinkGlobals.LinkCancel) { msg = "iptal"; return null; }

                element = wait.Until(e => e.FindElements(By.ClassName(tv))).ToList();

            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
            return element;
        }
        public IWebElement GetButtonElementBy(string t, string tv, out string msg)
        {
            msg = "";
            try
            {
                WebDriverWait wait = GetWait();
                if (!ExecuteScript(t, tv)) { wait.Until(e => ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("return document.readyState").Equals("complete")); }
                if (GlobalVars.CancelProcess || LinkGlobals.LinkCancel) { msg = "iptal"; return null; }

                // 1. Önce Hangi Seçiciyi (Locator) Kullanacağımızı Belirliyoruz
                By locator = null;
                switch (t)
                {
                    case "n": locator = By.Name(tv); break;
                    case "i": locator = By.Id(tv); break;
                    case "x": locator = By.XPath(tv); break;
                    case "c": locator = By.CssSelector(tv); break;
                    case "t": locator = By.LinkText(tv); break;
                }

                if (locator != null)
                {
                    // 2. Elementin var olmasını, görünür (Displayed) ve tıklanabilir (Enabled) olmasını bekliyoruz
                    Element = wait.Until(d =>
                    {
                        var el = d.FindElement(locator);
                        return (el != null && el.Displayed && el.Enabled) ? el : null;
                    });
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
            return Element;
        }
        public string GetTableFromPage(string t, string tv, int sr, int lr,  out string msg)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                IWebElement table = GetElementBy(t, tv, out msg);
                if(GlobalVars.CancelProcess || LinkGlobals.LinkCancel) { return  "iptal";  }

                if(table == null ){ msg = "Tablo bulunamadı"; return null; }
                
                List<IWebElement> trs = table.FindElements(By.TagName("tr")).ToList();
                if(trs == null || trs.Count == 0) { msg = "Tabloda satır bulunamadı"; return null; }
                if(trs.Count == sr - 1) { msg = "Tabloda veri bulunamadı"; return "boş"; }

                int count = 1;
                foreach (IWebElement tr in trs)
                {
                    if(count < sr) { count++; continue; }
                    
                    List<IWebElement> tds = tr.FindElements(By.TagName("td")).ToList();
                    foreach (IWebElement td in tds)
                    {
                        sb.Append($"{td.Text}\t");
                    }
                    sb.Append($"\r\n");
                    if(count == trs.Count - lr) { break; }
                    count++;
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public bool IsLoginSuccessful(out string msg)
        {
            msg = "";
            bool whnd = false, hasLoaded = false;
            bool isverenSistemi = Command.Contains("https://uyg.sgk.gov.tr/IsverenSistemi") ? true : false;
            string komut = "";
            Surucu.Driver.Manage().Window.Maximize();
            try
            {
                if(GlobalVars.AutoCaptcha == false)
                {
                    if(!FillTextBox(TextBoxType, TextBoxValue, "captchaText", out msg)) {
                        msg = "yeniden";
                        Message = "Sisteme giriş metin kutuları bulunamadı";
                        return false;  // ilk metin kutusu dolduktan sonra diğerlerinde sorun çıkarsa!!!
                    };
                }

                WebDriverWait wait = GetWait();
                if(LoginBtnClicked)
                {
                    int firstBck = Command.IndexOf("bck");
                    if(firstBck > 0)
                    {
                        int endOfBck = Command.IndexOf("\r\n", firstBck);
                        komut = Command.Substring(endOfBck + 2);
                    }
                }
                else
                {
                    komut = Command;
                }

                StringReader sr = new StringReader(komut); //return false;
                string line;
                int counter = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    if(GlobalVars.CancelProcess || LinkGlobals.LinkCancel) { 
                        Message = "İşlem kullanıcı tarafından iptal edildi."; msg = "iptal"; return false; 
                    }
                    string[] parts = line.Split(' ');
                    switch (parts[0])
                    {
                        case "hnd":
                            if(parts[1] == "2" || parts[1] == "t")
                            {
                                Surucu.Driver.SwitchTo().Window(Surucu.Driver.WindowHandles[1]);
                            }
                            else if(parts[1] == "l")
                            {
                                Surucu.Driver.SwitchTo().Window(Surucu.Driver.WindowHandles.Last());
                            }
                            break;
                        case "whnd": // işveren sisteminde pagesource işe yaramıyor; oturum başarılıysa ikinci pencere açılıyor.  
                            counter = 1;
                            while (Surucu.Driver.WindowHandles.Count == 1)
                            {
                                if(GlobalVars.CancelProcess || LinkGlobals.LinkCancel) { 
                                    Message = "İşlem kullanıcı tarafından iptal edildi."; msg = "iptal"; return false; }
                                Thread.Sleep(100);
                                counter++;
                                if( counter * 10 >= 180) {
                                    msg = "yeniden";
                                    Message = "Sayfa yüklenmesi çok uzun sürdüğü için işlem iptal edildi.";
                                    return false;
                                }
                            }
                            break;
                        case "bck":
                            Element = GetButtonElementBy(parts[1].Remove(0, 2), parts[2].Remove(0, 3), out msg);
                            if(Element == null) { 
                                msg = "yeniden"; Message =  "Giriş butonu bulunamadı"; LoginBtnClicked = false; return false; 
                            
                            } 
                            hasLoaded = false; int continueWaiting = 0; counter = 1;
                            while (!hasLoaded)
                            {
                                if(GlobalVars.CancelProcess || LinkGlobals.LinkCancel) { Message = "İşlem kullanıcı tarafından iptal edildi."; msg = "iptal"; return false; }
                                try
                                {
                                    if(!LoginBtnClicked) {LoginBtnClicked = true; Element.Click(); }
                                    if(isverenSistemi)
                                    {
                                        int cnt = 0;
                                        while (wait.Until(e => e.FindElement(By.Id("light"))).GetCssValue("display") == "block")
                                        {
                                            Thread.Sleep(500);
                                            cnt++;
                                            if(cnt == LinkGlobals.MaxWait * 2) continueWaiting++;
                                            if(continueWaiting >= 3)
                                            {
                                                msg = "yeniden";
                                                Message = "Sayfa yüklenmesi çok uzun sürdüğü için işlem iptal edildi.";
                                                return false;
                                            }
                                        }
                                    }
                                    while (continueWaiting >= 0 && IsElementEXISTS(By.TagName("body")) == null)
                                    { 
                                        Thread.Sleep(500); 
                                        counter++;
                                        if(counter  == LinkGlobals.MaxWait * 2)
                                        {
                                            continueWaiting++; break;
                                        }
                                        if(continueWaiting >= 3)
                                        {
                                            msg = "yeniden";
                                            Message = "Sayfa yüklenmesi çok uzun sürdüğü için işlem iptal edildi.";
                                            return false;
                                        }
                                    }
                                    hasLoaded = true;
                                }
                                catch (WebDriverException)
                                {
                                    continueWaiting++;
                                    if(continueWaiting >= 3) {
                                        msg = "yeniden";
                                        Message = "Sayfa yüklenmesi çok uzun sürdüğü için işlem iptal edildi.";
                                        return false;
                                    }
                                }
                            }
                            break;
                        case "err":

                            wait.Until(e => ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("return document.readyState").Equals("complete"));
                            if(isverenSistemi && Surucu.Driver.WindowHandles.Count == 2) continue; // işveren sistemi için
                            if(Surucu.Driver.PageSource.Replace("  "," ").Contains("Kullanıcı adı veya şifreleriniz hatalıdır") || Surucu.Driver.PageSource.Replace("  "," ").Contains("Lütfen verilerinizi kontrol ederek tekrar deneyiniz") || Surucu.Driver.PageSource.Replace("  "," ").Contains("alanlarından biri veya birkaçı"))
                            {
                                Message = "Kullanıcı adı veya şifreleriniz hatalıdır";
                                msg = "Kullanıcı adı veya şifreleriniz hatalıdır"; LoginBtnClicked = false; return false;
                            }
                            else if(Surucu.Driver.PageSource.Replace("  "," ").Contains("Güvenlik Anahtarı hatalıdır") || Surucu.Driver.PageSource.Replace("  "," ").Contains("Guvenlik Kodu Hatali"))
                            {
                                Message = "Güvenlik kodu hatalı girildi";
                                msg = "Güvenlik kodu hatalı girildi"; LoginBtnClicked = false; return false;
                            }else if(Surucu.Driver.PageSource.Replace("  "," ").Contains("yeri iz olmu"))
                            {
                                Message = "İş yeri iz olmuş, işlem yapılamaz";
                                msg = "İş yeri iz olmuş, işlem yapılamaz"; LoginBtnClicked = false; return false;
                            }
                            else if(Surucu.Driver.PageSource.Replace("  "," ").Contains("Sistem Tablosundan veri çekilirken hata oluştu"))
                            {
                                Message = "SGK sunucularında hata!"; 
                                msg = "SGK sunucularında hata!"; LoginBtnClicked = false; return false;
                            }
                            if(!GlobalVars.IsCompanyActive && GlobalVars.HesapTypeEtDbCr)
                            {
                                IWebElement btnMain = GetButtonElementBy("x", "/html/body/table[3]/tbody/tr/td/center/table/tbody/tr[2]/td[2]/center/b/a", out msg);
                                btnMain.Click();
                                int sayac = 0;
                                while (Surucu.Driver.WindowHandles.Count < 3)
                                {
                                    Thread.Sleep(100);
                                    sayac++;
                                    if(sayac == 200) return false;
                                }
                                
                                Surucu.Driver.SwitchTo().Window(Surucu.Driver.WindowHandles.Last());
                                wait.Until(e => ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("return document.readyState").Equals("complete"));
                                if(Surucu.Driver.PageSource.Replace("  "," ").Contains("İŞYERİ SİCİL NUMARASI")) return true; 
                            }
                            
                            break;
                        case "cfrm":
                            try
                            {
                                wait.Until(e => ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("return document.readyState").Equals("complete"));
                                for (int i = 0; i < 10; i++)
                                {
                                    if(Surucu.Driver.PageSource.Replace("  "," ").Contains(parts[3].Remove(0, 2).Replace("'", "")))
                                    {
                                        IWebElement btnOnay = GetButtonElementBy(parts[1].Remove(0, 2), parts[2].Remove(0, 3), out msg);
                                        btnOnay.Click();
                                    }
                                    Thread.Sleep(100);
                                }
                            }
                            catch (WebDriverException)
                            {
                                msg = "yeniden";
                                Message = "Uyarı kutucuğu kapatılamadığı için işlem iptal edildi"; return false;
                            }
                            break;
                        case "ils":
                            wait.Until(e => ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("return document.readyState").Equals("complete"));
                            if(whnd) break;
                            
                            if(!IsPageLoad(line.Remove(0, 4), LinkGlobals.MaxWait))
                            {
                                msg = "yeniden";
                                Message = $"Sayfa yüklenmesi çok uzun sürdüğü için işlem iptal edildi.";
                                LoginBtnClicked = false; return false;
                            }
                            
                            break;
                        case "ilh":
                            if(Surucu.Driver.Url.Contains(parts[1]))
                            {
                                return true;
                            }
                            else
                            {
                                if(GlobalVars.CancelProcess || LinkGlobals.LinkCancel) { Message = "İşlem kullanıcı tarafından iptal edildi."; msg = "iptal"; return false; }
                                WebDriverWait bekle = GetWait();
                                bekle.Until(e => ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("return document.readyState").Equals("complete"));
                                IWebElement errorAlert = IsElementEXISTS(By.XPath("//*[@id='runtime-body']/div[4]/div[2]/div/table/tbody/tr/td/span"), 5);

                                if(errorAlert != null)
                                {
                                    IWebElement captchaErrorAlert = IsElementEXISTS(By.XPath("//*[@id='runtime-body']/div[4]/div[2]/div/div/div/input"), 3);
                                    SearchReport.LoginMessage = errorAlert.Text;
                                    captchaErrorAlert.Click(); LoginError = true; LoginBtnClicked = false; return false;
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                Message = $"Oturum başlatılamadı! Hata: {msg}";
                LoginBtnClicked = false;
                return false;
            }
            Message = "Oturum başlatıldı...";
            LoginBtnClicked=false;
            return true;
        }
        public bool GoAhead(out string msg)
        {
            msg = "";
            bool hasLoaded = false; bool clicked = false; int continueWaiting = 1;
            bool isverenSistemi = Command.Contains("https://uyg.sgk.gov.tr/IsverenSistemi") || Command.Contains("İŞVEREN") ? true : false;
            WebDriverWait wait = GetWait();
            string komut = Command.IndexOf("alg") > 1 ? Command.Substring(Command.IndexOf("alg") + 3).Trim() : Command.Trim();
            if (!GlobalVars.IsCompanyActive && GlobalVars.HesapTypeEtDbCr) komut = Command.Substring(Command.IndexOf("lck"));
            StringReader sr = new StringReader(komut);
            string line;
            int counter = 1;
            try
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (GlobalVars.CancelProcess || LinkGlobals.LinkCancel)
                    {
                        Message = "İşlem kullanıcı tarafından iptal edildi."; msg = "iptal"; return false;
                    }
                    string[] parts = line.Split(' ');
                    switch (parts[0])
                    {
                        case "clk":
                            if (parts[1].Remove(0, 2) == "t")
                            {
                                Element = GetButtonElementBy(parts[1].Remove(0, 2), line.Remove(0, 11), out msg);
                            }
                            else
                            {
                                // ESKİ KOD: Element = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(parts[2].Remove(0, 3))));
                                // YENİ KOD: Elementin bulunmasını, görünür (Displayed) olmasını ve tıklanabilir (Enabled) olmasını bekliyoruz.
                                Element = wait.Until(d =>
                                {
                                    var el = d.FindElement(By.XPath(parts[2].Remove(0, 3)));
                                    return (el != null && el.Displayed && el.Enabled) ? el : null;
                                });
                            }
                            if (LinkGlobals.Ivd == true)
                            {

                                ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript($"document.getElementById(\"{parts[2].Remove(0, 3)}\").parentElement.setAttribute(\"style\", \"display:block\");");
                                ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript($"document.getElementById(\"{parts[2].Remove(0, 3)}\").parentElement.parentElement.parentElement.setAttribute(\"style\", \"display:block\");");

                                ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("arguments[0].scrollIntoView(true); ", Element);
                            }
                            if (Element == null)
                            {
                                Message = $"Oturum açma işleminden sonra devam edilemiyor. Hata: {msg}"; msg = "yeniden"; return false;
                            }
                            hasLoaded = false; clicked = false; continueWaiting = 1;
                            while (!hasLoaded)
                            {
                                try
                                {
                                    if (!clicked) { Element.Click(); clicked = true; }
                                    if (LinkGlobals.LinkCancel == true) { Message = "İşlem kullanıcı tarafından iptal edildi."; msg = "iptal"; return false; }
                                    while (continueWaiting > 1)
                                    {
                                        IsElementEXISTS(By.TagName("body"));
                                    }
                                    hasLoaded = true;
                                }
                                catch (WebDriverException)
                                {
                                    continueWaiting++;
                                    if (continueWaiting >= 3)
                                    {
                                        msg = "yeniden";
                                        Message = "Sayfa yüklenmesi çok uzun sürdüğü için işlem iptal edildi.";
                                        return false;
                                    }
                                }
                            }
                            if (LinkGlobals.Ivd == true)
                            {
                                ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript($"document.getElementById(\"{parts[2].Remove(0, 3)}\").parentElement.removeAttribute(\"style\");");
                                ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript($"document.getElementById(\"{parts[2].Remove(0, 3)}\").parentElement.parentElement.parentElement.removeAttribute(\"style\");");
                            }
                            break;
                        case "omo":
                            JustMouseHover(parts[1].Remove(0, 2), parts[2].Remove(0, 3), out msg);
                            break;
                        case "hnd":
                            if (parts[1] == "2" || parts[1] == "t")
                            {
                                Surucu.Driver.SwitchTo().Window(Surucu.Driver.WindowHandles[0]);
                                Actions action = new Actions(Surucu.Driver);
                                action.SendKeys(OpenQA.Selenium.Keys.Control + "t").Build().Perform();

                                //switch to the new tab
                                List<string> handle = Surucu.Driver.WindowHandles.ToList();
                                for (int i = 0; i < handle.Count; i++)
                                {
                                    if (!handle[i].Equals(Surucu.Driver.WindowHandles[0]))
                                    {
                                        Surucu.Driver.SwitchTo().Window(handle[i]);
                                    }
                                }
                            }
                            else if (parts[1] == "l")
                            {
                                Surucu.Driver.SwitchTo().Window(Surucu.Driver.WindowHandles.Last());
                            }
                            break;
                        case "stf":
                            int say = 1;
                            while (say <= 3)
                            {
                                if (LinkGlobals.LinkCancel == true) { Message = "İşlem kullanıcı tarafından iptal edildi."; msg = "iptal"; return false; }
                                try
                                {
                                    Surucu.Driver.SwitchTo().Frame(parts[1]);
                                    break;
                                }
                                catch (WebDriverException)
                                {
                                    say++;
                                    if (say >= 3)
                                    {
                                        msg = "yeniden";
                                        Message = "Sayfa yüklenmesi çok uzun sürdüğü için işlem iptal edildi.";
                                        return false;
                                    }
                                }

                            }
                            break;
                        case "bck":

                            try
                            {
                                if (counter < 11 || Command.Contains("https://uyg.sgk.gov.tr/EBorcuYoktur5510/amp/loginldap") || Command.Contains("https://ivd.gib.gov.tr")) { continue; } // counter 11 ?
                                else
                                {
                                    Element = GetElementBy(parts[1].Remove(0, 2), parts[2].Remove(0, 3), out msg);
                                    Element.Click();
                                }
                            }
                            catch (WebDriverException)
                            {
                                Message = $"Oturum açma işleminden sonra devam edilemiyor. Hata: {msg}";
                                msg = "yeniden";  // sayfayı yeniletsek ne olur!!!
                                return false;
                            }
                            break;
                        case "lne":
                            if (Surucu.Driver.PageSource.Replace("  ", " ").Contains("Yazmış olduğunuz")) { Message = msg = "Geçersiz kimlik numarası"; return false; }
                            if (Surucu.Driver.PageSource.Replace("  ", " ").Contains("HATA BİLDİRİM")) { Message = msg = "HATA BİLDİRİM"; return false; }
                            if (Surucu.Driver.PageSource.Replace("  ", " ").Contains("4a kaydı")) { Message = msg = "Sigortalının 4a kaydı bulunamamıştır"; return false; }
                            break;
                        case "lck":
                            hasLoaded = false; continueWaiting = 1;
                            while (!hasLoaded && continueWaiting < 4)
                            {
                                if (LinkGlobals.LinkCancel == true) { Message = "İşlem kullanıcı tarafından iptal edildi."; msg = "iptal"; return false; }
                                try
                                {
                                    if (IsPageLoad(line.Remove(0, 4), LinkGlobals.MaxWait))
                                    {
                                        hasLoaded = true; break;
                                    }
                                    continueWaiting++;
                                }
                                catch (WebDriverException)
                                {
                                    continueWaiting++;

                                }
                            }
                            if (continueWaiting >= 3)
                            {
                                msg = "yeniden";
                                Message = "Sayfa yüklenmesi çok uzun sürdüğü için işlem iptal edildi.";
                                return false;
                            }
                            break;
                    }
                    counter++;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                Message = $"Oturum açma işleminden sonra devam edilemiyor. Hata: {msg}";
                return false;
            }
            Message = msg;
            return true;
        }
        public bool IsPageLoad(string term, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if(Surucu.Driver.PageSource.Replace("  "," ").Contains(term))
                {
                    return true;
                }
                Thread.Sleep(100);
            }
            return false;
        }
        public void WaitForElement(string type, string element, out string msg)
        {
            msg = "";
            WebDriverWait wait = GetWait();

            try
            {
                // 1. Önce Hangi Seçiciyi (Locator) Kullanacağımızı Belirliyoruz
                By locator = null;
                switch (type)
                {
                    case "i": locator = By.Id(element); break;
                    case "x": locator = By.XPath(element); break;
                    case "n": locator = By.Name(element); break;
                }

                if (locator != null)
                {
                    // 2. Elementleri bul ve HEPSİNİN görünür olmasını bekle
                    wait.Until(d =>
                    {
                        var elements = d.FindElements(locator);

                        // Eğer en az 1 element bulunduysa VE bulunanların hepsi görünür durumdaysa listeyi döndür (işlemi onayla)
                        // Aksi takdirde (veya hiç element yoksa) null döndür ki süre dolana kadar denemeye devam etsin
                        return (elements.Count > 0 && elements.All(el => el.Displayed)) ? elements : null;
                    });
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }
        public void WaitForImageLoad(string element, out string msg, string method = "XPath")
        {
            msg = "";
            IWebElement image = null;
            try
            {
                IWebElement e = GetElementBy(method, element, out msg);
                bool imagePresent = (Boolean)((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("return arguments[0].complete && typeof arguments[0].naturalWidth != \"undefined\" && arguments[0].naturalWidth > 0", e);
                if(imagePresent == true)
                {
                    return;
                }

                else

                {
                    // You can increase or decrease the loop times based on tested application
                    for (int i = 0; i < 25; i++)
                    {
                        System.Threading.Thread.Sleep(1000);
                        image = Surucu.Driver.FindElement(By.XPath("xpath of container which is contained the image"));
                        imagePresent = (Boolean)((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("return arguments[0].complete && typeof arguments[0].naturalWidth != \"undefined\" && arguments[0].naturalWidth > 0", e);

                        if(imagePresent == true)
                        {
                            break;
                        }
                    }
                }
            }
            catch (NoSuchElementException) { }
        }
        public void JustMouseHover(string type, string tv, out string msg)
        {
            msg = "";
            WebDriverWait wait = GetWait();
            IWebElement menuItem = null;
            Actions action = new Actions(Surucu.Driver);

            try
            {
                // 1. Önce Hangi Seçiciyi (Locator) Kullanacağımızı Belirliyoruz
                By locator = null;
                switch (type)
                {
                    case "i": locator = By.Id(tv); break;
                    case "x": locator = By.XPath(tv); break;
                    case "n": locator = By.Name(tv); break;
                    case "c": locator = By.CssSelector(tv); break;
                    case "t": locator = By.LinkText(tv); break;
                }

                if (locator != null)
                {
                    // 2. Elementin hem sayfada var olmasını hem de görünür (Displayed) olmasını bekliyoruz
                    menuItem = wait.Until(d =>
                    {
                        var el = d.FindElement(locator);
                        return el.Displayed ? el : null;
                    });

                    action.MoveToElement(menuItem).Perform();
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }
        public void JustMouseHover(IWebElement element, out string msg)
        {
            msg = "";
            Actions action = new Actions(Surucu.Driver);
            try
            {
                action.MoveToElement(element).Perform();
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }
        public void KillDriver(out string msg)
        {
            msg = ""; 
            try
            {

                foreach (var process in Process.GetProcessesByName("geckodriver.exe"))
                {
                    process.Kill();
                }
            }
            catch (Exception)
            {
                msg = "geckodriver.exe bulunamadı";
            }
            
        }
        public void SetAttirbute()
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)Surucu.Driver;
                js.ExecuteScript("document.querySelector('body > table:nth-child(5) > tbody > tr > td > center > table > tbody > tr:nth-child(2)').innerHTML = document.querySelector('body > table:nth-child(5) > tbody > tr > td > center > table > tbody > tr:nth-child(2)').innerHTML.replace('/EBorcuYoktur5510/ebscr_files/keypad.js','');");
            }
            catch (Exception)
            {
                Console.WriteLine("SgkLinkService.cs 910");
            }
            
           
        }
        
    }
}