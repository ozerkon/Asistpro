using Models.Domain;
using System;
using System.Collections.Generic;

namespace DataServices
{
    public interface IAccrualDataService
    {
        int AddThkk(List<SgkThkk> lst, out string msg);
        int AddThkk(SgkThkk hlp, out string msg);
        int DeleteThkKs(List<int> cxs, DateTime tr1, DateTime tr2, out string msg);
        List<SgkThkk> GetAllThkk(out string msg); 
        List<SgkThkk> GetAllThkk(List<int> cxs, DateTime tr1, DateTime tr2, out string msg);
        int IsThkkExists(SgkThkk hlp, out string msg);

    }
}
