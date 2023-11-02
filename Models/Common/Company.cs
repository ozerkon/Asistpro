using System;
namespace Models.Common
{
    public class Company
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = String.Empty;
        public string CompanyId { get; set; } = String.Empty;
        public string CompanyId2 { get; set; } = String.Empty;
        public string SystemPassword { get; set; } = String.Empty;
        public string CompanyPassword { get; set; } = String.Empty;
        public int Fm { get; set; } = 1;
        public string Gun { get; set; } = String.Empty;
        public string Gp { get; set; } = String.Empty;
        public string Gs { get; set; } = String.Empty;
        public string Sgsc { get; set; } = String.Empty;
        public string Unvan { get; set; } = String.Empty;
        public string Adres { get; set; } = String.Empty;
        public string Sgm { get; set; } = String.Empty;
        public DateTime Kka { get; set; } = new DateTime(1923, 10, 29, 0 ,0 ,0);
        public DateTime Kkc { get; set; } = new DateTime(2123, 1, 1);
        public string Sc1 { get; set; } = String.Empty;
        public string Sc2 { get; set; } = String.Empty;
        public string Sc3 { get; set; } = String.Empty;
        public string Sc4 { get; set; } = String.Empty;
        public string Sc5 { get; set; } = String.Empty;
        public int Cu { get; set; } = 1;
        public DateTime Cd { get; set; } = DateTime.Now;

        public bool IsValid()
        {
            if (this.CompanyId.Trim().Length != 11) { return false; }
            if (this.CompanyId2.Trim().Length == 0) { return false; }
            if (this.SystemPassword.Trim().Length < 1) { return false; }//4
            if (this.CompanyPassword.Trim().Length < 2) { return false; }//4
            return true;
        }
    }
    public static class Changed
    {
        public static bool HasChanged { get; set; } = false;
        public static bool HasChangedForDialogBox { get; set; } = false;
        public static string FName { get; set; }
    }
    public class ExcelComp
    {
        public string CompanyName { get; set; } = String.Empty;
        public string CompanyId { get; set; } = String.Empty;
        public string CompanyId2 { get; set; } = String.Empty;
        public string SystemPassword { get; set; } = String.Empty;
        public string CompanyPassword { get; set; } = String.Empty;
        public string Gun { get; set; } = String.Empty;
        public string Gp { get; set; } = String.Empty;
        public string Gs { get; set; } = String.Empty;
        public string Sc1 { get; set; } = String.Empty;
        public string Sc2 { get; set; } = String.Empty;
        public string Sc3 { get; set; } = String.Empty;
        public string Sc4 { get; set; } = String.Empty;
        public string Sc5 { get; set; } = String.Empty;
    }

}
