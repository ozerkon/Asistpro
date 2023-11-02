using Dapper;
using Models.Common;
using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataServices
{
    public class PeriodCommand : DbBaseMySql
    {
        public int Insert(LeavePeriod prd)
        {
            string query = "INSERT INTO periods VALUES(null, @tcno, @period, @start , @end, @his, @ki, @srk)";
            return ConLocal.Execute(query, new
            {
                tcno = prd.Tcno,
                period = prd.Period,
                start = prd.Startdate,
                end = prd.Enddate,
                his = prd.His,
                ki = prd.Ki,
                srk = prd.Srk
            });
        }
        public int Delete(long id)
        {
            string query = "DELETE FROM periods WHERE id = @id";
            return ConLocal.Execute(query, new { id = id });

        }
        public int Delete(string tcno)
        {
            string query = "DELETE FROM periods WHERE tcno = @tcno";
            return ConLocal.Execute(query, new { tcno = tcno });

        }
        public int Update(LeavePeriod prd)
        {
            string query = @"UPDATE periods SET 
                        tcno = @tcno, 
                        period = @period, 
                        startdate = @startdate,
                        enddate = @enddate, 
                        his = @his, 
                        ki = @ki, 
                        srk = @srk
                        WHERE id = @conditionId";
            return ConLocal.Execute(query, new
            {
                tcno = prd.Tcno,
                period = prd.Period,
                startdate = prd.Startdate,
                enddate = prd.Enddate,
                his = prd.His,
                ki = prd.Ki,
                srk = prd.Srk,
                conditionId = prd.Id
            });

        }
    }
    public class LeavePeriodDataServiceMySql : DbBaseMySql, ILeavePeriodDataService
    {
        PeriodCommand lcmd = new PeriodCommand();
        public LeavePeriodDataServiceMySql()
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }
        public int AddLeavePeriod(LeavePeriod period, out string msg)
        {
            try
            {
                Personal p = (from x in GlobalVars.Personals where x.Tcno == period.Tcno select x).FirstOrDefault();
                msg = ""; int result = -5;
                List<LeavePeriod> prd = GetLeavePeriodByTcnoAndDate(period.Tcno, period.Startdate, period.Enddate, out msg);
                if (prd != null && prd.Count > 0)
                {
                    period.Id = prd[0].Id;
                    result = lcmd.Update(period); 
                    result = result > 0 ? 2 : 0;
                }
                else
                {
                    result = lcmd.Insert(period); 
                }
                return result;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
        }
       
        public int AddLeavePeriods(List<LeavePeriod> periods, out string msg)
        {
            msg = ""; int res = 0;
            try
            {
                foreach (LeavePeriod prd in periods)
                {
                    res += AddLeavePeriod(prd, out msg);
                }
                return res;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
        }

        public int DeleteLeavePeriod(long id, out string msg)
        {
            msg = "";
            try
            {
                return lcmd.Delete(id);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
        }
        public int DeleteLeavePeriodsByTcno(string tcno, out string msg)
        {
            msg = "";
            try
            {
                return lcmd.Delete(tcno);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
        }
        public LeavePeriod GetLeavePeriodById(long id, out string msg)
        {
            msg = ""; LeavePeriod prdlv = new LeavePeriod();
            string query = "SELECT * FROM periods WHERE id = @id";
            try
            {
                prdlv = ConLocal.Query<LeavePeriod>(query, new { id = id }).FirstOrDefault();
                return prdlv;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }
        public List<LeavePeriod> GetLeavePeriods(out string msg)
        {
            msg = ""; List<LeavePeriod> prds = new List<LeavePeriod>();
            string query = "SELECT * FROM periods";
            try
            {
                prds = ConLocal.Query<LeavePeriod>(query).ToList();
                return prds;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }
        public List<LeavePeriod> GetLeavePeriodsByTcno(string tcno, out string msg)
        {
            msg = ""; List<LeavePeriod> prds = new List<LeavePeriod>();
            string query = "SELECT * FROM periods WHERE tcno = @tcno";
            try
            {
                prds = ConLocal.Query<LeavePeriod>(query, new { tcno = tcno }).ToList();
                return prds;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }

        public List<LeavePeriod> GetLeavePeriodByTcnoAndDate(string tcno, DateTime sd, DateTime ed, out string msg)
        {
            msg = ""; List<LeavePeriod> prds = new List<LeavePeriod>();
            string query = "SELECT * FROM periods WHERE tcno = @tcno AND startdate >= @sd AND enddate <= @ed";
            try
            {
                prds = ConLocal.Query<LeavePeriod>(query, new { tcno = tcno, sd = sd, ed = ed }).ToList();
                return prds;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }

        public List<LeavePeriod> GetLeavePeriodsByCompanyAndDate(int cid, DateTime sd, DateTime ed, out string msg)
        {
            msg = "";
            List<LeavePeriod> prds = null;
            List<string> tcnos = (from x in GlobalVars.Personals where x.Cid == cid select x.Tcno).ToList();
            string query = "SELECT * FROM periods WHERE tcno IN @tcno AND startdate >= @sd AND enddate <= @ed";
            try
            {
                prds = ConLocal.Query<LeavePeriod>( query, new { tcno = tcnos.ToArray(), sd = sd, ed = ed }).ToList();
                return prds;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }

        public List<LeavePeriod> GetLeavePeriodsByCompany(int cid, out string msg)
        {
            msg = "";
            List<LeavePeriod> prds = null;
            List<string> tcnos = (from x in GlobalVars.Personals where x.Cid == cid select x.Tcno).ToList();
            string query = "SELECT * FROM periods WHERE tcno IN @tcno";
            try
            {
                prds = ConLocal.Query<LeavePeriod>(query, new { tcno = tcnos.ToArray() }).ToList();
                return prds;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }

        public int UpdateLeavePeriod(long id, LeavePeriod period, out string msg)
        {
            msg = "";
            try
            {
                period.Id = id;
                lcmd.Update(period); return 1;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }

        
    }
}
