using Models.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using WebDriverX;

namespace SgkAssistant.Forms.Common
{
    public partial class frmEkap : Telerik.WinControls.UI.RadForm
    {
        BackgroundWorker bgw;
        string msg = "";
        List<ihale> ihales = new List<ihale>();
        public frmEkap()
        {
            InitializeComponent();
            bgw = new BackgroundWorker();
            InitializeBgw();
        }
        private void InitializeBgw()
        {
            bgw.WorkerSupportsCancellation = true;
            bgw.DoWork += new DoWorkEventHandler(BgwDoWork);
            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BgwComplated);
        }

        private void BgwComplated(object sender, RunWorkerCompletedEventArgs e)
        {
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
                    lblMessage.Text = $"İşlem iptal edildi!" ;
                }));
                GlobalVars.CancelProcess = false;
            }
            else { lblMessage.Text = $"İşlem bitti"; }
            bgw.Dispose();
        }

        private void BgwDoWork(object sender, DoWorkEventArgs e)
        {
            ihales = IOC.ihaleDataServis.IhaleAl(out msg);
               
            if (Surucu.Driver == null ) { Surucu.Driver = IOC.WinHelpers.GetWebDriver(false, out msg); }
            WebDriverWait wait = new WebDriverWait(Surucu.Driver, TimeSpan.FromSeconds(LinkGlobals.MaxWait));
            Surucu.Driver.Url = "https://ekap.kik.gov.tr/EKAP/Ortak/IhaleArama/index.html";
            while (!Surucu.Driver.PageSource.Contains("İhale içinde geçen ifade"))
            {
                Thread.Sleep(500);
                Surucu.Driver.Navigate().Refresh();
                IOC.SgkLinksService.WaitForPageLoaded(out msg);
            }
            IWebElement searchBox = IOC.SgkLinksService.GetElementBy("x", "//*[@id=\"app\"]/div[1]/div/div/div[4]/div[2]/input", out msg);
            searchBox.SendKeys("mama");
            IWebElement ihaleDurumu = IOC.SgkLinksService.GetElementBy("x", "/html/body/div/div[1]/div/div/div[4]/div[4]/div[4]/select", out msg);
            SelectElement id = new SelectElement(ihaleDurumu);
            id.SelectByIndex(6);
            IWebElement idareninIli = IOC.SgkLinksService.GetElementBy("x", "/html/body/div/div[1]/div/div/div[4]/div[4]/div[5]/div/div/select", out msg);
            IWebElement enUstIdareler = IOC.SgkLinksService.GetElementBy("x", "/html/body/div/div[1]/div/div/div[4]/div[4]/div[8]/select", out msg);
            id = new SelectElement(enUstIdareler);
            id.SelectByIndex(8);

            IWebElement btnFiltrele = IOC.SgkLinksService.GetButtonElementBy("x", "/html/body/div/div[1]/div/div/div[4]/div[3]/button/span[2]", out msg);
            btnFiltrele.Click();
            IOC.SgkLinksService.WaitForPageLoaded(out msg);
            List<string> divTexts = new List<string>();
            List<IWebElement> ihaleler = new List<IWebElement>();
            
            bool hasMoreDivs = true;
            int i = 1;
            while (hasMoreDivs)
            {
                IWebElement div = Surucu.Driver.FindElement(By.XPath($"/html/body/div/div[2]/div/div[2]/div[{i++}]"));
                bool varMi = false;
                varMi = false;
                IWebElement ihaleKayitNo = div.FindElement(By.TagName("h6"));
                foreach (ihale item in ihales) { 
                    if (item.ihaleKayitNo == ihaleKayitNo.Text.Trim()) { varMi = true;   
                        ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("window.scrollBy(0, 96);");
                        Thread.Sleep(1000); break;

                    }
                }
                if (varMi) continue;
                //IWebElement icon = div.FindElement(By.ClassName("pr-2"));
                //string iconSinif = icon.GetAttribute("class");
                
                //if (iconSinif.Contains("shopping"))
                BilgileriAl(div);
                ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("window.scrollBy(0, 96);");
                Thread.Sleep(2000); 
            }
        }

        private void BilgileriAl(IWebElement element)
        {
            ihale ihale = new ihale();
            IWebElement btnBilgiler = element.FindElement(By.ClassName("btn-outline-info"));
            btnBilgiler.Click();
            Thread.Sleep(2000);
            IWebElement bntKapat = element.FindElement(By.ClassName("close"));
            IWebElement iFrame = element.FindElement(By.TagName("iframe"));
            Surucu.Driver.SwitchTo().Frame(iFrame);
            Thread.Sleep(2000);
            IWebElement btnIdareBilgileri = GetWebElement( "/html/body/form/div[4]/div/div/nav/ul/li[2]/a");
            IWebElement btnsozlesmeBilgileri = GetWebElement("/html/body/form/div[4]/div/div/nav/ul/li[5]/a");

            ihale.ihaleKayitNo = BilgiAl("i","ucBirBakistaIhale_lblIKNo");
            ihale.ihaleAdi = BilgiAl("i","ucBirBakistaIhale_lblIhaleAdi");
            ihale.ihaleTuru = BilgiAl("i","ucBirBakistaIhale_lblIhaleTuruUsulu");
            ihale.OnayTarihi = BilgiAl("i","ucBirBakistaIhale_lblIhaleOnayTarihi");
            btnIdareBilgileri.Click();
            Thread.Sleep(2000);
            ihale.idareAdi = BilgiAl("i","ucBirBakistaIhale_lblIdareAdi");
            ihale.il = BilgiAl("i","ucBirBakistaIhale_lblIdareIli");
            btnsozlesmeBilgileri.Click();
            Thread.Sleep(2000);
            ihale.sozlesmeBilgileri = BilgiAl("x","/html/body/form/div[4]/div/div/div/section[5]/div/div/div[1]/div/div[2]/div/div[2]/p/span[2]").Trim().Replace(" TRY","") ;
            ihale.yaklasikMaliyet =  BilgiAl("x","/html/body/form/div[4]/div/div/div/section[5]/div/div/div[1]/div/div[2]/div/div[3]/p/span[2]").Replace(" TRY","") ;
            ihale.enYuksekTeklif =  BilgiAl("x","/html/body/form/div[4]/div/div/div/section[5]/div/div/div[1]/div/div[2]/div/div[6]/p/span[2]").Replace(" TRY","") ;
            ihale.enDusukTeklif =  BilgiAl("x","/html/body/form/div[4]/div/div/div/section[5]/div/div/div[1]/div/div[2]/div/div[7]/p/span[2]").Replace(" TRY","") ;
            ihale.SozlesmeTarihi = BilgiAl("x","/html/body/form/div[4]/div/div/div/section[5]/div/div/div[1]/div/div[2]/div/div[5]/p/span[2]");
            ihale.ihaleyiAlan = BilgiAl("x","/html/body/form/div[4]/div/div/div/section[5]/div/div/div[1]/div/div[1]");
            IOC.ihaleDataServis.Insert(ihale, out msg);
            bntKapat.Click();
            Surucu.Driver.SwitchTo().ParentFrame();
        }
        private IWebElement GetWebElement( string path)
        {
            IWebElement element = null;
            while (element == null)
            {
                try
                {
                    element = Surucu.Driver.FindElement(By.XPath(path));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    element = null;
                }
                
            }
            return element;
        }
        private string BilgiAl(string type, string path)
        {
            string bilgi = "";
            while (bilgi == "")
            {
                switch (type)
                {
                    case "i":
                        try
                        {
                            bilgi = Surucu.Driver.FindElement(By.Id(path)).Text.Trim();
                        }
                        catch (Exception)
                        {
                            path = (path == "ucBirBakistaIhale_lblIdareAdi") ? "ucBirBakistaIhale_lblIdareAdiHiyerarsik" : path;
                        }
                        
                        break;
                    case "x":
                        bilgi = Surucu.Driver.FindElement(By.XPath(path)).Text.Trim();
                        break;
                }
            }
            return bilgi;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!bgw.IsBusy)
            {
                bgw.RunWorkerAsync();
            }
        }
    }
}
