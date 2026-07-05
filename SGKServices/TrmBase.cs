using Models.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using WebDriverX;

namespace SGKServices
{
    public class TrmBase 
    {
        public int tryCount { get; set; } = 1;
        protected Captcha.ICaptchaSolver CpiSolver ;
        protected bool AutoCaptcha = false;
        protected Company SgkLoginCredentials = new Company();
        protected WebDriverWait Wait = null;
        protected string CaptchaFilePath = @"C:\Windows\Temp\SGKAsistanCaptcha\";
        public void SetSgkLoginCredentials(Company s)
        {
            this.SgkLoginCredentials = s;
        }
        protected bool TryCreateFolder(string folderToCreate, bool clearExistingFiles)
        {
            int tryCount = 0;
            bool folderCreated = false;
            bool folderIsEmpty = false;
            while (tryCount++ < 10)
            {
                if (System.IO.Directory.Exists(folderToCreate) == false)
                {
                    System.IO.Directory.CreateDirectory(folderToCreate);
                    folderCreated = true; // Set flag when folder is created
                }
                else
                {
                    folderCreated = true;
                    break;
                }

            }
            tryCount = 0;
            if (folderCreated == false)
            {
                return false;
            }
            if (clearExistingFiles)
            {
                folderIsEmpty = false;
                while (tryCount++ < 10)
                {
                    int fileCount = 0;
                    System.IO.DirectoryInfo di = new DirectoryInfo(folderToCreate);
                    foreach (FileInfo file in di.EnumerateFiles())
                    {
                        file.Delete();
                        fileCount++;
                    }
                    if (fileCount == 0)
                    {
                        folderIsEmpty = true;
                        break;
                    }
                }
                return folderCreated && folderIsEmpty;
            }

            return folderCreated;
        }
        public void WaitForPageLoaded(out string msg)
        {
            msg = "";
            try
            {
                if (Surucu.Driver == null)
                {
                    msg = "Driver not initialized";
                    return;
                }
                Surucu.Driver.SwitchTo().Alert().Dismiss();
            }
            catch (NoAlertPresentException)
            {
                // handle this exception, or just ignore it
            }
            try
            {
                if (Surucu.Driver == null)
                {
                    msg = "Driver not initialized";
                    return;
                }
                WebDriverWait wait = GetWait();
                wait.Until(e => ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("return document.readyState").Equals("complete"));
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }
        public IWebElement IsElementEXISTS(By locator, uint timeoutInSeconds = 20)
        {
            try
            {
                if (Surucu.Driver == null)
                {
                    return null;
                }
                WebDriverWait wait = new WebDriverWait(Surucu.Driver, TimeSpan.FromSeconds(timeoutInSeconds));

                // Eski SeleniumExtras satırı silindi, yerine Selenium 4'ün yerleşik yapısı eklendi:
                return wait.Until(d => d.FindElement(locator));
            }
            catch (Exception)
            {
                return null;
            }
        }
        public void RemoveTempFiles(string folderToClear, out string msg)
        {
            msg = "";
            int tryCount = 0;
            try
            {
                while (tryCount++ < 10)
                {
                    int fileCount = 0;
                    System.IO.DirectoryInfo di = new DirectoryInfo(folderToClear);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                        fileCount++;
                    }
                    if (fileCount == 0)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
            
        }
        public void SetAutoCaptcha(bool cstatus)
        {
            this.AutoCaptcha = cstatus;
        }
        public void GotoUrl(string url, out string msg)
        {
            msg = "";
            try
            {
                Surucu.Driver.Url = url;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }

        }
        public bool ExecuteScript(string t, string tv)
        {
            Wait = GetWait();
            string selector = "";
            string command = "";

            // JS Kodlarını Promise kullanmadan sadece boolean (true/false) dönecek şekilde basitleştiriyoruz.
            switch (t)
            {
                case "x": // XPath
                    selector = tv.Replace("'", "\\'");
                    command = $"return document.evaluate('{selector}', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue != null;";
                    break;

                case "i": // ID
                    selector = $"#{tv}";
                    command = $"return document.querySelector('{selector}') != null;";
                    break;
                case "c":
                    selector = tv; // Gelen CSS seçiciyi olduğu gibi al
                    command = $"return document.querySelector('{selector}') != null;";
                    break;

                case "cl": // Class
                    selector = $".{tv}";
                    command = $"return document.querySelector('{selector}') != null;";
                    break;

                case "tn": // Tag Name
                    selector = tv;
                    command = $"return document.querySelector('{selector}') != null;";
                    break;

                case "n": // Name attribute
                    selector = tv;
                    command = $"return document.querySelector('[name=\"{selector}\"]') != null;";
                    break;

                case "t": // Text
                    selector = tv.Replace("'", "\\'");
                    // Array.from ile linkleri diziye çevirip '.some' ile text'i içeren var mı kontrol ediyoruz. (true/false döner)
                    command = $"return Array.from(document.querySelectorAll('a')).some(link => link.textContent.includes('{selector}'));";
                    break;
            }

            bool result = false;
            int tryCount = 0; // Metoda özel değişken olarak tanımlanması daha güvenlidir

            while (result == false && tryCount < 5)
            {
                if (GlobalVars.CancelProcess || LinkGlobals.LinkCancel) { return false; }

                try
                {
                    // ExecuteScript artık JavaScript'ten doğrudan boolean (true/false) değer döndürecek.
                    result = Wait.Until(e => (bool)((IJavaScriptExecutor)Surucu.Driver).ExecuteScript(command));
                }
                catch (OpenQA.Selenium.WebDriverTimeoutException)
                {
                    // Wait.Until zaman aşımına uğrarsa (element bulunamazsa) Timeout fırlatır.
                    // Sadece bu durumda sayfayı yenileyip tekrar denemeliyiz.
                    Surucu.Driver.Navigate().Refresh();
                    string msg;
                    WaitForPageLoaded(out msg);
                }
                catch (Exception)
                {
                    // Timeout dışında bir hata alırsak (örneğin geçersiz JS syntax) döngüyü kır.
                    // Aksi takdirde sonsuz sayfa yenileme döngüsüne girer.
                    break;
                }

                tryCount++;
            }

            return result;
        }

        //public bool ExecuteScript(string t, string tv)
        //{
        //    Wait = GetWait();
        //    string selector = "";
        //    string command =
        //        $"function waitForElm(selector) {{" +
        //            $"return new Promise(resolve => {{" +
        //                $"if (document.querySelector(selector)){{" +
        //                    $"return resolve(document.querySelector(selector));" +
        //                $"}}" +
        //            $"}});" +
        //        $"}}";
        //    switch (t)
        //    {
        //        case "x":
        //            selector = tv.Replace("'", "\\'");
        //            command = $"function checkIfElemExists(selector) {{" +
        //                            $"return new Promise(resolve => {{" +
        //                                $"var clickButton = document.evaluate (selector, document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;" +
        //                                    $"if (clickButton == null) {{ window.requestAnimationFrame(checkIfElemExists); }}" +
        //                                    $"else {{ return resolve(clickButton); }}" +
        //                            $"}})" +
        //                       $"}}" +
        //                       $"return checkIfElemExists('{selector}').then((elm) => {{return 'ready'}});";
        //            break;
        //        case "i":
        //            selector = $"#{tv}";
        //            command += $"return waitForElm('{selector}').then((elm) => {{return 'ready'}});";
        //            break;
        //        case "cl":
        //            selector = $".{tv}";
        //            command += $"return waitForElm('{selector}').then((elm) => {{return 'ready'}});";
        //            break;
        //        case "tn":
        //            selector = tv;
        //            command += $"return waitForElm('{selector}').then((elm) => {{return 'ready'}});";
        //            break;
        //        case "n":
        //            selector = tv;
        //            command += $"return waitForElm('[name=\"{selector}\"]').then((elm) => {{return 'ready'}});";
        //            break;
        //    }
        //    bool result = false; 
        //    while (result == false)
        //    {
        //        try
        //        {  result = Wait.Until(e => ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript(command).Equals("ready")); }
        //        catch { Surucu.Driver.Navigate().Refresh();  string msg; WaitForPageLoaded(out msg); }  
        //        tryCount++;
        //        if (tryCount >= 5)
        //            break;
        //    }
        //    tryCount = 1;  return result;
        //}
        public WebDriverWait GetWait()
        {
            WebDriverWait wait;
            try
            {
                wait = new WebDriverWait(Surucu.Driver, TimeSpan.FromSeconds(LinkGlobals.MaxWait));
            }
            catch
            {
                wait = null;
            }
            return wait;
        }
    }
}
