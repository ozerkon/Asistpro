using Models.Common;
using System.Collections.Generic;

namespace DataServices
{
    public interface ILinksDataService
    {
        int AddLink(Links l, out string msg);
        int AddLinks(List<Links> lst, out string msg);
        bool AddRemoveFav(int id, bool isFav, out string msg);
        int DeleteLink(Links l, out string msg);
        bool ExistLink(int id, out string msg);
        bool ExistLink(string head, out string msg);
        List<Links> GetAllLinks(out string msg);
        List<Links> GetAllLinksFromRs(out string msg);
        (string,string) GetCommandFromDb(int id, out string msg);
        List<string> GetDescriptions(out string msg);
        Links GetLinkById(int id, out string msg);
        bool ResetIdColumn(out string msg);
    }
}