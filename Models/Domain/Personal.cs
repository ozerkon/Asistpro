using System;

namespace Models.Common
{
    public class Personal
    {
        public long Id { get; set; } 
        public string Tcno  { get; set; }
        public string Ads  { get; set; }
        public int Cid { get; set; } = -1;
        public DateTime Dtr  { get; set; }
        public DateTime Igt  { get; set; }
        public DateTime Ict { get; set; } = DateTime.Now;
        public decimal Tih { get; set; } // toplam izin hakkı
        public bool Active { get; set; } = true;
    }
    public class ExcelPersonal
    {
        public string Tcno { get; set; }
        public string Ads { get; set; }
        public DateTime Dtr { get; set; }
        public DateTime Igt { get; set; }
        public decimal Tih { get; set; }
    }
    public static class PersonalChange
    {
        public static bool HasChanged { get; set; }
        public static bool HasChangedForDialogBox { get; set; }
    }
}
