using Models.Common;
using SgkAssistant.Helpers;
using SgkAssistant.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace SgkAssistant.Forms.Common
{
    partial class FLicence : Telerik.WinControls.UI.RadForm
    {
        bool allowStart = false;
        public FLicence()
        {
            InitializeComponent();

        }
        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            RadTextBox t = sender as RadTextBox;
            if (e.KeyCode == System.Windows.Forms.Keys.V && e.Modifiers == System.Windows.Forms.Keys.Control)
            {
                RemoveSpaces(t);
            }
            //Handle Ctrl+Ins
            if (e.KeyCode == System.Windows.Forms.Keys.Control && e.KeyCode == System.Windows.Forms.Keys.Insert)
            {
                RemoveSpaces(t);
            }
        }
        private void RemoveSpaces(RadTextBox t)
        {
            t.Text = t.Text.Replace(" ", string.Empty);
        }
        private void texCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == '\b' || e.KeyChar == 131072)  // Allowing BackSpace character
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        private void texCode_KeyUp(object sender, KeyEventArgs e)
        {
            RadTextBox t = sender as RadTextBox;
            t.Text = t.Text.ToUpper();
            if (e.KeyCode != System.Windows.Forms.Keys.Back && e.KeyCode != System.Windows.Forms.Keys.Left && e.KeyCode != System.Windows.Forms.Keys.Right && e.KeyCode != System.Windows.Forms.Keys.Delete)
            {
                t.SelectionStart = t.Text.Length;
                t.SelectionLength = 0;
            }
            switch (t.Name)
            {
                case "texCode1": if (t.TextLength == 4) { texCode2.Focus(); } break;
                case "texCode2": if (t.TextLength == 4) { texCode3.Focus(); } break;
                case "texCode3": if (t.TextLength == 4) { texCode4.Focus(); } break;
                case "texCode4": if (t.TextLength == 4) { texCode5.Focus(); } break;
                case "texCode5": if (t.TextLength == 4) { texCode6.Focus(); } break;
                case "texCode6": if (t.TextLength == 2) { texEmail.Focus(); } break;
            }
        }
        private void textBox_MouseDown(object sender, MouseEventArgs e)
        {
            RadTextBox t = sender as RadTextBox;
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                t.ContextMenu = new ContextMenu();
            }
        }
        private bool CheckFour(RadTextBox t)
        {
            Regex regex = new Regex(@"[a-zA-z0-9]{4}$");
            if (t.Text.Length == 0)
            {
                t.TextBoxElement.Border.ForeColor = Color.MediumVioletRed;
                return false;
            }
            else if (!regex.IsMatch(t.Text))
            {
                t.ForeColor = Color.Red;
                return false;
            }
            return true;
        }
        private bool CheckTwo(RadTextBox t)
        {
            Regex regex = new Regex(@"[0-9]{2}$");
            if (t.Text.Length == 0)
            {
                t.TextBoxElement.Border.ForeColor = Color.MediumVioletRed;
                return false;
            }
            else if (!regex.IsMatch(t.Text))
            {
                t.ForeColor = Color.Red;
                return false;
            }
            return true;
        }
        private bool CheckEmail(string email)
        {
            try
            {
                if (email.Length > 0)
                {
                    MailAddress m = new MailAddress(email);
                    return true;
                }
                else
                {
                    texEmail.TextBoxElement.BorderColor = Color.MediumVioletRed;
                    return false;
                }
            }
            catch (FormatException)
            {
                texEmail.ForeColor = Color.Red;
                return false;
            }
        }
        private bool CheckPass()
        {
            if (texLicencePass.TextLength == 0)
            {
                texLicencePass.TextBoxElement.BorderColor = Color.MediumVioletRed;
                return false;
            }
            else if (texLicencePass.TextLength < 6)
            {
                texLicencePass.ForeColor = Color.Red;
                return false;
            }
            return true;
        }
        private void texCode_Enter(object sender, EventArgs e)
        {
            RadTextBox t = sender as RadTextBox;
            t.ForeColor = Color.Black;
            t.TextBoxElement.Border.ForeColor = Color.FromArgb(255, 156, 189, 232);
        }
        private void text_Enter(object sender, EventArgs e)
        {
            RadTextBoxControl t = sender as RadTextBoxControl;
            t.ForeColor = Color.Black;
            t.TextBoxElement.BorderColor = Color.FromArgb(255, 156, 189, 232);
        }
        private void lblLicenceMessage_TextChanged(object sender, EventArgs e)
        {
            if (lblLicenceMessage.Text == string.Empty)
            {
                lblLicenceMessage.Visible = false;
            }
            else
            {
                lblLicenceMessage.Visible = true;
            }
        }
        private int CheckLicense()
        {
            string msg = "";
            string hc = $"{texCode1.Text}{texCode2.Text}{texCode3.Text}{texCode4.Text}{texCode5.Text}{texCode6.Text}";
            string e = texEmail.Text.Trim();
            string p = texLicencePass.Text.Trim();
            string xc = "ABCDEF0123456789";
            bool overRide = false;
            if (!xc.Contains(texCode1.Text.Substring(2, 1)) || !xc.Contains(texCode1.Text.Substring(3, 1)) || !xc.Contains(texCode2.Text.Substring(2, 1)) || !xc.Contains(texCode2.Text.Substring(3, 1)) || !xc.Contains(texCode3.Text.Substring(2, 1)) || !xc.Contains(texCode3.Text.Substring(3, 1)) || !xc.Contains(texCode4.Text.Substring(2, 1)) || !xc.Contains(texCode4.Text.Substring(3, 1)) || !xc.Contains(texCode5.Text.Substring(2, 1)) || !xc.Contains(texCode5.Text.Substring(3, 1)))
            {
                return 5; // Lisans anahtarı algoritması hatalı (rastgele lisans girildi)
            }
            string sc = PackageHelper.DecryptHc(hc);
            Sapkt pKc = new Sapkt(); int pkcDb;
            (pKc, pkcDb) = IOC.PkcData.CheckPkc(e, p, hc, out msg);
            if (pKc != null && pkcDb == 1) // kullanıcı var
            {
                string mac = WinHelpers.GetMacAdress();
                string discid = WinHelpers.DiskInfo();  // harddisk seri no
                Settings.Default.discid = discid;
                Package pk = new Package();
                if (pKc.Discid != null && pKc.Discid != discid && pKc.Mac != null && pKc.Mac != mac)
                {
                    return 6; // lisans başka bilgisayarda kullanılıyor
                }
                if (!pKc.Active) // lisans ilk kez kullanılıyor, satın alma tarihini bugün olarak değiştir
                {
                    pKc.Mac = mac;
                    pKc.Discid = discid;
                    pKc.Active = true;
                    overRide = false;
                    pk.Ptr = NtpClient.Complication(DateTime.Now, out msg);
                }
                else // lisans zaten kullanımda, satın alma tarihini değiştirme
                {
                    pk.Ptr = pKc.Sat;
                    overRide = true;
                }
                pk.Psc = sc; // short code
                pk.Pep = e;  // eposta
                pk.Pps = IOC.PkcData.EncPass(p); // password
                pk.Pla = hc; // lisans anahtarı,  hexadecimal code

                #region regControl

                string fd = "";
                int mounth = Convert.ToInt32(sc.Substring(4, 2));

                // harddiski formatlamış (ya da regedit kaydını ve veri tabanını silmiş) yeniden kurmaya çalışıyor
                if (overRide && PkcGlobals.IsDemoInstalling == false)
                {
                    Settings.Default.sgscEnc = "Gv*6BF37hgPB586esD?hv?_MF&ra@Ps6zzvpCefwxWGQr$DS-bQLZdQpW$8zvxGN";
                    fd = NtpClient.Complication(NtpClient.DeComplicationSd(pk.Ptr).AddMonths(mounth), out msg).ToString();
                }
                // daha önce kurmuş, yeniden tam sürüm kuruyor
                else if (PkcGlobals.EndDate != string.Empty && PkcGlobals.HasDemoInstalled == false && PkcGlobals.IsDemoInstalling == false)
                {
                    pk.Ptr = NtpClient.Complication(NtpClient.DeComplicationSd(PkcGlobals.EndDate).AddMonths(-mounth), out msg).ToString();
                }

                // daha önce kurmamış, sıfırdan tam sürüm kuruyor
                else if (PkcGlobals.EndDate == string.Empty && PkcGlobals.IsDemoInstalling == false)
                {
                    Settings.Default.sgscEnc = "Gv*6BF37hgPB586esD?hv?_MF&ra@Ps6zzvpCefwxWGQr$DS-bQLZdQpW$8zvxGN";
                    pk.Ptr = NtpClient.Complication(DateTime.Now, out msg);
                    fd = NtpClient.Complication(DateTime.Now.AddMonths(mounth), out msg);

                }
                pKc.Sat = pk.Ptr;
                pKc.Ubt = fd;
                PackageHelper.Ubt = pKc.Ubt;
                PackageHelper.Sat = pk.Ptr;
                Settings.Default.Save();
                #endregion
                int addPkcResult = IOC.PkcData.AddPkc(pk, out msg);
                int addSapktResult = IOC.PkcData.UpdatePkc(pKc, out msg);

                if (addPkcResult == 1)
                {

                    if (mounth == 1)
                    {
                        if (WinHelpers.SetAccountInfo(true, pk.Ptr, fd)) return 1; else return 0;
                    }
                    else
                    {
                        if (WinHelpers.SetAccountInfo(false, pk.Ptr, fd)) return 1; else return 0;
                    }
                }
                else
                {
                    return 0; // yerel veri tabanına kayıt yapılamadı
                }
            }
            else if (pKc == null && pkcDb == 2) // kullanıcı yok
            {
                return 2;
            }
            else if (pKc == null && pkcDb == 3)// geçersiz lisans (algoritma doğru ama; lisans veritabanında yok)
            {
                return 3;
            }
            else // veri tabanı bağlantı hatası
            {
                return 4;
            }
        }
        private void btnChangeLicence_Click(object sender, EventArgs e)
        {
            if (texEmail.Text.Trim() == "demo@asistpro.com")
            {
                lblLicenceMessage.Text = PackageHelper.Ay <= 1 ? "Zaten bu lisansı kullanıyorsunuz" : "Asistpro, tam sürümden demo sürüme indirgenemez"; return;
            }
            Cursor = Cursors.WaitCursor;
            allowStart = CheckFour(texCode1) && CheckFour(texCode2) && CheckFour(texCode3) && CheckFour(texCode4) && CheckFour(texCode5) && CheckTwo(texCode6) && CheckEmail(texEmail.Text.Trim()) && CheckPass();
            if (allowStart == false)
            {
                lblLicenceMessage.Text = "Lütfen formdaki hataları düzeltip tekrar deneyiniz.";
            }
            if (allowStart == true)
            {
                int cla = CheckLicense();
                switch (cla)
                {
                    case 0:
                        lblLicenceMessage.Text = $"Kayıt hatası, Asistpro'yu yönetici olarak çalıştırdığınızdan emin olun.";
                        break;
                    case 1:
                        lblLicenceMessage.Text = $"Lisans anahtarı değiştirildi, Asistpro yeniden başlatılıyor... ";
                        btnCancel.Enabled = false;
                        btnChangeLicence.Enabled = false;
                        break;
                    case 2:
                        lblLicenceMessage.Text = $"Kullanıcı bulunamadı, bilgilerinizi kontrol edip tekrar deneyin.";
                        break;
                    case 3:
                        lblLicenceMessage.Text = $"Geçersiz Lisans Anahtarı, bilgilerinizi kontrol edip tekrar deneyin.";
                        break;
                    case 4:
                        lblLicenceMessage.Text = $"Bağlantı hatası, internet bağlantınızı kontrol edip tekrar deneyin.";
                        break;
                    case 5:
                        lblLicenceMessage.Text = $"Geçersiz Lisans Anahtarı, bilgilerinizi kontrol edip tekrar deneyin.";
                        break;
                    case 6:
                        lblLicenceMessage.Text = $"Bu lisans başka bir bilgisayarda kullanılıyor!";
                        break;
                }
            }
            Cursor = Cursors.Default;
            if (btnCancel.Enabled == false)
            {
                string appPath = Path.GetDirectoryName(Application.ExecutablePath);
                string path = $@"{appPath}\Asistpro.exe";
                Thread.Sleep(5000);
                System.Diagnostics.Process.Start(path);
                Application.Exit();
            }
        }
        private void fLicence_Load(object sender, EventArgs e)
        {
            BringToFront();
            List<RadButton> radButtons = new List<RadButton>() { btnChangeLicence, btnCancel };
            foreach (RadButton btn in radButtons)
            {
                btn.ButtonElement.BorderElement.Visibility = ElementVisibility.Visible;
                btn.ButtonElement.BorderElement.ForeColor = System.Drawing.Color.FromArgb(120, 148, 186);
                btn.ButtonElement.ShowBorder = true;
                btn.ForeColor = System.Drawing.Color.FromArgb(21, 66, 139);
                btn.ElementTree.Control.Cursor = Cursors.Hand;
            }
        }
    }
}
