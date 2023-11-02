using Models.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Models.Domain
{
    public class SgkThkk : SgkAssistantBase
    {
        public string Cn { get; set; } = "";
        public DateTime Tya { get; set; } = DateTime.Now;
        public string Bm { get; set; } = "";
        public string Sgm { get; set; } = "";
        public decimal Tp { get; set; } = 0;
        public decimal Ip { get; set; }= 0;
        public decimal Kn14857 { get; set; }= 0;
        public decimal Kn15921 { get; set; }= 0;
        public decimal Kn6645 { get; set; }= 0;
        public decimal Kn15510 { get; set; }= 0;
        public decimal Kn2828 { get; set; }= 0;
        public decimal Kn6111 { get; set; }= 0;
        public decimal Kn17103 { get; set; }= 0;
        public decimal Kn17103I { get; set; }= 0;
        public decimal Kn27103 { get; set; }= 0;
        public decimal Kn27103I { get; set; }= 0;
        public decimal Kn37103 { get; set; }= 0;
        public decimal Kn37103I { get; set; }= 0;
        public decimal Kn7252 { get; set; }= 0;
        public decimal Kn17256 { get; set; }= 0;
        public decimal Kn7316 { get; set; }= 0;
        public decimal Kn7319 { get; set; }= 0;
        public decimal Kn5510 { get; set; }= 0;
        public decimal Kn4857 { get; set; }= 0;
        public decimal Kn159210 { get; set; }= 0;
        public decimal Kn3294 { get; set; }= 0;
        public decimal Odenecek { get; set; }= 0;
        public string PdfPath { get; set; } = "";
        public bool Onayli { get; set; } = true;

        public SgkThkk ConvertToSgkThkk(string source, int cx, string pdfPath, out string msg)
        {
            msg = "";
            SgkThkk thkk = new SgkThkk();
            List<string> lines = new List<string>();
            try
            {
                string x = ReadPdfFile(source, out msg);
                if (x == null)
                {
                    x = ReadPdfByPdfPig(source, out msg);
                    if (x == null) return null;
                }
                
                x = x.Replace("*", "");
                StringReader sr = new StringReader(x);
                string line = "", onayli = ""; 
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                }
                thkk.Cn = (from z in GlobalVars.Companies where z.Id == cx select z.CompanyName).FirstOrDefault();
                thkk.Cx = cx;
                thkk.Un = GlobalVars.ActiveUser;
                thkk.Cd = DateTime.Now;
                thkk.PdfPath = pdfPath;

                for (int i = 0; i < lines.Count; i++)
                {
                    string[] parts = lines[i].Split(' ');
                    if(lines[i].Contains("TAHAKKUK FİŞİ")) { thkk.Bm = lines[i].Substring(lines[i].Substring(0, lines[i].IndexOf(" Belge türü")).LastIndexOf(' ') + 1, (lines[i].IndexOf(" Belge türü")) -(lines[i].Substring(0, lines[i].IndexOf(" Belge türü")).LastIndexOf(' ') + 1));  }
                    if (parts[0] == "SGM(kod-ad)") { thkk.Sgm = lines[i].Substring(14).Replace("SGK ", "").Replace(" SOSYAL GÜVENLİK MERKEZİ", "").Replace(" SOSYAL GÜVENLİK MERKEZ", ""); continue; }
                    if (parts[0] == "BELGE") { onayli = lines[i+1].Trim(); thkk.Onayli = !onayli.Contains("ON") ? true : false;  continue; }
                    if (lines[i].Contains("AİT OLDUĞU YIL")) {thkk.Tya = DateTime.Parse(lines[i + 1].Trim().Replace("/", "."));  continue; }
                    if (lines[i].Contains("TOPLAM PRİM")) { 
                        thkk.Tp =  SetPoints(parts[2].Trim().Replace(".", "").Replace(",", ".").Trim()); continue; }
                    if (lines[i].Contains("14857 SAYILI")) { thkk.Kn14857 = SetPoints(lines[i - 1].Trim().Replace(".", "").Replace(",", ".").Trim()); continue; }
                    if (lines[i].Contains("15921 SAYILI")) { thkk.Kn15921 = SetPoints(lines[i - 1].Trim().Replace(".", "").Replace(",", ".").Trim()); continue; }
                    if (lines[i].Contains("6645 SAYILI"))  { thkk.Kn6645 = SetPoints(lines[i - 1].Trim().Replace(".", "").Replace(",", ".").Trim()); continue; }
                    if (lines[i].Contains("15510 SAYILI")) { thkk.Kn15510 = SetPoints(lines[i - 1].Trim().Replace(".", "").Replace(",", ".").Trim()); continue; }
                    if (lines[i].Contains("2828 SAYILI")) { thkk.Kn2828 = SetPoints(lines[i - 1].Trim().Replace(".", "").Replace(",", ".").Trim()); continue; }
                    if (lines[i].Contains("06111 SAYILI")) { thkk.Kn6111 = SetPoints(lines[i - 1].Trim().Replace(".", "").Replace(",", ".").Trim()); continue; }
                    if (lines[i].Contains("17103 SAYILI") && lines[i].Contains("PRİM İNDİRİMİ")) { thkk.Kn17103 = SetPoints(lines[i - 1].Trim().Replace(".", "").Replace(",", ".").Trim()); continue; }
                    if (lines[i].Contains("17103 SAYILI") && lines[i].Contains("İŞSİZLİK İNDİRİMİ")) {
                        try
                        { thkk.Kn17103I = SetPoints(parts[6].Trim().Replace(".", "").Replace(",", ".").Trim()); continue; }
                        catch { thkk.Kn17103I = SetPoints(lines[i - 1].Trim().Replace(".", "").Replace(",", ".").Trim()); continue; }
                    }
                    if (lines[i].Contains("27103 SAYILI") && lines[i].Contains("PRİM İNDİRİMİ")) { thkk.Kn27103 = SetPoints(lines[i - 1].Trim().Replace(".", "").Replace(",", ".").Trim()); continue; }
                    if (lines[i].Contains("27103 SAYILI") && lines[i].Contains("İŞSİZLİK İNDİRİMİ")) {
                        try
                        { thkk.Kn27103I = SetPoints(parts[6].Trim().Replace(".", "").Replace(",", ".").Trim()); continue; }
                        catch { thkk.Kn27103I = SetPoints(lines[i - 1].Trim().Replace(".", "").Replace(",", ".").Trim()); continue; }
                    }
                    if (lines[i].Contains("37103 SAYILI") && lines[i].Contains("PRİM İNDİRİMİ")) { thkk.Kn37103 = SetPoints(lines[i - 1].Trim().Replace(".", "").Replace(",", ".").Trim()); continue; }
                    if (lines[i].Contains("37103 SAYILI") && lines[i].Contains("İŞSİZLİK İNDİRİMİ")) {
                        try
                        { thkk.Kn37103I = SetPoints(parts[6].Trim().Replace(".", "").Replace(",", ".").Trim()); continue; }
                        catch { thkk.Kn37103I = SetPoints(lines[i - 1].Trim().Replace(".", "").Replace(",", ".").Trim()); continue; }
                    }
                    if (lines[i].Contains("7252 SAYILI")) { thkk.Kn7252 = SetPoints(lines[i - 1].Trim().Replace(".", "").Replace(",", ".").Trim()); continue; }
                    if (lines[i].Contains("17256 SAYILI")) { thkk.Kn17256 = SetPoints(lines[i - 1].Trim().Replace(".", "").Replace(",", ".").Trim()); continue; }
                    if (lines[i].Contains("7316 SAYILI")) { thkk.Kn7316 = SetPoints(lines[i - 1].Trim().Replace(".", "").Replace(",", ".").Trim()); continue; }
                    if (lines[i].Contains("7319 SAYILI")) { thkk.Kn7319 = SetPoints(lines[i - 1].Trim().Replace(".", "").Replace(",", ".").Trim()); continue; }
                    if (lines[i].Contains("05510 SAYILI")) { thkk.Kn5510 = SetPoints(lines[i - 1].Trim().Replace(".", "").Replace(",", ".").Trim()); continue; }
                    if (lines[i].Contains("4857 SAYILI")) { thkk.Kn4857 = SetPoints(lines[i - 1].Trim().Replace(".", "").Replace(",", ".").Trim()); continue; }
                    if (lines[i].Contains("159210 SAYILI")) { thkk.Kn159210 = SetPoints(lines[i - 1].Trim().Replace(".", "").Replace(",", ".").Trim()); continue; }
                    if (lines[i].Contains("3294 SAYILI")) { thkk.Kn3294 = SetPoints(lines[i - 1].Trim().Replace(".", "").Replace(",", ".").Trim()); continue; }
                    if (lines[i].Contains("ÖDENECEK NET TUTAR")) { 
                        thkk.Odenecek = SetPoints(parts[3].Trim().Replace(".", "").Replace(",", ".").Trim());  continue; }
                    if (lines[i].Contains("İŞSİZLİK TUTARI")) { thkk.Ip = SetPoints(parts[2].Trim().Replace(".", "").Replace(",", ".").Trim());  continue; }
                } 
            }
            catch (Exception ex)
            {
                msg = $"Hata: {ex.Message}"; return null;
            }
            return thkk;
        }
    }
    public class SgkThkkDownload
    {
        public string NewPath { get; set; }
        public int LoginId { get; set; }
        public string PdfId { get; set; }
    }
}
