using Dapper;
using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataServices
{
    class AccuralDataServiceMySql : DbBaseMySql, IAccrualDataService
    {
        public class ThkkCommand : DbBaseMySql
        {
            public int Insert(SgkThkk s)
            {
                string query = "INSERT INTO sgkthkk (cn, tya, bm, sgm, tp, ip,  kn14857, kn15921, kn6645, kn15510, kn2828, kn6111, kn17103, kn17103i, kn27103, kn7252, kn17256, kn7316, kn7319, kn5510, kn4857, kn159210, kn3294, odenecek, pdfPath, onayli, cx, un, cd)" +
                               " VALUES(@cn, @tya, @bm, @sgm, @tp, @ip, @kn14857, @kn15921, @kn6645, @kn15510, @kn2828, @kn6111, @kn17103, @kn17103i, @kn27103, @kn7252, @kn17256, @kn7316, @kn7319, @kn5510, @kn4857, @kn159210, @kn3294, @odenecek, @pdfPath, @onayli, @cx, @un, @cd)";
                try
                {
                    return ConLocal.ExecuteAsync(query, new
                    {
                        cn = s.Cn,
                        tya = s.Tya,
                        bm = s.Bm,
                        sgm = s.Sgm,
                        tp = s.Tp,
                        ip = s.Ip,
                        kn14857 = s.Kn14857,
                        kn15921 = s.Kn15921,
                        kn6645 = s.Kn6645,
                        kn15510 = s.Kn15510,
                        kn2828 = s.Kn2828,
                        kn6111 = s.Kn6111,
                        kn17103 = s.Kn17103,
                        kn17103i = s.Kn17103I,
                        kn27103 = s.Kn27103,
                        kn7252 = s.Kn7252,
                        kn17256 = s.Kn17256,
                        kn7316 = s.Kn7316,
                        kn7319 = s.Kn7319,
                        kn5510 = s.Kn5510,
                        kn4857 = s.Kn4857,
                        kn159210 = s.Kn159210,
                        kn3294 = s.Kn3294,
                        odenecek = s.Odenecek,
                        pdfPath = s.PdfPath,
                        onayli = s.Onayli,
                        cx = s.Cx,
                        un = s.Un,
                        cd = s.Cd
                    }).Result;
                }
                catch (Exception)
                {
                    return -1;
                }
                
            }
            public int Update(SgkThkk s)
            {
                string query = @"UPDATE sgkthkk SET 
                    cn = @cn,
                    tya = @tya,
                    sgm = @sgm,
                    bm =  @bm,
                    tp = @tp,
                    ip = @ip,
                    kn14857 = @kn14857,
                    kn15921 = @kn15921,
                    kn6645 = @kn6645,
                    kn15510 = @kn15510,
                    kn2828 = @kn2828,
                    kn6111 = @kn6111,
                    kn17103 = @kn17103,
                    kn17103i = @kn17103i,
                    kn27103 = @kn27103,
                    kn7252 = @kn7252,
                    kn17256 = @kn17256,
                    kn7316 = @kn7316,
                    kn7319 = @kn7319,
                    kn5510 = @kn5510,
                    kn4857 = @kn4857,
                    kn159210 = @kn159210,
                    kn3294 = @kn3294,
                    odenecek = @odenecek,
                    pdfPath = @pdfPath,
                    onayli = @onayli,
                    cx = @cx,
                    un = @un,
                    cd = @cd
                    WHERE id = @conditionId";
                return ConLocal.Execute(query, new
                {
                    cn = s.Cn,
                    tya = s.Tya,
                    sgm = s.Sgm,
                    bm = s.Bm,
                    tp = s.Tp,
                    ip = s.Ip,
                    kn14857 = s.Kn14857,
                    kn15921 = s.Kn15921,
                    kn6645 = s.Kn6645,
                    kn15510 = s.Kn15510,
                    kn2828 = s.Kn2828,
                    kn6111 = s.Kn6111,
                    kn17103 = s.Kn17103,
                    kn17103i = s.Kn17103I,
                    kn27103 = s.Kn27103,
                    kn7252 = s.Kn7252,
                    kn17256 = s.Kn17256,
                    kn7316 = s.Kn7316,
                    kn7319 = s.Kn7319,
                    kn5510 = s.Kn5510,
                    kn4857 = s.Kn4857,
                    kn159210 = s.Kn159210,
                    kn3294 = s.Kn3294,
                    odenecek = s.Odenecek,
                    pdfPath = s.PdfPath,
                    onayli = s.Onayli,
                    cx = s.Cx,
                    un = s.Un,
                    cd = s.Cd,
                    conditionId = s.Id
                });

            }
        }
        ThkkCommand hlpcmd = new ThkkCommand();
        public int AddThkk(List<SgkThkk> lst, out string msg)
        {
            msg = ""; int result = 0;
            try
            {
                foreach (SgkThkk thkk in lst)
                {
                    int thkkId = IsThkkExists(thkk, out msg);
                    if (thkkId == -10)
                    {
                        int sonuc = hlpcmd.Insert(thkk);
                        if (sonuc >= 1) result++;
                    }
                    else if (thkkId >= 1)
                    {
                        thkk.Id = thkkId;
                        result = (hlpcmd.Update(thkk) > 0 ? true : false) ? result + 1 : result;
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

        public int AddThkk(SgkThkk thkk, out string msg)
        {
            int resultAdd = 0;
            try
            {
                int thkkId = IsThkkExists(thkk, out msg);
                if (thkkId == -10)
                {
                    resultAdd = hlpcmd.Insert(thkk);
                }
                else if (thkkId >= 0)
                {
                    thkk.Id = thkkId;
                    int resultUd = hlpcmd.Update(thkk);
                    msg = $"{resultAdd} adet kayıt eklendi, {resultUd} adet kayıt güncellendi";
                }
                return resultAdd;

            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString(); return -1;
            }
        }

        public int DeleteThkKs(List<int> cxs, DateTime tr1, DateTime tr2, out string msg)
        {
            msg = "";
            string query = "DELETE FROM sgkthkk WHERE tya BETWEEN @d1 AND @d2 AND cx IN @c1";
            int result;
            try
            {
                result = ConLocal.Execute(query, new { d1 = tr1, d2 = tr2, c1 = cxs.ToArray() });
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                result = -1;
            }
            return result;
        }
        public List<SgkThkk> GetAllThkk(out string msg)
        {
            msg = "";
            try
            {
                List<SgkThkk> lst = new List<SgkThkk>();
                string query = "SELECT * FROM sgkthkk";
                lst = ConLocal.Query<SgkThkk>(query).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public List<SgkThkk> GetAllThkk(List<int> cxs, DateTime tr1, DateTime tr2, out string msg)
        {
            msg = "";
            List<SgkThkk> lst = null;
            tr1 = new DateTime(tr1.Year, tr1.Month, 1);
            tr2 = new DateTime(tr2.Year, tr2.Month, 1);
            try
            {
                lst = GetAllThkk(out msg).FindAll(q => cxs.Contains(Convert.ToInt32(q.Cx))).ToList();
                lst = (from x in lst where x.Tya >= tr1 && x.Tya <= tr2 select x).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public int IsThkkExists(SgkThkk thkk, out string msg)
        {
            msg = "";
            try
            {
                string query = "SELECT id, tya, sgm, tp, ip, kn14857, kn15921, kn6645, kn15510, kn2828, kn6111, kn17103, kn17103i, kn27103, kn7252, kn17256, kn7316, kn7319, kn5510, kn4857, kn159210, kn3294, odenecek, pdfPath, onayli, cx FROM sgkthkk WHERE tya = @tya AND sgm = @sgm AND tp = @tp AND ip = @ip AND kn14857 = @kn14857 AND kn15921 = @kn15921 AND kn6645 = @kn6645 AND kn15510 = @kn15510 AND kn2828 = @kn2828 AND kn6111 = @kn6111 AND kn17103 = @kn17103 AND kn17103i = @kn17103i AND kn27103 = @kn27103 AND kn7252 = @kn7252 AND kn17256 = @kn17256 AND kn7316 = @kn7316 AND kn7319 = @kn7319 AND kn5510 = @kn5510 AND kn4857 = @kn4857 AND kn159210 = @kn159210 AND kn3294 = @kn3294 AND odenecek = @odenecek AND pdfPath = @pdfPath AND onayli = @onayli AND cx = @cx";
                SgkHlp x = ConLocal.Query<SgkHlp>(query, new { tya = thkk.Tya, sgm = thkk.Sgm, tp = thkk.Tp, ip = thkk.Ip, kn14857 = thkk.Kn14857, kn15921 = thkk.Kn15921, kn6645 = thkk.Kn6645, kn15510 = thkk.Kn15510, kn2828 = thkk.Kn2828, kn6111 = thkk.Kn6111, kn17103 = thkk.Kn17103, kn17103i = thkk.Kn17103I, kn27103 = thkk.Kn27103, kn7252 = thkk.Kn7252, kn17256 = thkk.Kn17256, kn7316 = thkk.Kn7316, kn7319 = thkk.Kn7319, kn5510 = thkk.Kn5510, kn4857 = thkk.Kn4857, kn159210 = thkk.Kn159210, kn3294 = thkk.Kn3294, odenecek = thkk.Odenecek, pdfPath = thkk.PdfPath, onayli = thkk.Onayli, cx = thkk.Cx}).FirstOrDefault();

                if (x != null)
                {
                    return x.Id;
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

        
    }
}
