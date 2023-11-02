using Models.Common;
using SgkAssistant.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace SgkAssistant.Forms.Defs
{
    public partial class FUser : Telerik.WinControls.UI.RadForm
    {
        bool allowStart = false; Users usr = null;
        bool uf = false;
        public FUser(Users u = null)
        {
            usr = u;
            InitializeComponent();
        }
        
        private void fUser_Load(object sender, EventArgs e)
        {
            string msg = "";
            cbAuthority.Enabled = true;
            uf = IOC.UserDataService.ExistUser(1, out msg);
            if (uf)
            {
                cbAuthority.SelectedIndex = 1; 
            }
            else
            {
                cbAuthority.SelectedIndex = 0; cbAuthority.Enabled = false;
            }
            if (usr != null)
            {
                texName.Text = usr.Unm;
                texLname.Text = usr.Uln;
                texUname.Text = usr.Un;
                texPass.Text = usr.Up;
                texPass2.Text = usr.Upp;
                cbAuthority.SelectedIndex = (usr.Uy == "YÖNETİCİ") ? 0 : 1;
            }
            List<RadButton> radButtons = new List<RadButton>() { btnAdd, btnClear, btnExit };
            foreach (RadButton btn in radButtons)
            {
                btn.ButtonElement.BorderElement.Visibility = ElementVisibility.Visible;
                btn.ButtonElement.BorderElement.ForeColor = Color.FromArgb(120, 148, 186);
                btn.ButtonElement.ShowBorder = true;
                btn.ForeColor = Color.FromArgb(21, 66, 139);
                btn.ElementTree.Control.Cursor = Cursors.Hand;
            }
        }
        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string msg = "";
            try
            {
                if (texUname.Text.Trim().Length == 0)
                {
                    lblUname.Visible = true; allowStart = false;
                }else if (texPass.Text.Trim().Length == 0)
                {
                    lblPass.Visible = true; allowStart = false;
                }
                else if (texPass2.Text.Trim().Length == 0)
                {
                    lblPass2.Visible = true; allowStart = false;
                }
                else if (texPass.Text.Trim() != texPass2.Text.Trim())
                {
                    lblPass2.Visible = true; allowStart = false;
                }else if (cbAuthority.SelectedIndex != 0 && cbAuthority.SelectedIndex != 1)
                {
                    lblAuthority.Visible = true; allowStart = false;
                }
                lblMessage.Visible = true;
                if (!allowStart)
                {
                    lblMessage.Text = "Lütfen formu eksiksik olarak doldurup tekrar deneyin";
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
                            lblMessage.Text = $"Veritabanı hatası, lütfen tekrar deneyiniz";
                            break;
                        case 0:
                        case 1:
                            u.Id = uid;
                            int result = IOC.UserDataService.AddUser(u, out msg);
                            if (result == 1)
                            {
                                
                                Users newAdded=  IOC.UserDataService.GetUserByUn(u.Un, out msg);
                                if (newAdded == null) { lblMessage.Text = $"Veritabanı hatası, lütfen tekrar deneyiniz {msg}"; return; }
                                UserPrm prm = new UserPrm()
                                {
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
                                int addPrm = IOC.PkcData.SetUserPrm(prm, out msg);
                                if(addPrm != 1) { lblMessage.Text = $"Veritabanı hatası, lütfen tekrar deneyiniz {msg}"; IOC.UserDataService.DeleteUser(u, out msg); return; }
                                UserChanged.HasChanged = true;
                                lblMessage.Text = $"{u.Un} adlı kullanıcı eklendi";
                                texName.Focus();
                            }
                            else if (result == 2)
                            {
                                lblMessage.Text = $"{u.Un} adlı kullanıcının bilgileri değiştirildi";
                                UserChanged.HasChanged = true; 
                            }
                            else
                            {
                                lblMessage.Text = $"Veritabanı hatası, lütfen tekrar deneyiniz {msg}";
                            }
                            break;
                    }
                }
                if (uf == false)
                {
                    btnAdd.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }

        }
        
        private void btnClear_Click(object sender, EventArgs e)
        {
            texName.Clear();
            texLname.Clear();
            texUname.Clear();
            texPass.Clear();
            texPass2.Clear();
        }
        
        private void btnExit_Click(object sender, EventArgs e)
        {
            if (lblMessage.Text.Contains("eklendi"))
                DialogResult = DialogResult.OK;
            else
                  DialogResult = DialogResult.Cancel;  
            
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
                lblName.Visible = true; allowStart = false;
            }
            else
            {
                lblName.Visible = false; allowStart = true;
            }
        }

        private void texLname_KeyUp(object sender, KeyEventArgs e)
        {
            Regex rx = new Regex(@"^[a-zA-Z ğüşıöçĞÜŞİÖÇ]{2,20}$");
            if (!rx.IsMatch(texLname.Text.Trim()))
            {
                lblLname.Visible = true; allowStart = false;
            }
            else
            {
                lblLname.Visible = false; allowStart = true;
            }
        }

        private void texUname_KeyUp(object sender, KeyEventArgs e)
        {
            if(texUname.Text.Trim().Length == 0)
            {
                lblUname.Visible = true; allowStart = false;
            }
            else
            {
                lblUname.Visible = false; allowStart = true;
            }
        }

        private void texPass_KeyUp(object sender, KeyEventArgs e)
        {
            if (texPass.Text.Trim().Length == 0)
            {
                lblPass.Visible = true; allowStart = false;
            }
            else
            {
                lblPass.Visible = false; allowStart = true;
            }
        }

        private void texPass2_KeyUp(object sender, KeyEventArgs e)
        {
            if (texPass2.Text.Trim().Length == 0)
            {
                lblPass2.Visible = true; allowStart = false;
            }
            else
            {
                lblPass2.Visible = false; allowStart = true;
            }
        }

        private void cbAuthority_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (cbAuthority.SelectedIndex != 0 && cbAuthority.SelectedIndex != 1)
            {
                lblAuthority.Visible = true; allowStart = false;
            }
            else
            {
                lblAuthority.Visible = false; allowStart = true;
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
    }
}
