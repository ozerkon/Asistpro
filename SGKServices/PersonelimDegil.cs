using Models.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using WebDriverX;

namespace SGKServices
{
    public class PersonelimDegil : TrmBase
    {
        public List<Visit> MyEmployees { get; set; } = new List<Visit>();
        public List<Visit> NotMyEmployees { get; set; } = new List<Visit>();
        public List<DateTime> IseGirisCikis { get; set; } = new List<DateTime>();
        public List<IseGiriscikis> GirisCikisIslemleri { get; set; } = new List<IseGiriscikis>();
        public List<DateRange> CikisGiris { get; set; } = new List<DateRange>();
        public DateRange RaporAraligi { get; set; } = new DateRange();
        public string ErrorList { get; set; } = "";
        public List<string> Errors { get; set; }

        public bool DoWork(List<Visit> kontrolEdilecekler, out string msg)
        {
            msg = ""; ErrorList = "";
            Errors = new List<string>();
            try
            {
                foreach (Visit personel in kontrolEdilecekler)
                {
                    if (GlobalVars.CancelProcess) { return false; }
                    IseGirisCikis.Clear();
                    GirisCikisIslemleri.Clear();
                    CikisGiris.Clear();

                    if (personel.Cnm == SgkLoginCredentials.CompanyName)
                    {
                        if (IstenCikisTara(personel, out msg))
                        {
                            NotMyEmployees.Add(personel);
                        }
                        else
                        {
                            MyEmployees.Add(personel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            if (Errors.Count > 0)
            {
                Errors = Errors.Distinct().ToList();
                foreach (string err in Errors)
                    ErrorList += err;
            }
            return true;
        }

        private bool IstenCikisTara(Visit personel, out string msg) // true : işten çıkmış ve geri dönmemiş
        {
            msg = "";
            DateTime sonCikisTarihi = new DateTime();
            IWebElement linkAyrilis;
            try
            {
                var waitForLink = GetWait();
                string pageHtml = Surucu.Driver.PageSource;
                if (pageHtml.Contains("Sigortalı  İşe Giriş Kayıtları:")) // işe giriş kayıtları sayfasındaysak
                {
                    if (GlobalVars.CancelProcess) { msg = "iptal"; return false; }
                    IWebElement linkAnaMenu = waitForLink.Until(d => d.FindElement(By.LinkText("Ana Menü")));
                    linkAnaMenu.Click();
                }
                pageHtml = Surucu.Driver.PageSource;
                if (pageHtml.Contains("SİGORTALI İŞE GİRİŞ-AYRILIŞ BİLDİRGELERİ")) // eğer ana sayfadaysak
                {
                    if (GlobalVars.CancelProcess) { msg = "iptal"; return false; }
                    linkAyrilis = waitForLink.Until(d => d.FindElement(By.LinkText("İŞTEN AYRILIŞ GÖRÜNTÜLEME")));
                    linkAyrilis.Click();
                }

                sonCikisTarihi = ProcessCheckLeaveForTcno(personel, out msg); // iseGirisCikis listesine çıkış tarihlerini ekle ve son çıkış tarihini döndür
                if (sonCikisTarihi == new DateTime(2222, 2, 2)) { msg = "iptal"; return false; }
                if (sonCikisTarihi != DateTime.MinValue) // Eğer en az bir kez çıkış yapılmış ise
                {
                    if (!ProcessCheckHireForTcno(personel, out msg)) { msg = "iptal"; return false; } // iseGirisCikis listesine giriş tarihlerini ekle
                    if (GlobalVars.CancelProcess) { msg = "iptal"; return false; }
                    IseGirisCikis = IseGirisCikis.OrderBy(x => x.Date).ToList();
                    GirisCikisIslemleri = GirisCikisIslemleri.OrderBy(x => x.IslemTarihi.Date).ToList();
                    InOutOrder(GirisCikisIslemleri, personel.AdSoyad, out msg);
                    if (msg != "")
                    {
                        Errors.Add($"<li>{personel.AdSoyad} adlı personelin giriş çıkış kayıtları hatalı olduğu için \"Personelim Değil\" sorgulaması yapılamadı</li>"); return false;
                    }
                    int i = 0;
                    DateRange dr = new DateRange();
                    foreach (DateTime tarih in IseGirisCikis)
                    {
                        if (i % 2 == 0) // Çift numaralı tarihler işe giriş
                        {
                            dr = new DateRange { Sd = tarih };
                        }
                        else // Tek numaralı tarihler işten çıkış
                        {
                            dr.Ed = tarih;
                        }
                        if (i % 2 == 1 && i >= 1)
                        {
                            CikisGiris.Add(dr); /// dr adlı DateRange nesnesini, cikisGiris adlı DateRange listesine ekle
                        }
                        if (IseGirisCikis.Count % 2 == 1 && i == IseGirisCikis.Count - 1) // son çıkıştan sonra işe geri dönmemişse
                        {
                            dr.Ed = DateTime.MaxValue; // son işe giriş tarihini MaxValue yap.
                            CikisGiris.Add(dr); /// dr adlı DateRange nesnesini, cikisGiris adlı DateRange listesine ekle
                        }
                        i++;
                    }
                    RaporAraligi.Sd = personel.RaporBaslamaTarihi;
                    RaporAraligi.Ed = personel.RaporBitisTarihi;
                    //rapor tarih aralığı, çıkış giriş tarihleri arasında ise personelim değildir.
                    if (DateRangeCheck(personel.AdSoyad, RaporAraligi, CikisGiris, out msg)) return true;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return false;
        }

        private DateTime ProcessCheckLeaveForTcno(Visit personel, out string msg)
        {
            msg = "";
            DateTime cikisTarih = DateTime.MinValue;
            try
            {
                var wait = GetWait();
                string pageHtml = Surucu.Driver.PageSource;

                if (pageHtml.Contains("Sigortalı İşe Giriş Kayıtları:"))
                {
                    if (GlobalVars.CancelProcess) { msg = "iptal"; return new DateTime(2222, 2, 2); }
                    IWebElement linkAnaMenu = wait.Until(d => d.FindElement(By.LinkText("Ana Menü")));
                    linkAnaMenu.Click();
                }
                pageHtml = Surucu.Driver.PageSource;
                if (pageHtml.Contains("SİGORTALI İŞE GİRİŞ-AYRILIŞ BİLDİRGELERİ"))
                {
                    if (GlobalVars.CancelProcess) { msg = "iptal"; return new DateTime(2222, 2, 2); }
                    IWebElement linkAyrilis = wait.Until(d => d.FindElement(By.LinkText("İŞTEN AYRILIŞ GÖRÜNTÜLEME")));
                    linkAyrilis.Click();
                }
                if (GlobalVars.CancelProcess) { msg = "iptal"; return new DateTime(2222, 2, 2); }

                // ElementExists ve Displayed kontrolünü lambda ile yapıyoruz
                IWebElement tcno = wait.Until(d => {
                    var el = d.FindElement(By.Name("kimlikno"));
                    return el.Displayed ? el : null;
                });
                tcno.Clear();
                tcno.SendKeys(personel.Tcno);

                IWebElement sorgulaButton = Surucu.Driver.FindElement(By.Name("sorgulabtn"));
                wait.Until(d => sorgulaButton.Displayed);

                if (GlobalVars.CancelProcess) { msg = "iptal"; return new DateTime(2222, 2, 2); }
                sorgulaButton.Click();

                pageHtml = Surucu.Driver.PageSource;

                if (pageHtml.Contains("İsten Ayrilis Kayidi Bulunmamaktadir."))
                {
                    if (GlobalVars.CancelProcess) { msg = "iptal"; return new DateTime(2222, 2, 2); }
                    return DateTime.MinValue;
                }
                else if (pageHtml.Contains("Kayıtlar"))  //("Sigortalı İşten Ayrılış Kayıtları"))
                {
                    if (GlobalVars.CancelProcess) { msg = "iptal"; return new DateTime(2222, 2, 2); }
                    IWebElement form = Surucu.Driver.FindElement(By.Id("form1"));
                    var table = form.FindElement(By.TagName("table"));
                    var rows = table.FindElements(By.TagName("tr"));
                    int rowCount = rows.Count;
                    if (rows.Count > 8)
                    {
                        for (int i = 8; i < rowCount; i++)
                        {
                            if (GlobalVars.CancelProcess) { msg = "iptal"; return new DateTime(2222, 2, 2); }
                            var row = rows[i];
                            var rowTds2 = row.FindElements(By.TagName("td"));
                            string sIstenCikisTarihi = rowTds2[1].Text.Trim();
                            cikisTarih = DateTime.ParseExact(sIstenCikisTarihi, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            IseGirisCikis.Add(cikisTarih);
                            GirisCikisIslemleri.Add(new IseGiriscikis() { IslemTarihi = cikisTarih, IslemTuru = 'c' });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return cikisTarih;
        }

        private bool ProcessCheckHireForTcno(Visit personel, out string msg) // false: ise işten çıkmış ve geri dönmemiş
        {
            msg = "";
            DateTime girisTarih = DateTime.MinValue;
            try
            {
                if (GlobalVars.CancelProcess) { return false; }
                IWebElement linkAnaMenu = Surucu.Driver.FindElement(By.LinkText("Ana Menü"));
                linkAnaMenu.Click();
                var wait = GetWait();

                if (GlobalVars.CancelProcess) { return false; }
                IWebElement linkIseGiris = wait.Until(d => d.FindElement(By.LinkText("İŞE GİRİŞ GÖRÜNTÜLEME")));
                linkIseGiris.Click();

                wait.Until(d => d.FindElement(By.Name("kimlikno")).Displayed);
                IWebElement tcno = Surucu.Driver.FindElement(By.Name("kimlikno"));
                tcno.Clear();
                tcno.SendKeys(personel.Tcno);

                IWebElement sorgulaButton = Surucu.Driver.FindElement(By.Name("sorgulabtn"));
                wait.Until(d => sorgulaButton.Displayed);
                if (GlobalVars.CancelProcess) { return false; }
                sorgulaButton.Click();

                IWebElement form = Surucu.Driver.FindElement(By.Id("form1"));
                var table = form.FindElement(By.TagName("table"));
                var rows = table.FindElements(By.TagName("tr"));
                int rowCount = rows.Count;

                // XPath ile element görünürlüğü yerine By tabanlı görünürlük kontrolü lambda ile çözüldü
                wait.Until(d => d.FindElement(By.XPath("/html/body/table[3]/tbody/tr/td/center/table/tbody/tr[2]/td[2]/form/span/table/tbody/tr[6]/td[5]")).Displayed);

                for (int i = 5; i < rowCount; i++)
                {
                    if (GlobalVars.CancelProcess) { return false; }
                    var rowTds2 = rows[i].FindElements(By.TagName("td"));
                    string sIseGirisTarihi = rowTds2[4].Text.Trim();
                    girisTarih = DateTime.ParseExact(sIseGirisTarihi, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    IseGirisCikis.Add(girisTarih);
                    GirisCikisIslemleri.Add(new IseGiriscikis() { IslemTarihi = girisTarih, IslemTuru = 'g' });
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return true;
        }

        public bool DateRangeCheck(string ads, DateRange rapor, List<DateRange> lstCikisGiris, out string msg)
        {
            msg = ""; bool result = true;
            try
            {
                foreach (DateRange gc in lstCikisGiris)
                {
                    if (rapor.Sd >= gc.Sd && rapor.Sd <= gc.Ed && rapor.Ed >= gc.Sd && rapor.Ed <= gc.Ed)
                    {
                        result = false; break; // Raporlu olduğu süre boyunca personelim
                    }
                    else if (rapor.Sd < gc.Sd && rapor.Ed >= gc.Sd)
                    {
                        Errors.Add($"<li>{ads} adlı personel, {rapor.Sd:dd.MM.yyyy} tarihinde {(rapor.Ed - rapor.Sd).TotalDays + 1} günlük rapor almış ve {gc.Sd:dd.MM.yyyy} tarihindeki işe geri dönmüştür. <br>Raporun {rapor.Sd:dd.MM.yyyy}-{gc.Sd.AddDays(-1):dd.MM.yyyy} aralığında \"Firma Personeli Değildir\" geri kalan {gc.Sd:dd.MM.yyyy}-{rapor.Ed:dd.MM.yyyy} aralığı ise onaylanabilir</li>");
                        result = true; break; // Rapor başlangıcında personel değil, rapor bitmeden işe geri dönmüş
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return result;
        }

        public void InOutOrder(List<IseGiriscikis> lst, string ads, out string msg)
        {
            msg = "";
            char expectedTur = 'g'; // İlk işlem girişle başlamalı

            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].IslemTuru != expectedTur)
                {
                    if (expectedTur == 'g') // gelen değer c, sonrakinin g olması gerek
                    {
                        Errors.Add($"<li>{ads} adlı personelin {lst[i].IslemTarihi:dd.MM.yyyy} tarihinde girişi olmadan çıkışı yapılmış</li>"); expectedTur = 'g'; msg = "hata"; continue;
                    }
                    else // gelen değer g, sonrakinin c olması gerek 
                    {
                        Errors.Add($"<li>{ads} adlı personelin {lst[i].IslemTarihi:dd.MM.yyyy} tarihinde çıkışı olmadan girişi yapılmış</li>"); expectedTur = 'c'; msg = "hata"; continue;
                    }
                }

                // İşlemin türünü güncelle
                expectedTur = (expectedTur == 'g') ? 'c' : 'g';
            }
        }
    }
}