using System;
using System.Collections.Generic;

namespace Models.Common
{
    public class SgkIsci : ICloneable
    {
        public long Cx { get; set; } = 0;
        public string Cnm { get; set; } = string.Empty;   //firmaadi
        public string Onm { get; set; } = string.Empty;   //unvan
        public string FirmaSicilNo { get; set; } = string.Empty;

        public string Sgksicilno { get; set; } = string.Empty; public string ShowSgksicilno(bool isVisible) { return isVisible ? Sgksicilno : "******"; }
        public string Tcno { get; set; } = string.Empty; public string ShowTcno(bool isVisible) { return isVisible ? Tcno : "******"; }

        public string Cinsiyet { get; set; } = string.Empty;
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string IlkSoyad { get; set; } = string.Empty;

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    
    public class Visit : SgkIsci
    {
        public string AdSoyad { get; set; }
        public string Vaka { get; set; }
        public string RaporTakipNo { get; set; }
        public int RaporSiraNo { get; set; }
        public DateTime RaporBaslamaTarihi { get; set; }
        public DateTime RaporBitisTarihi { get; set; }
        public DateTime IsBasiKontrolTarihi { get; set; }
        public string CezaDurumu { get; set; }
        public string Aciklama { get; set; } = string.Empty;
        public DateTime PoliklinikTarihi { get; set; }
    }

    public class VisitsToBeProcessed : Visit
    {
        public int WorkingStatus { get; set; } = 0;
        public string ConfirmDate { get; set; }
        public string CancelConfirm { get; set; } 
    }
    public static class SearchReport
    {
        public static bool IsMultiSearch { get; set; } = true;
        public static string KimlikNo { get; set; }
        public static DateTime SearchDate { get; set; } = DateTime.Now;
        public static DateTime StartDate { get; set; } = DateTime.Now;
        public static DateTime EndDate { get; set; } = DateTime.Now;
        public static int ReportType { get; set; } = 2;
        public static int ReportTypeAfterRgvLoad { get; set; }
        public static int CaseType { get; set; } = 1;
        public static string CompanyNameSearching { get; set; } = string.Empty;
        public static string LastCaptchaPath { get; set; } = string.Empty;
        public static string LoginMessage { get; set; } = string.Empty;
        public static int ProcessType { get; set; } = 1;
        public static bool ConfirmError { get; set; } = false;
        public static bool GetConfirmPdf { get; set; } = false;

        public static string DownloadDir =  $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\Downloads\";
        public static string UserSelectedDir =  "";
        public static string DocumentsDir = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\";
    }

    public class ProcessReport
    {
        public string Tcid { get; set; }
        public string Fullname { get; set; }
        public string Vaka { get; set; }
        public DateTime Rbat { get; set; }
        public DateTime Rbit { get; set; }
        public string Rtno { get; set; }
        public int Rsno { get; set; }
        public string Rslt { get; set; }
        public DateTime Onyt { get; set; }
        
    }
    public class ConfirmReport : ProcessReport
    {
        public string Pdffile { get; set; } = string.Empty;
    }
    public class IseGiriscikis
    {
        public DateTime IslemTarihi { get; set; }
        public char IslemTuru { get; set; }
    }
}
