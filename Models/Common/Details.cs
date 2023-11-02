using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Common
{
    public class SourceIgb{
        public string Col1 { get; set; }
        public string Col2 { get; set; }
        public string Col3 { get; set; }
        public string Col4 { get; set; }
    }
    public class SourceSpvudk
    {
        public string Col1 { get; set; }
        public string Col2 { get; set; }
        public string Col3 { get; set; }
        public string Col4 { get; set; }
    }
    public class SourceSraod
    {
        public string Col1 { get; set; }
        public string Col2 { get; set; }
        public string Col3 { get; set; }
        public string Col4 { get; set; }
        public string Col5 { get; set; }
    }

    public static class DetailsNameTc
    {
        public static string Name { get; set; }
        public static int SraodCount { get; set; }
        public static string Tcno { get; set; }
    }
}
