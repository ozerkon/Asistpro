using Models.Common;
using SgkAssistant.Helpers;
using SgkAssistant.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace SgkAssistant.Forms.Defs
{
    public partial class FPackage : RadForm
    {
        bool allowStart = false;
        bool allowAddUser = false;
        bool readyFinish = false;

        Users usr = null;
        public FPackage()
        {
            RadWizardLocalizationProvider.CurrentProvider = new MyWizardLocalizationProvider();
            InitializeComponent();
        }

        private void fPackage_Load(object sender, EventArgs e)
        {
            BringToFront();
            List<RadButton> radButtons = new List<RadButton>() { btnLocalDBTest, btnAdd, btnClear, btnRefreshConnection };
            foreach (RadButton btn in radButtons)
            {
                btn.ButtonElement.BorderElement.Visibility = ElementVisibility.Visible;
                btn.ButtonElement.BorderElement.ForeColor = System.Drawing.Color.FromArgb(120, 148, 186);
                btn.ButtonElement.ShowBorder = true;
                btn.ForeColor = System.Drawing.Color.FromArgb(21, 66, 139);
                btn.ElementTree.Control.Cursor = Cursors.Hand;
            }
            string msg = "";
            
            radWizard.NextButton.Enabled = false;
            allowStart = false;
            lblLicenceMessage.Text = string.Empty;
            lblLicenceMessage.Visible = false;

            cbAuthority.Enabled = false;
            cbAuthority.SelectedIndex = 0;

            texImageTyperz.Enabled = false;
            texFreeOcr.Text = "K82964397288957";
            usr = IOC.UserDataService.GetUser(1, out msg);
            if (usr != null)
            {
                texName.Text = usr.Unm;
                texLname.Text = usr.Uln;
                texUname.Text = usr.Un;
                texPass.Text = IOC.UserDataService.DecPass(usr.Up);
                texPass2.Text = IOC.UserDataService.DecPass(usr.Upp);
                cbAuthority.SelectedIndex = (usr.Uy == "Y") ? 0 : 1;
            }
        }

        #region welcome
        private void cbEULA_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (cbEULA.CheckState == CheckState.Unchecked)
            {
                radWizard.NextButton.Enabled = false;
            }
            else
            {
                radWizard.NextButton.Enabled = true;
            }
        }
        #endregion

        #region page2Mysql
        private void rbDataBase_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            lblMessageDB.Text = "";
            if (sender == rbLitedb)
            {
                Settings.Default.dbType = 0;
                GlobalVars.DbType = 0;
                IOC.SetDbBase(GlobalVars.DbType);
                pnlMySQL.Enabled = false;
                radWizard.NextButton.Enabled = true;
                string litedbCs = $@"Filename={Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\SgkAsistan\Data\sgkasistan.db;Connection=shared;";
                GlobalVars.SetLiteDbCs(false);
                IOC.DBBase.SetConLocal(litedbCs);
            }
            else if(sender == rbMysql)
            {
                Settings.Default.dbType = 1;
                GlobalVars.DbType = 1;
                IOC.SetDbBase(GlobalVars.DbType);
                pnlMySQL.Enabled = true;
                radWizard.NextButton.Enabled = false;
            }
            
        }
        private void btnLocalDBTest_Click(object sender, EventArgs e)
        {
            string sv = texMysqlHost.Text.Trim();
            string un = texMysqlUn.Text.Trim();
            string up = texMysqlUp.Text.Trim();
            string pr = texMysqlPr.Text.Trim();
            if(!CheckMysqlFielsd(sv, un, up, pr)) { lblMessageDB.Text = "Lütfen bütün alanları doldurun"; return; }
            string mysqlcs = $"Server='{sv}';Database='information_schema';Uid='{un}';Pwd='{up}';Port={pr};";
            bool result = IOC.DBBase.TestConnection(mysqlcs);
            if (result)
            {
                lblMessageDB.Text = "Mysql sunucusu bağlantısı başarılı";
                GlobalVars.SetMsqcsLocal(sv, "asistan", un, up, pr);
                IOC.DBBase.SetConLocal(mysqlcs);
                Settings.Default.mschostL   = Encrypt.EncryptString(sv, GlobalVars.PassPhrase);
                Settings.Default.mscuidL    = Encrypt.EncryptString(un, GlobalVars.PassPhrase);
                Settings.Default.mscupL     = Encrypt.EncryptString(up, GlobalVars.PassPhrase);
                Settings.Default.mscdbL     = Encrypt.EncryptString("asistan", GlobalVars.PassPhrase);
                Settings.Default.mscprtL    = Encrypt.EncryptString(pr, GlobalVars.PassPhrase);
                Settings.Default.Save();
                radWizard.NextButton.Enabled = true;
            }
            else
            {
                lblMessageDB.Text = "Başarısız! Lütfen ayarlarınızı kontrol edip tekrar deneyiniz.";
            }
        }
        public bool CheckMysqlFielsd(string sv,  string un, string up, string pr)
        {
            if (sv == "")
            {
                texMysqlHost.Focus(); return false;
            }else if (un == "")
            {
                texMysqlUn.Focus(); return false;
            }
            else if(up == "")
            {
                texMysqlUp.Focus(); return false;
            }
            else if(pr == "")
            {
                texMysqlPr.Focus(); return false;
            }return true;
        }
        private void texMysqlPr_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || e.KeyChar == '\b' || Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        #endregion

        #region page3Licence
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
            if (e.KeyCode != Keys.Back && e.KeyCode != Keys.Left && e.KeyCode != Keys.Right && e.KeyCode != Keys.Delete)
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
            if (e.Button == MouseButtons.Right)
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
            if (!xc.Contains(texCode1.Text.Substring(2,1)) || !xc.Contains(texCode1.Text.Substring(3, 1)) || !xc.Contains(texCode2.Text.Substring(2, 1)) || !xc.Contains(texCode2.Text.Substring(3, 1)) || !xc.Contains(texCode3.Text.Substring(2, 1)) || !xc.Contains(texCode3.Text.Substring(3, 1)) || !xc.Contains(texCode4.Text.Substring(2, 1)) || !xc.Contains(texCode4.Text.Substring(3, 1)) || !xc.Contains(texCode5.Text.Substring(2, 1)) || !xc.Contains(texCode5.Text.Substring(3, 1)) )
            {
                return 5; // Lisans anahtarı algoritması hatalı (rastgele lisans girildi)
            }
            string sc = PackageHelper.DecryptHc(hc);
            Sapkt pKc = new Sapkt(); int pkcDb;
            (pKc , pkcDb) = IOC.PkcData.CheckPkc(e, p, hc, out msg);
            if (pKc != null &&  pkcDb == 1) // kullanıcı var
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
                pk.Pps = IOC.PkcData.EncPass( p ); // password
                pk.Pla = hc; // lisans anahtarı,  hexadecimal code
                
                #region regControl

                string fd = "";
                int mounth = Convert.ToInt32(sc.Substring(4, 2));
                if (mounth <= 1) PkcGlobals.IsDemoInstalling = true;
                
            
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
                List<Links> lstLink = IOC.LinksDataService.GetAllLinksFromRs(out msg);
                int addLinksResult = 0;
                foreach (Links l in lstLink)
                {
                    addLinksResult += IOC.LinksDataService.AddLink(l, out msg);
                }
                if (addPkcResult == 1) {
                    
                    if ( mounth == 1)
                    {
                        if (WinHelpers.SetAccountInfo(true, pk.Ptr, fd)) return 1; else return 0;
                    }
                    else
                    {
                        if(WinHelpers.SetAccountInfo(false, pk.Ptr, fd )) return 1; else return 0;
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
        private int AddDemoLicence()
        {
            string msg = "";
            string hc = "H4BDHK3FHK40HK3DHKA705";
            string sc = PackageHelper.DecryptHc(hc);
            string fd = "";
            string mac = WinHelpers.GetMacAdress();
            string discid = WinHelpers.DiskInfo();  // harddisk seri no
            Settings.Default.discid = discid;
            Package pk = new Package();
           
            pk.Ptr = NtpClient.Complication(DateTime.Now, out msg);
            pk.Psc = sc; // short code
            pk.Pep = texEmail.Text.Trim();  // eposta
            pk.Pps = IOC.PkcData.EncPass("sinerjia.net"); // password
            pk.Pla = hc; // lisans anahtarı,  hexadecimal code
            Settings.Default.sgscEnc = "Gv*6BF37hgPB586esD?hv?_MF&ra@Ps6zzvpCefwxWGQr$DS-bQLZdQpW$8zvxGN";
            Demo demo = IOC.PkcData.CheckDemoPkc(discid, out msg);
            
            if (demo == null) // uzak veri tabanı hatası
            {
                return 4;
            }
            
            if (demo.Discid == string.Empty) // ilk kez kuruluyor
            {
                pk.Ptr = NtpClient.Complication(DateTime.Now, out msg);
                fd = NtpClient.Complication(DateTime.Now.AddDays(15), out msg);
            }
            else if(demo.Discid != string.Empty && PkcGlobals.EndDate == string.Empty) // diski formatlamış, yeniden kuruyor
            {
                pk.Ptr = demo.Sat;
                fd = NtpClient.Complication(NtpClient.DeComplicationSd(pk.Ptr).AddDays(15), out msg).ToString();
            }
            else if (demo.Discid != string.Empty && PkcGlobals.EndDate != string.Empty) // programı ve veri tabanını silmiş, yeniden kuruyor
            {
                pk.Ptr = NtpClient.Complication(NtpClient.DeComplicationSd(PkcGlobals.EndDate).AddDays(-15), out msg).ToString();
                fd = PkcGlobals.EndDate;
            }
            demo.Discid = discid;
            demo.Mac = mac;
            demo.Sat = pk.Ptr;
            demo.Ubt = fd;
            PackageHelper.Ubt = demo.Ubt;
            PackageHelper.Sat = pk.Ptr;
            Settings.Default.Save();

            int demoResult = demo.Id == 0 ? IOC.PkcData.AddDemoPkc(demo, out msg) : IOC.PkcData.UpdateDemoPkc(demo, out msg);
            if(demoResult == 0) { return 4; }; // uzak veri tabanı hatası
            int addPkcResult = IOC.PkcData.AddPkc(pk, out msg);

            List<Links> lstLink = IOC.LinksDataService.GetAllLinksFromRs(out msg);
            if(lstLink == null) { return 4; }; // uzak veri tabanı hatası
            int addLinksResult = 0;
            foreach (Links l in lstLink)
            {
                addLinksResult += IOC.LinksDataService.AddLink(l, out msg);
            }
            if (addPkcResult == 1)
            {
                if (WinHelpers.SetAccountInfo(true, pk.Ptr, fd)) return 1; else return 0; // 1: regedit kaydı yapıldı, 2: regedit kaydı yapılamadı
            }
            else
            {
                return 0; // yerel veri tabanına kayıt yapılamadı
            }
            
        }
        private void btnRefreshConnection_Click(object sender, EventArgs e)
        {
            string msg = "";
            CheckConn(out msg);
        }
        private void CheckConn(out string msg)
        {
            msg = "";
           
            if (!NtpClient.GetTime(out msg)) 
            { 
                btnRefreshConnection.Visible = true;
            } 
            else 
            { 
                btnRefreshConnection.Visible = false;
            }
            lblLicenceMessage.Text = msg;
        }
        #endregion

        #region page4User
        private bool AddUser(out string msg)
        {
            msg = "";
            try
            {
                if (texUname.Text.Trim().Length == 0)
                {
                    lblUname.Visible = true; allowAddUser = false;
                }
                else if (texPass.Text.Trim().Length == 0)
                {
                    lblPass.Visible = true; allowAddUser = false;
                }
                else if (texPass2.Text.Trim().Length == 0)
                {
                    lblPass2.Visible = true; allowAddUser = false;
                }
                else if (texPass.Text.Trim() != texPass2.Text.Trim())
                {
                    lblPass2.Visible = true; allowAddUser = false;
                }
                else if (cbAuthority.SelectedIndex != 0 && cbAuthority.SelectedIndex != 1)
                {
                    lblAuthority.Visible = true; allowAddUser = false;
                }
                lblUserMessage.Visible = true;
                if (!allowAddUser)
                {
                    msg = "Lütfen formu eksiksik olarak doldurup tekrar deneyin";
                    return false;
                }
                else
                {
                    Users u = new Users()
                    {
                        Unm = texName.Text.Trim(),
                        Uln = texLname.Text.Trim(),
                        Un = texUname.Text.Trim(),
                        Up = IOC.UserDataService.EncPass(texPass.Text.Trim()),
                        Upp = IOC.UserDataService.EncPass(texPass2.Text.Trim()),
                        Uy = (cbAuthority.SelectedIndex == 0) ? "Y" : "K",
                        Hc = NtpClient.Complication(DateTime.Now, out msg)
                    };
                    (int checkUname, int uid) = IOC.UserDataService.ExistUser(u.Un, out msg);
                    switch (checkUname)
                    {
                        case 2:
                            msg = $"Veritabanı hatası, lütfen tekrar deneyiniz";
                            return false;
                        case 0:
                        case 1:
                            u.Id = uid;
                            int result = IOC.UserDataService.AddUser(u, out msg);
                            if (result == 1)
                            {
                                
                                Users newAdded =  IOC.UserDataService.GetUserByUn(u.Un, out msg);
                                UserPrm prm = new UserPrm() {
                                    Sid = Guid.NewGuid().ToString(),
                                    Fs = newAdded.Id,
                                    Cd = newAdded.Cd,
                                    HideBrowser = 1,
                                    AutoCaptcha = 1,
                                    DefaultBrowser = 1,
                                    KeepBrowser = 1,
                                    CloseAllBrowsers = 1,
                                    Sira = 1,
                                    Cid = newAdded.Uy == "Y" ? 1 : 0,
                                    Cid2 = newAdded.Uy == "Y" ? 1 : 0,
                                    Sp = newAdded.Uy == "Y" ? 1 : 0,
                                    Cp = newAdded.Uy == "Y" ? 1 : 0,
                                    Gun = newAdded.Uy == "Y" ? 1 : 0,
                                    Gp = newAdded.Uy == "Y" ? 1 : 0,
                                    Gs = newAdded.Uy == "Y" ? 1 : 0,
                                    Scd1 = 1,
                                    Scd2 = 1,
                                    Scd3 = 1,
                                    Scd4 = 1,
                                    Scd5 = 1
                                };
                                IOC.PkcData.SetUserPrm(prm, out msg);

                                texName.Focus();
                                if (cbAuthority.SelectedIndex == 0)
                                {
                                    btnAdd.Enabled = false;
                                }
                                msg = $"{u.Un} adlı kullanıcı eklendi";
                                return true;
                            }
                            else if (result == 2)
                            {
                                msg = $"{u.Un} adlı kullanıcının bilgileri değiştirildi";
                                return true;
                            }
                            else
                            {
                                msg = $"Veritabanı hatası, lütfen tekrar deneyiniz";
                                return false;
                            }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                msg = $"Veritabanı hatası, lütfen tekrar deneyiniz {ex.Message.ToString()}";
                return false;
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string msg = "";
            if (AddUser(out msg)) { readyFinish = true; }
            lblUserMessage.Text = msg;
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            texName.Clear();
            texLname.Clear();
            texUname.Clear();
            texPass.Clear();
            texPass2.Clear();
        }
        private void GetFocus(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(RadTextBox))
            {
                RadTextBox tx = sender as RadTextBox;
                tx.SelectAll();
                //tx.Focus();
            }
        }
        private void texName_KeyUp(object sender, KeyEventArgs e)
        {
            Regex rx = new Regex(@"^[a-zA-Z ğüşıöçĞÜŞİÖÇ]{2,20}$");
            if (!rx.IsMatch(texName.Text.Trim()))
            {
                lblName.Visible = true; allowAddUser = false;
            }
            else
            {
                lblName.Visible = false; allowAddUser = true;
            }
        }
        private void texLname_KeyUp(object sender, KeyEventArgs e)
        {
            Regex rx = new Regex(@"^[a-zA-Z ğüşıöçĞÜŞİÖÇ]{2,20}$");
            if (!rx.IsMatch(texLname.Text.Trim()))
            {
                lblLname.Visible = true; allowAddUser = false;
            }
            else
            {
                lblLname.Visible = false; allowAddUser = true;
            }
        }
        private void texUname_KeyUp(object sender, KeyEventArgs e)
        {
            if (texUname.Text.Trim().Length == 0)
            {
                lblUname.Visible = true; allowAddUser = false;
            }
            else
            {
                lblUname.Visible = false; allowAddUser = true;
            }
        }
        private void texPass_KeyUp(object sender, KeyEventArgs e)
        {
            if (texPass.Text.Trim().Length == 0)
            {
                lblPass.Visible = true; allowAddUser = false;
            }
            else
            {
                lblPass.Visible = false; allowAddUser = true;
            }
        }
        private void texPass2_KeyUp(object sender, KeyEventArgs e)
        {
            if (texPass2.Text.Trim().Length == 0)
            {
                lblPass2.Visible = true; allowAddUser = false;
            }
            else
            {
                lblPass2.Visible = false; allowAddUser = true;
            }
        }
        private void cbAuthority_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (cbAuthority.SelectedIndex != 0 && cbAuthority.SelectedIndex != 1)
            {
                lblAuthority.Visible = true; allowAddUser = false;
            }
            else
            {
                lblAuthority.Visible = false; allowAddUser = true;
            }
        }
        private void texUname_TextChanged(object sender, EventArgs e)
        {
            if (usr != null && usr.Id == 1)
            {
                string tempUn = usr.Un;
                if (texUname.Text.Trim() != tempUn)
                {
                    cbAuthority.SelectedIndex = 1;
                }
                else
                {
                    cbAuthority.SelectedIndex = 0;
                }
            }
        }
        private void lblUserMessage_TextChanged(object sender, EventArgs e)
        {
            if (lblUserMessage.Text == String.Empty)
            {
                lblUserMessage.Visible = false;
            }
            else
            {
                lblUserMessage.Visible = true;
            }
        }
        #endregion

        #region general
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
        private void radWizard_SelectedPageChanged(object sender, SelectedPageChangedEventArgs e)
        {
            string msg = "";
            if (e.SelectedPage == radWizard.Pages[1])
            {
                if (rbLitedb.IsChecked) { pnlMySQL.Enabled = false; radWizard.NextButton.Enabled = true; }
                else if(rbMysql.IsChecked) { pnlMySQL.Enabled = true; radWizard.NextButton.Enabled = false; }
                lblMessageDB.Text = string.Empty;
            }
            if (e.SelectedPage == radWizard.Pages[2])
            {
                lblLicenceMessage.Text = String.Empty;
                
                CheckConn(out msg);
            }
            else if (e.SelectedPage == radWizard.Pages[3])
            {
                lblUserMessage.Text = String.Empty;
            }
        }
        private void radWizard_Next(object sender, WizardCancelEventArgs e)
        {
            string msg = "";
            if (radWizard.SelectedPage == radWizard.Pages[2])
            {
                Cursor = Cursors.WaitCursor;
                allowStart = CheckFour(texCode1) && CheckFour(texCode2) && CheckFour(texCode3) && CheckFour(texCode4) && CheckFour(texCode5) && CheckTwo(texCode6) && CheckEmail(texEmail.Text.Trim()) && CheckPass();
                if (allowStart == false)
                {
                    lblLicenceMessage.Text = "Lütfen formdaki hataları düzeltip tekrar deneyiniz.";
                    e.Cancel = true;
                }
                if (allowStart == true)
                {
                    int cla = -1;
                    if (cbDemo.Checked)
                    {
                        cla = AddDemoLicence();
                    }
                    else
                    {
                        cla = CheckLicense();
                    }
                    switch (cla)
                    {
                        case 0:
                            e.Cancel = true;
                            lblLicenceMessage.Text = $"Kayıt hatası, Asistpro'yu yönetici olarak çalıştırdığınızdan emin olun.";
                            break;
                        case 1:
                            e.Cancel = false; lblUserMessage.Text = string.Empty;
                            break;
                        case 2:
                            e.Cancel = true;
                            lblLicenceMessage.Text = $"Kullanıcı bulunamadı, bilgilerinizi kontrol edip tekrar deneyin.";
                            break;
                        case 3:
                            e.Cancel = true;
                            lblLicenceMessage.Text = $"Geçersiz Lisans Anahtarı, bilgilerinizi kontrol edip tekrar deneyin.";
                            break;
                        case 4:
                            e.Cancel = true;
                            lblLicenceMessage.Text = $"Bağlantı hatası, internet bağlantınızı kontrol edip tekrar deneyin.";
                            break;
                        case 5:
                            e.Cancel = true;
                            lblLicenceMessage.Text = $"Geçersiz Lisans Anahtarı, bilgilerinizi kontrol edip tekrar deneyin.";
                            break;
                        case 6:
                            e.Cancel = true;
                            lblLicenceMessage.Text = $"Bu lisans başka bir bilgisayarda kullanılıyor!";
                            break;
                    }
                }
                Cursor = Cursors.Default;
            }

            else if (radWizard.SelectedPage == radWizard.Pages[4])
            {
                if (rbImageTyperz.CheckState == CheckState.Checked)
                {
                    Settings.Default.solverKey = texImageTyperz.Text.Trim();
                }
                else if (rbFreeOcr.CheckState == CheckState.Checked)
                {
                    Settings.Default.solverKey = texFreeOcr.Text.Trim();
                }
                Settings.Default.Save();
;                if (!readyFinish)
                {
                    e.Cancel = true;
                    lblUserMessage.Text = msg;
                }
            }
        }
        private void radWizard_Previous(object sender, WizardCancelEventArgs e)
        {
            string msg = ""; lblUserMessage.Text = string.Empty;
            if (radWizard.SelectedPage == radWizard.Pages[4])
            {
                if (texUname.Text.Trim() == "" || texLname.Text.Trim() == "" || texUname.Text.Trim() == "" || texPass.Text.Trim() == "" || texPass2.Text.Trim() == "")  return; 
                usr = IOC.UserDataService.GetUserByUn(texUname.Text.Trim(), out msg);
                btnAdd.Enabled = true;
                if (usr.Id == 1)
                {
                    cbAuthority.SelectedIndex = 0;
                }
                else
                {
                    cbAuthority.SelectedIndex = 1;
                }
            }
        }
        private void radWizard_Cancel(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void radWizard_Finish(object sender, EventArgs e)
        {
            Properties.Settings.Default.tabOrderEvizite = new TabOrder() { Index = 0, TabName = "rtEvizite", IsHide = false };
            Properties.Settings.Default.tabOrderLinks = new TabOrder() { Index = 1, TabName = "rtLinks", IsHide = false };
            Properties.Settings.Default.tabOrderHesapDurumu = new TabOrder() { Index = 2, TabName = "rtHesapDurumu", IsHide = false };
            Properties.Settings.Default.tabOrderHizmetListe = new TabOrder() { Index = 3, TabName = "rtHizmetListe", IsHide = false };
            Properties.Settings.Default.tabOrderIgic = new TabOrder() { Index = 4, TabName = "rtIgic", IsHide = false };
            Properties.Settings.Default.tabOrderLastName = new TabOrder() { Index = 5, TabName = "rtLastName", IsHide = false };
            Properties.Settings.Default.tabOrderSettings = new TabOrder() { Index = 6, TabName = "rtOptions", IsHide = false };
            Properties.Settings.Default.tabOrderTesvik = new TabOrder() { Index = 7, TabName = "rtTesvik", IsHide = false };
            Properties.Settings.Default.tabOrderYillik = new TabOrder() { Index = 8, TabName = "rtYillik", IsHide = false };
            Settings.Default.Save();
            this.DialogResult = DialogResult.OK;
        }

        #endregion
        private void radWizard_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            
        }
        private void btnSolverHelp_Click(object sender, EventArgs e)
        {
            FSolverHelp f = new FSolverHelp();
            f.ShowDialog();
        }
        private void rbCaptcha_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (sender == rbImageTyperz)
            {
                texFreeOcr.Enabled = false;
                texImageTyperz.Enabled = true;
            }
            else if (sender == rbFreeOcr)
            {
                texFreeOcr.Enabled = true;
                texImageTyperz.Enabled = false;
            }
        }
        private void cbDemo_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDemo.Checked)
            {
                texCode1.Text = "H4BD"; texCode1.Enabled = false;
                texCode2.Text = "HK3F"; texCode2.Enabled = false;
                texCode3.Text = "HK40"; texCode3.Enabled = false;
                texCode4.Text = "HK3D"; texCode4.Enabled = false;
                texCode5.Text = "HKA7"; texCode5.Enabled = false;
                texCode6.Text = "05";   texCode6.Enabled = false;
                texEmail.Text = "demo@sgkasistan.com"; texEmail.Enabled = false;
                texLicencePass.Text = "sinerjia.net"; texLicencePass.Enabled = false;
                PkcGlobals.IsDemoInstalling = true;
            }
            else
            {
                texCode1.Text = ""; texCode1.Enabled = true;
                texCode2.Text = ""; texCode2.Enabled = true;
                texCode3.Text = ""; texCode3.Enabled = true;
                texCode4.Text = ""; texCode4.Enabled = true;
                texCode5.Text = ""; texCode5.Enabled = true;
                texCode6.Text = ""; texCode6.Enabled = true;
                texEmail.Text = ""; texEmail.Enabled = true;
                texLicencePass.Text = ""; texLicencePass.Enabled = true;
                PkcGlobals.IsDemoInstalling = false;
            }
        }
    }
}
