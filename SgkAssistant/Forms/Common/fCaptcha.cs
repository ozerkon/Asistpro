using Models.Common;
using OpenQA.Selenium;
using SGKServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using WebDriverX;

namespace SgkAssistant.Forms.Sgk
{
    public partial class FCaptcha : Form
    {
        private string CaptchaText { get; set; } = string.Empty;
        private bool UserCancelled { get; set; } = false;
        private enum SgkPageEnum { Vizite, Isegiriscikis, Links, Isverensistemi }

        private string RefreshBtnType { get; set; } = "x"; // x = XPath, i = Id , n = Name, s = CssSelector, c = ClassName, t = TagName
        private string RefreshBtnValue { get; set; } = ""; // üstteki tipe göre alınan değer
        bool btnRc = false;
        int tryCount = 0;
        public FCaptcha(Bitmap image, string sgkPageType, bool isThereBtnRefreshCaptca = false)
        {
            CheckForIllegalCrossThreadCalls = false;
            
            InitializeComponent();
            pbCaptcha.Image = image;
            btnRc = isThereBtnRefreshCaptca;
            InitialSettings();
        }
        public void BringFront()
        {
            TopMost = true;
            BringToFront();
            txtCaptcha.SelectAll();
            txtCaptcha.Focus();
        }
        public void InitialSettings()
        {
            btnOK.Enabled = false;
            List<RadButton> radButtons = new List<RadButton>() { btnOK, btnCancel };
            foreach (RadButton btn in radButtons)
            {
                btn.ButtonElement.BorderElement.Visibility = ElementVisibility.Visible;
                btn.ButtonElement.BorderElement.ForeColor = Color.FromArgb(120, 148, 186);
                btn.ButtonElement.ShowBorder = true;
                btn.ForeColor = Color.FromArgb(21, 66, 139);
                btn.ElementTree.Control.Cursor = Cursors.Hand;
            }
            if (pbCaptcha.Image != null && pbCaptcha.Image.Size != null && pbCaptcha.Image.Width > 0 && pbCaptcha.Image.Height > 0)
            {
                pbCaptcha.Width = pbCaptcha.Image.Width;
                pbCaptcha.Height = pbCaptcha.Image.Height;
                pbCaptcha.Left = (454 - pbCaptcha.Width) / 2;
                pbCaptcha.Top = (100 - pbCaptcha.Height) / 2;
                txtCaptcha.Select();
                pbCaptcha.Refresh();
            }

            SearchReport.LoginMessage = String.Empty;
            //switch (sgkPageEnum)
            //{
            //    case SgkPageEnum.Vizite:
            //        radLabel1.Text = $"{IOC.VisitReportService.CompanyName}";
            //        break;
            //    case SgkPageEnum.Isegiriscikis:
            //        radLabel1.Text = $"{IOC.PersonelimDegil.CompanyName}";
            //        break;
            //    case SgkPageEnum.Links:
            //        radLabel1.Text = $"{IOC.SgkLinksService.CompanyName}"; txtCaptcha.Select();
            //        break;
            //    case SgkPageEnum.Isverensistemi:
            //        radLabel1.Text = $"{IOC.TgIsyerleriService.CompanyName}";
            //        RefreshBtnType = "i";
            //        RefreshBtnValue = "reload";
            //        break;
            //    default:
            //        break;
            //}
            radLabel2.Text = "İÇİN OTURUM AÇ";
            if (btnRc == true) { btnRefreshCaptcha.Visible = true; btnRefreshCaptcha.Enabled = true; }
            else { btnRefreshCaptcha.Visible = false; btnRefreshCaptcha.Enabled = false; }
            lblAlert.Text = SearchReport.LoginMessage;
        }
        private void fCaptcha_Load(object sender, EventArgs e)
        {
            Visible = true;
            BringFront();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            UserCancelled = true;
            this.DialogResult = DialogResult.None;
            IOC.VisitReportService.Message = "İşlem kullanıcı tarafından iptal edildi";
            IOC.SgkLinksService.Message = "İşlem kullanıcı tarafından iptal edildi";
            if (pbCaptcha.Image != null) pbCaptcha.Image.Dispose();
            Dispose();
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            btnOK.Enabled = false;
            if (LinkGlobals.Ivd == true && LinkGlobals.IvdSurveyBox == true)
            {
                IWebElement surveyBox = IOC.TrmBase.IsElementEXISTS(By.Id("gen__1927"), 3);
                if (surveyBox != null)
                {
                    surveyBox.Click(); LinkGlobals.IvdSurveyBox = false;
                }
            }
            CaptchaText = txtCaptcha.Text.Trim();
            string msg = "";
            try
            {
                IOC.SgkLinksService.FillPageLoginFields(out msg); 
                IOC.SgkLinksService.CaptchaCode = txtCaptcha.Text.Trim(); 
                if (IOC.SgkLinksService.IsLoginSuccessful(out msg))
                {
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    if (msg == "Kullanıcı adı veya şifreleriniz hatalıdır") { lblAlert.Text = "Kullanıcı adı veya şifreleriniz hatalıdır";  }
                    else if (msg.Contains("iz olmuş")) { lblAlert.Text = "İş yeri iz olmuş, işlem yapılamaz!";  }
                    else if (msg.Contains("sunucularında")) { lblAlert.Text = "SGK sunucularında hata oluştu!";  }
                    else if (msg.Contains("iptal")) {lblAlert.Text = $"<li><strong><span style=\"font-size: 10pt\">Hata: {IOC.SgkLinksService.Message}</span></strong></li>\r\n"; }

                    else if (msg == "Güvenlik kodu hatalı girildi") { lblAlert.Text = "Güvenlik kodu hatalı girildi"; SearchReport.LoginMessage = "Güvenlik kodu hatalı girildi"; }
                        string currentWindowHandle = Surucu.Driver.CurrentWindowHandle;
                        IList<string> totWindowHandles = new List<string>(Surucu.Driver.WindowHandles);
                        foreach (string handle in totWindowHandles)
                        {
                            Surucu.Driver.SwitchTo().Window(handle);
                        }
                        if (pbCaptcha.Image != null)
                        {
                            pbCaptcha.Image.Dispose();
                        }
                        for (int i = 0; i < 2; i++)
                        {
                            IOC.SgkLinksService.FillPageLoginFields(out msg);
                            if (IOC.SgkLinksService.WeCaptchaImg != null) break;
                        }
                        IOC.SgkLinksService.WaitForImageLoad(IOC.SgkLinksService.CaptchaValue, out msg, IOC.SgkLinksService.CaptchaType);
                        Thread.Sleep(500);
                        pbCaptcha.Image = IOC.CaptchaService.TakeScreenshot(IOC.SgkLinksService.WeCaptchaImg, out msg);
                        tryCount = 0;
                        while (pbCaptcha.Image == null)
                        {
                            pbCaptcha.Image = IOC.CaptchaService.TakeScreenshot(IOC.SgkLinksService.WeCaptchaImg, out msg);
                            if (tryCount < 50) { tryCount++; Thread.Sleep(100); }
                            else break;
                        }
                        Thread.Sleep(500);
                        if (pbCaptcha.Image != null && pbCaptcha.Image.Size != null && pbCaptcha.Image.Width > 0 && pbCaptcha.Image.Height > 0)
                        {
                            pbCaptcha.Width = pbCaptcha.Image.Width;
                            pbCaptcha.Height = pbCaptcha.Image.Height;
                            pbCaptcha.Left = (454 - pbCaptcha.Width) / 2;
                            pbCaptcha.Top = (100 - pbCaptcha.Height) / 2;
                            pbCaptcha.Refresh();
                            BringFront();
                        }
                        else { msg = "Güvenlik kodu yüklenemedi!";  }
                        BringFront();
                        DialogResult = DialogResult.None;
                    }
                        
                if (this.DialogResult == DialogResult.None)
                {
                    pbCaptcha.Refresh();
                    lblAlert.Text = SearchReport.LoginMessage != "" ? SearchReport.LoginMessage : lblAlert.Text;
                    BringFront();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                lblAlert.Text = $"{msg}";
            }
            finally
            {
                Application.OpenForms["fBaslangic"].WindowState = FormWindowState.Maximized;
            }

        }
        private void VisitLink()
        {
            System.Diagnostics.Process.Start("https://www.sinerjia.net/");
        }
        private void fCaptcha_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (pbCaptcha.Image != null)
            {
                pbCaptcha.Image.Dispose();
            }

        }
        private void btnRefreshCaptcha_Click(object sender, EventArgs e)
        {
            string msg = ""; string srcBefore = String.Empty; string srcAfter = String.Empty;
            string imgType = IOC.SgkLinksService.CaptchaType;
            string imgValue = IOC.SgkLinksService.CaptchaValue;
            lblAlert.Text = string.Empty;
            try
            {
                if (pbCaptcha.Image != null)
                {
                    pbCaptcha.Image.Dispose();
                }
                Surucu.Driver.Manage().Window.Maximize();
                if (LinkGlobals.Ivd == true)
                {
                    string hndl = Surucu.Driver.CurrentWindowHandle;
                    IWebElement frm = IOC.SgkLinksService.GetElementBy("i", "gen__1062", out msg);
                    Surucu.Driver.SwitchTo().Frame(frm);
                    imgType = "x";
                    imgValue = "/html/body/img";
                    srcBefore = IOC.CommonFuncs.GetImageSrc(imgValue, imgType, out msg);
                    Surucu.Driver.SwitchTo().Window(hndl);
                    IOC.CommonFuncs.ClickWebElement(LinkGlobals.RcbValue, out msg, LinkGlobals.RcbType);
                    Surucu.Driver.SwitchTo().Frame(frm);
                    IOC.CommonFuncs.WaitForPageLoad(out msg);
                    srcAfter = IOC.CommonFuncs.GetImageSrc(imgValue, imgType, out msg);
                    Surucu.Driver.SwitchTo().Window(hndl);
                }
                else
                {
                    srcBefore = IOC.CommonFuncs.GetImageSrc(imgValue, imgType, out msg); 
                    IOC.CommonFuncs.ClickWebElement(LinkGlobals.RcbValue, out msg, LinkGlobals.RcbType);
                    while (srcBefore == IOC.CommonFuncs.GetImageSrc(imgValue, imgType, out msg))
                    {
                        Thread.Sleep(100);
                    }
                    srcAfter = IOC.CommonFuncs.GetImageSrc(imgValue, imgType, out msg); 
                }
                imgType = IOC.SgkLinksService.CaptchaType;
                imgValue = IOC.SgkLinksService.CaptchaValue;
                if (srcAfter != string.Empty && srcBefore != string.Empty && srcAfter != srcBefore)
                {
                    IOC.SgkLinksService.WaitForImageLoad(imgValue, out msg, imgType);
                    bool getCaptchaPic = false; tryCount = 0;
                    while (!getCaptchaPic)
                    {
                        getCaptchaPic = IOC.SgkLinksService.GetCaptchaPicture(imgType, imgValue, out msg);
                        if(tryCount < 50) { tryCount++; Thread.Sleep(100); }
                        else break;
                    }
                    if (getCaptchaPic)
                    {
                        pbCaptcha.Image = IOC.CaptchaService.TakeScreenshot(IOC.SgkLinksService.WeCaptchaImg, out msg);
                        if (pbCaptcha.Image != null && pbCaptcha.Image.Size != null && pbCaptcha.Image.Width > 0 && pbCaptcha.Image.Height > 0)
                        {
                            pbCaptcha.Width = pbCaptcha.Image.Width;
                            pbCaptcha.Height = pbCaptcha.Image.Height;
                            pbCaptcha.Left = (454 - pbCaptcha.Width) / 2;
                            pbCaptcha.Top = (100 - pbCaptcha.Height) / 2;
                            pbCaptcha.Refresh();
                            BringFront();
                        }
                    }
                    lblAlert.Text = "";
                }
                else { lblAlert.Text = "Kod yenilenemedi!"; }
                Surucu.Driver.Manage().Window.Minimize();
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                lblAlert.Text = $"Kod yenilenemedi! Hata:{msg}";
            }
        }
        private void txtCaptcha_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (txtCaptcha.Text.Trim().Length > 0)
                {
                    btnOK.Enabled = true;
                    if (e.KeyCode == System.Windows.Forms.Keys.Enter)
                    {
                        btnOK_Click(null, null);
                    }
                }
                else
                {
                    btnOK.Enabled = false;
                }

            }
            catch (Exception)
            {
                lblAlert.Text = "Lütfen fare ile tıklayınız";
            }
        }
        private void lblAlert_TextChanged(object sender, EventArgs e)
        {
            BringFront();
        }
        private void lblSinerjia_Click(object sender, EventArgs e)
        {
            VisitLink();
        }

    }
}
