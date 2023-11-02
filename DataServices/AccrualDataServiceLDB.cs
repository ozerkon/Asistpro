using LiteDB;
using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataServices
{
    class AccrualDataServiceLdb : DbBaseLiteDb, IAccrualDataService
    {
        public AccrualDataServiceLdb()
        {
            ThkkConnect = LdbConnect.GetCollection<SgkThkk>("thkk");
            ThkkConnect.EnsureIndex(x => x.Id);
            ThkkConnect.EnsureIndex(x => x.Cx);
            ThkkConnect.EnsureIndex(x => x.Tya);
        }
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
                        int sonuc = ThkkConnect.Insert(thkk);
                        if (sonuc >= 1) result++;
                    }
                    else if (thkkId >= 0)
                    {
                        thkk.Id = thkkId;
                        result = ThkkConnect.Update(thkk) ? result + 1 : result;
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
            int result = 0;
            try
            {
                int thkkId = IsThkkExists(thkk, out msg);
                if (thkkId == -10)
                {
                    int sonuc = ThkkConnect.Insert(thkk);
                    if (sonuc >= 1) result = 1;
                }
                else if (thkkId >= 0)
                {
                    thkk.Id = thkkId;
                    result = ThkkConnect.Update(thkk) ? result + 1 : result;
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
        public int DeleteThkKs(List<int> cxs, DateTime tr1, DateTime tr2, out string msg)
        {
            msg = "";
            int result;
            try
            {
                result = ThkkConnect.DeleteMany(Query.And(Query.GTE("cx", cxs.Min()), Query.LTE("cx", cxs.Max()), Query.Between("tya", tr1, tr2)));
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                result = -1;
            }
            return result;
        }
        public List<SgkThkk> GetAllThkk(List<int> cxs, DateTime tr1, DateTime tr2, out string msg)
        {
            msg = "";
            List<SgkThkk> lst = null;
            tr1 = new DateTime(tr1.Year, tr1.Month, 1);
            tr2 = new DateTime(tr2.Year, tr2.Month, 1);
            try
            {
                lst = ThkkConnect.Find(q => cxs.Contains(Convert.ToInt32(q.Cx))).ToList();
                lst = (from x in lst where x.Tya >= tr1 && x.Tya <= tr2 select x).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }

        public List<SgkThkk> GetAllThkk(out string msg)
        {
            msg = "";
            List<SgkThkk> lst = null;
            try
            {
                lst = ThkkConnect.FindAll().ToList();
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
                SgkThkk temp = ThkkConnect.FindOne(Query.And(Query.EQ("cx", thkk.Cx), Query.EQ("tya", thkk.Tya), Query.EQ("sgm", thkk.Sgm), Query.EQ("tp", thkk.Tp), Query.EQ("ip", thkk.Ip), Query.EQ("kn14857", thkk.Kn14857), Query.EQ("kn15921", thkk.Kn15921), Query.EQ("kn6645", thkk.Kn6645), Query.EQ("kn15510", thkk.Kn15510), Query.EQ("kn2828", thkk.Kn2828), Query.EQ("kn6111", thkk.Kn6111), Query.EQ("kn17103", thkk.Kn17103), Query.EQ("kn17103i", thkk.Kn17103I), Query.EQ("kn27103", thkk.Kn27103), Query.EQ("kn7252", thkk.Kn7252), Query.EQ("kn17256", thkk.Kn17256), Query.EQ("kn7316", thkk.Kn7316), Query.EQ("kn7319", thkk.Kn7319), Query.EQ("kn5510", thkk.Kn5510), Query.EQ("kn4857", thkk.Kn4857), Query.EQ("kn159210", thkk.Kn159210), Query.EQ("kn3294", thkk.Kn3294), Query.EQ("odenecek", thkk.Odenecek), Query.EQ("pdfPath", thkk.PdfPath)));
                if (temp == null)
                {
                    return -10;
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
    }
}
