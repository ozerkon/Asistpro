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
        public static DbBase DBBase = DbFactory.CreateDbBase(Settings.Default.dbType);
        public static ICompanyDataService CompanyDataService = DbFactory.CreateCompanyDataService(Settings.Default.dbType);
        public static IUserDataService UserDataService = DbFactory.CreateUserDataService(Settings.Default.dbType);
        public static IPersonalDataService PersonalDataService = DbFactory.CreatePersonalDataService(Settings.Default.dbType);
        public static ILeaveDataService LeaveDataService = DbFactory.CreateLeaveDataService(Settings.Default.dbType);
        public static ILeavePeriodDataService LeavePeriodDataService = DbFactory.CreateLeavePeriodDataService(Settings.Default.dbType);
        public static ILinksDataService LinksDataService = DbFactory.CreateLinksDataService(Settings.Default.dbType);
        public static IPkcDataService PkcData = DbFactory.CreatePkcDataService(Settings.Default.dbType);
        public static ISgkDataService SgkDataService = DbFactory.CreateSgkDataService(Settings.Default.dbType);
        public static IAccrualDataService AccrualDataService = DbFactory.CreateAccuralDataService(Settings.Default.dbType);
        public static ICaptchaSolver Solver = SolverFactory.CreateSolver(Settings.Default.solverType);
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
