using LiteDB;
using Models.Common;
using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataServices
{
    public class LeavePeriodDataServiceLdb : DbBaseLiteDb, ILeavePeriodDataService
    {
        public LeavePeriodDataServiceLdb()
        {
            LeavesPeriodsConnect = LdbConnect.GetCollection<LeavePeriod>("periods");
            LeavesPeriodsConnect.EnsureIndex(x => x.Id);
        }
        public int AddLeavePeriod(LeavePeriod period, out string msg)
        {
            try
            {
                Personal prs = (from x in GlobalVars.Personals where x.Tcno == period.Tcno select x).FirstOrDefault();
                int result = -1; bool isOk;
                List<LeavePeriod> prd = GetLeavePeriodByTcnoAndDate(prs.Tcno, period.Startdate, period.Enddate, out msg);
                if (prd != null && prd.Count > 0)
                {
                    period.Id = prd[0].Id;
                    isOk = LeavesPeriodsConnect.Update(period);
                    result = isOk ? 2 : 0; // güncelleme başarılıysa result değerini 2 yap
                }
                else
                {
                    result = LeavesPeriodsConnect.Insert(period).AsInt32;
                    result = (result >= 1) ? 1 : 0;
                }

                return result;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }

        public int AddLeavePeriods(List<LeavePeriod> periods, out string msg)
        {
            int result = 0; msg = "";
            try
            {
                foreach (LeavePeriod prd in periods)
                {
                    result += AddLeavePeriod(prd, out msg);
                }
                return result;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }

        public int DeleteLeavePeriod(long id, out string msg)
        {
            msg = "";
            try
            {
                return LeavesPeriodsConnect.Delete(id) ? 1 : 0;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }
        public int DeleteLeavePeriodsByTcno(string tcno, out string msg)
        {
            msg = "";
            try
            {
                return LeavesPeriodsConnect.DeleteMany(Query.EQ("tcno", tcno) );
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }
        public LeavePeriod GetLeavePeriodById(long id, out string msg)
        {
            msg = ""; LeavePeriod prd;
            try
            {
                prd = LeavesPeriodsConnect.FindById(id);
                return prd;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public List<LeavePeriod> GetLeavePeriods(out string msg)
        {
            msg = "";
            List<LeavePeriod> prds = null;
            try
            {
                prds = LeavesPeriodsConnect.FindAll().ToList();
                return prds;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public List<LeavePeriod> GetLeavePeriodsByTcno(string tcno, out string msg)
        {
            msg = "";
            List<LeavePeriod> prds = null;
            try
            {
                prds = LeavesPeriodsConnect.Find(q => q.Tcno == tcno).ToList();
                return prds;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }

        public List<LeavePeriod> GetLeavePeriodByTcnoAndDate(string tcno, DateTime sd, DateTime ed, out string msg)
        {
            msg = ""; List<LeavePeriod> prds = null;
            try
            {
                prds = LeavesPeriodsConnect.Find(q => q.Tcno == tcno && q.Startdate >= sd && q.Enddate <= ed).ToList();
                return prds;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }

        public List<LeavePeriod> GetLeavePeriodsByCompanyAndDate(int cid, DateTime sd, DateTime ed, out string msg)
        {
            msg = "";
            List<LeavePeriod> prds = null;
            List<string> tcnos = (from x in GlobalVars.Personals where x.Cid == cid select x.Tcno).ToList();
            try
            {
                prds = LeavesPeriodsConnect.Find(q => tcnos.Contains(q.Tcno) && q.Startdate >= sd && q.Enddate <= ed).ToList();
                return prds;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }

        public List<LeavePeriod> GetLeavePeriodsByCompany(int cid, out string msg)
        {
            msg = "";
            List<LeavePeriod> prds = null;
            List<string> tcnos = (from x in GlobalVars.Personals where x.Cid == cid select x.Tcno).ToList();
            try
            {
                prds = LeavesPeriodsConnect.Find(q => tcnos.Contains(q.Tcno) ).ToList();
                return prds;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }

        public int UpdateLeavePeriod(long id, LeavePeriod period, out string msg)
        {
            int result = -1; bool isOk; msg = "";
            try
            {
                period.Id = id;
                isOk = LeavesPeriodsConnect.Update(period);
                result = isOk ? 2 : 0; // güncelleme başarılıysa result değerini 2 yap
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                result = -1;
            }
            return result;
        }

        
    }
}
