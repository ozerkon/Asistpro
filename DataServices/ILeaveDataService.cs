using Models.Domain;
using System;
using System.Collections.Generic;

namespace DataServices
{
    public interface ILeaveDataService
    {
        int AddLeave(Leaves leave, out string msg);
        int AddLeaves(List<Leaves> leaves, out string msg);
        
        int DeleteLeave(long id, out string msg);
        int DeleteAllLeaves(long pid, out string msg);
        int UpdateLeave(long id, Leaves leave, out string msg);
        List<Leaves> GetAllLeaves(out string msg);
        List<Leaves> GetAllLeavesByDate(DateTime sd, DateTime ed, out string msg);
        
        Leaves GetLeaveById(long id, out string msg);
       
        List<Leaves> GetLeavesByCompanies(List<int> cids, out string msg);
        List<Leaves> GetLeavesByCompanies(int cid, out string msg);

        List<Leaves> GetLeavesByCompaniesAndDate(List<int> cids, DateTime sd, DateTime ed, out string msg);
        
        List<Leaves> GetLeavesByPersonalId(long pid, out string msg);
        Leaves GetLeaveByPidAndDate(long pid, DateTime sd, DateTime ed, bool ph, out string msg);
        List<Leaves> GetLeavesByPidAndDate(long pid, DateTime sd, DateTime ed, out string msg);

    }
}