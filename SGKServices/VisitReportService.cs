using Models.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using WebDriverX;

namespace SGKServices
{
    public class VisitReportService : TrmBase
    {
        public IWebElement LinkOfReport;
        public string XPathOfLink;
        private List<Visit> tempVisit = new List<Visit>();
        public List<Visit> Visits = new List<Visit>();
        public List<ConfirmReport> ConfirmReports = new List<ConfirmReport>();
        public List<ProcessReport> ProcessReports = new List<ProcessReport>();
        public string Message { get; set; } = "";
        public string RgvTitleText = "";
        public bool HasPregnancyCheckFinish { get; set; } = false;
        CommonFuncs commonFuncs = new CommonFuncs();

        #region raportaramaislemleri
        public (List<Visit>, string , string ) StartSearch(out string msg)
        {
            msg = ""; 
            List<Visit> v = new List<Visit>();
            string lblMessage = String.Empty; 
            string rgvTitleText = String.Empty;
            try
            {
                switch (SearchReport.ReportType)
                {
                    case 1:

                        (v, lblMessage, rgvTitleText) = KimlikNoyaGoreArama(out msg);
                        break;
                    case 2:
                        (v, lblMessage, rgvTitleText) = TariheGoreArama(out msg);
                        break;
                    case 3:
                        (v, lblMessage, rgvTitleText) = OnayliRaporlar(out msg);
                        break;
                    case 4:
                        (v, lblMessage, rgvTitleText) = ArsivdekiRaporlar(out msg);
                        break;
                }
                return (v, lblMessage, rgvTitleText);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return (null, msg, string.Empty);
            }
            
        }
        private (List<Visit>, string, string) KimlikNoyaGoreArama(out string msg)
        {
            msg = ""; commonFuncs.SetDriver(Surucu.Driver);
            XPathOfLink = "/html/body/table[2]/tbody/tr/td[1]/table[5]/tbody/tr[2]/td/table/tbody/tr/td[2]/a";
            try
            {
                if(GlobalVars.CancelProcess == true) { return (null, "iptal", null); }
                IWebElement btnSearch = IsElementEXISTS(By.XPath("/html/body/table[2]/tbody/tr/td[2]/form/table/tbody/tr[4]/td/input"));
                commonFuncs.ClearTextBox("mernisNo", out msg);
                commonFuncs.FillTextBox("mernisNo", SearchReport.KimlikNo, out msg);
                commonFuncs.ChangeComboBox(SearchReport.CaseType, "rapor_turu", out msg);
                if (GlobalVars.CancelProcess == true) { return (null, "iptal", null); }
                commonFuncs.ClickWebElement("/html/body/table[2]/tbody/tr/td[2]/form/table/tbody/tr[4]/td/input", out msg);

                if (SearchReport.IsMultiSearch == true)
                {
                    string ipt = "";
                    (tempVisit, ipt) = MultiSearch(XPathOfLink, SearchReport.ReportType, out msg);
                    if(ipt == "iptal") { return (null, "iptal", null); }
                    else if (tempVisit != null)
                    {
                        RgvTitleText = $"{SearchReport.KimlikNo} kimlik numaralı personelin onaylanmamış raporları ";
                        Visits = Visits.Concat(tempVisit).ToList();
                    }
                    else
                    {
                        RgvTitleText = "Kayıt bulunamadı";
                    }
                }
                else
                {
                    commonFuncs.WaitForPageLoad(out msg);
                    if (!commonFuncs.IsPageContains("Kayit Bulunamadi!", out msg))
                    {
                        if (GlobalVars.CancelProcess == true) { return (null, "iptal", null); }
                        RgvTitleText = $"{SearchReport.KimlikNo} kimlik numaralı personelin onaylanmamış raporları ";
                        tempVisit = FillRadGridView("/html/body/table[2]/tbody/tr/td[2]/form/table[2]/tbody/tr/td/input[1]", "/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr", "/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[2]/td", out msg);
                        Visits = Visits.Concat(tempVisit).ToList();

                    }
                    else
                    {
                        RgvTitleText = "Kayıt Bulunamadı!";
                    }
                }
                if (SearchReport.ProcessType == 1) { return (Visits, Message, RgvTitleText); } else { return (null, null, null); }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return (null, string.Empty, msg);
            }
        }
        private (List<Visit>, string, string) TariheGoreArama(out string msg)
        {
            msg = ""; commonFuncs.SetDriver(Surucu.Driver);
            XPathOfLink = "/html/body/table[2]/tbody/tr/td[1]/table[5]/tbody/tr[3]/td/table/tbody/tr/td[2]/a";
            try
            {
                if (GlobalVars.CancelProcess == true) { return (null, "iptal", null); }
                if (SearchReport.SearchDate > DateTime.Now)
                {
                    Message = "Tarih bugünden büyük olamaz";
                }
                else
                {
                    commonFuncs.ClearTextBox("tarih", out msg);
                    commonFuncs.ChangeDateTimePicker(SearchReport.SearchDate, "tarih", out msg);
                    commonFuncs.ChangeComboBox(SearchReport.CaseType, "vaka", out msg);
                    if (GlobalVars.CancelProcess == true) { return (null, "iptal", null); }
                    commonFuncs.ClickWebElement("/html/body/table[2]/tbody/tr/td[2]/form/table/tbody/tr[4]/td/input", out msg);

                    if (SearchReport.IsMultiSearch == true)
                    {
                        if (GlobalVars.CancelProcess == true) { return (null, "iptal", null); }
                        string ipt = "";
                        (tempVisit, ipt) = MultiSearch(XPathOfLink, SearchReport.ReportType, out msg);
                        if (ipt == "iptal") { return (null, "iptal", null); }
                        else if (tempVisit != null)
                        {
                            RgvTitleText = $"{SearchReport.SearchDate.ToString("dd.MM.yyyy")} itibariyle onaylanmamış raporlar";
                            Visits = Visits.Concat(tempVisit).ToList();
                        }
                        else
                        {
                            RgvTitleText = $"{SearchReport.SearchDate.ToString("dd.MM.yyyy")} itibariyle onaylanmamış rapor yoktur ";
                        }
                    }
                    else
                    {
                        commonFuncs.WaitForPageLoad(out msg);
                        if (!commonFuncs.IsPageContains("Kayit Bulunamadi!", out msg))
                        {
                            if (GlobalVars.CancelProcess == true) { return (null, "iptal", null); }
                            RgvTitleText = $"{SearchReport.SearchDate.ToString("dd.MM.yyyy")} itibariyle onaylanmamış raporlar";
                            tempVisit = FillRadGridView("/html/body/table[2]/tbody/tr/td[2]/form/table[2]/tbody/tr/td/input[1]", "/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr", "/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[2]/td", out msg);
                            Visits = Visits.Concat(tempVisit).ToList();
                        }
                        else
                        {
                            if (GlobalVars.CancelProcess == true) { return (null, "iptal", null); }
                            RgvTitleText = $"{SearchReport.SearchDate.ToString("dd.MM.yyyy")} itibariyle onaylanmamış rapor yoktur ";
                        }
                    }

                }
                if (SearchReport.ProcessType == 1) { return (Visits, Message, RgvTitleText); } else { return (null, null, null); }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return (null, string.Empty, msg);
            }
        }
        private (List<Visit>, string, string) OnayliRaporlar(out string msg)
        {
            msg = ""; commonFuncs.SetDriver(Surucu.Driver);
            XPathOfLink = "/html/body/table[2]/tbody/tr/td[1]/table[5]/tbody/tr[5]/td/table/tbody/tr/td[2]/a";
            try
            {
                if (!Surucu.Driver.PageSource.Contains("Tarih Aralığını Giriniz")) commonFuncs.ClickWebElement(XPathOfLink, out msg);
                if (GlobalVars.CancelProcess == true) { return (null, "iptal", null); }
                if (SearchReport.EndDate >= SearchReport.StartDate)
                {
                    commonFuncs.ClearTextBox("tarih1", out msg);
                    commonFuncs.ClearTextBox("tarih2", out msg);
                    commonFuncs.ChangeDateTimePicker(SearchReport.StartDate, "tarih1", out msg);
                    commonFuncs.ChangeDateTimePicker(SearchReport.EndDate, "tarih2", out msg);
                    if (GlobalVars.CancelProcess == true) { return (null, "iptal", null); }
                    commonFuncs.ClickWebElement("/html/body/table[2]/tbody/tr/td[2]/form/table/tbody/tr[3]/td/input", out msg);
                    commonFuncs.WaitForPageLoad(out msg);
                    if (!commonFuncs.IsPageContains("Kayit Bulunamadi!", out msg))
                    {
                        if (GlobalVars.CancelProcess == true) { return (null, "iptal", null); }
                        RgvTitleText = $"{SearchReport.StartDate.ToString("dd.MM.yyyy")} - {SearchReport.EndDate.ToString("dd.MM.yyyy")} tarihleri arasındaki onaylanmış raporlar";
                        tempVisit = FillRadGridView("/html/body/table[2]/tbody/tr/td[2]/form/table[2]/tbody/tr/td/input[1]", "/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr", "/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[2]/td", out msg);
                        Visits = Visits.Concat(tempVisit).ToList();
                    }
                    else
                    {
                        if (GlobalVars.CancelProcess == true) { return (null, "iptal", null); }
                        RgvTitleText = $"{SearchReport.StartDate.ToString("dd.MM.yyyy")} - {SearchReport.EndDate.ToString("dd.MM.yyyy")} tarihleri arasında onaylanmış rapor yoktur ";
                    }
                }
                else
                {
                    Message = "Bitiş tarihi,başlangıç tarihinden daha eski olamaz!";
                }
                if (SearchReport.ProcessType == 1) { return (Visits, Message, RgvTitleText); } else { return (null, null, null); }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return (null, string.Empty, msg);
            }
        }
        private (List<Visit>, string, string) ArsivdekiRaporlar(out string msg)
        {
            msg = ""; commonFuncs.SetDriver(Surucu.Driver);
            try
            {
                XPathOfLink = "/html/body/table[2]/tbody/tr/td[1]/table[5]/tbody/tr[6]/td/table/tbody/tr/td[2]/a";
                if(!Surucu.Driver.PageSource.Contains("Tarih Aralığını Giriniz")) commonFuncs.ClickWebElement(XPathOfLink, out msg);
                if (GlobalVars.CancelProcess == true) { return (null, "iptal", null); }
                if (SearchReport.EndDate >= SearchReport.StartDate)
                {
                    commonFuncs.ClearTextBox("tarih1", out msg);
                    commonFuncs.ClearTextBox("tarih2", out msg);
                    commonFuncs.ChangeDateTimePicker(SearchReport.StartDate, "tarih1", out msg);
                    commonFuncs.ChangeDateTimePicker(SearchReport.EndDate, "tarih2", out msg);
                    if (GlobalVars.CancelProcess == true) { return (null, "iptal", null); }
                    commonFuncs.ClickWebElement("/html/body/table[2]/tbody/tr/td[2]/form/table/tbody/tr[3]/td/input", out msg);
                    commonFuncs.WaitForPageLoad(out msg);
                    if (!commonFuncs.IsPageContains("Arsivde Rapor Bulunamadi!", out msg))
                    {
                        if (GlobalVars.CancelProcess == true) { return (null, "iptal", null); }
                        RgvTitleText = $"Arşivinizdeki {SearchReport.StartDate.ToString("dd.MM.yyyy")} - {SearchReport.EndDate.ToString("dd.MM.yyyy")} tarihleri arasındaki raporlar";
                        tempVisit = FillRadGridView("/html/body/table[2]/tbody/tr/td[2]/form/center/input", "/html/body/table[2]/tbody/tr/td[2]/form/center/table/tbody/tr", "/html/body/table[2]/tbody/tr/td[2]/form/center/table/tbody/tr[2]/td", out msg);
                        Visits = Visits.Concat(tempVisit).ToList();
                    }
                    else
                    {
                        if (GlobalVars.CancelProcess == true) { return (null, "iptal", null); }
                        RgvTitleText = $"Arşivinizde {SearchReport.StartDate.ToString("dd.MM.yyyy")} - {SearchReport.EndDate.ToString("dd.MM.yyyy")} tarihleri arasında rapor yoktur ";
                    }
                }
                else
                {
                    Message = "Bitiş tarihi,başlangıç tarihinden daha eski olamaz!";
                }
                if (SearchReport.ProcessType == 1) { return (Visits, Message, RgvTitleText); } else { return (null, null, null); }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return (null, string.Empty, msg);
            }
        }
        public List<Visit> PregnancyCheck(List<Visit> lst, out string msg)
        {

            msg = "";
            try
            {
                foreach (Visit v in lst)
                {
                    if (GlobalVars.CancelProcess == true) { msg = "iptal"; return (null); }
                    if (v.Vaka == "ANALIK" && v.Cnm == SgkLoginCredentials.CompanyName)
                    {
                       (v.RaporBaslamaTarihi, v.RaporBitisTarihi, v.Aciklama) = GetReportDetails(v, out msg);
                        if (v.RaporBaslamaTarihi == DateTime.MinValue && v.RaporBitisTarihi == DateTime.MinValue && v.Aciklama == "iptal") { msg = "iptal";  return (null); }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
            HasPregnancyCheckFinish = true;
            return lst;
        }
        #endregion

        #region tablookumaislemleri

        private void FillVisitClassList(List<Visit> lst, List<IWebElement> rows, int rowCount, int columnCount, out string msg)
        {
            msg = "";
            Visit v = new Visit();
            try
            {
                for (int i = 3; i <= rowCount; i++)
                {
                    v = new Visit();
                    if (columnCount == 9)
                    {
                        v.Tcno = rows[i - 1].FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[{i}]/td[2]")).Text;
                        v.AdSoyad = rows[i - 1].FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[{i}]/td[3]")).Text;
                        v.Vaka = rows[i - 1].FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[{i}]/td[4]")).Text;
                        v.RaporTakipNo = rows[i - 1].FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[{i}]/td[5]")).Text;
                        v.RaporSiraNo = Convert.ToInt32(rows[i - 1].FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[{i}]/td[6]")).Text);
                        v.RaporBaslamaTarihi = DateTime.Parse(rows[i - 1].FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[{i}]/td[7]")).Text);
                        try
                        {
                            v.RaporBitisTarihi = DateTime.Parse(rows[i - 1].FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[{i}]/td[8]")).Text).AddDays(-1);
                        }
                        catch (Exception)
                        {

                            v.RaporBitisTarihi = DateTime.Parse(DateTime.Today.ToString());
                        }

                        v.IsBasiKontrolTarihi = DateTime.Parse(rows[i - 1].FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[{i}]/td[8]")).Text);
                        v.CezaDurumu = rows[i - 1].FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[{i}]/td[9]")).Text;
                        if (rows[i-1].GetCssValue("background-color") == "rgb(245, 169, 169)" || rows[i - 1].GetAttribute("style") == "background-color: rgb(245, 169, 169);") 
                        {
                            v.Aciklama = "Ödemesi Çıkmış";
                        }
                        else
                        {
                            v.Aciklama = "Ödemesi Çıkmamış";
                        }
                        
                        DateTime pt = DateTime.Parse(rows[i - 1].FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[{i}]/td[7]")).Text);
                        v.PoliklinikTarihi = (pt == DateTime.MinValue) ? v.RaporBaslamaTarihi : pt;
                    }
                    else if (columnCount == 8)
                    {
                        //Rapor Takip No Rapor Sira No   Tc Kimlik No Ad Soyad Vaka    Poliklinik Tarihi   Isbasi / Kontrol Tarihi
                        v.RaporTakipNo = rows[i - 1].FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[{i}]/td[2]")).Text;
                        v.RaporSiraNo = Convert.ToInt32(rows[i - 1].FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[{i}]/td[3]")).Text);
                        v.Tcno = rows[i - 1].FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[{i}]/td[4]")).Text;
                        v.AdSoyad = rows[i - 1].FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[{i}]/td[5]")).Text;
                        v.Vaka = rows[i - 1].FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[{i}]/td[6]")).Text;
                        v.PoliklinikTarihi = DateTime.Parse(rows[i - 1].FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[{i}]/td[7]")).Text);
                        v.IsBasiKontrolTarihi = DateTime.Parse(rows[i - 1].FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[{i}]/td[8]")).Text);
                    }
                    else if (columnCount == 6)
                    {
                        //TC Kimlik No Ad Soyad Rapor Başlama Tarihi    Işbaşı / Kontrol Tarihi Aciklama
                        v.Tcno = rows[i - 1].FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/center/table/tbody/tr[{i}]/td[2]")).Text;
                        v.AdSoyad = rows[i - 1].FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/center/table/tbody/tr[{i}]/td[3]")).Text;
                        v.RaporBaslamaTarihi = DateTime.Parse(rows[i - 1].FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/center/table/tbody/tr[{i}]/td[4]")).Text);
                        v.IsBasiKontrolTarihi = DateTime.Parse(rows[i - 1].FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/center/table/tbody/tr[{i}]/td[5]")).Text);
                        v.Aciklama = rows[i - 1].FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/center/table/tbody/tr[{i}]/td[6]")).Text;
                        try
                        {
                            v.RaporBitisTarihi = v.IsBasiKontrolTarihi.AddDays(-1);
                        }
                        catch (Exception)
                        {

                            v.RaporBitisTarihi = DateTime.Parse(DateTime.Today.ToString());
                        }
                    }
                    v.Cnm = SearchReport.CompanyNameSearching;
                    if (SgkLoginCredentials.Sgsc != string.Empty)
                    {
                        v.FirmaSicilNo = SgkLoginCredentials.Sgsc;
                    }
                    else {
                        v.FirmaSicilNo = string.Empty;
                    }
                    
                    lst.Add(v);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }

        private List<Visit> FillRadGridView(string tableXPath, string rowsXPath, string columnsXPath, out string msg)
        {
            msg = "";
            try
            {
                WebDriverWait wait = GetWait();

                // ExpectedConditions silindi, yerine elementin görünürlüğünü (Displayed) kontrol eden modern yapı eklendi
                IWebElement table = wait.Until(d =>
                {
                    var el = d.FindElement(By.XPath(tableXPath));
                    return el.Displayed ? el : null;
                });

                List<IWebElement> rows = Surucu.Driver.FindElements(By.XPath(rowsXPath)).ToList();
                List<IWebElement> columns = Surucu.Driver.FindElements(By.XPath(columnsXPath)).ToList();

                int rowCount = rows.Count;
                List<Visit> lst = new List<Visit>();
                FillVisitClassList(lst, rows, rowCount, columns.Count, out msg);
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }

        private (List<Visit>, string) MultiSearch(string linkXPath, int reportType, out string msg)
        {
            commonFuncs.SetDriver(Surucu.Driver);
            List<Visit> lst = new List<Visit>();
            IWebElement linkReport;
            IWebElement btnSearch;
            WebDriverWait wait = GetWait();

            // MODERN YAPI: Tekrarı önlemek için yerel metot (Local Function)
            IWebElement WaitForVisible(string xpath)
            {
                return wait.Until(d =>
                {
                    var el = d.FindElement(By.XPath(xpath));
                    return el.Displayed ? el : null;
                });
            }

            try
            {
                commonFuncs.WaitForPageLoad(out msg);
                if (commonFuncs.IsPageContains("Onay Bekleyen Rapor Listesi", out msg))
                {
                    // ExpectedConditions yerine WaitForVisible kullanıldı
                    IWebElement detayButon = WaitForVisible("/html/body/table[2]/tbody/tr/td[2]/form/table[2]/tbody/tr/td/input[1]");
                    List<IWebElement> rows = Surucu.Driver.FindElements(By.XPath("/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr")).ToList();
                    List<IWebElement> columns = Surucu.Driver.FindElements(By.XPath("/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[3]/td")).ToList();

                    FillVisitClassList(lst, rows, rows.Count, columns.Count, out msg);
                }

                //Hastalık i = 2, Analık i = 3
                for (int i = 2; i <= 3; i++)
                {
                    linkReport = WaitForVisible(linkXPath);
                    if (GlobalVars.CancelProcess == true) { return (null, "iptal"); }
                    linkReport.Click();

                    switch (reportType)
                    {
                        case 1:
                            if (GlobalVars.CancelProcess == true) { return (null, "iptal"); }
                            commonFuncs.FillTextBox("mernisNo", SearchReport.KimlikNo, out msg);
                            commonFuncs.ChangeComboBox(i, "rapor_turu", out msg);
                            break;

                        case 2:
                            if (GlobalVars.CancelProcess == true) { return (null, "iptal"); }
                            commonFuncs.ChangeDateTimePicker(SearchReport.SearchDate, "tarih", out msg);
                            commonFuncs.ChangeComboBox(i, "vaka", out msg);
                            break;
                    }
                    if (GlobalVars.CancelProcess == true) { return (null, "iptal"); }

                    btnSearch = WaitForVisible("/html/body/table[2]/tbody/tr/td[2]/form/table/tbody/tr[4]/td/input");

                    if (GlobalVars.CancelProcess == true) { return (null, "iptal"); }
                    btnSearch.Click();
                    commonFuncs.WaitForPageLoad(out msg);

                    if (commonFuncs.IsPageContains("Onay Bekleyen Rapor Listesi", out msg))
                    {
                        if (GlobalVars.CancelProcess == true) { return (null, "iptal"); }
                        IWebElement detayButon = WaitForVisible("/html/body/table[2]/tbody/tr/td[2]/form/table[2]/tbody/tr/td/input[1]");
                        List<IWebElement> rows = Surucu.Driver.FindElements(By.XPath("/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr")).ToList();
                        List<IWebElement> columns = Surucu.Driver.FindElements(By.XPath("/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[3]/td")).ToList();
                        FillVisitClassList(lst, rows, rows.Count, columns.Count, out msg);
                    }
                }
                if (lst.Count > 0)
                {
                    return (lst, "");
                }
                return (null, "");
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return (null, "");
            }
        }
        public (List<SourceIgb>, List<SourceSpvudk>, List<SourceSraod>) GetReportDetails(VisitsToBeProcessed V, out string msg)
        {
            msg = ""; string kngaMessage = "", kngaTitle = ""; List<Visit> v;
            string rbtOld = V.RaporBaslamaTarihi.ToString("yyyy-MM-dd");
            string ibtOld = V.IsBasiKontrolTarihi.ToString("yyyy-MM-dd");
            int reportType = SearchReport.ReportTypeAfterRgvLoad;
            SearchReport.ProcessType = 5;
            SearchReport.IsMultiSearch = false;
            WebDriverWait wait = GetWait();

            // MODERN YAPI: Tekrarı önlemek için yerel metot
            IWebElement WaitForVisible(string xpath)
            {
                return wait.Until(d =>
                {
                    var el = d.FindElement(By.XPath(xpath));
                    return el.Displayed ? el : null;
                });
            }

            IWebElement btnShowDetail = null; IWebElement reportListTable = null;
            IWebElement tableIgb = null; IWebElement tableSpvudk = null; IWebElement tableSraob;
            List<SourceIgb> sourceIgBs = new List<SourceIgb>();
            List<SourceSpvudk> sourceSpvudKs = new List<SourceSpvudk>();
            List<SourceSraod> sourceSraoDs = new List<SourceSraod>();
            SourceIgb sourceIgb;
            SourceSpvudk sourceSpvudk;
            SourceSraod sourceSraod;

            try
            {
                switch (reportType)
                {
                    case 1:
                    case 2:
                        SearchReport.KimlikNo = V.Tcno;
                        SearchReport.CaseType = (V.Vaka == "IS KAZASI") ? 1 : (V.Vaka == "HASTALIK") ? 2 : (V.Vaka == "ANALIK") ? 3 : 4;
                        (v, kngaMessage, kngaTitle) = KimlikNoyaGoreArama(out msg); if (kngaMessage == "iptal") { msg = "iptal"; return (null, null, null); }
                        wait.Until(e => ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("return document.readyState").Equals("complete"));

                        reportListTable = WaitForVisible("/html/body/table[2]/tbody/tr/td[2]/form/table[1]");
                        btnShowDetail = WaitForVisible("/html/body/table[2]/tbody/tr/td[2]/form/table[2]/tbody/tr/td/input[1]");
                        break;
                    case 3:
                        SearchReport.StartDate = V.PoliklinikTarihi;
                        SearchReport.EndDate = V.PoliklinikTarihi;
                        (v, kngaMessage, kngaTitle) = OnayliRaporlar(out msg); if (kngaMessage == "iptal") { msg = "iptal"; return (null, null, null); }
                        wait.Until(e => ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("return document.readyState").Equals("complete"));

                        reportListTable = WaitForVisible("/html/body/table[2]/tbody/tr/td[2]/form/table[1]");
                        btnShowDetail = WaitForVisible("/html/body/table[2]/tbody/tr/td[2]/form/table[2]/tbody/tr/td/input");
                        break;
                    case 4:
                        SearchReport.StartDate = V.RaporBaslamaTarihi;
                        SearchReport.EndDate = V.RaporBaslamaTarihi;
                        (v, kngaMessage, kngaTitle) = ArsivdekiRaporlar(out msg); if (kngaMessage == "iptal") { msg = "iptal"; return (null, null, null); }
                        wait.Until(e => ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("return document.readyState").Equals("complete"));

                        reportListTable = WaitForVisible("/html/body/table[2]/tbody/tr/td[2]/form/center/table");
                        btnShowDetail = WaitForVisible("/html/body/table[2]/tbody/tr/td[2]/form/center/input");
                        break;
                }

                IList<IWebElement> rows = reportListTable.FindElements(By.TagName("tr"));
                IList<IWebElement> columns = reportListTable.FindElements(By.TagName("td"));
                IWebElement radio;

                if (reportType < 4)
                {
                    radio = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[3]/td[1]/input"));
                }
                else
                {
                    radio = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/form/center/table/tbody/tr[3]/td[1]/input"));
                }

                if (rows.Count > 3)
                {
                    int index = 1;
                    foreach (IWebElement row in rows)
                    {
                        if (reportType < 4 && row.Text.Contains(V.RaporTakipNo) && row.Text.Contains(rbtOld) && row.Text.Contains(ibtOld))
                        {
                            radio = row.FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[{index}]/td[1]/input"));
                        }
                        else if (reportType == 4 && row.Text.Contains(V.Tcno) && row.Text.Contains(rbtOld) && row.Text.Contains(ibtOld))
                        {
                            radio = row.FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/center/table/tbody/tr[{index}]/td[1]/input"));
                        }
                        index++;
                    }
                }

                radio.Click();
                if (GlobalVars.CancelProcess == true) { msg = "iptal"; return (null, null, null); }
                btnShowDetail.Click();

                wait.Until(e => ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("return document.readyState").Equals("complete"));

                if (reportType == 3)
                {
                    if (GlobalVars.CancelProcess == true) { msg = "iptal"; return (null, null, null); }

                    tableSraob = WaitForVisible("/html/body/table[2]/tbody/tr/td[2]/form/table");
                    List<IWebElement> tableSraobRows = tableSraob.FindElements(By.TagName("tr")).ToList();

                    sourceSraod = new SourceSraod();
                    sourceSraod.Col1 = Surucu.Driver.FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table/tbody/tr[2]/td[1]")).Text;
                    sourceSraod.Col2 = Surucu.Driver.FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table/tbody/tr[2]/td[2]")).Text;
                    sourceSraod.Col3 = Surucu.Driver.FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table/tbody/tr[2]/td[3]")).Text;
                    sourceSraod.Col4 = Surucu.Driver.FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table/tbody/tr[2]/td[4]")).Text;
                    sourceSraod.Col5 = Surucu.Driver.FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table/tbody/tr[2]/td[5]")).Text;
                    sourceSraoDs.Add(sourceSraod);

                    for (int index = 3; index <= tableSraobRows.Count; index++)
                    {
                        sourceSraod = new SourceSraod();
                        sourceSraod.Col1 = Surucu.Driver.FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table/tbody/tr[{index}]/td[1]")).Text;
                        sourceSraod.Col2 = Surucu.Driver.FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table/tbody/tr[{index}]/td[2]")).Text;
                        sourceSraod.Col3 = Surucu.Driver.FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table/tbody/tr[{index}]/td[3]")).Text;
                        sourceSraod.Col4 = Surucu.Driver.FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table/tbody/tr[{index}]/td[4]")).Text;
                        sourceSraod.Col5 = Surucu.Driver.FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table/tbody/tr[{index}]/td[5]")).Text;
                        sourceSraoDs.Add(sourceSraod);
                    }
                    DetailsNameTc.SraodCount = tableSraobRows.Count - 2;
                }
                else
                {
                    if (GlobalVars.CancelProcess == true) { msg = "iptal"; return (null, null, null); }

                    tableIgb = WaitForVisible("/html/body/table[2]/tbody/tr/td[2]/center/table");
                    List<IWebElement> tableIgbRows = tableIgb.FindElements(By.TagName("tr")).ToList();

                    sourceIgb = new SourceIgb();
                    sourceIgb.Col1 = "TC Kimlik No";
                    sourceIgb.Col2 = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[2]/td[2]")).Text;
                    sourceIgb.Col3 = "Ad Soyad";
                    sourceIgb.Col4 = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[2]/td[4]")).Text;
                    sourceIgBs.Add(sourceIgb); sourceIgb = new SourceIgb();

                    sourceIgb.Col1 = "Rapor Takip No";
                    sourceIgb.Col2 = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[3]/td[2]")).Text;
                    sourceIgb.Col3 = "Rapor Sıra No";
                    sourceIgb.Col4 = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[3]/td[4]")).Text;
                    sourceIgBs.Add(sourceIgb); sourceIgb = new SourceIgb();

                    sourceIgb.Col1 = "Sağlık Tesisi Adı";
                    sourceIgb.Col2 = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[4]/td[2]")).Text;
                    sourceIgb.Col3 = "Düzenleyen Poliklinik Kodu";
                    sourceIgb.Col4 = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[4]/td[4]")).Text;
                    sourceIgBs.Add(sourceIgb); sourceIgb = new SourceIgb();

                    sourceIgb.Col1 = "Polikinik Tarihi";
                    sourceIgb.Col2 = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[5]/td[2]")).Text;
                    sourceIgb.Col3 = "Poliklinik Defter Sıra No";
                    sourceIgb.Col4 = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[5]/td[4]")).Text;
                    sourceIgBs.Add(sourceIgb); sourceIgb = new SourceIgb();

                    sourceIgb.Col1 = "Vaka";
                    sourceIgb.Col2 = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[6]/td[2]")).Text;
                    sourceIgb.Col3 = "Rapor Durumu";
                    sourceIgb.Col4 = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[6]/td[4]")).Text;
                    sourceIgBs.Add(sourceIgb); sourceIgb = new SourceIgb();

                    sourceIgb.Col1 = "Hastane Yatış Tarihi";
                    sourceIgb.Col2 = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[7]/td[2]")).Text;
                    sourceIgb.Col3 = "Hastane Çıkıs Tarihi";
                    sourceIgb.Col4 = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[7]/td[4]")).Text;
                    sourceIgBs.Add(sourceIgb); sourceIgb = new SourceIgb();

                    sourceIgb.Col1 = "Rapor Baslama Tarihi";
                    sourceIgb.Col2 = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[8]/td[2]")).Text;
                    sourceIgb.Col3 = "Rapor Bitiş Tarihi";
                    sourceIgb.Col4 = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[8]/td[4]")).Text;
                    sourceIgBs.Add(sourceIgb); sourceIgb = new SourceIgb();

                    sourceIgb.Col1 = "Rapor Türü";
                    sourceIgb.Col2 = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[9]/td[2]")).Text;
                    sourceIgb.Col3 = "Ekrana Düştüğü Tarih";
                    sourceIgb.Col4 = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[9]/td[4]")).Text;
                    sourceIgBs.Add(sourceIgb); sourceIgb = new SourceIgb();

                    sourceIgb.Col1 = "İş Kazası Tarihi";
                    sourceIgb.Col2 = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[10]/td[2]")).Text;
                    sourceIgb.Col3 = String.Empty;
                    sourceIgb.Col4 = String.Empty;
                    sourceIgBs.Add(sourceIgb);

                    if (tableIgbRows.Count == 11)
                    {
                        sourceIgb = new SourceIgb();
                        sourceIgb.Col1 = "İzin Başlama Tarihi";
                        sourceIgb.Col2 = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[11]/td[2]")).Text;
                        sourceIgb.Col3 = "Tahmini Bebek Dogum Tarihi";
                        sourceIgb.Col4 = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[11]/td[4]")).Text;
                        sourceIgBs.Add(sourceIgb);
                    }

                    tableSpvudk = IsElementEXISTS(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/form/table[1]"));
                    List<IWebElement> tableSpvudkRows = tableSpvudk.FindElements(By.TagName("tr")).ToList();

                    if (tableSpvudkRows.Count > 2)
                    {
                        if (GlobalVars.CancelProcess == true) { msg = "iptal"; return (null, null, null); }

                        sourceSpvudk = new SourceSpvudk();
                        sourceSpvudk.Col1 = "Yıl"; sourceSpvudk.Col2 = "Ay"; sourceSpvudk.Col3 = "Prim"; sourceSpvudk.Col4 = "Ücret Dışı Kazançlar";
                        sourceSpvudKs.Add(sourceSpvudk); sourceSpvudk = new SourceSpvudk();

                        for (int i = 3; i < tableSpvudkRows.Count; i++)
                        {
                            sourceSpvudk.Col1 = Surucu.Driver.FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/center/form/table[1]/tbody/tr[{i}]/td[1]")).Text;
                            sourceSpvudk.Col2 = Surucu.Driver.FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/center/form/table[1]/tbody/tr[{i}]/td[2]")).Text;
                            sourceSpvudk.Col3 = Surucu.Driver.FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/center/form/table[1]/tbody/tr[{i}]/td[3]")).Text;
                            sourceSpvudk.Col4 = Surucu.Driver.FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/center/form/table[1]/tbody/tr[{i}]/td[4]")).Text;
                            sourceSpvudKs.Add(sourceSpvudk); sourceSpvudk = new SourceSpvudk();
                        }

                        sourceSpvudk.Col1 = "Sigortalının Günlük Kazancı (PEK)";
                        sourceSpvudk.Col2 = Surucu.Driver.FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/center/form/table[1]/tbody/tr[{tableSpvudkRows.Count}]/td[2]")).Text;
                        sourceSpvudk.Col3 = String.Empty;
                        sourceSpvudk.Col4 = String.Empty;
                        sourceSpvudKs.Add(sourceSpvudk);
                    }
                }

                DetailsNameTc.Tcno = V.Tcno;
                DetailsNameTc.Name = V.AdSoyad;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return (null, null, null);
            }

            Visits.Clear(); tempVisit.Clear();
            return (sourceIgBs, sourceSpvudKs, sourceSraoDs);
        }

        public (DateTime, DateTime, string) GetReportDetails(Visit V, out string msg) // Analık türündeki raporlar için
        {
            commonFuncs.SetDriver(Surucu.Driver);
            msg = "";
            string tarihRba = "";
            string tarihRbi = "";
            string durum = "";
            string kngaMessage = "", kngaTitle = "";
            List<Visit> v;

            string rbtOld = V.RaporBaslamaTarihi.ToString("yyyy-MM-dd");
            string ibtOld = V.IsBasiKontrolTarihi.ToString("yyyy-MM-dd");
            SearchReport.ProcessType = 5;
            SearchReport.IsMultiSearch = false;
            WebDriverWait wait = GetWait();

            // MODERN YAPI: Tekrarı önlemek için yerel metot
            IWebElement WaitForVisible(string xpath)
            {
                return wait.Until(d =>
                {
                    var el = d.FindElement(By.XPath(xpath));
                    return el.Displayed ? el : null;
                });
            }

            try
            {
                SearchReport.KimlikNo = V.Tcno;
                SearchReport.CaseType = 3;
                (v, kngaMessage, kngaTitle) = KimlikNoyaGoreArama(out msg);
                if (kngaMessage == "iptal") { return (DateTime.MinValue, DateTime.MinValue, "iptal"); }

                wait.Until(e => ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("return document.readyState").Equals("complete"));

                // ExpectedConditions silindi, WaitForVisible kullanıldı
                IWebElement reportListTable = WaitForVisible("/html/body/table[2]/tbody/tr/td[2]/form/table[1]");
                IWebElement btnShowDetail = WaitForVisible("/html/body/table[2]/tbody/tr/td[2]/form/table[2]/tbody/tr/td/input[1]");

                if (GlobalVars.CancelProcess) { return (DateTime.MinValue, DateTime.MinValue, "iptal"); }

                IList<IWebElement> rows = reportListTable.FindElements(By.TagName("tr"));
                IWebElement radio = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[3]/td[1]/input"));

                if (rows.Count > 3)
                {
                    int index = 1;
                    foreach (IWebElement row in rows)
                    {
                        if (GlobalVars.CancelProcess) { return (DateTime.MinValue, DateTime.MinValue, "iptal"); }

                        if (index > 2)
                        {
                            string rsn = row.FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[{index}]/td[6]")).Text;
                            if (row.Text.Contains(V.RaporTakipNo) && V.RaporSiraNo.ToString() == rsn && row.Text.Contains(rbtOld) && row.Text.Contains(ibtOld))
                            {
                                radio = row.FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[{index}]/td[1]/input"));
                                break;
                            }
                        }
                        index++;
                    }
                }

                radio.Click();
                if (GlobalVars.CancelProcess) { return (DateTime.MinValue, DateTime.MinValue, "iptal"); }

                btnShowDetail.Click();
                commonFuncs.WaitForPageLoad(out msg);

                if (commonFuncs.IsPageContains("Dogum Sonrasi Analik", out msg))
                {
                    if (GlobalVars.CancelProcess) { return (DateTime.MinValue, DateTime.MinValue, "iptal"); }

                    string hyt = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[7]/td[2]")).Text;
                    if (string.IsNullOrWhiteSpace(hyt) || hyt == "0001-01-01")
                    {
                        if (GlobalVars.CancelProcess) { return (DateTime.MinValue, DateTime.MinValue, "iptal"); }
                        tarihRba = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[8]/td[2]")).Text;
                    }
                    else
                    {
                        tarihRba = hyt;
                    }

                    tarihRbi = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[8]/td[4]")).Text;
                    durum = "Doğum Sonrası Analık";
                }
                else if (commonFuncs.IsPageContains("Dogum Oncesi Analik Calisir", out msg))
                {
                    if (GlobalVars.CancelProcess) { return (DateTime.MinValue, DateTime.MinValue, "iptal"); }

                    tarihRba = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[11]/td[2]")).Text;
                    string rbit = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[8]/td[4]")).Text;

                    if (rbit.Contains("0001"))
                    {
                        if (GlobalVars.CancelProcess) { return (DateTime.MinValue, DateTime.MinValue, "iptal"); }
                        tarihRbi = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[11]/td[4]")).Text;
                    }
                    else
                    {
                        tarihRbi = rbit;
                    }

                    durum = "Doğum Öncesi Analık, Çalışır";
                }

                // Return işlemini güvenli parse ile try bloğu içinde yapıyoruz
                DateTime parsedRba = string.IsNullOrWhiteSpace(tarihRba) ? DateTime.MinValue : DateTime.Parse(tarihRba);
                DateTime parsedRbi = string.IsNullOrWhiteSpace(tarihRbi) ? DateTime.MinValue : DateTime.Parse(tarihRbi);

                return (parsedRba, parsedRbi, durum);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                // Hata durumunda parse etmeye çalışıp programı çökertecek yapıyı önlüyoruz
                return (DateTime.MinValue, DateTime.MinValue, "Hata oluştu");
            }
        }
        #endregion

        #region raporonaylama_ve_iptal_etme

        public (string, string, int) DoConfirmReport(List<VisitsToBeProcessed> onaylanacaklar, out string msg)
        {
            msg = "";
            string kngaMessage = "", kngaTitle = "";
            List<Visit> v;
            commonFuncs.SetDriver(Surucu.Driver);

            int reportType = SearchReport.ReportType;
            SearchReport.ProcessType = 5;
            SearchReport.IsMultiSearch = false;
            WebDriverWait wait = GetWait();

            IWebElement btnShowDetail = null;
            IWebElement reportListTable = null;
            ConfirmReport cr;

            // MODERN YAPI: Hem görünürlük hem de isteğe bağlı tıklanabilirlik kontrolü yapan yerel metot
            IWebElement WaitForElement(By locator, bool requireClickable = false)
            {
                return wait.Until(d =>
                {
                    var el = d.FindElements(locator).FirstOrDefault(e => e.Displayed);
                    if (el != null && requireClickable && !el.Enabled) return null;
                    return el;
                });
            }

            try
            {
                foreach (VisitsToBeProcessed V in onaylanacaklar)
                {
                    if (GlobalVars.CancelProcess) return ("iptal", RgvTitleText, 0);

                    cr = new ConfirmReport();

                    switch (reportType)
                    {
                        case 1:
                        case 2:
                            SearchReport.KimlikNo = V.Tcno;
                            SearchReport.CaseType = (V.Vaka == "IS KAZASI") ? 1 : (V.Vaka == "HASTALIK") ? 2 : (V.Vaka == "ANALIK") ? 3 : 4;
                            cr.Vaka = V.Vaka;

                            (v, kngaMessage, kngaTitle) = KimlikNoyaGoreArama(out msg);
                            if (kngaMessage == "iptal") return ("iptal", RgvTitleText, 0);

                            wait.Until(e => ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("return document.readyState").Equals("complete"));

                            reportListTable = WaitForElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/form/table[1]"));
                            btnShowDetail = WaitForElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/form/table[2]/tbody/tr/td/input[1]"));
                            break;

                        case 4:
                            SearchReport.StartDate = V.RaporBaslamaTarihi;
                            SearchReport.EndDate = V.RaporBaslamaTarihi;

                            (v, kngaMessage, kngaTitle) = ArsivdekiRaporlar(out msg);
                            if (kngaMessage == "iptal") return ("iptal", RgvTitleText, 0);

                            wait.Until(e => ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("return document.readyState").Equals("complete"));

                            reportListTable = WaitForElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/form/center/table"));
                            btnShowDetail = WaitForElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/form/center/input"));
                            wait.Until(e => ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("return document.readyState").Equals("complete"));
                            break;
                    }

                    IList<IWebElement> rows = reportListTable.FindElements(By.TagName("tr"));
                    IWebElement radio;

                    if (reportType < 4)
                    {
                        if (GlobalVars.CancelProcess) return ("iptal", RgvTitleText, 0);
                        radio = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[3]/td[1]/input"));
                    }
                    else
                    {
                        if (GlobalVars.CancelProcess) return ("iptal", RgvTitleText, 0);
                        radio = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/form/center/table/tbody/tr[3]/td[1]/input"));
                    }

                    if (rows.Count > 3)
                    {
                        int index = 1;
                        foreach (IWebElement row in rows)
                        {
                            if (GlobalVars.CancelProcess) return ("iptal", RgvTitleText, 0);

                            if (reportType < 4 && row.Text.Contains(V.RaporTakipNo))
                            {
                                radio = row.FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[{index}]/td[1]/input"));
                            }
                            else if (reportType == 4 && row.Text.Contains(V.Tcno) && row.Text.Contains(V.RaporBaslamaTarihi.ToString("yyyy-MM-dd")))
                            {
                                radio = row.FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/center/table/tbody/tr[{index}]/td[1]/input"));
                            }
                            index++;
                        }
                    }

                    if (GlobalVars.CancelProcess) return ("iptal", RgvTitleText, 0);
                    radio.Click();

                    IWebElement texConfirmDate;
                    IWebElement comboWorkingStatus;
                    IWebElement btnOnay;
                    IWebElement btnPersonelimDegil;

                    cr.Tcid = V.Tcno;
                    cr.Fullname = V.AdSoyad;
                    cr.Rtno = V.RaporTakipNo;
                    cr.Rsno = V.RaporSiraNo;
                    cr.Rbat = V.RaporBaslamaTarihi;
                    cr.Rbit = V.RaporBitisTarihi;
                    cr.Onyt = DateTime.Now;

                    if (V.WorkingStatus != 2)
                    {
                        if (GlobalVars.CancelProcess) return ("iptal", RgvTitleText, 0);
                        btnShowDetail.Click();

                        if (SearchReport.ReportType == 4)
                        {
                            if (GlobalVars.CancelProcess) return ("iptal", RgvTitleText, 0);
                            V.RaporTakipNo = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[3]/td[2]")).Text;
                            V.RaporSiraNo = Convert.ToInt32(Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[3]/td[4]")).Text);
                        }

                        if (Surucu.Driver.PageSource.Replace("  ", " ").Contains(V.Tcno))
                        {
                            if (GlobalVars.CancelProcess) return ("iptal", RgvTitleText, 0);

                            texConfirmDate = WaitForElement(By.Name("onayBitisTarihi"));
                            if (V.ConfirmDate.Length == 18) { V.ConfirmDate = $"0{V.ConfirmDate}".Remove(10); }
                            texConfirmDate.SendKeys(V.ConfirmDate);

                            comboWorkingStatus = WaitForElement(By.Name("calismaDurumu"));

                            // Rapor için ödeme yapılmışsa, yalnızca "çalışmamıştır" seçeneği vardır. Öbür türlü "çalışmıştır" ya da "çalışmamıştır" seçilebilir.
                            if (!Surucu.Driver.PageSource.Replace("  ", " ").Contains("stirahat Raporu "))
                            {
                                commonFuncs.ChangeComboBox(V.WorkingStatus, "calismaDurumu", out msg);
                            }

                            btnOnay = WaitForElement(By.Name("kaydet"));

                            if (reportType == 4)
                            {
                                if (GlobalVars.CancelProcess) return ("iptal", RgvTitleText, 0);
                                cr.Vaka = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[6]/td[2]")).Text;
                                cr.Rbit = DateTime.Parse(Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[8]/td[4]")).Text);
                                cr.Rtno = Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[3]/td[2]")).Text;
                                cr.Rsno = Convert.ToInt32(Surucu.Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/center/table/tbody/tr[3]/td[4]")).Text);
                            }

                            if (GlobalVars.CancelProcess) return ("iptal", RgvTitleText, 0);
                            btnOnay.Click();

                            if (Surucu.Driver.PageSource.Replace("  ", " ").Contains("başarıyla kaydedilmiştir"))
                            {
                                cr.Rslt = "Rapor onaylandı";
                                if (SearchReport.GetConfirmPdf)
                                {
                                    IWebElement btnDokum = WaitForElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[3]/tbody/tr/td/a"), true);
                                    string oldFn = $@"{SearchReport.DownloadDir}PdfTable.pdf";
                                    string newFn = $@"{SearchReport.DocumentsDir}\{V.Cnm}-{V.AdSoyad}-{V.RaporTakipNo}-{V.RaporSiraNo}.pdf";

                                    if (File.Exists(oldFn))
                                    {
                                        File.Delete(oldFn);
                                    }
                                    btnDokum.Click();

                                    if (WaitForFileDl(oldFn))
                                    {
                                        if (ChangeFileName($"{oldFn}", $"{newFn}", out msg)) { cr.Pdffile = newFn; } else { cr.Pdffile = "Dosya indirilemedi"; }
                                    }
                                    else { cr.Pdffile = "Dosya indirilemedi"; }
                                }
                            }
                            else if (Surucu.Driver.PageSource.Replace("  ", " ").Contains("Gunun Tarihinden Sonra"))
                            {
                                SearchReport.ConfirmError = true;
                                cr.Rslt = $"Rapor onaylanamadı \"Sectiginiz Tarihler Günün Tarihinden Sonra Olamaz!\" hatası alındı. Rapor bitiş tarihi: {cr.Rbit.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture)}, Seçtiğiniz Tarih: {V.ConfirmDate}";
                                cr.Pdffile = "Dosya indirilemedi";
                            }
                            else
                            {
                                SearchReport.ConfirmError = true;
                                cr.Rslt = "Rapor onaylanamadı";
                                cr.Pdffile = "Dosya indirilemedi";
                            }
                        }
                    }
                    else
                    {
                        if (GlobalVars.CancelProcess) return ("iptal", RgvTitleText, 0);
                        btnPersonelimDegil = WaitForElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/form/table[2]/tbody/tr/td/input[2]"));
                        btnPersonelimDegil.Click();

                        if (Surucu.Driver.PageSource.Replace("  ", " ").Contains("Sigortalinin Isveren Bilgileri Guncellenemedi."))
                        {
                            SearchReport.ConfirmError = true;
                            cr.Rslt = "Personelim değil yapılamadı";
                        }
                        else
                        {
                            cr.Rslt = "Personelim değil yapıldı";
                        }
                    }
                    ConfirmReports.Add(cr);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                Message = $"Rapor onaylama işlemi tamamlanamadı, lütfen tekrar deneyin. Hata : {msg}";
                return (Message, RgvTitleText, ConfirmReports.Count);
            }

            return (Message, RgvTitleText, ConfirmReports.Count);
        }

        public (string, string, int) DoCancelReport(List<VisitsToBeProcessed> iptalEdilecekler, out string msg)
        {
            msg = "";
            string kngaMessage, kngaTitle;
            List<Visit> v;
            int cancelled = 0;
            commonFuncs.SetDriver(Surucu.Driver);

            ProcessReport ur;
            WebDriverWait wait = GetWait();

            // MODERN YAPI: DOM yenilenmelerine karşı elementi dinamik ve güvenli bekleyen yerel metot
            IWebElement WaitForElement(By locator)
            {
                return wait.Until(d =>
                {
                    var el = d.FindElements(locator).FirstOrDefault(e => e.Displayed);
                    return el;
                });
            }

            try
            {
                foreach (VisitsToBeProcessed personel in iptalEdilecekler)
                {
                    if (GlobalVars.CancelProcess) return ("iptal", RgvTitleText, 0);

                    ur = new ProcessReport()
                    {
                        Tcid = personel.Tcno,
                        Fullname = personel.AdSoyad,
                        Vaka = personel.Vaka,
                        Rtno = personel.RaporTakipNo,
                        Rsno = personel.RaporSiraNo,
                        Rbat = personel.PoliklinikTarihi,
                        Rbit = personel.IsBasiKontrolTarihi,
                        Onyt = DateTime.Now
                    };

                    SearchReport.StartDate = personel.PoliklinikTarihi;
                    SearchReport.EndDate = personel.PoliklinikTarihi;

                    (v, kngaMessage, kngaTitle) = OnayliRaporlar(out msg);
                    if (kngaMessage == "iptal") return ("iptal", RgvTitleText, 0);

                    if (GlobalVars.CancelProcess) return ("iptal", RgvTitleText, 0);

                    // Tablo görünene kadar güvenli bekleme yapılıyor
                    IWebElement reportListTable = WaitForElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/form/table[1]"));
                    IList<IWebElement> rows = reportListTable.FindElements(By.TagName("tr"));

                    // Dinamik locator tanımlamaları
                    By radioLocator = By.XPath("/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[3]/td[1]/input");
                    By btnOnayLocator = By.XPath("/html/body/table[2]/tbody/tr/td[2]/form/table[2]/tbody/tr/td/input[1]");

                    IWebElement radio = WaitForElement(radioLocator);
                    IWebElement btnOnay = WaitForElement(btnOnayLocator);

                    if (rows.Count > 3)
                    {
                        int index = 1;
                        foreach (IWebElement row in rows)
                        {
                            if (GlobalVars.CancelProcess) return ("iptal", RgvTitleText, 0);
                            if (row.Text.Contains(personel.RaporTakipNo))
                            {
                                radio = row.FindElement(By.XPath($"/html/body/table[2]/tbody/tr/td[2]/form/table[1]/tbody/tr[{index}]/td[1]/input"));
                            }
                            index++;
                        }
                    }

                    if (GlobalVars.CancelProcess) return ("iptal", RgvTitleText, 0);
                    radio.Click();
                    btnOnay.Click();

                    WaitForPageLoaded(out msg);
                    if (GlobalVars.CancelProcess) return ("iptal", RgvTitleText, 0);

                    // KRİTİK İYİLEŞTİRME: Sayfa yüklendikten sonra elementler DOM'da tazece yeniden aranıyor (StaleElement önlemi)
                    radio = WaitForElement(radioLocator);
                    btnOnay = WaitForElement(btnOnayLocator);

                    radio.Click();
                    btnOnay.Click();

                    // Sayfa kaynağındaki çift boşluklar temizlenerek kontrol sağlanıyor
                    string pageSource = Surucu.Driver.PageSource.Replace("  ", " ");
                    if (pageSource.Contains("Raporun Odemesi Yapilmis,Onay Iptal Edilemez!") || pageSource.Contains("Ödeme yapıldığı için işlem yapamazsınız!"))
                    {
                        if (GlobalVars.CancelProcess) return ("iptal", RgvTitleText, 0);
                        Message = "Raporun Ödemesi Yapılmış, Onay İptal Edilemez!";
                        ur.Rslt = "Raporun ödemesi yapılmış, onay iptal edilemez!";
                    }
                    else
                    {
                        cancelled++;
                        Message = $"{personel.Tcno} kimlik numaralı personelin {personel.RaporTakipNo} takip numaralı raporunun onayı iptal edildi.";
                        ur.Rslt = $"{personel.RaporTakipNo} takip numaralı raporunun onayı iptal edildi";
                    }
                    ProcessReports.Add(ur);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                Message = $"Rapor iptal etme işlemi tamamlanamadı. Hata: {msg}";
            }

            RgvTitleText = "Rapor iptal etme işlemi tamamlandı";
            return (Message, RgvTitleText, cancelled);
        }
        #endregion

        #region yardimcimetotlar 

        public bool WaitForFileDl(string path)
        {
            
            for (var i = 0; i < 30; i++)
            {
                if (File.Exists(path)) { break; }
                Thread.Sleep(1000);
            }
            var length = new FileInfo(path).Length;
            for (var i = 0; i < 30; i++)
            {
                Thread.Sleep(1000);
                var newLength = new FileInfo(path).Length;
                if (newLength == length && length != 0) { return true; }
            }
            return false;
        }
        public bool ChangeFileName(string oldFn, string newFn, out string msg)
        {
            msg = "";
            try
            {
                if (!File.Exists(oldFn))
                {
                    return false;
                }
                if (File.Exists(newFn))
                {
                    File.Delete(newFn);
                }else{
                    File.Move(oldFn, newFn);
                    return true;
                }
                Thread.Sleep(1000);
                if (File.Exists(newFn))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString(); return false;
            }
        }
        #endregion
    }
}
