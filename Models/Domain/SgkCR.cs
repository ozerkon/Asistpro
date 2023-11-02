using Models.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Models.Domain
{
    public class SgkCr : SgkAssistantBase
    {
        public string Cn { get; set; }
        public string Kn { get; set; }
        public string Ty { get; set; }
        public string Tn { get; set; }
        public string Bt { get; set; }
        public decimal Ba { get; set; } = 0;
        public decimal Gz { get; set; } = 0;
        public decimal Tm { get; set; } = 0;
        public decimal Tp { get; set; } = 0;

        public List<SgkCr> ConvertToSgkCr(string sdb, int cx)
        {
            List<SgkCr> lst = new List<SgkCr>();
            StringReader sr = new StringReader(sdb);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                line = line.Replace("-,", "-0,").Replace("-.", "-0.").Replace(".", "").Replace(",", ".").Replace(" ", "");
                string[] parts = line.Split('\t');

                SgkCr sgkCr = new SgkCr();

                sgkCr.Cn = (from x in GlobalVars.Companies where x.Id == cx select x.CompanyName).FirstOrDefault();
                sgkCr.Id = 0;
                sgkCr.Kn = parts[0] != "" ? parts[0] : "";
                sgkCr.Ty = parts[1] != "" ? parts[1] : "";
                sgkCr.Tn = parts[2] != "" ? parts[2] : "";
                sgkCr.Bt = parts[3] != "" ? parts[3] : "";
                sgkCr.Ba = parts[4] != "" ? SetPoints(parts[4]) : 0;
                sgkCr.Gz = parts[5] != "" ? SetPoints(parts[5]) : 0;
                sgkCr.Tm = parts[6] != "" ? SetPoints(parts[6]) : 0;
                sgkCr.Tp = parts[7] != "" ? SetPoints(parts[7]) : 0;

                sgkCr.Cx = cx;
                sgkCr.Un = GlobalVars.ActiveUser;
                sgkCr.Cd = DateTime.Now;
                lst.Add(sgkCr);
            }
            if (lst.Count == 0) lst.Add(new SgkCr() { Cn = (from x in GlobalVars.Companies where x.Id == cx select x.CompanyName).FirstOrDefault() });
            return lst;
        }
    }
}
