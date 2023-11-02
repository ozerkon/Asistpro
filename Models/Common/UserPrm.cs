using System;

namespace Models.Common
{
    public class UserPrm
    {
        public int Id { get; set; }
        public string Sid { get; set; } = ""; // guid 32char
        public int Fs { get; set; } = 0;
        public DateTime Cd { get; set; } = DateTime.Now;

        #region browserSettings
        public int HideBrowser { get; set; } = 1;
        public int AutoCaptcha { get; set; } = 1;
        public int DefaultBrowser { get; set; } = 1;
        public int KeepBrowser { get; set; } = 0;
        public int CloseAllBrowsers { get; set; } = 0;
        #endregion
        #region companyListSetting
        public int Sira { get; set; } = 1;
        public int Cid { get; set; } = 0;
        public int Cid2 { get; set; } = 0;
        public int Sp { get; set; } = 0;
        public int Cp { get; set; } = 0;
        public int Gun { get; set; } = 0;
        public int Gp { get; set; } = 0;
        public int Gs { get; set; } = 0;
        public int Scd1 { get; set; } = 0;
        public int Scd2 { get; set; } = 0;
        public int Scd3 { get; set; } = 0;
        public int Scd4 { get; set; } = 0;
        public int Scd5 { get; set; } = 0;
        #endregion

    }
    public class Prm
    {

    }
}
