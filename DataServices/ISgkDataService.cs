using Models.Domain;
using System;
using System.Collections.Generic;

namespace DataServices
{
    public interface ISgkDataService
    {
        int Add6661(List<Sgk6661> lst, out string msg);
        int AddCr(List<SgkCr> lst, out string msg);
        int AddEt(List<SgkEt> lst, out string msg);
        int AddHl(List<SgkHl> lst, out string msg);
        int AddHlp(List<SgkHlp> lst, out string msg);
        int AddHlp(SgkHlp hlp, out string msg);
        int AddIgl(List<SgkIgl> lst, out string msg);
        int AddMe(List<SgkMe> lst, out string msg);
        int DeleteHLs(List<int> cxs, DateTime tr1, DateTime tr2, out string msg);
        int DeleteHlPs(List<int> cxs, DateTime tr1, DateTime tr2, out string msg);
        (int, int) AddPd(List<SgkDb> lst, out string msg);
        List<Sgk6661> GetAll6661(out string msg);
        List<Sgk6661> GetAll6661(List<int> cxs, string ya, out string msg);
        List<SgkCr> GetAllCr(List<int> cxs, out string msg);
        List<SgkDb> GetAllDb(int cx, string yil, string ay, out string msg);
        List<SgkDb> GetAllDb(List<int> cxs, out string msg);
        List<SgkEt> GetAllEt(List<int> cxs, out string msg);
        List<SgkHl> GetAllHl(out string msg);
        List<SgkHl> GetAllHl(List<int> cxs, DateTime tr1, DateTime tr2, out string msg);
        List<SgkHlp> GetAllHlp(out string msg);
        List<SgkHlp> GetAllHlp(List<int> cxs, DateTime tr1, DateTime tr2, out string msg);
        List<SgkIgl> GetAllIgl(out string msg);
        List<SgkIgl> GetAllIgl(List<int> cxs, DateTime tr1, DateTime tr2, out string msg);
        List<SgkMe> GetAllMe(List<int> cxs, out string msg);
        int RemoveUnapproveds(List<DateTime> tyas, out string msg);
        int GetHlId(SgkHl hl, out string msg);
        int GetHlpId(SgkHlp hlp, out string msg);
        int IsDbExists(int cx, string yil, string ay, out string msg);
        int IsHlExists(SgkHl hl, out string msg);
        int IsHlpExists(SgkHlp hlp, out string msg);
        int IsIglExists(SgkIgl igl, out string msg);
        void RemoveDuplicate();
    }
}