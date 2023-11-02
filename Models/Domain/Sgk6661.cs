using Models.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Domain
{
    public class Sgk6661 : SgkAssistantBase
    {
        public string Cn { get; set; }
        public string Yil { get; set; }
        public string Ay { get; set; }
        public int Fgs { get; set; } = 0;
        public decimal Dt { get; set; } = 0;
        public DateTime Tt { get; set; }

        public List<Sgk6661> ConvertToSgk6661(string sdb, int cx)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = GlobalVars.SetCulture();
            List<Sgk6661> lst = new List<Sgk6661>();
            StringReader sr = new StringReader(sdb);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                //line = line.Replace("-,", "-0,").Replace("-.", "-0.").Replace(".", "").Replace(",", ".").Replace(" ", "");
                string[] parts = line.Split('\t');
                parts[4] = parts[4].Substring(0, parts[4].Length - 3);
                Sgk6661 aaab = new Sgk6661() { };
                aaab.Cx = cx;
                aaab.Cn = (from x in GlobalVars.Companies where x.Id == cx select x.CompanyName).FirstOrDefault();
                aaab.Un = GlobalVars.ActiveUser;
                aaab.Cd = DateTime.Now;
                
                aaab.Yil = parts[1] != "" ? parts[1] : "";
                aaab.Ay = parts[2] != "" ? parts[2] : "";
                aaab.Fgs = parts[3] != "" ? Convert.ToInt32(parts[3]) : 0;
                aaab.Dt = parts[4] != "" ? SetPoints(parts[4]) : 0;
                aaab.Tt = parts[5] != "" ? DateTime.ParseExact(parts[5].Replace("/","."), "dd.MM.yyyy", System.Threading.Thread.CurrentThread.CurrentCulture) : DateTime.Now;

                lst.Add(aaab);
            }
            if(lst.Count == 0 ) lst.Add(new Sgk6661() { Cn = (from x in GlobalVars.Companies where x.Id == cx select x.CompanyName).FirstOrDefault() });
            return lst;
        }
    }
}
