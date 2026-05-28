using Models.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using WebDriverX;

namespace SGKServices
{
    public class CommonFuncs : TrmBase
    {
        public IWebElement ClickElement { get; set; }
        public IWebElement CaptchaImg { get; set; }

        public void SetDriver(IWebDriver webDriver) { Surucu.Driver = webDriver; }

        public void FillTextBox(string element, string text, out string msg)
        {
            msg = "";
            try
            {
                if (Surucu.Driver == null)
                {
                    msg = "Driver not initialized";
                    return;
                }
                IWebElement textbox = Surucu.Driver.FindElement(By.Name(element));
                textbox.SendKeys(text);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public void ClearTextBox(string element, out string msg)
        {
            msg = "";
            try
            {
                if (Surucu.Driver == null)
                {
                    msg = "Driver not initialized";
                    return;
                }
                IWebElement textbox = Surucu.Driver.FindElement(By.Name(element));
                textbox.Clear();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public void ChangeComboBox(int index, string comboBox, out string msg)
        {
            msg = "";
            try
            {
                if (Surucu.Driver == null)
                {
                    msg = "Driver not initialized";
                    return;
                }
                IWebElement element = Surucu.Driver.FindElement(By.Name(comboBox));
                new SelectElement(element).SelectByIndex(index);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public void ChangeComboBox(string value, IWebElement element, out string msg)
        {
            msg = "";
            try
            {
                new SelectElement(element).SelectByText(value);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public void ChangeDateTimePicker(DateTime date, string dateTimePicker, out string msg)
        {
            msg = "";
            try
            {
                if (Surucu.Driver == null)
                {
                    msg = "Driver not initialized";
                    return;
                }
                IWebElement element = Surucu.Driver.FindElement(By.Name(dateTimePicker));
                element.Clear();
                element.SendKeys(date.ToString("dd.MM.yyyy"));
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public void ClickWebElement(string element, out string msg, string method = "XPath")
        {
            msg = "";
            try
            {
                if (Surucu.Driver == null)
                {
                    msg = "Driver not initialized";
                    return;
                }
                By locator = method.ToLower() switch
                {
                    "i" => By.Id(element),
                    "c" => By.ClassName(element),
                    "s" => By.CssSelector(element),
                    "t" => By.TagName(element),
                    "l" => By.LinkText(element),
                    "x" => By.XPath(element),
                    _ => By.XPath(element)
                };

                ClickElement = Surucu.Driver.FindElement(locator);
                ClickElement.Click();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }

        public bool IsPageContains(string text, out string msg)
        {
            msg = "";
            try
            {
                if (Surucu.Driver == null)
                {
                    msg = "Driver not initialized";
                    return false;
                }
                return Surucu.Driver.PageSource.Replace("  ", " ").Contains(text);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }

        public string GetImageSrc(string element, string type, out string msg)
        {
            msg = "";
            if (Surucu.Driver == null)
            {
                msg = "Driver not initialized";
                return string.Empty;
            }
            WebDriverWait wait = new WebDriverWait(Surucu.Driver, TimeSpan.FromSeconds(LinkGlobals.MaxWait));
            try
            {
                By locator = type.ToLower() switch
                {
                    "x" => By.XPath(element),
                    "t" => By.TagName(element),
                    "c" => By.ClassName(element),
                    "s" => By.CssSelector(element),
                    "n" => By.Name(element),
                    "i" => By.Id(element),
                    _ => null
                };

                if (locator == null) return string.Empty;

                // Harici paket (ExpectedConditions) olmadan, elementin görünürlüğünü doğrudan lambda ile kontrol ediyoruz
                CaptchaImg = wait.Until(d => d.FindElements(locator).FirstOrDefault(e => e.Displayed));

                return CaptchaImg?.GetAttribute("src") ?? string.Empty;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return string.Empty;
            }
        }

        public void WaitForPageLoad(out string msg)
        {
            msg = "";
            try
            {
                Surucu.Driver.SwitchTo().Alert().Dismiss();
            }
            catch (NoAlertPresentException)
            {
                // JavaScript uyarısı yoksa görmezden geliyoruz
            }
            try
            {
                WebDriverWait wait = GetWait();
                wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
        }
    }
}