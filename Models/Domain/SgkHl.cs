using Models.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Models.Domain
{
    public class SgkHl : SgkAssistantBase
    {
        public string Cn { get; set; }
        public DateTime Tya { get; set; }
        public DateTime Hya { get; set; }
        public string Bt { get; set; }
        public string Bm { get; set; }
        public string Kn { get; set; }
        public int Tcs { get; set; }
        public int Tgs { get; set; }
        public decimal Tpt { get; set; }
        public string PdfPath { get; set; }
        public List<SgkHl> ConvertToSgkHl(string sdb, List<string> pdfs, int cx)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = GlobalVars.SetCulture();
            List<SgkHl> lst = new List<SgkHl>();

            StringReader sr = new StringReader(sdb);
            string line = "";
            int i = 0;
            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split('\t');
                parts[7] = parts[7].Replace(" TL", "").Replace(".", "").Replace(",", ".").Replace(" ", "");
                SgkHl hl = new SgkHl() { };
                hl.Cx = cx;
                hl.Cn = (from x in GlobalVars.Companies where x.Id == cx select x.CompanyName).FirstOrDefault();
                hl.Un = GlobalVars.ActiveUser;
                hl.Cd = DateTime.Now;

                hl.Tya = parts[0] != "" ? DateTime.Parse(parts[0].Replace("/", ".")) : DateTime.Now;
                hl.Hya = parts[1] != "" ? DateTime.Parse(parts[1].Replace("/", ".")) : DateTime.Now;
                hl.Bt = parts[2] != "" ? parts[2] : "";
                hl.Bm = parts[3] != "" ? parts[3] : "";
                hl.Kn = parts[4] != "" ? parts[4] : "";
                hl.Tcs = parts[5] != "" ? Convert.ToInt32(parts[5]) : 0;
                hl.Tgs = parts[6] != "" ? Convert.ToInt32(parts[6]) : 0;
                hl.Tpt = parts[7] != "" ? SetPoints(parts[7]): 0;
                hl.PdfPath = pdfs[i];
                lst.Add(hl);
                i++;
            }
            if (lst.Count == 0) lst.Add(new SgkHl() { Cn = (from x in GlobalVars.Companies where x.Id == cx select x.CompanyName).FirstOrDefault() });
            return lst;
        }
    }
    public class SgkHlDownload
    {
        public string NewPath { get; set; }
        public int LoginId { get; set; }
        public int Tcs { get; set; }
        public string PdfId { get; set; }
    }
}
