using Models.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WebDriverX;

namespace SGKServices.Captcha
{
    public class CaptchaService : TrmBase
    {
        public IWebElement CaptchaPic { get; set; }

        public Bitmap TakeScreenshot(IWebElement element, out string msg)
        {
            msg = "";
            try
            {
                if (element == null) return null;
                TryCreateFolder(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\SgkAsistan\Temp", false);
                string fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg";
                CaptchaFilePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\SgkAsistan\Temp\" + fileName;
                SearchReport.LastCaptchaPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\SgkAsistan\Temp\" + fileName;
                byte[] byteArray = ((ITakesScreenshot)Surucu.Driver).GetScreenshot().AsByteArray;
                Bitmap screenshot = new Bitmap(new MemoryStream(byteArray));
                Rectangle croppedImage = new Rectangle(element.Location.X, element.Location.Y, element.Size.Width, element.Size.Height);
                screenshot = screenshot.Clone(croppedImage, screenshot.PixelFormat);
                screenshot.Save(CaptchaFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                Bitmap bit = new Bitmap(CaptchaFilePath);
                GlobalVars.ImageWidth = element.Size.Width;
                return bit;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }

        public void RefreshCaptchaImage(PictureBox pb, string type, string value, out string msg)
        {
            msg = "";
            try
            {
                WebDriverWait wait = GetWait();
                if(!ExecuteScript(type, value)) wait.Until(e => ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("return document.readyState").Equals("complete"));
                switch (type)
                {
                    case "x":
                        CaptchaPic = wait.Until(e => e.FindElement(By.XPath(value)));
                        break;
                    case "i":
                        CaptchaPic = wait.Until(e => e.FindElement(By.Id(value)));
                        break;
                    case "n":
                        CaptchaPic = wait.Until(e => e.FindElement(By.Name(value)));
                        break;
                    case "s":
                        CaptchaPic = wait.Until(e => e.FindElement(By.CssSelector(value)));
                        break;
                    case "c":
                        CaptchaPic = wait.Until(e => e.FindElement(By.ClassName(value)));
                        break;
                    case "t":
                        CaptchaPic = wait.Until(e => e.FindElement(By.TagName(value)));
                        break;
                }
                pb.Image = TakeScreenshot(CaptchaPic, out msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }

    }

}
