namespace Models.Common
{
    public class Package
    {
        public int Id { get; set; }
        public string Psc { get; set; }
        public string Pep { get; set; } 
        public string Pps { get; set; } 
        public string Ptr { get; set; } 
        public string Pla { get; set; } 
    }
    public class Sapkt
    {
        public int Id { get; set; } 
        public string Email { get; set; } 
        public string Pass { get; set; } 
        public string Lc { get; set; } 
        public string Sc { get; set; } 
        public string Hc { get; set; } 
        public string Sat { get; set; } 
        public string Ubt { get; set; }
        public bool Active { get; set; } 
        public string Discid { get; set; }
        public string Mac { get; set; }
        public bool Inuse { get; set; }
    }
    public class Demo
    {
        public int Id { get; set; }
        public string Sat { get; set; } = string.Empty;
        public string Ubt { get; set; } = string.Empty;
        public string Discid { get; set; } = string.Empty;
        public string Mac { get; set; } = string.Empty;
        public bool Inuse { get; set; }
    }
    public static class PkcGlobals
    {
        public static bool HasDemoInstalled { get; set; }
        public static bool IsDemoInstalling { get; set; }
        public static string EndDate { get; set; } = string.Empty;
    }
}
