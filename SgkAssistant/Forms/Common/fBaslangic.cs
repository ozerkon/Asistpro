using Models.Common;
using Models.Domain;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SgkAssistant.Forms.Defs;
using SgkAssistant.Forms.Sgk;
using SgkAssistant.Helpers;
using SgkAssistant.LinkOperations;
using SgkAssistant.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Localization;
using WebDriverX;

namespace SgkAssistant.Forms.Common
{
    public partial class FBaslangic : RadRibbonForm
    {
        #region commonVars
        enum ModulTypeEnum { Vizite, Links, Lastname, Hesapdurumu, Hizmet, Igl, Checkup, Tesvik, Izin, Options, Tahakkuk, HizmetUcretsiz };
        ModulTypeEnum modulType = ModulTypeEnum.Vizite;
        private bool soyadError = false;
        private bool acError = false;
        private bool viziteFirtOpen = true;
        private bool linksFirtOpen = true;
        private bool soyadFirstOpen = true;
        private bool sgkFirstOpen = true;
        private bool hlFirstOpen = true;
        private bool iglFirstOpen = true;
        private bool leaveFirstOpen = true;
        private bool isTcnoValid = false;
        private string pdfFolder = "";
        private string pdfHluFolder = "";
        private string tempFolder = "";
        private string msg = "";
        private int hlTryCount = 3;
        private RadTextBoxControl texLeaveTckn;
        private GridViewSelectedRowsCollection selectedRows;
        private GridDataCellElement hoverCellLeaves, hoverCellCompany;
        private RadContextMenu companyContex;
        private RadMenuItem companyInfo, companyList;
        #endregion

        #region viziteVars
        private delegate void SafeCallDelegate(List<Visit> visitsReceived);
        private delegate void SafeFreshDelegate();

        enum ReportTypeEnum { Tcno, RaporTarihi, OnayliRapor, ArsivRaporu };
        ReportTypeEnum reportType = ReportTypeEnum.RaporTarihi;
        
        public enum CaseTypeEnnum { Hepsi, Iskazasi, Hastalik, Analik };

        public CaseTypeEnnum CaseType = CaseTypeEnnum.Hepsi;
        enum PersonelEnum { Check, Dontcheck }
        PersonelEnum personelEnum = PersonelEnum.Dontcheck;
        enum ProcessTypeEnum { Raportarama, Personelimdegiltarama, Raporonaylama, Onayiptaletme, Raporayrintilari, Pregnancycheck };
        ProcessTypeEnum processType = ProcessTypeEnum.Raportarama;
        enum ListTypeEnum { Yok, Onayli, Onaylanacak };
        ListTypeEnum listType = ListTypeEnum.Yok;
        enum ExportTypeEnum { Ony, Onstc, Onstr, Arv }
        ExportTypeEnum exportTypeEnum = ExportTypeEnum.Onstr;

        List<SourceIgb> sourceIgBs = new List<SourceIgb>();
        List<SourceSpvudk> sourceSpvudKs = new List<SourceSpvudk>();
        List<SourceSraod> sourceSraoDs = new List<SourceSraod>();
        private List<Visit> visitsReceived = new List<Visit>();
        private List<VisitsToBeProcessed> visitsToBeProcessed = new List<VisitsToBeProcessed>();

        bool enableGetDetail = false;
        bool allowStartConfirm;
        string rgvTitleText = string.Empty, viziteRapor="";
        BackgroundWorker bgwSearch;
        BackgroundWorker bgwConfirm;
        BackgroundWorker bgwDetails;

        DateTimePicker dtpViziteFirst = new DateTimePicker();
        DateTimePicker dtpViziteEnd = new DateTimePicker();
        DateTimePicker dtpSearchDate = new DateTimePicker();
        #endregion

        #region linksVars
        bool tempHideBrowser = false;
        public new string CompanyName { get; set; }
        public enum GrpSortEnum { Asc, Desc, None };
        GrpSortEnum grpSort = GrpSortEnum.None; 
        public enum SgrpSortEnum { Asc, Desc, None };
        SgrpSortEnum sgrpSort = SgrpSortEnum.None;
        public enum AdSortEnum { Asc, Desc, None};
        AdSortEnum adSort = AdSortEnum.None;
        public enum FavSortEnum { Asc, Desc, None};
        FavSortEnum favSort = FavSortEnum.None;
        StarShape shape = new StarShape();
        BackgroundWorker bgwLinks;

        #endregion

        #region soyadVars
        Company login = new Company();
        string report = "";
        bool hasErrorSoyad = false;
        List<LastNames> listeLastNames = new List<LastNames>();

        BackgroundWorker bgwSoyad;
        #endregion

        #region hesapDurumuVars
        private delegate void SafeCallSgkDonemselBorc(List<SgkDb> borc); 
        private delegate void SafeCallSgkEmanet(List<SgkEt> emnt);
        private delegate void SafeCallSgkMossip(List<SgkMe> emnt);
        private delegate void SafeCallSgkIcra(List<SgkCr> icr);
        private delegate void SafeCallSgk6661(List<Sgk6661> aaab);
        private delegate void SafeCallSgkIgl(List<SgkIgl> aaab);
        

        private delegate void SafeFreshDelegateSgkOtomasyon();
        private delegate void SafeFreshDelegateSgkMossip();
        enum HesapTypeEnum { Donemselborc, EborcuYoktur, Emanet, Icra, Destek6661, Igl, Hl, Hlu, Hlo, Tahakkuk}
        HesapTypeEnum hesapType = HesapTypeEnum.Donemselborc;
        //enum DebtAndCreditEnum { Donemselborc, Emanet, Icra, Destek6661 }
        //DebtAndCreditEnum debtAndCredit = DebtAndCreditEnum.Donemselborc;

        RadMenuItem btnPrintEt = new RadMenuItem("Banka/İşveren Emanet Tahsilatları");
        RadMenuItem btnPrintMe = new RadMenuItem("Mossip Emanet Tahsilatları");

        List<SgkDb> lstDb = new List<SgkDb>();
        List<SgkEt> lstEt = new List<SgkEt>();
        List<SgkMe> lstMe = new List<SgkMe>();
        List<SgkCr> lstCr = new List<SgkCr>();
        List<Sgk6661> lst6661 = new List<Sgk6661>();
        List<SgkIgl> lstIgl = new List<SgkIgl>();
        
        BackgroundWorker bgwSgk;
        string sgkOtomRapor = "";
        #endregion

        #region hizmetListesiVars
        private delegate void SafeCallSgkHl(List<SgkHl> hl);
        private delegate void SafeCallSgkHlp(List<SgkHlp> hlp);
        private delegate void SafeCallSgkThkk(List<SgkThkk> thkk);
        List<SgkHl> lstHl = new List<SgkHl>();
        List<SgkHlDownload> lstHlDownload = new List<SgkHlDownload>();
        List<SgkHlp> lstHlp = new List<SgkHlp>();
        List<SgkThkk> lstThkk = new List<SgkThkk>();
        List<SgkThkkDownload> lstThkkDownload = new List<SgkThkkDownload>();
        RadMenuItem btnPrintOnaylibildirge = new RadMenuItem("Onaylı Bildirgeler");
        RadMenuItem btnPrintHizmetliste = new RadMenuItem("Hizmet Listesi");

        #endregion

        #region sgkCheckUpVars

        #endregion

        #region iglVars
        DateTimePicker dtpIglFirst = new DateTimePicker();
        DateTimePicker dtpIglLast = new DateTimePicker();
        #endregion

        #region izinVars
        private DateTimePicker dtpLeaveFirst = new DateTimePicker();
        private DateTimePicker dtpLeaveLast = new DateTimePicker();
        private List<Leaves> lstLeaves = new List<Leaves>();
        private delegate void SafeCallLeaves(List<Leaves> leaves);
        private enum IzinTypeEnum { Personal, Addleave, Addleaves, Tcnoall, Tcnodate, Companyall, Companydate}
        private IzinTypeEnum izinType = IzinTypeEnum.Companyall;
        private IzinTypeEnum izinTypeTemp = IzinTypeEnum.Companyall;
        private Company izinComp = new Company();
        private RadContextMenu izinContex;
        private RadMenuItem copyCell, copyRow, copyAllRow, edit, showPeriod, showAll, expand, collapse;
        private bool hierarchy = false, isPersonal = false;
        private RadGridView rgvTemp = new RadGridView();
        private List<Personal> personalsTemp = new List<Personal>();
        private List<LeavePeriod> periodsTemp = new List<LeavePeriod>();
        private string toolTipText = "";
        #endregion
        public FBaslangic()
        {
            PrintDialogsLocalizationProvider.CurrentProvider = new MyPrintDialogsLocalizationProvider();
            RadGridLocalizationProvider.CurrentProvider = new Localization();
            RadMessageLocalizationProvider.CurrentProvider = new MyRadMessageLocalizationProvider();
            ColorDialogLocalizationProvider.CurrentProvider = new CustomColorDialogLocalizationProvider();
            
            bgwSearch = new BackgroundWorker();
            bgwConfirm = new BackgroundWorker();
            bgwDetails = new BackgroundWorker();
            bgwSoyad = new BackgroundWorker();
            bgwSgk = new BackgroundWorker();
            bgwLinks = new BackgroundWorker();
            InitializeComponent();
            List<TabOrder> tabOrders = new List<TabOrder>();
            tabOrders.Add(new TabOrder() { Index = Settings.Default.tabOrderEvizite.Index, TabName = Settings.Default.tabOrderEvizite.TabName, IsHide = Settings.Default.tabOrderEvizite.IsHide });
            tabOrders.Add(new TabOrder() { Index = Settings.Default.tabOrderLinks.Index, TabName = Settings.Default.tabOrderLinks.TabName, IsHide = Settings.Default.tabOrderLinks.IsHide });
            tabOrders.Add(new TabOrder() { Index = Settings.Default.tabOrderHesapDurumu.Index, TabName = Settings.Default.tabOrderHesapDurumu.TabName, IsHide = Settings.Default.tabOrderHesapDurumu.IsHide });
            tabOrders.Add(new TabOrder() { Index = Settings.Default.tabOrderHizmetListe.Index, TabName = Settings.Default.tabOrderHizmetListe.TabName, IsHide = Settings.Default.tabOrderHizmetListe.IsHide });
            tabOrders.Add(new TabOrder() { Index = Settings.Default.tabOrderIgic.Index, TabName = Settings.Default.tabOrderIgic.TabName, IsHide = Settings.Default.tabOrderIgic.IsHide });
            tabOrders.Add(new TabOrder() { Index = Settings.Default.tabOrderLastName.Index, TabName = Settings.Default.tabOrderLastName.TabName, IsHide = Settings.Default.tabOrderLastName.IsHide });
            tabOrders.Add(new TabOrder() { Index = Settings.Default.tabOrderSettings.Index, TabName = Settings.Default.tabOrderSettings.TabName, IsHide = false });
            tabOrders.Add(new TabOrder() { Index = Settings.Default.tabOrderTesvik.Index, TabName = Settings.Default.tabOrderTesvik.TabName, IsHide = Settings.Default.tabOrderTesvik.IsHide });
            tabOrders.Add(new TabOrder() { Index = Settings.Default.tabOrderYillik.Index, TabName = Settings.Default.tabOrderYillik.TabName, IsHide = Settings.Default.tabOrderYillik.IsHide });

            tabOrders.Sort((a, b) => a.Index.CompareTo(b.Index));
            IOC.DesignHelper.OrderRibbonTabs(ribbonBar, tabOrders);
            ribbonBar.RibbonBarElement.TabStripElement.SelectedItem =   ribbonBar.RibbonBarElement.TabStripElement.Items[0];
            InitializeBgwSearch();
            InitializeBgwConfirm();
            InitializeBgwDetails();
            InitializeBgwSoyad();
            InitializeBgwSgk();
            InitializeBgwLinks();
            this.rgvLinkList.AllowSearchRow = true;
            this.rgvLinkList.CurrentRowChanging += RgvHelpers.GridViews_CurrentRowChanging;
            rgvCompanyList.CurrentRowChanging += RgvHelpers.GridViews_CurrentRowChanging;
            rgvLastName.CurrentRowChanging += RgvHelpers.GridViews_CurrentRowChanging;
            rgvLeaves.CurrentRowChanging += RgvHelpers.GridViews_CurrentRowChanging;
            rgvMossip.CurrentRowChanging += RgvHelpers.GridViews_CurrentRowChanging;
            rgvReportList.CurrentRowChanging += RgvHelpers.GridViews_CurrentRowChanging;
            rgvSgkOtomasyon.CurrentRowChanging += RgvHelpers.GridViews_CurrentRowChanging;
            rgvTemp.CurrentRowChanging += RgvHelpers.GridViews_CurrentRowChanging;
        }
        private void fBaslangic_Load(object sender, EventArgs e)
        {
            selectedRows = new GridViewSelectedRowsCollection();
            //IOC.sgkDataService.RemoveDuplicate();
            RgvHelpers.RgvLeaves = rgvLeaves;
            izinContex = new RadContextMenu();
            copyCell = new RadMenuItem("Hücreyi Kopyala","copyCell"); izinContex.Items.Add(copyCell); copyCell.Click += RgvHelpers.LeaveContext_Click;// LeaveContext_Click;
            copyRow = new RadMenuItem("Satırı Kopyala","copyRow"); izinContex.Items.Add(copyRow); copyRow.Click += RgvHelpers.LeaveContext_Click;
            copyAllRow = new RadMenuItem("Bütün Satırları Kopyala","copyAllRow"); izinContex.Items.Add(copyAllRow); copyAllRow.Click += RgvHelpers.LeaveContext_Click;
            edit = new RadMenuItem("Personel Bilgilerini Göster","edit"); izinContex.Items.Add(edit); edit.Click += RgvHelpers.LeaveContext_Click;
            expand = new RadMenuItem("Tümünü Genişlet","expand"); izinContex.Items.Add(expand); expand.Click += RgvHelpers.LeaveContext_Click;
            collapse = new RadMenuItem("Tümünü Daralt", "collapse"); izinContex.Items.Add(collapse); collapse.Click += RgvHelpers.LeaveContext_Click;
            showAll = new RadMenuItem("Personelin Bütün İzinleri Göster", "showAll"); izinContex.Items.Add(showAll); showAll.Click += RgvHelpers.LeaveContext_Click;
            showPeriod = new RadMenuItem("Bu Dönemin İzinlerini Göster", "showPeriod"); izinContex.Items.Add(showPeriod); showPeriod.Click += RgvHelpers.LeaveContext_Click;
            companyContex = new RadContextMenu();
            companyInfo = new RadMenuItem("Firma Bilgilerini Göster", "bilgi"); companyContex.Items.Add(companyInfo); companyInfo.Click += RgvHelpers.CompanyContext_Click;
            companyList = new RadMenuItem("Firma Listesini Göster","liste"); companyContex.Items.Add(companyList); companyList.Click += RgvHelpers.CompanyContext_Click;

            CheckForIllegalCrossThreadCalls = false;
            ThemeResolutionService.ApplicationThemeName = Settings.Default.theme;
            ribbonBar.RibbonBarElement.TabStripElement.SelectedItem = rtEvizite;
            ribbonTabs_Click(rtEvizite, null);
            FillCompanyGridView();
            CommonStartupOptions();
            FLogin f = new FLogin();
            if (f.ShowDialog() == DialogResult.Cancel)
            {
                Application.Exit();
            }
            if (GlobalVars.NewVersionfound)
            {
                DialogResult newVersion = RadMessageBox.Show("Asistpro'nun yeni sürümü bulundu, şimdi indirip yüklemek ister misiniz?", "Yeni Sürüm Bulundu", MessageBoxButtons.YesNo, RadMessageIcon.Question, "Eğer şu anda yüklemek istemiyorsanız daha sonra Ayarlar sekmesinden Hakkında butonuna basarak açılan iletişim penceresindeki Güncellemeler sekmesinden yeni sürümü indirebilirsiniz");
                if (newVersion == DialogResult.Yes)
                {
                    FAbout fa = new FAbout(true);
                    fa.ShowDialog();
                }
            }
            try
            {
                GridViewSearchRowInfo row = rgvLeaves.MasterView.TableSearchRow as GridViewSearchRowInfo;
                row.IsVisible = !row.IsVisible;
            }
            catch 
            {

            }
            
            DateTimeHelper.GetDateTime();
            if(!DateTimeHelper.Msg.Contains("Hata") && GlobalVars.Ct.Date < GlobalVars.LastLoginDate.Date)
            {
                RadMessageBox.Show("Bilgisayarınızın tarih saat ayarları hatalı ya da internet bağlantısı yok.\r\nLütfen tarih ve saati internet üzerinden otomatik olarak alınacak şekilde ayarlayın","Tarih saat hatalı", MessageBoxButtons.OK);
                Application.Exit();
            }
            //IOC.companyDataService.copycomp();
        }
        #region commonMethods
        private void btnSettings_Click(object sender, EventArgs e)
        {
            FSettings f = new FSettings(ribbonBar);
            f.ShowDialog();
        }
        private void ribbonBar_ExitButton_Click(object sender, EventArgs e)
        {
            Dispose();
        }
        private void CommonStartupOptions()
        {
            spCompanyList.SizeInfo.SizeMode = Telerik.WinControls.UI.Docking.SplitPanelSizeMode.Absolute;
            Size s = new Size();
            s.Width = 300;
            spCompanyList.SizeInfo.AbsoluteSize = s;

            this.ribbonBar.ShowLayoutModeButton = false;
            List<RadDropDownButtonElement> rddbeList = new List<RadDropDownButtonElement>() {btnPrintHESAP, btnPrintHLP, btnPrintLeave, btnExportLeave  };
            foreach (RadDropDownButtonElement btn in rddbeList)
            {
                btn.BorderElement.Visibility = ElementVisibility.Visible;
                btn.BorderElement.ForeColor = Color.Black;
                btn.ShowArrow = false;
                btn.ElementTree.Control.Cursor = Cursors.Hand;
                //btn.BorderElement.Shape = customShape1;
            }
            List<RadButtonElement> btnList = new List<RadButtonElement>() { btnStartSearch, btnStartProcess, btnExcel, btnPrint, btnVisitLink, btnRefreshLinks, btnSortMainGroup, btnSortSubGroup, btnSortName, btnSortFav, btnGetPD, btnGetPDFromDB, btnEborcuYoktur, btnGetET, btnGetEtFromDb, btnGetCR, btnGetCrFromDB, btnGet6661, btnGet6661FromDB, btnExcelHESAP, btnGetIGL, btnGetIGLFromDB, btnPrintIGL, btnExcelIGL, btnGetHL, btnGetHlFromDB, btnDeleteHL, btnHlSeperate, btnHlAside, btnAnalyzeHL, btnExcelHLP, btnPaste, btnClean, btnStartSoyad, btnCompanyList, btnAddCompany, btnAddCompanies, btnUsers, btnAddUser, btnSettings, btnAbout, btnPasteTesvik, btnCleanTesvik, btnProcessTesvik, btnAdvertTesvik, btnLeaveSearch,  btnAddLeave, btnAddLeaves, btnPersonals};
            
            foreach (RadButtonElement btn in btnList)
            {
                btn.BorderElement.Visibility = ElementVisibility.Visible;
                btn.BorderElement.ForeColor = Color.Black;
                btn.ShowBorder = true;
                btn.ElementTree.Control.Cursor = Cursors.Hand;
            }
            List<RadButtonElement> radButtonElementss = new List<RadButtonElement>() { btnConfirmResult, btnSoyadUpdateReport, btnHideMessage, btnOpenFile };
            foreach (RadButtonElement btn in radButtonElementss)
            {
                btn.BorderElement.Visibility = ElementVisibility.Visible;
                btn.BorderElement.ForeColor = Color.FromArgb(120, 148, 186);
                btn.ForeColor = Color.FromArgb(21, 66, 139);
                btn.ShowBorder = true;
                btn.ElementTree.Control.Cursor = Cursors.Hand;
            }
            List<RadButton> radButtons = new List<RadButton>() { btnStartSoyad2, btnAddPerson};
            foreach (RadButton btn in radButtons)
            {
                btn.ButtonElement.BorderElement.Visibility = ElementVisibility.Visible;
                btn.ButtonElement.BorderElement.ForeColor = Color.FromArgb(120, 148, 186);
                btn.ButtonElement.ShowBorder = true;
                btn.ForeColor = Color.FromArgb(21, 66, 139);
                btn.ElementTree.Control.Cursor = Cursors.Hand;
            }
            List<RadGridView> listRgv = new List<RadGridView>() { rgvCompanyList, rgvLastName, rgvLinkList, rgvMossip, rgvReportList, rgvSgkOtomasyon, rgvLeaves };
            foreach (RadGridView rgv in listRgv)
            {
                rgv.TableElement.SearchHighlightColor = Color.OrangeRed;
                rgv.GridViewElement.TitleLabelElement.Font = new Font(Settings.Default.rgvTitleFontName, Settings.Default.rgvTitleFontSize, Settings.Default.rgvTitleFontStyle); 
                rgv.GridViewElement.TitleLabelElement.ForeColor = Settings.Default.rgvTitleFontColor;
                rgv.GridViewElement.TableElement.Font = new Font(Settings.Default.rgvFontName, Settings.Default.rgvFontSize, Settings.Default.rgvFontStyle); 
                rgv.GridViewElement.TableElement.ForeColor = Settings.Default.rgvFontColor;
                rgv.TableElement.BackColor = rgv != rgvCompanyList? Color.Transparent : Color.White;
                rgv.BackgroundImageLayout = ImageLayout.Center;
                rgv.ShowNoDataText = false;
            }
            LinkGlobals.BrowserType = Settings.Default.browser;
            GlobalVars.DisposeDriver = Settings.Default.disposeDriver;
            GlobalVars.AutoCaptcha = Settings.Default.autoCaptcha;
            GlobalVars.PublicHolidays = Settings.Default.publicHolidays;
            rgvReportList.BackgroundImage = Resources.evizite;
            cbHLDeleteRecords.ToolTipText = "Seçilen dönem aralığına ait pdf dosyaları daha önce indirilmişse eski dosyaları silip yeniden indirir.";
            this.Text = $"ASİSTPRO V{Assembly.GetExecutingAssembly().GetName().Version}";
            LinkGlobals.LstLinks = IOC.LinksDataService.GetAllLinks(out msg);
        }
        private void ViziteStartupOptions()
        {

            reportType = ReportTypeEnum.RaporTarihi;
            rgvReportList.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            rgvReportList.GridViewElement.TitleLabelElement.Padding = new Padding(0, 2, 0, 2);

            pnlWaitVizite.Visible = false;
            lblMessage.Visibility = ElementVisibility.Collapsed;
            btnOpenFile.Visibility = ElementVisibility.Collapsed;
            btnHideMessage.Visibility = ElementVisibility.Collapsed;
            btnConfirmResult.Visibility = ElementVisibility.Collapsed;
            btnSoyadUpdateReport.Visibility = ElementVisibility.Collapsed;
            texTckn.Visibility = ElementVisibility.Collapsed;
            btnStartProcess.Enabled = false;

            ddlVaka.DataSource = IOC.DesignHelper.SetVakaComboBox();
            ddlVaka.DisplayMember = "key";
            ddlVaka.ValueMember = "value";
            ddlVaka.SelectedIndex = 3;

            texTckn.TextBoxItem.GotFocus += texTckn_Enter;

            dtpSearchDate.CustomFormat = "dd.MM.yyyy";
            dtpSearchDate.Format = DateTimePickerFormat.Custom;
            dtpSearchDate.MinDate = new DateTime(1990, 1, 1);
            dtpSearchDate.MaxDate = DateTime.Today;
            RadHostItem dtpSearchDateHost = new RadHostItem(dtpSearchDate);
            
            this.rbbgTcnoDate.Children.Add(dtpSearchDateHost);

            dtpViziteFirst.CustomFormat = "dd.MM.yyyy";
            dtpViziteFirst.Format = DateTimePickerFormat.Custom;
            dtpViziteFirst.MinDate = new DateTime(1990, 1, 1);
            dtpViziteFirst.MaxDate = DateTime.Today;
            RadHostItem dtpStartDateHost = new RadHostItem(dtpViziteFirst);
            dtpStartDateHost.AutoSize = false;
            dtpStartDateHost.Size = new Size(150, 20);
            this.rbbgReportStartDate.Children.Add(dtpStartDateHost);

            dtpViziteEnd.CustomFormat = "dd.MM.yyyy";
            dtpViziteEnd.Format = DateTimePickerFormat.Custom;
            dtpViziteEnd.MinDate = new DateTime(1990, 1, 1);
            dtpViziteEnd.MaxDate = DateTime.Today;
            RadHostItem dtpEndDateHost = new RadHostItem(dtpViziteEnd);
            dtpEndDateHost.AutoSize = false;
            dtpEndDateHost.Size = new Size(150, 20);
            this.rbbgReportEndDate.Children.Add(dtpEndDateHost);

            dtpSearchDate.ValueChanged += dtp_ValueChanged;
            dtpViziteFirst.ValueChanged += dtp_ValueChanged;
            dtpViziteEnd.ValueChanged += dtp_ValueChanged;


            checkPersonelimDegil.ToolTipText = "Personelin raporlu olduğu günlerde, firmanın elemanı olup olmadığını kontrol eder.\r\nBu seçenek tarama işleminin süresini uzatabilir!";
            checkDownloadPdf.ToolTipText = $"Rapor onaylandıktan sonra onay dökümünü bilgisayarınızın {SearchReport.DocumentsDir} klasörüne\r\nkaydeder. Dosya adını \"Firma Adı-Ad Soyad-Rapor Takip No-Rapor Sıra No.pdf\" olarak ayarlar";
            checkReportDetails.ToolTipText = "Rapor üzerine çift tıklama SGK sunucularından rapor ayrıntıları getirir ve ayrı bir pencerede görüntüler";
            
            viziteFirtOpen = false;

        }
        private void LinksStartupOptions()
        {
            string msg = "";
            rgvLinkList.MasterTemplate.CaseSensitive = false;
            rgvCompanyList.MasterTemplate.CaseSensitive = false;

            IOC.WinHelpers.FillRgvWithLinks(rgvLinkList);
            IOC.LinkOps.SetSizesForLinkListRgv(rgvLinkList, rgvCompanyList, spCompanyList);
            IOC.LinkOps.FavColumnSet(rgvLinkList, out msg);
            rgvLinkList.CurrentRow = null;
            linksFirtOpen = false;
            lblMessage.Text = msg;
        }
        private void SoyadStartupOptions()
        {
            pnlWaitSoyad.Visible = false;
            rwbLastNames.StopWaiting();
            rgvLastName.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            btnStartSoyad.Enabled = false;
            btnSoyadUpdateReport.Visibility = ElementVisibility.Collapsed;
            soyadFirstOpen = false;
        }
        private void HesapDurumuStartupOptions()
        {
            pnlWaitHESAP.Visible = false;
            spRgvMossip.Collapsed = true;
            int cy = DateTime.Now.Year;
            Dictionary<string, string> dc6661 = new Dictionary<string, string>();
            for (int i = cy; i >= 2016; i--)
            {
                dc6661.Add(i.ToString(), i.ToString());
            }
            ddl6661Year.DataSource = dc6661;
            ddl6661Year.DisplayMember = "key";
            ddl6661Year.ValueMember = "value";
            ddl6661Year.SelectedIndex = 0;


            sgkFirstOpen = false;
        }
        private void HlStartupOptions()
        {
            pnlWaitHESAP.Visible = false;
            spRgvMossip.Collapsed = true;

            int cy = DateTime.Now.Year;
            Dictionary<string, int> dcYear = new Dictionary<string, int>();
            for (int i = cy; i >= 1900; i--)
            {
                dcYear.Add(i.ToString(), i);
            }
            ddlHlStartYear.DataSource = dcYear;
            ddlHlStartYear.DisplayMember = "key";
            ddlHlStartYear.ValueMember = "value";
            ddlHlStartYear.SelectedIndex = 0;

            ddlHlEndYear.DataSource = dcYear;
            ddlHlEndYear.DisplayMember = "key";
            ddlHlEndYear.ValueMember = "value";
            ddlHlEndYear.SelectedIndex = 0;

            Dictionary<string, int> dcMounth = new Dictionary<string, int>();
            dcMounth.Add("OCAK", 1);
            dcMounth.Add("ŞUBAT", 2);
            dcMounth.Add("MART", 3);
            dcMounth.Add("NİSAN", 4);
            dcMounth.Add("MAYIS", 5);
            dcMounth.Add("HAZİRAN", 6);
            dcMounth.Add("TEMMUZ", 7);
            dcMounth.Add("AĞUSTOS", 8);
            dcMounth.Add("EYLÜL", 9);
            dcMounth.Add("EKİM", 10);
            dcMounth.Add("KASIM", 11);
            dcMounth.Add("ARALIK", 12);

            ddlHlStartMount.DataSource = dcMounth;
            ddlHlStartMount.DisplayMember = "key";
            ddlHlStartMount.ValueMember = "value";
            ddlHlStartMount.SelectedIndex = 0;

            ddlHlEndMount.DataSource = dcMounth;
            ddlHlEndMount.DisplayMember = "key";
            ddlHlEndMount.ValueMember = "value";
            ddlHlEndMount.SelectedIndex = 0;

            hlFirstOpen = false;
        }
        private void IglStartupOptions()
        {
            dtpIglFirst.CustomFormat = "dd.MM.yyyy";
            dtpIglFirst.Format = DateTimePickerFormat.Custom;
            dtpIglFirst.MinDate = new DateTime(2014, 6, 7);
            dtpIglFirst.MaxDate = DateTime.Today;
            RadHostItem datePickerHost = new RadHostItem(dtpIglFirst);
            this.rbbgIGLStartDate.Children.Add(datePickerHost);

            dtpIglLast.CustomFormat = "dd.MM.yyyy";
            dtpIglLast.Format = DateTimePickerFormat.Custom;
            dtpIglLast.MinDate = new DateTime(2014, 6, 7);
            dtpIglLast.MaxDate = DateTime.Today;
            RadHostItem datePickerHost2 = new RadHostItem(dtpIglLast);
            this.rbbgIGLEndDate.Children.Add(datePickerHost2);

            dtpIglFirst.ValueChanged += dtp_ValueChanged;
            dtpIglLast.ValueChanged += dtp_ValueChanged;
            iglFirstOpen = false;
        }
        private void LeaveStartupOptions()
        {
            texLeaveTckn = new RadTextBoxControl();
            texLeaveTckn.MaxLength = 11;
            texLeaveTckn.KeyUp += textAddTcno_KeyUp;
            texLeaveTckn.KeyPress += textAddTcno_KeyPress;
            texLeaveTckn.TextChanged += textAddTcno_TextChanged;
            texLeaveTckn.Enter += TexLeaveTckn_Enter;
            RadHostItem tcknHost = new RadHostItem(texLeaveTckn);

            this.rbbgLeaveTckn.Children.Add(tcknHost);

            dtpLeaveFirst.CustomFormat = "dd.MM.yyyy";
            dtpLeaveFirst.Format = DateTimePickerFormat.Custom;
            dtpLeaveFirst.MinDate = new DateTime(1980, 1, 1);
            dtpLeaveFirst.MaxDate = DateTime.Today.AddYears(30);
            RadHostItem datePickerHostLeave = new RadHostItem(dtpLeaveFirst);
            this.rbbgLeaveStartDate.Children.Add(datePickerHostLeave);

            dtpLeaveLast.CustomFormat = "dd.MM.yyyy";
            dtpLeaveLast.Format = DateTimePickerFormat.Custom;
            dtpLeaveLast.MinDate = new DateTime(1980, 1, 1);
            dtpLeaveLast.MaxDate = DateTime.Today.AddYears(30);
            RadHostItem datePickerHostLeave2 = new RadHostItem(dtpLeaveLast);
            this.rbbgLeaveEndDate.Children.Add(datePickerHostLeave2);

            dtpLeaveFirst.ValueChanged += dtp_ValueChanged;
            dtpLeaveLast.ValueChanged += dtp_ValueChanged;

            btnPrintPersonals.Click += btnPrint_Click;
            btnPrintPersonalPeriods.Click += btnPrint_Click;
            btnPrintPersonalsPeriods.Click += btnPrint_Click;

            btnExportPersonals.Click += btnExcel_Click;
            btnExportPersonalPeriods.Click += btnExcel_Click;
            btnExportPersonalsPeriods.Click += btnExcel_Click;

            leaveFirstOpen = false;
        }
        private void TexLeaveTckn_Enter(object sender, EventArgs e)
        {
            rbLeavesTCKN.CheckState = CheckState.Checked;
            rbLeavesSearchType_Click(rbLeavesTCKN, new EventArgs() );
        }
        private void ribbonTabs_Click(object sender, EventArgs e)
        {
            statusStript.Visible = true; 
            btnSoyadUpdateReport.Visibility = ElementVisibility.Collapsed;
            btnOpenFile.Visibility = ElementVisibility.Collapsed;
            btnConfirmResult.Visibility = ElementVisibility.Collapsed;
            btnHideMessage.Visibility = ElementVisibility.Collapsed;

            selectedRows = rgvCompanyList.SelectedRows;
            //List<int> selectedRowsIndexes = (from x in selectedRows select x.Index).ToList();
            List<int> selectedRowsIndexes = (from x in selectedRows select Convert.ToInt32(x.Cells["Id"].Value)).ToList();
            rgvCompanyList.MultiSelect = true;
            if (sender == rtEvizite)
            {
                spLinks.Collapsed = true;
                spSoyad.Collapsed = true;
                spReports.Collapsed = false;
                spSgkOtomasyon.Collapsed = true;
                spSgkCheckUp.Collapsed = true;
                spLeaves.Collapsed = true;
                modulType = ModulTypeEnum.Vizite;
                if (viziteFirtOpen) {
                    ViziteStartupOptions();
                    ShowOptionParameters(reportType);
                }
            }
            else if (sender == rtLinks)
            {
                spLinks.Collapsed = false;
                spSoyad.Collapsed = true;
                spReports.Collapsed = true;
                spSgkOtomasyon.Collapsed = true;
                spLeaves.Collapsed = true;
                btnVisitLink.Image = Settings.Default.browser == 0 ? Resources.firefox_64 : Resources.chrome_64;
                IOC.SgkLinksService.SetAutoCaptcha(false);
                spSgkCheckUp.Collapsed = true;
                modulType = ModulTypeEnum.Links;

                if (linksFirtOpen) LinksStartupOptions();
            }
            else if (sender == rtLastName)
            {
                spLinks.Collapsed = true;
                spSoyad.Collapsed = false;
                spReports.Collapsed = true;
                spSgkOtomasyon.Collapsed = true;
                spSgkCheckUp.Collapsed = true;
                spLeaves.Collapsed = true;
                modulType = ModulTypeEnum.Lastname;
                rgvLastName.BackgroundImage =  Resources.lastname;
                btnSoyadUpdateReport.Text = "Güncelleme Raporu";
                rwbLastNames.Text = "Soyadı Güncelleme İşlemi Yapılıyor";
                btnCancelSoyad.Text = "Güncellemeyi İptal Et";
                if (soyadFirstOpen) SoyadStartupOptions();
                btnStartSoyad2.Image = Resources.start;
                btnStartSoyad2.Text = "Soyadı Güncelleme İşlemini Başlat";
            }
            else if (sender == rtTesvik) {
                spLinks.Collapsed = true;
                spSoyad.Collapsed = false;
                spReports.Collapsed = true;
                spSgkOtomasyon.Collapsed = true;
                spSgkCheckUp.Collapsed = true;
                spLeaves.Collapsed = true;
                modulType = ModulTypeEnum.Tesvik;
                rgvLastName.BackgroundImage = Resources.tesvik;
                btnSoyadUpdateReport.Text = "Sorgulama Raporu";
                rwbLastNames.Text = "Potansiyel Teşvik Sorgulama İşlemi Yapılıyor";
                btnCancelSoyad.Text = "Sorgulamayı İptal Et";
                if (soyadFirstOpen) SoyadStartupOptions();
                
                btnStartSoyad2.Image = Resources.search;
                btnStartSoyad2.Text = "Potansiyel Teşvik Sorgulamayı Başlat";
            }
            else if (sender == rtHesapDurumu)
            {
                spLinks.Collapsed = true;
                spSoyad.Collapsed = true;
                spReports.Collapsed = true;
                spSgkOtomasyon.Collapsed = false;
                spSgkCheckUp.Collapsed = true;
                spLeaves.Collapsed = true;
                spRgvMossip.Collapsed = rgvMossip.DataSource != null ? false : true;
                rgvSgkOtomasyon.BackgroundImage = rgvMossip.DataSource != null ? null : Resources.hesapdurumu;
                modulType = ModulTypeEnum.Hesapdurumu;
                if (sgkFirstOpen) HesapDurumuStartupOptions();
                FreshRgvSgkOtomasyon(); FreshRgvSgkMossip();
            }
            else if (sender == rtHizmetListe)
            {
                spLinks.Collapsed = true;
                spSoyad.Collapsed = true;
                spReports.Collapsed = true;
                spSgkOtomasyon.Collapsed = false;
                spLeaves.Collapsed = true;
                spRgvMossip.Collapsed = true;
                rgvSgkOtomasyon.BackgroundImage = rgvMossip.DataSource != null ? null : Resources.hizmetlistesi;
                rgvMossip.BackgroundImage = rgvMossip.DataSource != null ? null : Resources.hizmetlistesipersonel;
                spSgkCheckUp.Collapsed = true;
                modulType = ModulTypeEnum.Hizmet;
                if (rbHizmetListesi.CheckState == CheckState.Checked) rbHizmetListesi_rbTahakkuk_Click(rbHizmetListesi, null);
                else if (rbHizmetListesiUnpaid.CheckState == CheckState.Checked) rbHizmetListesi_rbTahakkuk_Click(rbHizmetListesiUnpaid, null);
                else if (rbTahakkuk.CheckState == CheckState.Checked) rbHizmetListesi_rbTahakkuk_Click(rbTahakkuk, null);
                if (hlFirstOpen) HlStartupOptions(); FreshRgvSgkOtomasyon(); FreshRgvSgkMossip();
            }
            else if (sender == rtOptions) {
                modulType = ModulTypeEnum.Options;
                if(Users.ActiveUser.Yetki == 1) { 
                    btnAddCompany.Enabled = true;
                    btnAddCompanies.Enabled = (rgvCompanyList.RowCount > 0) ? true : false;
                    btnCompanyList.Enabled = true;
                    btnAddUser.Enabled = true;
                }
                else
                {
                    btnAddCompany.Enabled = false;
                    btnAddCompanies.Enabled = false;
                    btnCompanyList.Enabled = false;
                    btnAddUser.Enabled = false;
                }
            }
            else if (sender == rtIgic)
            {
                spLinks.Collapsed = true;
                spSoyad.Collapsed = true;
                spReports.Collapsed = true;
                spSgkOtomasyon.Collapsed = false;
                spRgvMossip.Collapsed = true;
                spSgkCheckUp.Collapsed = true;
                spLeaves.Collapsed = true;
                modulType = ModulTypeEnum.Igl;
                rgvSgkOtomasyon.BackgroundImage = rgvSgkOtomasyon.DataSource != null ? null : Resources.igic;
                if (iglFirstOpen) IglStartupOptions(); FreshRgvSgkOtomasyon();
            }
            else if (sender == rtYillik)
            {
                GlobalVars.Personals = IOC.PersonalDataService.GetAllPersonals(out msg);
                
                //rgvCompanyList.MultiSelect = false;
                //rgvCompanyList.Rows[0].IsSelected = true;
                if (leaveFirstOpen) LeaveStartupOptions();
                texLeaveTckn.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                texLeaveTckn.AutoCompleteDataSource = (from x in GlobalVars.Personals select x.Tcno).ToList();
                spLinks.Collapsed = true;
                spSoyad.Collapsed = true;
                spReports.Collapsed = true;
                spSgkOtomasyon.Collapsed = true;
                spRgvMossip.Collapsed = true;
                spSgkCheckUp.Collapsed = true;
                spLeaves.Collapsed = false;
                modulType = ModulTypeEnum.Izin;
                rgvLeaves.BackgroundImage =  null;
                FreshRgvLeaves();
                //rgvCompanyList.SelectedRows[0].Cells[1].Value.ToString();
                //GlobalVars.IzinComp = IOC.WinHelpers.GetSelectedCompaniesFromRgv(rgvCompanyList, out msg).First();
            }
            //selectedRowsIndexes.Reverse();
            foreach (int index in selectedRowsIndexes)
            {
                if (index == -1) continue;
                foreach (GridViewRowInfo row in rgvCompanyList.Rows)
                {
                    if (Convert.ToInt32(row.Cells["Id"].Value) == index) 
                    { row.IsSelected = true; break; }
                }
                //rgvCompanyList.Rows[Index].IsSelected = true;
            }
        }
        private void rgvCompanyList_FilterExpressionChanged(object sender, FilterExpressionChangedEventArgs e)
        {
            e.FilterExpression = e.FilterExpression.ToUpper();
        }
        private void rgvCompanyList_SelectionChanged(object sender, EventArgs e)
        {
            if (rgvCompanyList.SelectedRows.Count > 0)
            {
                btnStartSearch.Enabled = true; btnVisitLink.Enabled = true; btnStartSoyad.Enabled = true;
                rgvCompanyList.TitleText = $"{rgvCompanyList.SelectedRows.Count} TANE FİRMA SEÇİLDİ";
            }
            else
            {
                btnStartSearch.Enabled = false; btnVisitLink.Enabled = false; btnStartSoyad.Enabled = false;
            }

            if (modulType == ModulTypeEnum.Links && rgvCompanyList.SelectedRows.Count > 5)
            {
                List<GridViewRowInfo> selectedRows = new List<GridViewRowInfo>();
                int counter = 1;
                foreach (var row in rgvCompanyList.SelectedRows)
                {
                    if (counter > 5)
                    {
                        break;
                    }
                    selectedRows.Add(row);
                    counter++;
                }
                rgvCompanyList.ClearSelection();
                foreach (var item in selectedRows)
                {
                    item.IsSelected = true;
                }
            }
            if(modulType == ModulTypeEnum.Izin && rgvCompanyList.SelectedRows.Count > 0) {
                GlobalVars.IzinComp = IOC.WinHelpers.GetSelectedCompaniesFromRgv(rgvCompanyList, out msg).First(); 
            }
        }
        private void rgvCompanyList_MouseEnter(object sender, EventArgs e)
        {
            string msg;
            try
            {
                if (Changed.HasChanged == true)
                {
                    bool yuklendi = IOC.WinHelpers.SetCompanyListRgv(rgvCompanyList, out msg);
                    if (!yuklendi)
                    {
                        DialogResult dr = RadMessageBox.Show($"Veri tabanından firma listesi yüklenmesi başarısız oldu ({msg})\r\nTekrar denemek için \"Tekrar Dene\" butonuna basın", "Firma Listesi Yüklenemedi!", MessageBoxButtons.RetryCancel);
                        if (dr == DialogResult.Retry)
                        {
                            rgvCompanyList_MouseEnter(null, null);
                        }
                        else
                        {
                            Application.Exit();
                        }
                    }

                    btnAddCompanies.Enabled = (rgvCompanyList.RowCount > 0 && Users.ActiveUser.Yetki == 1) ? true : false;
                    Changed.HasChanged = false;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }
        private void rgvCompany_MouseMove(object sender, MouseEventArgs e)
        {
            if (hoverCellCompany != null && hoverCellCompany.RowInfo != null)
            {
                hoverCellCompany.RowInfo.Tag = null;
                hoverCellCompany.RowInfo.InvalidateRow();
            }

            GridDataCellElement dataCellElement = rgvCompanyList.ElementTree.GetElementAtPoint(e.Location) as GridDataCellElement;
            if (dataCellElement != null && dataCellElement.RowInfo != null)
            {
                dataCellElement.RowInfo.Tag = "HoverMeFlag";
                hoverCellCompany = dataCellElement;
                dataCellElement.RowInfo.InvalidateRow();
            }
        }
        private void rgvCompanyList_MouseLeave(object sender, EventArgs e)
        {
            if (rgvCompanyList.Rows.Count == 0 ) return;
            foreach (GridViewRowInfo rowInfo in rgvCompanyList.Rows)  { rowInfo.Tag = "";      }
            if (rgvCompanyList.SelectedRows.Count == 0)
                rgvCompanyList.Rows[0].IsSelected = true;
        }
        private void GridViews_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            string cin = e.CellElement.ColumnInfo.Name; 
            if (e.CellElement.MasterTemplate.Owner == rgvCompanyList || e.CellElement.MasterTemplate.Owner == rgvLeaves)
            {
                if (e.CellElement is GridDataCellElement && e.CellElement.RowInfo.Tag != null && e.CellElement.RowInfo.Tag.ToString() == "HoverMeFlag")
                {
                    e.CellElement.BackColor = Color.Red;
                    e.CellElement.GradientStyle = GradientStyles.Solid;
                    e.CellElement.ForeColor = Color.Black;
                    e.CellElement.DrawFill = true;
                }
                else if (e.Row.IsSelected || (e.CellElement.RowInfo.Tag != null && e.CellElement.RowInfo.Tag.ToString() == "SelectMeFlag") )
                {
                    e.CellElement.DrawFill = true;
                    e.CellElement.ForeColor = Color.White;
                    e.CellElement.GradientStyle = GradientStyles.Solid;
                    e.CellElement.BackColor = Color.Black;
                    e.CellElement.BorderBoxStyle = BorderBoxStyle.FourBorders;
                    e.CellElement.BorderLeftColor = Color.White;
                    e.CellElement.BorderRightColor = Color.White;
                    //e.Row.IsSelected = true;
                }
                else
                {
                    e.CellElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                    e.CellElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                    e.CellElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
                    e.CellElement.ResetValue(LightVisualElement.BorderBoxStyleProperty, ValueResetFlags.Local);
                    e.CellElement.ResetValue(LightVisualElement.BorderRightColorProperty, ValueResetFlags.Local);
                    e.CellElement.ResetValue(LightVisualElement.BorderLeftColorProperty, ValueResetFlags.Local);
                    e.CellElement.ForeColor = Color.Black; 
                }
            }
            else if (e.CellElement.MasterTemplate.Owner == rgvSgkOtomasyon && hesapType == HesapTypeEnum.Donemselborc)
            {
                
                if (cin == "Cn" || cin == "Yil" || cin == "Ay" || cin == "Drm")
                {
                    e.CellElement.Padding = new Padding(20, 0, 0, 0);
                }
                else if ( cin == "Pb" || cin == "PbGz" || cin == "Ipcb" || cin == "IpcbGz" || cin == "Ekpb" || cin == "EkpbGz" || cin == "Oivb" || cin == "OivbGz" || cin == "Ib" || cin == "IbGz" || cin == "Dvb" || cin == "DvbGz" || cin == "Dpgzb" || cin == "DpgzbGz" || cin == "Dekpgzb" || cin == "Sbt" || cin == "SbtGz")
                {
                    e.CellElement.Padding = new Padding(0, 0, 20, 0);
                }
            }
            else if (e.CellElement.MasterTemplate.Owner == rgvSgkOtomasyon && hesapType == HesapTypeEnum.Emanet)
            {
                if (e.CellElement.ColumnInfo.Name == "Bt")
                {
                    e.CellElement.Padding = new Padding(20, 0, 0, 0);
                }
                else if (e.CellElement.ColumnInfo.Name == "Ttr")
                {
                    e.CellElement.Padding = new Padding(0, 0, 20, 0);
                }
            }
            else if (e.CellElement.MasterTemplate.Owner == rgvSgkOtomasyon && hesapType == HesapTypeEnum.Icra)
            {
                if (e.CellElement.ColumnInfo.Name == "Ba" || e.CellElement.ColumnInfo.Name == "Gz" || e.CellElement.ColumnInfo.Name == "Tm" || e.CellElement.ColumnInfo.Name == "Tp")
                {
                    e.CellElement.Padding = new Padding(0, 0, 20, 0);
                }
            }
            else if (e.CellElement.MasterTemplate.Owner == rgvSgkOtomasyon && hesapType == HesapTypeEnum.Destek6661)
            {
                if (e.CellElement.ColumnInfo.Name == "Dt")
                {
                    e.CellElement.Padding = new Padding(0, 0, 20, 0);
                }
            }
            else if (e.CellElement.MasterTemplate.Owner == rgvMossip && hesapType == HesapTypeEnum.Emanet)
            {
                if (e.CellElement.ColumnInfo.Name == "Etr")
                {
                    e.CellElement.Padding = new Padding(0, 0, 20, 0);
                }
                else if (e.CellElement.ColumnInfo.Name == "Tur")
                {
                    e.CellElement.Padding = new Padding(20, 0, 0, 0);
                }
            }
            
            else if (e.CellElement.MasterTemplate.Owner == rgvReportList)
            {
                if (e.CellElement is GridDateTimeCellElement)
                {
                    GridDateTimeCellElement cell = (GridDateTimeCellElement)e.CellElement;
                    if (cell.Value == null || cell.Value == System.DBNull.Value)
                    {
                        cell.Text = "null";
                    }
                }
            }

            if (e.CellElement.MasterTemplate.Owner == rgvLeaves)
            {
                if (e.Column.Name == "Ict" && Convert.ToBoolean(e.Row.Cells["Active"].Value))
                {
                    e.CellElement.ForeColor = Color.Transparent;
                    e.CellElement.Enabled = false;
                    e.CellElement.DrawText = false;
                }
                else if (e.Column.Name == "Ict" && !Convert.ToBoolean(e.Row.Cells["Active"].Value))
                {
                    e.CellElement.ForeColor = e.CellElement.BackColor == Color.Black? Color.White : Color.Black;
                    e.CellElement.Enabled = true;
                    e.CellElement.DrawText = true;
                }

                if (e.CellElement.ColumnInfo.Name == "Ads")
                {
                    e.CellElement.Padding = new Padding(5, 0, 0, 0);
                }
                else if (e.CellElement.ColumnInfo.Name == "Tih")
                {
                    e.CellElement.Padding = new Padding(0, 0, 100, 0);
                }
            }
            if (e.CellElement.MasterTemplate.Owner == rgvSgkOtomasyon && modulType == ModulTypeEnum.Hizmet)
            {
                if (e.CellElement.ColumnInfo.Name == "Bt" || e.CellElement.ColumnInfo.Name == "Bm" || e.CellElement.ColumnInfo.Name == "Kn") e.CellElement.Padding = new Padding(20, 0, 0, 0);
                else if(e.CellElement.ColumnInfo.Name == "Tcs" || e.CellElement.ColumnInfo.Name == "Tgs" || e.CellElement.ColumnInfo.Name == "Tpt") e.CellElement.Padding = new Padding(0, 0, 20, 0);
            }
            if (e.CellElement.MasterTemplate.Owner == rgvMossip && modulType == ModulTypeEnum.Hizmet)
            {
                if (cin == "Cn" || cin == "Ads" || cin == "Mk" || cin == "Bm" || cin == "Bt" || cin == "Kk") e.CellElement.Padding = new Padding(20, 0, 0, 0);
                else if (cin == "Utl" || cin == "Itl" || cin == "Gun" || cin == "Ucg" || cin == "EGun" || cin == "GGun" || cin == "CGun" || cin == "Icn" || cin == "Egn") e.CellElement.Padding = new Padding(0, 0, 20, 0);
            }
            if (e.CellElement.MasterTemplate.Owner == rgvSgkOtomasyon && modulType == ModulTypeEnum.Tahakkuk && hesapType == HesapTypeEnum.Tahakkuk)
            {
                if (cin == "Cn" || cin == "Sgm" || cin == "Bm") e.CellElement.Padding = new Padding(20, 0, 0, 0);
                else if (cin == "Tp" || cin == "Ip" || cin == "Kn14857" || cin == "Kn15921" || cin == "Kn6645" || cin == "Kn15510" || cin == "Kn2828" || cin == "Kn6111" || cin == "Kn17103" || cin == "Kn17103i" || cin == "Kn27103" || cin == "Kn7252" || cin == "Kn17256" || cin == "Kn7316" || cin == "Kn7319" || cin == "Kn5510" || cin == "Kn4857" || cin == "Kn159210" || cin == "Kn3294") e.CellElement.Padding = new Padding(0, 0, 20, 0);
            }
            if (e.CellElement.MasterTemplate.Owner == rgvSgkOtomasyon && modulType == ModulTypeEnum.Igl)
            {
                if(cin == "Cn" || cin == "Tc" || cin == "Ads" || cin == "Gc" || cin == "Isl" || cin == "Isa") e.CellElement.Padding = new Padding(20, 0, 0, 0);
                else if(cin == "Stn" || cin == "Ipc") e.CellElement.Padding = new Padding(0, 0, 20, 0);
            }
        }
        private void GridViews_ViewRowFormatting(object sender, RowFormattingEventArgs e)
        {
            
            if (e.RowElement is GridSummaryRowElement)
            {
                e.RowElement.RowInfo.Height = 30; 
            }
            if (e.RowElement is GridFilterRowElement)
            {
                e.RowElement.RowInfo.Height = 34;
                e.RowElement.BorderBoxStyle = BorderBoxStyle.FourBorders;
                e.RowElement.BorderWidth = 2;
                e.RowElement.BorderColor = Color.Red;
            }
            if (e.RowElement.MasterTemplate.Owner == rgvCompanyList || e.RowElement.MasterTemplate.Owner == rgvLeaves)
            {
                if (e.RowElement.IsSelected) 
                {
                    e.RowElement.DrawFill = true; 
                    e.RowElement.GradientStyle = GradientStyles.Solid;
                    e.RowElement.BackColor = Color.Black; // It doesn't work !
                    e.RowElement.ForeColor = Color.White; // It works
                }
                else
                {
                    e.RowElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                    e.RowElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                    e.RowElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
                    e.RowElement.ResetValue(LightVisualElement.ForeColorProperty, ValueResetFlags.Local);
                }
            }
            else if (e.RowElement.MasterTemplate.Owner == rgvLastName)
            {
                e.RowElement.DrawFill = true;
                rgvLastName.BackgroundImage = Resources.lastname;
                rgvLastName.TableElement.BackColor = Color.Transparent;
                rgvLastName.BackgroundImageLayout = ImageLayout.Center;
                rgvLastName.ShowNoDataText = false;
                if (modulType == ModulTypeEnum.Lastname)
                {
                    rgvLastName.BackgroundImage = Resources.lastname;
                }
                else if (modulType == ModulTypeEnum.Tesvik)
                {
                    rgvLastName.BackgroundImage = Resources.tesvik;
                }
            }
            else if (e.RowElement.MasterTemplate.Owner == rgvLinkList)
            {

            }
            else if (e.RowElement.MasterTemplate.Owner == rgvMossip)
            {
                if (rgvMossip.DataSource == null)
                {
                    e.RowElement.DrawFill = false;
                    rgvMossip.BackgroundImage = Resources.hizmetlistesi;
                    rgvMossip.TableElement.BackColor = Color.Transparent;
                    rgvMossip.BackgroundImageLayout = ImageLayout.Center;
                    rgvMossip.ShowNoDataText = false;
                }
                else
                {
                    //e.RowElement.DrawFill = true;
                    rgvMossip.BackgroundImage = null;
                }
            }
            else if (e.RowElement.MasterTemplate.Owner == rgvReportList)
            {
                if (rgvReportList.DataSource == null)
                {
                    e.RowElement.DrawFill = false;
                    rgvReportList.BackgroundImage = Resources.evizite;
                    rgvReportList.TableElement.BackColor = Color.Transparent;
                    rgvReportList.BackgroundImageLayout = ImageLayout.Center;
                    rgvReportList.ShowNoDataText = false;
                }
                else
                {
                    //e.RowElement.DrawFill = true;
                    rgvReportList.BackgroundImage = null;
                }
            }
            else if (e.RowElement.MasterTemplate.Owner == rgvSgkOtomasyon)
            {
                rgvSgkOtomasyon.BackgroundImage = Resources.igic;
                if (rgvSgkOtomasyon.DataSource == null)
                {
                    e.RowElement.DrawFill = true;
                    rgvSgkOtomasyon.TableElement.BackColor = Color.Transparent;
                    rgvSgkOtomasyon.BackgroundImageLayout = ImageLayout.Center;
                    rgvSgkOtomasyon.ShowNoDataText = false;
                    switch (modulType)
                    {
                        case ModulTypeEnum.Hesapdurumu:
                            rgvSgkOtomasyon.BackgroundImage = Resources.hesapdurumu;
                            break;
                        case ModulTypeEnum.Hizmet:
                            rgvSgkOtomasyon.BackgroundImage = Resources.hizmetlistesipersonel;
                            break;
                        case ModulTypeEnum.Igl:
                            rgvSgkOtomasyon.BackgroundImage = Resources.igic;
                            break;
                    }
                }
                else
                {
                    //e.RowElement.DrawFill = true;
                    rgvSgkOtomasyon.BackgroundImage = null;
                }
            }
            if (e.RowElement.MasterTemplate.Owner == rgvLeaves)
            {
                GridViewHierarchyRowInfo hierarchyRow = e.RowElement.RowInfo.Parent as GridViewHierarchyRowInfo;
                if (hierarchyRow != null)
                {
                    for (int i = 0; i < hierarchyRow.Cells.Count; i++)
                    {
                        e.RowElement.BackColor = i % 2 == 0 ? Color.AliceBlue : Color.PeachPuff;
                        e.RowElement.BorderBoxStyle = BorderBoxStyle.FourBorders;
                        e.RowElement.BorderWidth = 1;
                        e.RowElement.BorderColor = Color.Black;
                        e.RowElement.DrawBorder = true;
                    }
                }
            }
        }
        private void GridViews_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {

            if (e.CellElement.MasterTemplate.Owner == rgvCompanyList)
            {

            }
            else if (e.CellElement.MasterTemplate.Owner == rgvLastName)
            {

            }
            else if (e.CellElement.MasterTemplate.Owner == rgvLinkList)
            {
                GridCheckBoxCellElement checkBoxCell = e.CellElement as GridCheckBoxCellElement;
                GridFilterCheckBoxCellElement filterCheckBoxCell = e.CellElement as GridFilterCheckBoxCellElement;
                if (checkBoxCell != null || filterCheckBoxCell != null)
                {
                    RadCheckBoxEditor editor = ((GridDataCellElement)e.CellElement).Editor as RadCheckBoxEditor;
                    RadCheckBoxEditorElement el = editor.EditorElement as RadCheckBoxEditorElement;
                    el.Checkmark.Shape = shape;
                    el.Checkmark.CheckElement.CheckPrimitiveStyle = Telerik.WinControls.Enumerations.CheckPrimitiveStyleEnum.None;
                    el.Checkmark.Fill.GradientStyle = GradientStyles.Solid;
                    el.Checkmark.MinSize = new Size(20, 20);
                    if (el.CheckState == Telerik.WinControls.Enumerations.ToggleState.On)
                    {
                        el.Checkmark.Fill.BackColor = Color.Gold;
                        el.Checkmark.CheckElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                        el.Checkmark.CheckElement.ResetValue(LightVisualElement.NumberOfColorsProperty, ValueResetFlags.Local);
                        el.Checkmark.CheckElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
                        el.Checkmark.CheckElement.ResetValue(LightVisualElement.ForeColorProperty, ValueResetFlags.Local);
                    }
                    else if (el.CheckState == Telerik.WinControls.Enumerations.ToggleState.Off)
                    {
                        el.Checkmark.Fill.BackColor = Color.AntiqueWhite;
                        el.Checkmark.CheckElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                        el.Checkmark.CheckElement.ResetValue(LightVisualElement.NumberOfColorsProperty, ValueResetFlags.Local);
                        el.Checkmark.CheckElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
                        el.Checkmark.CheckElement.ResetValue(LightVisualElement.ForeColorProperty, ValueResetFlags.Local);
                    }
                    else
                    {
                        el.Checkmark.Fill.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                        el.Checkmark.Fill.BackColor = Color.Black;
                        el.Checkmark.Fill.BackColor2 = Color.Gold;
                        el.Checkmark.Fill.NumberOfColors = 2;
                        el.Checkmark.Fill.GradientStyle = GradientStyles.Gel;
                    }
                }
            }

            if (e.CellElement is GridFilterCellElement)
            {
                e.CellElement.DrawBorder = true;
                e.CellElement.BorderBoxStyle = BorderBoxStyle.FourBorders;
                e.CellElement.BorderWidth = 2;
                e.CellElement.BorderColor = Color.Red;
                Font font = new Font(e.CellElement.Font.Name, 9, FontStyle.Bold);
                e.CellElement.Font = font;
                e.CellElement.TextAlignment = ContentAlignment.MiddleCenter;

            }
            if (e.CellElement is GridSummaryCellElement)
            {
                e.CellElement.DrawBorder = true;
                e.CellElement.BorderBoxStyle = BorderBoxStyle.FourBorders;
                e.CellElement.BorderLeftWidth = 0;
                e.CellElement.BorderRightWidth = 0;
                e.CellElement.BorderBottomWidth = 1;
                e.CellElement.BorderTopWidth = 1;
                e.CellElement.BorderTopColor = Color.Black;
                Font font = new Font(e.CellElement.Font.Name, 9, FontStyle.Bold);

                e.CellElement.Font = font;
                e.CellElement.TextAlignment = ContentAlignment.MiddleCenter;
                e.CellElement.TextAlignment = e.Column.FieldName == "Ads" || e.Column.FieldName == "Cn" || e.Column.FieldName == "Yil"  ? ContentAlignment.MiddleLeft : ContentAlignment.MiddleRight;
                e.CellElement.Padding = e.Column.FieldName == "Ads" || e.Column.FieldName == "Cn" || e.Column.FieldName == "Yil" ? new Padding(20, 0, 0, 0) : new Padding(0, 0, 20, 0);
                
            }
            if (e.CellElement.RowInfo is GridViewGroupRowInfo)
            {
                e.CellElement.DrawFill = true;
                e.CellElement.BackColor = Color.Aquamarine;
                Font font = new Font(e.CellElement.Font.Name, 9, FontStyle.Bold);
                e.CellElement.Font = font;
                e.CellElement.TextAlignment = ContentAlignment.MiddleLeft;
                e.CellElement.GradientStyle = Telerik.WinControls.GradientStyles.Solid;
            }
        }
        private void GridViews_CreateRowInfo(object sender, GridViewCreateRowInfoEventArgs e)
        {
            if (e.RowInfo is GridViewSearchRowInfo) e.RowInfo = new CustomSearchRow(e.ViewInfo);
        }
        private void Gridviews_PrintCellFormatting(object sender, PrintCellFormattingEventArgs e)
        {
            if (e.Column is GridViewCheckBoxColumn && e.Row.Cells[e.Column.Index].Value is bool)
            {
                e.PrintCell.Text = ((bool)e.Row.Cells[e.Column.Index].Value) ? "EVET" : "HAYIR";
            }
        }
        private void FillCompanyGridView()
        {
            string msg = "";
            bool sgscOk = IOC.CompanyDataService.FillCompanyCheck(out msg);
            bool yuklendi = IOC.WinHelpers.SetCompanyListRgv(rgvCompanyList, out msg);
            if (!yuklendi)
            {
                string mesaj = $"Veri tabanından firma listesi yüklenmesi başarısız oldu ({msg})  \r\nTekrar denemek için \"Tekrar Dene\" butonuna basın";
                DialogResult dr = RadMessageBox.Show(mesaj, "Firma Listesi Yüklenemedi!",MessageBoxButtons.RetryCancel);
                if (dr == DialogResult.Retry)
                {
                    FillCompanyGridView();
                }
                else {
                    Application.Exit();
                }
            }
        }
        private void lblMessage_TextChanged(object sender, EventArgs e)
        {
            if (lblMessage.Text == String.Empty)
            {
                lblMessage.Visibility = ElementVisibility.Collapsed; btnHideMessage.Visibility = ElementVisibility.Collapsed;
            }
            else
            {
                lblMessage.Visibility = ElementVisibility.Visible; btnHideMessage.Visibility = ElementVisibility.Visible;
            }
            if (lblMessage.Text.Contains("ChromeDriver only supports"))
            {
                lblMessage.Text = "Chrome sürümü güncel değil. Lütfen Chrome'u güncelleyip tekrar deneyin."; return;
            }
        }
        private void ribbonBar_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            FAbout f = new FAbout();
            f.ShowDialog();
        }
        private void fBaslangic_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Process.Start($@"{Application.StartupPath}\CleanBrowser.bat");
            if (Settings.Default.closeAllBrowsers)
            {
            string strCmdLine =
            "/C @ECHO ON" +
            "ECHO Google Chrome, kapatılıyor..." +
            "taskkill / IM \"chrome.exe\" / F" +
            "ECHO chromedriver.exe, kapatılıyor..." +
            "taskkill / IM \"chromedriver.exe\" / F" +
            "ECHO Firefox, kapatılıyor..." +
            "taskkill / IM \"firefox.exe\" / F" +
            "ECHO geckodriver.exe, kapatılıyor..." +
            "taskkill / IM \"geckodriver.exe\" / F";
            
                System.Diagnostics.Process.Start("CMD.exe", strCmdLine);
            }
            
        }
              
        #region DateTimePickers
        private void dtp_ValueChanged(object sender, EventArgs e)
        {
            if (sender == dtpSearchDate)
            {
                SearchReport.SearchDate = (DateTime)dtpSearchDate.Value;
            }
            else if (sender == dtpViziteFirst)
            {
                SearchReport.StartDate = (DateTime)dtpViziteFirst.Value;
                dtpViziteEnd.MinDate = SearchReport.StartDate;
            }
            else if (sender == dtpViziteEnd)
            {
                SearchReport.EndDate = (DateTime)dtpViziteFirst.Value;
                if (dtpViziteEnd.Value < dtpViziteFirst.Value)
                {
                    dtpViziteEnd.Value = DateTime.Today;
                }
            }
            else if (sender == dtpIglFirst)
            {
                CheckUpVars.StartDate = (DateTime)dtpIglFirst.Value;
                dtpIglLast.MinDate = CheckUpVars.StartDate;
            }
            else if (sender == dtpIglLast)
            {
                CheckUpVars.EndDate = (DateTime)dtpIglFirst.Value;
                if (dtpIglLast.Value < dtpIglFirst.Value)
                {
                    dtpIglLast.Value = DateTime.Today;
                }
            }

        }
        private void DdlHlSelectedValueChanged(object sender, Telerik.WinControls.UI.Data.ValueChangedEventArgs e)
        {
            if (!hlFirstOpen)
            {
                int yilS = ddlHlStartYear.SelectedItem != null? ((KeyValuePair<string, int>)ddlHlStartYear.SelectedItem.DataBoundItem).Value : DateTime.Now.Year ;
                int ayS = ddlHlStartMount.SelectedItem != null ? ((KeyValuePair<string, int>)ddlHlStartMount.SelectedItem.DataBoundItem).Value : DateTime.Now.Month;
                CheckUpVars.StartDate = new DateTime(yilS, ayS, 1);

                int yilE = ddlHlEndYear.SelectedItem != null? ((KeyValuePair<string, int>)ddlHlEndYear.SelectedItem.DataBoundItem).Value: DateTime.Now.Year;
                int ayE = ddlHlEndMount.SelectedItem != null? ((KeyValuePair<string, int>)ddlHlEndMount.SelectedItem.DataBoundItem).Value: DateTime.Now.Month;
                int gun = DateTime.DaysInMonth(yilE, ayE);
                CheckUpVars.EndDate = new DateTime(yilE, ayE, gun);

                if (sender == ddlHlStartYear)
                {
                    foreach (RadListDataItem item in ddlHlEndYear.Items)
                    {
                        int yilEd = ((KeyValuePair<string, int>)item.DataBoundItem).Value;
                        if (yilEd < yilS) { item.Enabled = false; }
                        else if (yilEd > yilS) { item.Enabled = true; }
                        else { item.Enabled = true; item.Selected = true; }
                    }
                } 
                else if (sender == ddlHlStartMount)
                {    
                    foreach (RadListDataItem item in ddlHlEndMount.Items)
                    {
                        int ayEd = ((KeyValuePair<string, int>)item.DataBoundItem).Value;
                        if (yilS == yilE)
                        {
                            if (ayEd < ayS) { item.Enabled = false; }
                            else if (ayEd > ayS) { item.Enabled = true; }
                            else { item.Enabled = true; item.Selected = true; }
                        }
                        else
                        {
                            item.Enabled = true;
                        }
                    }
                }
                else if (sender == ddlHlEndYear)
                {
                    foreach (RadListDataItem item in ddlHlEndMount.Items)
                    {
                        int ayEd = ((KeyValuePair<string, int>)item.DataBoundItem).Value;
                        if (yilS == yilE)
                        {
                            if (ayEd < ayS) { item.Enabled = false; }
                            else if (ayEd > ayS) { item.Enabled = true; }
                            else { item.Enabled = true; item.Selected = true; }
                        }
                        else
                        {
                            item.Enabled = true;
                        }
                    }
                }
                else if (sender == ddlHlEndMount)
                {

                }
            }
        }
        #endregion
        #endregion

        #region vizite

        #region bgw
        private void ProcessStarted()
        {
            lblReport.Text = string.Empty; 
            btnConfirmResult.Visibility = ElementVisibility.Collapsed;
            IOC.SgkLinksService.LoginBtnClicked = false;
            pnlWaitVizite.Visible = true;
            rgvCompanyList.Enabled = false;
            rwbVizite.StartWaiting();
            btnCancelVizite.Enabled = true;
            lblReport.Text = string.Empty;
            ribbonBar.Enabled = false;
            btnOpenFile.Enabled = false;
            btnHideMessage.Enabled = false;
            this.ribbonBar.ShowLayoutModeButton = false;
        }
        private void ProcessFinished()
        {
            pnlWaitVizite.Visible = false;
            rgvCompanyList.Enabled = true;
            rwbVizite.StopWaiting();
            btnCancelVizite.Enabled = false;
            ribbonBar.Enabled = true;
            btnOpenFile.Enabled = true;
            btnHideMessage.Enabled = true;
            this.ribbonBar.ShowLayoutModeButton = false;
            GlobalVars.CancelProcess = false;
            LinkGlobals.LinkCancel = false;
            if (Settings.Default.disposeDriver && Surucu.Driver != null) { Surucu.Driver.Dispose(); Surucu.Driver = null; WinHelpers.ClearAll(); }
            IOC.LinkOps.DisposeProcess();
        }
        private void FillRgvReports(List<Visit> visits)
        {
            if (rgvReportList.InvokeRequired)
            {
                var d = new SafeCallDelegate(FillRgvReports);
                rgvReportList.Invoke(d, new object[] { visits });
            }
            else
            {
                rgvReportList.DataSource = visits;
            }
        }
        private void FreshRgvReports()
        {
            if (rgvReportList.InvokeRequired)
            {
                var d = new SafeFreshDelegate(FreshRgvReports);
                rgvReportList.Invoke(d, null);
            }
            else
            {
                rgvReportList.DataSource = null;
                rgvReportList.Rows.Clear();
                rgvReportList.Columns.Clear();
                rgvReportList.Refresh();
            }
        }
        private void InitializeBgwSearch()
        {
            bgwSearch.WorkerSupportsCancellation = true;
            bgwSearch.DoWork += new DoWorkEventHandler(BgwSearchDoWork);
            bgwSearch.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BgwSearchComplated);
            bgwSearch.ProgressChanged += new ProgressChangedEventHandler(BgwSearchChanged);
        }
        private void InitializeBgwConfirm()
        {
            bgwConfirm.WorkerSupportsCancellation = true;
            bgwConfirm.DoWork += new DoWorkEventHandler(BgwConfirmDoWork);
            bgwConfirm.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BgwConfirmComplated);
            bgwConfirm.ProgressChanged += new ProgressChangedEventHandler(BgwConfirmChanged);
        }
        private void InitializeBgwDetails()
        {
            bgwDetails.WorkerSupportsCancellation = true;
            bgwDetails.DoWork += new DoWorkEventHandler(BgwDetailsDoWork);
            bgwDetails.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BgwDetailsComplated);
            bgwDetails.ProgressChanged += new ProgressChangedEventHandler(BgwDetailsChanged);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            GlobalVars.CancelProcess = true;
            lblReport.Text += "<html><strong><span style=\"font-size: 10pt\">İPTAL TALEBİ ALINDI!</span></strong></html>\r\n";
            btnCancelVizite.Text = "İşlem iptal ediliyor...";
            btnCancelVizite.Enabled = false;
            if (bgwConfirm.IsBusy)
            {
                bgwConfirm.CancelAsync();
            }
            else if (bgwSearch.IsBusy)
            {
                bgwSearch.CancelAsync();
            }
            else if (bgwDetails.IsBusy)
            {
                bgwDetails.CancelAsync();
            }

        }
        private void BgwSearchDoWork(object sender, DoWorkEventArgs e)
        {
            lblReport.Text = string.Empty; btnConfirmResult.Visibility = ElementVisibility.Collapsed;
            IOC.TrmBase.tryCount = 1;
            GlobalVars.CancelProcess = false;
            IOC.VisitReportService.Message = String.Empty;
            IOC.VisitReportService.HasPregnancyCheckFinish = false;
            SearchReport.CaseType = (ddlVaka.SelectedIndex == 0) ? 1 : (ddlVaka.SelectedIndex == 1) ? 2 : (ddlVaka.SelectedIndex == 2) ? 3 : 1;
            SearchReport.IsMultiSearch = (ddlVaka.SelectedIndex == 3) ? true : false;
            processType = ProcessTypeEnum.Raportarama; SearchReport.ProcessType = 1;
            bool allowStart = true;
            viziteRapor = "";
            GlobalVars.ProcessReport = $"<html><ul>";
            lblReport.Text = GlobalVars.ProcessReport;
            if (rgvCompanyList.SelectedRows.Count > 1 && reportType == ReportTypeEnum.Tcno)
            {
                var confirmResult = RadMessageBox.Show("Kimlik numarasına göre arama için birden fazla firma seçildi, devam edilsin mi?", "Birden fazla firma seçildi", MessageBoxButtons.YesNo, RadMessageIcon.Question, "Taranacak kimlik numarasına sahip personelin birden fazla firmada çalıştığına eminseniz devam ediniz. Aksi halde tarama işleminin süresi gereksiz yere uzayacaktır");
                if (confirmResult == DialogResult.No)
                {
                    rgvCompanyList.ClearSelection(); allowStart = false;
                }
            }
            string msg;
            if (reportType == ReportTypeEnum.Tcno && allowStart == true)
            {
                isTcnoValid = false;
                SearchReport.KimlikNo = texTckn.Text.Trim();
                isTcnoValid = IOC.WinHelpers.IsTcnoValid(SearchReport.KimlikNo, out msg);
                if (!isTcnoValid) { lblMessage.Text = msg; texTckn.Focus(); return; }
            }
            if (bgwSearch.CancellationPending == true) { e.Cancel = true; return; }
            if (allowStart)
            {
                switch (reportType)
                {
                    case ReportTypeEnum.Tcno:
                        lblViziteReportHeader.Text = $"<html><span style=\"font-size: 12pt\"><strong>KİMLİK NUMARASINA GÖRE RAPOR TARAMA</strong></span></html>";
                        IOC.LinkOps.CmdUrlVurl = LinkGlobals.LstLinks.Where(x => x.Id == 54).Select(x => new CmdUrlVurl { Cmd = x.Cmd, Url = x.Url, Vurl = x.Vurl }).FirstOrDefault();
                        break;
                    case ReportTypeEnum.RaporTarihi:
                        lblViziteReportHeader.Text = $"<html><span style=\"font-size: 12pt\"><strong>TARİHE GÖRE RAPOR TARAMA</strong></span></html>";
                        IOC.LinkOps.CmdUrlVurl = LinkGlobals.LstLinks.Where(x => x.Id == 55).Select(x => new CmdUrlVurl { Cmd = x.Cmd, Url = x.Url, Vurl = x.Vurl }).FirstOrDefault();
                        break;
                    case ReportTypeEnum.OnayliRapor:
                        lblViziteReportHeader.Text = $"<html><span style=\"font-size: 12pt\"><strong>ONAYLI RAPORLARI TARAMA</strong></span></html>";
                        IOC.LinkOps.CmdUrlVurl = LinkGlobals.LstLinks.Where(x => x.Id == 56).Select(x => new CmdUrlVurl { Cmd = x.Cmd, Url = x.Url, Vurl = x.Vurl }).FirstOrDefault();
                        break;
                    case ReportTypeEnum.ArsivRaporu:
                        lblViziteReportHeader.Text = $"<html><span style=\"font-size: 12pt\"><strong>ARŞİVDEKİ RAPORLARI TARAMA</strong></span></html>";
                        IOC.LinkOps.CmdUrlVurl = LinkGlobals.LstLinks.Where(x => x.Id == 57).Select(x => new CmdUrlVurl { Cmd = x.Cmd, Url = x.Url, Vurl = x.Vurl }).FirstOrDefault();
                        break;
                    default:
                        break;
                }

                viziteRapor += lblViziteReportHeader.Text;
                btnOpenFile.Visibility = ElementVisibility.Collapsed;
                lblMessage.Text = string.Empty;
               
                if (visitsReceived != null && visitsReceived.Count > 0) visitsReceived.Clear();
                if (visitsToBeProcessed != null && visitsToBeProcessed.Count > 0) visitsToBeProcessed.Clear();
                if (IOC.VisitReportService.Visits.Count > 0) IOC.VisitReportService.Visits.Clear();
                if (IOC.PersonelimDegil.MyEmployees != null && IOC.PersonelimDegil.MyEmployees.Count > 0) IOC.PersonelimDegil.MyEmployees.Clear();
                if (IOC.PersonelimDegil.NotMyEmployees != null && IOC.PersonelimDegil.NotMyEmployees.Count > 0) IOC.PersonelimDegil.NotMyEmployees.Clear();
                FreshRgvReports();
                try
                {
                    List<Company> lst = IOC.WinHelpers.GetSelectedCompaniesFromRgv(rgvCompanyList, out msg);
                    foreach (Company login in lst)
                    {
                        if (bgwSearch.CancellationPending == true) { e.Cancel = true; return; }
                        if(!ViziteCek(login, out msg)){
                            if (bgwSearch.CancellationPending == true) { e.Cancel = true; return; }
                            else {
                                viziteRapor += GlobalVars.ProcessReport; GlobalVars.ProcessReport = "<html>";
                                lblReport.Text = GlobalVars.ProcessReport;
                                continue; 
                            }
                        }
                        IOC.LinkOps.LogOut("t", "Çıkış Yap", "E-VİZİTE", out msg);
                    }
                    if (visitsReceived != null && visitsReceived.Count > 0)
                    {
                        rgvReportList.Columns.Add(WinHelpers.AddCheckBoxColumnToRgv());

                        // Vaka türü analık olanlar için rapor başlama tarihini rapor ayrıntılarından bulma
                        if (SearchReport.ReportType != 3)
                        {
                            List<string> listForPc = new List<string>();
                            // soldaki listeden seçilen bütün firmalar için değil sadece ANALIK vaka durumu olan personellerin firmalarını al.
                            foreach (Visit v in visitsReceived)
                            {
                                if (v.Vaka == "ANALIK" && !listForPc.Contains(v.Cnm))
                                { // firma bir kez eklenmişse ikinci kez ekleme
                                    listForPc.Add(v.Cnm);
                                }
                            }
                            string[] compsForPregnancyCheck = listForPc.ToArray(); // listeyi diziye dönüştür.
                            if (compsForPregnancyCheck.Count() > 0)
                            {
                                processType = ProcessTypeEnum.Pregnancycheck;
                                foreach (Company login in lst)
                                {
                                    if (compsForPregnancyCheck.Contains(login.CompanyName))
                                    {
                                        if (bgwSearch.CancellationPending == true) { e.Cancel = true; return; }
                                        if (!ViziteCek(login, out msg))
                                        {
                                            if (bgwSearch.CancellationPending == true) { e.Cancel = true; return; }
                                            else {
                                                viziteRapor += GlobalVars.ProcessReport; GlobalVars.ProcessReport = "<html>";
                                                lblReport.Text = GlobalVars.ProcessReport;
                                                continue;
                                            }
                                        }
                                        IOC.LinkOps.LogOut("t", "Çıkış Yap", "E-VİZİTE", out msg);
                                    }
                                }
                            }
                        }
                        if (personelEnum == PersonelEnum.Check) //  kontrol açık ise
                        {
                            GlobalVars.ProcessReport += $"<li><span style=\"font-size: 10pt\"><strong>PERSONELİM DEĞİL kontrolü için SGK İşveren Sistemi açılıyor</strong></span></li>";
                            lblReport.Text = GlobalVars.ProcessReport;
                            List<string> listForPd = new List<string>();

                            foreach (Visit v in visitsReceived)
                            {
                                listForPd.Add(v.Cnm);
                            }
                            string[] compsForPersonelimDegil = listForPd.ToArray();
                            processType = ProcessTypeEnum.Personelimdegiltarama; SearchReport.ProcessType = 2;
                            foreach (Company login in lst)
                            {
                                if (compsForPersonelimDegil.Contains(login.CompanyName))
                                {
                                    if (!PersonelCheck(login, out msg))
                                    {
                                        if (bgwSearch.CancellationPending == true) { e.Cancel = true; return; }
                                        else
                                        {
                                            viziteRapor += GlobalVars.ProcessReport; GlobalVars.ProcessReport = "<html>";
                                            lblReport.Text = GlobalVars.ProcessReport;
                                            continue;
                                        }
                                    }
                                    IOC.LinkOps.LogOut("t", "Çıkış", "İŞE GİRİŞ-AYRILIŞ", out msg);
                                }
                            }
                            if (IOC.PersonelimDegil.MyEmployees.Count > 0 || IOC.PersonelimDegil.NotMyEmployees.Count > 0)
                            {
                                visitsReceived = IOC.PersonelimDegil.NotMyEmployees.Concat(IOC.PersonelimDegil.MyEmployees).ToList();
                            }
                            if(IOC.PersonelimDegil.NotMyEmployees.Count > 0)
                            {
                                foreach (Visit personel in IOC.PersonelimDegil.NotMyEmployees)
                                {
                                    GlobalVars.ProcessReport += $"<li><span style=\"font-size: 10pt\"><strong>{personel.AdSoyad} raporlu olduğu {personel.RaporBaslamaTarihi:dd.MM.yyyy} - {personel.RaporBitisTarihi:dd.MM.yyyy} tarih aralığında firma personeli değil olarak saptandı</strong></span></li>";
                                    lblReport.Text = GlobalVars.ProcessReport;
                                }
                            }
                        }

                    } // visitsReceived > 0 sonu
                    if (SearchReport.ReportType == 2 && visitsReceived.Count >= 1) rgvTitleText = $"{SearchReport.SearchDate:dd.MM.yyyy} itibariyle onaylanmamış raporlar";
                    else if (SearchReport.ReportType == 3 && visitsReceived.Count >= 1) rgvTitleText = $"{SearchReport.StartDate.ToString("dd.MM.yyyy")} - {SearchReport.EndDate.ToString("dd.MM.yyyy")} tarihleri arasındaki onaylanmış raporlar";
                    else if (SearchReport.ReportType == 4 && visitsReceived.Count >= 1) rgvTitleText = $"Arşivinizdeki {SearchReport.StartDate.ToString("dd.MM.yyyy")} - {SearchReport.EndDate.ToString("dd.MM.yyyy")} tarihleri arasındaki raporlar";
                    rgvReportList.TitleText = rgvTitleText;
                    if (bgwSearch.CancellationPending == true) { e.Cancel = true; return; }

                    #region html
                    viziteRapor += lblReport.Text.Replace(@"</html><html>", " ");
                    viziteRapor = viziteRapor.Replace(@"</html><html>", " ").Replace("12pt","18pt").Replace("10pt","14pt");
                    IOC.ExportService.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"{DateTime.Now:dd-MM-yyyy}-{viziteRapor.Substring(44, viziteRapor.IndexOf("</strong>") - 44)}-{DateTime.Now.Ticks}.html").Replace("  ", " ");
                    using (StreamWriter writer = new StreamWriter(IOC.ExportService.FileName))
                    {
                        StringBuilder stringBuilder = new StringBuilder(); stringBuilder.Append(viziteRapor);
                        writer.WriteLine(stringBuilder);
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    msg = ex.Message.ToString();
                    lblMessage.Text = $"Rapor tarama başlatılamıyor! Hata: {msg}";
                }
            }
        }
        private void BgwSearchComplated(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                lblMessage.Text = $"Rapor tarama sırasında hata oluştu! {e.Error.Message}";
                FreshRgvReports();
            }
            else if (e.Cancelled)
            {
                lblMessage.Text = $"Rapor tarama iptal edildi!";
                FreshRgvReports();
            }
            else
            {
                if (visitsReceived != null && visitsReceived.Count > 0)
                {
                    FillRgvReports(visitsReceived);
                    if (reportType != ReportTypeEnum.OnayliRapor)
                    {
                        listType = ListTypeEnum.Onaylanacak;
                        btnStartProcess.Text = "Onaylamayı Başlat";
                        rgvReportList.Columns.Add(Helpers.WinHelpers.AddDateColumnToRgv());
                        rgvReportList.Columns.Add(Helpers.WinHelpers.AddComboBoxColumnToRgv());
                        rgvReportList.Columns[21].DataType = typeof(DateTime);
                        rgvReportList.Columns[21].FormatString = "{0: dd.MM.yyyy}";
                        DatePickerSet();

                        foreach (var row in rgvReportList.Rows)
                        {
                            int notMyEmployeeCount = IOC.PersonelimDegil.NotMyEmployees.Count;
                            if (row.Index < notMyEmployeeCount)
                            {
                                row.Cells["CalismaDurumu"].Value = "2";
                            }
                            else if (row.Index >= notMyEmployeeCount)
                            {
                                row.Cells["CalismaDurumu"].Value = "0";
                            }
                        }
                    }
                    else if (reportType == ReportTypeEnum.OnayliRapor)
                    {
                        listType = ListTypeEnum.Onayli;
                        btnStartProcess.Text = "Onayları İptal Et";
                        checkDownloadPdf.Enabled = false;
                    }
                    switch (reportType)
                    {
                        case ReportTypeEnum.Tcno:
                            SearchReport.ReportTypeAfterRgvLoad = 1;
                            break;
                        case ReportTypeEnum.RaporTarihi:
                            SearchReport.ReportTypeAfterRgvLoad = 2;
                            break;
                        case ReportTypeEnum.OnayliRapor:
                            SearchReport.ReportTypeAfterRgvLoad = 3;
                            break;
                        case ReportTypeEnum.ArsivRaporu:
                            SearchReport.ReportTypeAfterRgvLoad = 4;
                            break;
                        default:
                            break;
                    }
                    switch (reportType)
                    {
                        case ReportTypeEnum.Tcno:
                            exportTypeEnum = ExportTypeEnum.Onstc;
                            break;
                        case ReportTypeEnum.RaporTarihi:
                            exportTypeEnum = ExportTypeEnum.Onstr;
                            break;
                        case ReportTypeEnum.OnayliRapor:
                            exportTypeEnum = ExportTypeEnum.Ony;
                            break;
                        case ReportTypeEnum.ArsivRaporu:
                            exportTypeEnum = ExportTypeEnum.Arv;
                            break;
                    }
                    SetColumns();
                    rgvReportList.Refresh();
                    rgvReportList.BestFitColumns();
                }
                btnOpenFile.Visibility = ElementVisibility.Visible;
                btnOpenFile.Text = "Dosyayı Aç";
                lblMessage.Text = "Sorgulama tamamlandı, ayrıntılar için Dosyayı Aç butonuna basınız.";
            }
            bgwSearch.Dispose();
            ProcessFinished();
             
        }
        private void BgwSearchChanged(object sender, ProgressChangedEventArgs e)
        {

        }
        private void BgwConfirmDoWork(object sender, DoWorkEventArgs e)
        {
            IOC.TrmBase.tryCount = 1;
            GlobalVars.CancelProcess = false;
            GlobalVars.ProcessReport = $"<html><ul>";
            lblReport.Text = GlobalVars.ProcessReport;
            lblReport.Text = string.Empty;
            
            viziteRapor = lblViziteReportHeader.Text;
            bool calismistir = false;
            bool pd = false;
            string msg;
            Surucu.Driver = Surucu.Driver == null ? IOC.WinHelpers.GetWebDriver(Settings.Default.hideBrowser, out msg) : Surucu.Driver;
            allowStartConfirm = true;

            switch (reportType)
            {
                case ReportTypeEnum.Tcno:
                    IOC.LinkOps.CmdUrlVurl = LinkGlobals.LstLinks.Where(x => x.Id == 54).Select(x => new CmdUrlVurl { Cmd = x.Cmd, Url = x.Url, Vurl = x.Vurl }).FirstOrDefault();
                    break;
                case ReportTypeEnum.RaporTarihi:
                    IOC.LinkOps.CmdUrlVurl = LinkGlobals.LstLinks.Where(x => x.Id == 55).Select(x => new CmdUrlVurl { Cmd = x.Cmd, Url = x.Url, Vurl = x.Vurl }).FirstOrDefault();
                    break;
                case ReportTypeEnum.OnayliRapor:
                    IOC.LinkOps.CmdUrlVurl = LinkGlobals.LstLinks.Where(x => x.Id == 56).Select(x => new CmdUrlVurl { Cmd = x.Cmd, Url = x.Url, Vurl = x.Vurl }).FirstOrDefault();
                    break;
                case ReportTypeEnum.ArsivRaporu:
                    IOC.LinkOps.CmdUrlVurl = LinkGlobals.LstLinks.Where(x => x.Id == 57).Select(x => new CmdUrlVurl { Cmd = x.Cmd, Url = x.Url, Vurl = x.Vurl }).FirstOrDefault();
                    break;
                default:
                    break;
            }

            if (listType == ListTypeEnum.Onaylanacak)
            {
                lblViziteReportHeader.Text = $"<html><span style=\"font-size: 12pt\"><strong>RAPOR ONAYLAMA</strong></span></html>";
                processType = ProcessTypeEnum.Raporonaylama; SearchReport.ProcessType = 3;
                foreach (GridViewRowInfo row in rgvReportList.Rows)
                {
                    if (row.Cells[22].Value.ToString() == "1" && row.Cells[0].Value != null && (Boolean)row.Cells[0].Value == true)
                    {
                        if (bgwConfirm.CancellationPending == true) { e.Cancel = true; return; }
                        calismistir = true; break;
                    }
                }
                foreach (GridViewRowInfo row in rgvReportList.Rows)
                {
                    if (row.Cells[22].Value.ToString() == "2" && row.Cells[0].Value != null && (Boolean)row.Cells[0].Value == true && reportType == ReportTypeEnum.ArsivRaporu)
                    {
                        if (bgwConfirm.CancellationPending == true) { e.Cancel = true; return; }
                        pd = true; break;
                    }
                }
                if (calismistir == true)
                {
                    DialogResult confirmResult = RadMessageBox.Show("Eğer raporun ödemesi yapılmışa 'Çalışmıştır' olarak kaydedilemez, 'Çalışmamıştır' olarak kaydedilecektir. Devam edilsin mi?", "Çalışmıştır olarak işaretlenen personel var!", MessageBoxButtons.YesNo, RadMessageIcon.Question, "Raporun ödemesinin yapılıp yapılmadığından emin değilseniz 'Rapor Ayrıntıları Etkin' seçeneğini işaretledikten sonra satırın üzerine çift tıklayarak rapor ayrıntılarını görüntüleyebiliriniz.");
                    if (confirmResult == DialogResult.No)
                    {
                        allowStartConfirm = false;
                    }
                }
                else if (pd == true)
                {
                    DialogResult confirmResult = RadMessageBox.Show("SGK E-Vizite Sisteminde Arşiv Raporları için \"Personelim Değil\" seçeneği bulunmamaktadır. Eğer devam ederseniz bu personeller için bir işlem yapılamayacaktır. Devam Etmek İstiyor musunuz?", "Personelim Değil olarak işaretlenen personel var!", MessageBoxButtons.YesNo, RadMessageIcon.Question, "SGK E-Vizite Sisteminde Arşiv Raporları için \"Personelim Değil\" seçeneği bulunmamaktadır; listede bilgi amaçlı olarak gösterilmişlerdir. Bu personelleri çalışmamıştır olarak işaretleyebilir ya da seçimden çıkarıp daha sonra SGK E-Vizite sayfasından manuel bildirim yapabilirsiniz.");
                    if (confirmResult == DialogResult.No)
                    {
                        allowStartConfirm = false;
                    }
                    else
                    {
                        foreach (GridViewRowInfo row in rgvReportList.Rows)
                        {
                            if (bgwConfirm.CancellationPending == true) { e.Cancel = true; return; }
                            if (row.Cells[22].Value.ToString() == "2" && row.Cells[0].Value != null && (Boolean)row.Cells[0].Value == true)
                            {
                                row.Cells[0].Value = false;
                            }
                        }
                        List<VisitsToBeProcessed> silinecekler = new List<VisitsToBeProcessed>();
                        foreach (VisitsToBeProcessed vtbp in visitsToBeProcessed)
                        {
                            if (bgwConfirm.CancellationPending == true) { e.Cancel = true; return; }
                            if (vtbp.WorkingStatus == 2)
                            {
                                silinecekler.Add(vtbp);
                            }
                        }
                        foreach (VisitsToBeProcessed vtbr in silinecekler)
                        {
                            if (bgwConfirm.CancellationPending == true) { e.Cancel = true; return; }
                            visitsToBeProcessed.Remove(vtbr);
                        }
                    }
                }

            }
            else if (listType == ListTypeEnum.Onayli)
            {
                lblViziteReportHeader.Text = $"<html><span style=\"font-size: 12pt\"><strong>RAPOR ONAYI İPTAL ETME</strong></span></html>";
                processType = ProcessTypeEnum.Onayiptaletme; SearchReport.ProcessType = 4;
                DialogResult confirmResult = RadMessageBox.Show("Eğer raporun ödemesi yapılmışa iptal edilemez! Ödemesi yapılan raporlar için işlem yapılmayacaktır. Devam Etmek İstiyor musunuz?", "İptal işlemlerini onayla!", MessageBoxButtons.YesNo, RadMessageIcon.Question, "Raporun ödemesinin yapılıp yapılmadığından emin değilseniz  'Rapor Ayrıntıları Etkin' seçeneğini işaretledikten sonra satırın üzerine çift tıklayarak rapor ayrıntılarını görüntüleyebiliriniz.");
                if (confirmResult == DialogResult.No)
                {
                    allowStartConfirm = false;
                }
            }
            if (allowStartConfirm == true)
            {
                if (bgwConfirm.CancellationPending == true) { e.Cancel = true; return; }
                lblMessage.Text = String.Empty;
                List<string> list = new List<string>();
                try
                {
                    foreach (VisitsToBeProcessed v in visitsToBeProcessed)
                    {
                        list.Add(v.Cnm);
                    }
                    String[] firmaList = list.ToArray();
                    if (bgwConfirm.CancellationPending == true) { e.Cancel = true; return; }
                    List<Company> lst = (from x in GlobalVars.Companies where list.Contains(x.CompanyName) select x).ToList();

                    foreach (Company login in lst)
                    {
                        lblReport.Text = GlobalVars.ProcessReport;
                        if (bgwConfirm.CancellationPending == true) { e.Cancel = true; return; }
                        if (!ViziteCek(login, out msg))
                        {
                            if (bgwSearch.CancellationPending == true) { e.Cancel = true; return; }
                            else
                            {
                                viziteRapor += GlobalVars.ProcessReport; GlobalVars.ProcessReport = "<html>";
                                lblReport.Text = GlobalVars.ProcessReport;
                                continue;
                            }
                        }
                        IOC.LinkOps.LogOut("t", "Çıkış Yap", "E-VİZİTE", out msg);
                    }
                    if (bgwConfirm.CancellationPending == true) { e.Cancel = true; return; }
                }
                catch (Exception ex)
                {
                    msg = ex.Message.ToString();
                    lblMessage.Text = (reportType == ReportTypeEnum.OnayliRapor) ? $"Onay iptal işlemi başarısız! Hata: {msg}" : (reportType == ReportTypeEnum.Tcno || reportType == ReportTypeEnum.RaporTarihi || reportType == ReportTypeEnum.ArsivRaporu) ? $"Onaylama işlemi başarısız! Hata: {msg}" : String.Empty;
                }
            }
            else
            {
                e.Cancel = true; ProcessFinished(); return;
            }

            #region html
            viziteRapor += lblReport.Text.Replace(@"</html><html>", " ");
            viziteRapor = viziteRapor.Replace(@"</html><html>", " ").Replace("12pt", "18pt").Replace("10pt", "14pt");
            IOC.ExportService.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"{DateTime.Now:dd-MM-yyyy}-{viziteRapor.Substring(44, viziteRapor.IndexOf("</strong>") - 44)}-{DateTime.Now.Ticks}.html").Replace("  ", " ");
            using (StreamWriter writer = new StreamWriter(IOC.ExportService.FileName))
            {
                StringBuilder stringBuilder = new StringBuilder(); stringBuilder.Append(viziteRapor);
                writer.WriteLine(stringBuilder);
            }
            #endregion
        }
        private void BgwConfirmComplated(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                switch (processType)
                {
                    case ProcessTypeEnum.Raporonaylama:
                        lblMessage.Text = $"Rapor onaylama sırasında hata oluştu! {e.Error.Message}";
                        break;
                    case ProcessTypeEnum.Onayiptaletme:
                        lblMessage.Text = $"Onay iptali sırasında hata oluştu! {e.Error.Message}";
                        break;
                }
            }
            else if (e.Cancelled)
            {
                switch (processType)
                {
                    case ProcessTypeEnum.Raporonaylama:
                        lblMessage.Text = $"Rapor onaylama iptal edildi!";
                        break;
                    case ProcessTypeEnum.Onayiptaletme:
                        lblMessage.Text = $"Onay iptal işlemi iptal edildi!";
                        break;
                }

            }
            else
            {
                btnConfirmResult.Enabled = true; btnConfirmResult.Visibility = ElementVisibility.Visible;
                switch (processType)
                {
                    case ProcessTypeEnum.Raporonaylama:
                        btnConfirmResult.Text = "Onay Sonucu";
                        if (SearchReport.ConfirmError == true)
                        {
                            lblMessage.Text = "Rapor onaylama işlemi hatalarla tamamlandı.";
                        }
                        else
                        {
                            lblMessage.Text = "Rapor onaylama işlemi hatasız tamamlandı.";
                        }

                        break;
                    case ProcessTypeEnum.Onayiptaletme:
                        btnConfirmResult.Text = "İptal Sonucu";
                        if (SearchReport.ConfirmError == true)
                        {
                            lblMessage.Text = "Onay iptal işlemi hatalarla tamamlandı.";
                        }
                        else
                        {
                            lblMessage.Text = "Onay iptal işlemi hatasız tamamlandı.";
                        }

                        break;
                }


            }
            bgwConfirm.Dispose();
            ProcessFinished();
            
        }
        private void BgwConfirmChanged(object sender, ProgressChangedEventArgs e)
        {

        }
        private void BgwDetailsDoWork(object sender, DoWorkEventArgs e)
        {
            IOC.TrmBase.tryCount = 1;
            GlobalVars.CancelProcess = false;
            GlobalVars.ProcessReport = $"<html><ul>";
            lblReport.Text = GlobalVars.ProcessReport;
            lblReport.Text = string.Empty; btnConfirmResult.Visibility = ElementVisibility.Collapsed;
            processType = ProcessTypeEnum.Raporayrintilari;
            string msg;
            try
            {
                Company lgn = IOC.WinHelpers.GetLoginFromReports(rgvReportList, out msg);
                if (!ViziteCek(lgn, out msg))
                {
                    if (bgwSearch.CancellationPending == true) { e.Cancel = true; return; }
                    else
                    {
                        lblMessage.Text = $"Hata: {msg}";
                        GlobalVars.ProcessReport += $"<li><span style=\"font-size: 10pt; color: red\"><strong>Hata: {msg}</strong></span></li></ul></html>";
                        lblReport.Text = GlobalVars.ProcessReport;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Hata: {ex.Message}";
            }
        }
        private void BgwDetailsComplated(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                lblMessage.Text = $"Rapor ayrıntılarını alma sırasında hata oluştu! {e.Error.Message}";
            }
            else if (e.Cancelled)
            {
                lblMessage.Text = $"Rapor ayrıntısı alma iptal edildi!";
            }
            else
            {
                if (sourceIgBs.Count != 0 || sourceSpvudKs.Count != 0 || sourceSraoDs.Count != 0)
                {
                    FReportDetails reportDetails = new Forms.Common.FReportDetails(sourceIgBs, sourceSpvudKs, sourceSraoDs);
                    FormLoader.ShowForm(reportDetails, true, true);
                }
                else if (enableGetDetail != true)
                {
                    lblMessage.Text = "\"Rapor ayrıntılarına etkin\" seçeneği işaretli değil";
                }
                else
                {
                    lblMessage.Text = "Rapor ayrıntılarına ulaşılamıyor; lütfen internet bağlantınızı kontrol edip tekrar deneyin";
                }
            }
            bgwDetails.Dispose();
            ProcessFinished();
           
        }
        private void BgwDetailsChanged(object sender, ProgressChangedEventArgs e)
        {

        }
        #endregion

        #region radiobuttons
        public void ReportTypeClick(object sender, EventArgs e)
        {
            if (sender == rbKNGA)
            {
                reportType = ReportTypeEnum.Tcno; SearchReport.ReportType = 1;
                checkPersonelimDegil.Enabled = true; btnStartProcess.Text = "Onaylamayı Başlat"; checkDownloadPdf.Enabled = true; lblTariheGore.Text = "TC Kimlik No";
            }
            if (sender == rbTGA)
            {
                reportType = ReportTypeEnum.RaporTarihi; SearchReport.ReportType = 2;
                checkPersonelimDegil.Enabled = true; btnStartProcess.Text = "Onaylamayı Başlat"; checkDownloadPdf.Enabled = true; lblTariheGore.Text = "Tarih Seçimi";
            }
            if (sender == rbOR)
            {
                reportType = ReportTypeEnum.OnayliRapor; SearchReport.ReportType = 3;
                checkPersonelimDegil.Enabled = false; checkPersonelimDegil.CheckState = CheckState.Unchecked; btnStartProcess.Text = "Onayları İptal Et"; checkDownloadPdf.Enabled = false;
            }
            if (sender == rbArchive)
            {
                reportType = ReportTypeEnum.ArsivRaporu; SearchReport.ReportType = 4;
                checkPersonelimDegil.Enabled = true; btnStartProcess.Text = "Onaylamayı Başlat"; checkDownloadPdf.Enabled = true;
            }
            ShowOptionParameters(reportType);
            if (rgvReportList.Rows.Count > 0)
            {
                if (visitsToBeProcessed.Count > 0) visitsToBeProcessed.Clear();
                foreach (var row in rgvReportList.Rows)
                {
                    row.Cells[0].Value = false;
                }
            }
            switch (listType)
            {
                case ListTypeEnum.Onayli:
                    btnStartProcess.Text = "Onayları İptal Et";
                    break;
                case ListTypeEnum.Onaylanacak:
                    btnStartProcess.Text = "Onaylamayı Başlat";
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region checkboxes
        private void checkPersonelimDegil_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (checkPersonelimDegil.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
            {
                personelEnum = PersonelEnum.Check;
            }
            else { personelEnum = PersonelEnum.Dontcheck; }
        }
        private void checkReportDetails_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (checkReportDetails.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
            {
                enableGetDetail = true;
            }
            else { enableGetDetail = false; }
        }
        private void checkDownloadPdf_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (checkDownloadPdf.CheckState == CheckState.Checked)
            {
                SearchReport.GetConfirmPdf = true;

                FolderBrowserDialog f = new FolderBrowserDialog();
                f.ShowNewFolderButton = true;
                f.Description = "Kayıt konumunu seçin";

                if (f.ShowDialog() == DialogResult.OK)
                {
                    SearchReport.DocumentsDir = f.SelectedPath;
                }
                else
                {
                    checkDownloadPdf.CheckState = CheckState.Unchecked;
                    SearchReport.GetConfirmPdf = false;
                }

            }
            else if (checkDownloadPdf.CheckState == CheckState.Unchecked)
            {
                SearchReport.GetConfirmPdf = false;
            }
        }
        #endregion

        #region dropdownlists
        private void ddlVaka_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            RadDropDownListElement cb = (RadDropDownListElement)sender;

            if (cb.SelectedIndex == 0) { CaseType = CaseTypeEnnum.Iskazasi; SearchReport.CaseType = 1; SearchReport.IsMultiSearch = false; }
            else if (cb.SelectedIndex == 1) { CaseType = CaseTypeEnnum.Hastalik; SearchReport.CaseType = 2; SearchReport.IsMultiSearch = false; }
            else if (cb.SelectedIndex == 2) { CaseType = CaseTypeEnnum.Analik; SearchReport.CaseType = 3; SearchReport.IsMultiSearch = false; }
            else if (cb.SelectedIndex == 3) { CaseType = CaseTypeEnnum.Hepsi; SearchReport.CaseType = 1; SearchReport.IsMultiSearch = true; }
        }
        #endregion

        #region buttons
        private void btnStartSearch_Click(object sender, EventArgs e)
        {
            lblMessage.Text = String.Empty;
            if(rgvCompanyList.DataSource == null || rgvCompanyList.SelectedRows.Count <= 0) { lblMessage.Text = "Tarama için en az bir firma seçmelisiniz"; return; }
            rwbVizite.Text = "RAPOR TARAMA İŞLEMİ DEVAM EDİYOR...";
            btnCancelVizite.Text = "Rapor taramayı iptal et";
            if (!bgwSearch.IsBusy)
            {
                bgwSearch.RunWorkerAsync();
            }
            ProcessStarted();
        }
        private void btnStartProcess_Click(object sender, EventArgs e)
        {
            lblReport.Text = string.Empty;
            switch (listType)
            {
                case ListTypeEnum.Onayli:
                    lblReport.Text += "<html><strong><span style=\"font-size: 10pt\">ONAY İPTAL İŞLEMİ BAŞLATILDI</font></strong></html>\r\n\r\n";
                    btnCancelVizite.Text = "Onay iptal işlemini iptal et";
                    rwbVizite.Text = "ONAY İPTAL İŞLEMİ DEVAM EDİYOR...";
                    break;
                case ListTypeEnum.Onaylanacak:
                    lblReport.Text += "<html><strong><span style=\"font-size: 10pt\">RAPOR ONAYLAMA İŞLEMİ BAŞLATILDI</font></strong></html>\r\n\r\n";
                    btnCancelVizite.Text = "Rapor onaylamayı iptal et";
                    rwbVizite.Text = "RAPOR ONAYLAMA İŞLEMİ DEVAM EDİYOR...";
                    break;
                default:
                    break;
            }

            if (!bgwConfirm.IsBusy)
            {
                bgwConfirm.RunWorkerAsync();
            }
            ProcessStarted();
        }
        private void btnExcel_Click(object sender, EventArgs e)
        {
            string title = "", msg;
            IOC.ExportService.CreateFile(out msg);
            try
            {
                switch (modulType)
                {
                    case ModulTypeEnum.Vizite:
                        if (rgvReportList.RowCount == 0) { lblMessage.Text = "Lütfen önce tarama başlatın"; return; }
                        List<VisitsToBeProcessed> lst;
                        switch (exportTypeEnum)
                        {
                            case ExportTypeEnum.Ony:
                                SearchReport.ReportTypeAfterRgvLoad = 3;
                                lst = IOC.WinHelpers.GetAllVisitsFromRgv(rgvReportList, out msg);
                                title = $"{SearchReport.StartDate.ToString("dd.MM.yyyy")} - {SearchReport.EndDate.ToString("dd.MM.yyyy")} TARİHLERİ ARASINDAKİ ONAYLI RAPORLAR";
                                IOC.ExportService.CreateFileForVisits(title, "onayli", lst, out msg);
                                break;
                            case ExportTypeEnum.Onstr:
                                SearchReport.ReportTypeAfterRgvLoad = 2;
                                lst = IOC.WinHelpers.GetAllVisitsFromRgv(rgvReportList, out msg);
                                title = $"{SearchReport.SearchDate.ToString("dd.MM.yyyy")} İTİBARİYLE ONAYLANMAMIŞ RAPORLAR";
                                IOC.ExportService.CreateFileForVisits(title, "onaylanacak", lst, out msg);
                                break;
                            case ExportTypeEnum.Onstc:
                                SearchReport.ReportTypeAfterRgvLoad = 1;
                                lst = IOC.WinHelpers.GetAllVisitsFromRgv(rgvReportList, out msg);
                                title = $"{SearchReport.KimlikNo} KİMLİK NUMARALI PERSONELİN ONAYLANMAMIŞ RAPORLARI";
                                IOC.ExportService.CreateFileForVisits(title, "onaylanacak", lst, out msg);
                                break;
                            case ExportTypeEnum.Arv:
                                SearchReport.ReportTypeAfterRgvLoad = 4;
                                lst = IOC.WinHelpers.GetAllVisitsFromRgv(rgvReportList, out msg);
                                title = $"{SearchReport.StartDate.ToString("dd.MM.yyyy")} - {SearchReport.EndDate.ToString("dd.MM.yyyy")} TARİHLERİ ARASINDAKİ ARŞİVLENMİŞ RAPORLAR";
                                IOC.ExportService.CreateFileForVisits(title, "arsiv", lst, out msg);
                                break;
                        }
                        break;
                    case ModulTypeEnum.Hesapdurumu:
                        bool goXls = false;
                        GlobalVars.LstDb = lstDb; if (lstDb != null && lstDb.Count > 0) { title = "DÖNEMSEL BORÇLAR"; IOC.ExportService.CreateXlsHesap(title, 1, out msg); goXls = true; }
                        GlobalVars.LstEt = lstEt; if (lstEt != null && lstEt.Count > 0) { title = "EMANETTEKİ TAHSİLATLAR"; IOC.ExportService.CreateXlsHesap(title, 2, out msg); goXls = true; }
                        GlobalVars.LstMe = lstMe; if (lstMe != null && lstMe.Count > 0) { title = "MOSSİP EMANET TAHSİLATLARI"; IOC.ExportService.CreateXlsHesap(title, 3, out msg); goXls = true; }
                        GlobalVars.LstCr = lstCr; if (lstCr != null && lstCr.Count > 0) { title = "İCRA BİLGİLERİ"; IOC.ExportService.CreateXlsHesap(title, 4, out msg); goXls = true; }
                        GlobalVars.Lst6661 = lst6661; if (lst6661 != null && lst6661.Count > 0) { title = "6661 ASGARİ DESTEK"; IOC.ExportService.CreateXlsHesap(title, 5, out msg); goXls = true; }
                        if (!goXls) { lblMessage.Text = "Excel'e aktarılacak bir tablo bulunamadı."; return; } else { title = "HESAP DURUMU"; }
                        break;
                    case ModulTypeEnum.Igl:
                        bool goXlsIgl = false; 
                        GlobalVars.LstIgl = lstIgl; if (lstIgl != null && lstIgl.Count > 0) { title = "İŞE GİRİŞ_İŞTEN ÇIKIŞ LİSTESİ"; IOC.ExportService.CreateXlsHesap(title, 7, out msg); goXlsIgl = true; }
                        if (!goXlsIgl) { lblMessage.Text = "Excel'e aktarılacak bir tablo bulunamadı."; return; } else { title = "İŞE GİRİŞ_İŞTEN ÇIKIŞ LİSTESİ"; }

                        break;
                    case ModulTypeEnum.Hizmet:
                        bool goXlsHlp = false;
                        GlobalVars.LstHlp = lstHlp; if (lstHlp != null && lstHlp.Count > 0) { title = "HİZMET LİSTESİ"; IOC.ExportService.CreateXlsHesap(title, 6, out msg); goXlsHlp = true; }
                        if (!goXlsHlp) { lblMessage.Text = "Excel'e aktarılacak bir tablo bulunamadı."; return; }  title = "HİZMET LİSTESİ"; 

                        break;
                    case ModulTypeEnum.Tahakkuk:
                        bool goXlsThkk = false;
                        GlobalVars.LstThkk = lstThkk; if(lstThkk != null && lstThkk.Count > 0) { title = "TAHAKKUKLAR"; IOC.ExportService.CreateXlsHesap(title, 8, out msg); goXlsThkk = true; }
                        if (!goXlsThkk) { lblMessage.Text = "Excel'e aktarılacak bir tablo bulunamadı."; return; } title = "TAHAKKUKLAR";
                        break;
                    case ModulTypeEnum.Izin:
                        if (GlobalVars.PersonalsForLeave == null || GlobalVars.PersonalsForLeave.Count == 0) { lblMessage.Text = "Excel'e aktarılacak bir tablo bulunamadı."; return; }
                        if (sender == btnExportPersonals)
                        {
                            title = $"{GlobalVars.IzinComp.CompanyName} PERSONEL LİSTESİ"; IOC.ExportService.CreateFileForPersonals(title, GlobalVars.Personals, out msg);
                        }
                        else if (sender == btnExportPersonalPeriods)
                        {
                            personalsTemp = new List<Personal>(); personalsTemp.AddRange(GlobalVars.PersonalsForLeave);
                            periodsTemp = new List<LeavePeriod>(); periodsTemp.AddRange(GlobalVars.LeavePeriods);
                            GridViewRowInfo row = rgvLeaves.CurrentRow;

                            GlobalVars.PersonalsForLeave = (from x in personalsTemp where x.Tcno == row.Cells[1].Value.ToString() select x).ToList();
                            GlobalVars.LeavePeriods = (from x in periodsTemp where x.Tcno == row.Cells[1].Value.ToString() select x).ToList();

                            title = $"{GlobalVars.PersonalsForLeave[0].Ads} İZİN DÖNEMLERİ LİSTESİ"; IOC.ExportService.CreateFileForPersonalsAndPeriods(title, GlobalVars.PersonalsForLeave, GlobalVars.LeavePeriods, out msg);

                            GlobalVars.PersonalsForLeave = personalsTemp;
                            GlobalVars.LeavePeriods = periodsTemp;
                        }
                        else if (sender == btnExportPersonalsPeriods)
                        {
                            title = $"{GlobalVars.IzinComp.CompanyName} PERSONEL VE İZİN DÖNEMLERİ LİSTESİ"; IOC.ExportService.CreateFileForPersonalsAndPeriods(title, GlobalVars.PersonalsForLeave, GlobalVars.LeavePeriods, out msg);
                        }
                        break;
                }

                
                if (IOC.ExportService.Save(title, out msg)) {
                    lblMessage.Text = msg; btnOpenFile.Visibility = ElementVisibility.Visible;
                } else
                {
                    lblMessage.Text = msg;
                    btnOpenFile.Visibility = ElementVisibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                lblMessage.Text = $"Excel'e aktarma başarısız! Hata: {msg}";
            }
        }
        private void btnHideMessage_Click(object sender, EventArgs e)
        {
            lblMessage.Text = String.Empty;
            btnConfirmResult.Visibility = ElementVisibility.Collapsed;
            btnHideMessage.Visibility = ElementVisibility.Collapsed;
            btnOpenFile.Visibility = ElementVisibility.Collapsed;
            btnSoyadUpdateReport.Visibility = ElementVisibility.Collapsed;
        }
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            string location = modulType == ModulTypeEnum.HizmetUcretsiz? pdfHluFolder : hesapType == HesapTypeEnum.EborcuYoktur ? SearchReport.UserSelectedDir : IOC.ExportService.FileName;
            Process.Start(location);
        }
        private void btnConfirmResult_Click(object sender, EventArgs e)
        {
            switch (listType)
            {
                case ListTypeEnum.Onayli:
                    fUnconfirmDetails fu = new fUnconfirmDetails();
                    fu.ShowDialog();
                    break;
                case ListTypeEnum.Onaylanacak:
                    if (IOC.VisitReportService.ConfirmReports != null && IOC.VisitReportService.ConfirmReports.Count > 0)
                    {
                        FConfirmDetails f = new FConfirmDetails();
                        f.ShowDialog();
                    }
                    break;
            }
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!PrepareForPrint(sender)) { return;}
            
            RadGridView rgv = new RadGridView();
            try
            {
                RadPrintDocument document = new RadPrintDocument();
                document.DefaultPageSettings.Landscape = true;
                document.DefaultPageSettings.PrinterSettings.Copies = 2;
                switch (modulType)
                {
                    case ModulTypeEnum.Vizite:
                        document.AssociatedObject = this.rgvReportList; rgv = rgvReportList;
                        break;
                    case ModulTypeEnum.Hesapdurumu:
                    case ModulTypeEnum.Igl:
                        switch (hesapType)
                        {
                            case HesapTypeEnum.Donemselborc:
                            case HesapTypeEnum.Icra:
                            case HesapTypeEnum.Destek6661:
                            case HesapTypeEnum.Igl:
                                document.AssociatedObject = this.rgvSgkOtomasyon; rgv = rgvSgkOtomasyon;
                                break;
                            case HesapTypeEnum.Emanet:
                                if (sender == btnPrintEt)
                                {
                                    document.AssociatedObject = this.rgvSgkOtomasyon; rgv = rgvSgkOtomasyon;
                                }
                                else if (sender == btnPrintMe)
                                {
                                    document.AssociatedObject = this.rgvMossip; rgv = rgvMossip;

                                }
                                break;
                        }
                        break;
                    case ModulTypeEnum.Hizmet:
                        if (sender == btnPrintOnaylibildirge)
                        {
                            document.AssociatedObject = this.rgvSgkOtomasyon; rgv = rgvSgkOtomasyon;
                        }
                        else if (sender == btnPrintHizmetliste)
                        {
                            document.AssociatedObject = this.rgvMossip; rgv = rgvMossip;
                            document.Landscape = true;
                        }
                        break;
                    case ModulTypeEnum.Tahakkuk:
                        document.AssociatedObject = this.rgvSgkOtomasyon; rgv = rgvSgkOtomasyon;
                        break;
                    case ModulTypeEnum.Izin:
                        if (sender == btnPrintPersonals)
                        {
                            hierarchy = false; isPersonal = false;
                            document.AssociatedObject = this.rgvLeaves; rgv = rgvLeaves;
                        }
                        else if (sender == btnPrintPersonalPeriods)
                        {
                            hierarchy = true; isPersonal = true;
                            document.AssociatedObject = this.rgvLeaves; rgv = rgvLeaves;
                        }
                        else if (sender == btnPrintPersonalsPeriods)
                        {
                            hierarchy = true; isPersonal = false;
                            document.AssociatedObject = this.rgvLeaves; rgv = rgvLeaves;
                        }
                        break;
                }

                GridPrintStyle style = new GridPrintStyle();
                int gridWidth = 0;
                int i = 0;
                spCompanyList.Collapsed = true;
                for (i = 0; i <= rgv.ColumnCount - 1; i++)
                {
                    if (rgv.Columns[i].IsVisible)
                    {
                        gridWidth += rgv.Columns[i].Width;
                    }
                }

                Margins margins = document.DefaultPageSettings.Margins;
                int documentWidth = document.DefaultPageSettings.Bounds.Width;
                documentWidth -= margins.Left + margins.Right;

                if (gridWidth < documentWidth)
                {
                    style.CellFont = new Font(FontFamily.GenericSansSerif, 8.0F, FontStyle.Italic);
                    document.DefaultPageSettings.Landscape = false;
                }
                else
                {
                    style.CellFont = new Font(FontFamily.GenericSansSerif, 6.0F);
                    document.DefaultPageSettings.Landscape = true;
                }

                style.FitWidthMode = PrintFitWidthMode.FitPageWidth;
                style.PrintGrouping = true;
                style.PrintSummaries = true;
                style.PrintHeaderOnEachPage = true;
                style.PrintHiddenColumns = false;
                if (hierarchy)
                {
                    style.PrintHierarchy = true;
                    style.HierarchyIndent = 20;
                    style.ChildViewPrintMode = ChildViewPrintMode.PrintCurrentlyActiveView;
                    style.PrintAllPages = true;
                    style.PrintHiddenRows = false;
                }
                rgv.PrintStyle = style;

                document.DefaultPageSettings.Margins = new System.Drawing.Printing.Margins(10, 10, 10, 10);
                document.AssociatedObject = rgv;

                RadPrintPreviewDialog dialog = new RadPrintPreviewDialog();
                dialog.Document = document;
                dialog.ShowDialog();
                AfterPrint();
            }
            catch (Exception)
            {
                lblMessage.Text = "Yazdırma işlemi tamamlanamadı, lütfen yazıcı ayarlarını kontrol edip tekrar deneyin";
            }
        }
        private bool PrepareForPrint(object sender)
        {
            switch (modulType)
            {
                case ModulTypeEnum.Vizite:
                    if (rgvReportList.RowCount == 0) { lblMessage.Text = "Yazdırılacak bir şey bulunamadı. Lütfen önce tarama başlatın"; return false; }
                    spCompanyList.Collapsed = true;
                    rgvReportList.Columns[0].IsVisible = false;
                    rgvReportList.Columns[8].IsVisible = false;
                    rgvReportList.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
                    rgvReportList.BestFitColumns();
                    break;
                case ModulTypeEnum.Hesapdurumu:
                    switch (hesapType)
                    {
                        case HesapTypeEnum.Donemselborc:
                            if (lstDb.Count == 0) { lblMessage.Text = "Yazdırılacak dönemsel borç bulunamadı. Lütfen önce tarama başlatın veya indirilenleri listeleyin."; return false; }
                            spCompanyList.Collapsed = true;
                            rgvSgkOtomasyon.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
                            foreach (var item in rgvSgkOtomasyon.Columns)
                            {
                                item.MinWidth = 0;
                                item.Width = 20;
                            }
                            rgvSgkOtomasyon.BestFitColumns();
                            break;
                        case HesapTypeEnum.Icra:
                            if (lstCr.Count == 0) { lblMessage.Text = "Yazdırılacak icra bilgisi bulunamadı. Lütfen önce tarama başlatın veya indirilenleri listeleyin."; return false; }
                            spCompanyList.Collapsed = true;
                            foreach (var item in rgvSgkOtomasyon.Columns)
                            {
                                item.MinWidth = 0;
                                item.Width = 20;
                            }
                            rgvSgkOtomasyon.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
                            rgvSgkOtomasyon.BestFitColumns();
                            break;
                        case HesapTypeEnum.Destek6661:
                            if (lst6661.Count == 0) { lblMessage.Text = "Yazdırılacak 6661 destek bilgisi bulunamadı. Lütfen önce tarama başlatın veya indirilenleri listeleyin."; return false; }
                            spCompanyList.Collapsed = true;
                            foreach (var item in rgvSgkOtomasyon.Columns)
                            {
                                item.MinWidth = 0;
                                item.Width = 20;
                            }
                            rgvSgkOtomasyon.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
                            rgvSgkOtomasyon.BestFitColumns();
                            break;
                        case HesapTypeEnum.Igl:
                            if (lstIgl.Count == 0) { lblMessage.Text = "Yazdırılacak işe giriş/çıkış bilgisi bulunamadı. Lütfen önce tarama başlatın veya indirilenleri listeleyin."; return false; }
                            spCompanyList.Collapsed = true;
                            foreach (var item in rgvSgkOtomasyon.Columns)
                            {
                                item.MinWidth = 0;
                                item.Width = 20;
                            }
                            rgvSgkOtomasyon.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
                            rgvSgkOtomasyon.BestFitColumns();
                            break;
                        case HesapTypeEnum.Emanet:
                            if (sender == btnPrintEt)
                            {
                                if (lstEt.Count == 0) { lblMessage.Text = "Yazdırılacak banka/işveren emanet tahsilatı bilgisi bulunamadı. Lütfen önce tarama başlatın veya indirilenleri listeleyin."; return false; }
                                spCompanyList.Collapsed = true;
                                //foreach (var item in rgvSgkOtomasyon.Columns)
                                //{
                                //    item.MinWidth = 0;
                                //    item.Width = 20;
                                //}
                                rgvSgkOtomasyon.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
                                rgvSgkOtomasyon.BestFitColumns();
                            }
                            else if (sender == btnPrintMe)
                            {
                                if (lstMe.Count == 0) { lblMessage.Text = "Yazdırılacak mossip emanet tahsilatı bilgisi bulunamadı. Lütfen önce tarama başlatın veya indirilenleri listeleyin."; return false; }
                                spCompanyList.Collapsed = true;
                                //foreach (var item in rgvMossip.Columns)
                                //{
                                //    item.MinWidth = 0;
                                //    item.Width = 20;
                                //}
                                rgvMossip.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
                                rgvMossip.BestFitColumns();
                            }
                            else
                            {
                                lblMessage.Text = "Lütfen açılan menüden yazdırmak istediğiniz tabloyu seçin."; return false;
                            }
                            break;
                    }
                    break;
                case ModulTypeEnum.Hizmet:
                    if (sender == btnPrintOnaylibildirge)
                    {
                        if (lstHl.Count == 0) { lblMessage.Text = "Yazdırılacak onaylı bildirge bulunamadı. Lütfen önce tarama başlatın veya indirilenleri listeleyin."; return false; }
                        rgvSgkOtomasyon.Columns["OpenPdf"].IsVisible = false;
                        rgvSgkOtomasyon.Columns["OpenHl"].IsVisible = false;
                        spCompanyList.Collapsed = true;
                        spRgvMossip.Collapsed = true;
                        rgvSgkOtomasyon.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
                        rgvSgkOtomasyon.BestFitColumns();
                    }
                    else if (sender == btnPrintHizmetliste)
                    {
                        if (lstHlp.Count == 0) { lblMessage.Text = "Yazdırılacak hizmet listesi bulunamadı. Lütfen önce tarama başlatın veya indirilenleri listeleyin."; return false; }
                        spCompanyList.Collapsed = true;
                        spRgvSgkOtomasyon.Collapsed = true;
                        rgvMossip.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
                        rgvMossip.BestFitColumns();
                    }
                    else
                    {
                        lblMessage.Text = "Lütfen açılan menüden yazdırmak istediğiniz tabloyu seçin."; return false;
                    }
                    break;
                case ModulTypeEnum.Tahakkuk:
                    if(lstThkk.Count == 0) { { lblMessage.Text = "Yazdırılacak tahakkuk bulunamadı. Lütfen önce tarama başlatın veya indirilenleri listeleyin."; return false; } }
                    rgvSgkOtomasyon.Columns["OpenPdf"].IsVisible = false;
                    spCompanyList.Collapsed = true;
                    spRgvMossip.Collapsed = true;
                    rgvSgkOtomasyon.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
                    rgvSgkOtomasyon.BestFitColumns();
                    break;
                case ModulTypeEnum.Igl:
                    if (sender == btnPrintIGL)
                    {
                        if (lstIgl.Count == 0) { lblMessage.Text = "Yazdırılacak işe giriş/işten çıkış listesi bulunamadı. Lütfen önce tarama başlatın veya indirilenleri listeleyin."; return false; }
                        spCompanyList.Collapsed = true;
                        spRgvMossip.Collapsed = true;
                        foreach (var item in rgvSgkOtomasyon.Columns)
                        {
                            item.MinWidth = 0;
                            item.Width = 20;
                        }
                        rgvSgkOtomasyon.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
                        rgvSgkOtomasyon.BestFitColumns();

                    }
                    break;
                case ModulTypeEnum.Izin:
                    if(rgvLeaves.DataSource == null) { lblMessage.Text = "Yazdırılacak bir şey bulunamadı. Lütfen önce bir listeleme işlemi başlatın "; return false; }
                    if (sender == btnPrintPersonalPeriods)
                    {
                        personalsTemp = new List<Personal>(); personalsTemp.AddRange( GlobalVars.PersonalsForLeave);
                        periodsTemp = new List<LeavePeriod>(); periodsTemp.AddRange( GlobalVars.LeavePeriods);
                        GridViewRowInfo row = rgvLeaves.CurrentRow;

                        GlobalVars.PersonalsForLeave = (from x in personalsTemp where x.Tcno == row.Cells[1].Value.ToString() select x).ToList();
                        GlobalVars.LeavePeriods = (from x in periodsTemp where x.Tcno == row.Cells[1].Value.ToString() select x).ToList();
                        RgvHelpers.SetLeaveListRgv(rgvLeaves, out msg);
                    }
                    else
                    {
                        rgvTemp.DataSource = null;
                    }
                    break;
            }
            return true;
        }
        private void AfterPrint()
        {
            spCompanyList.Collapsed = false;
            switch (modulType)
            {
                case ModulTypeEnum.Vizite:
                    rgvReportList.Columns[0].IsVisible = true;
                    rgvReportList.Columns[8].IsVisible = true;
                    rgvReportList.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    break;
                case ModulTypeEnum.Hesapdurumu:
                case ModulTypeEnum.Igl:
                    switch (hesapType)
                    {
                        case HesapTypeEnum.Donemselborc:
                            IOC.SgkAutomations.SetRgvPd(rgvSgkOtomasyon, GlobalVars.DebtLayout);
                            break;
                        case HesapTypeEnum.Emanet:
                            spRgvMossip.Collapsed = false;
                            IOC.SgkAutomations.SetRgvEt(rgvSgkOtomasyon);
                            IOC.SgkAutomations.SetRgvMe(rgvMossip);
                            break;
                        case HesapTypeEnum.Icra:
                            IOC.SgkAutomations.SetRgvCr(rgvSgkOtomasyon);
                            break;
                        case HesapTypeEnum.Destek6661:
                            IOC.SgkAutomations.SetRgv6661(rgvSgkOtomasyon);
                            break;
                        case HesapTypeEnum.Igl:
                            IOC.SgkAutomations.SetRgvIgl(rgvSgkOtomasyon);
                            break;
                        default:
                            break;
                    }
                    break;
                case ModulTypeEnum.Hizmet:
                    spRgvMossip.Collapsed = false;
                    spRgvSgkOtomasyon.Collapsed = false;
                    rgvSgkOtomasyon.Columns["OpenPdf"].IsVisible = true;
                    rgvSgkOtomasyon.Columns["OpenHl"].IsVisible = true;
                    IOC.SgkAutomations.SetRgvHl(rgvSgkOtomasyon, true);
                    IOC.SgkAutomations.SetRgvHlp(rgvMossip);
                    break;
                case ModulTypeEnum.Tahakkuk:
                    spRgvMossip.Collapsed = true;
                    spRgvSgkOtomasyon.Collapsed = false;
                    rgvSgkOtomasyon.Columns["OpenPdf"].IsVisible = true;
                    IOC.SgkAutomations.SetRgvThkk(rgvSgkOtomasyon);
                    //IOC.SgkAutomations.HideThkkColumn(rgvSgkOtomasyon, lstThkk);
                    break;
                case ModulTypeEnum.Izin:
                    if (isPersonal)
                    {
                        GlobalVars.PersonalsForLeave = personalsTemp;
                        GlobalVars.LeavePeriods = periodsTemp;
                        RgvHelpers.SetLeaveListRgv(rgvLeaves, out msg);
                    }
                    break;
            }
        }
        #endregion

        #region rapor_tarama_onay_ve_iptal_isegiriscikiskontrolu
        private bool ViziteCek(Company login, out string msg)
        {
            acError = false;
            lblReport.Text = GlobalVars.ProcessReport;
            if (Surucu.Driver == null || !IOC.LinkOps.IsBrowserOpen()) 
                Surucu.Driver = IOC.WinHelpers.GetWebDriver(Settings.Default.hideBrowser, out msg); 

            try
            {
                IOC.VisitReportService.SetSgkLoginCredentials(login);
                
                SearchReport.CompanyNameSearching = login.CompanyName;
                
                IOC.SgkLinksService.Command = IOC.LinkOps.CmdUrlVurl.Cmd;
                IOC.SgkLinksService.Url = IOC.LinkOps.CmdUrlVurl.Url;
                IOC.SgkLinksService.Vurl = IOC.LinkOps.CmdUrlVurl.Vurl;
                IOC.SgkLinksService.SetSgkLoginCredentials(login);
                //url = IOC.SgkLinksService.Command.Substring(4, IOC.SgkLinksService.Command.IndexOf('\r') - 4);
                
                IOC.LinkOps.VisitLink(login, IOC.SgkLinksService.Url, ref acError, out msg, ref lblReport);

                if (msg.Contains("iptal")) { return false; }
                else if (msg == "continue" || msg.Contains("Hata"))
                {
                    GlobalVars.ProcessReport += $"<li><span style=\"font-size: 10pt; color: red\"><strong>{SearchReport.CompanyNameSearching} için oturum açılamadı! {IOC.SgkLinksService.Message}</strong></span></li></ul></html><ul>";
                    lblReport.Text = GlobalVars.ProcessReport;
                    lblMessage.Text = $"{SearchReport.CompanyNameSearching} için oturum açılamadı! {IOC.SgkLinksService.Message}";
                    return false; ;
                }
                else if (msg == "continueGoAhead")
                {
                    GlobalVars.ProcessReport += $"<li><span style=\"font-size: 10pt; color: red\"><strong>Oturum açıldıktan sonra devam edilemiyor. Hata: {IOC.SgkLinksService.Message}</strong></span></li></ul></html><ul>";
                    lblReport.Text = GlobalVars.ProcessReport;
                    lblMessage.Text = $"Sayfada hata! {IOC.SgkLinksService.Message}";
                    return false; ;
                }
                try
                {
                    if (processType == ProcessTypeEnum.Raportarama)
                    {
                        if (bgwSearch.CancellationPending || bgwConfirm.CancellationPending || bgwDetails.CancellationPending) { return false; }
                        string reportType ="";
                        switch (SearchReport.ReportType)
                        {
                            case 1:
                            case 2:
                            case 4:
                                reportType = "Onaylanmamış";
                                break;
                            case 3:
                                reportType = "Onaylanmış";
                                break;
                        }

                        GlobalVars.ProcessReport += $"<li><strong><span style=\"font-size: 10pt\">{reportType} raporlar taranıyor</span></strong></li>";
                        lblReport.Text = GlobalVars.ProcessReport;
                        SearchReport.KimlikNo = texTckn.Text.Trim();
                        SearchReport.StartDate = (DateTime)dtpViziteFirst.Value;
                        SearchReport.EndDate = (DateTime)dtpViziteEnd.Value;
                        (visitsReceived, lblMessage.Text, rgvTitleText) = IOC.VisitReportService.StartSearch(out msg);
                        int count = (from x in visitsReceived where x.Cnm == login.CompanyName select x).Count();
                        if (count == 0) {
                            GlobalVars.ProcessReport += $"<li><strong><span style=\"font-size: 10pt\">{reportType} rapor yok</span></strong></li>";
                            lblReport.Text = GlobalVars.ProcessReport;
                        } else
                        {
                            GlobalVars.ProcessReport += $"<li><strong><span style=\"font-size: 10pt\">{count} adet {reportType.ToLower()} rapor bulundu</span></strong></li>";
                            lblReport.Text = GlobalVars.ProcessReport;
                        }    
                        if (lblMessage.Text == "iptal") { return false; }
                        Thread.Sleep(2000);
                    }
                    else if (processType == ProcessTypeEnum.Raporonaylama)
                    {
                        if (bgwSearch.CancellationPending || bgwConfirm.CancellationPending || bgwDetails.CancellationPending) { return false; }

                        GlobalVars.ProcessReport += $"<li><strong><span style=\"font-size: 10pt\">Onaylama işlemi başlatılıyor</span></strong></li>";
                        lblReport.Text = GlobalVars.ProcessReport;
                        List<VisitsToBeProcessed> lst = (from x in visitsToBeProcessed where x.Cnm == login.CompanyName select x).ToList();
                        int submitted = 0;
                        (lblMessage.Text, rgvTitleText, submitted) = IOC.VisitReportService.DoConfirmReport(lst, out msg);
                        if (lblMessage.Text == "iptal") {
                            GlobalVars.ProcessReport += $"<li><strong><span style=\"font-size: 10pt\">Rapor onaylama işlemi iptal edildi</span></strong></li>";
                            lblReport.Text = GlobalVars.ProcessReport;
                            return false; 
                        }else if (lblMessage.Text.Contains("tamamlanamadı"))
                        {
                            GlobalVars.ProcessReport += $"<li><strong><span style=\"color: red; font-size: 10pt\">{lblMessage.Text}<br>{lst.Count} adet rapordan {submitted} tanesi onaylandı</span></strong></li>";
                            lblReport.Text = GlobalVars.ProcessReport;
                            return false;
                        }
                        GlobalVars.ProcessReport += $"<li><strong><span style=\"font-size: 10pt\">{submitted} adet rapor onaylandı</span></strong></li>";
                        lblReport.Text = GlobalVars.ProcessReport;
                        Thread.Sleep(2000);
                    }
                    else if (processType == ProcessTypeEnum.Onayiptaletme)
                    {
                        if (bgwSearch.CancellationPending || bgwConfirm.CancellationPending || bgwDetails.CancellationPending) { return false; }
                        List<VisitsToBeProcessed> lst = (from x in visitsToBeProcessed where x.Cnm == login.CompanyName select x).ToList();
                        int unSubmitted = 0;

                        GlobalVars.ProcessReport += $"<li><strong><span style=\"font-size: 10pt\">{lst.Count} adet rapor onayı iptal ediliyor</span></strong></li>";
                        lblReport.Text = GlobalVars.ProcessReport;
                        (lblMessage.Text, rgvTitleText, unSubmitted) = IOC.VisitReportService.DoCancelReport(lst, out msg);
                        if (lblMessage.Text == "iptal")
                        {
                            GlobalVars.ProcessReport += $"<li><strong><span style=\"font-size: 10pt\">Rapor onay iptal işlemi iptal edildi</span></strong></li>";
                            lblReport.Text = GlobalVars.ProcessReport;
                            return false;
                        }
                        else if (lblMessage.Text.Contains("tamamlanamadı"))
                        {
                            GlobalVars.ProcessReport += $"<li><strong><span style=\"color: red; font-size: 10pt\">{lblMessage.Text}<br>{lst.Count} adet rapordan {unSubmitted} tanesinin onayı iptal edildi</span></strong></li>";
                            lblReport.Text = GlobalVars.ProcessReport;
                            return false;
                        }
                        GlobalVars.ProcessReport += $"<li><strong><span style=\"font-size: 10pt\">{lst.Count} adet rapordan {unSubmitted} tanesinin onayı iptal edildi</span></strong></li>";
                        lblReport.Text = GlobalVars.ProcessReport;
                    }
                    else if (processType == ProcessTypeEnum.Raporayrintilari)
                    {
                        if (bgwSearch.CancellationPending || bgwConfirm.CancellationPending || bgwDetails.CancellationPending) { return false; }

                        VisitsToBeProcessed v = IOC.WinHelpers.GetVisitFromRgv(rgvReportList, out msg);
                        GlobalVars.ProcessReport += $"<li><strong><span style=\"font-size: 10pt\">{v.Tcno} kimlik numaralı personelin rapor ayrıntılarına ulaşılıyor</span></strong></li>";
                        lblReport.Text = GlobalVars.ProcessReport;
                        (sourceIgBs, sourceSpvudKs, sourceSraoDs) = IOC.VisitReportService.GetReportDetails(v, out msg);
                        if (sourceIgBs == null && sourceSpvudKs == null && sourceSraoDs == null && msg == "iptal") { return false; }
                        Thread.Sleep(1000);
                    }
                    else if (processType == ProcessTypeEnum.Pregnancycheck)
                    {
                        if (bgwSearch.CancellationPending || bgwConfirm.CancellationPending || bgwDetails.CancellationPending) { return false; }

                        GlobalVars.ProcessReport += "<li><span style=\"font-size: 10pt\">Vaka durumu 'ANALIK' olan raporlar için, rapor ayrıntıları sayfasından 'Rapor Başlama Tarihleri' alınıyor</span></li>";
                        lblReport.Text = GlobalVars.ProcessReport;
                        visitsReceived = IOC.VisitReportService.PregnancyCheck(visitsReceived, out msg);
                        if (visitsReceived == null && msg == "iptal") { return false; }
                    }

                }
                catch (Exception)
                {
                    GlobalVars.ProcessReport += "<li><span style=\"font-size: 10pt\">SGK sunucularından yanıt alınamadı; lütfen internet bağlantınızı kontrol edip tekrar deneyin</span></li>";
                    lblReport.Text = GlobalVars.ProcessReport;
                    lblMessage.Text = "SGK sunucularından yanıt alınamadı; lütfen internet bağlantınızı kontrol edip tekrar deneyin";
                }
                finally
                {
                    GlobalVars.ProcessReport += "</ul></html>";
                    lblReport.Text = GlobalVars.ProcessReport;
                    viziteRapor += lblReport.Text;
                    lblReport.Text = $"<html><ul>";
                    GlobalVars.ProcessReport = lblReport.Text;
                }
            }
            catch (Exception ex)
            {
                GlobalVars.ProcessReport += $"<li><strong><span style=\"color: red; font-size: 10pt\">Hata: {ex.Message}</span></strong></li></ul></html>";
                viziteRapor += lblReport.Text;
                GlobalVars.ProcessReport = $"<html><ul>";
                lblReport.Text = GlobalVars.ProcessReport;
                lblMessage.Text = $"Hata: {ex.Message}";
                msg = ex.Message.ToString();
                return false;
            }
            return true;
        }
        private bool PersonelCheck(Company login, out string msg)
        {
            acError = false; msg = "";
            if (Surucu.Driver == null || !IOC.LinkOps.IsBrowserOpen())
                Surucu.Driver = IOC.WinHelpers.GetWebDriver(Settings.Default.hideBrowser, out msg);

            rgvReportList.TitleText = "Rapor tarama işlemi bitti. Personelim değil kontrol işlemi başlatıldı";
            try
            {
                
                IOC.PersonelimDegil.SetSgkLoginCredentials(login);
                IOC.VisitReportService.SetSgkLoginCredentials(login);

                if (bgwSearch.CancellationPending || bgwConfirm.CancellationPending || bgwDetails.CancellationPending) { return false; }
                SearchReport.CompanyNameSearching = login.CompanyName;
                IOC.LinkOps.CmdUrlVurl = LinkGlobals.LstLinks.Where(x => x.Id == 41).Select(x => new CmdUrlVurl { Cmd = x.Cmd,Url = x.Url, Vurl = x.Vurl }).FirstOrDefault();
                IOC.SgkLinksService.Command = IOC.LinkOps.CmdUrlVurl.Cmd;
                IOC.SgkLinksService.Url = IOC.LinkOps.CmdUrlVurl.Url;
                IOC.SgkLinksService.Vurl = IOC.LinkOps.CmdUrlVurl.Vurl;
                IOC.SgkLinksService.SetSgkLoginCredentials(login);
                
                IOC.LinkOps.VisitLink(login, IOC.SgkLinksService.Url, ref acError, out msg, ref lblReport);

                if (msg.Contains("iptal")) { return false; }
                else if (msg == "continue" || msg.Contains("Hata"))
                {
                    GlobalVars.ProcessReport += $"<li><strong><span style=\"color: red; font-size: 10pt\">{SearchReport.CompanyNameSearching} için oturum açılamadı! {IOC.SgkLinksService.Message}</span></strong></li></ul></html><ul>";
                    lblReport.Text = GlobalVars.ProcessReport;
                    lblMessage.Text = $"{SearchReport.CompanyNameSearching} için oturum açılamadı! {IOC.SgkLinksService.Message}";
                    lblReport.Text = "<html>"; return false; ;
                }
                else if (msg == "continueGoAhead")
                {
                    GlobalVars.ProcessReport += $"<li><strong><span style=\"color: red; font-size: 10pt\">Oturum açıldıktan sonra devam edilemiyor. Hata: {IOC.SgkLinksService.Message}</span></strong></li></ul></html><ul>";
                    lblReport.Text = GlobalVars.ProcessReport;
                    lblMessage.Text = $"Oturum açıldıktan sonra devam edilemiyor. Hata: {IOC.SgkLinksService.Message}";
                    lblReport.Text = "<html>"; return false; ;
                }

                try
                {
                    GlobalVars.ProcessReport += $"<li><strong><span style=\"font-size: 10pt\">{visitsReceived.Count} adet rapor için personelim değil sorgusu başladı! {IOC.SgkLinksService.Message}</span></strong></li>";
                    lblReport.Text = GlobalVars.ProcessReport;
                    viziteRapor += $"<li><strong><span style=\"font-size: 10pt\">{visitsReceived.Count} adet rapor için personelim değil sorgusu başladı! {IOC.SgkLinksService.Message}</span></strong></li>";
                    bool pdResult = IOC.PersonelimDegil.DoWork(visitsReceived, out msg);
                    if (IOC.PersonelimDegil.ErrorList != "")
                    {
                        GlobalVars.ProcessReport += $"<li><strong><span style=\"color: red; font-size: 10pt\">İşe giriş/çıkış kayıtları hatalı olan personel(ler) var!<ul>{IOC.PersonelimDegil.ErrorList}</ul></span></strong></li>";
                        lblReport.Text = GlobalVars.ProcessReport;
                        RadMessageBox.Show($"{IOC.PersonelimDegil.ErrorList.Replace("<li>", "* ").Replace("</li>","\r\n")}", "İşe giriş/çıkışı hatalı personel var!", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                    }
                    if (!pdResult) 
                        return false; 
                }
                catch (Exception)
                {
                    lblReport.Text += $"<li><strong><span style=\"color: red; font-size: 10pt\">Personelim değil sorgusunda hata: {msg}</span></strong></li></ul></html>";
                    lblMessage.Text = $"Personelim değil sorgusunda hata: {msg}";
                    return false;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                lblReport.Text += $"<li><strong><span style=\"color: red; font-size: 10pt\">Personelim değil kontrolü yapılamadı! Hata: {msg}</span></strong></li></ul></html>";
                lblMessage.Text = $"Personelim değil kontrolü yapılamadı! Hata: {msg}";
                return false;
            }
            return true;
        }
        #endregion
        private void texTckn_Enter(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
        }

        #region rgvReportList
        private void rgvReportList_RowsChanged(object sender, GridViewCollectionChangedEventArgs e)
        {

        }
        private void rgvReportList_ValueChanged(object sender, EventArgs e)
        {
            string msg = "";
            try
            {
                // Onay kutusu işaretlenir ya da işaret kaldırılırsa
                if (rgvReportList.ActiveEditor is RadCheckBoxEditor && rgvReportList.CurrentCell.RowIndex >= 0)
                {
                    if (exportTypeEnum != ExportTypeEnum.Arv)
                    {
                        string raporTakipNo = this.rgvReportList.CurrentRow.Cells["RaporTakipNo"].Value.ToString();
                        string checkState = rgvReportList.ActiveEditor.Value.ToString();
                        if (checkState == "On")
                        {
                            visitsToBeProcessed.Add(IOC.WinHelpers.GetVisitFromRgv(rgvReportList, out msg));
                        }
                        else if (checkState == "Off")
                        {
                            visitsToBeProcessed.RemoveAll(rtn => rtn.RaporTakipNo == raporTakipNo);
                        }
                    }
                    else
                    {
                        string tcno = rgvReportList.CurrentRow.Cells["TCNO"].Value.ToString();
                        DateTime rbt = DateTime.Parse(rgvReportList.CurrentRow.Cells["RaporBaslamaTarihi"].Value.ToString());
                        DateTime ibkt = DateTime.Parse(rgvReportList.CurrentRow.Cells["IsBasiKontrolTarihi"].Value.ToString());
                        string checkState = rgvReportList.ActiveEditor.Value.ToString();
                        if (checkState == "On")
                        {
                            visitsToBeProcessed.Add(IOC.WinHelpers.GetVisitFromRgv(rgvReportList, out msg));
                        }
                        else if (checkState == "Off")
                        {
                            visitsToBeProcessed.RemoveAll(prs => prs.Tcno == tcno && prs.RaporBaslamaTarihi.Day == rbt.Day && prs.IsBasiKontrolTarihi.Day == ibkt.Day);
                        }
                    }

                    visitsToBeProcessed = visitsToBeProcessed.OrderBy(x => x.Cnm).ToList();
                    btnStartProcess.Enabled = checkDownloadPdf.Enabled = (visitsToBeProcessed.Count > 0) ? true : false;
                }
                // Eğer checkbox onaylandıktan sonra  çalışma durumu değiştirilirse
                else if (rgvReportList.ActiveEditor is RadDropDownListEditor && rgvReportList.CurrentCell.RowIndex >= 0)
                {

                    string ws = rgvReportList.ActiveEditor.Value.ToString();
                    string cd = rgvReportList.CurrentRow.Cells[21].Value.ToString();
                    if (reportType != ReportTypeEnum.ArsivRaporu && rgvReportList.CurrentRow.Cells[0].Value != null && rgvReportList.CurrentRow.Cells[0].Value.ToString() == "True")
                    {
                        foreach (var v in visitsToBeProcessed)
                        {
                            if (v.RaporTakipNo == rgvReportList.CurrentRow.Cells["RaporTakipNo"].Value.ToString())
                            {
                                v.WorkingStatus = (ws == "0") ? 0 : (ws == "1") ? 1 : 2;
                                v.ConfirmDate = cd;
                            }
                        }
                    }
                    else if (reportType == ReportTypeEnum.ArsivRaporu && rgvReportList.CurrentRow.Cells[0].Value != null && rgvReportList.CurrentRow.Cells[0].Value.ToString() == "True")
                    {
                        foreach (var v in visitsToBeProcessed)
                        {
                            string tcno = rgvReportList.CurrentRow.Cells["TCNO"].Value.ToString();
                            DateTime rbt = DateTime.Parse(rgvReportList.CurrentRow.Cells["RaporBaslamaTarihi"].Value.ToString());
                            DateTime ibkt = DateTime.Parse(rgvReportList.CurrentRow.Cells["IsBasiKontrolTarihi"].Value.ToString());
                            if (v.Tcno == tcno && v.RaporBaslamaTarihi.Day == rbt.Day && v.IsBasiKontrolTarihi.Day == ibkt.Day)
                            {
                                v.WorkingStatus = (ws == "0") ? 0 : (ws == "1") ? 1 : 2;
                                v.ConfirmDate = cd;
                            }
                        }
                    }
                }
                // Eğer checkbox onaylandıktan sonra tarih değiştirilirse
                else if (rgvReportList.ActiveEditor is RadDateTimeEditor && rgvReportList.CurrentCell.RowIndex >= 0)
                {
                    string cd = "";
                    string ws = rgvReportList.CurrentRow.Cells[22].Value.ToString();
                    if (rgvReportList.ActiveEditor.Value != null)
                    {
                        cd = rgvReportList.ActiveEditor.Value.ToString();
                    }
                    else
                    {
                        rgvReportList.ActiveEditor.Value = DateTime.Now;
                    }
                    if (reportType != ReportTypeEnum.ArsivRaporu && rgvReportList.CurrentRow.Cells[0].Value != null && rgvReportList.CurrentRow.Cells[0].Value.ToString() == "True")
                    {
                        foreach (var v in visitsToBeProcessed)
                        {
                            if (v.RaporTakipNo == rgvReportList.CurrentRow.Cells["RaporTakipNo"].Value.ToString())
                            {
                                v.WorkingStatus = (ws == "0") ? 0 : (ws == "1") ? 1 : 2;
                                v.ConfirmDate = cd;
                            }
                        }
                    }
                    else if (reportType == ReportTypeEnum.ArsivRaporu && rgvReportList.CurrentRow.Cells[0].Value != null && rgvReportList.CurrentRow.Cells[0].Value.ToString() == "True")
                    {
                        foreach (var v in visitsToBeProcessed)
                        {
                            string tcno = rgvReportList.CurrentRow.Cells["TCNO"].Value.ToString();
                            DateTime rbt = DateTime.Parse(rgvReportList.CurrentRow.Cells["RaporBaslamaTarihi"].Value.ToString());
                            DateTime ibkt = DateTime.Parse(rgvReportList.CurrentRow.Cells["IsBasiKontrolTarihi"].Value.ToString());
                            if (v.Tcno == tcno && v.RaporBaslamaTarihi.Day == rbt.Day && v.IsBasiKontrolTarihi.Day == ibkt.Day)
                            {
                                v.WorkingStatus = (ws == "0") ? 0 : (ws == "1") ? 1 : 2;
                                v.ConfirmDate = cd;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                lblMessage.Text = $"Tablodan veri okunamıyor! Hata: {msg}";
            }
        }
        private void rgvReportList_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            string msg = "";
            if (SearchReport.ReportType != 3 && rgvReportList.CurrentRow.Index >= 0)
            {
                int index = rgvReportList.CurrentRow.Index;
                RadDateTimeEditor editor = rgvReportList.ActiveEditor as RadDateTimeEditor;
                if (editor == null)
                {
                    return;
                }
                editor.MaxValue = DateTime.Now;
                try
                {
                    editor.MinValue = Convert.ToDateTime(rgvReportList.Rows[index].Cells[7].Value, CultureInfo.CurrentCulture);
                }
                catch (Exception ex)
                {
                    msg = ex.Message.ToString();
                    editor.MinValue = DateTime.Now;
                }
            }
        }
        private void rgvReportList_RowFormatting(object sender, RowFormattingEventArgs e)
        {
            if (rgvReportList.DataSource != null && rgvReportList.ColumnCount > 21 && e.RowElement.RowInfo.Cells["CalismaDurumu"].Value != null)
            {
                if (e.RowElement.RowInfo.Cells["CalismaDurumu"].Value.ToString() == "0")
                {
                    e.RowElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                    e.RowElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
                    e.RowElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                    e.RowElement.DrawFill = true;
                    e.RowElement.GradientStyle = GradientStyles.Solid;
                    e.RowElement.BackColor = Properties.Settings.Default.cmmColor; // Color.PaleTurquoise;
                }
                else if (e.RowElement.RowInfo.Cells["CalismaDurumu"].Value.ToString() == "1")
                {
                    e.RowElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                    e.RowElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
                    e.RowElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                    e.RowElement.DrawFill = true;
                    e.RowElement.GradientStyle = GradientStyles.Solid;
                    e.RowElement.BackColor = Properties.Settings.Default.cmColor; // Color.PaleGreen;
                }
                else if (e.RowElement.RowInfo.Cells["CalismaDurumu"].Value.ToString() == "2")
                {
                    e.RowElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                    e.RowElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
                    e.RowElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                    e.RowElement.DrawFill = true;
                    e.RowElement.GradientStyle = GradientStyles.Solid;
                    e.RowElement.BackColor = Properties.Settings.Default.pdColor; // Color.PaleVioletRed;
                }
            }
        }
        private void rgvReportList_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (enableGetDetail != true) return;
            lblViziteReportHeader.Text = $"<html><span style=\"font-size: 12pt\"><strong>RAPOR AYRINTISI ALMA</strong></span></html>";
            rwbVizite.Text = "RAPOR AYRINTISI ALMA İŞLEMİ DEVAM EDİYOR";
            btnCancelVizite.Text = "Rapor ayrıntısı almayı iptal et";
            if (!bgwDetails.IsBusy)
            {
                bgwDetails.RunWorkerAsync();
            }
            ProcessStarted();
        }
        private void rgvReportList_HeaderCellToggleStateChanged(object sender, GridViewHeaderCellEventArgs e)
        {
            string msg = "";
            try
            {
                if (e.State == Telerik.WinControls.Enumerations.ToggleState.On)
                {
                    foreach (GridViewRowInfo rows in rgvReportList.Rows)
                    {
                        rows.Cells[0].Value = true;
                    }
                    visitsToBeProcessed = IOC.WinHelpers.GetAllVisitsFromRgv(rgvReportList, out msg);
                    btnStartProcess.Enabled = true; checkDownloadPdf.Enabled = true;
                }
                else if (e.State == Telerik.WinControls.Enumerations.ToggleState.Off)
                {
                    foreach (GridViewRowInfo rows in rgvReportList.Rows)
                    {
                        rows.Cells[0].Value = false;
                    }
                    btnStartProcess.Enabled = false; checkDownloadPdf.Enabled = false;
                    visitsToBeProcessed.Clear();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                lblMessage.Text = $"Tablodan veri okunamıyor! Hata: {msg}";
            }
        }
        private void rgvReportList_FilterExpressionChanged(object sender, FilterExpressionChangedEventArgs e)
        {
            e.FilterExpression = e.FilterExpression.ToUpper();
        }
        #endregion

        #region görünüm_ayarları
        private void ShowOptionParameters(ReportTypeEnum reportType)
        {
            switch (reportType)
            {
                case ReportTypeEnum.Tcno:
                    rbbgDateLabels.Visibility = ElementVisibility.Collapsed;
                    rbbgDates.Visibility = ElementVisibility.Collapsed;
                    rbbgSearchDate.Visibility = ElementVisibility.Visible;
                    rbbgStartDateAndTckn.Visibility = ElementVisibility.Visible;
                    texTckn.Visibility = ElementVisibility.Visible; texTckn.Enabled = true;
                    dtpSearchDate.Visible = false; dtpSearchDate.Enabled = true;
                    //dtpStartDate.Enabled = false; dtpStartDate.Visible = false;
                    //dtpEndDate.Enabled = false; dtpEndDate.Visible = false;
                    ddlVaka.Enabled = true; ddlVaka.Visibility = ElementVisibility.Visible;
                    rbgTaramaKriterleri.Text = "Vaka Türü Seçimi ve TCKN Girişi";
                    break;
                case ReportTypeEnum.RaporTarihi:
                    rbbgDateLabels.Visibility = ElementVisibility.Collapsed;
                    rbbgDates.Visibility = ElementVisibility.Collapsed;
                    rbbgSearchDate.Visibility = ElementVisibility.Visible;
                    rbbgStartDateAndTckn.Visibility = ElementVisibility.Visible;
                    texTckn.Visibility = ElementVisibility.Collapsed; texTckn.Enabled = true;
                    dtpSearchDate.Visible = true; dtpSearchDate.Enabled = true;
                    //dtpStartDate.Enabled = false; dtpStartDate.Visible = false;
                    //dtpEndDate.Enabled = false; dtpEndDate.Visible = false;
                    ddlVaka.Enabled = true; ddlVaka.Visibility = ElementVisibility.Visible;
                    rbgTaramaKriterleri.Text = "Vaka Türü ve Tarih Seçimi";

                    break;
                case ReportTypeEnum.OnayliRapor:
                case ReportTypeEnum.ArsivRaporu:
                    rbbgDateLabels.Visibility = ElementVisibility.Visible;
                    rbbgDates.Visibility = ElementVisibility.Visible;
                    rbbgSearchDate.Visibility = ElementVisibility.Collapsed;
                    rbbgStartDateAndTckn.Visibility = ElementVisibility.Collapsed;
                    texTckn.Enabled = false; texTckn.Visibility = ElementVisibility.Collapsed;
                    dtpSearchDate.Enabled = false; dtpSearchDate.Visible = false; ;
                    //dtpStartDate.Enabled = true; dtpStartDate.Visible = true;
                    //dtpEndDate.Enabled = true; dtpEndDate.Visible = true;
                    ddlVaka.Enabled = false; ddlVaka.Visibility = ElementVisibility.Collapsed;
                    rbgTaramaKriterleri.Text = "Başlangıç-Bitiş Tarihleri Seçimi";

                    break;
                default:
                    break;
            }
        }
        private void SetColumns()
        {
            if (rgvReportList.Rows.Count > 0)
            {
                GridViewDataColumn columnToMove;
                int[] columnsToHide = new int[] { };
                for (int i = 1; i < rgvReportList.ColumnCount; i++)
                {
                    rgvReportList.Columns[i].ReadOnly = true;

                }
                columnToMove = rgvReportList.Columns[16]; // TCKN sütunu
                rgvReportList.Columns.RemoveAt(16);
                rgvReportList.Columns.Insert(1, columnToMove);

                rgvReportList.Columns[0].DataType = typeof(Boolean);
                rgvReportList.Columns[0].AllowFiltering = false;
                rgvReportList.Columns[0].Width = 44;  rgvReportList.Columns[0].MaxWidth = 44;
                rgvReportList.Columns[1].HeaderText = "TC Kimlik No";
                rgvReportList.Columns[1].Width = 80;  rgvReportList.Columns[1].MaxWidth = 120;
                rgvReportList.Columns[2].HeaderText = "Ad Soyad";
                rgvReportList.Columns[2].MinWidth = 150;
                rgvReportList.Columns[3].HeaderText = "Vaka";
                rgvReportList.Columns[3].Width = 60;  rgvReportList.Columns[3].MaxWidth = 80;
                rgvReportList.Columns[4].HeaderText = "Takip No";
                rgvReportList.Columns[4].Width = 120;  rgvReportList.Columns[4].MaxWidth = 190;
                rgvReportList.Columns[5].HeaderText = "Sıra No";
                rgvReportList.Columns[5].Width = 50;  rgvReportList.Columns[5].MaxWidth = 50;
                rgvReportList.Columns[6].HeaderText = "Başlama Tarihi";
                rgvReportList.Columns[6].DataType = typeof(DateTime);
                rgvReportList.Columns[6].Width = 90;  rgvReportList.Columns[6].MaxWidth = 120;
                rgvReportList.Columns[6].FormatString = "{0: dd.MM.yyyy}";
                rgvReportList.Columns[7].HeaderText = "Bitiş Tarihi";
                rgvReportList.Columns[7].Width = 90;  rgvReportList.Columns[7].MaxWidth = 120;
                rgvReportList.Columns[7].DataType = typeof(DateTime);
                rgvReportList.Columns[7].FormatString = "{0: dd.MM.yyyy}";
                rgvReportList.Columns[8].HeaderText = "İşbaşı Tarihi";
                rgvReportList.Columns[8].Width = 90;  rgvReportList.Columns[8].MaxWidth = 120;
                rgvReportList.Columns[8].DataType = typeof(DateTime);
                rgvReportList.Columns[8].FormatString = "{0: dd.MM.yyyy}";
                rgvReportList.Columns[9].HeaderText = "Ceza Durumu";
                rgvReportList.Columns[10].HeaderText = "Açıklama";
                rgvReportList.Columns[11].HeaderText = "Poliklinik Tarihi";
                rgvReportList.Columns[11].DataType = typeof(DateTime);
                rgvReportList.Columns[11].FormatString = "{0: dd.MM.yyyy}";
                rgvReportList.Columns[13].HeaderText = "Firma Adı";
                rgvReportList.Columns[13].MinWidth = 200;
                switch (SearchReport.ReportType)
                {
                    case 1:
                    case 2:
                        columnsToHide = new int[] { 12, 13, 14, 15, 16, 17, 18, 19, 20 };
                        rgvReportList.Columns[21].ReadOnly = false;
                        rgvReportList.Columns[21].Width = 90;  rgvReportList.Columns[21].MaxWidth = 120;
                        rgvReportList.Columns[22].ReadOnly = false;
                        rgvReportList.Columns[22].Width = 120;  rgvReportList.Columns[22].MaxWidth = 160;
                        break;
                    case 3:
                        columnToMove = rgvReportList.Columns[11];
                        rgvReportList.Columns.RemoveAt(11);
                        rgvReportList.Columns.Insert(8, columnToMove);
                        columnsToHide = new int[] { 7, 8, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
                        break;
                    case 4:
                        columnsToHide = new int[] { 4, 5, 6, 8, 10, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
                        rgvReportList.Columns[21].ReadOnly = false;
                        rgvReportList.Columns[21].Width = 90;  rgvReportList.Columns[21].MaxWidth = 120;
                        rgvReportList.Columns[22].ReadOnly = false;
                        rgvReportList.Columns[22].Width = 120;  rgvReportList.Columns[22].MaxWidth = 160;
                        break;
                }
                columnToMove = rgvReportList.Columns[13]; 
                rgvReportList.Columns.RemoveAt(13);
                rgvReportList.Columns.Insert(1, columnToMove);
                foreach (int i in columnsToHide)
                {
                    rgvReportList.Columns[i].IsVisible = false;
                }
            }
        }
        public void DatePickerSet()
        {
            string msg = "";
            try
            {
                foreach (GridViewRowInfo rowInfo in rgvReportList.Rows)
                {
                    DateTime tarih = new DateTime();
                    foreach (GridViewCellInfo cellInfo in rowInfo.Cells)
                    {
                        if (cellInfo.ColumnInfo.Name == "RaporBitisTarihi" && reportType != ReportTypeEnum.ArsivRaporu)
                        {
                            tarih = Convert.ToDateTime(cellInfo.Value, CultureInfo.CurrentCulture);
                        }
                        else if (cellInfo.ColumnInfo.Name == "IsBasiKontrolTarihi" && reportType == ReportTypeEnum.ArsivRaporu)
                        {
                            tarih = Convert.ToDateTime(cellInfo.Value, CultureInfo.CurrentCulture);
                            tarih = tarih.AddDays(-1);
                        }
                    }
                    foreach (GridViewCellInfo cellInfo in rowInfo.Cells)
                    {
                        if (cellInfo.ColumnInfo.Name == "TarihSec")
                        {
                            if (tarih > DateTime.Now.Date)
                            {
                                cellInfo.Value = DateTime.Now;
                            }
                            else
                            {
                                cellInfo.Value = tarih;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                lblMessage.Text = $"Tarih seç sütunu ayarlanamıyor! Hata: {msg}";
            }

        }
       
        #endregion

        #endregion

        #region links
        private void InitializeBgwLinks()
        {
            bgwLinks.WorkerSupportsCancellation = true;
            bgwLinks.DoWork += new DoWorkEventHandler(BgwlinksDoWork);
            bgwLinks.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BgwBgwlinksComplated);
            bgwLinks.ProgressChanged += new ProgressChangedEventHandler(BgwlinksChanged);
        }
        private void BgwlinksChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void BgwBgwlinksComplated(object sender, RunWorkerCompletedEventArgs e)
        {
            
            if (modulType == ModulTypeEnum.Tesvik)
            {
                if (e.Error != null)
                {
                    lblMessage.Text = $"Teşvik sorgulama sırasında hata oluştu! {e.Error.Message}";
                    btnSoyadUpdateReport.Visibility = ElementVisibility.Collapsed;
                }
                else if (e.Cancelled)
                {
                    lblMessage.Text = $"Teşvik sorgulama işlemi iptal edildi!";
                    btnSoyadUpdateReport.Visibility = ElementVisibility.Collapsed;
                }
                else
                {
                    GlobalVars.LstInc = (from x in GlobalVars.LstInc where x.Tk != String.Empty select x).ToList();
                    lblMessage.Text = lblMessage.Text.Contains("Lütfen önce en az bir firma seçiniz")? lblMessage.Text : GlobalVars.LstInc.Count > 0 ? $"Girilen kimlik numaralarından {(from x in GlobalVars.LstInc select x.Tcno).Distinct().Count()} adedi için teşvik (5510 hariç) bulunmuştur" : $"Girilen kimlik numaralarına ait teşvik(5510 hariç) bulunamamıştır.";
                }
                ProcessFinishedSoyad();
            }
            else
            {
                if (e.Error != null)
                {
                    lblMessage.Text = $"Linkin açılması sırasında hata oluştu! {e.Error.Message}";
                }
                else if (e.Cancelled)
                {
                    lblMessage.Text = $"Link açma işlemi iptal edildi!";
                    ProcessFinishedLinks();
                }
            }
            ProcessFinishedLinks();
        }
        private void BgwlinksDoWork(object sender, DoWorkEventArgs e)
        {
            tempHideBrowser = Settings.Default.hideBrowser;
            Settings.Default.hideBrowser = false; Settings.Default.Save();
            IOC.TrmBase.tryCount = 1; lblMessage.Text = ""; 
            GlobalVars.CancelProcess = false; 
            GlobalVars.ProcessReport = "<html><ul>";
            string msg = "";
            if (modulType == ModulTypeEnum.Links && rgvCompanyList.SelectedRows.Count > 5)
            {
                lblMessage.Text = "Aynı anda en fazla 5 firma için link açabilirsiniz.";
                List<GridViewRowInfo> selectedRows = new List<GridViewRowInfo>();
                int counter = 1;
                foreach (var row in rgvCompanyList.SelectedRows)
                {
                    if (counter > 5)
                    {
                        break;
                    }
                    selectedRows.Add(row);
                    counter++;
                }
                rgvCompanyList.ClearSelection();
                foreach (var item in selectedRows)
                {
                    item.IsSelected = true;
                }
            }
            
            if (modulType == ModulTypeEnum.Links)
            {
                bool isRgvLinkListOk = (rgvCompanyList.SelectedRows.Count > 0 && rgvLinkList.CurrentRow.Index >= 0) ? true : false;
                if (!isRgvLinkListOk) { lblMessage.Text = "En az bir firma seçmeniz gerekli!"; return; }
                //IOC.SgkLinksService.Command = rgvLinkList.CurrentRow.Cells[9].Value.ToString();
                //IOC.SgkLinksService.Url = rgvLinkList.CurrentRow.Cells[5].Value.ToString();

                IOC.LinkOps.CmdUrlVurl = LinkGlobals.LstLinks.Where(x => x.Id == Convert.ToInt32(rgvLinkList.CurrentRow.Cells[0].Value)).Select(x => new CmdUrlVurl { Cmd = x.Cmd, Url = x.Url, Vurl = x.Vurl }).FirstOrDefault();
            }
            else if (modulType == ModulTypeEnum.Tesvik)
            {
                int counter = 1;
                while (rgvLinkList.DataSource == null)
                {
                    IOC.WinHelpers.FillRgvWithLinks(rgvLinkList);
                    if (rgvLinkList.DataSource != null) { break; }
                    counter++;
                    if(counter == 3) { lblMessage.Text = "Veri tabanı hatası, lütfen daha sonra tekrar deneyin."; return;  }
                } 
                //foreach (GridViewRowInfo row in rgvLinkList.Rows)
                //{
                //    if( row.Cells[4].Value.ToString() == "POTANSİYEL TEŞVİK SORGULAMA")
                //    {
                //        IOC.SgkLinksService.Command = row.Cells[9].Value.ToString();
                //        IOC.SgkLinksService.Url = row.Cells[5].Value.ToString();
                //        break;
                //    }
                //}
                IOC.LinkOps.CmdUrlVurl = LinkGlobals.LstLinks.Where(x => x.Id == 22).Select(x => new CmdUrlVurl { Cmd = x.Cmd, Url = x.Url, Vurl = x.Vurl }).FirstOrDefault();
            }
            IOC.SgkLinksService.Command = IOC.LinkOps.CmdUrlVurl.Cmd;
            IOC.SgkLinksService.Url = IOC.LinkOps.CmdUrlVurl.Url;
            IOC.SgkLinksService.Vurl = IOC.LinkOps.CmdUrlVurl.Vurl;
            List<Company> lst = IOC.WinHelpers.GetSelectedCompaniesFromRgv(rgvCompanyList, out msg);
            if (lst == null || lst.Count == 0) { lblMessage.Text = "Lütfen önce en az bir firma seçiniz"; return; }
            if (modulType == ModulTypeEnum.Tesvik)
            {
                foreach (Company item in lst)
                {
                    if (msg.Contains("iptal")) { e.Cancel = true; return; }
                    List<Company> companies = new List<Company>() { item };
                    IOC.LinkOps.StartProcessForLinks(companies, ref acError, out msg, ref lblReportSoyad);
                    if (!msg.Contains("Hata"))
                    {
                        List<string> tckns = new List<string>();
                        foreach (GridViewRowInfo row in rgvLastName.Rows)
                        {
                            tckns.Add(row.Cells[0].Value.ToString());
                        }
                        
                        IOC.IncentiveOps.GetIncentive(tckns, item.CompanyName, ref lblReportSoyad, out msg);
                        if (msg.Contains("iptal")) { e.Cancel = true; return; }
                        else if (msg.Contains("Hata")) { lblMessage.Text = msg; }
                    }
                    else
                    {
                        lblReportSoyad.Text += $"{item.CompanyName} için potansiyel teşvik sorgulaması yapılamadı {msg}";
                        continue;
                    }
                    
                }
            }
                
            else 
                IOC.LinkOps.StartProcessForLinks(lst, ref acError, out msg, ref lblReportLink); 
            if (msg.Contains("iptal")){   e.Cancel = true; return;    }
            
            lblMessage.Text = msg;
        }
        private void ProcessStartedLinks()
        {
            LinkGlobals.IsLink = true;
            modulType = ModulTypeEnum.Links;
            IOC.SgkLinksService.LoginBtnClicked = false;
            ribbonBar.Enabled = false;
            rgvCompanyList.Enabled = false;
            pnlWaitLinks.Visible = true;
            rwbLinks.StartWaiting();
            btnCancelLink.Enabled = true;
            btnCancelLink.Text = "Link açmayı iptal et";
            lblReportLink.Text = "";
        }
        private void ProcessFinishedLinks()
        {
            LinkGlobals.IsLink = false;
            ribbonBar.Enabled = true;
            rgvCompanyList.Enabled = true;
            pnlWaitLinks.Visible = false;
            rwbLinks.StopWaiting();
            btnCancelLink.Enabled = false;
            lblReportLink.Text = "";
            GlobalVars.CancelProcess = false;
            LinkGlobals.LinkCancel = false;
            Settings.Default.hideBrowser = tempHideBrowser; Settings.Default.Save();
            IOC.LinkOps.DisposeProcess();
        }
        #region controls
        private void btnVisitLink_Click(object sender, EventArgs e)
        {
            ProcessStartedLinks();
            if (!bgwLinks.IsBusy)
            {
                bgwLinks.RunWorkerAsync();
            }
        }
        private void btnCancelLink_Click(object sender, EventArgs e)
        {
            GlobalVars.CancelProcess = true; LinkGlobals.LinkCancel = true;
            lblReportLink.Text += "<html><strong><span style=\"font-size: 10pt\">İPTAL TALEBİ ALINDI!</span></strong></html>\r\n";
            btnCancelLink.Text = "İşlem iptal ediliyor...";
            btnCancelLink.Enabled = false;
            if (bgwLinks.IsBusy)
            {
                bgwLinks.CancelAsync();
            }
            ProcessFinishedLinks();
        }
        private void btnRefreshLinks_Click(object sender, EventArgs e)
        {
            string msg = "";
            IOC.WinHelpers.FillRgvWithLinks(rgvLinkList);
            IOC.LinkOps.FavColumnSet(rgvLinkList, out msg);
            rgvLinkList.Refresh();
            lblMessage.Text = msg;
        }
        private void btnOrder_Click(object sender, EventArgs e)
        {

            rgvLinkList.MasterTemplate.SortDescriptors.Clear();
            if (sender == btnSortMainGroup)
            {
                if (grpSort == GrpSortEnum.None || grpSort == GrpSortEnum.Desc)
                {
                    rgvLinkList.MasterTemplate.SortDescriptors.Add("grp", ListSortDirection.Ascending);
                    grpSort = GrpSortEnum.Asc;
                    btnSortMainGroup.Image = Properties.Resources.sort_down_64;
                }
                else if (grpSort == GrpSortEnum.Asc)
                {
                    rgvLinkList.MasterTemplate.SortDescriptors.Add("grp", ListSortDirection.Descending);
                    grpSort = GrpSortEnum.Desc; btnSortMainGroup.Image = Properties.Resources.sort_up_64;
                }
                sgrpSort = SgrpSortEnum.None; adSort = AdSortEnum.None; favSort = FavSortEnum.None;
                btnSortName.Image = Properties.Resources.sort_64;               btnSortFav.Image = Properties.Resources.sort_64;                btnSortSubGroup.Image = Properties.Resources.sort_64;
            }
            else if (sender == btnSortSubGroup)
            {
                if (sgrpSort == SgrpSortEnum.None || sgrpSort == SgrpSortEnum.Desc)
                {
                    rgvLinkList.MasterTemplate.SortDescriptors.Add("sgrp", ListSortDirection.Ascending);
                    sgrpSort = SgrpSortEnum.Asc;
                    btnSortSubGroup.Image = Properties.Resources.sort_down_64;
                }
                else if (sgrpSort == SgrpSortEnum.Asc)
                {
                    rgvLinkList.MasterTemplate.SortDescriptors.Add("sgrp", ListSortDirection.Descending);
                    sgrpSort = SgrpSortEnum.Desc;
                    btnSortSubGroup.Image = Properties.Resources.sort_up_64;
                }
                grpSort = GrpSortEnum.None; adSort = AdSortEnum.None; favSort = FavSortEnum.None;
                btnSortName.Image = Properties.Resources.sort_64;                btnSortFav.Image = Properties.Resources.sort_64;                btnSortMainGroup.Image = Properties.Resources.sort_64;
            }
            else if (sender == btnSortName)
            {
                if (adSort == AdSortEnum.None || adSort == AdSortEnum.Desc)
                {
                    rgvLinkList.MasterTemplate.SortDescriptors.Add("ad", ListSortDirection.Ascending);
                    adSort = AdSortEnum.Asc;
                    btnSortName.Image = Properties.Resources.sort_down_64;
                }
                else if (adSort == AdSortEnum.Asc)
                {
                    rgvLinkList.MasterTemplate.SortDescriptors.Add("ad", ListSortDirection.Descending);
                    adSort = AdSortEnum.Desc;
                    btnSortName.Image = Properties.Resources.sort_up_64;
                }
                grpSort = GrpSortEnum.None; sgrpSort = SgrpSortEnum.None; favSort = FavSortEnum.None;
                btnSortFav.Image = Properties.Resources.sort_64;                btnSortMainGroup.Image = Properties.Resources.sort_64;                btnSortSubGroup.Image = Properties.Resources.sort_64;
            }
            else if (sender == btnSortFav)
            {
                if (favSort == FavSortEnum.None || favSort == FavSortEnum.Desc)
                {
                    rgvLinkList.MasterTemplate.SortDescriptors.Add("AddRemoveFavs", ListSortDirection.Ascending);
                    favSort = FavSortEnum.Asc;
                    btnSortFav.Image = Properties.Resources.sort_down_64;
                }
                else if (favSort == FavSortEnum.Asc)
                {
                    rgvLinkList.MasterTemplate.SortDescriptors.Add("AddRemoveFavs", ListSortDirection.Descending);
                    favSort = FavSortEnum.Desc;
                    btnSortFav.Image = Properties.Resources.sort_up_64;
                }
                grpSort = GrpSortEnum.None; sgrpSort = SgrpSortEnum.None; adSort = AdSortEnum.None;
                btnSortName.Image = Properties.Resources.sort_64;           btnSortMainGroup.Image = Properties.Resources.sort_64;                btnSortSubGroup.Image = Properties.Resources.sort_64;
            }
        }
        #endregion

        #region rgvLinkList_events
        private void rgvLinkList_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            ProcessStartedLinks();
            if (!bgwLinks.IsBusy)
            {
                bgwLinks.RunWorkerAsync();
            }
        }
        private void rgvLinkList_RowFormatting(object sender, RowFormattingEventArgs e)
        {
            string msg = "";
            try
            {
                if (e.RowElement.RowInfo.Index % 2 == 1)
                {
                    e.RowElement.DrawFill = true;
                    e.RowElement.GradientStyle = GradientStyles.Solid;
                    e.RowElement.BackColor = Color.Bisque;
                }
                else if (e.RowElement.RowInfo.Index % 2 == 0)
                {
                    e.RowElement.DrawFill = true;
                    e.RowElement.GradientStyle = GradientStyles.Solid;
                    e.RowElement.BackColor = Color.AliceBlue;
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                lblMessage.Text = $"Satır renklendirme işlemi başarısız! Hata: {msg}";
            }
        }
        private void rgvLinkList_FilterExpressionChanged(object sender, FilterExpressionChangedEventArgs e)
        {
            e.FilterExpression = e.FilterExpression.ToUpper();
        }
        private void rgvLinkList_ValueChanged(object sender, EventArgs e)
        {
            int rowIndex = -1; int linkId = -1;
            string msg;
            try
            {
                if (rgvLinkList.SelectedRows.Count > 0 && rgvLinkList.CurrentCell != null)
                {

                    rowIndex = rgvLinkList.CurrentCell.RowIndex;
                    linkId = Convert.ToInt32(rgvLinkList.CurrentRow.Cells[0].Value);
                }
                if (rowIndex >= 0 && linkId >= 0 && rgvLinkList.CurrentColumn.Name == "AddRemoveFavs")
                {
                    bool x = Convert.ToBoolean(rgvLinkList.CurrentCell.Value);
                    if (x)
                    {
                        if (IOC.LinksDataService.AddRemoveFav(linkId, true, out msg))
                            lblMessage.Text = rgvLinkList.CurrentRow.Cells[4].Value.ToString() + ", FAVORİLERE EKLENDİ.";
                        rgvLinkList.Refresh();
                    }
                    else if (!x)
                    {
                        if (IOC.LinksDataService.AddRemoveFav(linkId, false, out msg))
                            lblMessage.Text = rgvLinkList.CurrentRow.Cells[4].Value.ToString() + ", FAVORİLERDEN ÇIKARILDI.";
                        rgvLinkList.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                lblMessage.Text = $"Favoriler sütunu ayarlanamıyor! Hata: {msg}";
            }
        }
        private void rgvLinkList_CellMouseMove(object sender, MouseEventArgs e)
        {
            
            RadCheckmark elementUnderMouse = this.rgvLinkList.ElementTree.GetElementAtPoint(e.Location) as RadCheckmark;
            if (elementUnderMouse != null)
            {
                this.rgvLinkList.Cursor = Cursors.Hand;
            }
            else
            {
                this.rgvLinkList.Cursor = Cursors.Default;
            }
            
        }
        private void rgvLinkList_MouseEnter(object sender, EventArgs e)
        {
            if (LinkGlobals.HasUpdated)
            {
                string msg = "";
                IOC.WinHelpers.FillRgvWithLinks(rgvLinkList);
                IOC.LinkOps.FavColumnSet(rgvLinkList, out msg);
                rgvLinkList.Refresh();
                LinkGlobals.HasUpdated = false;
            }
        }
        #endregion

        #endregion

        #region soyad
        private void InitializeBgwSoyad()
        {
            bgwSoyad.WorkerSupportsCancellation = true;
            bgwSoyad.DoWork += new DoWorkEventHandler(BgwSoyadDoWork);
            bgwSoyad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BgwSoyadComplated);
            bgwSoyad.ProgressChanged += new ProgressChangedEventHandler(BgwSoyadChanged);
        }
        private void BgwSoyadDoWork(object sender, DoWorkEventArgs e)
        {
            string msg;
            IOC.TrmBase.tryCount = 1;
            GlobalVars.CancelProcess = false;
            report = string.Empty;
            IOC.SgkLinksService.CaptchaCode = null;
            listeLastNames.Clear();
            acError = false;

            // 1. UI İlk Hazırlık İşlemleri (Invoke ile Ana Thread üzerinde çalıştırılıyor)
            Invoke((Action)(() =>
            {
                btnSoyadUpdateReport.Visibility = ElementVisibility.Collapsed;
                btnSoyadUpdateReport.Enabled = true;
                GlobalVars.ProcessReport += $"<html><ul>";
                lblReport.Text = GlobalVars.ProcessReport;
            }));

            if (Surucu.Driver == null || !IOC.LinkOps.IsBrowserOpen())
                Surucu.Driver = IOC.WinHelpers.GetWebDriver(Settings.Default.hideBrowser, out msg);

            IOC.LinkOps.CmdUrlVurl = LinkGlobals.LstLinks.Where(x => x.Id == 53).Select(x => new CmdUrlVurl { Cmd = x.Cmd, Url = x.Url, Vurl = x.Vurl }).FirstOrDefault();
            IOC.SgkLinksService.Command = IOC.LinkOps.CmdUrlVurl?.Cmd;
            IOC.SgkLinksService.Url = IOC.LinkOps.CmdUrlVurl?.Url;
            IOC.SgkLinksService.Vurl = IOC.LinkOps.CmdUrlVurl?.Vurl;
            IOC.SgkLinksService.SetSgkLoginCredentials(login);
            IOC.SgkLinksService.Command = IOC.LinkOps.CmdUrlVurl?.Cmd;

            try
            {
                WebDriverWait wait = IOC.SgkLinksService.GetWait();

                // 2. UI Kontrolünden Seçili Şirketi Güvenle Alıyoruz
                int currentCompanyId = 0;
                Invoke((Action)(() =>
                {
                    if (rgvCompanyList.CurrentRow != null && rgvCompanyList.CurrentRow.Cells[0].Value != null)
                    {
                        currentCompanyId = Convert.ToInt32(rgvCompanyList.CurrentRow.Cells[0].Value);
                    }
                }));

                login = GlobalVars.Companies.Where(x => x.Id == currentCompanyId).FirstOrDefault();
                if (login == null) return;

                IOC.SgkLinksService.SetSgkLoginCredentials(login);
                IOC.SgkLinksService.CompanyName = login.CompanyName;

                IOC.LinkOps.VisitLink(login, IOC.SgkLinksService.Url, ref acError, out msg, ref lblReportSoyad);

                if (msg.Contains("iptal")) { e.Cancel = true; return; }
                else if (msg == "continue" || msg.Contains("Hata"))
                {
                    Invoke((Action)(() =>
                    {
                        GlobalVars.ProcessReport += $"<li><strong><span style=\"color: red; font-size: 10pt\">{login.CompanyName} için oturum açılamadı! {IOC.SgkLinksService.Message}</span></strong></li></ul></html><ul>";
                        lblReport.Text = GlobalVars.ProcessReport;
                        lblMessage.Text = $"{IOC.SgkLinksService.CompanyName} için oturum açılamadı! {IOC.SgkLinksService.Message}";
                    }));
                    e.Cancel = true; return;
                }
                else if (msg == "continueGoAhead")
                {
                    Invoke((Action)(() =>
                    {
                        GlobalVars.ProcessReport += $"<li><strong><span style=\"color: red; font-size: 10pt\">Oturum açıldıktan sonra devam edilemiyor. Hata: {IOC.SgkLinksService.Message}</span></strong></li></ul></html><ul>";
                        lblReport.Text = GlobalVars.ProcessReport;
                        lblMessage.Text = $"Oturum açıldıktan sonra devam edilemiyor. Hata: {IOC.SgkLinksService.Message}";
                    }));
                    e.Cancel = true; return;
                }

                if (bgwSoyad.CancellationPending) { e.Cancel = true; return; }

                // 3. UI Grid'deki TCKN verilerini güvenli bir listeye kopyalıyoruz (Cross-thread engelleme)
                List<string> tcknList = new List<string>();
                Invoke((Action)(() =>
                {
                    foreach (GridViewRowInfo row in rgvLastName.Rows)
                    {
                        if (row.Cells[0].Value != null)
                        {
                            tcknList.Add(row.Cells[0].Value.ToString().Trim());
                        }
                    }
                    lblReport.Text += "<li><strong><span style=\"font-size: 10pt\">Soyad güncelleme başlatılıyor</li>";
                }));

                // Belleğe aldığımız liste miktarı kadar lastnames şablonu oluşturuluyor
                for (int idx = 0; idx < tcknList.Count; idx++)
                {
                    listeLastNames.Add(new LastNames() { Tcno = "x", Asf = "y", Asl = "z" });
                }

                if (msg.Contains("Oturum başlatıldı"))
                {
                    // --- BÖLÜM 1: Ad-Soyad Sorgulama Döngüsü ---
                    for (int i = 0; i < tcknList.Count; i++)
                    {
                        if (bgwSoyad.CancellationPending) { e.Cancel = true; return; }

                        string currentTc = tcknList[i];
                        DetailsNameTc.Tcno = currentTc;

                        Invoke((Action)(() =>
                        {
                            lblReportSoyad.Text += $"<li><strong><span style=\"font-size: 10pt\">{currentTc} için İşe Giriş Bildirgesi'nden Ad-Soyad sorgulanıyor</span></strong></li>";
                        }));

                        soyadError = IOC.SgkLinksService.FillTextBox("x", "/html/body/table[3]/tbody/tr/td/table/tbody/tr[2]/td[2]/table[3]/tbody/tr/td/table/tbody/tr/td/form/table/tbody/tr/td/table/tbody/tr[6]/td/table/tbody/tr/td[4]/nobr/input[1]", "DetailsNameTC.tcno", out msg);

                        if (!soyadError)
                        {
                            Invoke((Action)(() =>
                            {
                                lblReportSoyad.Text += $"<li><strong><span style=\"color: red; font-size: 10pt\">{currentTc} için sorgulama yapılamadı. Kimlik no girilecek metin kutusuna erişilemedi.</span></strong></li>";
                            }));
                            return;
                        }

                        IOC.SgkLinksService.Command = "clk t:x tv:/html/body/table[3]/tbody/tr/td/table/tbody/tr[2]/td[2]/table[3]/tbody/tr/td/table/tbody/tr/td/form/table/tbody/tr/td/table/tbody/tr[6]/td/table/tbody/tr/td[4]/nobr/input[2]\n\rlne\r\nlck Kimlik Numarası :";

                        if (IOC.SgkLinksService.GoAhead(out msg))
                        {
                            if (bgwSoyad.CancellationPending) { e.Cancel = true; return; }

                            // Selenium 4 Standartlarına Uygun Bekleme (ExpectedConditions bağımlılığı olmadan)
                            IWebElement tcNo = wait.Until(d => d.FindElement(By.XPath("/html/body/table[3]/tbody/tr/td/table/tbody/tr[2]/td[2]/table[3]/tbody/tr/td/table/tbody/tr/td/form/table/tbody/tr/td/table/tbody/tr[8]/td")));
                            IWebElement adSoyad = wait.Until(d => d.FindElement(By.XPath("/html/body/table[3]/tbody/tr/td/table/tbody/tr[2]/td[2]/table[3]/tbody/tr/td/table/tbody/tr/td/form/table/tbody/tr/td/table/tbody/tr[9]/td")));

                            Thread.Sleep(500);
                            if (tcNo.Text != null && tcNo.Text.Contains("Kimlik Numarası :"))
                            {
                                listeLastNames[i].Tcno = currentTc;
                                listeLastNames[i].Asf = adSoyad.Text.Trim();

                                Invoke((Action)(() =>
                                {
                                    lblReportSoyad.Text += $"<li><strong><span style=\"font-size: 10pt\">{currentTc} : {adSoyad.Text.Trim()} eşleşmesi tespit edildi</span></strong></li>";
                                }));
                            }
                        }
                        else
                        {
                            hasErrorSoyad = true;
                            listeLastNames[i].Tcno = currentTc;
                            if (msg.Contains("Geçersiz"))
                            {
                                listeLastNames[i].Asf = "Geçersiz kimlik numarası";
                                Invoke((Action)(() =>
                                {
                                    lblReportSoyad.Text += $"<li><strong><span style=\"color: red; font-size: 10pt\">{currentTc},için 'Geçersiz Kimlik Numarası' hatası alındı</span></strong></li>";
                                }));
                            }
                            else if (msg == "HATA BİLDİRİM")
                            {
                                Invoke((Action)(() =>
                                {
                                    lblReportSoyad.Text += $"<li><strong><span style=\"color: red; font-size: 10pt\">{currentTc},için 'Geçersiz Kimlik Numarası' hatası alındı</span></strong></li>";
                                }));
                                Surucu.Driver.Url = "https://uyg.sgk.gov.tr/SigortaliTescil/jsp/anamenu.jsp";
                            }
                        }
                    }
                    if (bgwSoyad.CancellationPending) { e.Cancel = true; return; }
                }

                Invoke((Action)(() =>
                {
                    lblReportSoyad.Text += $"<li><strong><span style=\"font-size: 10pt\">Soyadı Güncelleme sayfasına geçiliyor</span></strong></li>";
                }));

                IOC.SgkLinksService.Command = "clk t:t tv:Ana Menü\n\rlck yeri Sicil No\n\rclk t:x tv:/html/body/center/table[3]/tbody/tr/td/table/tbody/tr[2]/td[2]/table[2]/tbody/tr[22]/td/font/a\n\rlck m Sistemi verilerine g";

                if (IOC.SgkLinksService.GoAhead(out msg))
                {
                    // --- BÖLÜM 2: Güncelleme İşlemi Döngüsü ---
                    for (int j = 0; j < tcknList.Count; j++)
                    {
                        if (bgwSoyad.CancellationPending) { e.Cancel = true; return; }

                        string currentTc = tcknList[j];
                        DetailsNameTc.Tcno = currentTc;

                        Invoke((Action)(() =>
                        {
                            lblReportSoyad.Text += $"<li><strong><span style=\"font-size: 10pt\">{currentTc} için Soyadı Güncellemesi yapılıyor</span></strong></li>";
                        }));

                        IOC.SgkLinksService.FillTextBox("x", "/html/body/table[3]/tbody/tr/td/center/table/tbody/tr[2]/td[2]/form/span/table/tbody/tr[3]/td[1]/input", "DetailsNameTC.tcno", out msg);
                        IOC.SgkLinksService.Command = "clk t:x tv:/html/body/table[3]/tbody/tr/td/center/table/tbody/tr[2]/td[2]/form/span/table/tbody/tr[3]/td[2]/input\n\rlne\r\nlck Sigortalının kimlik bilgileri";

                        if (IOC.SgkLinksService.GoAhead(out msg))
                        {
                            Thread.Sleep(300);
                        }
                        else
                        {
                            Invoke((Action)(() =>
                            {
                                if (msg.Contains("Geçersiz"))
                                {
                                    lblReportSoyad.Text += $"<li><strong><span style=\"color: red; font-size: 10pt\">{currentTc},için 'Geçersiz Kimlik Numarası' hatası alındı</span></strong></li>";
                                }
                                else if (msg == "HATA BİLDİRİM")
                                {
                                    lblReportSoyad.Text += $"<li><strong><span style=\"color: red; font-size: 10pt\">{currentTc},için 'Geçersiz Kimlik Numarası' hatası alındı</span></strong></li>";
                                    Surucu.Driver.Url = "https://uyg.sgk.gov.tr/SigortaliTescil/jsp/nufus.jsp";
                                }
                                else if (msg.Contains("4a kaydı"))
                                {
                                    lblReportSoyad.Text += $"<li><strong><span style=\"color: red; font-size: 10pt\">{currentTc},için 'Sigortalının 4a kaydı bulunamamıştır' hatası alındı</span></strong></li>";
                                }
                            }));
                        }
                    }
                    if (bgwSoyad.CancellationPending) { e.Cancel = true; return; }
                }

                Invoke((Action)(() =>
                {
                    lblReportSoyad.Text += $"<li><strong><span style=\"font-size: 10pt\">Teyit için tekrar İşe Giriş Bildirgesi sayfasına geçiliyor</span></strong></li>";
                }));

                IOC.SgkLinksService.Command = "clk t:t tv:Ana Menü\n\rlck yeri Sicil No\n\rclk t:t tv:SİGORTALI İŞE GİRİŞ BİLDİRGESİ\n\rlck Sigortalının";

                if (IOC.SgkLinksService.GoAhead(out msg))
                {
                    // --- BÖLÜM 3: Son Teyit (Doğrulama) Döngüsü ---
                    for (int k = 0; k < tcknList.Count; k++)
                    {
                        if (bgwSoyad.CancellationPending) { e.Cancel = true; return; }

                        string currentTc = tcknList[k];
                        DetailsNameTc.Tcno = currentTc;

                        Invoke((Action)(() =>
                        {
                            lblReportSoyad.Text += $"<li><strong><span style=\"font-size: 10pt\">{currentTc} için İşe Giriş Bildirgesi'nden Ad-Soyad sorgulanıyor</span></strong></li>";
                        }));

                        IOC.SgkLinksService.FillTextBox("x", "/html/body/table[3]/tbody/tr/td/table/tbody/tr[2]/td[2]/table[3]/tbody/tr/td/table/tbody/tr/td/form/table/tbody/tr/td/table/tbody/tr[6]/td/table/tbody/tr/td[4]/nobr/input[1]", "DetailsNameTC.tcno", out msg);
                        IOC.SgkLinksService.Command = "clk t:x tv:/html/body/table[3]/tbody/tr/td/table/tbody/tr[2]/td[2]/table[3]/tbody/tr/td/table/tbody/tr/td/form/table/tbody/tr/td/table/tbody/tr[6]/td/table/tbody/tr/td[4]/nobr/input[2]\n\rlne\n\rlck Kimlik Numarası :";

                        if (IOC.SgkLinksService.GoAhead(out msg))
                        {
                            if (bgwSoyad.CancellationPending) { e.Cancel = true; return; }

                            IWebElement tcNo = wait.Until(d => d.FindElement(By.XPath("/html/body/table[3]/tbody/tr/td/table/tbody/tr[2]/td[2]/table[3]/tbody/tr/td/table/tbody/tr/td/form/table/tbody/tr/td/table/tbody/tr[8]/td")));
                            IWebElement adSoyad = wait.Until(d => d.FindElement(By.XPath("/html/body/table[3]/tbody/tr/td/table/tbody/tr[2]/td[2]/table[3]/tbody/tr/td/table/tbody/tr/td/form/table/tbody/tr/td/table/tbody/tr[9]/td")));

                            Thread.Sleep(500);
                            if (tcNo.Text != null && tcNo.Text.Contains("Kimlik Numarası :"))
                            {
                                listeLastNames[k].Asl = adSoyad.Text.Trim();
                                Invoke((Action)(() =>
                                {
                                    lblReportSoyad.Text += $"<li><strong><span style=\"font-size: 10pt\">{currentTc} : {adSoyad.Text.Trim()} eşleşmesi tespit edildi</span></strong></li>";
                                }));
                            }
                        }
                        else
                        {
                            hasErrorSoyad = true;
                            Invoke((Action)(() =>
                            {
                                if (msg.Contains("Geçersiz"))
                                {
                                    listeLastNames[k].Asl = "Geçersiz kimlik numarası";
                                    lblReportSoyad.Text += $"<li><strong><span style=\"color: red; font-size: 10pt\">{currentTc},için 'Geçersiz Kimlik Numarası' hatası alındı</span></strong></li>";
                                }
                                else if (msg == "HATA BİLDİRİM")
                                {
                                    lblReportSoyad.Text += $"<li><strong><span style=\"color: red; font-size: 10pt\">{currentTc},için 'Geçersiz Kimlik Numarası' hatası alındı</span></strong></li>";
                                    Surucu.Driver.Url = "https://uyg.sgk.gov.tr/SigortaliTescil/jsp/anamenu.jsp";
                                }
                            }));
                        }
                    }
                    if (bgwSoyad.CancellationPending) { e.Cancel = true; return; }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                Invoke((Action)(() =>
                {
                    if (msg.Contains("arçacığı durdurul"))
                    {
                        lblMessage.Text = $"İşlem kullanıcı tarafından iptal edildi.";
                    }
                    else
                    {
                        lblMessage.Text = $"Güncelleme başarısız! Hata: {msg}";
                    }
                }));
            }

            Thread.Sleep(3000);
            Invoke((Action)(() =>
            {
                btnSoyadUpdateReport.Visibility = ElementVisibility.Visible;
            }));
        }
        private void BgwSoyadComplated(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                lblMessage.Text = $"Hata: ! {e.Error.Message}";
            }
            else if (e.Cancelled)
            {
                lblMessage.Text = $"İşlem iptal edildi!"; btnSoyadUpdateReport.Enabled = false;
            }
            else
            {
                lblReportSoyad.Text += $"İşlem tamamlandı\r\n";
                if (hasErrorSoyad)
                {

                    lblMessage.Text = "Bazı soyadı güncellenmeleri yapılamadı. Ayrıntılar için \"Güncelleme Raporu\"butonuna basınız";
                }
                else
                {
                    lblMessage.Text = "Bütün soyadı güncellenmeleri yapıldı. Ayrıntılar için \"Güncelleme Raporu\"butonuna basınız";
                }
            }
            bgwSoyad.Dispose();
            ProcessFinishedSoyad();
        }
        private void BgwSoyadChanged(object sender, ProgressChangedEventArgs e)
        {

        }
        private void ProcessStartedSoyad()
        {
            IOC.SgkLinksService.LoginBtnClicked = false;
            ribbonBar.Enabled = false;
            rgvCompanyList.Enabled = false;
            btnCancelSoyad.Enabled = true;
            lblReportSoyad.Text = string.Empty;
            btnSoyadUpdateReport.Visibility = ElementVisibility.Collapsed;
            pnlAddLastName.Enabled = false;
            pnlWaitSoyad.Visible = true;
            rwbLastNames.StartWaiting();
        }
        private void ProcessFinishedSoyad()
        {
            ribbonBar.Enabled = true;
            rgvCompanyList.Enabled = true;
            btnCancelSoyad.Enabled = false;
            btnSoyadUpdateReport.Visibility = ElementVisibility.Visible;
            pnlWaitSoyad.Visible = false;
            pnlAddLastName.Enabled = true;
            rwbLastNames.StopWaiting();
            GlobalVars.CancelProcess = false;
            LinkGlobals.LinkCancel = false;
            if (Settings.Default.disposeDriver && Surucu.Driver != null) { Surucu.Driver.Dispose(); Surucu.Driver = null; WinHelpers.ClearAll(); }
            IOC.LinkOps.DisposeProcess();
        }
        private bool PasteIntoGrid(RadGridView source, out string msg)
        {
            msg = "";
            DataObject o = (DataObject)Clipboard.GetDataObject();
            try
            {
                if (o.GetDataPresent(DataFormats.Text))
                {
                    string[] pastedRows = Regex.Split(o.GetData(DataFormats.Text).ToString(), "\r\n");
                    Match match;
                    foreach (string pastedRow in pastedRows)
                    {
                        match = Regex.Match(pastedRow, @"[1-9]{1}[0-9]{9}[02468]{1}");
                        if (match.Groups[0].Value == "")
                        {
                            continue;
                        }
                        if (!IOC.WinHelpers.IsTcnoExist(rgvLastName, match.Groups[0].Value, out msg))
                        {
                            source.Rows.Add(match.Groups[0].Value);
                        }
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                lblMessage.Text = msg;
                return false;
            }
        }

        #region butonlar
        private void btnStartProcessSoyad_Click(object sender, EventArgs e)
        {
            report = String.Empty; listeLastNames.Clear(); 
            ProcessStartedSoyad();
            if (!bgwSoyad.IsBusy)
            {
                bgwSoyad.RunWorkerAsync();
            }

        }
        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            try
            {
                if (textAddTcno.Text.Trim() != null)
                {
                    isTcnoValid = false; string msg = ""; SearchReport.KimlikNo = textAddTcno.Text.Trim();
                    isTcnoValid = IOC.WinHelpers.IsTcnoValid(SearchReport.KimlikNo, out msg);
                    if (!isTcnoValid) { lblMessage.Text = msg; textAddTcno.Focus();  return; }
                    foreach (var row in rgvLastName.Rows)
                    {
                        if (row.Cells[0].Value.ToString() == textAddTcno.Text.Trim())
                        {
                            lblMessage.Text = "Eklemeye çalıştığınız kimlik numarası zaten listede var!";
                            return;
                        }
                    }

                    lblMessage.Text = String.Empty;
                    rgvLastName.Rows.Add(textAddTcno.Text.Trim());
                }
                else
                {
                    lblMessage.Text = String.Empty;
                }
                textAddTcno.SelectAll();
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Kimlik numarası eklenemedi, lütfen tekrar deneyin {ex.Message.ToString()}";
            }

        }
        private void btnPaste_Click(object sender, EventArgs e)
        {
            string msg = "";
            if (!PasteIntoGrid(rgvLastName, out msg))
            {
                lblMessage.Text = $"Panodan veri alınamadı! Hata: {msg}";
            }
        }
        private void btnClean_Click(object sender, EventArgs e)
        {
            lblMessage.Text = String.Empty;
            rgvLastName.Rows.Clear();
        }
        private void btnReport_Click(object sender, EventArgs e)
        {
            RadForm f = new RadForm();
            if (modulType == ModulTypeEnum.Tesvik)
            {
                f = new FIncentiveReport(GlobalVars.LstInc);
            }
            else
            {
                f = new FLastNamesReport(listeLastNames);
            }
            f.ShowDialog();
        }
        private void btnCancelSoyad_Click(object sender, EventArgs e)
        {
            btnCancelSoyad.Enabled = false;
            if (modulType == ModulTypeEnum.Lastname)
            {
                btnCancelSoyad.Text = "Soyad güncelleme işlemi iptal ediliyor...";
                bgwSoyad.CancelAsync();
            }
            else if (modulType == ModulTypeEnum.Tesvik)
            {
                btnCancelSoyad.Text = "Teşvik sorgulama işlemi iptal ediliyor...";
                LinkGlobals.LinkCancel = true;
                bgwLinks.CancelAsync();
            }
        }
        #endregion

        #region rgvEvents

        private void rgvFamilyName_RowsChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            if (rgvCompanyList.RowCount > 0 && rgvLastName.RowCount > 0)
            {
                btnStartSoyad.Enabled = true;
                btnStartSoyad2.Enabled = true;
                btnProcessTesvik.Enabled = true;
            }
            else
            {
                btnProcessTesvik.Enabled = false;
                btnStartSoyad2.Enabled = false;
                btnStartSoyad.Enabled = false;
            }

        }
        private void rgvFamilyName_UserAddingRow(object sender, GridViewRowCancelEventArgs e)
        {
            string msg = "";
            int count;
            try
            {
                if (e.Rows[0].Cells[0].Value != null)
                {
                    count = e.Rows[0].Cells[0].Value.ToString().Trim().Length;
                    Regex rx = new Regex(@"[0-9]{1}[0-9]{9}[02468]{1}");
                    if (!rx.IsMatch(e.Rows[0].Cells[0].Value.ToString()) || count != 11)
                    {
                        lblMessage.Text = "Geçersiz kimlik numarası!";
                        e.Cancel = true;
                    }
                    else
                    {
                        foreach (var row in rgvLastName.Rows)
                        {
                            if (row.Cells[0].Value.ToString() == e.Rows[0].Cells[0].Value.ToString())
                            {
                                lblMessage.Text = "Eklemeye çalıştığınız kimlik numarası zaten listede var!";
                                e.Cancel = true;
                            }
                        }
                        if (!e.Cancel)
                        {
                            lblMessage.Text = String.Empty;
                        }
                    }
                }
                else
                {
                    e.Cancel = false; lblMessage.Text = String.Empty;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                lblMessage.Text = $"Kimlik numarası eklenemedi, lütfen tekrar deneyin. Hata: {msg}";
            }

        }
        #endregion

        #region tcknTextBoxes
        private void textAddTcno_KeyUp(object sender, KeyEventArgs e)
        {
            if (sender == textAddTcno && e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                btnAddPerson_Click(null, null);
            }
            
        }
        private void textAddTcno_TextChanged(object sender, EventArgs e)
        {
            if (textAddTcno.Text.Trim() == "")
            {
                lblMessage.Text = String.Empty;
                textAddTcno.Clear();
            }
        }
        private void textAddTcno_KeyPress(object sender, KeyPressEventArgs e)
       {
            RadTextBoxElement radTextBox = sender as RadTextBoxElement;
            RadTextBoxControl radTextBoxControl = sender as RadTextBoxControl;
            if (Char.IsDigit(e.KeyChar) || e.KeyChar == '\b' || Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }

            if (sender == texTckn && radTextBox.Text.Trim().Length == 11 )
            {
                if (e.KeyChar == '\b' || Char.IsControl(e.KeyChar))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
        }
        #endregion

        #endregion

        #region SgkOtomasyon

        #region SGK_OtomasyonBGW
        private void InitializeBgwSgk()
        {
            bgwSgk.WorkerSupportsCancellation = true;
            bgwSgk.DoWork += new DoWorkEventHandler(BgwSgkDoWork);
            bgwSgk.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BgwSgkComplated);
            bgwSgk.ProgressChanged += new ProgressChangedEventHandler(BgwSgkChanged);
        }
        private void BgwSgkDoWork(object sender, DoWorkEventArgs e)
        {
            IOC.TrmBase.tryCount = 1;
            GlobalVars.CancelProcess = false;
            string msg;
            IOC.SgkLinksService.CaptchaCode = null;
            LinkGlobals.Ivd = false;
            lblMessage.Text = string.Empty;
            sgkOtomRapor = "";
            btnCancelHESAP.Enabled = true;
            GlobalVars.ProcessReport = $"<html><ul>";
            lblReportHESAP.Text = GlobalVars.ProcessReport;
            tempFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\SgkAsistan\Pdftmp";
            pdfFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\SgkAsistan\Pdf";
            try
            {
                if (rgvCompanyList.SelectedRows.Count > 0)
                {
                    switch (hesapType)
                    {
                        case HesapTypeEnum.Donemselborc:
                            IOC.LinkOps.CmdUrlVurl = LinkGlobals.LstLinks.Where(x => x.Id == 4).Select(x=> new CmdUrlVurl { Cmd = x.Cmd,Url = x.Url, Vurl = x.Vurl }).FirstOrDefault();
                            lblReportHEADER.Text = $"<html><p><span style=\"font-size: 12pt\"><strong>DÖNEMSEL  BORÇ  SORGULAMA</strong></span></html>";
                            break;
                        case HesapTypeEnum.EborcuYoktur:
                            IOC.LinkOps.CmdUrlVurl = LinkGlobals.LstLinks.Where(x => x.Id == 52).Select(x => new CmdUrlVurl { Cmd = x.Cmd,Url = x.Url, Vurl = x.Vurl }).FirstOrDefault(); lblReportHEADER.Text = $"<html><p><span style=\"font-size: 12pt\"><strong>E  BORCU  YOKTUR  SORGULAMA</strong></span></html>";
                            break;
                        case HesapTypeEnum.Emanet:
                            IOC.LinkOps.CmdUrlVurl = LinkGlobals.LstLinks.Where(x => x.Id == 5).Select(x => new CmdUrlVurl { Cmd = x.Cmd,Url = x.Url, Vurl = x.Vurl }).FirstOrDefault(); lblReportHEADER.Text = $"<html><p><span style=\"font-size: 12pt\"><strong>EMANETTEKİ  TAHSİLATLARI  SORGULAMA</strong></span></html>";
                            break;
                        case HesapTypeEnum.Icra:
                            IOC.LinkOps.CmdUrlVurl = LinkGlobals.LstLinks.Where(x => x.Id == 6).Select(x => new CmdUrlVurl { Cmd = x.Cmd,Url = x.Url, Vurl = x.Vurl }).FirstOrDefault(); lblReportHEADER.Text = $"<html><p><span style=\"font-size: 12pt\"><strong>İCRA  BİLGİLERİ  SORGULAMA</strong></span></html>";
                            break;
                        case HesapTypeEnum.Destek6661:
                            IOC.LinkOps.CmdUrlVurl = LinkGlobals.LstLinks.Where(x => x.Id == 20).Select(x => new CmdUrlVurl { Cmd = x.Cmd,Url = x.Url, Vurl = x.Vurl }).FirstOrDefault(); lblReportHEADER.Text = $"<html><p><span style=\"font-size: 12pt\"><strong>6661 ASGARİ  DESTEK  SORGULAMA</strong></span></html>";
                            break;
                        case HesapTypeEnum.Igl:

                            IOC.LinkOps.CmdUrlVurl = LinkGlobals.LstLinks.Where(x => x.Id == 50).Select(x => new CmdUrlVurl { Cmd = x.Cmd,Url = x.Url, Vurl = x.Vurl }).FirstOrDefault(); lblReportHEADER.Text = $"<html><p><span style=\"font-size: 12pt\"><strong>İŞYERİ HAREKET  LİSTESİ  SORGULAMA</strong></span></html>";
                            break;
                        case HesapTypeEnum.Hl:
                        case HesapTypeEnum.Tahakkuk:
                            lstHl.Clear(); lstHlp.Clear(); lstHlDownload.Clear();
                            DdlHlSelectedValueChanged(null, null);
                            SearchReport.DownloadDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\SgkAsistan\Pdftmp";
                            IOC.LinkOps.CmdUrlVurl = cbHlUnapproved.CheckState == CheckState.Checked? LinkGlobals.LstLinks.Where(x => x.Id == 15).Select(x => new CmdUrlVurl { Cmd = x.Cmd, Url = x.Url, Vurl = x.Vurl }).FirstOrDefault() : LinkGlobals.LstLinks.Where(x => x.Id == 13).Select(x => new CmdUrlVurl { Cmd = x.Cmd,Url = x.Url, Vurl = x.Vurl }).FirstOrDefault();
                            lblReportHEADER.Text = hesapType == HesapTypeEnum.Hl? $"<html><p><span style=\"font-size: 10pt\"><strong>HİZMET LİSTESİ SORGULAMA</strong></span></p></html>" : $"<html><p><span style=\"font-size: 10pt\"><strong>TAHAKKUK SORGULAMA</strong></span></p></html>";
                            break;
                        case HesapTypeEnum.Hlu:
                            lstHl.Clear(); lstHlp.Clear(); lstHlDownload.Clear();
                            DdlHlSelectedValueChanged(null, null);
                            SearchReport.DownloadDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\SgkAsistan\Pdftmp";
                            IOC.LinkOps.CmdUrlVurl = LinkGlobals.LstLinks.Where(x => x.Id == 13).Select(x => new CmdUrlVurl { Cmd = x.Cmd, Url = x.Url, Vurl = x.Vurl }).FirstOrDefault();
                            lblReportHEADER.Text = $"<html><p><span style=\"font-size: 10pt\"><strong>HİZMET LİSTESİ (ÜCRETSİZ) SORGULAMA</strong></span></p></html>";

                            break;
                    }
                    sgkOtomRapor += lblReportHEADER.Text;
                    IOC.SgkLinksService.Command = IOC.LinkOps.CmdUrlVurl.Cmd;
                    IOC.SgkLinksService.Vurl = IOC.LinkOps.CmdUrlVurl.Vurl ;
                    IOC.SgkLinksService.Url = IOC.LinkOps.CmdUrlVurl.Url;
                    List<Company> lst = IOC.WinHelpers.GetSelectedCompaniesFromRgv(rgvCompanyList, out msg);
                    Surucu.Driver = IOC.WinHelpers.GetWebDriver(Settings.Default.hideBrowser, out msg);
                    foreach (Company login in lst)
                    {
                        GlobalVars.IsCompanyActive = login.Kkc.Year > 3000 ? true : false;
                        if (!GlobalVars.IsCompanyActive && hesapType == HesapTypeEnum.Igl) { 
                            lblReportHESAP.Text += $"<p></p><ul><li><span style=\"color: navy\">{login.CompanyName} adlı firma {login.Kkc:dd.MM.yyyy} tarihinde kanun kapsamı dışına çıktığı için sorgulama işlemi yapılamaz</span></li></ul></html>"; 
                            sgkOtomRapor += lblReportHESAP.Text; lblReportHESAP.Text = "<html><ul>"; GlobalVars.ProcessReport = lblReportHESAP.Text;
                            lblMessage.Text = $"{login.CompanyName} adlı firma {login.Kkc:dd.MM.yyyy} tarihinde kanun kapsamı dışına çıktığı için sorgulama işlemi yapılamaz";
                            continue; }
                        GlobalVars.HesapTypeEtDbCr = (hesapType == HesapTypeEnum.Donemselborc || hesapType == HesapTypeEnum.Emanet || hesapType == HesapTypeEnum.Icra) ? true : false;

                        CompanyName = login.CompanyName;
                        
                        if (bgwSgk.CancellationPending == true) { e.Cancel = true; return; }
                        IOC.SgkLinksService.LoginError = false;
                        LinkGlobals.HasRefreshButton = false;
                        
                        IOC.SgkLinksService.CompanyName = login.CompanyName;
                        IOC.SgkLinksService.SetSgkLoginCredentials(login);

                        //string url = IOC.SgkLinksService.Command.Substring(4, IOC.SgkLinksService.Command.IndexOf('\r') - 4);

                        IOC.LinkOps.VisitLink(login, IOC.SgkLinksService.Url, ref acError, out msg, ref lblReportHESAP);
                        if (bgwSgk.CancellationPending == true) { e.Cancel = true; return; }

                        if (msg.Contains("iptal")) { e.Cancel = true; return;  }
                        else if (msg == "continue" || msg.Contains("Hata"))
                        {
                            GlobalVars.ProcessReport += $"<li><strong><span style=\"color: red; font-size: 10pt\">{IOC.SgkLinksService.CompanyName} için oturum açılamadı! {IOC.SgkLinksService.Message}</span></strong></li></ul></html><ul>";
                            lblReportHESAP.Text = GlobalVars.ProcessReport;
                            sgkOtomRapor += GlobalVars.ProcessReport; lblReportHESAP.Text = "<html><ul>"; GlobalVars.ProcessReport = lblReportHESAP.Text; continue;
                        }
                        else if (msg == "continueGoAhead")
                        {
                            lblReportHESAP.Text += $"<li><strong><span style=\"color: red; font-size: 10pt\">Oturum açıldıktan sonra devam edilemiyor. Hata: {IOC.SgkLinksService.Message}</span></strong></li></ul></html><ul>";
                            lblMessage.Text = $"Oturum açıldıktan sonra devam edilemiyor. Hata: {IOC.SgkLinksService.Message}";
                            sgkOtomRapor += GlobalVars.ProcessReport; lblReportHESAP.Text = "<html><ul>"; GlobalVars.ProcessReport = lblReportHESAP.Text; continue;
                        }

                        switch (hesapType)
                        {
                            case HesapTypeEnum.Donemselborc:
                                lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">Dönemsel borç sayfasına ulaşıldı, borç kayıtları aranıyor.</span></strong></li>";
                                string dbTable = IOC.SgkLinksService.GetTableFromPage("x", Settings.Default.tblPD , 2, 1, out msg);
                                IOC.LinkOps.LogOut("t", "Çıkış", "E-BİLDİRGE", out msg);
                                if (dbTable == "iptal") { e.Cancel = true; return; }

                                if (dbTable == null) { lblReportHESAP.Text += $"<li><strong><span style=\"color: red\">{CompanyName} için dönemsel borçlar tablosuna erişilemedi. {msg}</span></strong></li></ul></html>";
                                    sgkOtomRapor += lblReportHESAP.Text; lblReportHESAP.Text = "<html><ul>"; GlobalVars.ProcessReport = lblReportHESAP.Text;
                                    continue; }
                                if (dbTable.Trim().Length == 0) { 
                                    lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">Dönemsel borç yok</span></strong></li></ul></html>";
                                    sgkOtomRapor += lblReportHESAP.Text; lblReportHESAP.Text = "<html><ul>"; GlobalVars.ProcessReport = lblReportHESAP.Text;
                                    lstDb.Add(new SgkDb() { Cd = DateTime.Now, Cx = login.Id, Cn = (from x in GlobalVars.Companies where x.Id == login.Id select x.CompanyName).FirstOrDefault() });
                                    continue;
                                }
                                else { lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">Borç kayıtları alınıyor</span></strong></li>"; }
                                lstDb.AddRange(IOC.SgkDb.ConvertToSgkDb(dbTable, login.Id));
                                break;
                            case HesapTypeEnum.EborcuYoktur:
                                lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">E Borcu Yoktur sayfasına ulaşıldı, borç kayıtları aranıyor.</span></strong></li>";
                                if(Surucu.Driver.PageSource.Replace("  "," ").Contains("E-Borcu Yoktur Aktivasyonu Bulunmamaktadır"))
                                {
                                    lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt; color: red\">Firmanın \"E-Borcu Yoktur Aktivasyonu\" yok. {msg}</span></strong></li></ul></html>";
                                    sgkOtomRapor += lblReportHESAP.Text; lblReportHESAP.Text = "<html><ul>"; GlobalVars.ProcessReport = lblReportHESAP.Text;
                                }
                                    IWebElement btnBarkodluPDF = IOC.SgkLinksService.GetElementBy("i", "ihaleOlmayanTurkiyeGeneliBorcKontrol_eBorcuYazdirPdfOlustur", out msg);
                                if (btnBarkodluPDF == null)
                                {
                                    lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt; color: red\">{CompanyName} için Barkodlu PDF dosyası indirilemiyor. {msg}</span></strong></li></ul></html>";
                                    sgkOtomRapor += lblReportHESAP.Text; lblReportHESAP.Text = "<html><ul>"; GlobalVars.ProcessReport = lblReportHESAP.Text;
                                }
                                string dateTime = DateTime.Now.ToString("ddMMyyyy_HHmm");
                                btnBarkodluPDF.Click();
                                string sicilNo = login.Sgsc.Substring(9, 7); //18102023_150625

                                // dosya indi mi?
                                bool hasDownloaded = false, hasToBreak = false;
                                FileInfo[] files; FileInfo pdfFile = new FileInfo("temp");
                                for (int j = 0; j < 100; j++)
                                {
                                    files = new DirectoryInfo(SearchReport.DownloadDir).GetFiles("*.pdf");
                                    if (files != null && files.Count() > 0)
                                    {
                                        foreach (FileInfo file in files)
                                        {
                                            if (file.Name.Contains(sicilNo) && file.Name.Contains(dateTime) && file.Length != 0) { hasDownloaded = true; pdfFile = file; hasToBreak = true;  break; }
                                        }
                                        if (hasToBreak) { lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">Barkodlu pdf dosyası indirildi</span></strong></li>"; break; }
                                    }
                                    else { hasDownloaded = false; }
                                    Thread.Sleep(500);
                                }
                                if (!hasDownloaded)
                                {
                                    lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt; color: red\">Barkodlu pdf dosyası indirilemedi</span></strong></li>";
                                    sgkOtomRapor += lblReportHESAP.Text; lblReportHESAP.Text = "<html><ul>"; GlobalVars.ProcessReport = lblReportHESAP.Text;
                                    continue;
                                }
                                File.Copy(pdfFile.FullName, Path.Combine(SearchReport.UserSelectedDir, $"{login.CompanyName}_Borcu_Yoktur.pdf"), true);
                                File.Delete(pdfFile.FullName);
                                // 
                                IOC.LinkOps.LogOut("x", "//*[@id='navigation']/li[5]", "İŞVEREN SİSTEMİ", out msg); Thread.Sleep(2000);
                                break;
                            case HesapTypeEnum.Emanet:
                                lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">Emanetteki Tahsilatlar sayfasına ulaşıldı, tahsilat kayıtları aranıyor.</span></strong></li>";
                                string emnTable = IOC.SgkLinksService.GetTableFromPage("x", Settings.Default.tblET, 2, 0, out msg);
                                string msspTable = IOC.SgkLinksService.GetTableFromPage("x", Settings.Default.tblME, 2, 0, out msg);
                                IOC.LinkOps.LogOut("t", "Çıkış", "E-BİLDİRGE", out msg);
                                if (emnTable == "iptal") { e.Cancel = true;  return; }
                                if (msspTable == "iptal") { e.Cancel = true; return; }
                                if (emnTable == null || msspTable == null) { lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt; color: red\">{CompanyName} için emanet tahsilatı ve mossip emanet tahsilat tablolarına erişilemedi. {msg}</span></strong></li></ul></html>";
                                    sgkOtomRapor += lblReportHESAP.Text; lblReportHESAP.Text = "<html><ul>"; GlobalVars.ProcessReport = lblReportHESAP.Text; continue; }
                                if (emnTable == "boş") { lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">Emanette tahsilat yok</span></strong></li>"; lstEt.Add(new SgkEt() { Tt = DateTime.Now, Cd = DateTime.Now, Cx = login.Id, Cn = (from x in GlobalVars.Companies where x.Id == login.Id select x.CompanyName).FirstOrDefault() }); }
                                else{ lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">Emanetteki tahsilat kayıtları alınıyor</span></strong></li>"; lstEt.AddRange(IOC.EmanetMossip.ConvertToSgkEt(emnTable, login.Id)); }
                                if (msspTable.Trim().Contains("GÖSTERİLECEK KAYIT YOK")) { lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">Mossip emanet tahsilatı yok</span></strong></li>"; lstMe.Add(new SgkMe() { Byt = DateTime.Now,  Cd = DateTime.Now, Cx = login.Id, Cn = (from x in GlobalVars.Companies where x.Id == login.Id select x.CompanyName).FirstOrDefault() }); }
                                else { lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">Mossip emanet tahsilat kayıtları alınıyor</span></strong></li>"; lstMe.AddRange(IOC.EmanetMossip.ConvertToSgkMe(msspTable, login.Id)); }
                                break;
                            case HesapTypeEnum.Icra:
                                lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">İcra Bilgileri sayfasına ulaşıldı, icra kayıtları aranıyor.</span></strong></li>";
                                if(!Surucu.Driver.PageSource.Replace("  "," ").Contains("Kart No")) { 
                                    lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">İcra ve taksit borcu yoktur</span></strong></li></ul></html>";
                                    sgkOtomRapor += lblReportHESAP.Text; lblReportHESAP.Text = "<html><ul>"; GlobalVars.ProcessReport = lblReportHESAP.Text;
                                    lstCr.Add(new SgkCr() { Cd = DateTime.Now, Cx = login.Id, Cn = (from x in GlobalVars.Companies where x.Id == login.Id select x.CompanyName).FirstOrDefault() }); 
                                    IOC.LinkOps.LogOut("t", "Çıkış", "E-BİLDİRGE", out msg); 
                                    continue; }
                                string crTable = IOC.SgkLinksService.GetTableFromPage("x", Settings.Default.tblICR, 2, 1, out msg);
                                IOC.LinkOps.LogOut("t", "Çıkış", "E-BİLDİRGE", out msg);
                                if (crTable == "iptal") { e.Cancel = true;  return; }
                            
                                if (crTable == null) { lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt; color: red\">{CompanyName} için icra bilgileri tablosuna erişilemedi. {msg}</span></strong></li></ul></html>";
                                    sgkOtomRapor += lblReportHESAP.Text; lblReportHESAP.Text = "<html><ul>"; GlobalVars.ProcessReport = lblReportHESAP.Text; continue; }
                                lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">İcra kayıtları alınıyor</span></strong></li>";
                                lstCr.AddRange(IOC.SgkCr.ConvertToSgkCr(crTable, login.Id));
                                break;
                            case HesapTypeEnum.Destek6661:
                                lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">6661 Asgari Destek sayfasına ulaşıldı, destek kayıtları aranıyor.</span></strong></li>";
                                IWebElement slcSecim = IOC.SgkLinksService.GetElementBy("i", "secim", out msg) ;
                                if(slcSecim == null) { lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt; color: red\">{CompanyName} için 6661 asgari destek seçilemiyor. {msg}</span></strong></li></ul></html>";
                                    sgkOtomRapor += lblReportHESAP.Text; lblReportHESAP.Text = "<html><ul>"; GlobalVars.ProcessReport = lblReportHESAP.Text; continue; }
                                new SelectElement(slcSecim).SelectByIndex(3);Thread.Sleep(1000);
                                IWebElement slcSec = IOC.SgkLinksService.GetElementBy("i", "sec", out msg);// driver.FindElement(By.Id("sec"));
                                if (slcSec == null) { lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt; color: red\">{CompanyName} için yil seçilemiyor. {msg}</span></strong></li></ul></html>";
                                    sgkOtomRapor += lblReportHESAP.Text; lblReportHESAP.Text = "<html><ul>"; GlobalVars.ProcessReport = lblReportHESAP.Text; continue; }
                                List<IWebElement> opts = slcSec.FindElements(By.TagName("option")).ToList();
                                bool yilVar = false; string selectedYear = "";
                                selectedYear = ddl6661Year.SelectedValue == null? DateTime.Now.Year.ToString() : ddl6661Year.SelectedValue.ToString();
                                for (int i = 0; i < opts.Count; i++)
                                {
                                    if (opts[i].Text == selectedYear)
                                    {
                                        new SelectElement(slcSec).SelectByIndex(i); yilVar = true; break;
                                    }
                                }
                                if(!yilVar) new SelectElement(slcSec).SelectByIndex(0); 
                                Thread.Sleep(1000);
                                IWebElement btnListele = IOC.SgkLinksService.GetElementBy("i", "secimBelirle_0", out msg);// driver.FindElement(By.Id("secimBelirle_0"));

                                if(btnListele == null) { lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt; color: red\">{CompanyName} için 6661 asgari destek listesi açılamıyor. {msg}</span></strong></li></ul></html>";
                                    sgkOtomRapor += lblReportHESAP.Text; lblReportHESAP.Text = "<html><ul>"; GlobalVars.ProcessReport = lblReportHESAP.Text; IOC.LinkOps.LogOut("x", "//*[@id='navigation']/li[5]", "İŞVEREN SİSTEMİ", out msg); continue; }
                                btnListele.Click();
                                IOC.CommonFuncs.WaitForPageLoad(out msg);
                                if(Surucu.Driver.PageSource.Replace("  "," ").Contains("Kayıt Bulunamamıştır.")) {
                                    lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">{selectedYear} yılı için 6661 asgari destek kaydı yoktur</span></strong></li></ul></html>";
                                    sgkOtomRapor += lblReportHESAP.Text; lblReportHESAP.Text = "<html><ul>"; GlobalVars.ProcessReport = lblReportHESAP.Text;
                                    lst6661.Add(new Sgk6661() { Cd = DateTime.Now, Cx = login.Id, Cn = (from x in GlobalVars.Companies where x.Id == login.Id select x.CompanyName).FirstOrDefault(), Yil = selectedYear, Ay = "01", Dt = 0, Fgs = 0, Tt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0,0,0), Un = GlobalVars.ActiveUser }) ; IOC.LinkOps.LogOut("x", "//*[@id='navigation']/li[5]", "İŞVEREN SİSTEMİ", out msg); continue; }
                                string _6661Table = Convert.ToInt32(selectedYear) >=2019 ? IOC.SgkLinksService.GetTableFromPage("x", Settings.Default.tbl6661, 2, 0, out msg) : IOC.SgkLinksService.GetTableFromPage("x", Settings.Default.tbl6661.Replace("table[2]", "table"), 2, 0, out msg);
                                IOC.LinkOps.LogOut("x", "//*[@id='navigation']/li[5]", "İŞVEREN SİSTEMİ", out msg);
                                if (_6661Table == "iptal") { e.Cancel = true; return; }
                                if (_6661Table == null) { lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt; color: red\">{CompanyName} için 6661 asgari destek tablosu okunamıyor. {msg}</span></strong></li></ul></html>";
                                    sgkOtomRapor += lblReportHESAP.Text; lblReportHESAP.Text = "<html><ul>"; GlobalVars.ProcessReport = lblReportHESAP.Text; continue; }
                                lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">6661 asgari destek kayıtları alınıyor</span></strong></li>";
                                lst6661.AddRange(IOC.Sgk6661.ConvertToSgk6661(_6661Table, login.Id));
                                break;
                            case HesapTypeEnum.Igl:
                                DateTime iglMinDate = new DateTime(2014, 6, 7);
                                CheckUpVars.StartDate = (DateTime)dtpIglFirst.Value >= iglMinDate ? (DateTime)dtpIglFirst.Value : iglMinDate;
                                CheckUpVars.EndDate = (DateTime)dtpIglLast.Value > iglMinDate ? (DateTime)dtpIglLast.Value : iglMinDate;
                                lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">{CheckUpVars.StartDate:dd/MM/yyyy} - {CheckUpVars.EndDate:dd/MM/yyyy} tarihleri arasındaki işe giriş/işten çıkış kayıtları aranıyor.</span></strong></li>";
                                List<string> iglTables = new List<string>(); bool shouldBreak = false; List<int> removeIds = new List<int>();
                                try
                                {
                                    for (DateTime sd = CheckUpVars.StartDate; sd < CheckUpVars.EndDate;)
                                    {
                                        string gun = $"{ sd:dd}"; string ay = $"{ sd:MM}"; string yil = $"{ sd:yyyy}";
                                        IOC.SgkLinksService.FillTextBox("n", "tx_hrktBasTarGG", gun, out msg);
                                        IOC.SgkLinksService.FillTextBox("n", "tx_hrktBasTarAA", ay, out msg);
                                        IOC.SgkLinksService.FillTextBox("n", "tx_hrktBasTarYY", yil, out msg);
                                        sd = sd.AddDays(15);
                                        gun = $"{ sd:dd}"; ay = $"{ sd:MM}"; yil = $"{ sd:yyyy}";

                                        IOC.SgkLinksService.FillTextBox("n", "tx_hrktBitTarGG", gun, out msg);
                                        IOC.SgkLinksService.FillTextBox("n", "tx_hrktBitTarAA", ay, out msg);
                                        IOC.SgkLinksService.FillTextBox("n", "tx_hrktBitTarYY", yil, out msg);
                                        IWebElement sorgulabtn = IOC.SgkLinksService.GetButtonElementBy("n", "sorgulabtn", out msg);
                                        sorgulabtn.Click();
                                        IOC.CommonFuncs.WaitForPageLoad(out msg);
                                        if (Surucu.Driver.PageSource.Replace("  "," ").Contains("t bulunamad") || Surucu.Driver.PageSource.Replace("  "," ").Contains("Nothing found to display")) { continue; }
                                        IWebElement pageBanner = null; int tryCount = 1;
                                        while (pageBanner == null)
                                        {
                                            if (tryCount != 1) { Surucu.Driver.Navigate().Refresh(); IOC.CommonFuncs.WaitForPageLoad(out msg); }
                                            pageBanner = IOC.SgkLinksService.GetElementBy("cl", "pagebanner", out msg);
                                            tryCount++;
                                            if (tryCount == 5) break;
                                        }
                                        if (pageBanner == null)
                                        {
                                            lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt; color: red\">Sayfa yüklenmesi sırasında hata oluştu, Lütfen sonra tekrar deneyiniz</span></strong></li>"; shouldBreak = true; break;
                                        }
                                        string pageBannerText = pageBanner.Text;
                                        int spaceLoc = pageBannerText.IndexOf(' ');
                                        int igCount = (pageBannerText.Contains("Bir kay") || pageBannerText.Contains("One")) ? 1 : Convert.ToInt32(pageBannerText.Substring(0, spaceLoc).Replace(",", ""));
                                        if (igCount > 100)
                                        {
                                            int itrCount = igCount % 100 == 0 ? igCount / 100 - 1 : igCount / 100;

                                            iglTables.Add(IOC.SgkLinksService.GetTableFromPage("cl", Settings.Default.tblIGL, 2, 0, out msg));
                                            IWebElement btnNext = null; tryCount = 1;
                                            while(btnNext == null) { 
                                                if (tryCount != 1) { Surucu.Driver.Navigate().Refresh(); IOC.CommonFuncs.WaitForPageLoad(out msg); ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("arguments[0].scrollIntoView(true); ", pageBanner); } 
                                                btnNext = IOC.SgkLinksService.GetButtonElementBy("c", $".pagelinks a:nth-of-type({itrCount + 1})", out msg); tryCount++;
                                                if (tryCount == 5) break;
                                            }
                                            if (btnNext == null)
                                            {
                                                lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt; color: red\">Giriş/Çıkış kayıtları okunurken hata (sonraki sayfa butonu bulunamadı) oluştu\r\n Lütfen {login.CompanyName} için daha sonra tekrar deneyiniz</span></strong></li>"; shouldBreak = true; break;
                                            }

                                            IOC.CommonFuncs.WaitForPageLoad(out msg);
                                            for (int i = 0; i < itrCount; i++)
                                            {
                                                btnNext.Click();
                                                IOC.CommonFuncs.WaitForPageLoad(out msg); 
                                                if (itrCount - i > 1) {
                                                    btnNext = null; tryCount = 1;
                                                    while (btnNext == null)
                                                    {
                                                        if (tryCount != 1) { Surucu.Driver.Navigate().Refresh(); IOC.CommonFuncs.WaitForPageLoad(out msg); ((IJavaScriptExecutor)Surucu.Driver).ExecuteScript("arguments[0].scrollIntoView(true); ", pageBanner); }
                                                        btnNext = igCount < 1000 ? IOC.SgkLinksService.GetButtonElementBy("c", $".pagelinks a:nth-of-type({itrCount + 3})", out msg) : IOC.SgkLinksService.GetButtonElementBy("c", $".pagelinks a:nth-of-type({itrCount + 1})", out msg); tryCount++;
                                                        if (tryCount == 5) break;
                                                    }
                                                    if (btnNext == null)
                                                    {
                                                        lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt; color: red\">Giriş/Çıkış kayıtları okunurken hata (sonraki sayfa butonu bulunamadı) oluştu\r\n Lütfen {login.CompanyName} için daha sonra tekrar deneyiniz</span></strong></li>"; shouldBreak = true; break;
                                                    }
                                                }
                                                iglTables.Add(IOC.SgkLinksService.GetTableFromPage("cl", Settings.Default.tblIGL, 2, 0, out msg));
                                            }
                                        }
                                        else
                                        {
                                            iglTables.Add(IOC.SgkLinksService.GetTableFromPage("cl", Settings.Default.tblIGL, 2, 1, out msg));
                                        }
                                        sd = sd.AddDays(1);
                                    }if (shouldBreak) { IOC.LinkOps.LogOut("t", "Çıkış", "İŞE GİRİŞ-AYRILIŞ", out msg); sgkOtomRapor += lblReportHESAP.Text; lblReportHESAP.Text = "<html><ul>"; GlobalVars.ProcessReport = lblReportHESAP.Text; continue; } // bu firmanın tabloları okunurken hata olduysa sonraki firmaya geç.
                                }
                                catch (Exception ex)
                                {
                                    lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt; color: red\">{CompanyName} için işe giriş / işten çıkış verileri alınamadı.<br>Lütfen daha sonra tekrar deneyiniz. Hata: {ex.Message}</span></strong></li></ul></html>";
                                    sgkOtomRapor += lblReportHESAP.Text; lblReportHESAP.Text = "<html><ul>"; GlobalVars.ProcessReport = lblReportHESAP.Text;
                                    IOC.LinkOps.LogOut("t", "Çıkış", "İŞE GİRİŞ-AYRILIŞ", out msg);
                                    continue;
                                }
                                    
                                if (iglTables.Count == 0 ) { lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">{CheckUpVars.StartDate:dd/MM/yyyy} - {CheckUpVars.EndDate:dd/MM/yyyy} tarihleri arasındaki işe giriş/işten çıkış kaydı yok</span></strong></li></ul></html>";
                                    sgkOtomRapor += lblReportHESAP.Text; lblReportHESAP.Text = "<html><ul>"; GlobalVars.ProcessReport = lblReportHESAP.Text;
                                    IOC.LinkOps.LogOut("t", "Çıkış", "İŞE GİRİŞ-AYRILIŞ", out msg);
                                    continue; }
                                    
                                string iglTable = "";
                                foreach (string tbl in iglTables)
                                {
                                    iglTable += tbl;
                                }
                                if (iglTable == "iptal") { e.Cancel = true;  return; }
                                lstIgl.AddRange(IOC.SgkIgl.ConvertToSgkIgl(iglTable, login.Id));
                                MatchCollection matches = Regex.Matches(iglTable, "\r\n");

                                lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">{CheckUpVars.StartDate:dd/MM/yyyy} - {CheckUpVars.EndDate:dd/MM/yyyy} tarihleri arasında {matches.Count} adet işe giriş/işten çıkış kaydı bulundu</span></strong></li>";
                                IOC.LinkOps.LogOut("t", "Çıkış", "İŞE GİRİŞ-AYRILIŞ", out msg);
                                break;
                            case HesapTypeEnum.Hl:
                            case HesapTypeEnum.Hlu:
                                lstHlDownload.Clear();
                                if(cbHlUnapproved.CheckState == CheckState.Checked)
                                {
                                    HizmetListesiOnayliOnaysiz(false, login, out msg); if (msg == "iptal") { e.Cancel = true; return; } else if (msg == "continue") continue;
                                    Surucu.Driver.Url = "https://ebildirge.sgk.gov.tr/EBildirgeV2/tahakkuk/tahakkukonaylanmisTahakkukDonemBilgileriniYukle.action";
                                }
                                HizmetListesiOnayliOnaysiz(true, login, out msg); if (msg == "iptal") { e.Cancel = true; return; } else if (msg == "continue") continue;
                                if (lstHlDownload.Count == 0) { sgkOtomRapor += lblReportHESAP.Text; lblReportHESAP.Text = "<html><ul>"; GlobalVars.ProcessReport = lblReportHESAP.Text; IOC.LinkOps.LogOut("x", "//*[@id='navigation']/li[3]/a", "E-BİLDİRGE V2", out msg); continue; }

                                lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">{login.CompanyName} için PDF indirme bitti, indirilen dosyalar çözümleniyor</span></strong></li></ul>";

                                bool hlpError = false; 
                                foreach (SgkHlDownload down in lstHlDownload)
                                {
                                    List<SgkHlp> sgkHlps = IOC.SgkHlp.ConvertToSgkHlp(down.NewPath, down.LoginId, down.Tcs, down.PdfId, out msg);
                                    if (msg.Contains("Hata") || sgkHlps == null)
                                    {
                                        hlpError = true; 
                                        lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt; color: red\">{down.PdfId} dosyası çözümlenirken hata oluştu <br>{msg}</span></strong></li>";
                                        break;
                                    }
                                    else
                                    {
                                        lstHlp.AddRange(sgkHlps);
                                    }
                                }
                                if (hlpError) {
                                    string pdfPath = "Pdf";
                                    lstHlp.RemoveAll(s => s.Cx == login.Id);          lstHl.RemoveAll(s => s.Cx == login.Id);
                                    lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt; color: red\"><li>PDF okuma hatası: {msg}. Lütfen \"C:\\ProgramData\\SgkAsistan\\{pdfPath}\\{login.Id}\" klasöründeki ilgili dönemlere ait pdf dosyalarını siliniz.</span></strong></li>";
                                    lblMessage.Text = $"PDF okuma hatası: {msg}. Lütfen \"C:\\ProgramData\\SgkAsistan\\{pdfPath}\\{login.Id}\" klasöründeki ilgili dönemlere ait pdf dosyalarını siliniz.";
                                    IOC.LinkOps.LogOut("x", "//*[@id='navigation']/li[3]/a", "E-BİLDİRGE V2", out msg);
                                    sgkOtomRapor += lblReportHESAP.Text; lblReportHESAP.Text = "<html><ul>"; GlobalVars.ProcessReport = lblReportHESAP.Text;
                                    continue;
                                }
                                #region bm_IPTAL
                                List<SgkHlp> lstIptal = (from x in lstHlp where x.Bm.ToUpper().Contains("PTAL") select x).ToList();
                                lstHlp.RemoveAll(x => x.Bm.ToUpper().Contains("PTAL"));
                                if (lstIptal.Count > 0)
                                {
                                    foreach (SgkHlp h in lstIptal)
                                    {
                                        lstHlp.RemoveAll(x => x.Ads == h.Ads && x.Bt == h.Bt && x.CGun == h.CGun && x.Cn == h.Cn && x.Cx == h.Cx && x.Egn == h.Egn && x.EGun == h.EGun && x.GGun == h.GGun && x.Gun == h.Gun && x.Icn == h.Icn && x.Itl == h.Itl && x.Kk == h.Kk && x.Mk == h.Mk && x.Tcno == h.Tcno && x.Ucg == h.Ucg && x.Utl == h.Utl && x.Ya == h.Ya && (x.Bm == "ASIL" || x.Bm == "asıl" || x.Bm == "asil"));
                                    }
                                }
                                #endregion
                                lblReportHESAP.Text += "</li>";
                                IOC.LinkOps.LogOut("x", "//*[@id='navigation']/li[3]/a", "E-BİLDİRGE V2", out msg);
                                break;
                            case HesapTypeEnum.Tahakkuk:
                                lstThkkDownload.Clear();
                                if (cbHlUnapproved.CheckState == CheckState.Checked)
                                {
                                    HizmetListesiOnayliOnaysiz(false, login, out msg); if (msg == "iptal") { e.Cancel = true; return; } else if (msg == "continue") continue;
                                    Surucu.Driver.Url = "https://ebildirge.sgk.gov.tr/EBildirgeV2/tahakkuk/tahakkukonaylanmisTahakkukDonemBilgileriniYukle.action";
                                    lblReportHESAP.Text += $"</ul></li>";
                                }
                                HizmetListesiOnayliOnaysiz(true, login, out msg); if (msg == "iptal") { e.Cancel = true; return; } else if (msg == "continue") continue;
                                if (lstThkkDownload.Count == 0) { lblReportHESAP.Text += "</ul></li></html>";
                                    sgkOtomRapor += lblReportHESAP.Text; lblReportHESAP.Text = "<html><ul>"; GlobalVars.ProcessReport = lblReportHESAP.Text;
                                    IOC.LinkOps.LogOut("x", "//*[@id='navigation']/li[3]/a", "E-BİLDİRGE V2", out msg); continue; }
                                lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">PDF indirme bitti, indirilen dosyalar çözümleniyor</span></strong></li></ul>";

                                bool thkkError = false;
                                foreach (SgkThkkDownload down in lstThkkDownload)
                                {
                                    SgkThkk thkk = IOC.SgkThkk.ConvertToSgkThkk(down.NewPath, down.LoginId, down.NewPath, out msg);
                                    if (msg.Contains("Hata") || thkk == null)
                                    {
                                        thkkError = true;
                                        lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt; color: red\">{down.PdfId} dosyası çözümlenirken hata oluştu <br>{msg}</span></strong></li>";
                                        break;
                                    }
                                    else
                                    {
                                        lstThkk.Add(thkk);
                                    }
                                }
                                if (thkkError)
                                {
                                    string pdfPath = "Pdf"; lstThkk.RemoveAll(s => s.Cx == login.Id);
                                    lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt; color: red\">PDF okuma hatası: {msg}. Lütfen \"C:\\ProgramData\\SgkAsistan\\{pdfPath}\\{login.Id}\" klasöründeki ilgili dönemlere ait pdf dosyalarını siliniz.</span></strong></li>";
                                    lblMessage.Text = $"PDF okuma hatası: {msg}. Lütfen \"C:\\ProgramData\\SgkAsistan\\{pdfPath}\\{login.Id}\" klasöründeki ilgili dönemlere ait pdf dosyalarını siliniz.";
                                    sgkOtomRapor += lblReportHESAP.Text; lblReportHESAP.Text = "<html><ul>"; GlobalVars.ProcessReport = lblReportHESAP.Text;
                                    IOC.LinkOps.LogOut("x", "//*[@id='navigation']/li[3]/a", "E-BİLDİRGE V2", out msg);
                                    continue;
                                }
                                lblReportHESAP.Text += "</li>";
                                IOC.LinkOps.LogOut("x", "//*[@id='navigation']/li[3]/a", "E-BİLDİRGE V2", out msg);
                                break;
                        }
                        if (bgwSgk.CancellationPending == true) { e.Cancel = true; return; }
                        lblReportHESAP.Text += "</ul></html>"; sgkOtomRapor += lblReportHESAP.Text; lblReportHESAP.Text = "<html><ul>"; GlobalVars.ProcessReport = lblReportHESAP.Text;
                        
                    }
                    #region VeriTabani
                    lblReportHESAP.Text = $"<html><p><p></p><span style=\"font-size: 10pt\"><strong>VERİ TABANINA KAYIT</strong></span><p></p></p>";
                    switch (hesapType)
                    {
                        case HesapTypeEnum.Donemselborc:
                            int resultEkle = 0, resultGuncelle = 0;
                            (resultEkle, resultGuncelle) = IOC.SgkDataService.AddPd(lstDb, out msg);
                            if (resultEkle == -1 && resultGuncelle == -1) { 
                                lblMessage.Text = $"Veri tabanı hatası: {msg}"; 
                                lblReportHESAP.Text += $"<ul><li>Veri tabanı hatası: {msg}</li></ul>";}
                            else if (resultEkle == 0 && resultGuncelle == 0) { lblMessage.Text = $"Veri tabanındaki kayıtlar güncel, yeni kayıt eklenmedi"; lblReportHESAP.Text += $"<ul><li>Veri tabanındaki kayıtlar güncel, yeni kayıt eklenmedi</li></ul>"; }
                            else { lblMessage.Text = $"{resultEkle} adet kayıt veri tabanına eklendi, {resultGuncelle} adet kayıt güncellendi"; lblReportHESAP.Text += $"<ul><li>{resultEkle} adet kayıt veri tabanına eklendi, {resultGuncelle} adet kayıt güncellendi</li></ul>"; }
                            break;
                        case HesapTypeEnum.Emanet:
                            int resultEt = 0; int resultMe = 0;
                            resultEt = IOC.SgkDataService.AddEt(lstEt, out msg);
                            resultMe = IOC.SgkDataService.AddMe(lstMe, out msg);
                            if (resultEt == -1 || resultMe == -1) { 
                                lblMessage.Text = $"Veri tabanı hatası: {msg}"; 
                                lblReportHESAP.Text += $"<ul><li>Veri tabanı hatası: {msg}</li></ul>"; }
                            else { lblMessage.Text = $"{resultEt} adet emanetteki tahsilat ve {resultMe} adet mossip emanet kaydı veri tabanına eklendi/güncellendi"; 
                                lblReportHESAP.Text += $"<ul><li>{resultEt} adet emanetteki tahsilat ve {resultMe} adet mossip emanet kaydı veri tabanına eklendi/güncellendi</li></ul>"; }
                            break;
                        case HesapTypeEnum.Icra:
                            int resultCr = IOC.SgkDataService.AddCr(lstCr, out msg);
                            if (resultCr == -1) { 
                                lblMessage.Text = $"Veri tabanı hatası: {msg}"; 
                                lblReportHESAP.Text += $"<ul><li>Veri tabanı hatası: {msg}</li></ul>"; }
                            else if (resultCr > 0) { lblMessage.Text = $"{resultCr} adet icra kaydı veri tabanına eklendi/güncellendi"; lblReportHESAP.Text += $"<ul><li>{resultCr} adet icra kaydı veri tabanına eklendi/güncellendi</li></ul>"; }
                            break;
                        case HesapTypeEnum.Destek6661:
                            int result6661 = IOC.SgkDataService.Add6661(lst6661, out msg);
                            if (result6661 == -1) { 
                                lblMessage.Text = $"Veri tabanı hatası: {msg}"; 
                                lblReportHESAP.Text += $"<ul><li>Veri tabanı hatası: {msg}</li></ul>"; }
                            else if (result6661 > 0) { lblMessage.Text = $"{result6661} adet 6661 asgari desteği kaydı veri tabanına eklendi/güncellendi"; lblReportHESAP.Text += $"<ul><li>{result6661} adet 6661 asgari desteği kaydı veri tabanına eklendi/güncellendi</li></ul>"; }
                            break;
                        case HesapTypeEnum.Igl:
                            if (lstIgl == null || lstIgl.Count == 0) { lblReportHESAP.Text += "<html><li>Belirtilen tarih aralığında İşe Giriş/İşten Çıkış kaydı yok</li>"; lblMessage.Text = $"Belirtilen tarih aralığında İşe Giriş/İşten Çıkış kaydı yok"; break; }
                            lblReportHESAP.Text += $"<ul><li>İşe Giriş/İşten Çıkış listesi veri tabanına kaydediliyor <span style=\"color:red\">(Lütfen Asistpro'yu kapatmayınız!)</li>";
                            int resultIgl = IOC.SgkDataService.AddIgl(lstIgl, out msg);
                            if (resultIgl == -1) {
                                lblMessage.Text = $"Veri tabanı hatası: {msg}"; 
                                lblReportHESAP.Text += $"<ul><li>Veri tabanı hatası: {msg}</li></ul>"; }
                            else if (resultIgl > 0) { lblMessage.Text = $"{resultIgl} adet işe giriş-çıkış kaydı veri tabanına eklendi/güncellendi"; 
                                lblReportHESAP.Text += $"<li>{resultIgl} adet işe giriş-çıkış kaydı veri tabanına eklendi/güncellendi</li></ul>"; }
                            break;
                        case HesapTypeEnum.Hl:
                        case HesapTypeEnum.Hlo:
                            List<DateTime> tyas = (from x in lstHl select x.Tya).Distinct().ToList();
                            IOC.SgkDataService.RemoveUnapproveds(tyas, out msg);
                            lblReportHESAP.Text += $"<ul><li>PDF çözümleme bitti, veri tabanına kayıt işlemi başladı <span style=\"color:red\">(Bu aşamadan sonra işlem iptal edilemez!)</span><ul>";
                            btnCancelHESAP.Enabled = false;
                            lblReportHESAP.Text += $"<li>Onaylı bildirgeler veri tabanına kaydediliyor</li>";
                            int resultHl = IOC.SgkDataService.AddHl(lstHl, out msg);
                            if (resultHl == -1) { lblMessage.Text = $"Veri tabanı hatası: {msg}"; lblReportHESAP.Text += $"<ul><li>Veri tabanı hatası: {msg}</li></ul>"; }
                            else if (resultHl > 0) { lblReportHESAP.Text += $"<li>{resultHl} adet onaylı bildirge kaydı veri tabanına eklendi/güncellendi</li>"; }

                            lblReportHESAP.Text += $"<li><strong>Hizmet listeleri veri tabanına kaydediliyor <span style=\"color:red\">(Bu işlemin süresi, pdf dosyalarındaki toplam personel sayısına bağlı olarak değişebilir. Lütfen kayıt işlemi bitene kadar Asistpro'yu kapatmayınız.)</strong></span></li><li><strong>%00 kaydedildi";
                            int resultHlp = 0;
                            int sayac = 0;
                            int yuzde = 0;
                            foreach (SgkHlp hlp in lstHlp)
                            {
                                resultHlp += IOC.SgkDataService.AddHlp(hlp, out msg);
                                if (lstHlp.Count > 10000)
                                {
                                    if (sayac % 1000 == 0)
                                    {
                                        lblReportHESAP.Text = lblReportHESAP.Text.Remove(lblReportHESAP.Text.Length - 14, 14);
                                        yuzde = 100 * sayac / lstHlp.Count;
                                        lblReportHESAP.Text += yuzde < 10 ? $"%0{yuzde} kaydedildi" : $"%{yuzde} kaydedildi";
                                    }
                                }
                                else if (lstHlp.Count > 1000)
                                {
                                    if (sayac % 100 == 0)
                                    {
                                        lblReportHESAP.Text = lblReportHESAP.Text.Remove(lblReportHESAP.Text.Length - 14);
                                        yuzde = 100 * sayac / lstHlp.Count;
                                        lblReportHESAP.Text += yuzde < 10 ? $"%0{yuzde} kaydedildi" : $"%{yuzde} kaydedildi";
                                    }
                                }
                                else
                                {
                                    if (sayac % 10 == 0)
                                    {
                                        lblReportHESAP.Text = lblReportHESAP.Text.Remove(lblReportHESAP.Text.Length - 14);
                                        yuzde = 100 * sayac / lstHlp.Count;
                                        lblReportHESAP.Text += yuzde < 10 ? $"%0{yuzde} kaydedildi" : $"%{yuzde} kaydedildi";
                                    }
                                }

                                sayac++;
                            }
                            lblReportHESAP.Text = lblReportHESAP.Text.Remove(lblReportHESAP.Text.Length - 14);
                            lblReportHESAP.Text += $"%100 kaydedildi";
                            lblReportHESAP.Text += $"</li></ul></li><li>Veri tabanına kayıt işlemi bitti</li>";
                            if (resultHlp == -1) { lblMessage.Text = $"Veri tabanı hatası: {msg}"; }
                            else if (resultHlp > 0) { lblReportHESAP.Text += $"<li>{resultHlp} adet hizmet listesi kaydı veri tabanına eklendi/güncellendi</li>"; }
                            break;
                        case HesapTypeEnum.Tahakkuk:
                            lblReportHESAP.Text += $"<ul><li>PDF çözümleme bitti, veri tabanına kayıt işlemi başladı <span style=\"color:red\">(Bu aşamadan sonra işlem iptal edilemez!)</span><ul>";
                            btnCancelHESAP.Enabled = false;
                            lblReportHESAP.Text += $"<li>Tahakkuklar veri tabanına kaydediliyor</li>";
                            int resultThkk = IOC.AccrualDataService.AddThkk(lstThkk, out msg);
                            lblReportHESAP.Text += $"</ul></li><li>Veri tabanına kayıt işlemi bitti</li>";
                            if (resultThkk == -1) { lblMessage.Text = $"Veri tabanı hatası: {msg}"; lblReportHESAP.Text += $"<ul><li>Veri tabanı hatası: {msg}</li></ul>"; }
                            else if (resultThkk > 0) { lblReportHESAP.Text += $"<li>{resultThkk} adet tahakkuk kaydı veri tabanına eklendi/güncellendi</li>"; }
                            break;
                    }
                    lblReportHESAP.Text += $"</html>";
                    sgkOtomRapor += lblReportHESAP.Text.Replace(@"</html><html>", " ");
                    sgkOtomRapor = sgkOtomRapor.Replace(@"</html><html>", " ").Replace("12pt", "18pt").Replace("10pt", "14pt"); 
                    IOC.ExportService.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"{DateTime.Now:dd-MM-yyyy}-{sgkOtomRapor.Substring(47, sgkOtomRapor.IndexOf("</strong>") - 47)}-{DateTime.Now.Ticks}.html").Replace("  ", " ");
                    using (StreamWriter writer = new StreamWriter(IOC.ExportService.FileName))
                    {
                        StringBuilder stringBuilder = new StringBuilder(); stringBuilder.Append(sgkOtomRapor);
                        writer.WriteLine(stringBuilder);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.Contains("timed out after")? "Sayfa yanıt vermiyor, lütfen daha sonra tekrar deneyin ": ex.Message.ToString();
                lblMessage.Text = msg;
            }
        }
        private void BgwSgkComplated(object sender, RunWorkerCompletedEventArgs e)
        {
            string msg;
            if (e.Error != null)
            {
                switch (hesapType)
                {
                    case HesapTypeEnum.Donemselborc:
                        lblMessage.Text = $"Dönemsel borç tarama sırasında hata oluştu! {e.Error.Message}";
                        break;
                    case HesapTypeEnum.Emanet:
                        lblMessage.Text = $"Emanetteki tahsilatları tarama sırasında hata oluştu! {e.Error.Message}";
                        break;
                    case HesapTypeEnum.Icra:
                        lblMessage.Text = $"İcra bilgileri tarama sırasında hata oluştu! {e.Error.Message}";
                        break;
                    case HesapTypeEnum.Destek6661:
                        lblMessage.Text = $"6661 asgari destek tarama sırasında hata oluştu! {e.Error.Message}";
                        break;
                    case HesapTypeEnum.Igl:
                        lblMessage.Text = $"İşegiriş-çıkış tarama sırasında hata oluştu! {e.Error.Message}";
                        break;
                    case HesapTypeEnum.Hl:
                    case HesapTypeEnum.Hlu:
                        lblMessage.Text = $"Hizmet listesi tarama sırasında hata oluştu! {e.Error.Message}";
                        break;
                    case HesapTypeEnum.Tahakkuk:
                        lblMessage.Text = $"Tahakkuk tarama sırasında hata oluştu! {e.Error.Message}";
                        break;
                }
                FreshRgvSgkOtomasyon(); FreshRgvSgkMossip();
            }
            else if (e.Cancelled)
            {
                switch (hesapType)
                {
                    case HesapTypeEnum.Donemselborc:
                        lblMessage.Text = $"Dönemsel borç tarama iptal edildi!";
                        break;
                    case HesapTypeEnum.Emanet:
                        lblMessage.Text = $"Emanetteki tahsilatları tarama iptal edildi!";
                        break;
                    case HesapTypeEnum.Icra:
                        lblMessage.Text = $"İcra bilgileri tarama iptal edildi!";
                        break;
                    case HesapTypeEnum.Destek6661:
                        lblMessage.Text = $"6661 destek sorgulama iptal edildi!";
                        break;
                    case HesapTypeEnum.Igl:
                        lblMessage.Text = $"İşe giriş-çıkış tarama iptal edildi!";
                        break;
                    case HesapTypeEnum.Hl:
                    case HesapTypeEnum.Hlu:
                        lblMessage.Text = $"Hizmet listesi tarama iptal edildi!";
                        break;
                    case HesapTypeEnum.Tahakkuk:
                        lblMessage.Text = $"Tahakkuk tarama iptal edildi!";
                        break;
                }
                GlobalVars.CancelProcess = false;
                FreshRgvSgkOtomasyon(); FreshRgvSgkMossip();
            }
            else
             {
                System.Threading.Thread.CurrentThread.CurrentCulture = GlobalVars.SetCulture();
                switch (hesapType)
                {
                    case HesapTypeEnum.Donemselborc:
                            if(lstDb.Count == 0) { ProcessFinishedHesap(); return;}
                            rgvSgkOtomasyon.SummaryRowsBottom.Clear();
                            rgvSgkOtomasyon.SummaryRowsTop.Clear();
                            FreshRgvSgkOtomasyon();
                            FillRgvSgkDb(lstDb);
                        
                            IOC.SgkAutomations.SetRgvPd(rgvSgkOtomasyon, GlobalVars.DebtLayout);
                            rgvSgkOtomasyon.TitleText = $"Seçilen Firmaların Dönemsel Borç Kayıtları ({lstDb.Count} adet)";
                            
                        break;
                    case HesapTypeEnum.EborcuYoktur:
                        lblMessage.Text = $"PDF dosyaları {SearchReport.UserSelectedDir} dizinine indirildi"; btnOpenFile.Visibility = ElementVisibility.Visible;
                        break;
                    case HesapTypeEnum.Emanet:
                            if (lstEt.Count == 0 && lstMe.Count == 0) { ProcessFinishedHesap(); return; }
                            spRgvMossip.Collapsed = false;
                            spRgvSgkOtomasyon.Collapsed = false;
                            spRgvMossip.Width = spcSgkOtomasyon.Width / 2;
                            spRgvSgkOtomasyon.Width = spcSgkOtomasyon.Width / 2;
                            rgvSgkOtomasyon.SummaryRowsBottom.Clear();
                            rgvSgkOtomasyon.SummaryRowsTop.Clear();
                            FreshRgvSgkOtomasyon(); FreshRgvSgkMossip();
                            FillRgvSgkEt(lstEt); FillRgvSgkMossip(lstMe);
                            IOC.SgkAutomations.SetRgvEt(rgvSgkOtomasyon);
                            rgvSgkOtomasyon.TitleText = $"Seçilen Firmaların Emanetteki Tahsilat Kayıtları ({lstEt.Count} adet)";
                            IOC.SgkAutomations.SetRgvMe(rgvMossip);
                            rgvMossip.TitleText = $"Seçilen Firmaların Mossip Emanette Tahsilat Kayıtları ({lstMe.Count} adet)";
                            
                        break;
                    case HesapTypeEnum.Icra:
                            if (lstCr.Count == 0) { ProcessFinishedHesap(); return; }
                            spRgvMossip.Collapsed = true;
                            rgvSgkOtomasyon.SummaryRowsBottom.Clear();
                            rgvSgkOtomasyon.SummaryRowsTop.Clear();
                            FreshRgvSgkOtomasyon();
                            FillRgvSgkCr(lstCr);
                            IOC.SgkAutomations.SetRgvCr(rgvSgkOtomasyon);
                            rgvSgkOtomasyon.TitleText = $"Seçilen Firmaların İcra Kayıtları ({lstCr.Count} adet)";
                            
                        break;
                    case HesapTypeEnum.Destek6661:
                            if (lst6661.Count == 0) { ProcessFinishedHesap(); return; }
                            spRgvMossip.Collapsed = true;
                            rgvSgkOtomasyon.SummaryRowsBottom.Clear();
                            rgvSgkOtomasyon.SummaryRowsTop.Clear();
                            FreshRgvSgkOtomasyon();
                            FillRgvSgk6661(lst6661);
                            IOC.SgkAutomations.SetRgv6661(rgvSgkOtomasyon);
                            rgvSgkOtomasyon.TitleText = $"Seçilen Firmaların 6661 Asgari Desteği Kayıtları ({lst6661.Count} adet)";
                            
                        break;
                    case HesapTypeEnum.Igl:
                            if (lstIgl.Count == 0) { ProcessFinishedHesap(); rgvSgkOtomasyon.TitleText = $"Belirtilen tarih aralığında  İşe Giriş - İşten Çıkış Kaydı yok"; return; }
                            spRgvMossip.Collapsed = true;
                            rgvSgkOtomasyon.SummaryRowsBottom.Clear();
                            rgvSgkOtomasyon.SummaryRowsTop.Clear();
                            FreshRgvSgkOtomasyon();
                            FillRgvSgkIgl(lstIgl);
                            IOC.SgkAutomations.SetRgvIgl(rgvSgkOtomasyon);
                            int gSay = 0, cSay = 0;
                            if(lstIgl.Count > 0)
                            {
                                gSay = (from x in lstIgl where x.Gc.Contains("r") select x).Count();
                                cSay = (from x in lstIgl where x.Gc.Contains("k") select x).Count();
                            }
                            rgvSgkOtomasyon.TitleText = $"Seçilen Firmaların İşe Giriş - İşten Çıkış Kayıtları ({gSay} adet giriş, {cSay} adet çıkış)";
                            IGL_Colorize();
                            
                        break;
                    case HesapTypeEnum.Hl:
                    case HesapTypeEnum.Hlu:
                    case HesapTypeEnum.Hlo:
                        if (lstHl.Count == 0 && lstHlp.Count == 0) { ProcessFinishedHesap(); FreshRgvSgkOtomasyon(); rgvSgkOtomasyon.TitleText = $"Seçilen Firmaların Belirtilen Tarih Aralığında Hizmet Listesi Kaydı Yok"; return; }
                        //spRgvMossip.Collapsed = false; 
                        spRgvMossip.Collapsed = true;
                        spRgvSgkOtomasyon.Collapsed = false; 
                        spRgvMossip.Width = spcSgkOtomasyon.Width / 2;
                        spRgvSgkOtomasyon.Width = spcSgkOtomasyon.Width / 2;

                        rgvSgkOtomasyon.SummaryRowsBottom.Clear();
                        rgvSgkOtomasyon.SummaryRowsTop.Clear();
                        FreshRgvSgkOtomasyon();
                        FillRgvSgkHl(lstHl);
                        IOC.SgkAutomations.SetRgvHl(rgvSgkOtomasyon);
                        rgvSgkOtomasyon.TitleText = hesapType == HesapTypeEnum.Hl ? $"Seçilen Firmaların Onaylı Bildirge Listeleri ({lstHl.Count} adet)" : $"Seçilen Firmaların Onay Bekleyen Bildirge Listeleri ({lstHl.Count} adet)";

                        rgvMossip.SummaryRowsBottom.Clear();
                        rgvMossip.SummaryRowsTop.Clear();
                        FreshRgvSgkMossip();
                        FillRgvSgkHlp(lstHlp);
                        IOC.SgkAutomations.SetRgvHlp(rgvMossip);
                        rgvMossip.TitleText = hesapType == HesapTypeEnum.Hl ? $"Seçilen Firmaların Hizmet Listesi Kayıtları ({lstHlp.Count} adet)" : $"Seçilen Firmaların Onaylanmamış Hizmet Listesi Kayıtları ({lstHlp.Count} adet)";

                        RgvHelpers.HLP_Colorize(rgvMossip);

                        tempFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\SgkAsistan\Pdftmp";
                        IOC.TrmBase.RemoveTempFiles(tempFolder, out msg);
                        if(hesapType == HesapTypeEnum.Hlu) { lblMessage.Text = $"PDF dosyaları {pdfHluFolder} dizinine indirildi"; btnOpenFile.Visibility = ElementVisibility.Visible; }
                        else { lblMessage.Text = $""; btnOpenFile.Visibility = ElementVisibility.Collapsed; }
                        break;
                    case HesapTypeEnum.Tahakkuk:
                        if (lstThkk.Count == 0) { ProcessFinishedHesap(); FreshRgvSgkOtomasyon(); rgvSgkOtomasyon.TitleText = $"Seçilen Firmaların Belirtilen Tarih Aralığında Tahakkuk Kaydı Yok"; return; }
                        spRgvMossip.Collapsed = true;
                        spRgvSgkOtomasyon.Collapsed = false;
                        spRgvMossip.Width = spcSgkOtomasyon.Width / 2;
                        spRgvSgkOtomasyon.Width = spcSgkOtomasyon.Width / 2;

                        rgvSgkOtomasyon.SummaryRowsBottom.Clear();
                        rgvSgkOtomasyon.SummaryRowsTop.Clear();
                        FreshRgvSgkOtomasyon();
                        FillRgvSgkThkk(lstThkk);
                        IOC.SgkAutomations.SetRgvThkk(rgvSgkOtomasyon);
                        //IOC.SgkAutomations.HideThkkColumn(rgvSgkOtomasyon, lstThkk);
                        rgvSgkOtomasyon.TitleText = $"Seçilen Firmaların Tahakkuk Kayıtları ({lstThkk.Count} adet)";
                        
                        tempFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\SgkAsistan\Pdftmp";
                        IOC.TrmBase.RemoveTempFiles(tempFolder, out msg);
                        
                        break;
                }
                btnOpenFile.Visibility = ElementVisibility.Visible;
                btnOpenFile.Text = (modulType == ModulTypeEnum.HizmetUcretsiz || hesapType == HesapTypeEnum.EborcuYoktur )? "Klasörü Aç" : "Dosyayı Aç";
                if (modulType != ModulTypeEnum.HizmetUcretsiz && hesapType != HesapTypeEnum.EborcuYoktur) lblMessage.Text = "Sorgulama tamamlandı, ayrıntılar için Dosyayı Aç butonuna basınız.";
            }
            
            ProcessFinishedHesap();
        }
        private void BgwSgkChanged(object sender, ProgressChangedEventArgs e)
        {
        }
        private void FillRgvSgkDb(List<SgkDb> borclar)
        {
            if (rgvSgkOtomasyon.InvokeRequired)
            {
                var d = new SafeCallSgkDonemselBorc(FillRgvSgkDb);
                rgvSgkOtomasyon.Invoke(d, new object[] { borclar });
            }
            else
            {
                rgvSgkOtomasyon.DataSource = borclar;
            }
        }
        private void FillRgvSgkEt(List<SgkEt> emnt)
        {
            if (rgvSgkOtomasyon.InvokeRequired)
            {
                var d = new SafeCallSgkEmanet(FillRgvSgkEt);
                rgvSgkOtomasyon.Invoke(d, new object[] { emnt });
            }
            else
            {
                rgvSgkOtomasyon.DataSource = emnt;
            }
        }
        private void FillRgvSgkMossip(List<SgkMe> emnt)
        {
            if (rgvMossip.InvokeRequired)
            {
                var d = new SafeCallSgkMossip(FillRgvSgkMossip);
                rgvMossip.Invoke(d, new object[] { emnt });
            }
            else
            {
                rgvMossip.DataSource = emnt;
            }
        }
        private void FillRgvSgkCr(List<SgkCr> icr) 
        {
            if (rgvSgkOtomasyon.InvokeRequired)
            {
                var d = new SafeCallSgkIcra(FillRgvSgkCr);
                rgvSgkOtomasyon.Invoke(d, new object[] { icr });
            }
            else
            {
                rgvSgkOtomasyon.DataSource = icr;
            }
        }
        private void FillRgvSgk6661(List<Sgk6661> aaab)
        {
            if (rgvSgkOtomasyon.InvokeRequired)
            {
                var d = new SafeCallSgk6661(FillRgvSgk6661);
                rgvSgkOtomasyon.Invoke(d, new object[] { aaab });
            }
            else
            {
                rgvSgkOtomasyon.DataSource = aaab;
            }
        }
        private void FillRgvSgkIgl(List<SgkIgl> igl)
        {
            if (rgvSgkOtomasyon.InvokeRequired)
            {
                var d = new SafeCallSgkIgl(FillRgvSgkIgl);
                rgvSgkOtomasyon.Invoke(d, new object[] { igl });
            }
            else
            {
                rgvSgkOtomasyon.DataSource = igl;
            }
        }
        private void FillRgvSgkHl(List<SgkHl> hl)
        {
            if (rgvSgkOtomasyon.InvokeRequired)
            {
                var d = new SafeCallSgkHl(FillRgvSgkHl);
                rgvSgkOtomasyon.Invoke(d, new object[] { hl });
            }
            else
            {
                rgvSgkOtomasyon.DataSource = hl;
            }
        }
        private void FillRgvLeaves(List<Leaves> leaves)
        {
            if (rgvLeaves.InvokeRequired)
            {
                var d = new SafeCallLeaves(FillRgvLeaves);
                rgvLeaves.Invoke(d, new object[] { leaves });
            }
            else
            {
                rgvLeaves.DataSource = leaves;
            }
        }
        private void FillRgvSgkHlp(List<SgkHlp> hlp)
        {
            if (rgvMossip.InvokeRequired)
            {
                var d = new SafeCallSgkHlp(FillRgvSgkHlp);
                rgvMossip.Invoke(d, new object[] { hlp });
            }
            else
            {
                rgvMossip.DataSource = hlp;
            }
        }
        private void FillRgvSgkThkk(List<SgkThkk> thkk)
        {
            if (rgvSgkOtomasyon.InvokeRequired)
            {
                var d = new SafeCallSgkThkk(FillRgvSgkThkk);
                rgvSgkOtomasyon.Invoke(d, new object[] { thkk });
            }
            else
            {
                rgvSgkOtomasyon.DataSource = thkk;
            }
        }
        private void FreshRgvSgkOtomasyon()
        {
            if (rgvSgkOtomasyon.InvokeRequired)
            {
                var d = new SafeFreshDelegateSgkOtomasyon(FreshRgvSgkOtomasyon);
                rgvSgkOtomasyon.Invoke(d, null);
            }
            else
            {
                rgvSgkOtomasyon.TitleText = "";
                rgvSgkOtomasyon.DataSource = null;
                rgvSgkOtomasyon.Rows.Clear();
                rgvSgkOtomasyon.SummaryRowsBottom.Clear();
                rgvSgkOtomasyon.SummaryRowsTop.Clear();
                rgvSgkOtomasyon.Columns.Clear();
                rgvSgkOtomasyon.Refresh();
            }
        }
        private void FreshRgvSgkMossip()
        {
            if (rgvMossip.InvokeRequired)
            {
                var d = new SafeFreshDelegateSgkMossip(FreshRgvSgkMossip);
                rgvMossip.Invoke(d, null);
            }
            else
            {
                rgvMossip.TitleText = "";
                rgvMossip.DataSource = null;
                rgvMossip.Rows.Clear();
                rgvMossip.SummaryRowsBottom.Clear();
                rgvMossip.SummaryRowsTop.Clear();
                rgvMossip.Columns.Clear();
                rgvMossip.Refresh();
            }
        }
        private void FreshRgvLeaves()
        {
            if (rgvLeaves.InvokeRequired)
            {
                var d = new SafeFreshDelegateSgkOtomasyon(FreshRgvLeaves);
                rgvLeaves.Invoke(d, null);
            }
            else
            {
                rgvLeaves.TitleText = "";
                rgvLeaves.DataSource = null;
                rgvLeaves.Rows.Clear();
                rgvLeaves.SummaryRowsBottom.Clear();
                rgvLeaves.SummaryRowsTop.Clear();
                rgvLeaves.Columns.Clear();
                rgvLeaves.Refresh();
            }
        }
        private void ProcessStartedHesap()
        {
            LinkGlobals.LinkCancel = false;
            GlobalVars.CancelProcess = false;
            IOC.SgkLinksService.LoginBtnClicked = false;
            lblReportHESAP.Text = ""; spRgvMossip.Collapsed = true;
            btnCancelHESAP.Enabled = true;
            rwbHESAP.Text = "İŞLEM DEVAM EDİYOR";
            ribbonBar.Enabled = false;
            rgvCompanyList.Enabled = false;
            pnlWaitHESAP.Visible = true;
            btnOpenFile.Visibility = ElementVisibility.Collapsed;
            rwbHESAP.StartWaiting();
        }
        private void ProcessFinishedHesap()
        {
            ribbonBar.Enabled = true;
            rgvCompanyList.Enabled = true;
            pnlWaitHESAP.Visible = false;
            rwbHESAP.StopWaiting();
            GlobalVars.CancelProcess = false;
            LinkGlobals.LinkCancel = false;
           if (Settings.Default.disposeDriver && Surucu.Driver != null) { Surucu.Driver.Dispose(); Surucu.Driver = null; WinHelpers.ClearAll(); }
            IOC.LinkOps.DisposeProcess();
        }
        private void HizmetListesiOnayliOnaysiz(bool approved, Company login, out string msg)
        {
            msg = ""; string searchType = hesapType == HesapTypeEnum.Tahakkuk ? "tahakkuk" : "onaylı bildirge"; string docType = hesapType == HesapTypeEnum.Tahakkuk ? "tahakkuk" : "bildirge";
            if (approved)
            {
                lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">{CheckUpVars.StartDate:yyyy/MM} - {CheckUpVars.EndDate:yyyy/MM} dönem aralığındaki {searchType} kayıtları aranıyor.</span></strong></li>";
                //List<string> hlTables = new List<string>();
                int startIndex = (DateTime.Now.Month - CheckUpVars.StartDate.Month) + 12 * (DateTime.Now.Year - CheckUpVars.StartDate.Year) + 1;
                startIndex = startIndex == 0 ? 1 : startIndex;
                int finishIndex = (DateTime.Now.Month - CheckUpVars.EndDate.Month) + 12 * (DateTime.Now.Year - CheckUpVars.EndDate.Year) + 1;
                finishIndex = finishIndex == 0 ? 1 : finishIndex;
                //IOC.commonFuncs.ChangeComboBox(startIndex, "hizmet_yil_ay_index", out msg);
                //IOC.commonFuncs.ChangeComboBox(finishIndex, "hizmet_yil_ay_index_bitis", out msg);
                IWebElement startDdl = Surucu.Driver.FindElement(By.Name("hizmet_yil_ay_index"));
                IWebElement endDdl = Surucu.Driver.FindElement(By.Name("hizmet_yil_ay_index_bitis"));
                if (!startDdl.Text.Contains($"{CheckUpVars.StartDate:yyyy/MM}"))
                {
                    string kkc = "";
                    if (login.Kkc < DateTime.Now) kkc = $"(firma {login.Kkc:dd/MM/yyyy} tarihinde kanun kapsamının dışına çıkmış)";
                    lblReportHESAP.Text += $"<li><strong><span style=\"color: red; font-size: 10pt\">Belirtilen tarih aralığı seçilemiyor {kkc}</span></strong></li>" ; 
                    return;
                }
                IOC.CommonFuncs.ChangeComboBox($"{CheckUpVars.StartDate:yyyy/MM}", startDdl, out msg);
                IOC.CommonFuncs.ChangeComboBox($"{CheckUpVars.EndDate:yyyy/MM}", endDdl, out msg);

                IOC.CommonFuncs.ClickWebElement("tahakkukonaylanmisTahakkukDonemSecildi_0", out msg, "i");
            }
            if (Surucu.Driver.PageSource.Replace("  "," ").Contains("Bildirge Bulunamadı"))
            {
                lblReportHESAP.Text += approved ? $"<li><strong><span style=\"font-size: 10pt\">Belirtilen tarih aralığında {searchType} yok</span></strong></li>" : $"<li><strong><span style=\"font-size: 10pt\">Onaylanmamış {docType} yok</span></strong></li>";
                return;
            }
            List<IWebElement> rows = null;
            try
            {
                IWebElement mainTable = Surucu.Driver.FindElement(By.ClassName(Settings.Default.tblHl));
                if (mainTable == null)
                {
                    lblReportHESAP.Text += approved ? $"<li><strong><span style=\"color: red; font-size: 10pt\">{CompanyName} için {searchType} tablosu okunamıyor. {msg}</span></strong></li></ul>" : $"<li><strong><span style=\"color: red; font-size: 10pt\">{CompanyName} için onay bekleyen bildirge listesi tablosu okunamıyor. {msg}</span></strong></li></ul>";
                    msg = "continue";
                    return;
                }
                rows = mainTable.FindElements(By.TagName("tr")).ToList();
                lblReportHESAP.Text += approved ? $"<li><strong><span style=\"font-size: 10pt\">{rows.Count - 2} adet {searchType} bulundu</span></strong></li>" : $"<li><strong><span style=\"font-size: 10pt\">{rows.Count - 1} adet onay bekleyen {docType} bulundu</span></strong></li>";
            }
            catch (NoSuchElementException ex)
            {
                msg = ex.Message;
                lblReportHESAP.Text += approved ? $"<li><strong><span style=\"font-size: 10pt; color: red\">{CompanyName} için {searchType} tablosu okunamıyor." : $"<li><strong><span style=\"font-size: 10pt; color: red\">{CompanyName} için onay bekleyen bildirge listesi tablosu okunamıyor.";
                lblReportHESAP.Text += $"<br>Lütfen internet bağlantınızın kalitesini kontrol edip tekrar deneyin <br>Web Driver hata mesajı: {msg}</span></strong></li></ul>";
                msg = "continue";
                return;
            }

            lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">PDF dosyaları indiriliyor...<ul>";
            List<int> rowNumbers = new List<int>(); int startRow = approved ? 2 : 1;
            for (int i = startRow; i < rows.Count; i++)
            {
                rowNumbers.Add(i);
            }
            hlTryCount = 3;
            bool hlSonuc = hesapType == HesapTypeEnum.Tahakkuk? TahakkukIndir(approved, rows, rowNumbers, login, out msg) :  HizmetListesiIndir(approved, rows, rowNumbers, login, out msg);
            if (!hlSonuc && msg == "iptal") { return; }
            if (!hlSonuc && msg == "indirilemedi")
            {
                lblReportHESAP.Text += approved ? $"<li><strong><span style=\"font-size: 10pt; color: red\">{login.CompanyName} adlı firmaya ait {searchType} PDF dosyaları indirilemedi. Lütfen daha sonra tekrar deneyin</span></strong></li>" : $"<li><strong><span style=\"font-size: 10pt; color: red\">{login.CompanyName} adlı firmaya ait onay bekleyen bildirge listesi PDF dosyaları indirilemedi. Lütfen daha sonra tekrar deneyin</span></strong></li>";
                msg = "continue";
                return;
            }
        }
        private bool HizmetListesiIndir(bool approved, List<IWebElement> rows, List<int> rowNumbers, Company login, out string msg)
        {
            msg = ""; List<int> eksikSatirlar = new List<int>(); string donem = "", pdfid = "", pathPrefix = "", tdNo = ""; int pdfidstart = 0, pdfidlenth = 0, colno = approved ? 1 : 3; IWebElement pdfButton = null, pdfSelectRb = null;
            foreach (int rowNo in rowNumbers)
            {
                if (bgwSgk.CancellationPending == true) { msg = "iptal"; return false; }
                SgkHl hl = new SgkHl(); SgkHlDownload hlDownload = new SgkHlDownload();
                if (approved)
                {
                    tdNo = modulType == ModulTypeEnum.Hizmet ? "10" : "11";
                    pdfButton = rows[rowNo].FindElement(By.XPath($"//*[@id='contentContainer']/div/table/tbody/tr[2]/td/table/tbody/tr[2]/td[2]/div/table/tbody/tr[{rowNo + 1}]/td[{tdNo}]/div/a[2]"));
                    pdfid = pdfButton.GetAttribute("onclick");
                    pdfidstart = pdfid.IndexOf(",") + 2;
                    pdfidlenth = pdfid.IndexOf("')") - pdfidstart;
                    pdfid = pdfid.Substring(pdfidstart, pdfidlenth);
                    pathPrefix = "//*[@id='contentContainer']/div/table/tbody/tr[2]/td/table/tbody/tr[2]/td[2]/div/table/tbody/tr[";
                }
                else
                {
                    pdfSelectRb = rows[rowNo].FindElement(By.XPath($"//*[@id='onayBekleyenTahakkuklarForm']/table/tbody/tr[{rowNo + 1}]/td[1]/p/input"));
                    pdfid = "Onaysiz_" + pdfSelectRb.GetAttribute("value").Trim();
                    pdfSelectRb.Click();
                    pdfButton = Surucu.Driver.FindElement(By.Id("hizmetFisPdfId"));
                    pathPrefix = "//*[@id='onayBekleyenTahakkuklarForm']/table[1]/tbody/tr[";
                }

                string tya = rows[rowNo].FindElement(By.XPath($"{pathPrefix}{rowNo + 1}]/td[{colno}]/p")).Text;
                string hya = rows[rowNo].FindElement(By.XPath($"{pathPrefix}{rowNo + 1}]/td[{colno + 1}]/p")).Text;
                string bt = rows[rowNo].FindElement(By.XPath($"{pathPrefix}{rowNo + 1}]/td[{colno + 2}]/p")).Text;
                string bm = rows[rowNo].FindElement(By.XPath($"{pathPrefix}{rowNo + 1}]/td[{colno + 3}]/p")).Text;
                IWebElement weKn = rows[rowNo].FindElement(By.XPath($"{pathPrefix}{rowNo + 1}]/td[{colno + 4}]/p"));
                string kn = weKn != null ? weKn.Text : " ";
                string tcs = rows[rowNo].FindElement(By.XPath($"{pathPrefix}{rowNo + 1}]/td[{colno + 5}]/p")).Text;
                string tgs = rows[rowNo].FindElement(By.XPath($"{pathPrefix}{rowNo + 1}]/td[{colno + 6}]/p")).Text;
                string tpt = rows[rowNo].FindElement(By.XPath($"{pathPrefix}{rowNo + 1}]/td[{colno + 7}]/p")).Text;
                donem = tya;
                hl.Cx = login.Id;
                hl.Cn = login.CompanyName;
                hl.Un = GlobalVars.ActiveUser;
                hl.Cd = DateTime.Now;
                hl.Tya = tya != "" ? DateTime.Parse(tya) : DateTime.Now;
                hl.Hya = hya != "" ? DateTime.Parse(hya) : DateTime.Now;
                hl.Bt = bt;
                hl.Bm = bm;
                hl.Kn = kn;
                hl.Tcs = tcs != "" ? Convert.ToInt32(tcs) : 0;
                hl.Tgs = tgs != "" ? Convert.ToInt32(tgs) : 0;
                hl.Tpt = tpt != "" ? IOC.AssistantBase.SetPoints(tpt.Replace(" TL", "").Replace(".", "").Replace(",", ".").Replace(" ", "")): 0;
                // Belge mahiyeti İPTAL ise tpt negatif olsun 
                if (bm.ToUpper().Contains("PTAL")) { hl.Tpt *= -1; hl.Tcs *= -1; hl.Tgs *= -1; }
                if (pdfButton == null) { lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt; color: red\">{rowNo}. satır için PDF indirme butonu bulunamadı {msg}</span></strong></li>"; continue; }
                bool hasDownloaded = false;
                // dosya var mı kontrol et. 
                string firmaMainFolder = hesapType == HesapTypeEnum.Hl ? Path.Combine(pdfFolder, login.Id.ToString()) : Path.Combine(pdfHluFolder, login.CompanyName);
                string firmaDonemFolder = Path.Combine(firmaMainFolder, tya.Replace("/", "-"));
                pdfid = $"{login.Id.ToString()}-Hizmet-" + pdfid;
                string newPath = Path.Combine(firmaDonemFolder, $"{pdfid}.pdf");
                hl.PdfPath = newPath;


                if (File.Exists(newPath) && cbHLDeleteRecords.CheckState == CheckState.Unchecked)
                {
                    FileInfo file = new FileInfo(newPath);
                    if (file.Length == 0) { hasDownloaded = false; File.Delete(newPath); eksikSatirlar.Add(rowNo); }
                    else
                    {
                        string kanunNumara = (kn == " ") ? "------" : kn;
                        lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">Dönem: {tya} Belge türü: {bt} Belge mahiyeti: {bm} Kanun no: {kanunNumara}, daha önce indirilmiş</span></strong></li>";
                        hlDownload.NewPath = newPath; hlDownload.LoginId = login.Id; hlDownload.Tcs = hl.Tcs; hlDownload.PdfId = pdfid;
                        lstHl.Add(hl);
                        lstHlDownload.Add(hlDownload);
                        continue;
                    }
                }
                Actions actions = new Actions(Surucu.Driver);
                actions.MoveToElement(pdfButton);
                actions.Perform();
                pdfButton.Click();
                FileInfo[] files = new FileInfo[1];
                for (int j = 0; j < 100; j++)
                {
                    files = new DirectoryInfo(tempFolder).GetFiles("*.pdf");
                    if (files != null && files.Count() > 0)
                    {
                        if (files[0].Length == 0) { hasDownloaded = false; File.Delete(files[0].FullName); eksikSatirlar.Add(rowNo); break; }
                        hasDownloaded = true;
                        string kanunNumara = (kn == " ") ? "-----" : kn;
                        lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">Dönem: {tya} Belge türü: {bt} Belge mahiyeti: {bm} Kanun no: {kanunNumara}, indirildi</span></strong></li>";
                        break;
                    }
                    else { hasDownloaded = false; }
                    Thread.Sleep(500);
                }
                if (!hasDownloaded)
                {
                    lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt; color: red\">{rowNo - 1}. satır için PDF dosyası indirilemedi</span></strong></li>";
                    eksikSatirlar.Add(rowNo);
                    continue;
                }

                string downloadedFileName = files[0].FullName;
                IOC.WinHelpers.TryCreateFolder(firmaMainFolder, false);
                IOC.WinHelpers.TryCreateFolder(firmaDonemFolder, false);

                File.Copy(downloadedFileName, newPath, true);
                File.Delete(downloadedFileName);
                hlDownload.NewPath = newPath; hlDownload.LoginId = login.Id; hlDownload.Tcs = hl.Tcs; hlDownload.PdfId = pdfid;

                Thread.Sleep(1000);
                if (Surucu.Driver.WindowHandles.Count > 1) { Surucu.Driver.SwitchTo().Window(Surucu.Driver.WindowHandles[0]); }
                lstHl.Add(hl);
                lstHlDownload.Add(hlDownload);
            }

            if (eksikSatirlar.Count > 0)
            {
                lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">Asistpro, eksik veya hatalı dosyaları tekrar indirmeyi deniyor.,.</span></strong></li>";
                while (hlTryCount > 0)
                {
                    hlTryCount--;
                    if (eksikSatirlar.Count == 0) break;
                    HizmetListesiIndir(approved, rows, eksikSatirlar, login, out msg);
                }
            }
            if (eksikSatirlar.Count > 0)
            {
                string ekskstrl = "";
                foreach (int item in eksikSatirlar)
                {
                    ekskstrl += item.ToString() + ". ";
                }
                lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt; color: red\">{login.CompanyName}, {donem} dönemi {ekskstrl} satırlardaki pdf dosyaları indirilemedi! Lütfen daha sonra tekrar deneyin.</span></strong></li>";
                msg = "indirilemedi";
                /* bu firmaya ait hl ve hldownload kayıtlarını sil */
                lstHl.RemoveAll(s => s.Cx == login.Id);
                lstHlDownload.RemoveAll(s => s.LoginId == login.Id);
                return false;
            }

            return true;
        }
        private bool TahakkukIndir(bool approved, List<IWebElement> rows, List<int> rowNumbers, Company login, out string msg)
        {
            msg = ""; List<int> eksikSatirlar = new List<int>(); string donem = "", pdfid = "", pathPrefix = ""; int pdfidstart = 0, pdfidlenth = 0, colno = approved ? 1 : 3; IWebElement pdfButton = null, pdfSelectRb = null; ;
            foreach (int rowNo in rowNumbers)
            {
                if (bgwSgk.CancellationPending == true) { msg = "iptal"; return false; }
                SgkThkkDownload thkkDownload = new SgkThkkDownload();
                if (approved) { 
                    pdfButton = rows[rowNo].FindElement(By.XPath($"//*[@id='contentContainer']/div/table/tbody/tr[2]/td/table/tbody/tr[2]/td[2]/div/table/tbody/tr[{rowNo + 1}]/td[9]/div/a[2]"));
                    pdfid = pdfButton.GetAttribute("onclick");
                    pdfidstart = pdfid.IndexOf(",") + 2;
                    pdfidlenth = pdfid.IndexOf("')") - pdfidstart;
                    pdfid = pdfid.Substring(pdfidstart, pdfidlenth);
                    pathPrefix = "//*[@id='contentContainer']/div/table/tbody/tr[2]/td/table/tbody/tr[2]/td[2]/div/table/tbody/tr[";
                } else
                {
                    pdfSelectRb = rows[rowNo].FindElement(By.XPath($"//*[@id='onayBekleyenTahakkuklarForm']/table/tbody/tr[{rowNo + 1}]/td[1]/p/input"));
                    pdfid = "Onaysiz_" + pdfSelectRb.GetAttribute("value").Trim();
                    pdfSelectRb.Click();
                    pdfButton = Surucu.Driver.FindElement(By.Id("tahakkukFisPdfId"));
                    pathPrefix = "//*[@id='onayBekleyenTahakkuklarForm']/table[1]/tbody/tr[";
                }

                string tya = rows[rowNo].FindElement(By.XPath($"{pathPrefix}{rowNo + 1}]/td[{colno}]/p")).Text;
                string hya = rows[rowNo].FindElement(By.XPath($"{pathPrefix}{rowNo + 1}]/td[{colno + 1}]/p")).Text;
                string bt = rows[rowNo].FindElement(By.XPath($"{pathPrefix}{rowNo + 1}]/td[{colno + 2}]/p")).Text;
                string bm = rows[rowNo].FindElement(By.XPath($"{pathPrefix}{rowNo + 1}]/td[{colno + 3}]/p")).Text;
                IWebElement weKn = rows[rowNo].FindElement(By.XPath($"{pathPrefix}{rowNo + 1}]/td[{colno + 4}]/p"));
                string kn = weKn != null ? weKn.Text : " ";
                string tcs = rows[rowNo].FindElement(By.XPath($"{pathPrefix}{rowNo + 1}]/td[{colno + 5}]/p")).Text;
                string tgs = rows[rowNo].FindElement(By.XPath($"{pathPrefix}{rowNo + 1}]/td[{colno + 6}]/p")).Text;
                string tpt = rows[rowNo].FindElement(By.XPath($"{pathPrefix}{rowNo + 1}]/td[{colno + 7}]/p")).Text;
                donem = tya;

                if (pdfButton == null) { lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt; color: red\">{rowNo}. satır için PDF indirme butonu bulunamadı {msg}</span></strong></li>"; continue; }
                bool hasDownloaded = false;
                // dosya var mı kontrol et. 
                string firmaMainFolder = Path.Combine(pdfFolder, login.Id.ToString());
                string firmaDonemFolder = Path.Combine(firmaMainFolder, tya.Replace("/", "-"));
                pdfid = $"{login.Id.ToString()}-Tahakkuk-" + pdfid;
                string newPath = Path.Combine(firmaDonemFolder, $"{pdfid}.pdf");

                if (File.Exists(newPath) && cbHLDeleteRecords.CheckState == CheckState.Unchecked)
                {
                    FileInfo file = new FileInfo(newPath);
                    if (file.Length == 0) { hasDownloaded = false; File.Delete(newPath); eksikSatirlar.Add(rowNo); }
                    else
                    {
                        string kanunNumara = (kn == " ") ? "------" : kn;
                        lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">Dönem: {tya} Belge türü: {bt} Belge mahiyeti: {bm} Kanun no: {kanunNumara}, daha önce indirilmiş</span></strong></li>";
                        thkkDownload.NewPath = newPath; thkkDownload.LoginId = login.Id;  thkkDownload.PdfId = pdfid;
                        lstThkkDownload.Add(thkkDownload);
                        continue;
                    }
                }
                Actions actions = new Actions(Surucu.Driver);
                actions.MoveToElement(pdfButton);
                actions.Perform();
                pdfButton.Click();
                FileInfo[] files = new FileInfo[1];
                for (int j = 0; j < 100; j++)
                {
                    files = new DirectoryInfo(tempFolder).GetFiles("*.pdf");
                    if (files != null && files.Count() > 0)
                    {
                        if (files[0].Length == 0) { hasDownloaded = false; File.Delete(files[0].FullName); eksikSatirlar.Add(rowNo); break; }
                        hasDownloaded = true;
                        string kanunNumara = (kn == " ") ? "-----" : kn;
                        lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">Dönem: {tya} Belge türü: {bt} Belge mahiyeti: {bm} Kanun no: {kanunNumara}, indirildi</span></strong></li>";
                        break;
                    }
                    else { hasDownloaded = false; }
                    Thread.Sleep(500);
                }
                if (!hasDownloaded)
                {
                    lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt; color: red\">{rowNo - 1}. satır için PDF dosyası indirilemedi</span></strong></li>";
                    eksikSatirlar.Add(rowNo);
                    continue;
                }

                string downloadedFileName = files[0].FullName;
                IOC.WinHelpers.TryCreateFolder(firmaMainFolder, false);
                IOC.WinHelpers.TryCreateFolder(firmaDonemFolder, false);

                File.Copy(downloadedFileName, newPath, true);
                File.Delete(downloadedFileName);
                thkkDownload.NewPath = newPath; thkkDownload.LoginId = login.Id;  thkkDownload.PdfId = pdfid;

                Thread.Sleep(1000);
                if (Surucu.Driver.WindowHandles.Count > 1) { Surucu.Driver.SwitchTo().Window(Surucu.Driver.WindowHandles[0]); }
                lstThkkDownload.Add(thkkDownload);
            }
            
            if (eksikSatirlar.Count > 0)
            {
                lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt\">Asistpro, eksik veya hatalı dosyaları tekrar indirmeyi deniyor.,.</span></strong></li>";
                while (hlTryCount > 0)
                {
                    hlTryCount--;
                    if (eksikSatirlar.Count == 0) break;
                    TahakkukIndir(approved, rows, eksikSatirlar, login, out msg);
                }
            }
            if (eksikSatirlar.Count > 0)
            {
                string ekskstrl = "";
                foreach (int item in eksikSatirlar)
                {
                    ekskstrl += item.ToString() + ". ";
                }
                lblReportHESAP.Text += $"<li><strong><span style=\"font-size: 10pt; color: red\">{login.CompanyName}, {donem} dönemi {ekskstrl} satırlardaki pdf dosyaları indirilemedi! Lütfen daha sonra tekrar deneyin.</span></strong></li>";
                msg = "indirilemedi";
                /* bu firmaya ait hl ve hldownload kayıtlarını sil */
                lstThkkDownload.RemoveAll(s => s.LoginId == login.Id);
                return false;
            }
            
            return true;
        }
        #endregion

        #region sgkButtons
        private void rbSelectDebtLayout_Click(object sender, EventArgs e)
        {
            if (rgvSgkOtomasyon.RowCount > 0 && hesapType == HesapTypeEnum.Donemselborc)
            {
                if (sender == rbAllDebts)
                {
                    GlobalVars.DebtLayout = false;
                }
                else if (sender == rbSimpleDebts)
                {
                    GlobalVars.DebtLayout = true;
                }
                IOC.SgkAutomations.ChangePdLayout(rgvSgkOtomasyon, GlobalVars.DebtLayout);
            }
            else if (rgvSgkOtomasyon.RowCount < 0 && hesapType == HesapTypeEnum.Donemselborc)
            {
                if (sender == rbAllDebts)
                {
                    GlobalVars.DebtLayout = false;
                }
                else if (sender == rbSimpleDebts)
                {
                    GlobalVars.DebtLayout = true;
                }
            }
        }
        private void GetHesap(object sender, EventArgs e)
        {
            lstDb.Clear();             lstEt.Clear();            lstMe.Clear();             lstCr.Clear();             lst6661.Clear();     rgvReportList.DataSource = null;

            spRgvMossip.Collapsed = true; IOC.SgkAutomations.FromDb = false;
            if(sender == btnGetET)
            {
                btnPrintEt.Click += btnPrint_Click;
                btnPrintMe.Click += btnPrint_Click;
                btnPrintHESAP.Items.AddRange(btnPrintEt, btnPrintMe);
            }
            else { btnPrintHESAP.Items.Clear(); }

            if (sender == btnGetHL && modulType == ModulTypeEnum.Hizmet)
            {
                btnPrintOnaylibildirge.Click += btnPrint_Click;
                btnPrintHizmetliste.Click += btnPrint_Click;
                btnPrintHLP.Items.AddRange(btnPrintOnaylibildirge, btnPrintHizmetliste);
            }
            else { btnPrintHLP.Items.Clear(); }

            if (sender == btnGetPD)
            {
                hesapType = HesapTypeEnum.Donemselborc;    lstDb.Clear(); //debtAndCredit = DebtAndCreditEnum.Donemselborc;
            }
            else if (sender == btnEborcuYoktur)
            {
                hesapType = HesapTypeEnum.EborcuYoktur;
                FolderBrowserDialog f = new FolderBrowserDialog();
                f.ShowNewFolderButton = true;
                f.Description = "Kayıt konumunu seçin";

                if (f.ShowDialog() == DialogResult.OK)
                    SearchReport.UserSelectedDir = f.SelectedPath;
                else
                    return;
                SearchReport.DownloadDir = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\Downloads\";
            }
            else if (sender == btnGetET)
            {
                hesapType = HesapTypeEnum.Emanet;       lstEt.Clear(); lstMe.Clear();  //debtAndCredit = DebtAndCreditEnum.Emanet;
            }
            else if (sender == btnGetCR)
            {
                hesapType = HesapTypeEnum.Icra;  lstCr.Clear(); //debtAndCredit = DebtAndCreditEnum.Icra;
            }
            else if (sender == btnGet6661)
            {
                hesapType = HesapTypeEnum.Destek6661;  lst6661.Clear();  //debtAndCredit = DebtAndCreditEnum.Destek6661;
            }
            else if (sender == btnGetIGL)
            {
                DayOfWeek day = DateTime.Now.DayOfWeek;
                DateTime simdi = DateTime.Now; 
                if (day >= DayOfWeek.Monday && day <= DayOfWeek.Friday &&  (simdi.Hour >= 8 && simdi.Hour < 12 || simdi.Hour >= 13 && simdi.Hour < 17)) { lblMessage.Text = "SGK'nın bu hizmeti mesai saatleri içinde çalışmamaktadır.(08:00-12:00 13:00-17:00)"; return; }
                CheckUpVars.StartDate = dtpIglFirst.Value; CheckUpVars.EndDate = dtpIglLast.Value;
                if (CheckUpVars.EndDate < CheckUpVars.StartDate) { lblMessage.Text = "Bitiş dönemi başlangıç döneminden küçük olamaz!"; ProcessFinishedHesap(); return; }
                hesapType = HesapTypeEnum.Igl; lstIgl.Clear();
            }
            else if (sender == btnGetHL && hesapType == HesapTypeEnum.Hl)
            {
                lstHl.Clear(); lstHlp.Clear();
            }
            else if (sender == btnGetHL && hesapType == HesapTypeEnum.Tahakkuk)
            {
                lstThkk.Clear();
            }
            else if (sender == btnGetHL && hesapType == HesapTypeEnum.Hlu)
            {
                lstHl.Clear(); lstHlp.Clear();
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Dosyaların indirileceği klasörü seçin";
                fbd.ShowDialog();
                if (fbd.SelectedPath == "") return;
                pdfHluFolder = fbd.SelectedPath;
            }
            
            ProcessStartedHesap();
            if (!bgwSgk.IsBusy)
            {
                bgwSgk.RunWorkerAsync();
            }
        }
        private void GetHesapFromDb(object sender, EventArgs e)
        {
            lstDb.Clear(); lstEt.Clear(); lstMe.Clear(); lstCr.Clear(); lst6661.Clear(); rgvReportList.DataSource = null;
            if (sender == btnGetEtFromDb)
            {
                btnPrintEt.Click += btnPrint_Click;
                btnPrintMe.Click += btnPrint_Click;
                btnPrintHESAP.Items.AddRange(btnPrintEt, btnPrintMe);
            }
            else { btnPrintHESAP.Items.Clear(); }
            if (sender == btnGetHlFromDB)
            {
                btnPrintOnaylibildirge.Click += btnPrint_Click;
                btnPrintHizmetliste.Click += btnPrint_Click;
                btnPrintHLP.Items.AddRange(btnPrintOnaylibildirge, btnPrintHizmetliste);
            }
            else { btnPrintHLP.Items.Clear(); }

            CheckUpVars.StartDate = dtpIglFirst.Value; CheckUpVars.EndDate = dtpIglLast.Value;
            if (CheckUpVars.EndDate < CheckUpVars.StartDate) { lblMessage.Text = "Bitiş dönemi başlangıç döneminden küçük olamaz"; return; }
            IOC.SgkAutomations.FromDb = true;
            lblMessage.Text = "";
            string msg;
            List<int> iynos = IOC.WinHelpers.GetSelectedCompaniesIDs(rgvCompanyList, out msg);
            if (iynos.Count == 0) { lblMessage.Text = "Lütfen, Önce işlem yapılacak firmaları seçin"; return; }
            if (sender == btnGetPDFromDB)
            {
                hesapType = HesapTypeEnum.Donemselborc; /*debtAndCredit = DebtAndCreditEnum.Donemselborc;*/ spRgvMossip.Collapsed = true; spRgvSgkOtomasyon.Collapsed = false;
                lstDb = IOC.SgkDataService.GetAllDb(iynos, out msg);
                if (lstDb == null) { lblMessage.Text = $"Veri tabanı bağlantı hatası: {msg}"; return; }
                if (lstDb.Count == 0) {FreshRgvSgkOtomasyon(); rgvSgkOtomasyon.TitleText = $"Seçilen Firmaların Veri Tabanına Kaydedilmiş Dönemsel Borç Kaydı Yoktur";  return; }
                rgvSgkOtomasyon.SummaryRowsBottom.Clear();
                rgvSgkOtomasyon.SummaryRowsTop.Clear();
                FreshRgvSgkOtomasyon();
                FillRgvSgkDb(lstDb);
                IOC.SgkAutomations.SetRgvPd(rgvSgkOtomasyon, GlobalVars.DebtLayout);
                rgvSgkOtomasyon.TitleText = $"Seçilen Firmaların Dönemsel Borç Kayıtları ({lstDb.Count} adet)";
            }
            else if (sender == btnGetEtFromDb)
            {
                hesapType = HesapTypeEnum.Emanet; //debtAndCredit = DebtAndCreditEnum.Emanet; 
                spRgvSgkOtomasyon.Collapsed = false;
                spRgvMossip.Collapsed = false;
                spRgvMossip.Width = spcSgkOtomasyon.Width / 2;
                spRgvSgkOtomasyon.Width = spcSgkOtomasyon.Width / 2;
                lstEt = IOC.SgkDataService.GetAllEt(iynos, out msg);
                lstMe = IOC.SgkDataService.GetAllMe(iynos, out msg);
                if (lstEt == null && lstMe == null) { lblMessage.Text = $"Veri tabanı bağlantı hatası: {msg}"; return; }
                rgvSgkOtomasyon.SummaryRowsBottom.Clear();
                rgvSgkOtomasyon.SummaryRowsTop.Clear();
                rgvMossip.SummaryRowsBottom.Clear();
                rgvMossip.SummaryRowsTop.Clear();
                if (lstEt.Count > 0 && lstMe.Count > 0)
                {
                    FreshRgvSgkOtomasyon(); FreshRgvSgkMossip();
                    FillRgvSgkEt(lstEt); FillRgvSgkMossip(lstMe);
                    IOC.SgkAutomations.SetRgvEt(rgvSgkOtomasyon);
                    rgvSgkOtomasyon.TitleText = $"Seçilen Firmaların Emanetteki Tahsilat Kayıtları ({lstEt.Count} adet)";
                    IOC.SgkAutomations.SetRgvMe(rgvMossip);
                    rgvMossip.TitleText = $"Seçilen Firmaların Mossip Emanette Tahsilat Kayıtları ({lstMe.Count} adet)";
                }
                else if (lstEt.Count == 0 && lstMe.Count > 0)
                {
                    FreshRgvSgkOtomasyon(); rgvSgkOtomasyon.TitleText = $"Seçilen Firmaların Emanette Tahsilatı Yoktur"; 
                    FillRgvSgkMossip(lstMe); rgvMossip.TitleText = $"Seçilen Firmaların Mossip Emanette Tahsilat Kayıtları ({lstMe.Count} adet)"; IOC.SgkAutomations.SetRgvMe(rgvMossip);
                }
                else if (lstEt.Count > 0 && lstMe.Count == 0)
                {
                    FreshRgvSgkMossip(); rgvMossip.TitleText = $"Seçilen Firmaların Mossip Emanet Tahsilatı Yoktur"; 
                    FillRgvSgkEt(lstEt); rgvSgkOtomasyon.TitleText = $"Seçilen Firmaların Emanetteki Tahsilat Kayıtları ({lstEt.Count} adet)"; IOC.SgkAutomations.SetRgvEt(rgvSgkOtomasyon);
                }
                else
                {

                    FreshRgvSgkMossip(); rgvMossip.TitleText = $"Seçilen Firmaların Veri Tabanına Kaydedilmiş Mossip Emanet Tahsilat Kaydı Yoktur"; 
                    FreshRgvSgkOtomasyon(); rgvSgkOtomasyon.TitleText = $"Seçilen Firmaların Veri Tabanına Kaydedilmiş Emanetteki Tahsilat Kaydı Yoktur"; 
                }
            }
            else if (sender == btnGetCrFromDB )
            {
                hesapType = HesapTypeEnum.Icra; /*debtAndCredit = DebtAndCreditEnum.Icra;*/ spRgvMossip.Collapsed = true; spRgvSgkOtomasyon.Collapsed = false;
                lstCr = IOC.SgkDataService.GetAllCr(iynos, out msg);
                if (lstCr == null) { lblMessage.Text = $"Veri tabanı bağlantı hatası: {msg}"; FreshRgvSgkOtomasyon(); return; }
                if (lstCr.Count == 0) {FreshRgvSgkOtomasyon(); rgvSgkOtomasyon.TitleText = $"Seçilen Firmaların Veri Tabanına Kaydedilmiş İcra Kaydı Yoktur"; return; }
                rgvSgkOtomasyon.SummaryRowsBottom.Clear();
                rgvSgkOtomasyon.SummaryRowsTop.Clear();
                FreshRgvSgkOtomasyon();
                FillRgvSgkCr(lstCr);
                IOC.SgkAutomations.SetRgvCr(rgvSgkOtomasyon);
                rgvSgkOtomasyon.TitleText = $"Seçilen Firmaların İcra Kayıtları ({lstCr.Count} adet)";
            }
            else if (sender == btnGet6661FromDB)
            {
                hesapType = HesapTypeEnum.Destek6661; /*debtAndCredit = DebtAndCreditEnum.Destek6661;*/ spRgvMossip.Collapsed = true; spRgvSgkOtomasyon.Collapsed = false;
                string yil = ddl6661Year.SelectedValue.ToString();
                lst6661 = IOC.SgkDataService.GetAll6661(iynos, yil, out msg);
                if (lst6661 == null) { lblMessage.Text = $"Veri tabanı bağlantı hatası: {msg}"; FreshRgvSgkOtomasyon(); return; }
                if (lst6661.Count == 0) {FreshRgvSgkOtomasyon(); rgvSgkOtomasyon.TitleText = $"Seçilen Firmaların Veri Tabanına Kaydedilmiş 6661 Asgari Destek Kaydı Yoktur"; return; }
                rgvSgkOtomasyon.SummaryRowsBottom.Clear();
                rgvSgkOtomasyon.SummaryRowsTop.Clear();
                FreshRgvSgkOtomasyon();
                FillRgvSgk6661(lst6661);
                IOC.SgkAutomations.SetRgv6661(rgvSgkOtomasyon);
                rgvSgkOtomasyon.TitleText = $"Seçilen Firmaların 6661 Asgari Destek Kayıtları ({lst6661.Count} adet)";
            }
            else if (sender == btnGetIGLFromDB)
            {
                hesapType = HesapTypeEnum.Igl; spRgvMossip.Collapsed = true; spRgvSgkOtomasyon.Collapsed = false;
                CheckUpVars.StartDate = dtpIglFirst.Value; CheckUpVars.EndDate = dtpIglLast.Value;
                if (CheckUpVars.EndDate < CheckUpVars.StartDate) { lblMessage.Text = "Bitiş dönemi başlangıç döneminden küçük olamaz!"; return; }
                lstIgl = IOC.SgkDataService.GetAllIgl(iynos, CheckUpVars.StartDate, CheckUpVars.EndDate, out msg);
                if(lstIgl == null) { lblMessage.Text = $"Veri tabanı bağlantı hatası: {msg}"; FreshRgvSgkOtomasyon(); return; }
                if (lstIgl.Count == 0) {FreshRgvSgkOtomasyon(); rgvSgkOtomasyon.TitleText = $"Seçilen Firmaların Veri Tabanına Kaydedilmiş İşe giriş-Çıkış Kaydı Yoktur"; return; }
                rgvSgkOtomasyon.SummaryRowsBottom.Clear();
                rgvSgkOtomasyon.SummaryRowsTop.Clear();
                FreshRgvSgkOtomasyon();
                FillRgvSgkIgl(lstIgl);
                IOC.SgkAutomations.SetRgvIgl(rgvSgkOtomasyon);
                int gSay = 0, cSay = 0;
                if (lstIgl.Count > 0)
                {
                    gSay = (from x in lstIgl where x.Gc.Contains("r") select x).Count();
                    cSay = (from x in lstIgl where x.Gc.Contains("k") select x).Count();
                }
                rgvSgkOtomasyon.TitleText = $"Seçilen Firmaların İşe Giriş - İşten Çıkış Kayıtları ({gSay} adet giriş, {cSay} adet çıkış)";
                IGL_Colorize();
            }
            else if (sender == btnGetHlFromDB && modulType == ModulTypeEnum.Hizmet)
            {
                hesapType = HesapTypeEnum.Hl; 
                spRgvMossip.Collapsed = true;
                spRgvSgkOtomasyon.Collapsed = false;
                //spRgvMossip.Width = spcSgkOtomasyon.Width / 2;
                //spRgvSgkOtomasyon.Width = spcSgkOtomasyon.Width / 2;
                DdlHlSelectedValueChanged(null, null);
                //CheckUpVars.StartDate = (DateTime)dtpIglFirst.Value; CheckUpVars.EndDate = (DateTime)dtpIglLast.Value;
                lstHl = IOC.SgkDataService.GetAllHl(iynos, CheckUpVars.StartDate, CheckUpVars.EndDate, out msg);
                if(lstHl == null) { lblMessage.Text = $"Veri tabanı bağlantı hatası: {msg}"; FreshRgvSgkOtomasyon(); return; }
                if(lstHl.Count == 0) {FreshRgvSgkOtomasyon(); rgvSgkOtomasyon.TitleText = $"Seçilen Firmaların Belirtilen Dönem Aralığı İçin Veri Tabanına Kaydedilmiş Onaylı Bildirge Yükleme Kaydı Yoktur"; } 
                else
                {
                    rgvSgkOtomasyon.SummaryRowsBottom.Clear();
                    rgvSgkOtomasyon.SummaryRowsTop.Clear();
                    FreshRgvSgkOtomasyon();
                    FillRgvSgkHl(lstHl);
                    IOC.SgkAutomations.SetRgvHl(rgvSgkOtomasyon);
                    rgvSgkOtomasyon.TitleText = $"Seçilen Firmaların Onaylı Bildirge Yükleme Kayıtları ({lstHl.Count} adet)";
                }
                lstHlp = IOC.SgkDataService.GetAllHlp(iynos, CheckUpVars.StartDate, CheckUpVars.EndDate, out msg);
                if (lstHlp == null) { lblMessage.Text = $"Veri tabanı bağlantı hatası: {msg}"; FreshRgvSgkMossip(); return; }
                if (lstHlp.Count == 0) {FreshRgvSgkMossip(); rgvMossip.TitleText = $"Seçilen Firmaların Belirtilen Dönem Aralığı İçin Veri Tabanına Kaydedilmiş Hizmet Listesi Kaydı Yoktur"; return; }
                rgvMossip.SummaryRowsBottom.Clear();
                rgvMossip.SummaryRowsTop.Clear();
                FreshRgvSgkMossip();
                FillRgvSgkHlp(lstHlp);
                IOC.SgkAutomations.SetRgvHlp(rgvMossip);
                rgvMossip.TitleText =  $"Seçilen Firmaların Hizmet Listesi Kayıtları ({lstHlp.Count} adet)";
                RgvHelpers.HLP_Colorize(rgvMossip);
            }
            else if (sender == btnGetHlFromDB && modulType == ModulTypeEnum.Tahakkuk)
            {
                hesapType = HesapTypeEnum.Tahakkuk;
                spRgvMossip.Collapsed = true;
                spRgvSgkOtomasyon.Collapsed = false;
                DdlHlSelectedValueChanged(null, null);
                lstThkk = IOC.AccrualDataService.GetAllThkk(iynos, CheckUpVars.StartDate, CheckUpVars.EndDate, out msg);
                if (lstThkk == null) { lblMessage.Text = $"Veri tabanı bağlantı hatası: {msg}"; FreshRgvSgkOtomasyon(); return; }
                if (lstThkk.Count == 0) {FreshRgvSgkOtomasyon(); rgvSgkOtomasyon.TitleText = $"Seçilen Firmaların Belirtilen Dönem Aralığı İçin Veri Tabanına Kaydedilmiş Tahakkuk Kaydı Yoktur"; }
                else
                {
                    rgvSgkOtomasyon.SummaryRowsBottom.Clear();
                    rgvSgkOtomasyon.SummaryRowsTop.Clear();
                    FreshRgvSgkOtomasyon();
                    FillRgvSgkThkk(lstThkk);
                    IOC.SgkAutomations.SetRgvThkk(rgvSgkOtomasyon);
                    //IOC.SgkAutomations.HideThkkColumn(rgvSgkOtomasyon, lstThkk);
                    rgvSgkOtomasyon.TitleText = $"Seçilen Firmaların Tahakkuk Kayıtları ({lstThkk.Count} adet)";
                }
            }
        }
        public void HLP_Colorize()
        {
            ConditionalFormattingObject cfCalisan = new ConditionalFormattingObject("calisan", ConditionTypes.Equal, "0", "", true);
            cfCalisan.RowBackColor = Settings.Default.hlCalisanColor;
            this.rgvMossip.Columns["GGun"].ConditionalFormattingObjectList.Add(cfCalisan);

            ConditionalFormattingObject cfoGiris = new ConditionalFormattingObject("giris", ConditionTypes.Greater, "0", "", true);
            cfoGiris.RowBackColor = Settings.Default.gGunColor;
            this.rgvMossip.Columns["GGun"].ConditionalFormattingObjectList.Add(cfoGiris);

            ConditionalFormattingObject cfoCikis = new ConditionalFormattingObject("cikis", ConditionTypes.Greater, "0", "", true);
            cfoCikis.RowBackColor = Settings.Default.cGunColor;
            this.rgvMossip.Columns["CGun"].ConditionalFormattingObjectList.Add(cfoCikis);
        }
        public void IGL_Colorize()
        {
            ConditionalFormattingObject cfCikis = new ConditionalFormattingObject("cikis", ConditionTypes.Contains, "k", "", true);
            cfCikis.RowBackColor = Settings.Default.iglCikisColor;
            this.rgvSgkOtomasyon.Columns["Gc"].ConditionalFormattingObjectList.Add(cfCikis);

            ConditionalFormattingObject cfGiris = new ConditionalFormattingObject("giris", ConditionTypes.Contains, "r", "", true);
            cfGiris.RowBackColor = Settings.Default.iglGirisColor;
            this.rgvSgkOtomasyon.Columns["Gc"].ConditionalFormattingObjectList.Add(cfGiris);

        }
        private void btnCancelHESAP_Click(object sender, EventArgs e)
        {
            GlobalVars.CancelProcess = true;
            if (bgwSgk.IsBusy)
            {
                lblReportHESAP.Text += "<li><strong><span style=\"font-size: 10pt\">İPTAL TALEBİ ALINDI! İşlem iptal edilecek, lütfen bekleyiniz.</span></strong></li>";
                rwbHESAP.Text = "İŞLEM İPTAL EDİLİYOR...";
                btnCancelHESAP.Enabled = false;
                bgwSgk.CancelAsync();
            }
        }
        private void btnDeleteHL_Click(object sender, EventArgs e) 
        {
            string msg = "";
            if (ddlHlStartYear.SelectedItem == null) ddlHlStartYear.SelectedIndex = 0;
            int yilS = ((KeyValuePair<string, int>)ddlHlStartYear.SelectedItem.DataBoundItem).Value;
            if (ddlHlStartMount.SelectedItem == null) ddlHlStartMount.SelectedIndex = 0;
            int ayS = ((KeyValuePair<string, int>)ddlHlStartMount.SelectedItem.DataBoundItem).Value;
            CheckUpVars.StartDate = new DateTime(yilS, ayS, 1);

            if (ddlHlEndYear.SelectedItem == null) ddlHlEndYear.SelectedIndex = 0;
            int yilE = ((KeyValuePair<string, int>)ddlHlEndYear.SelectedItem.DataBoundItem).Value;
            if (ddlHlEndMount.SelectedItem == null) ddlHlEndMount.SelectedIndex = 0;
            int ayE = ((KeyValuePair<string, int>)ddlHlEndMount.SelectedItem.DataBoundItem).Value;
            int gun = DateTime.DaysInMonth(yilE, ayE);
            CheckUpVars.EndDate = new DateTime(yilE, ayE, gun);

            string donem = (yilS == yilE && ayS == ayE) ? $"{yilS}/{ayS} dönemine" : $"{yilS}/{ayS} - {yilE}/{ayE} dönem aralığına";
            DialogResult confirmResult = modulType == ModulTypeEnum.Hizmet? 
                RadMessageBox.Show($"Seçilen firmaların {donem} ait hizmet listesi kayıtları veri tabanından silinecek,\r\nDaha önce indirilmiş olan pdf dosyaları da silinecek; devam edilsin mi?", "Hizmet Listelerini Sil", MessageBoxButtons.YesNo) : 
                RadMessageBox.Show($"Seçilen firmaların {donem} ait tahakkuk kayıtları veri tabanından silinecek,\r\nDaha önce indirilmiş olan pdf dosyaları da silinecek; devam edilsin mi?", "Tahakkukları Sil", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                int resDelHl = 0, resDelHlp = 0, resDelThkk = 0;
                List<int> iynos = IOC.WinHelpers.GetSelectedCompaniesIDs(rgvCompanyList, out msg);
                if(modulType == ModulTypeEnum.Hizmet)
                {
                    resDelHl = IOC.SgkDataService.DeleteHLs(iynos, CheckUpVars.StartDate, CheckUpVars.EndDate, out msg);
                    if (resDelHl == -1) { lblMessage.Text = $"Veri tabanı hatası, lütfen daha sonra tekrar deneyiniz. Hata: {msg}"; return; }
                    resDelHlp = IOC.SgkDataService.DeleteHlPs(iynos, CheckUpVars.StartDate, CheckUpVars.EndDate, out msg);
                    if (resDelHlp == -1) { lblMessage.Text = $"Veri tabanı hatası, lütfen daha sonra tekrar deneyiniz. Hata: {msg}"; return; }
                }else if(modulType == ModulTypeEnum.Tahakkuk)
                {
                    resDelThkk = IOC.AccrualDataService.DeleteThkKs(iynos, CheckUpVars.StartDate, CheckUpVars.EndDate, out msg);
                    if (resDelThkk == -1) { lblMessage.Text = $"Veri tabanı hatası, lütfen daha sonra tekrar deneyiniz. Hata: {msg}"; return; }
                }
                
                int resDelFile = 0; int cantDelete = 0;
                foreach (int cx in iynos)
                {
                    for (DateTime s = CheckUpVars.StartDate; s <= CheckUpVars.EndDate; s = s.AddMonths(1))
                    {
                        string ay = s.Month < 10 ? $"0{s.Month}" : s.Month.ToString();
                        tempFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + $@"\SgkAsistan\Pdf\{cx}\{s.Year}-{ay}";
                        DirectoryInfo di = new DirectoryInfo(tempFolder);
                        if (!di.Exists) { continue; }
                        foreach (FileInfo file in di.GetFiles())
                        {
                            try
                            {
                                if(modulType == ModulTypeEnum.Hizmet)
                                {
                                    if (file.Name.Contains("Tahakkuk")) continue;
                                }else if(modulType == ModulTypeEnum.Tahakkuk)
                                {
                                    if (!file.Name.Contains("Tahakkuk")) continue;
                                }
                                file.Delete(); resDelFile++;
                            }
                            catch (Exception)
                            {
                                cantDelete++;
                                continue;
                            }
                        }
                    }
                }
                lblMessage.Text = modulType == ModulTypeEnum.Hizmet? $"{resDelHl} adet onaylı bildirge, {resDelHlp} adet hizmet listesi kaydı ve {resDelFile} adet pdf dosyası silindi." : $"{resDelThkk} adet tahakkuk kaydı ve {resDelFile} adet pdf dosyası silindi.";
            }
        }
        private void btnAnalyzeHL_Click(object sender, EventArgs e)
        {
            IOC.SgkAutomations.LstHlp = lstHlp;
            IOC.SgkAutomations.AnalyzeHl();
        }
        private void btnHlSeperate_Click(object sender, EventArgs e)
        {
            FHlp f = new FHlp(lstHlp, lstIgl);
            f.Show();
        }
        private void btnHlAside_Click(object sender, EventArgs e)
        {
            switch (spRgvMossip.Collapsed)
            {
                case true:
                    spRgvMossip.Collapsed = false;
                    btnHlAside.Text = "Yan Paneli\r\nKapat";
                    break;
                case false:
                    spRgvMossip.Collapsed = true;
                    btnHlAside.Text = "Listeyi Yan\r\nPanelde Aç";
                    break;
            }
        }

        #endregion

        #region SgkOtomasyonRGV

        private void rgvSgkOtomasyon_GroupByChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            if (hesapType == HesapTypeEnum.Donemselborc && e.GridViewTemplate.GroupDescriptors != null && e.GridViewTemplate.GroupDescriptors.Count == 1)
            {
                string exp = e.GridViewTemplate.GroupDescriptors[0].Expression;
                IOC.SgkAutomations.AddSummariesToPd(rgvSgkOtomasyon, exp);
            }
        }
        private void rgvSgkOtomasyon_CommandCellClick(object sender, GridViewCellEventArgs e)
        {
            string location = "";
            int i = e.RowIndex;
            int c = e.ColumnIndex;
            switch (hesapType)
            {
                case HesapTypeEnum.Hl:
                case HesapTypeEnum.Hlu:
                    
                    SgkHl hl = new SgkHl()
                    {
                        Cn = rgvSgkOtomasyon.Rows[i].Cells[0].Value.ToString(),
                        Tya = Convert.ToDateTime(rgvSgkOtomasyon.Rows[i].Cells[1].Value),
                        Hya = Convert.ToDateTime(rgvSgkOtomasyon.Rows[i].Cells[2].Value),
                        Bt = rgvSgkOtomasyon.Rows[i].Cells[3].Value.ToString(),
                        Bm = rgvSgkOtomasyon.Rows[i].Cells[4].Value.ToString(),
                        Kn = rgvSgkOtomasyon.Rows[i].Cells[5].Value == null? "" : rgvSgkOtomasyon.Rows[i].Cells[5].Value.ToString()
                    };
                    if(c == 14)
                    {
                        location = rgvSgkOtomasyon.Rows[i].Cells[9].Value.ToString();
                        Process.Start(location);
                    }
                    else
                    {
                        List<SgkHlp> sgkHlps = (from q in lstHlp where q.Cn == hl.Cn && q.Ya.Year == hl.Tya.Year && q.Ya.Month == hl.Tya.Month && q.Bt == hl.Bt && q.Bm == hl.Bm && q.Kk == hl.Kn select q).ToList(); 
                        FHlp f = new FHlp(sgkHlps, lstIgl);
                        f.Show();
                    }
                    
                    break;
                case HesapTypeEnum.Tahakkuk:
                    location = rgvSgkOtomasyon.Rows[i].Cells[27].Value.ToString();
                    Process.Start(location);
                    break;
            }
        }
        private void spRgvMossip_VisibleChanged(object sender, EventArgs e)
        {
            switch (spRgvMossip.Collapsed)
            {
                case false:
                    btnHlAside.Text = "Yan Paneli\r\nKapat";
                    break;
                case true:
                    btnHlAside.Text = "Listeyi Yan\r\nPanelde Aç";
                    break;
            }
        }
        private void rbHizmetListesi_rbTahakkuk_Click(object sender, EventArgs e)
        {
            if(sender == rbHizmetListesi || sender == rbHizmetListesiUnpaid) {
                modulType = sender == rbHizmetListesi? ModulTypeEnum.Hizmet : ModulTypeEnum.HizmetUcretsiz; 
                hesapType = sender == rbHizmetListesi? HesapTypeEnum.Hl : HesapTypeEnum.Hlu;
                btnGetHL.Text = "<html><p>Sgk'dan Hizmet</p><p>Listesini İndir</p></html>";
                btnGetHlFromDB.Text = "<html><p>İndirilen</p><p>Bildirgeleri Göster</p></html>";
                btnDeleteHL.Text = "<html><p>İndirilen</p><p>Bildirgeleri Sil</p></html>";
                cbHlUnapproved.Text = "Onay bekleyen bildirgeleri de indir";
                btnAnalyzeHL.Enabled = true; 
                btnHlSeperate.Enabled = true;
                btnHlAside.Enabled = true;
            }
            else if (sender == rbTahakkuk)
            {
                modulType = ModulTypeEnum.Tahakkuk; hesapType = HesapTypeEnum.Tahakkuk;
                btnGetHL.Text = "<html><p>Sgk'dan Tahakkukları</p><p>İndir</p></html>";
                btnGetHlFromDB.Text = "<html><p>İndirilen</p><p>Tahakkukları Göster</p></html>";
                btnDeleteHL.Text = "<html><p>İndirilen</p><p>Tahakkukları Sil</p></html>";
                cbHlUnapproved.Text = "Onay bekleyen tahakkukları da indir";
                btnAnalyzeHL.Enabled = false;
                btnHlSeperate.Enabled = false;
                btnHlAside.Enabled = false;
            }

            btnGetHlFromDB.Enabled = sender == rbHizmetListesiUnpaid ? false : true;
            btnDeleteHL.Enabled = sender == rbHizmetListesiUnpaid ? false : true;
        }
        private void rgvSgkOtomasyon_GroupSummaryEvaluate(object sender, GroupSummaryEvaluationEventArgs e)
        {
            //decimal value = 0;
            //foreach (GridViewRowInfo row in this.rgvSgkOtomasyon.Rows)
            //{
            //    if (row.Cells["bm"].Value.ToString() == "İPTAL" || !e.SummaryItem.FormatString.Contains("0:C")) continue;
            //    value += (decimal)row.Cells[e.SummaryItem.Name].Value;
            //}

            //e.Value = value;
        }
        private void rgvCompanyList_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            if (rgvCompanyList.CurrentCell != null && rgvCompanyList.CurrentCell.RowIndex != -1)
            {
                GridViewRowInfo row = rgvCompanyList.CurrentRow;
                GlobalVars.CurrentCompany = IOC.WinHelpers.GetSelectedCompaniesFromRgv(rgvCompanyList, out msg).First();
                if (row.Index >= 0) e.ContextMenu = companyContex.DropDown;
            }
        }
        private void rgvLinkList_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            RadTextBoxEditor editor = this.rgvLinkList.ActiveEditor as RadTextBoxEditor;
            try
            {
                if (editor != null && rgvLinkList.CurrentCell.Value != null)
                {
                    editor.Value = string.Format("{0:c}", int.Parse((string)this.rgvLinkList.CurrentCell.Value));
                }
            }
            catch 
            {

            }
        }

        private void rgvLeaves_RowFormatting(object sender, RowFormattingEventArgs e)
        {
            e.RowElement.DrawFill = true;
            e.RowElement.GradientStyle = GradientStyles.Solid;
            int rowIndex = e.RowElement.RowInfo.Index;
            GridViewHierarchyRowInfo hierarchyRow = e.RowElement.RowInfo.Parent as GridViewHierarchyRowInfo;
            if (rowIndex % 2 == 0 && hierarchyRow == null)
                e.RowElement.BackColor = Settings.Default.leavePersonalOddColor;
            else if(rowIndex % 2 == 1 && hierarchyRow == null)
                e.RowElement.BackColor = Settings.Default.leavePersonalEvenColor;
            else if (rowIndex % 2 == 0 && hierarchyRow != null)
                e.RowElement.BackColor = Settings.Default.leavePeriodOddColor;
            else if (rowIndex % 2 == 1 && hierarchyRow != null)
                e.RowElement.BackColor = Settings.Default.leavePeriodEvenColor;
        }

        private void btnStartSoyad2_Click(object sender, EventArgs e)
        {
            switch (modulType)
            {
                case ModulTypeEnum.Lastname:
                    btnStartProcessSoyad_Click(btnStartSoyad, new EventArgs());
                    break;
                case ModulTypeEnum.Tesvik:
                    btnProcessTesvik_Click(btnProcessTesvik, new EventArgs());
                    break;
            }
        }

        #endregion

        #endregion

        #region options
        private void OptionsButtons_Click(object sender, EventArgs e)
        {
            Form f = new Form();
            if (sender == btnCompanyList)
            {
                f = new FCompanyList();
            }
            else if (sender == btnAddCompany)
            {
                f = new FCompany();
            }
            else if (sender == btnAddCompanies)
            {
                f = new FCompanyAdd();
            }
            else if (sender == btnUsers)
            {
                f = new FUserList();
                
            }
            else if (sender == btnAddUser)
            {
                f = new FUser();
            }
            else if (sender == btnSettings)
            {
                f = new FSettings(ribbonBar);

            }
            else if (sender == btnAbout)
            {
                f = new FAbout();
            }
            Invoke((Action)(() =>
            {
                f.ShowDialog();
            }));
            if (Changed.HasChanged)
            {
                switch (Users.ActiveUser.Yetki)
                {
                    case 1:
                        btnAddCompany.Enabled = true;
                        btnAddCompanies.Enabled = (rgvCompanyList.RowCount > 0) ? true : false;
                        btnCompanyList.Enabled = true;
                        btnAddUser.Enabled = true;
                        break;
                    case 0:

                        btnAddCompany.Enabled = false;
                        btnAddCompanies.Enabled = false;
                        btnCompanyList.Enabled = false;
                        btnAddUser.Enabled = false;
                        break;
                }
            }
        }

        #endregion

        #region tesvik
        private void btnTesvik_Click(object sender, EventArgs e)
        {
            FSinerjiA f = new FSinerjiA();
            f.ShowDialog();
        }
        private void btnProcessTesvik_Click(object sender, EventArgs e)
        {
            LinkGlobals.LinkCancel = false;
            report = String.Empty; listeLastNames.Clear(); GlobalVars.LstInc.Clear();
            ProcessStartedSoyad();
            if (!bgwLinks.IsBusy)
            {
                bgwLinks.RunWorkerAsync(); 
            }
        }
        #endregion

        #region izin
        private void rbLeavesSearchType_Click(object sender, EventArgs e)
        {
            if(sender == rbLeavesTCKN && cbLeavesDATES.CheckState == CheckState.Unchecked) izinType = IzinTypeEnum.Tcnoall;
            else if(sender == rbLeavesTCKN && cbLeavesDATES.CheckState == CheckState.Checked) izinType = IzinTypeEnum.Tcnodate;
            else if(sender == rbLeavesCOMPANY && cbLeavesDATES.CheckState == CheckState.Unchecked) izinType = IzinTypeEnum.Companyall;
            else if(sender == rbLeavesCOMPANY && cbLeavesDATES.CheckState == CheckState.Checked) izinType = IzinTypeEnum.Companydate;
        }
        private void cbLeavesDATES_CheckStateChanged(object sender, EventArgs e)
        {
            if (cbLeavesDATES.CheckState == CheckState.Unchecked && rbLeavesTCKN.CheckState == CheckState.Checked) izinType = IzinTypeEnum.Tcnoall;
            else if (cbLeavesDATES.CheckState == CheckState.Checked && rbLeavesTCKN.CheckState == CheckState.Checked) izinType = IzinTypeEnum.Tcnodate;
            else if (cbLeavesDATES.CheckState == CheckState.Unchecked && rbLeavesCOMPANY.CheckState == CheckState.Checked) izinType = IzinTypeEnum.Companyall;
            else if (cbLeavesDATES.CheckState == CheckState.Checked && rbLeavesCOMPANY.CheckState == CheckState.Checked) izinType = IzinTypeEnum.Companydate;
        }
        private void LeaveButtons_Click(object sender, EventArgs e)
        {
            
            isTcnoValid = false; GlobalVars.Iynos.Clear();
            SearchReport.KimlikNo = texLeaveTckn.Text.Trim();
            string msg, titleText;
            List<Company> lst = IOC.WinHelpers.GetSelectedCompaniesFromRgv(rgvCompanyList, out msg).ToList();
            if (rgvCompanyList.SelectedRows.Count == 1)
            {
                GlobalVars.IzinComp = IOC.WinHelpers.GetSelectedCompaniesFromRgv(rgvCompanyList, out msg).First();
            }
            foreach (Company company in lst)
            {
                GlobalVars.Iynos.Add(company.Id);
            }
            DateTime sd = dtpLeaveFirst.Value.Date;
            DateTime ed = dtpLeaveLast.Value.Date;
            lblMessage.Text = "";
            izinTypeTemp = izinType;
            bool isOk;
            if (sender == btnPersonals)
            {

                izinType = IzinTypeEnum.Personal;
                FPersonalList f = new FPersonalList(GlobalVars.IzinComp);
                f.ShowDialog();
                izinType = izinTypeTemp;
                return;
            }
            else if (sender == btnAddLeave)
            {
                izinType = IzinTypeEnum.Addleave;
                FLeave f = new FLeave(GlobalVars.IzinComp);
                f.ShowDialog();
                izinType = izinTypeTemp;
                return;
            }
            else if (sender == btnAddLeaves)
            {
                izinType = IzinTypeEnum.Addleaves;
                FLeavesAdd f = new FLeavesAdd();
                f.ShowDialog();
                izinType = izinTypeTemp;
                return;
            }
            else if (sender == btnLeaveSearch && izinType == IzinTypeEnum.Tcnoall)
            {
                rgvLeaves.DataSource = null; rgvLeaves.Columns.Clear();
                isTcnoValid = IOC.WinHelpers.IsTcnoValid(SearchReport.KimlikNo, out msg);
                if (!isTcnoValid) { lblMessage.Text = msg; texLeaveTckn.Focus(); return; }

                SearchReport.KimlikNo = texLeaveTckn.Text.Trim();
                (isOk, titleText) = IOC.PersonalService.GetLeavesTcnoAll(out msg);
                if (!isOk) { lblMessage.Text = msg != "" ? $"{msg}" : ""; rgvLeaves.TitleText = titleText; return; }
                lblMessage.Text = ""; rgvLeaves.TitleText = titleText;
            }
            else if (sender == btnLeaveSearch && izinType == IzinTypeEnum.Tcnodate)
            {
                rgvLeaves.DataSource = null; rgvLeaves.Columns.Clear();
                if (ed < sd) { lblMessage.Text = "Başlangıç tarihi, bitiş tarihinden daha ileride!"; return; }
                isTcnoValid = IOC.WinHelpers.IsTcnoValid(SearchReport.KimlikNo, out msg);
                if (!isTcnoValid) { lblMessage.Text = msg; texLeaveTckn.Focus(); return; }
                SearchReport.KimlikNo = texLeaveTckn.Text.Trim();

                (isOk, titleText) = IOC.PersonalService.GetLeavesTcnoAndDate(sd, ed, out msg);
                if (!isOk) { lblMessage.Text = msg != "" ? $"{msg}" : ""; rgvLeaves.TitleText = titleText; return; }
                lblMessage.Text = ""; rgvLeaves.TitleText = titleText;
            }
            else if (sender == btnLeaveSearch && izinType == IzinTypeEnum.Companyall)
            {
                foreach (Company company in lst)
                {
                    if (company.Id == company.Fm) IOC.PersonalService.AskBranchs(company.Id);
                }
                rgvLeaves.DataSource = null; rgvLeaves.Columns.Clear();
                (isOk, titleText) = IOC.PersonalService.GetLeavesCompanyAll(out msg);
                if (!isOk) { lblMessage.Text = msg != "" ? $"{msg}" : ""; rgvLeaves.TitleText = titleText; return; }
                lblMessage.Text = ""; rgvLeaves.TitleText = titleText;

            }
            else if (sender == btnLeaveSearch && izinType == IzinTypeEnum.Companydate)
            {
                foreach (Company company in lst)
                {
                    if (company.Id == company.Fm) IOC.PersonalService.AskBranchs(company.Id);
                }
                rgvLeaves.DataSource = null; rgvLeaves.Columns.Clear();
                if (ed < sd) { lblMessage.Text = "Başlangıç tarihi, bitiş tarihinden daha ileride!"; return; }

                (isOk, titleText) = IOC.PersonalService.GetLeavesByCompaniesAndDate(sd, ed, out msg);
                if (!isOk) { lblMessage.Text = msg != "" ? $"{msg}" : ""; rgvLeaves.TitleText = titleText; return; }
                lblMessage.Text = ""; rgvLeaves.TitleText = titleText;
            }

            if (GlobalVars.PersonalsForLeave.Count > 0) RgvHelpers.SetLeaveListRgv(rgvLeaves, out msg); 
            else rgvLeaves.Columns.Clear(); 
        }
        private void rgvLeaves_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            if (rgvLeaves.CurrentCell != null && rgvLeaves.CurrentCell.RowIndex != -1)
            {
                GridViewRowInfo row = rgvLeaves.CurrentCell.ViewTemplate.MasterViewInfo.CurrentRow;
                if (row == null) return;
                if (row.Cells.Count == 9) izinContex.Items.Remove(showPeriod);
                else izinContex.Items.Add(showPeriod);

                if (row.Index >= 0) e.ContextMenu = izinContex.DropDown;
            }
            
        }
        private void rgvLeaves_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                if (PersonalChange.HasChanged || LeavesChanged.HasChanged)
                {
                    LeaveButtons_Click(btnLeaveSearch, new EventArgs());
                    PersonalChange.HasChanged = false;
                    LeavesChanged.HasChanged = false;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                lblMessage.Text = $"Hata: {msg}";
            }
        }
        private void rgvLeaves_MouseMove(object sender, MouseEventArgs e)
        {
            if (hoverCellLeaves != null && hoverCellLeaves.RowInfo != null)
            {
                hoverCellLeaves.RowInfo.Tag = null;
                hoverCellLeaves.RowInfo.InvalidateRow();
            }

            GridDataCellElement dataCellElement = rgvLeaves.ElementTree.GetElementAtPoint(e.Location) as GridDataCellElement;
            GridSummaryCellElement gridSummaryCell = rgvLeaves.ElementTree.GetElementAtPoint(e.Location) as GridSummaryCellElement;
            if (dataCellElement != null && dataCellElement.RowInfo != null )
            {
                dataCellElement.RowInfo.Tag = "HoverMeFlag";
                hoverCellLeaves = dataCellElement;
                dataCellElement.RowInfo.InvalidateRow();
                GridViewHierarchyRowInfo hierarchyRow = dataCellElement.RowInfo.Parent as GridViewHierarchyRowInfo;
                if (hierarchyRow != null)
                {
                    foreach (GridViewRowInfo row in rgvLeaves.Rows) row.IsSelected = false;
                    
                    hierarchyRow.Tag = "SelectMeFlag";
                    for (int i = 0; i < rgvLeaves.Rows.Count - 1; i++)
                    {
                        if (i == hierarchyRow.Index) continue;
                        rgvLeaves.Rows[i].Tag = "";
                    }
                }
                if (dataCellElement.RowInfo.Cells.Count == 8 && dataCellElement.RowInfo.Index != -1)
                {
                    TimeSpan ts = Convert.ToDateTime(dataCellElement.RowInfo.Cells[4].Value).Subtract(Convert.ToDateTime(dataCellElement.RowInfo.Cells[3].Value));
                    long pid = (from x in GlobalVars.Personals where x.Tcno == dataCellElement.RowInfo.Cells[1].Value.ToString() select x.Id).FirstOrDefault();
                    string tcno = dataCellElement.RowInfo.Cells[1].Value.ToString();
                    DateTime sd = Convert.ToDateTime(dataCellElement.RowInfo.Cells[3].Value);
                    DateTime ed = Convert.ToDateTime(dataCellElement.RowInfo.Cells[4].Value);
                    List<Leaves> leaves = (from q in GlobalVars.Leaves where q.Pid == pid && q.Startdate >= sd && q.Enddate >= sd && q.Startdate <= ed && q.Enddate <= ed select q).ToList();
                    decimal unPaidDays = (from x in leaves where !x.Paid select x.Timeval).Sum();
                    string unPaidString = unPaidDays > 0 ? $"{unPaidDays} gün ücretsiz izin kullanmıştır" : "";
                    if (ts.Days < 365 && dataCellElement.RowInfo.Cells[2].Value.ToString() == "1. DÖNEM") toolTipText = "Personel bir yılını doldurmadığı için izin hak etmemiştir";
                    else if (ts.Days <= 365 && Convert.ToDecimal(dataCellElement.RowInfo.Cells[5].Value) == 0) toolTipText = "Personel bu dönemde izin hak edecek kadar çalışmamıştır";
                    else toolTipText =  $"Personel bu dönemde {Convert.ToDecimal(ts.Days)-unPaidDays} gün çalışıp {Convert.ToDecimal(dataCellElement.RowInfo.Cells[5].Value)} gün izin hak etmiştir. {unPaidString}";
                }
                else if (dataCellElement.RowInfo.Cells.Count == 9 && dataCellElement.RowInfo.Index != -1)
                {
                    toolTipText = dataCellElement.RowInfo.Cells[2].Value.ToString();
                }
            }
        }
        private void rgvLeaves_ToolTipTextNeeded(object sender, ToolTipTextNeededEventArgs e)
        {
            e.Offset = new Size(1, 1);
            e.ToolTipText = toolTipText;
        }

        #endregion
    }
}
