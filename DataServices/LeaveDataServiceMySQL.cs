using Dapper;
using Models.Common;
using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataServices
{
    public class LeaveCommand : DbBaseMySql
    {
        public int Insert(Leaves l)
        {
            string query = "INSERT INTO alv VALUES(null, @trxtype, @pid, @cid , @trxdate, @startdate, @enddate, @timeval, @timeunit, @cd, @docref, @notes, @cu)";
            return ConLocal.Execute(query, new
            {
                trxtype = l.Trxtype,
                pid = l.Pid,
                cid = l.Cid,
                trxdate = l.Trxdate,
                startdate = l.Startdate,
                enddate = l.Enddate,
                timeval = l.Timeval,
                timeunit = l.Timeunit,
                cd = l.Cd,
                docref = l.Docref,
                notes = l.Notes,
                cu = l.Cu
            });
        }
        public int Delete(long id)
        {
            string query = "DELETE FROM alv WHERE id = @id";
            return ConLocal.Execute(query, new { id = id });

        }
        public int DeleteAll(long pid)
        {
            string query = "DELETE FROM alv WHERE pid = @pid";
            return ConLocal.Execute(query, new { pid = pid });

        }
        public int Update(Leaves l)
        {
            string query = @"UPDATE alv SET 
                        trxtype = @trxtype, 
                        pid = @pid, 
                        cid = @cid,
                        trxdate = @trxdate, 
                        startdate = @startdate, 
                        enddate = @enddate, 
                        timeval = @timeval, 
                        timeunit = @timeunit, 
                        cd = @cd, 
                        docref = @docref, 
                        notes = @notes, 
                        cu = @cu   
                        WHERE id = @conditionId";
            return ConLocal.Execute(query, new
            {
                trxtype = l.Trxtype,
                pid = l.Pid,
                cid = l.Cid,
                trxdate = l.Trxdate,
                startdate = l.Startdate,
                enddate = l.Enddate,
                timeval = l.Timeval,
                timeunit = l.Timeunit,
                cd = l.Cd,
                docref = l.Docref,
                notes = l.Notes,
                cu = l.Cu,
                conditionId = l.Id
            });

        }
    }
    public class LeaveDataServiceMySql : DbBaseMySql, ILeaveDataService
    {
        LeaveCommand lcmd = new LeaveCommand();
        public int AddLeave(Leaves leave, out string msg)
        {
            try
            {
                msg = "";
                if (GlobalVars.Personals == null)
                {
                    msg = "Personals list is null";
                    return -1;
                }
                Personal p = (from x in GlobalVars.Personals where x.Id == leave.Pid select x).FirstOrDefault();
                if (p == null)
                {
                    msg = "Personal not found";
                    return -1;
                }
                Leaves alv = GetLeaveByPidAndDate(p.Id, leave.Startdate, leave.Enddate, leave.Ph, out msg);
                if (alv == null) { lcmd.Insert(leave); return 1; }
                else { return 0; }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
        }
        public int AddLeaves(List<Leaves> leaves, out string msg)
        {
            msg = ""; int res = 0;
            try
            {
                foreach (Leaves leave in leaves)
                {
                    res += lcmd.Insert(leave);
                }
                return res;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
        }
        public int DeleteLeave(long id, out string msg)
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
        public int DeleteAllLeaves(long pid, out string msg)
        {
            msg = "";
            try
            {
                return lcmd.DeleteAll(pid);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
        }
        public int UpdateLeave(long id, Leaves leave, out string msg)
        {
            msg = "";
            try
            {
                leave.Id = id; 
                lcmd.Update(leave); return 1;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }
        public Leaves GetLeaveById(long id, out string msg)
        {
            msg = ""; Leaves alv = new Leaves();
            string query = "SELECT * FROM alv WHERE id = @id";
            try
            {
                alv = ConLocal.Query<Leaves>(query, new { id = id }).FirstOrDefault();
                return alv;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }

        public List<Leaves> GetAllLeaves(out string msg)
        {
            msg = ""; List<Leaves> alvs = new List<Leaves>();
            string query = "SELECT * FROM alv";
            try
            {
                alvs = ConLocal.Query<Leaves>(query).ToList();
                return alvs;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }
        public List<Leaves> GetAllLeavesByDate(DateTime sd, DateTime ed, out string msg)
        {
            msg = ""; List<Leaves> l = new List<Leaves>();
            string query = "SELECT * FROM alv WHERE startdate >= @sd AND enddate <= @ed";
            try
            {
                l = ConLocal.Query<Leaves>(query, new { sd = sd, ed = ed }).ToList();
                return l;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }

        public List<Leaves> GetLeavesByPersonalId(long pid, out string msg)
        {
            msg = ""; List<Leaves> alvs = new List<Leaves>();
            string query = "SELECT * FROM alv WHERE pid = @pid";
            try
            {
                alvs = ConLocal.Query<Leaves>(query, new { pid = pid }).ToList();
                return alvs;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }
        public List<Leaves> GetLeavesByCompanies(List<int> cids, out string msg)
        {
            msg = ""; List<Leaves> alvs = new List<Leaves>();
            string query = "SELECT * FROM alv WHERE cid IN @cids";
            try
            {
                alvs = ConLocal.Query<Leaves>(query, new { cids = cids.ToArray() }).ToList();
                return alvs;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }
        public List<Leaves> GetLeavesByCompanies(int cid, out string msg)
        {
            msg = ""; List<Leaves> alvs = new List<Leaves>();
            string query = "SELECT * FROM alv WHERE cid IN @cids";
            try
            {
                alvs = ConLocal.Query<Leaves>(query, new { cids = cid }).ToList();
                return alvs;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }
        public List<Leaves> GetLeavesByCompaniesAndDate(List<int> cids, DateTime sd, DateTime ed, out string msg)
        {
            msg = ""; List<Leaves> alvs = new List<Leaves>();
            string query = "SELECT * FROM alv WHERE cid IN @cids AND startdate >= @sd AND enddate <= @ed";
            try
            {
                alvs = ConLocal.Query<Leaves>(query, new { cids = cids.ToArray(), sd = sd, ed = ed }).ToList();
                return alvs;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }
        public Leaves GetLeaveByPidAndDate(long pid, DateTime sd, DateTime ed,  bool ph,out string msg)
        {
            msg = ""; Leaves alv = new Leaves();
            string query = "SELECT * FROM alv WHERE pid = @pid AND startdate >= @sd AND enddate <= @ed AND ph = @ph";
            try
            {
                alv = ConLocal.Query<Leaves>(query, new { pid = pid, sd = sd, ed = ed , ph = ph }).FirstOrDefault();
                return alv;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }

        public List<Leaves> GetLeavesByPidAndDate(long pid, DateTime sd, DateTime ed, out string msg)
        {
            msg = ""; List<Leaves> alvs = new List<Leaves>();
            string query = "SELECT * FROM alv WHERE pid == @pid AND startdate >= @sd AND enddate <= @ed";
            try
            {
                alvs = ConLocal.Query<Leaves>(query, new { pid = pid, sd = sd, ed = ed}).ToList();
                return alvs;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }
    }
}
