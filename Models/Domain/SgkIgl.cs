using Models.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Models.Domain
{
    public class SgkIgl : SgkAssistantBase
    {
        public string Cn { get; set; }
        public string Tc { get; set; }
        public string Ads { get; set; }
        public string Gc { get; set; }
        public DateTime Tr { get; set; }
        public decimal Stn { get; set; }
        public decimal Ipc { get; set; }
        public string Isl { get; set; }
        public DateTime Ist { get; set; }
        public string Isa { get; set; }

        public List<SgkIgl> ConvertToSgkIgl(string sdb, int cx)
        {
            List<SgkIgl> lst = new List<SgkIgl>();
            System.Threading.Thread.CurrentThread.CurrentCulture = GlobalVars.SetCulture();
            StringReader sr = new StringReader(sdb);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split('\t');
                parts[5] = parts[5].Replace("-,", "-0,").Replace("-.", "-0.").Replace(".", "").Replace(",", ".").Replace(" ", "");
                SgkIgl igl = new SgkIgl() { };
                igl.Cx = cx;
                igl.Cn = (from x in GlobalVars.Companies where x.Id == cx select x.CompanyName).FirstOrDefault();
                igl.Un = GlobalVars.ActiveUser;
                igl.Cd = DateTime.Now;

                igl.Tc = parts[0] != "" ? parts[0] : "";
                igl.Ads = parts[1] != "" ? parts[1] : "";
                igl.Gc = parts[2] != "" ? parts[2] : "";
                igl.Tr = parts[3] != "" ? DateTime.ParseExact(parts[3].Replace("/", "."), "dd.MM.yyyy",System.Threading.Thread.CurrentThread.CurrentCulture) : DateTime.Now;
                igl.Stn = parts[4] != "" ? SetPoints(parts[4])  : 0;
                igl.Ipc = parts[5] != "" ? SetPoints(parts[5])  : 0;
                igl.Isl = parts[6] != "" ? parts[6] : "";
                igl.Ist = parts[7] != "" ? DateTime.ParseExact(parts[7].Replace("/", "."), "dd.MM.yyyy", System.Threading.Thread.CurrentThread.CurrentCulture) : DateTime.Now;
                igl.Isa = parts[8] != "" ? parts[8] : "";

                lst.Add(igl);
            }
            if (lst.Count == 0) lst.Add(new SgkIgl() { Cn = (from x in GlobalVars.Companies where x.Id == cx select x.CompanyName).FirstOrDefault() });
            return lst;
        }
    }
      
}