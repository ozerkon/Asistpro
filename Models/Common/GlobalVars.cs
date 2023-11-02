using Models.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Models.Common
{
    public static class GlobalVars
    {
        public static List<Company> Companies { get; set; }
        public static List<Personal> Personals { get; set; }
        public static List<Personal> PersonalsForLeave { get; set; } 
        public static List<Leaves> Leaves { get; set; }
        public static List<Leaves> LeavesForDialog { get; set; }
        public static List<LeavePeriod> LeavePeriods { get; set; }
        public static Company IzinComp { get; set; }
        public static List<int> Iynos { get; set; } = new List<int>();
        public static Company CurrentCompany { get; set; }
        public static bool IsCompanyActive = true;
        public static bool HesapTypeEtDbCr = false;
        public static bool DebtLayout { get; set; } = true;
        public static int ActiveUser { get; set; } = 1;
        public static byte DbType { get; set; } = 1;
        public static string ErrorReport { get; set; } = "";
        public static string ProcessReport { get; set; } = "";
        public static List<PublicHolidays> PublicHolidays { get; set; }
        public static bool DisposeDriver { get; set; } = true;
        public static UserPrm UserPrm { get; set; }
        public static List<SgkDb> LstDb { get; set; } = new List<SgkDb>();
        public static List<SgkEt> LstEt { get; set; } = new List<SgkEt>();
        public static List<SgkMe> LstMe { get; set; } = new List<SgkMe>();
        public static List<SgkCr> LstCr { get; set; } = new List<SgkCr>();
        public static List<Sgk6661> Lst6661 { get; set; } = new List<Sgk6661>();
        public static List<SgkHlp> LstHlp { get; set; } = new List<SgkHlp>();
        public static List<SgkIgl> LstIgl { get; set; } = new List<SgkIgl>();
        public static List<Tesvik> LstTsvk { get; set; } = new List<Tesvik>();
        public static List<Incentive> LstInc { get; set; } = new List<Incentive>();
        public static List<SgkThkk> LstThkk { get; set; } = new List<SgkThkk>();
        public static bool CancelProcess { get; set; } = false;
        public static string SgscEnc { get; set; } 
        public static string CmpCheck { get; set; }
        public static string PassPhrase { get; set; } = "9Pp_A2GN5^fcTQ?*$8^tUGxyPra3WUa7";
        public static byte SolverType { get; set; } = 2; // 1: imagetyperz, 2: free ocr api
        public static string SolverKey { get; set; } = "K82964397288957";
        public static int OcrEngine { get; set; } = 1;
        public static int ActiveTab { get; set; } = 0;
        public static bool AutoCaptcha { get; set; }
        public static int ImageWidth { get; set; } = 0;
        public static bool ChromeHiddenError { get; set; }
        public static bool NewVersionfound { get; set; } = false;
        public static DateTime Ct { get; set; } // current time
        public static DateTime LastLoginDate { get; set; } 
        public static CultureInfo SetCulture()
        {
            CultureInfo trTr = new CultureInfo("tr-TR");
            trTr.DateTimeFormat = new CultureInfo("tr-TR").DateTimeFormat; //CultureInfo.InvariantCulture.DateTimeFormat;
            trTr.DateTimeFormat.DateSeparator = "/";
            trTr.NumberFormat.CurrencyDecimalDigits = 2;
            trTr.NumberFormat.CurrencyDecimalSeparator = ",";
            trTr.NumberFormat.CurrencyGroupSeparator = ".";
            return trTr;
        }

        private static string MsqcsLocal { get; set; }
        public static void SetMsqcsLocal(string sv, string db, string un, string up, string pr)
        {
            MsqcsLocal = $"Server='{sv}';Database='{db}';Uid='{un}';Pwd='{up}';Port={pr};";
        }
        public static string GetMsqcsLocal()
        {
            return MsqcsLocal;
        }
        
        private static string LiteDbCs { get; set; }
        public static void SetLiteDbCs(bool pw, string pwd = "")
        {
            if (!pw)
            {
                LiteDbCs = $@"Filename={Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\SgkAsistan\Data\sgkasistan.db;Connection=shared;";
            }
            else
            {
                LiteDbCs = $@"Filename={Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\SgkAsistan\Data\sgkasistan.db;Password={pwd}Connection=shared;";
            }
        }
        public static string GetliteDbCs()
        {
            return LiteDbCs;
        }
        
        private static string MsqcsRemote { get; set; }
        public static void SetMsqcsRemote(string sv, string db, string un, string up, string pr)
        {
            MsqcsRemote = $"Server='{sv}';Database='{db}';Uid='{un}';Pwd='{up}';Port={pr};";
        }
        public static string GetMsqcsRemote()
        {
            return MsqcsRemote;
        }
    }

}
