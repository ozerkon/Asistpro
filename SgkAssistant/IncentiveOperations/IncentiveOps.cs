using Models.Common;
using Models.Domain;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SGKServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Telerik.WinControls.UI;
using WebDriverX;

namespace SgkAssistant.IncentiveOperations
{
    public class IncentiveOps : TrmBase
    {
        public void GetIncentive(List<string> tckns, string cn, ref RadLabel lbl, out string msg)
        {
            int sayac = 1;
            bool elementFound = true;
            lbl.Text += "<html>";
            msg = "";
            try
            {
                IWebElement textTckn;
                IWebElement slcEdu;
                IWebElement btnSearch;

                Wait = IOC.SgkLinksService.GetWait();

                foreach (string tcno in tckns)
                {
                    //bool passPersonal = false;
                    lbl.Text += $"<strong><span style=\"font-size: 10pt\">{tcno} kimlik numarası için potansiyel teşvik aranıyor... </span></strong>\r\n";
                    if (LinkGlobals.LinkCancel) { msg = "iptal"; return; }
                    
                    textTckn = IOC.SgkLinksService.GetElementBy("i", "uygunTesvikSorgula_tcKimlikNo", out msg);
                    slcEdu = IOC.SgkLinksService.GetElementBy("i", "uygunTesvikSorgula_egitimDurumu", out msg);
                    btnSearch = IOC.SgkLinksService.GetElementBy("i", "uygunTesvikSorgula_0", out msg);
                    textTckn.Clear();
                    textTckn.SendKeys(tcno);
                    new SelectElement(slcEdu).SelectByIndex(1);
                    //btnSearch.Click();
                    bool hasLoaded = false, clicked = false; ; int continueWaiting = 1;
                    while (!hasLoaded)
                    {
                        try
                        {
                            if (!clicked)
                            {
                                clicked = true;
                                btnSearch.Click();
                            }
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
                                break;
                            }
                        }
                    }
                    if (!hasLoaded) continue;
                    
                    if (Surucu.Driver.PageSource.Replace("  "," ").Contains("Geçersiz Bir TC Kimlik Numarası Girdiniz."))
                    {
                        lbl.Text += $"<strong><span style=\"font-size: 10pt\">{tcno} için geçersiz kimlik numarası hatası alındı... </span></strong>\r\n";
                        continue;
                    }

                    for (int i = 1; i <= 10; i++)
                    {
                        Incentive tsvk = new Incentive();
                        tsvk.Cn = cn;
                        tsvk.Tcno = tcno;
                        IWebElement row = null;
                        sayac = 1; elementFound = true;
                        while (row == null)
                        {
                            row = IOC.SgkLinksService.GetElementBy("x", $"/html/body/div/div/table/tbody/tr[2]/td/table/tbody/tr[2]/td[2]/div/table[{i}]/tbody/tr[1]/th", out msg);
                            sayac++; Thread.Sleep(100);
                            if (sayac == 200) { elementFound = false; break; }
                        }
                        if (!elementFound)
                        {
                            lbl.Text += $"<strong><span style=\"font-size: 10pt; color:red\">Hata: Kanun numaraları tablosu bulunamadı</span></strong>\r\n";
                            continue;
                        }
                        string bgColor = row.GetCssValue("background-color");
                        if (bgColor.Contains("179, 255, 217"))
                        {
                            row.Click();
                            string kanunNo = row.GetAttribute("onclick");
                            kanunNo = kanunNo.Remove(0, 11).Replace(")", "");
                            if (kanunNo == "5510" || kanunNo == "7252 " || kanunNo == "17256 " || kanunNo == "27256  ") continue;
                            string selector = $"{kanunNo}Class";
                            List<IWebElement> trs = null; sayac = 1; elementFound = true;
                            while (trs == null)
                            {
                                trs = Wait.Until(e => e.FindElements(By.XPath($"//tr[@class='{selector} hepsi']"))).ToList();
                                sayac++; Thread.Sleep(100);
                                if (sayac == 200) { elementFound = false; break; }
                            }
                            if (!elementFound)
                            {
                                lbl.Text += $"<strong><span style=\"font-size: 10pt; color:red\">Hata: {kanunNo} numaralı kanunun satırı bulunamadı</span></strong>\r\n";
                                continue;
                            }

                            foreach (IWebElement tr in trs)
                            {
                                IWebElement th = null; sayac = 1; elementFound = true;
                                while (th == null)
                                {
                                    th = tr.FindElement(By.TagName("th")); sayac++; Thread.Sleep(100);
                                    if(sayac == 200) { elementFound = false; break; }
                                }
                            
                                if (!elementFound) { 
                                    lbl.Text += $"<strong><span style=\"font-size: 10pt; color:red\">Hata: {kanunNo} numaralı kanuna ait bilgiler alınamadı</span></strong>\r\n";
                                    continue;
                                }
                                IWebElement td = null; 
                                bool exitLoop = false;
                                tsvk.No = kanunNo;
                                string tdx = th.Text.Trim().Replace("\r\n", "");
                                if (tdx.Contains("Süre"))
                                {
                                        td = tr.FindElement(By.TagName("td"));
                                        tsvk.Ts = td.Text.Trim();
                                }else if (tdx.Contains("Başlangıç-Bitiş"))
                                {
                                    td = tr.FindElement(By.TagName("td"));
                                    tsvk.Bbd = td.Text.Trim();
                                }
                                else if (tdx.Contains("Olunacak"))
                                {
                                    td = tr.FindElement(By.TagName("td"));
                                    tsvk.Ios = Convert.ToInt32(td.Text.Trim().Replace(".0",""));
                                }
                                else if (tdx.Contains("Tutar"))
                                {
                                    td = tr.FindElement(By.TagName("td"));
                                    tsvk.Tk = td.Text.Trim();
                                    exitLoop = true;
                                }
                                
                                if (exitLoop)
                                {
                                    break;
                                }
                            }
                            lbl.Text += $"<strong><span style=\"font-size: 10pt; color:darkgreen\">{tcno} için {tsvk.No} numaralı kanundan {tsvk.Tk} potansiyel kazanç bulundu</span></strong>\r\n";
                        }
                        GlobalVars.LstInc.Add(tsvk);
                    }       
                }
            }
            catch (Exception ex)
            {
                msg = $"Hata: {ex.Message}";
            }
            finally
            {
                lbl.Text += "</html>";
            }
        }
    }
}
