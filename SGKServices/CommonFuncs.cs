using Models.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
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
                IWebElement textbox = Surucu.Driver.FindElement(By.Name(element));
                textbox.SendKeys(text);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }
        public void ClearTextBox(string element, out string msg)
        {
            msg = "";
            try
            {
                IWebElement textbox = Surucu.Driver.FindElement(By.Name(element));
                textbox.Clear();
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }
        public void ChangeComboBox(int index, string comboBox, out string msg)
        {
            msg = "";
            try
            {
                IWebElement element = Surucu.Driver.FindElement(By.Name(comboBox));
                new SelectElement(element).SelectByIndex(index);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
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
                msg = ex.Message.ToString();
            }
        }
        public void ChangeDateTimePicker(DateTime date, string dateTimePicker, out string msg)
        {
            msg = "";
            try 
            { 
                IWebElement element = Surucu.Driver.FindElement(By.Name(dateTimePicker));
                element.Clear();
                element.SendKeys(date.ToString("dd.MM.yyyy"));
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }
        public void ClickWebElement(string element, out string msg, string method = "XPath")
        {
            msg = "";
            try
            {
                switch (method)
                {
                    case "i":
                        ClickElement = Surucu.Driver.FindElement(By.Id($"{element}"));
                        break;
                    case "c":
                        ClickElement = Surucu.Driver.FindElement(By.ClassName($"{element}"));
                        break;
                    case "s":
                        ClickElement = Surucu.Driver.FindElement(By.CssSelector($"{element}"));
                        break;
                    case "t":
                        ClickElement = Surucu.Driver.FindElement(By.TagName($"{element}"));
                        break;
                    case "l":
                        ClickElement = Surucu.Driver.FindElement(By.LinkText($"{element}"));
                        break;
                    case "x":
                        ClickElement = Surucu.Driver.FindElement(By.XPath($"{element}"));
                        break;
                    default:
                        ClickElement = Surucu.Driver.FindElement(By.XPath($"{element}"));
                        break;
                }
                ClickElement.Click();
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }
        public bool IsPageContains(string text, out string msg)
        {
            msg = "";
            try
            {
                return Surucu.Driver.PageSource.Replace("  "," ").Contains(text);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }
        }
        public string GetImageSrc(string element, string type,  out string msg)
        {
            msg = ""; 
            WebDriverWait wait = new WebDriverWait(Surucu.Driver, TimeSpan.FromSeconds(LinkGlobals.MaxWait));
            try
            {
                switch (type)
                {
                    case "x":
                        CaptchaImg = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(element)));
                        break;
                    case "t":
                        CaptchaImg = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.TagName(element)));
                        break;
                    case "c":
                        CaptchaImg = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName(element)));
                        break;
                    case "s":
                        CaptchaImg = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(element)));
                        break;
                    case "n":
                        CaptchaImg = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name(element)));
                        break;
                    case "i":
                        CaptchaImg = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(element)));
                        break;
                    default:
                        return String.Empty;
                }
                return CaptchaImg.GetAttribute("src");
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return String.Empty;
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
                // handle this exception, or just ignore it
            }
            try
            {
                WebDriverWait wait = GetWait();
                wait.Until(e => ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("return document.readyState").Equals("complete"));
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }
        
    }
}
