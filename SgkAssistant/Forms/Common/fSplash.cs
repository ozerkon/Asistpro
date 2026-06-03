using Models.Common;
using SgkAssistant.Forms.Defs;
using SgkAssistant.Helpers;
using SgkAssistant.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SgkAssistant.Forms.Common
{
    public partial class FSplash : Telerik.WinControls.UI.RadForm
    {

        BackgroundWorker bgw;
        bool allowSession = false;
        public FSplash()
        {
            //Settings.Default.dbType = 1; GlobalVars.DbType = 1; Settings.Default.Save();
            //TempEnc();
            if (Settings.Default.tabOrderEvizite == null)
            {
                Settings.Default.tabOrderEvizite = new TabOrder() { Index = 0, TabName = "rtEvizite", IsHide = false };
                Settings.Default.tabOrderLinks = new TabOrder() { Index = 1, TabName = "rtLinks", IsHide = false };
                Settings.Default.tabOrderHesapDurumu = new TabOrder() { Index = 2, TabName = "rtHesapDurumu", IsHide = false };
                Settings.Default.tabOrderHizmetListe = new TabOrder() { Index = 3, TabName = "rtHizmetListe", IsHide = false };
                Settings.Default.tabOrderIgic = new TabOrder() { Index = 4, TabName = "rtIgic", IsHide = false };
                Settings.Default.tabOrderLastName = new TabOrder() { Index = 5, TabName = "rtLastName", IsHide = false };
                Settings.Default.tabOrderSettings = new TabOrder() { Index = 6, TabName = "rtOptions", IsHide = false };
                Settings.Default.tabOrderTesvik = new TabOrder() { Index = 7, TabName = "rtTesvik", IsHide = false };
                Settings.Default.tabOrderYillik = new TabOrder() { Index = 8, TabName = "rtYillik", IsHide = false };
                Settings.Default.Save();
            }
            
            InitializeComponent();
            GlobalVars.SolverKey = Settings.Default.solverType == 1 ? Settings.Default.solverKeyIT : Settings.Default.solverKey;
            GlobalVars.SolverType = Settings.Default.solverType;
            radWaitingBar1.StartWaiting();
            bgw = new BackgroundWorker();
            bgw.WorkerSupportsCancellation = true;
            bgw.DoWork += new DoWorkEventHandler(BgwDoWork);
            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BgwComplated);
        }
        private void TempEnc()
        {
            Settings.Default.mschostR = Encrypt.DecryptString(Settings.Default.mschostR, GlobalVars.PassPhrase);
            Settings.Default.mscuidR = Encrypt.DecryptString(Settings.Default.mscuidR, GlobalVars.PassPhrase);
            Settings.Default.mscupR = Encrypt.DecryptString(Settings.Default.mscupR, GlobalVars.PassPhrase);
            Settings.Default.mscdbR = Encrypt.DecryptString(Settings.Default.mscdbR, GlobalVars.PassPhrase);
            Settings.Default.mscprtR = Encrypt.DecryptString(Settings.Default.mscprtR, GlobalVars.PassPhrase);

            Settings.Default.mschostL = Encrypt.DecryptString(Settings.Default.mschostL, GlobalVars.PassPhrase);
            Settings.Default.mscuidL = Encrypt.DecryptString(Settings.Default.mscuidL, GlobalVars.PassPhrase);
            Settings.Default.mscupL = Encrypt.DecryptString(Settings.Default.mscupL, GlobalVars.PassPhrase);
            Settings.Default.mscdbL = Encrypt.DecryptString(Settings.Default.mscdbL, GlobalVars.PassPhrase);
            Settings.Default.mscprtL = Encrypt.DecryptString(Settings.Default.mscprtL, GlobalVars.PassPhrase);

            Settings.Default.Save();
        }
        private void fSplash_Load(object sender, EventArgs e)
        {
            btnCancel.Visible = false;
            lblMessage.Visible = false;
            Control.CheckForIllegalCrossThreadCalls = false;
            if (!bgw.IsBusy)
            {
                bgw.RunWorkerAsync();
            }
        }
        private void BgwDoWork(object sender, DoWorkEventArgs e)
        {
            InitialSettings();
            UpdateCheck();
        }
        private void BgwComplated(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                lblMessage.Text = e.Error.Message;
            }
            else
            {
                if (allowSession == true)
                {
                    this.DialogResult = DialogResult.OK;
                }
            }
        }
        private bool CheckPackage(out string msg)
        {
            Package p = new Package();
            try
            {
                p = IOC.PkcData.GetPackageInfo(out msg);

                if (p == null)
                {
                    return false;
                }
                PackageHelper.DecryptHc(p.Pla);
                return true;
            }
            catch (Exception ex)
            {
                msg = "Hata: " + ex.Message.ToString();
                return false;
            }
        }
        private void InitialSettings()
        {
            string msg = "";

            if (Settings.Default.FirstTimeRunningThisVersion)
            {
                Settings.Default.Upgrade(); // Eski sürümlerden kalan ayarları güvenle taşır/temizler
                Settings.Default.FirstTimeRunningThisVersion = false;
                Settings.Default.Save();
            }

            GlobalVars.DbType = 0;
            GlobalVars.SgscEnc = Settings.Default.sgscEnc;
            GlobalVars.CmpCheck = Settings.Default.cmpCheck;
            GlobalVars.DbType = Settings.Default.dbType;
            SetPublicHolidays();
            //GlobalVars.ct = NtpClient.GetNetworkTime(out msg);
            GlobalVars.Ct = DateTime.Now; // NtpClient.GetNetworkTime(out msg);

            if (GlobalVars.Ct < Settings.Default.lastLoginDate)
            {
                lblMessage.Visible = true;
                radWaitingBar1.StopWaiting();
                radWaitingBar1.Visible = false;
                lblMessage.Text = "Bilgisayarınızın tarih saat ayarı yanlış! Lütfen düzeltip, tekrar deneyin";
                btnCancel.Visible = true;
                return;
            }
            #region authority
            if (!WinHelpers.IsAdministrator())
            {
                this.WindowState = FormWindowState.Minimized;
                FWarningAuthority f = new FWarningAuthority();
                Invoke((Action)(() =>
                {
                    f.ShowDialog();
                }));
            }
            else
            {
                if (!CheckFilesAndFolders())
                {
                    lblMessage.Text = "Gerekli klasörler oluşturulamadı"; lblMessage.Visible = true; btnCancel.Visible = true; return;
                }
                else
                {
                    string discid = WinHelpers.DiskInfo();  // harddisk seri no 
                    Settings.Default.discid = discid;

                    // Değerleri önce değişkenlere alıp temizleyelim
                    string hostL = Encrypt.DecryptString(Settings.Default.mschostL.Trim(), GlobalVars.PassPhrase);
                    string dbL = Encrypt.DecryptString(Settings.Default.mscdbL.Trim(), GlobalVars.PassPhrase);
                    string uidL = Encrypt.DecryptString(Settings.Default.mscuidL.Trim(), GlobalVars.PassPhrase);
                    string upL = Encrypt.DecryptString(Settings.Default.mscupL.Trim(), GlobalVars.PassPhrase);
                    string prtL = Encrypt.DecryptString(Settings.Default.mscprtL.Trim(), GlobalVars.PassPhrase);

                    // Remote ayarları
                    GlobalVars.SetMsqcsRemote(
                        Encrypt.DecryptString(Settings.Default.mschostR.Trim(), GlobalVars.PassPhrase),
                        Encrypt.DecryptString(Settings.Default.mscdbR.Trim(), GlobalVars.PassPhrase),
                        Encrypt.DecryptString(Settings.Default.mscuidR.Trim(), GlobalVars.PassPhrase),
                        Encrypt.DecryptString(Settings.Default.mscupR.Trim(), GlobalVars.PassPhrase),
                        Encrypt.DecryptString(Settings.Default.mscprtR.Trim(), GlobalVars.PassPhrase)
                    );

                    // Sorun yoksa Local ayarları ata
                    GlobalVars.SetMsqcsLocal(hostL, dbL, uidL, upL, prtL);

                    GlobalVars.SetLiteDbCs(false);
                }
            }
            int au = WinHelpers.GetAccountInfo();
            switch (au)
            {
                case 2:
                    this.WindowState = FormWindowState.Minimized;
                    FWarningAuthority f = new FWarningAuthority();
                    Invoke((Action)(() =>
                    {
                        f.ShowDialog();
                    }));
                    break;
            }
            #endregion

            if (!CheckPackage(out msg))
            {
                if (msg.Contains("Unable to connect"))
                {
                    lblMessage.Visible = true;
                    radWaitingBar1.StopWaiting();
                    radWaitingBar1.Visible = false;
                    lblMessage.Text = "Lütfen internet bağlantınızı kontrol edip tekrar deneyin";
                    btnCancel.Visible = true;
                    return;
                }
                this.WindowState = FormWindowState.Minimized;
                FPackage fp = new FPackage();
                Invoke((Action)(() =>
                {
                    if (fp.ShowDialog() == DialogResult.OK)
                    {
                        allowSession = true;
                    }
                    else { allowSession = false; }

                }));
            }
            else
            {
                allowSession = true;
            }
        }
        private void SetPublicHolidays()
        {
            //Settings.Default.publicHolidays = null; Settings.Default.Save();
            if (Settings.Default.publicHolidays == null) Settings.Default.publicHolidays = new List<PublicHolidays>();
            List<PublicHolidays> delete = (from x in Settings.Default.publicHolidays where x.Day == DateTime.MinValue select x).ToList();
            foreach (PublicHolidays day in delete)
            {
                Settings.Default.publicHolidays.Remove(day);
            }
            Settings.Default.Save();
            List<PublicHolidays> phs = new List<PublicHolidays>();
            phs = new List<PublicHolidays>() {
                new PublicHolidays(){ Day = new DateTime(1981, 7, 31) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1982, 7, 21) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1983, 7, 11) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1984, 6, 29) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1985, 6, 19) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1986, 6, 8) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1987, 5, 28) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1988, 5, 16) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1989, 5, 5) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1990, 4, 25) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1991, 4, 15) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1992, 4, 3) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1993, 3, 23) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1994, 3, 12) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1995, 3, 2) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1996, 2, 19) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1997, 2, 8) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1998, 1, 28) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1999, 1, 18) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2000, 1, 7) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2001, 12, 15) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2002, 12, 4) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2003, 11, 24) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2004, 11, 13) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2005, 11, 2) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2006, 10, 22) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2007, 10, 11) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2008, 9, 29) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2009, 9, 19) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2010, 9, 8) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2011, 8, 29) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2012, 8, 18) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2013, 8, 7) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2014, 7, 27) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2015, 7, 16) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2016, 7, 4) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2017, 6, 24) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2018, 6, 14) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2019, 6, 4) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2020, 5, 23) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2021, 5, 12) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2022, 5, 1) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2023, 4, 20) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2024, 4, 9) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2025, 3, 29) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2026, 3, 19) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2027, 3, 8) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2028, 2, 25) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2029, 2, 13) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2030, 2, 3) , Ft = false, Desc = "Ramazan Bayramı Arefesi" },

                new PublicHolidays(){ Day = new DateTime(1992,4,4) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(1993,3,24) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(1994,3,13) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(1995,3,3) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(1996,2,20) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(1997,2,9) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(1998,1,29) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(1999,1,19) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2000,1,8) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2001,12,16) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2002,12,5) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2003,11,25) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2004,11,14) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2005,11,3) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2006,10,23) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2007,10,12) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2008,9,30) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2009,9,20) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2010,9,9) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2011,8,30) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2012,8,19) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2013,8,8) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2014,7,28) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2015,7,17) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2016,7,5) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2017,6,25) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2018,6,15) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2019,6,5) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2020,5,24) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2021,5,13) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2022,5,2) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2023,4,21) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2024,4,10) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2025,3,30) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2026,3,20) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2027,3,9) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2028,2,26) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2029,2,14) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2030,2,4) , Ft = true, Desc = "Ramazan Bayramının 1. Günü" },

                new PublicHolidays(){ Day = new DateTime(1981,8,2) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1982,7,23) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1983,7,13) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1984,7,1) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1985,6,21) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1986,6,10) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1987,5,30) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1988,5,18) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1989,5,7) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1990,4,27) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1991,4,17) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1992,4,5) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1993,3,25) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1994,3,14) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1995,3,4) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1996,2,21) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1997,2,10) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1998,1,30) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1999,1,20) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2000,1,9) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2001,12,17) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2002,12,6) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2003,11,26) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2004,11,15) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2005,11,4) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2006,10,24) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2007,10,13) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2008,10,1) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2009,9,21) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2010,9,10) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2011,8,31) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2012,8,20) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2013,8,9) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2014,7,29) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2015,7,18) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2016,7,6) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2017,6,26) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2018,6,16) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2019,6,6) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2020,5,25) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2021,5,14) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2022,5,3) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2023,4,22) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2024,4,11) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2025,3,31) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2026,3,21) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2027,3,10) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2028,2,27) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2029,2,15) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2030,2,5) , Ft = true, Desc = "Ramazan Bayramının 2. Günü" },

                new PublicHolidays(){ Day = new DateTime(1981,8,3) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1982,7,24) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1983,7,14) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1984,7,2) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1985,6,22) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1986,6,11) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1987,5,31) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1988,5,19) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1989,5,8) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1990,4,28) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1991,4,18) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1992,4,6) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1993,3,26) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1994,3,15) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1995,3,5) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1996,2,22) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1997,2,11) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1998,1,31) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1999,1,21) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2000,1,10) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2001,12,18) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2002,12,7) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2003,11,27) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2004,11,16) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2005,11,5) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2006,10,25) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2007,10,14) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2008,10,2) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2009,9,22) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2010,9,11) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2011,9,1) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2012,8,21) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2013,8,10) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2014,7,30) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2015,7,19) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2016,7,7) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2017,6,27) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2018,6,17) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2019,6,7) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2020,5,26) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2021,5,15) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2022,5,4) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2023,4,23) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2024,4,12) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2025,4,1) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2026,3,22) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2027,3,11) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2028,2,28) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2029,2,16) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2030,2,6) , Ft = true, Desc = "Ramazan Bayramının 3. Günü" },

                new PublicHolidays(){ Day = new DateTime(1981,10,7) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1982,9,26) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1983,9,16) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1984,9,5) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1985,8,25) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1986,8,15) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1987,8,4) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1988,7,23) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1989,7,12) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1990,7,2) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1991,6,22) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1992,6,10) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1993,5,31) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1994,5,20) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1995,5,9) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1996,4,27) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1997,4,17) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1998,4,6) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(1999,3,27) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2000,3,15) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2001,3,4) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2002,2,21) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2003,2,10) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2004,1,31) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2005,1,19) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2006,1,9) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2007,12,19) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2008,12,7) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2009,11,26) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2010,11,15) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2011,11,5) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2012,10,24) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2013,10,14) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2014,10,3) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2015,9,23) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2016,9,11) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2017,8,31) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2018,8,20) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2019,8,10) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2020,7,30) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2021,7,19) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2022,7,8) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2023,6,27) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2024,6,15) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2025,6,5) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2026,5,26) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2027,5,15) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2028,5,4) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2029,4,23) , Ft = false, Desc = "Kurban Bayramı Arefesi" },
                new PublicHolidays(){ Day = new DateTime(2030,4,30) , Ft = false, Desc = "Kurban Bayramı Arefesi" },

                new PublicHolidays(){ Day = new DateTime(1981,10,8) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(1982,9,27) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(1983,9,17) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(1984,9,6) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(1985,8,26) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(1986,8,16) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(1987,8,5) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(1988,7,24) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(1989,7,13) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(1990,7,3) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(1991,6,23) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(1992,6,11) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(1993,6,1) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(1994,5,21) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(1995,5,10) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(1996,4,28) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(1997,4,18) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(1998,4,7) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(1999,3,28) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2000,3,16) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2001,3,5) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2002,2,22) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2003,2,11) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2004,2,1) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2005,1,20) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2006,1,10) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2007,12,20) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2008,12,8) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2009,11,27) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2010,11,16) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2011,11,6) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2012,10,25) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2013,10,15) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2014,10,4) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2015,9,24) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2016,9,12) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2017,9,1) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2018,8,21) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2019,8,11) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2020,7,31) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2021,7,20) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2022,7,9) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2023,6,28) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2024,6,16) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2025,6,6) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2026,5,27) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2027,5,16) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2028,5,5) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2029,4,24) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },
                new PublicHolidays(){ Day = new DateTime(2030,4,13) , Ft = true, Desc = "Kurban Bayramının 1. Günü" },

                new PublicHolidays(){ Day = new DateTime(1981,10,9) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1982,9,28) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1983,9,18) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1984,9,7) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1985,8,27) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1986,8,17) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1987,8,6) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1988,7,25) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1989,7,14) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1990,7,4) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1991,6,24) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1992,6,12) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1993,6,2) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1994,5,22) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1995,5,11) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1996,4,29) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1997,4,19) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1998,4,8) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(1999,3,29) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2000,3,17) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2001,3,6) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2002,2,23) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2003,2,12) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2004,2,2) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2005,1,21) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2006,1,11) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2007,12,21) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2008,12,9) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2009,11,28) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2010,11,17) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2011,11,7) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2012,10,26) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2013,10,16) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2014,10,5) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2015,9,25) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2016,9,13) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2017,9,2) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2018,8,22) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2019,8,12) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2020,8,1) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2021,7,21) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2022,7,10) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2023,6,29) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2024,6,17) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2025,6,7) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2026,5,28) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2027,5,17) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2028,5,6) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2029,4,25) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },
                new PublicHolidays(){ Day = new DateTime(2030,4,14) , Ft = true, Desc = "Kurban Bayramının 2. Günü" },

                new PublicHolidays(){ Day = new DateTime(1981,10,10) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1982,9,29) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1983,9,19) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1984,9,8) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1985,8,28) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1986,8,18) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1987,8,7) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1988,7,26) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1989,7,15) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1990,7,5) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1991,6,25) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1992,6,13) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1993,6,3) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1994,5,23) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1995,5,12) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1996,4,30) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1997,4,20) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1998,4,9) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(1999,3,30) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2000,3,18) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2001,3,7) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2002,2,24) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2003,2,13) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2004,2,3) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2005,1,22) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2006,1,12) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2007,12,22) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2008,12,10) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2009,11,29) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2010,11,18) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2011,11,8) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2012,10,27) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2013,10,17) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2014,10,6) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2015,9,26) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2016,9,14) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2017,9,3) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2018,8,23) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2019,8,13) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2020,8,2) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2021,7,22) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2022,7,11) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2023,6,30) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2024,6,18) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2025,6,8) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2026,5,29) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2027,5,18) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2028,5,7) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2029,4,26) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },
                new PublicHolidays(){ Day = new DateTime(2030,4,15) , Ft = true, Desc = "Kurban Bayramının 3. Günü" },

                new PublicHolidays(){ Day = new DateTime(1981,10,11) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(1982,9,30) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(1983,9,20) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(1984,9,9) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(1985,8,29) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(1986,8,19) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(1987,8,8) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(1988,7,27) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(1989,7,16) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(1990,7,6) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(1991,6,26) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(1992,6,14) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(1993,6,4) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(1994,5,24) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(1995,5,13) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(1996,5,1) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(1997,4,21) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(1998,4,10) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(1999,3,31) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2000,3,19) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2001,3,8) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2002,2,25) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2003,2,14) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2004,2,4) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2005,1,23) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2006,1,13) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2007,12,23) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2008,12,11) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2009,11,30) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2010,11,19) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2011,11,9) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2012,10,28) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2013,10,18) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2014,10,7) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2015,9,27) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2016,9,15) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2017,9,4) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2018,8,24) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2019,8,14) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2020,8,3) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2021,7,23) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2022,7,12) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2023,7,1) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2024,6,19) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2025,6,9) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2026,5,30) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2027,5,19) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2028,5,8) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2029,4,27) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
                new PublicHolidays(){ Day = new DateTime(2030,4,16) , Ft = true, Desc = "Kurban Bayramının 4. Günü" },
            };
            for (int i = 1981; i < 2030; i++)
            {
                phs.Add(new PublicHolidays() { Day = new DateTime(i, 1, 1), Ft = true, Desc = $"{i} Yılbaşı" });
                phs.Add(new PublicHolidays() { Day = new DateTime(i, 4, 23), Ft = true, Desc = "Ulusal Egemenlik ve Çocuk Bayramı" });
                phs.Add(new PublicHolidays() { Day = new DateTime(i, 5, 19), Ft = true, Desc = "Atatürk’ü Anma ve Gençlik ve Spor Bayramı" });
                phs.Add(new PublicHolidays() { Day = new DateTime(i, 8, 30), Ft = true, Desc = "Zafer Bayramı" });
                phs.Add(new PublicHolidays() { Day = new DateTime(i, 10, 28), Ft = false, Desc = "Cumhuriyet Bayramı Arefesi" });
                phs.Add(new PublicHolidays() { Day = new DateTime(i, 10, 29), Ft = true, Desc = "Cumhuriyet Bayramı" });
            }
            for (int i = 2017; i < 2030; i++)
            {
                phs.Add(new PublicHolidays() { Day = new DateTime(i, 7, 15), Ft = true, Desc = "Demokrasi ve Milli Birlik Günü" });
            }
            for (int i = 2010; i < 2030; i++)
            {
                phs.Add(new PublicHolidays() { Day = new DateTime(i, 5, 1), Ft = true, Desc = "Emek ve dayanışma günü" });
            }
            
            foreach (PublicHolidays ph in phs)
            {
                if (!Settings.Default.publicHolidays.Any(x => x.Day.Date == ph.Day.Date && x.Ft == ph.Ft)) { Settings.Default.publicHolidays.Add(ph);  }
            }
            Settings.Default.Save();
        }
        private void radLabel1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.sinerjia.net/");
        }
        private bool CheckFilesAndFolders()
        {
            try
            {
                string mainFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\SgkAsistan";
                TryCreateFolder(mainFolder, false);
                string subFolder = $"{mainFolder}\\Data";
                TryCreateFolder(subFolder, false);
                subFolder = $"{mainFolder}\\Pdf";
                TryCreateFolder(subFolder, false);
                subFolder = $"{mainFolder}\\Pdftmp";
                TryCreateFolder(subFolder, false);
                subFolder = $"{mainFolder}\\Temp";
                TryCreateFolder(subFolder, false);
                //WinHelpers.CreateShortcut("Asistpro", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Assembly.GetExecutingAssembly().Location);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public bool TryCreateFolder(string folderToCreate, bool clearExistingFiles)
        {
            int tryCount = 0;
            bool folderCreated = false;
            bool folderIsEmpty = false;
            while (tryCount++ < 10)
            {
                if (System.IO.Directory.Exists(folderToCreate) == false)
                {
                    System.IO.Directory.CreateDirectory(folderToCreate);
                }
                else
                {
                    folderCreated = true;
                    break;
                }

            }
            tryCount = 0;
            if (folderCreated == false)
            {
                return false;
            }
            if (clearExistingFiles)
            {
                folderIsEmpty = false;
                while (tryCount++ < 10)
                {
                    int fileCount = 0;
                    System.IO.DirectoryInfo di = new DirectoryInfo(folderToCreate);
                    foreach (FileInfo file in di.EnumerateFiles())
                    {
                        file.Delete();
                        fileCount++;
                    }
                    if (fileCount == 0)
                    {
                        folderIsEmpty = true;
                        break;
                    }
                }
                return folderCreated && folderIsEmpty;
            }

            return folderCreated;
        }
        public void UpdateCheck()
        {
            bool allowStart = false;
            TimeSpan diffResult = DateTime.Now.Subtract(Settings.Default.lastUpdateCheckDate);
            switch (Settings.Default.updateFrequency)
            {
                case 1:
                    allowStart = true;
                    break;
                case 2:
                    allowStart = diffResult.Days > 0 ? true : false;
                    break;
                case 3:
                    allowStart = diffResult.Days > 6 ? true : false;
                    break;
                case 4:
                    allowStart = diffResult.Days > 29 ? true : false;
                    break;
            }
            if (allowStart)
            {
                CheckForUpdate checkForUpdate = new CheckForUpdate(this);
                checkForUpdate.OnCheckForUpdate();
            }
        }


       
    }
}
