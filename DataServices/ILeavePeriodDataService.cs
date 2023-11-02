using Models.Domain;
using System;
using System.Collections.Generic;

namespace DataServices
{
    public interface ILeavePeriodDataService
    {
        int AddLeavePeriod(LeavePeriod period, out string msg);
        int AddLeavePeriods(List<LeavePeriod> periods, out string msg);
        int DeleteLeavePeriod(long id, out string msg);
        int DeleteLeavePeriodsByTcno(string tcno, out string msg);
        int UpdateLeavePeriod(long id, LeavePeriod period, out string msg);
        LeavePeriod GetLeavePeriodById(long id, out string msg);
        List<LeavePeriod> GetLeavePeriods(out string msg);
        List<LeavePeriod> GetLeavePeriodsByTcno(string tcno, out string msg);
        List<LeavePeriod> GetLeavePeriodByTcnoAndDate(string tcno, DateTime sd, DateTime ed, out string msg);
        List<LeavePeriod> GetLeavePeriodsByCompany(int cid, out string msg);
        List<LeavePeriod> GetLeavePeriodsByCompanyAndDate(int cid, DateTime sd, DateTime ed, out string msg);
    }
}
