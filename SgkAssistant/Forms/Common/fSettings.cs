using GridViewCustomCells;
using Models.Common;
using Models.Domain;
using SgkAssistant.Helpers;
using SgkAssistant.Properties;
using SGKServices.Captcha;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace SgkAssistant.Forms.Common
{
    public partial class FSettings : RadForm
    {
        private List<TabOrder> tabOrders = new List<TabOrder>();
        private UserPrm up = new UserPrm(); string msg = "", browserName = "";
        private RadCheckBox cbAutoCaptcha, cbHideBrowser, cbKeepBrowser, cbCloseAllBrowsers, cbSira, cbCid, cbCid2, cbSp, cbCp, cbGun, cbGp, cbGs, cbScd1, cbScd2, cbScd3, cbScd4, cbScd5;
        private PublicHolidays deletePh = new PublicHolidays();
        private RadRibbonBar rrb = new RadRibbonBar();
        private byte dbTypeFirst = 0, dbTypeLast = 0;
        public FSettings(RadRibbonBar rrb)
        {

            this.rrb = rrb;
            up = GlobalVars.UserPrm;
            InitializeComponent();
            //CustomDragDropService customService = new CustomDragDropService(rgvTabs.GridViewElement);
            //rgvTabs.GridViewElement.RegisterService(customService);
        }
        private void fSettings_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            texImageTyperz.Enabled = false;
            AddControl();
            GetSettings();
            dbTypeFirst = GlobalVars.DbType;
        }
        private void rgvPublicHolidays_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            List<DateTime> phs = new List<DateTime>()
            {
                new DateTime(DateTime.Now.Year, 1,1),
                new DateTime(DateTime.Now.Year, 4,23),
                new DateTime(DateTime.Now.Year, 5,1),
                new DateTime(DateTime.Now.Year, 5,19),
                new DateTime(DateTime.Now.Year, 7,15),
                new DateTime(DateTime.Now.Year, 8,30),
                new DateTime(DateTime.Now.Year, 10,28),
                new DateTime(DateTime.Now.Year, 10,29),
            };
            if (e.ActiveEditor is RadDateTimeEditor)
            {
                foreach (DateTime ph in phs)
                {
                    DateTime cellDate = Convert.ToDateTime(rgvPublicHolidays.CurrentRow.Cells[0].Value);
                    if (ph.Month == cellDate.Month && ph.Day == cellDate.Day && cellDate.Year != 1)
                    {
                        lblMessage.Text = $"{cellDate.ToString("dd.MM.yyyy")} tarihi zaten eklenmiş, lütfen yeniden deneyin";
                        rgvPublicHolidays.CurrentRow.Cells[0].Value = DateTime.Today;
                        break;
                    }
                }
            }
        }
        private void rgvPublicHolidays_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            e.Cancel = false;
            List<DateTime> phs = new List<DateTime>()
            {
                new DateTime(DateTime.Now.Year, 1,1),
                new DateTime(DateTime.Now.Year, 4,23),
                new DateTime(DateTime.Now.Year, 5,1),
                new DateTime(DateTime.Now.Year, 5,19),
                new DateTime(DateTime.Now.Year, 7,15),
                new DateTime(DateTime.Now.Year, 8,30),
                new DateTime(DateTime.Now.Year, 10,28),
                new DateTime(DateTime.Now.Year, 10,29),
            };
            foreach (DateTime ph in phs)
            {
                DateTime cellDate = Convert.ToDateTime(rgvPublicHolidays.CurrentRow.Cells[0].Value);
                if (ph.Month == cellDate.Month && ph.Day == cellDate.Day && cellDate.Year != 1)
                {
                    lblMessage.Text = "Bu tatil günü değiştirilemez";
                    e.Cancel = true;
                    break;
                }
            }
        }
        private void rgvPublicHolidays_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            e.Cancel = false;
            List<DateTime> phs = new List<DateTime>()
            {
                new DateTime(DateTime.Now.Year, 1,1),
                new DateTime(DateTime.Now.Year, 4,23),
                new DateTime(DateTime.Now.Year, 5,1),
                new DateTime(DateTime.Now.Year, 5,19),
                new DateTime(DateTime.Now.Year, 7,15),
                new DateTime(DateTime.Now.Year, 8,30),
                new DateTime(DateTime.Now.Year, 10,28),
                new DateTime(DateTime.Now.Year, 10,29),
            };
            foreach (DateTime ph in phs)
            {
                DateTime cellDate = Convert.ToDateTime(rgvPublicHolidays.CurrentRow.Cells[0].Value);
                if (ph.Month == cellDate.Month && ph.Day == cellDate.Day && cellDate.Year != 1)
                {
                    lblMessage.Text = "Bu tatil günü değiştirilemez";
                    e.Cancel = true;
                    break;
                }
            }
        }
        private void rgvPublicHolidays_UserAddingRow(object sender, GridViewRowCancelEventArgs e)
        {
            e.Cancel = false;
            foreach (GridViewCellInfo cell in rgvPublicHolidays.CurrentRow.Cells)
            {
                if (cell.Value == null) { lblMessage.Text = "Lütfen bütün alanları doldurunuz"; e.Cancel = true; return; }
            }
            foreach (PublicHolidays ph in GlobalVars.PublicHolidays)
            {
                if(ph.Day.Date == Convert.ToDateTime(rgvPublicHolidays.CurrentRow.Cells[0].Value).Date)
                {
                    lblMessage.Text = "Bu tarih zaten eklenmiş";
                    e.Cancel = true;
                    break;
                }
            }
        }
        private void rgvPublicHolidays_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            PublicHolidays ph = new PublicHolidays()
            {
                Day = Convert.ToDateTime(e.Row.Cells[0].Value),
                Ft = Convert.ToBoolean(e.Row.Cells[1].Value),
                Desc = e.Row.Cells[2].Value.ToString()
            };
            Settings.Default.publicHolidays.Add(ph);
            GlobalVars.PublicHolidays = Settings.Default.publicHolidays;
            Settings.Default.Save();
            lblMessage.Text = $"{ph.Day.Date.ToString("dd.MM.yyyy")} tarihi eklendi";
        }
        private void rgvPublicHolidays_UserDeletedRow(object sender, GridViewRowEventArgs e)
        {
            Settings.Default.publicHolidays.RemoveAll(r => r.Day == deletePh.Day);
            GlobalVars.PublicHolidays = Settings.Default.publicHolidays;
            Settings.Default.Save();
            lblMessage.Text = $"{deletePh.Day.Date.ToString("dd.MM.yyyy")} tarihi silindi";
        }
        private void rgvPublicHolidays_UserDeletingRow(object sender, GridViewRowCancelEventArgs e)
        {
            deletePh = new PublicHolidays();
            DateTime dateTime = Convert.ToDateTime(rgvPublicHolidays.CurrentRow.Cells[0].Value);
            if (
                (dateTime.Month == 1 && dateTime.Day == 1) || 
                (dateTime.Month == 4 && dateTime.Day == 23) || 
                (dateTime.Month == 5 && dateTime.Day == 1) || 
                (dateTime.Month == 5 && dateTime.Day == 19) || 
                (dateTime.Month == 7 && dateTime.Day == 15) || 
                (dateTime.Month == 8 && dateTime.Day == 30) || 
                (dateTime.Month == 10 && dateTime.Day == 28) || 
                (dateTime.Month == 10 && dateTime.Day == 29) )
            {
                lblMessage.Text = "Bu tatil günü silinemez"; e.Cancel = true;
            }
            else
            {
                deletePh = new PublicHolidays()
                {
                    Day = Convert.ToDateTime(rgvPublicHolidays.CurrentRow.Cells[0].Value),
                    Ft = Convert.ToBoolean(rgvPublicHolidays.CurrentRow.Cells[1].Value),
                    Desc = rgvPublicHolidays.CurrentRow.Cells[2].Value.ToString()
                };
            }
        }
        
        private void lblMessage_TextChanged(object sender, EventArgs e)
        {
            lblMessage.Visible = lblMessage.Text != "" ? true : false;
        }
        private void rbUpdate_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (sender == rbUpdateNever)
            {
                Settings.Default.updateFrequency = 0;
            }
            else if (sender == rbUpdateOpening)
            {
                Settings.Default.updateFrequency = 1;
            }
            else if (sender == rbUpdateOnceADay)
            {
                Settings.Default.updateFrequency = 2;
            }
            else if (sender == rbUpdateOnceAWeek)
            {
                Settings.Default.updateFrequency = 3;
            }
            else if (sender == rbUpdateOnceAMounth)
            {
                Settings.Default.updateFrequency = 4;
            }
            
        }
        private void ddlBrowser_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            browserName = ddlBrowser.SelectedItem.Text;
            lblKeepBrowser.Text = $"İşlem bitince {browserName} kapatılsın";
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
        private void btnUpdateLinks_Click(object sender, EventArgs e)
        {
            bool updt = RefreshLinkList(out msg);
            lblMessage.Visible = true;
            LinkGlobals.HasUpdated = updt;
            lblMessage.Text = updt ? "Link listesi güncellendi" : $"Liste güncellenemedi! {msg}";
        }
        private void labels_ToolTipTextNeeded(object sender, ToolTipTextNeededEventArgs e)
        {
            RadLabelElement lbl = sender as RadLabelElement;
            if (lbl.Text.Contains("Bekle"))
            {
                e.ToolTipText = "İnternet tarayıcının sayfanın yüklenmesi, sayfadaki ilgili nesnelerin (butonlar, linkler, metin kutuları, captcha kodu resimleri, tablolar vs.) bulunması için bekleyeceği maksimum süre. Yavaş internet bağlantıları için en az 60 sn seçiniz.";
            }else if(lbl.Text.Contains("bütün"))
            {
                e.ToolTipText = "Programdan çıkıldığında bilgisayarınızda açık olan bütün Google Chrome ve Firefox pencerelerini kapatır. Dikkat: Asistpro haricinde açılmış olanları da kapatır.";
            }
            
        }
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            tabOrders.Clear();
            Settings.Default.cmmColor = colorBoxCmm.Value;
            Settings.Default.cmColor = colorBoxCm.Value;
            Settings.Default.pdColor = colorBoxPd.Value;

            Settings.Default.hlCalisanColor = colorBoxHlCalisan.Value;
            Settings.Default.cGunColor = colorBoxHlCikis.Value;
            Settings.Default.gGunColor = colorBoxHlGiris.Value;

            Settings.Default.leavePersonalOddColor = colorBoxPersonalOdd.Value;
            Settings.Default.leavePersonalEvenColor = colorBoxPersonalEven.Value;

            Settings.Default.leavePeriodOddColor = colorBoxPeriodOdd.Value;
            Settings.Default.leavePeriodEvenColor = colorBoxPeriodEven.Value;

            Settings.Default.iglGirisColor = colorBoxIglGiris.Value;
            Settings.Default.iglCikisColor = colorBoxIglCikis.Value;

            Settings.Default.autoCaptcha = cbAutoCaptcha.Checked;
            Settings.Default.hideBrowser = cbHideBrowser.Checked;
            Settings.Default.browser = ddlBrowser.SelectedIndex;
            Settings.Default.disposeDriver = cbKeepBrowser.Checked;
            Settings.Default.closeAllBrowsers = cbCloseAllBrowsers.Checked;
            Settings.Default.wait = Convert.ToInt32(nudWait.Value);

            Settings.Default.sira = Convert.ToInt32(cbSira.Checked);
            Settings.Default.cid = Convert.ToInt32(cbCid.Checked);
            Settings.Default.cid2 = Convert.ToInt32(cbCid2.Checked);
            Settings.Default.sp = Convert.ToInt32(cbSp.Checked);
            Settings.Default.cp = Convert.ToInt32(cbCp.Checked);
            Settings.Default.gun = Convert.ToInt32(cbGun.Checked);
            Settings.Default.gp = Convert.ToInt32(cbGp.Checked);
            Settings.Default.gs = Convert.ToInt32(cbGs.Checked);
            Settings.Default.scd1 = Convert.ToInt32(cbScd1.Checked);
            Settings.Default.scd2 = Convert.ToInt32(cbScd2.Checked);
            Settings.Default.scd3 = Convert.ToInt32(cbScd3.Checked);
            Settings.Default.scd4 = Convert.ToInt32(cbScd4.Checked);
            Settings.Default.scd5 = Convert.ToInt32(cbScd5.Checked);

            Settings.Default.theme = ddlTheme.SelectedItem.Text;
            ThemeResolutionService.ApplicationThemeName = Settings.Default.theme;
            if (rbImageTyperz.CheckState == CheckState.Checked)
            {
                Settings.Default.solverKeyIT = texImageTyperz.Text.Trim();
                Settings.Default.solverType = 1;
                GlobalVars.SolverKey = Settings.Default.solverKeyIT;
                GlobalVars.SolverType = Settings.Default.solverType; 
            }
            else
            {
                Settings.Default.solverKey = texFreeOcr.Text.Trim();
                Settings.Default.solverType = 2;
                GlobalVars.SolverKey = Settings.Default.solverKey;
                GlobalVars.SolverType = Settings.Default.solverType; 
            }
            IOC.Solver = SolverFactory.CreateSolver(Settings.Default.solverType);
            #region tabOrder
            foreach (var item in rgvTabs.Rows)
            {
                TabOrder tabOrder = new TabOrder() { Index = item.Index, TabName = item.Cells[0].Value.ToString(), IsHide = Convert.ToBoolean(item.Cells[2].Value) };
                tabOrders.Add(tabOrder);
            }

            IOC.DesignHelper.OrderRibbonTabs(rrb, tabOrders);
            foreach (TabOrder tab in tabOrders)
            {
                switch (tab.TabName)
                {
                    case "rtEvizite":
                        Settings.Default.tabOrderEvizite.Index = tab.Index; Settings.Default.tabOrderEvizite.IsHide = tab.IsHide;
                        break;
                    case "rtLinks":
                        Settings.Default.tabOrderLinks.Index = tab.Index; Settings.Default.tabOrderLinks.IsHide = tab.IsHide;
                        break;
                    case "rtHesapDurumu":
                        Settings.Default.tabOrderHesapDurumu.Index = tab.Index; Settings.Default.tabOrderHesapDurumu.IsHide = tab.IsHide;
                        break;
                    case "rtHizmetListe":
                        Settings.Default.tabOrderHizmetListe.Index = tab.Index; Settings.Default.tabOrderHizmetListe.IsHide = tab.IsHide;
                        break;
                    case "rtIgic":
                        Settings.Default.tabOrderIgic.Index = tab.Index; Settings.Default.tabOrderIgic.IsHide = tab.IsHide;
                        break;
                    case "rtLastName":
                        Settings.Default.tabOrderLastName.Index = tab.Index; Settings.Default.tabOrderLastName.IsHide = tab.IsHide;
                        break;
                    case "rtOptions":
                        Settings.Default.tabOrderSettings.Index = tab.Index; Settings.Default.tabOrderSettings.IsHide = false;
                        break;
                    case "rtTesvik":
                        Settings.Default.tabOrderTesvik.Index = tab.Index; Settings.Default.tabOrderTesvik.IsHide = tab.IsHide;
                        break;
                    case "rtYillik":
                        Settings.Default.tabOrderYillik.Index = tab.Index; Settings.Default.tabOrderYillik.IsHide = tab.IsHide;
                        break;
                }
            }
            #endregion
            #region database
            if (dbTypeFirst != GlobalVars.DbType)
            {
                dbTypeLast = GlobalVars.DbType;
                MoveDB();
            }
            #endregion
            Settings.Default.Save();

            GlobalVars.SolverType = Settings.Default.solverType;
            GlobalVars.AutoCaptcha = Settings.Default.autoCaptcha;
            LinkGlobals.BrowserType = Settings.Default.browser;
            LinkGlobals.MaxWait = Settings.Default.wait;
            GlobalVars.DisposeDriver = Settings.Default.disposeDriver;
            lblMessage.Visible = true; lblMessage.Text = "Kaydedildi";
            Changed.HasChanged = true;
        }
        private void GetSettings()
        {
            colorBoxCmm.Value = Settings.Default.cmmColor;
            colorBoxCm.Value = Settings.Default.cmColor;
            colorBoxPd.Value = Settings.Default.pdColor;

            colorBoxHlCalisan.Value = Settings.Default.hlCalisanColor;
            colorBoxHlGiris.Value = Settings.Default.gGunColor;
            colorBoxHlCikis.Value = Settings.Default.cGunColor;

            colorBoxPersonalOdd.Value = Settings.Default.leavePersonalOddColor;
            colorBoxPersonalEven.Value = Settings.Default.leavePersonalEvenColor;

            colorBoxPeriodOdd.Value = Settings.Default.leavePeriodOddColor;
            colorBoxPeriodEven.Value = Settings.Default.leavePeriodEvenColor;

            colorBoxIglGiris.Value = Settings.Default.iglGirisColor;
            colorBoxIglCikis.Value = Settings.Default.iglCikisColor;

            cbAutoCaptcha.Checked = Settings.Default.autoCaptcha;
            cbHideBrowser.Checked = Settings.Default.hideBrowser;
            ddlBrowser.SelectedIndex = Settings.Default.browser;
            cbKeepBrowser.Checked = Settings.Default.disposeDriver;
            cbCloseAllBrowsers.Checked = Settings.Default.closeAllBrowsers;
            nudWait.Value = Settings.Default.wait;

            cbSira.Checked = Convert.ToBoolean(Settings.Default.sira);
            cbCid.Checked = Convert.ToBoolean(Settings.Default.cid);
            cbCid2.Checked = Convert.ToBoolean(Settings.Default.cid2);
            cbSp.Checked = Convert.ToBoolean(Settings.Default.sp);
            cbCp.Checked = Convert.ToBoolean(Settings.Default.cp);
            cbGun.Checked = Convert.ToBoolean(Settings.Default.gun);
            cbGp.Checked = Convert.ToBoolean(Settings.Default.gp);
            cbGs.Checked = Convert.ToBoolean(Settings.Default.gs);
            cbScd1.Checked = Convert.ToBoolean(Settings.Default.scd1);
            cbScd2.Checked = Convert.ToBoolean(Settings.Default.scd2);
            cbScd3.Checked = Convert.ToBoolean(Settings.Default.scd3);
            cbScd4.Checked = Convert.ToBoolean(Settings.Default.scd4);
            cbScd5.Checked = Convert.ToBoolean(Settings.Default.scd5);

            ddlTheme.SelectedValue = Settings.Default.theme;

            texFreeOcr.Text = Settings.Default.solverKey;
            texImageTyperz.Text = Settings.Default.solverKeyIT;
            if (Settings.Default.solverType == 1)
            {
                rbImageTyperz.CheckState = CheckState.Checked;
            }
            else
            {
                rbFreeOcr.CheckState = CheckState.Checked;
            }

            switch (Settings.Default.updateFrequency)
            {
                case 0:
                    rbUpdateNever.CheckState = CheckState.Checked;
                    break;
                case 1:
                    rbUpdateOpening.CheckState = CheckState.Checked;
                    break;
                case 2:
                    rbUpdateOnceADay.CheckState = CheckState.Checked;
                    break;
                case 3:
                    rbUpdateOnceAWeek.CheckState = CheckState.Checked;
                    break;
                case 4:
                    rbUpdateOnceAMounth.CheckState = CheckState.Checked;
                    break;
            }

            GridViewTextBoxColumn tabNameColumn = new GridViewTextBoxColumn();
            tabNameColumn.Name = "Name";
            tabNameColumn.FieldName = "Name";
            tabNameColumn.IsVisible = false;
            GridViewTextBoxColumn textBoxColumn = new GridViewTextBoxColumn();
            textBoxColumn.Name = "Sekme";
            textBoxColumn.FieldName = "Sekme";
            textBoxColumn.HeaderText = "Sekme";
            GridViewCheckBoxColumn checkBoxColumn = new GridViewCheckBoxColumn();
            checkBoxColumn.Name = "Gizle";
            checkBoxColumn.FieldName = "Gizle";
            checkBoxColumn.HeaderText = "GİZLE";
            checkBoxColumn.EnableHeaderCheckBox = false;
            checkBoxColumn.EditMode = EditMode.OnValidate;
            checkBoxColumn.ShouldCheckDataRows = true;
            checkBoxColumn.ThreeState = false;
            checkBoxColumn.MaxWidth = 100;
            rgvTabs.Columns.Add(tabNameColumn);
            rgvTabs.Columns.Add(textBoxColumn);
            rgvTabs.Columns.Add(checkBoxColumn);
            tabOrders.Add(new TabOrder() { Index = Settings.Default.tabOrderEvizite.Index, TabName = Settings.Default.tabOrderEvizite.TabName, IsHide = Settings.Default.tabOrderEvizite.IsHide} );
            tabOrders.Add(new TabOrder() { Index = Settings.Default.tabOrderLinks.Index, TabName = Settings.Default.tabOrderLinks.TabName, IsHide = Settings.Default.tabOrderLinks.IsHide} );
            tabOrders.Add(new TabOrder() { Index = Settings.Default.tabOrderHesapDurumu.Index, TabName = Settings.Default.tabOrderHesapDurumu.TabName, IsHide = Settings.Default.tabOrderHesapDurumu.IsHide} );
            tabOrders.Add(new TabOrder() { Index = Settings.Default.tabOrderHizmetListe.Index, TabName = Settings.Default.tabOrderHizmetListe.TabName, IsHide = Settings.Default.tabOrderHizmetListe.IsHide} );
            tabOrders.Add(new TabOrder() { Index = Settings.Default.tabOrderIgic.Index, TabName = Settings.Default.tabOrderIgic.TabName, IsHide = Settings.Default.tabOrderIgic.IsHide} );
            tabOrders.Add(new TabOrder() { Index = Settings.Default.tabOrderLastName.Index, TabName = Settings.Default.tabOrderLastName.TabName, IsHide = Settings.Default.tabOrderLastName.IsHide} );
            tabOrders.Add(new TabOrder() { Index = Settings.Default.tabOrderSettings.Index, TabName = Settings.Default.tabOrderSettings.TabName, IsHide = false} );
            tabOrders.Add(new TabOrder() { Index = Settings.Default.tabOrderTesvik.Index, TabName = Settings.Default.tabOrderTesvik.TabName, IsHide = Settings.Default.tabOrderTesvik.IsHide} );
            tabOrders.Add(new TabOrder() { Index = Settings.Default.tabOrderYillik.Index, TabName = Settings.Default.tabOrderYillik.TabName, IsHide = Settings.Default.tabOrderYillik.IsHide} );

            tabOrders.Sort( ( a, b ) => a.Index.CompareTo( b.Index ) );
            foreach (TabOrder tabOrder in tabOrders)
            {
                switch (tabOrder.TabName)
                {
                    case "rtEvizite":
                        rgvTabs.Rows.Add(tabOrder.TabName, "E-VİZİTE", tabOrder.IsHide);
                        break;
                    case "rtLinks":
                        rgvTabs.Rows.Add(tabOrder.TabName, "LİNKLER", tabOrder.IsHide);
                        break;
                    case "rtHesapDurumu":
                        rgvTabs.Rows.Add(tabOrder.TabName, "SGK BORÇ ALACAK DURUMU", tabOrder.IsHide);
                        break;
                    case "rtHizmetListe":
                        rgvTabs.Rows.Add(tabOrder.TabName, "HİZMET LİSTESİ", tabOrder.IsHide);
                        break;
                    case "rtIgic":
                        rgvTabs.Rows.Add(tabOrder.TabName, "İŞE GİRİŞ İŞTEN ÇIKIŞ", tabOrder.IsHide);
                        break;
                    case "rtLastName":
                        rgvTabs.Rows.Add(tabOrder.TabName, "SOYADI GÜNCELLEME", tabOrder.IsHide);
                        break;
                    case "rtOptions":
                        rgvTabs.Rows.Add(tabOrder.TabName, "AYARLAR", tabOrder.IsHide);
                        break;
                    case "rtTesvik":
                        rgvTabs.Rows.Add(tabOrder.TabName, "PERSONEL TEŞVİKLERİ", tabOrder.IsHide);
                        break;
                    case "rtYillik":
                        rgvTabs.Rows.Add(tabOrder.TabName, "YILLIK İZİN TAKİBİ", tabOrder.IsHide);
                        break;
                }
                
            }

            rgvTabs.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            rgvTabs.Columns["Sekme"].ReadOnly = true;
            rgvTabs.Columns["Gizle"].ReadOnly = false;

            GridViewDateTimeColumn phDateColumn = new GridViewDateTimeColumn();
            phDateColumn.Name = "date";
            phDateColumn.FieldName = "date";
            phDateColumn.Width = 80;
            phDateColumn.MaxWidth = 80;
            phDateColumn.TextAlignment = ContentAlignment.MiddleCenter;
            phDateColumn.FormatInfo = new System.Globalization.CultureInfo("tr-TR");
            phDateColumn.FormatString = "{0:dd.MM.yyyy}";
            phDateColumn.HeaderText = "TARİH";

            Dictionary<string, bool> ft = new Dictionary<string, bool>() { };
            ft.Add(" Tam Gün", true);
            ft.Add(" Yarım Gün", false);
            IndicatedComboBoxColumn phFtColumn = new IndicatedComboBoxColumn();
            phFtColumn.Name = "ft";
            phFtColumn.HeaderText = "TATİL GÜNÜ";
            phFtColumn.DataSource = new BindingSource(ft, null);
            phFtColumn.ValueMember = "Value";
            phFtColumn.DisplayMember = "Key";
            phFtColumn.Width = 90;
            phFtColumn.MaxWidth = 90;
            phFtColumn.AllowFiltering = false;
            phFtColumn.AllowGroup = false;
            phFtColumn.AllowSort = false;
            phFtColumn.AllowSearching = false;
            phFtColumn.AllowReorder = false;
            phFtColumn.DropDownStyle = RadDropDownStyle.DropDownList;

            GridViewTextBoxColumn phDescColumn = new GridViewTextBoxColumn();
            phDescColumn.Name = "desc";
            phDescColumn.FieldName = "desc";
            phDescColumn.HeaderText = "AÇIKLAMA";
            phDescColumn.AutoEllipsis = true;
            phDescColumn.FormatInfo =  new System.Globalization.CultureInfo("tr-TR");
            rgvPublicHolidays.Columns.Add(phDateColumn);
            rgvPublicHolidays.Columns.Add(phFtColumn);
            rgvPublicHolidays.Columns.Add(phDescColumn);

            GlobalVars.PublicHolidays.Sort((a, b) => a.Day.Date.CompareTo(b.Day.Date));
            foreach (PublicHolidays ph in GlobalVars.PublicHolidays)
            {
                rgvPublicHolidays.Rows.Add(ph.Day.Date, ph.Ft ? true : false, $" {ph.Desc}" );
            }
            rgvPublicHolidays.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            if (Settings.Default.dbType == 0)
            {
                rbLitedb.CheckState = CheckState.Checked;
            }
            else
            {
                rbMysql.CheckState = CheckState.Checked;
                texMysqlHost.Text = Encrypt.DecryptString(Settings.Default.mschostL, GlobalVars.PassPhrase);
                texMysqlUn.Text = Encrypt.DecryptString(Settings.Default.mscuidL, GlobalVars.PassPhrase);
                texMysqlUp.Text = Encrypt.DecryptString(Settings.Default.mscupL, GlobalVars.PassPhrase);
                texMysqlPr.Text = Encrypt.DecryptString(Settings.Default.mscprtL, GlobalVars.PassPhrase);
            }
        }
        private void AddControl()
        {
            cbHideBrowser = CheckBoxFactory.Create("", Convert.ToInt32(Settings.Default.hideBrowser), up.HideBrowser); tblBrowser.Controls.Add(cbHideBrowser, 1, 0); cbHideBrowser.Margin = new Padding(5, 12, 0, 0);
            cbAutoCaptcha = CheckBoxFactory.Create("", Convert.ToInt32(Settings.Default.autoCaptcha), up.AutoCaptcha); tblBrowser.Controls.Add(cbAutoCaptcha, 1, 1); cbAutoCaptcha.Margin = new Padding(5, 12, 0, 0);
            cbKeepBrowser = CheckBoxFactory.Create( "", Convert.ToInt32(Settings.Default.disposeDriver), up.KeepBrowser); tblBrowser.Controls.Add(cbKeepBrowser, 1, 3); cbKeepBrowser.Margin = new Padding(5, 12, 0, 0);
            cbCloseAllBrowsers = CheckBoxFactory.Create( "", Convert.ToInt32(Settings.Default.closeAllBrowsers), up.CloseAllBrowsers); tblBrowser.Controls.Add(cbCloseAllBrowsers, 1, 4); cbCloseAllBrowsers.Margin = new Padding(5, 12, 0, 0);

            cbSira = CheckBoxFactory.Create("", Settings.Default.sira, up.Sira);    tblCompany.Controls.Add(cbSira, 1, 0);  cbSira.Margin = new Padding(2, 11, 0, 0);
            cbCid = CheckBoxFactory.Create("", Settings.Default.cid, up.Cid);       tblCompany.Controls.Add(cbCid, 1, 2);   cbCid.Margin = new Padding(2, 11, 0, 0);
            cbCid2 = CheckBoxFactory.Create("", Settings.Default.cid2, up.Cid2);    tblCompany.Controls.Add(cbCid2, 1, 3);  cbCid2.Margin = new Padding(2, 11, 0, 0);
            cbSp = CheckBoxFactory.Create("", Settings.Default.sp, up.Sp);          tblCompany.Controls.Add(cbSp, 1, 4);    cbSp.Margin = new Padding(2, 11, 0, 0);
            cbCp = CheckBoxFactory.Create("", Settings.Default.cp, up.Cp);          tblCompany.Controls.Add(cbCp, 1, 5);    cbCp.Margin = new Padding(2, 11, 0, 0);
            cbGun = CheckBoxFactory.Create("", Settings.Default.gun, up.Gun);       tblCompany.Controls.Add(cbGun, 1, 6);   cbGun.Margin = new Padding(2, 11, 0, 0);

            cbGp = CheckBoxFactory.Create("", Settings.Default.gp, up.Gp);          tblCompany.Controls.Add(cbGp, 3, 0);    cbGp.Margin = new Padding(2, 11, 0, 0);
            cbGs = CheckBoxFactory.Create("", Settings.Default.gs, up.Gs);          tblCompany.Controls.Add(cbGs, 3, 1);    cbGs.Margin = new Padding(2, 11, 0, 0);
            cbScd1 = CheckBoxFactory.Create( "", Settings.Default.scd1, up.Scd1);   tblCompany.Controls.Add(cbScd1, 3, 2);  cbScd1.Margin = new Padding(2, 11, 0, 0);
            cbScd2 = CheckBoxFactory.Create( "", Settings.Default.scd2, up.Scd2);   tblCompany.Controls.Add(cbScd2, 3, 3);  cbScd2.Margin = new Padding(2, 11, 0, 0);
            cbScd3 = CheckBoxFactory.Create( "", Settings.Default.scd3, up.Scd3);   tblCompany.Controls.Add(cbScd3, 3, 4);  cbScd3.Margin = new Padding(2, 11, 0, 0);
            cbScd4 = CheckBoxFactory.Create( "", Settings.Default.scd4, up.Scd4);   tblCompany.Controls.Add(cbScd4, 3, 5);  cbScd4.Margin = new Padding(2, 11, 0, 0);
            cbScd5 = CheckBoxFactory.Create( "", Settings.Default.scd5, up.Scd5);   tblCompany.Controls.Add(cbScd5, 3, 6);  cbScd5.Margin = new Padding(2, 11, 0, 0);

            List<RadButton> radButtons = new List<RadButton>() { btnSave, btnUpdateLinks, btnClose };
            foreach (RadButton btn in radButtons)
            {
                btn.ButtonElement.BorderElement.Visibility = ElementVisibility.Visible;
                btn.ButtonElement.BorderElement.ForeColor = Color.FromArgb(120, 148, 186);
                btn.ButtonElement.ShowBorder = true;
                btn.ForeColor = Color.FromArgb(21, 66, 139);
                btn.ElementTree.Control.Cursor = Cursors.Hand;
            }
        }
        private bool RefreshLinkList(out string msg)
        {
            msg = "";
            try
            {
                List<Links> lstLink = IOC.LinksDataService.GetAllLinksFromRs(out msg);
                if(lstLink == null) { return false; }
                int addLinksResult = 0;
                foreach (Links l in lstLink)
                {
                    addLinksResult += IOC.LinksDataService.AddLink(l, out msg);
                }
                LinkGlobals.LstLinks = lstLink;
                return true;
                
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }

        #region page2Mysql
        private void rbDataBase_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            lblMessage.Text = "";
            if (sender == rbLitedb)
            {
                Settings.Default.dbType = 0;
                GlobalVars.DbType = 0;
                IOC.SetDbBase(GlobalVars.DbType);
                pnlMySQL.Enabled = false;
                string litedbCs = $@"Filename={Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\SgkAsistan\Data\sgkasistan.db;Connection=shared;";
                GlobalVars.SetLiteDbCs(false);
                IOC.DBBase.SetConLocal(litedbCs);
            }
            else if (sender == rbMysql)
            {
                Settings.Default.dbType = 1;
                GlobalVars.DbType = 1;
                IOC.SetDbBase(GlobalVars.DbType);
                pnlMySQL.Enabled = true;
            }
        }
        private void btnLocalDBTest_Click(object sender, EventArgs e)
        {
            string sv = texMysqlHost.Text.Trim();
            string un = texMysqlUn.Text.Trim();
            string up = texMysqlUp.Text.Trim();
            string pr = texMysqlPr.Text.Trim();
            if (!CheckMysqlFielsd(sv, un, up, pr)) { lblMessage.Text = "Lütfen bütün alanları doldurun"; return; }
            string mysqlcs = $"Server='{sv}';Database='information_schema';Uid='{un}';Pwd='{up}';Port={pr};";
            bool result = IOC.DBBase.TestConnection(mysqlcs);
            if (result)
            {
                lblMessage.Text = "Mysql sunucusu bağlantısı başarılı";
                GlobalVars.SetMsqcsLocal(sv, "asistan", un, up, pr);
                IOC.DBBase.SetConLocal(mysqlcs);
                Settings.Default.mschostL = Encrypt.EncryptString(sv, GlobalVars.PassPhrase);
                Settings.Default.mscuidL = Encrypt.EncryptString(un, GlobalVars.PassPhrase);
                Settings.Default.mscupL = Encrypt.EncryptString(up, GlobalVars.PassPhrase);
                Settings.Default.mscdbL = Encrypt.EncryptString("asistan", GlobalVars.PassPhrase);
                Settings.Default.mscprtL = Encrypt.EncryptString(pr, GlobalVars.PassPhrase);
                Settings.Default.Save();
            }
            else
            {
                lblMessage.Text = "Başarısız! Lütfen ayarlarınızı kontrol edip tekrar deneyiniz.";
            }
        }
        public bool CheckMysqlFielsd(string sv, string un, string up, string pr)
        {
            if (sv == "")
            {
                texMysqlHost.Focus(); return false;
            }
            else if (un == "")
            {
                texMysqlUn.Focus(); return false;
            }
            else if (up == "")
            {
                texMysqlUp.Focus(); return false;
            }
            else if (pr == "")
            {
                texMysqlPr.Focus(); return false;
            }
            return true;
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

        private bool MoveDB()
        {
            GlobalVars.DbType = dbTypeFirst;  Settings.Default.dbType = dbTypeFirst; Settings.Default.Save();IOC.SetDbBase(GlobalVars.DbType);

            List<Company> lstCompanies = IOC.CompanyDataService.GetCompanies(out msg);
            List<Leaves> lstLeaves = IOC.LeaveDataService.GetAllLeaves(out msg);
            List<int> iynos = (from x in lstCompanies select x.Id).ToList();
            string companyCheck = IOC.CompanyDataService.GetSgscEnc(out msg); 
            List<Links> lstLinks = IOC.LinksDataService.GetAllLinks(out msg);
            Package package = IOC.PkcData.GetPackageInfo(out msg);
            List<LeavePeriod> lstPeriods = IOC.LeavePeriodDataService.GetLeavePeriods(out msg); 
            List<Personal> lstPersonal = IOC.PersonalDataService.GetAllPersonals(out msg); 
            List<Sgk6661> lst6661 = IOC.SgkDataService.GetAll6661(out msg); 
            List<SgkCr> lstCrs = IOC.SgkDataService.GetAllCr(iynos, out msg); 
            List<SgkDb> lstDbs = IOC.SgkDataService.GetAllDb(iynos, out msg); 
            List<SgkEt> lstEts = IOC.SgkDataService.GetAllEt(iynos, out msg); 
            List<SgkHl> lstHls = IOC.SgkDataService.GetAllHl(out msg); 
            List<SgkHlp> lstHlps = IOC.SgkDataService.GetAllHlp(out msg);
            List<SgkIgl> lstIgls = IOC.SgkDataService.GetAllIgl(out msg); 
            List<SgkMe> lstMes = IOC.SgkDataService.GetAllMe(iynos, out msg); 
            List<SgkThkk> lstThkks = IOC.AccrualDataService.GetAllThkk(out msg); 
            List<UserPrm> lstUserPrms = IOC.PkcData.GetUserPrms(out msg); 
            List<Users> lstUsers = IOC.UserDataService.GetAllUsers(out msg);

            GlobalVars.DbType = dbTypeLast; Settings.Default.dbType = dbTypeLast; Settings.Default.Save(); IOC.SetDbBase(GlobalVars.DbType);
            IOC.DBBase.TruncateTables();

            IOC.LeaveDataService.AddLeaves(lstLeaves, out msg);
            foreach (Company c in lstCompanies)
            {
                IOC.CompanyDataService.AddCompany(c, PackageHelper.Mcc, out msg);
            }
            IOC.CompanyDataService.UpdateSgscEnc(companyCheck, out msg);
            IOC.LinksDataService.AddLinks(lstLinks, out msg);
            IOC.PkcData.AddPkc(package, out msg);
            IOC.LeavePeriodDataService.AddLeavePeriods(lstPeriods, out msg);
            IOC.PersonalDataService.AddPersonals(lstPersonal, out msg);
            IOC.SgkDataService.Add6661(lst6661, out msg);
            IOC.SgkDataService.AddCr(lstCrs, out msg);
            IOC.SgkDataService.AddPd(lstDbs, out msg);
            IOC.SgkDataService.AddEt(lstEts, out msg);
            IOC.SgkDataService.AddHl(lstHls, out msg);
            IOC.SgkDataService.AddHlp(lstHlps, out msg);
            IOC.SgkDataService.AddIgl(lstIgls, out msg);
            IOC.SgkDataService.AddMe(lstMes, out msg);
            IOC.AccrualDataService.AddThkk(lstThkks, out msg);
            foreach (UserPrm prm in lstUserPrms)
            {
                IOC.PkcData.SetUserPrm(prm, out msg);
            }
            foreach (Users us in lstUsers)
            {
                IOC.UserDataService.AddUser(us, out msg);
            }
            
            return true;
        }
        #endregion
    }
}
