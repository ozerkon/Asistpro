using Models.Common;
using System.Collections.Generic;

namespace DataServices
{
    public interface IPersonalDataService
    {
        int AddPersonal(Personal p, out string msg);
        int AddPersonals(List<Personal> p, out string msg);
        int DeletePersonal(long id, out string msg);

        List<Personal> GetAllPersonals(out string msg);
        List<Personal> GetPersonalsByCompany(int cid, out string msg);

        Personal GetPersonalById(long id, out string msg);
        Personal GetPersonalByTcno(string tcno, out string msg);
    }
}
