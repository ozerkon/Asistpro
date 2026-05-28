using System;

namespace SgkAssistant.Helpers
{
    public static class PackageHelper
    {
        #region properties
        public static int Ay { get; set; } = 0;
        public static int Gun { get; set; } = 0;
        public static int Mcc { get; set; } = 0;
        public static string Sat { get; set; }
        public static string Ubt { get; set; }
        #endregion

        #region methods

        public static string DecryptHc(string hc)
        {
            if (string.IsNullOrEmpty(hc) || hc.Length < 22)
            {
                throw new ArgumentException("Invalid HC format");
            }
            string sc = "";
            for (int i = 0; i <= 16; i += 4)
            {
                if (i + 4 <= hc.Length)
                {
                    sc += DecryptFourHex(hc.Substring(i, 4));
                }
            }
            sc += hc.Substring(20, 2);

            switch (sc.Substring(2, 2))
            {
                case "13":
                    Mcc = 10;
                    break;
                case "15":
                    Mcc = 20;
                    break;
                case "27":
                    Mcc = 30;
                    break;
                case "39":
                    Mcc = 40;
                    break;
                case "51":
                    Mcc = 50;
                    break;
                case "63":
                    Mcc = 100;
                    break;
                case "75":
                    Mcc = 250;
                    break;
                case "87":
                    Mcc = 500;
                    break;
                case "99":
                    Mcc = 50000;
                    break;
                default:
                    break;
            }

            Ay = Convert.ToInt32(sc.Substring(4, 2));
            string gunFromHc = sc.Substring(9, 1) + sc.Substring(13, 1) + sc.Substring(17, 1) + sc.Substring(21, 1);
            if (!int.TryParse(gunFromHc, out int gunValue))
            {
                throw new FormatException($"Invalid Gun format: {gunFromHc}");
            }
            Gun = gunValue;
            return sc;
        }
        private static string DecryptFourHex(string s)
        {
            string h = "ABCDEF0123456789";
            int x = -1;
            if (!h.Contains(s.Substring(0, 1)) && !h.Contains(s.Substring(1, 1)))
            {
                x = int.Parse(s.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            }
            else if (!h.Contains(s.Substring(0, 1)))
            {
                x = int.Parse(s.Substring(1, 3), System.Globalization.NumberStyles.HexNumber);
            }
            else
            {
                x = int.Parse(s, System.Globalization.NumberStyles.HexNumber);
            }
            if (x >= 1000)
            {
                return x.ToString();
            }
            else if (x >= 100 && x < 1000)
            {
                return "0" + x.ToString();
            }
            else
            {
                return "00" + x.ToString();
            }
        }
        
        #endregion
    }
}

