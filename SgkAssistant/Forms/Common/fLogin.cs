using Models.Common;
using SgkAssistant.Forms.Common;
using SgkAssistant.Forms.Defs;
using SgkAssistant.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace SgkAssistant.Forms.Sgk
{
    public partial class FLogin : Telerik.WinControls.UI.RadForm
    {
        List<Users> usr = new List<Users>();
        Users u = new Users();
        bool btnCfuMode = true; // true : FUser açar, false: FLicence açar

        public FLogin(Users us = null)
        {
            InitializeComponent();
            u = us;
        }
        
        private void fLogin_Load(object sender, EventArgs e)
        {
            btnCfuMode = true;
            List<RadButton> radButtons = new List<RadButton>() { btnLogin, btnCancel, btnRefresh };
            foreach (RadButton btn in radButtons)
            {
                btn.ButtonElement.BorderElement.Visibility = ElementVisibility.Visible;
                btn.ButtonElement.BorderElement.ForeColor = Color.FromArgb(120, 148, 186);
                btn.ButtonElement.ShowBorder = true;
                btn.ForeColor = Color.FromArgb(21, 66, 139);
                btn.ElementTree.Control.Cursor = Cursors.Hand;
            }
            texUname.Text = Properties.Settings.Default.lastUserName;
            btnRefresh.Visible = false;
            string msg;
            usr = IOC.UserDataService.GetAllUsers(out msg);
            lblMessage.Text = string.Empty;
            FirstUserCreated(out msg);
            if (u != null)
            {
                texUname.Text = u.Un;
            }
        }
        
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string msg;
            
            (bool isSuccess, Users u) = IOC.UserDataService.ExistUser(texUname.Text, texUpass.Text, out msg);
            if (!isSuccess)
            {
                lblMessage.Text = $"Kullanıcı adı veya şifre hatalı {msg}";
            }
            else
            {
                Properties.Settings.Default.lastUserName = texUname.Text;
                Properties.Settings.Default.lastLoginDate = DateTime.Now;
                GlobalVars.LastLoginDate = Properties.Settings.Default.lastLoginDate;
                Properties.Settings.Default.Save();
                Hide();
                Users.ActiveUser.Id = u.Id;
                Users.ActiveUser.LoginTime = DateTime.Now;
                Users.ActiveUser.Yetki = (u.Uy == "Y")? 1 : 0;
                GlobalVars.UserPrm = IOC.PkcData.GetUserPrm(u.Id, out msg);
                DialogResult = DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            
            if (Users.ActiveUser.UserChangeRequest == false)
            {
                Application.Exit();
            }
            this.DialogResult = DialogResult.Cancel;
            
        }
       
        private void VisitLink()
        {
            System.Diagnostics.Process.Start("http://www.sinerjia.net/");
        }

        private void btnCreateFirstUser_Click(object sender, EventArgs e)
        {
            if (btnCfuMode)
            {
                FUser f = new FUser();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    btnLogin.Enabled = true;
                    btnCreateFirstUser.Visible = false;
                }
            }
            else
            {
                FLicence f = new FLicence();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    btnLogin.Enabled = true;
                    btnCreateFirstUser.Visible = false;
                }
            }
        }

        private void FirstUserCreated(out string msg)
        {
            msg = "";
            try
            {
                if(usr != null && usr.Count == 0 )
                {
                    btnCreateFirstUser.Enabled = true;
                    btnCreateFirstUser.Visible = true;
                    btnLogin.Enabled = false;
                }
                else
                {
                    btnCreateFirstUser.Enabled = false;
                    btnCreateFirstUser.Visible = false;
                    if (PackageHelper.Ay <= 1)
                    {
                        CheckDemoTime(out msg);
                    }
                    else
                    {
                        CheckRemainTime(out msg);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                lblMessage.Text = $"Veri tabanına bağlanılamıyor! {msg}";
            }
            
        }

        private void CheckDemoTime(out string msg)
        {
            msg = "";
            try
            {
                //DateTime ct = NtpClient.GetNetworkTime(out msg);
                Package p = IOC.PkcData.GetPackageInfo(out msg);
                DateTime frd = NtpClient.DeComplicationSd(p.Ptr);
                TimeSpan diffResult = frd.AddDays(PackageHelper.Gun).Subtract(GlobalVars.Ct);
                int dayRemain = diffResult.Days;

                if (frd != DateTime.MinValue)
                {
                    lblMessage.Text = $"Demo sürümün süresinin dolmasına {dayRemain} gün kaldı";
                    btnRefresh.Visible = false;
                    btnLogin.Enabled = true; 
                }
                if (dayRemain <= 0)
                {
                    lblMessage.Text = "Demo sürümün süresi doldu";
                    btnCreateFirstUser.Text = "Lisans Anahtarını Değiştir";
                    btnCreateFirstUser.Visible = true;
                    btnCreateFirstUser.Image = Properties.Resources.key;
                    btnCfuMode = false;
                    btnLogin.Enabled = false;
                }
            }
            catch (Exception)
            {
                lblMessage.Text = msg;
                btnRefresh.Visible = true;
                btnLogin.Enabled = false;
            }
        }

        private void CheckRemainTime(out string msg)
        {
            msg = "";
            try
            {
                Package p =  IOC.PkcData.GetPackageInfo(out msg);
                int mounth = Convert.ToInt32( p.Psc.Substring(4, 2));
                DateTime frd = NtpClient.DeComplicationSd(p.Ptr);

                TimeSpan diffResult = frd.AddMonths(mounth).Subtract(GlobalVars.Ct);
                int dayRemain = diffResult.Days;
                if (frd != DateTime.MinValue && dayRemain <= 30 )
                {
                    lblMessage.Text = $"Üyeliğinizin bitmesine {dayRemain} gün kaldı";
                    btnRefresh.Visible = false;
                    btnLogin.Enabled = true;
                }
                if (dayRemain <= 0)
                {
                    lblMessage.Text = "Üyelik süresi doldu. https://www.sinerjia.net üzerinden sürenizi uzatabilirsiniz";
                    btnCreateFirstUser.Text = "Lisans Anahtarını Değiştir";
                    btnCreateFirstUser.Visible = true;
                    btnCreateFirstUser.Enabled = true;
                    btnCreateFirstUser.Font = new Font("Segoe UI", 12F);
                    btnCreateFirstUser.Image = Properties.Resources.key;
                    btnCfuMode = false;
                    btnLogin.Enabled = false;
                }
            }
            catch (Exception)
            {
                lblMessage.Text = msg;
                btnRefresh.Visible = true;
                btnLogin.Enabled = false;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            string msg;
            CheckDemoTime(out msg);
        }

        private void texUpass_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode ==  Keys.Enter && btnLogin.Enabled == true)
            {
                btnLogin_Click(null, null);
            }
            
        }

        private void radLabel1_Click(object sender, EventArgs e)
        {
            VisitLink();
        }
    }
}
