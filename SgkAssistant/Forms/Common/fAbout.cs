using Models.Common;
using SgkAssistant.Helpers;
using SgkAssistant.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace SgkAssistant.Forms.Common
{
    partial class FAbout : Telerik.WinControls.UI.RadForm
    {
        private readonly CheckForUpdate checkForUpdate = null;
        private bool bypass = false;
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer(); string msg = "";
        private long percent;
        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                // Get all Title attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                // If there is at least one Title attribute
                if (attributes.Length > 0)
                {
                    // Select the first one
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    // If it is not an empty string, return it
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }
                // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                // Get all Description attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                // If there aren't any Description attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Description attribute, return its value
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                // Get all Product attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                // If there aren't any Product attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Product attribute, return its value
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                // Get all Copyright attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                // If there aren't any Copyright attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Copyright attribute, return its value
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                // Get all Company attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                // If there aren't any Company attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Company attribute, return its value
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        public FAbout(bool bypass = false)
        {
            InitializeComponent();

            this.checkForUpdate = new CheckForUpdate(this);
            this.Text = String.Format("{0}", $"{AssemblyTitle} Hakkında");
            this.radLabelProductName.Text = AssemblyProduct;
            this.radLabelVersion.Text = String.Format("Sürüm {0}", AssemblyVersion);
            this.radLabelCopyright.Text = AssemblyCopyright;
            this.radLabelCompanyName.Text = $"Asistpro, bir {AssemblyCompany} ürünüdür.";
            this.radTextBoxDescription.Text = AssemblyDescription;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.bypass = bypass;
        }
        private void fAbout_Load(object sender, EventArgs e)
        {
            pnlMessage.Visible = false;
            pnlInstall.Visible = false;
            lblMessage.Text = string.Empty;
            lblAlert.Text = string.Empty;

            rwbNewVersion.StopWaiting();
            rwbNewVersion.Visible = false;
            List<RadButton> radButtons = new List<RadButton>() { btnCheckUpdate, btnInstallNew, btnChangeLicence };
            foreach (RadButton btn in radButtons)
            {
                btn.ButtonElement.BorderElement.Visibility = ElementVisibility.Visible;
                btn.ButtonElement.BorderElement.ForeColor = Color.FromArgb(120, 148, 186);
                btn.ButtonElement.ShowBorder = true;
                btn.ForeColor = Color.FromArgb(21, 66, 139);
                btn.ElementTree.Control.Cursor = Cursors.Hand;
            }
            Package p = IOC.PkcData.GetPackageInfo(out msg);
            int mounth = Convert.ToInt32(p.Psc.Substring(4, 2));
            DateTime frd = NtpClient.DeComplicationSd(p.Ptr);
            TimeSpan diffResult = PackageHelper.Ay > 1 ? frd.AddMonths(mounth).Subtract(GlobalVars.Ct) : frd.AddDays(PackageHelper.Gun).Subtract(GlobalVars.Ct); 
            int dayRemain = diffResult.Days;

            lblLicenceType.Text = PackageHelper.Ay <= 1 ? "Demo Sürüm" : "Tam Sürüm";
            lblStartDate.Text = NtpClient.DeComplicationSd(p.Ptr).ToString("dd.MM.yyyy");
            lblEndDate.Text = PackageHelper.Ay > 1 ? NtpClient.DeComplicationSd(p.Ptr).AddMonths(mounth).ToString("dd.MM.yyyy") : NtpClient.DeComplicationSd(p.Ptr).AddDays(PackageHelper.Gun).ToString("dd.MM.yyyy");
            lblRemainDay.Text = $"{dayRemain.ToString()} gün";
            lblMcc.Text = PackageHelper.Mcc.ToString();

            if (bypass)
            {
                tabAboutUtdate.SelectedIndex = 1;
                btnCheckUpdate_Click(null, null);
                Thread.Sleep(500);
                checkForUpdate.StopThread();
                btnInstallNew_Click(null, null);
            }
        }
        private void fAbout_FormClosing(object sender, FormClosingEventArgs e)
        {
            checkForUpdate.StopThread();
        }
        public bool OnCheckForUpdateFinished(DownloadedVersionInfo versionInfo)
        {
            if ((versionInfo.Error) || (versionInfo.InstallerUrl.Length == 0) || (versionInfo.LatestVersion == null))
            {
                lblMessage.Text = "Yeni sürüm kontrolü yapılamadı!" ;
                lblMessage2.Text = "Lütfen internet bağlantınızı kontrol edip tekrar deneyin.";
                lblMessage3.Text = "";
                return false;
            }
            Version curVer = Assembly.GetExecutingAssembly().GetName().Version;
            if (curVer.CompareTo(versionInfo.LatestVersion) >= 0)
            {
                lblMessage.Text = "Asistpro güncel";
                lblMessage2.Text = $"Sürüm: {curVer}";
                lblMessage3.Text = "";
                return false;
            }

            //new version found, ask the user if he wants to download the installer
            lblMessage.Text = "Yeni sürüm bulundu";
            lblMessage2.Text = $"Yüklü sürüm: {curVer}";
            lblMessage3.Text = $"Yeni sürüm: {versionInfo.LatestVersion}";
            pnlInstall.Visible = true;
            return true; 
        }
        public void OnDownloadInstallerinished(DownloadInstallerInfo info)
        {
            if (info.Error)
            {
                //MessageBox.Show(this, "Error while downloading the installer", "Check for updates", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblAlert.Text = "Yükleyici indirilemedi! ";
                return;
            }
            // ask the user if he want to start the installer
            if (DialogResult.Yes != MessageBox.Show(this, "Yeni sürümü şimdi yüklemek istiyor musunuz?\r\nYeni sürüm yüklendikten sonra Asistpro'yu yeniden başlatmanız gerekecektir", "Yeni sürüm indirildi", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                // it not - remove the downloaded file
                pnlMessage.Visible = false;
                pnlInstall.Visible = false;
                lblMessage.Text = string.Empty;
                lblAlert.Text = string.Empty;
                btnCheckUpdate.Enabled = true;
                rwbNewVersion.StopWaiting();
                rwbNewVersion.Visible = false;
                try
                {
                    File.Delete(info.Path);
                }
                catch { }
                return;
            }
            // run the installer and exit the app
            try
            {
                Process.Start(info.Path);
                this.Close();
                Application.Exit();
            }
            catch (Exception)
            {
                MessageBox.Show(this, "Yükleme esnasında hata oluştu.", "Yükleme hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try
                {
                    File.Delete(info.Path);
                }
                catch { }
                return;
            }
            return;
        }
        private void InitializeTimer()
        {
            timer.Interval = 1000;
            timer.Enabled = true;
            timer1_Tick(null, null);

            timer.Tick += new EventHandler(timer1_Tick);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            rwbNewVersion.Text = $"Yeni sürüm indiriliyor % {percent.ToString()} ";
        }
        public void DownloadedBytes(long bytesRead, int contentLength)
        {
            
            if (bytesRead < contentLength)
            {
                InitializeTimer();
                percent = 100 * bytesRead / contentLength; 
                timer1_Tick(null, null);
            }
            else
            {
                timer.Enabled = false;
            }
            
        }
        private void btnCheckUpdate_Click(object sender, EventArgs e)
        {
            checkForUpdate.OnCheckForUpdate();
        }
        private void lblMessage_TextChanged(object sender, EventArgs e)
        {
            if (lblMessage.Text == string.Empty)
            {
                pnlMessage.Visible = false;
            }
            else
            {
                pnlMessage.Visible = true;
            }
        }
        private void btnInstallNew_Click(object sender, EventArgs e)
        {
            
            if (rwbNewVersion.IsWaiting)
            {
                rwbNewVersion.StopWaiting(); rwbNewVersion.Visible = false;
                btnInstallNew.Text = "Yeni Sürümü İndir";
                btnInstallNew.Image = Resources.download;
                btnCheckUpdate.Enabled = true;
            }
            else
            {
                rwbNewVersion.StartWaiting(); rwbNewVersion.Visible = true;
                btnInstallNew.Text = "İndirmeyi İptal Et";
                btnInstallNew.Image = Resources.stop;
                btnCheckUpdate.Enabled = false;
            }
            checkForUpdate.OnGetNewVersion();
        }
        private void lblAlert_TabIndexChanged(object sender, EventArgs e)
        {
            if (lblAlert.Text == string.Empty)
            {
                lblAlert.Visible = false;
            }
            else
            {
                lblAlert.Visible = true;
            }
        }
        private void btnChangeLicence_Click(object sender, EventArgs e)
        {
            FLicence f = new FLicence();
            f.ShowDialog();
        }
    }
}
