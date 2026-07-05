using Models.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Models.Domain
{
    public class SgkHlp : SgkAssistantBase
    {
        public string Cn { get; set; }
        public string Tcno { get; set; }
        public string Ads { get; set; }
        public decimal Utl { get; set; }
        public decimal Itl { get; set; }
        public int Gun { get; set; }
        public int Ucg { get; set; }
        public int EGun { get; set; }
        public int GGun { get; set; }
        public int CGun { get; set; }
        public int Icn { get; set; }
        public int Egn { get; set; }
        public string Mk { get; set; }
        public DateTime Ya { get; set; }
        public string Bm { get; set; }
        public string Bt { get; set; }
        public string Kk { get; set; }
        public int Ttl { get; set; }
        public string Pdfid { get; set; }

        /* PDF DOSYASINDAKİ ELEMAN SAYISI KADAR ELEMAN EKLENDİĞİNDEN EMİN OL */
        public List<SgkHlp> ConvertToSgkHlp(string source, int cx, int ttl, string pdfid, out string msg)
        {
            msg = "";
            List<SgkHlp> eList = new List<SgkHlp>();
            int step = 10;
            try
            {
                
                string x = ReadPdfFile(source, out msg);
                step = 20;
                if (x == null)
                {
                    x = ReadPdfByPdfPig(source, out msg);
                    if (x == null) return null;
                }
                Regex rx = new Regex(@"[0-9]{1}[0-9]{9}[02468]{1}");
                x = x.Replace("*", "");
                StringReader sr = new StringReader(x);
                string line = "", mahiyet = "", belgeCesidi = "", kanunKodu = ""; DateTime donem = DateTime.MinValue;
                step = 30;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(' ');
                    if (parts[0] == "Yıl") {   donem = DateTime.Parse(parts[4].Replace("/", ".")); continue; }
                    if (parts[0] == "Belge") { belgeCesidi = parts[3].Trim(); continue; }
                    if (parts[0] == "Mahiyet") 
                    { mahiyet = parts[2].Trim(); continue; }
                    if (parts[0] == "Kanun") { kanunKodu = parts[2].Trim(); break; }
                }
                step = 40;
                while ((line = sr.ReadLine()) != null)
                {
                    SgkHlp ele = new SgkHlp();
                    string[] parts = line.Split(' ');
                    if (parts.Count() < 14) { continue; }
                    if (!rx.IsMatch(parts[1])) { continue; }

                    ele.Cx = cx;
                    ele.Cn = (from z in GlobalVars.Companies where z.Id == cx select z.CompanyName).FirstOrDefault();
                    ele.Un = GlobalVars.ActiveUser;
                    ele.Cd = DateTime.Now;
                    ele.Tcno = parts[1].Trim();
                    int h = parts.Count() - 1; //15
                    ele.Mk = parts[h--].Trim();
                    ele.Egn = Convert.ToInt32(parts[h--].Trim());
                    ele.Icn = Convert.ToInt32(parts[h--].Trim());
                    ele.CGun = Convert.ToInt32(parts[h--].Trim());
                    ele.GGun = Convert.ToInt32(parts[h--].Trim());
                    ele.EGun = Convert.ToInt32(parts[h--].Trim());
                    ele.Ucg = Convert.ToInt32(parts[h--].Trim());
                    ele.Gun = Convert.ToInt32(parts[h--].Trim());
                    ele.Itl = parts[h].Contains("--")? 0:  SetPoints(parts[h--].Replace(".", "").Replace(",", ".").Trim());
                    ele.Utl = parts[h].Contains("--")? 0:  SetPoints(parts[h--].Replace(".", "").Replace(",", ".").Trim());
                    for (int i = 2; i <= h; i++)
                    {
                        ele.Ads += $"{parts[i].Trim()} ";
                    }
                    ele.Ya = donem;
                    ele.Bm = mahiyet;
                    ele.Bt = belgeCesidi;
                    ele.Kk = kanunKodu;
                    ele.Ttl = ttl;
                    ele.Pdfid = pdfid;
                    eList.Add(ele);
                }
                step = 50;
            }
            catch (Exception ex)
            {
                msg = $"Hata {step}: {ex.Message}"; // Console.WriteLine(ex.Message);
                /* hata varsa bu firmaya ait hlp kayıtlarını listede çıkar */
                eList = (from x in eList where x.Cx != cx select x).ToList();
            }
            return eList;
        }
        
    }
}
