using Models.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Models.Domain
{
    public class SgkEt : SgkAssistantBase
    {
        //Tahsilat Tarihi Dönem Yıl   Dönem Ay    Borç Türü   Tahsilat Tutar
        public string Cn { get; set; }
        public DateTime Tt { get; set; }
        public string Yil { get; set; } = "";
        public string Ay { get; set; } = "";
        public string Bt { get; set; } = "";
        public decimal Ttr { get; set; } = 0;
    }
    public class SgkMe : SgkAssistantBase
    {
        //Bankaya Yatırılma Tarihi	Emanetteki Tahsilat Tutarı	Tahsilat Türü
        public string Cn { get; set; }
        public DateTime Byt { get; set; }
        public decimal Etr { get; set; } = 0;
        public string Tur { get; set; } = "";
    }

    public class EmntMssp : SgkAssistantBase
    {
        public List<SgkEt> ConvertToSgkEt(string sdb, int cx)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = GlobalVars.SetCulture();
            List<SgkEt> lst = new List<SgkEt>();
            StringReader sr = new StringReader(sdb);
            string line = "";
            try
            {
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split('\t');
                    parts[3] = parts[3].Contains("izlik") ? " İşsizlik" : parts[3];
                    parts[4] = parts[4].Replace("-,", "-0,").Replace("-.", "-0.").Replace(".", "").Replace(",", ".").Replace(" ", "");

                    SgkEt sgkEt = new SgkEt();

                    sgkEt.Cn = (from x in GlobalVars.Companies where x.Id == cx select x.CompanyName).FirstOrDefault();
                    sgkEt.Id = 0;
                    sgkEt.Tt = parts[0] != "" ? DateTime.Parse(parts[0].Replace("/", ".")) : DateTime.Now;
                    sgkEt.Yil = parts[1] != "" ? parts[1] : "";
                    sgkEt.Ay = parts[2] != "" ? parts[2] : "";
                    sgkEt.Bt = parts[3] != "" ? parts[3] : "";
                    sgkEt.Ttr = parts[4] != "" ? SetPoints(parts[4]) : 0;

                    sgkEt.Cx = cx;
                    sgkEt.Un = GlobalVars.ActiveUser;
                    sgkEt.Cd = DateTime.Now;
                    lst.Add(sgkEt);
                }
            }
            catch
            {
                return null;
            }
            
            if (lst.Count == 0) lst.Add(new SgkEt() { Cn = (from x in GlobalVars.Companies where x.Id == cx select x.CompanyName).FirstOrDefault() });
            return lst;
        }
        public List<SgkMe> ConvertToSgkMe(string sdb, int cx)
        {
            List<SgkMe> blme = new List<SgkMe>();
            StringReader sr = new StringReader(sdb);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] parts = line.Split('\t');
                //parts[1] = parts[1].Replace(".", "").Replace(",", ".").Replace(" ", "");

                SgkMe me = new SgkMe();
                me.Cn = (from x in GlobalVars.Companies where x.Id == cx select x.CompanyName).FirstOrDefault();
                me.Id = 0;
                me.Byt = parts[0] != "" ? DateTime.Parse(parts[0]) : DateTime.Now;
                me.Etr = parts[1] != "" ? SetPoints(parts[1])  : 0;
                me.Tur = parts[2] != "" ? parts[2] : "";
                                
                me.Cx = cx;
                me.Un = GlobalVars.ActiveUser;
                me.Cd = DateTime.Now;
                blme.Add(me);
            }
            if (blme.Count == 0) blme.Add(new SgkMe() { Cn = (from x in GlobalVars.Companies where x.Id == cx select x.CompanyName).FirstOrDefault() });
            return blme;

        }
    }
}
