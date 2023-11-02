using Models.Common;
using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataServices
{
    public class LeaveDataServiceLdb : DbBaseLiteDb, ILeaveDataService
    {
        public LeaveDataServiceLdb()
        {
            LeavesConnect = LdbConnect.GetCollection<Leaves>("leaves");
            LeavesConnect.EnsureIndex(x => x.Id);
        }
        public int AddLeave(Leaves leave, out string msg)
        {
            try
            {
                Personal p = (from x in GlobalVars.Personals where x.Id == leave.Pid select x).FirstOrDefault();
                int result = -1; 
                Leaves alv = GetLeaveByPidAndDate(p.Id, leave.Startdate, leave.Enddate, leave.Ph , out msg);
                if (alv !=null)
                {
                    result =  0; 
                }
                else
                {
                    result = LeavesConnect.Insert(leave).AsInt32;
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
        public int AddLeaves(List<Leaves> leaves, out string msg)
        {
            int result = 0; msg = "";
            try
            {
                foreach (Leaves alv in leaves)
                {
                    result += AddLeave(alv, out msg);
                }
                return result;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }

        public int DeleteLeave(long id, out string msg)
        {
            msg = "";
            try
            {
                return LeavesConnect.Delete(id) ? 1 : 0;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }
        public int DeleteAllLeaves(long pid, out string msg)
        {
            msg = "";
            try
            {
                return LeavesConnect.DeleteMany(q => q.Pid == pid);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }
        public int UpdateLeave(long id, Leaves leave, out string msg)
        {
            int result = -1; bool isOk; msg = "";
            try
            {
                leave.Id = id;
                isOk = LeavesConnect.Update(leave);
                result = isOk ? 1 : 0; // güncelleme başarılıysa result değerini 1 yap
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                result = -1;
            }
            return result;
        }
        public Leaves GetLeaveById(long id, out string msg)
        {
            msg = ""; Leaves alv;
            try
            {
                alv = LeavesConnect.FindById(id);
                return alv;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public List<Leaves> GetAllLeaves(out string msg)
        {
            msg = "";
            List<Leaves> alvs = null;
            try
            {
                alvs = LeavesConnect.FindAll().ToList();
                return alvs;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }

        public List<Leaves> GetAllLeavesByDate(DateTime sd, DateTime ed, out string msg)
        {
            msg = "";
            List<Leaves> alvs = null;
            try
            {
                alvs = LeavesConnect.Find(q => q.Startdate >= sd && q.Enddate <= ed).ToList();
                return alvs;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public List<Leaves> GetLeavesByPersonalId(long pid, out string msg)
        {
            msg = "";
            List<Leaves> alvs = null;
            try
            {
                alvs = LeavesConnect.Find(q => q.Pid == pid).ToList();
                return alvs;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public List<Leaves> GetLeavesByCompanies(List<int> cids, out string msg)
        {
            msg = "";
            List<Leaves> lst = null;
            try
            {
                lst = LeavesConnect.Find(q => cids.Contains(Convert.ToInt32(q.Cid))).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public List<Leaves> GetLeavesByCompanies(int cid, out string msg)
        {
            msg = "";
            List<Leaves> lst = null;
            try
            {
                lst = LeavesConnect.Find(q => q.Cid == cid).ToList();
                return lst;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public List<Leaves> GetLeavesByCompaniesAndDate(List<int> cids, DateTime sd, DateTime ed, out string msg)
        {
            msg = "";
            List<Leaves> alvs = null;
            try
            {
                alvs = LeavesConnect.Find(q => cids.Contains(Convert.ToInt32(q.Cid)) && q.Startdate >= sd && q.Enddate <= ed).ToList();
                return alvs;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public Leaves GetLeaveByPidAndDate(long pid, DateTime sd, DateTime ed, bool ph, out string msg)
        {
            msg = "";  Leaves alv = null;
            try
            {
                alv = LeavesConnect.Find(q => q.Pid == pid && q.Startdate == sd && q.Enddate == ed && q.Ph == ph).FirstOrDefault();
                return alv;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public List<Leaves> GetLeavesByPidAndDate(long pid, DateTime sd, DateTime ed, out string msg)
        {
            msg = "";
            List<Leaves> alvs = null;
            try
            {
                alvs = LeavesConnect.Find(q => q.Pid == pid && q.Startdate >= sd && q.Enddate <= ed).ToList();
                return alvs;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }

    }
}
