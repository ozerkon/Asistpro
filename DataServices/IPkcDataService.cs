using Models.Common;
using System.Collections.Generic;

namespace DataServices
{
    public interface IPkcDataService
    {
        int AddPkc(Package pk, out string msg);
        (Sapkt, int) CheckPkc(string e, string p, string h, out string msg);
        Package GetPackageInfo(out string msg);
        List<Sapkt> GetAllPackage(out string msg);
        int UpdatePkc(Sapkt pkc, out string msg);
        Demo CheckDemoPkc(string discid, out string msg);
        int UpdateDemoPkc(Demo pkc, out string msg);
        int AddDemoPkc(Demo pkc, out string msg);
        string DecPass(string ps);
        string EncPass(string ps);
        
        Surum GetNewVersion(out string msg);

        List<UserPrm> GetUserPrms(out string msg);
        UserPrm GetUserPrm(int fs, out string msg);
        int SetUserPrm(UserPrm prm, out string msg);
        bool DeleteUserPrm(long id, out string msg);
    }
}