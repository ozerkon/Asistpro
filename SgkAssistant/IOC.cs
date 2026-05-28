using DataServices;
using Models.Common;
using Models.Domain;
using SgkAssistant.Helpers;
using SgkAssistant.IncentiveOperations;
using SgkAssistant.LinkOperations;
using SgkAssistant.Properties;
using SGKServices;
using SGKServices.Captcha;

namespace SgkAssistant
{
    public class IOC
    {
        private static DbBase _dbBase;
        private static ICompanyDataService _companyDataService;
        private static IUserDataService _userDataService;
        private static IPersonalDataService _personalDataService;
        private static ILeaveDataService _leaveDataService;
        private static ILeavePeriodDataService _leavePeriodDataService;
        private static ILinksDataService _linksDataService;
        private static IPkcDataService _pkcData;
        private static ISgkDataService _sgkDataService;
        private static IAccrualDataService _accrualDataService;
        private static ICaptchaSolver _solver;

        public static DbBase DBBase
        {
            get
            {
                if (_dbBase == null)
                    _dbBase = DbFactory.CreateDbBase(Settings.Default.dbType);
                return _dbBase;
            }
        }

        public static ICompanyDataService CompanyDataService
        {
            get
            {
                if (_companyDataService == null)
                    _companyDataService = DbFactory.CreateCompanyDataService(Settings.Default.dbType);
                return _companyDataService;
            }
        }

        public static IUserDataService UserDataService
        {
            get
            {
                if (_userDataService == null)
                    _userDataService = DbFactory.CreateUserDataService(Settings.Default.dbType);
                return _userDataService;
            }
        }

        public static IPersonalDataService PersonalDataService
        {
            get
            {
                if (_personalDataService == null)
                    _personalDataService = DbFactory.CreatePersonalDataService(Settings.Default.dbType);
                return _personalDataService;
            }
        }

        public static ILeaveDataService LeaveDataService
        {
            get
            {
                if (_leaveDataService == null)
                    _leaveDataService = DbFactory.CreateLeaveDataService(Settings.Default.dbType);
                return _leaveDataService;
            }
        }

        public static ILeavePeriodDataService LeavePeriodDataService
        {
            get
            {
                if (_leavePeriodDataService == null)
                    _leavePeriodDataService = DbFactory.CreateLeavePeriodDataService(Settings.Default.dbType);
                return _leavePeriodDataService;
            }
        }

        public static ILinksDataService LinksDataService
        {
            get
            {
                if (_linksDataService == null)
                    _linksDataService = DbFactory.CreateLinksDataService(Settings.Default.dbType);
                return _linksDataService;
            }
        }

        public static IPkcDataService PkcData
        {
            get
            {
                if (_pkcData == null)
                    _pkcData = DbFactory.CreatePkcDataService(Settings.Default.dbType);
                return _pkcData;
            }
        }

        public static ISgkDataService SgkDataService
        {
            get
            {
                if (_sgkDataService == null)
                    _sgkDataService = DbFactory.CreateSgkDataService(Settings.Default.dbType);
                return _sgkDataService;
            }
        }

        public static IAccrualDataService AccrualDataService
        {
            get
            {
                if (_accrualDataService == null)
                    _accrualDataService = DbFactory.CreateAccuralDataService(Settings.Default.dbType);
                return _accrualDataService;
            }
        }

        public static ICaptchaSolver Solver
        {
            get
            {
                if (_solver == null)
                    _solver = SolverFactory.CreateSolver(Settings.Default.solverType);
                return _solver;
            }
        }
        public static TrmBase TrmBase = new TrmBase();
        public static WinHelpers WinHelpers = new WinHelpers();
        public static PersonalService PersonalService = new PersonalService();
        public static SgkLinksService SgkLinksService = new SgkLinksService();
        public static VisitReportService VisitReportService = new VisitReportService();
        public static CaptchaService CaptchaService = new CaptchaService();
        public static DesignHelper DesignHelper = new DesignHelper();
        public static CommonFuncs CommonFuncs = new CommonFuncs();
        public static PersonelimDegil PersonelimDegil = new PersonelimDegil();
        public static ExportService ExportService = new ExportService();
        public static DateRange DateRange = new DateRange();
        public static SgkAssistantBase AssistantBase = new SgkAssistantBase();
        public static SgkDb SgkDb = new SgkDb();
        public static SgkAutomations SgkAutomations = new SgkAutomations();
        public static EmntMssp EmanetMossip = new EmntMssp();
        public static SgkCr SgkCr = new SgkCr();
        public static Sgk6661 Sgk6661 = new Sgk6661();
        public static SgkIgl SgkIgl = new SgkIgl();
        public static SgkHl SgkHl = new SgkHl();
        public static SgkHlp SgkHlp = new SgkHlp();
        public static SgkThkk SgkThkk = new SgkThkk();

        public static LinkOps LinkOps = new LinkOps();
        public static IncentiveOps IncentiveOps = new IncentiveOps();
        public static void SetDbBase(int dbType)
        {
            DBBase = DbFactory.CreateDbBase(Settings.Default.dbType);
            CompanyDataService = DbFactory.CreateCompanyDataService(Settings.Default.dbType);
            UserDataService = DbFactory.CreateUserDataService(Settings.Default.dbType);
            PersonalDataService = DbFactory.CreatePersonalDataService(Settings.Default.dbType);
            LeaveDataService = DbFactory.CreateLeaveDataService(Settings.Default.dbType);
            LeavePeriodDataService = DbFactory.CreateLeavePeriodDataService(Settings.Default.dbType);
            LinksDataService = DbFactory.CreateLinksDataService(Settings.Default.dbType);
            PkcData = DbFactory.CreatePkcDataService(Settings.Default.dbType);
            SgkDataService = DbFactory.CreateSgkDataService(Settings.Default.dbType);
            AccrualDataService = DbFactory.CreateAccuralDataService(Settings.Default.dbType);
        }
    }
}
