using Models.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Models.Domain
{
    public class SgkDb : SgkAssistantBase
    {
        public string Cn { get; set; }
        public string Yil { get; set; } = "";
        public string Ay { get; set; } = "";
        public string Drm { get; set; } = "";
        public decimal Pb { get; set; } = 0;
        public decimal PbGz { get; set; } = 0;
        public decimal Ipcb { get; set; } = 0;
        public decimal IpcbGz { get; set; } = 0;
        public decimal Ekpb { get; set; } = 0;
        public decimal EkpbGz { get; set; } = 0;
        public decimal Oivb { get; set; } = 0;
        public decimal OivbGz { get; set; } = 0;
        public decimal Ib { get; set; } = 0;
        public decimal IbGz { get; set; } = 0;
        public decimal Dvb { get; set; } = 0;
        public decimal DvbGz { get; set; } = 0;
        public decimal Dpgzb { get; set; } = 0;
        public decimal DpgzbGz { get; set; } = 0;
        public decimal Dekpgzb { get; set; } = 0;
        public decimal Sbt { get; set; } = 0;
        public decimal SbtGz { get; set; } = 0;

        public List<SgkDb> ConvertToSgkDb(string sdb, int cx)
        {
            List<SgkDb> lst = new List<SgkDb>();
            StringReader sr = new StringReader(sdb);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                line = line.Replace("-,","-0,").Replace(".", "").Replace(",",".").Replace(" ","");
                string[] parts = line.Split('\t');
                
                SgkDb db = new SgkDb() { };
                db.Cx = cx;
                db.Cn = (from x in GlobalVars.Companies where x.Id == cx select x.CompanyName).FirstOrDefault();
                db.Un = GlobalVars.ActiveUser;
                db.Cd = DateTime.Now;
                db.Yil = parts[0] != "" ? parts[0] : "";
                db.Ay = parts[1] != "" ? parts[1] : "";
                db.Drm = parts[2] != "" ? parts[2] : "";
                db.Pb = parts[3] != "" ? SetPoints(parts[3])  : 0;
                db.PbGz = parts[4] != "" ? SetPoints(parts[4])  : 0;
                db.Ipcb = parts[5] != "" ? SetPoints(parts[5])  : 0;
                db.IpcbGz = parts[6] != "" ? SetPoints(parts[6])  : 0;
                db.Ekpb = parts[7] != "" ? SetPoints(parts[7])  : 0;
                db.EkpbGz = parts[8] != "" ? SetPoints(parts[8])  : 0;
                db.Oivb = parts[8] != "" ? SetPoints(parts[9])  : 0;
                db.OivbGz = parts[10] != "" ? SetPoints(parts[10])  : 0;
                db.Ib = parts[11] != "" ? SetPoints(parts[11])  : 0;
                db.IbGz = parts[12] != "" ? SetPoints(parts[12])  : 0;
                db.Dvb = parts[13] != "" ? SetPoints(parts[13])  : 0;
                db.DvbGz = parts[14] != "" ? SetPoints(parts[14])  : 0;
                db.Dpgzb = parts[15] != "" ? SetPoints(parts[15])  : 0;
                db.DpgzbGz = parts[16] != "" ? SetPoints(parts[16])  : 0;
                db.Dekpgzb = parts[17] != "" ? SetPoints(parts[17])  : 0;
                db.Sbt = parts[18] != "" ? SetPoints(parts[18])  : 0;
                db.SbtGz = parts[19] != "" ? SetPoints(parts[19])  : 0;

                lst.Add(db);
            }
            
            return lst;
        }
    }
}
