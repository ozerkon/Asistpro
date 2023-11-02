using System.Collections.Generic;

namespace Models.Common
{
    public class Links
    {
        public int Id { get; set; }
        public string Grp { get; set; }
        public string Sgrp { get; set; }
        public string Vers { get; set; }
        public string Head { get; set; }
        public string Url { get; set; }
        public string Vurl { get; set; }
        public string Desc { get; set; }
        public string Tags { get; set; }
        public string Cmd { get; set; }
        public bool Fav { get; set; }
    }
    public static class LinkGlobals
    {
        public static bool HasRefreshButton { get; set; }
        public static string RcbType { get; set; } 
        public static string RcbValue { get; set; }
        public static int BrowserType { get; set; } = 1; // 0: firefox, 1 : chrome
        public static bool IsLink { get; set; }
        public static bool Ivd { get; set; }
        public static bool IvdSurveyBox { get; set; } = true;
        public static bool HasUpdated { get; set; } = false;
        public static bool LinkCancel { get; set; } = false;
        public static int MaxWait { get; set; } = 20;
        public static List<Links> LstLinks { get; set; } = null;
    }
    
}
