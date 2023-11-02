using Models.Common;
using System.Collections.Generic;

namespace DataServices
{
    public interface ICompanyDataService
    {
        bool FillCompanyCheck(out string msg);
        int AddSgscEnc(string sgsc, out string msg);
        int UpdateSgscEnc(string sgsc, out string msg);
        string GetSgscEnc(out string msg);
        int AddCompany(Company c, int mcc, out string msg);
        int DeleteCompany(Company c, string sgscEnc, out string msg);
        Company ExistCompanyByRegNo(string regNo, out string msg);
        List<Company> GetCompanies(out string msg);
        List<Company> GetCompaniesByFm(int fm, out string msg);
        Company GetCompanyById(int id, out string msg);
        Company GetCompanyByNameAndRegNo(string fa, string ssn, out string msg);
        Company GetCompanyBySgkIds(string cid, string cid2, out string msg);
        Dictionary<string, int> GetCompanyCenters(out string msg);
        int GetCompanyId(string companyId, string companyId2, out string msg);
        string GetCompanyNameById(int id, out string msg);
        int GetLastId(out string msg);
        void UpdateFm(Company comp, int fm, out string msg);
        void ResetCompanyId(out string msg);
        void Copycomp();
    }
}