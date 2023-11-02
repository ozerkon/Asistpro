using LiteDB;
using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataServices
{
    public class SgkDataServiceLdb : DbBaseLiteDb, ISgkDataService
    {
        public SgkDataServiceLdb()
        {
            SgkDbConnect = LdbConnect.GetCollection<SgkDb>("dbs");
            SgkDbConnect.EnsureIndex(x => x.Id);
            SgkDbConnect.EnsureIndex(x => x.Cx);
            SgkDbConnect.EnsureIndex(x => x.Yil);
            SgkDbConnect.EnsureIndex(x => x.Ay);

            SgkEtConnect = LdbConnect.GetCollection<SgkEt>("ets");
            SgkEtConnect.EnsureIndex(x => x.Id);
            SgkEtConnect.EnsureIndex(x => x.Cx);
            SgkEtConnect.EnsureIndex(x => x.Tt);
            SgkEtConnect.EnsureIndex(x => x.Yil);
            SgkEtConnect.EnsureIndex(x => x.Ay);

            SgkMeConnect = LdbConnect.GetCollection<SgkMe>("mes");
            SgkMeConnect.EnsureIndex(x => x.Id);
            SgkMeConnect.EnsureIndex(x => x.Cx);
            SgkMeConnect.EnsureIndex(x => x.Byt);
            SgkMeConnect.EnsureIndex(x => x.Tur);

            SgkCrConnect = LdbConnect.GetCollection<SgkCr>("crs");
            SgkCrConnect.EnsureIndex(x => x.Id);
            SgkCrConnect.EnsureIndex(x => x.Cx);
            SgkCrConnect.EnsureIndex(x => x.Kn);
            SgkCrConnect.EnsureIndex(x => x.Ty);
            SgkCrConnect.EnsureIndex(x => x.Bt);

            Sgk6661Connect = LdbConnect.GetCollection<Sgk6661>("aaab");
            Sgk6661Connect.EnsureIndex(x => x.Id);
            Sgk6661Connect.EnsureIndex(x => x.Cx);
            Sgk6661Connect.EnsureIndex(x => x.Yil);
            Sgk6661Connect.EnsureIndex(x => x.Ay);

            IglConnect = LdbConnect.GetCollection<SgkIgl>("igl");
            IglConnect.EnsureIndex(x => x.Id);
            IglConnect.EnsureIndex(x => x.Cx);
            IglConnect.EnsureIndex(x => x.Tc);

            HlConnect = LdbConnect.GetCollection<SgkHl>("hl");
            HlConnect.EnsureIndex(x => x.Id);
            HlConnect.EnsureIndex(x => x.Cx);
            HlConnect.EnsureIndex(x => x.Hya);

            HlpConnect = LdbConnect.GetCollection<SgkHlp>("hlp");
            HlpConnect.EnsureIndex(x => x.Id);
            HlpConnect.EnsureIndex(x => x.Cx);
            HlpConnect.EnsureIndex(x => x.Tcno);
            HlpConnect.EnsureIndex(x => x.Ya);

        }
        public List<SgkDb> GetAllDb(List<int> cxs, out string msg)
        {
            msg = "";
            List<SgkDb> lst = null;
            try
            {
                lst = SgkDbConnect.Find(q => cxs.Contains(Convert.ToInt32(q.Cx))).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }

        }
        public List<SgkDb> GetAllDb(int cx, string yil, string ay, out string msg)
        {
            msg = "";
            List<SgkDb> lst = null;
            try
            {
                lst = SgkDbConnect.Find(Query.And(Query.EQ("cx", cx), Query.EQ("yil", yil), Query.EQ("ay", ay))).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }

        }
        public int IsDbExists(int cx, string yil, string ay, out string msg)
        {
            msg = "";
            try
            {
                bool x = SgkDbConnect.Exists(Query.And(Query.EQ("cx", cx), Query.EQ("yil", yil), Query.EQ("ay", ay)));
                if (x)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }
        public (int, int) AddPd(List<SgkDb> lst, out string msg)
        {
            int resultEkle = 0, resultGuncelle = 0;
            List<SgkDb> lstGuncelle = new List<SgkDb>();
            List<SgkDb> lstUpdate = new List<SgkDb>();
            List<SgkDb> lstEkle = new List<SgkDb>();
            msg = "";
            try
            {
                foreach (SgkDb sd in lst)
                {
                    lstGuncelle = lstGuncelle.Concat(GetAllDb(sd.Cx, sd.Yil, sd.Ay, out msg)).ToList();

                    int varmi = IsDbExists(sd.Cx, sd.Yil, sd.Ay, out msg);
                    if (varmi == 0)
                    {
                        lstEkle.Add(sd);
                    }
                }
                if (lstGuncelle.Count > 0)
                {
                    List<int> ids = (from lg in lstGuncelle orderby lg.Id select lg.Id).ToList();
                    List<int> cxs = (from lg in lstGuncelle orderby lg.Id select lg.Cx).ToList();
                    List<string> yils = (from lg in lstGuncelle orderby lg.Id select lg.Yil).ToList();
                    List<string> ays = (from lg in lstGuncelle orderby lg.Id select lg.Ay).ToList();

                    lstUpdate = (from x in lst where cxs.Contains(x.Cx) && yils.Contains(x.Yil) && ays.Contains(x.Ay) select x).ToList();
                    foreach (SgkDb sdb in lstUpdate)
                    {
                        for (int i = 0; i < ids.Count; i++)
                        {
                            if (sdb.Cx == cxs[i] && sdb.Yil == yils[i] && sdb.Ay == ays[i])
                            {
                                sdb.Id = ids[i];
                            }
                        }

                    }
                    resultGuncelle = SgkDbConnect.Update(lstUpdate);
                }

                resultEkle = SgkDbConnect.InsertBulk(lstEkle);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString(); return (-1, -1);
            }

            return (resultEkle, resultGuncelle);
        }

        public List<SgkEt> GetAllEt(List<int> cxs, out string msg)
        {
            msg = "";
            List<SgkEt> lst = null;
            try
            {
                lst = SgkEtConnect.Find(q => cxs.Contains(Convert.ToInt32(q.Cx))).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }

        }
        public int AddEt(List<SgkEt> lst, out string msg)
        {
            msg = "";
            try
            {
                foreach (SgkEt et in lst)
                {
                    BsonExpression e1 = BsonExpression.Create("$.cx = @0", et.Cx);
                    SgkEtConnect.DeleteMany(e1);
                }
                return SgkEtConnect.InsertBulk(lst);

            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString(); return -1;
            }

        }

        public List<SgkMe> GetAllMe(List<int> cxs, out string msg)
        {
            msg = "";
            List<SgkMe> lst = null;
            try
            {
                lst = SgkMeConnect.Find(q => cxs.Contains(Convert.ToInt32(q.Cx))).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }

        }
        public int AddMe(List<SgkMe> lst, out string msg)
        {
            msg = "";
            try
            {
                foreach (SgkMe et in lst)
                {
                    BsonExpression e1 = BsonExpression.Create("$.cx = @0", et.Cx);
                    SgkMeConnect.DeleteMany(e1);
                }
                return SgkMeConnect.InsertBulk(lst);

            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString(); return -1;
            }

        }

        public List<SgkCr> GetAllCr(List<int> cxs, out string msg)
        {
            msg = "";
            List<SgkCr> lst = null;
            try
            {
                lst = SgkCrConnect.Find(q => cxs.Contains(Convert.ToInt32(q.Cx))).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }

        }
        public int AddCr(List<SgkCr> lst, out string msg)
        {
            msg = "";
            try
            {
                foreach (SgkCr et in lst)
                {
                    BsonExpression e1 = BsonExpression.Create("$.cx = @0", et.Cx);
                    SgkCrConnect.DeleteMany(e1);
                }
                return SgkCrConnect.InsertBulk(lst);

            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString(); return -1;
            }

        }

        public List<Sgk6661> GetAll6661(out string msg)
        {
            msg = "";
            List<Sgk6661> lst = null;
            try
            {
                lst = Sgk6661Connect.FindAll().ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }

        }
        public List<Sgk6661> GetAll6661(List<int> cxs, string ya, out string msg)
        {
            msg = "";
            List<Sgk6661> lst = null;
            try
            {
                lst = Sgk6661Connect.Find(q => cxs.Contains(Convert.ToInt32(q.Cx))).ToList();
                lst = (from x in lst where x.Yil == ya select x).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }

        }
        public int Add6661(List<Sgk6661> lst, out string msg)
        {
            msg = "";
            try
            {
                foreach (Sgk6661 et in lst)
                {
                    BsonExpression e1 = BsonExpression.Create("$.cx = @0 AND $.yil = @1", et.Cx, et.Yil);
                    Sgk6661Connect.DeleteMany(e1);
                }
                return Sgk6661Connect.InsertBulk(lst);

            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString(); return -1;
            }

        }

        public List<SgkIgl> GetAllIgl(out string msg)
        {
            msg = "";
            List<SgkIgl> lst = null;
            try
            {
                lst = IglConnect.FindAll().ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }

        }
        public List<SgkIgl> GetAllIgl(List<int> cxs, DateTime tr1, DateTime tr2, out string msg)
        {
            msg = "";
            List<SgkIgl> lst = null;
            try
            {
                lst = IglConnect.Find(q => cxs.Contains(Convert.ToInt32(q.Cx))).ToList();
                lst = (from x in lst where x.Tr >= tr1 && x.Tr <= tr2 select x).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }

        }
        public int IsIglExists(SgkIgl igl, out string msg)
        {
            msg = "";
            try
            {
                //bool x = iglConnect.Exists(Query.And(Query.EQ("cx", igl.cx), Query.EQ("tc", igl.tc), Query.EQ("gc", igl.gc), Query.EQ("tr", igl.tr), Query.EQ("ist", igl.ist), Query.EQ("isa", igl.isa)));
                SgkIgl iglx = IglConnect.FindOne(Query.And(Query.EQ("cx", igl.Cx), Query.EQ("tc", igl.Tc), Query.EQ("gc", igl.Gc), Query.EQ("tr", igl.Tr), Query.EQ("ist", igl.Ist), Query.EQ("isa", igl.Isa)));
                if (iglx != null)
                {
                    //SgkIgl iglx = iglConnect.FindOne(Query.And(Query.EQ("cx", igl.cx), Query.EQ("tc", igl.tc), Query.EQ("gc", igl.gc), Query.EQ("tr", igl.tr), Query.EQ("ist", igl.ist), Query.EQ("isa", igl.isa)));
                    return iglx.Id;
                }
                else
                {
                    return -10;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }
        public int AddIgl(List<SgkIgl> lst, out string msg)
        {
            msg = ""; int result = 0;
            try
            {
                foreach (SgkIgl igl in lst)
                {
                    int isExists = IsIglExists(igl, out msg);
                    if (isExists == -10)
                    {
                        result += IglConnect.Insert(igl) > 0 ? 1 : 0;
                    }
                    else if (isExists >= 0)
                    {
                        igl.Id = isExists;
                        result = (IglConnect.Update(igl)) ? result + 1 : result;
                    }
                    else
                    {
                        result = -1;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString(); return -1;
            }

        }

        public int RemoveUnapproveds(List<DateTime> tyas, out string msg)
        {
            msg = "";
            try
            {
                int x = HlConnect.DeleteMany(q => tyas.Equals(q.Tya) && q.PdfPath.Contains("Onaysiz"));
                int y = HlpConnect.DeleteMany(q => tyas.Equals(q.Ya) && q.Pdfid.StartsWith("Onaysiz"));
                return x + y;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }
        public List<SgkHl> GetAllHl(out string msg)
        {
            msg = "";
            List<SgkHl> lst = null;
            try
            {
                lst = HlConnect.FindAll().ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }

        }
        public List<SgkHl> GetAllHl(List<int> cxs, DateTime tr1, DateTime tr2, out string msg)
        {
            msg = "";
            List<SgkHl> lst = null;
            try
            {
                lst = HlConnect.Find(q => cxs.Contains(Convert.ToInt32(q.Cx))).ToList();
                lst = (from x in lst where x.Hya >= tr1 && x.Hya <= tr2 select x).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }

        }
        public int GetHlId(SgkHl hl, out string msg)
        {
            msg = "";
            try
            {
                SgkHl temp = HlConnect.FindOne(Query.And(Query.EQ("cx", hl.Cx), Query.EQ("tya", hl.Tya), Query.EQ("hya", hl.Hya), Query.EQ("bt", hl.Bt), Query.EQ("bm", hl.Bm), Query.EQ("kn", hl.Kn), Query.EQ("tcs", hl.Tcs), Query.EQ("tgs", hl.Tgs), Query.EQ("tpt", hl.Tpt)));
                if (temp == null)
                {
                    return 0;
                }
                else
                {
                    return temp.Id;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public int IsHlExists(SgkHl hl, out string msg)
        {
            msg = "";
            try
            {
                hl.Kn = hl.Kn == " " ? null : hl.Kn;
                //List<SgkHl> onaysiz = hlConnect.Find(Query.Contains("pdfPath", "Onaysiz")).ToList();
                SgkHl temp = HlConnect.FindOne(Query.And(Query.EQ("cx", hl.Cx), Query.EQ("tya", hl.Tya), Query.EQ("hya", hl.Hya), Query.EQ("bt", hl.Bt), Query.EQ("bm", hl.Bm), Query.EQ("kn", hl.Kn), Query.EQ("tcs", hl.Tcs), Query.EQ("tgs", hl.Tgs), Query.EQ("tpt", hl.Tpt)));
                if (temp == null)
                {
                    return -10;
                }
                else
                {
                    return temp.Id;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }
        public int AddHl(List<SgkHl> lst, out string msg)
        {
            msg = ""; int result = 0;
            try
            {
                foreach (SgkHl hl in lst)
                {
                    int hlId = IsHlExists(hl, out msg);
                    if (hlId == -10)
                    {
                        int sonuc = HlConnect.Insert(hl);
                        if (sonuc >= 1) result++;
                    }
                    else if (hlId >= 0)
                    {
                        hl.Id = hlId;
                        result = HlConnect.Update(hl) ? result + 1 : result;
                    }
                    else
                    {
                        result = -1;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString(); return -1;
            }
        }
        public int DeleteHLs(List<int> cxs, DateTime tr1, DateTime tr2, out string msg) 
        { 
            msg = ""; 
            int result = 0;
            try
            {
               result =  HlConnect.DeleteMany(Query.And(Query.GTE("cx", cxs.Min()), Query.LTE("cx", cxs.Max()), Query.Between("tya", tr1, tr2)));
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                result = -1;
            }
            return result; 
        }

        public List<SgkHlp> GetAllHlp(out string msg)
        {
            msg = "";
            List<SgkHlp> lst = null;
            try
            {
                lst = HlpConnect.FindAll().ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public List<SgkHlp> GetAllHlp(List<int> cxs, DateTime tr1, DateTime tr2, out string msg)
        {
            msg = "";
            List<SgkHlp> lst = null;
            tr1 = new DateTime(tr1.Year, tr1.Month, 1);
            tr2 = new DateTime(tr2.Year, tr2.Month, 1);
            try
            {
                lst = HlpConnect.Find(q => cxs.Contains(Convert.ToInt32(q.Cx))).ToList();
                lst = (from x in lst where x.Ya >= tr1 && x.Ya <= tr2 select x).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public int GetHlpId(SgkHlp hlp, out string msg)
        {
            msg = "";
            try
            {
                SgkHlp temp = HlpConnect.FindOne(Query.And(Query.EQ("cx", hlp.Cx), Query.EQ("tcno", hlp.Tcno), Query.EQ("utl", hlp.Utl), Query.EQ("itl", hlp.Itl), Query.EQ("gun", hlp.Gun), Query.EQ("eGun", hlp.EGun), Query.EQ("gGun", hlp.GGun), Query.EQ("cGun", hlp.CGun), Query.EQ("icn", hlp.Icn), Query.EQ("egn", hlp.Egn), Query.EQ("mk", hlp.Mk), Query.EQ("ya", hlp.Ya), Query.EQ("bm", hlp.Bm), Query.EQ("bt", hlp.Bt), Query.EQ("kk", hlp.Kk)));
                if (temp == null)
                {
                    return 0;
                }
                else
                {
                    return temp.Id;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public int IsHlpExists(SgkHlp hlp, out string msg)
        {
            msg = "";
            try
            {
                SgkHlp temp = HlpConnect.FindOne(Query.And(Query.EQ("cx", hlp.Cx), Query.EQ("tcno", hlp.Tcno), Query.EQ("utl", hlp.Utl), Query.EQ("itl", hlp.Itl), Query.EQ("gun", hlp.Gun), Query.EQ("eGun", hlp.EGun), Query.EQ("gGun", hlp.GGun), Query.EQ("cGun", hlp.CGun), Query.EQ("icn", hlp.Icn), Query.EQ("egn", hlp.Egn), Query.EQ("mk", hlp.Mk), Query.EQ("ya", hlp.Ya), Query.EQ("bm", hlp.Bm), Query.EQ("bt", hlp.Bt), Query.EQ("kk", hlp.Kk)));
                if (temp == null)
                {
                    return -10;
                }
                else
                {
                    return temp.Id;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }
        public int AddHlp(List<SgkHlp> lst, out string msg)
        {
            msg = ""; int result = 0;
            try
            {
                foreach (SgkHlp hlp in lst)
                {
                    int isExists = IsHlpExists(hlp, out msg);
                    if (isExists == -10)
                    {
                        int sonuc = HlpConnect.Insert(hlp);
                        if (sonuc >= 1) result++;
                    }
                    else if (isExists >= 0)
                    {
                        hlp.Id = GetHlpId(hlp, out msg);
                        result = HlpConnect.Update(hlp) ? result + 1 : result;
                    }
                    else
                    {
                        result = -1;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString(); return -1;
            }
        }
        public int AddHlp(SgkHlp hlp, out string msg)
        {
            int result = 0;
            try
            {
                int hlpId = IsHlpExists(hlp, out msg);
                if (hlpId == -10)
                {
                    int sonuc = HlpConnect.Insert(hlp);
                    if (sonuc >= 1) result = 1;
                }
                else if (hlpId >= 0)
                {
                    hlp.Id = hlpId;
                    result = HlpConnect.Update(hlp) ? result + 1 : result;
                }
                else
                {
                    result = -1;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString(); return -1;
            }

            return result;
        }
        public int DeleteHlPs(List<int> cxs, DateTime tr1, DateTime tr2, out string msg) 
        {
            msg = "";
            int result;
            try
            {
                result = HlpConnect.DeleteMany(Query.And(Query.GTE("cx", cxs.Min()), Query.LTE("cx", cxs.Max()), Query.Between("ya", tr1, tr2)));
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                result = -1;
            }
            return result;
        }
        public void RemoveDuplicate()
        {
            string msg = "";
            List<SgkHlp> lst = GetAllHlp(new List<int> { 2 }, new DateTime(2015, 1, 1), DateTime.Today, out msg);
            List<SgkHlp> dup = new List<SgkHlp>();
            foreach (SgkHlp y in lst)
            {
                List<SgkHlp> temp =  (from x in lst where x.Ads == y.Ads && x.Bm == y.Bm && x.Bt == y.Bt && x.CGun == y.CGun && x.Cx == y.Cx && x.Egn == y.Egn && x.EGun == y.EGun && x.GGun == y.GGun && x.Gun == y.Gun && x.Icn == y.Icn && x.Itl == y.Itl && x.Kk == y.Kk && x.Mk == y.Mk && x.Tcno == y.Tcno && x.Ttl == y.Ttl && x.Ucg == y.Ucg && x.Utl == y.Utl && x.Ya == y.Ya && x.Cd != y.Cd select x).ToList();
                if (temp != null && temp.Count > 0)
                {
                    dup.AddRange(temp);
                }
            }
        }
    }
}
