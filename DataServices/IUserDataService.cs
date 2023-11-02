using Models.Common;
using System.Collections.Generic;

namespace DataServices
{
    public interface IUserDataService
    {
        int AddUser(Users us, out string msg);
        string DecPass(string ps);
        int DeleteUser(Users us, out string msg);
        string EncPass(string ps);
        bool ExistUser(int id, out string msg);
        (int, int) ExistUser(string un, out string msg);
        (bool, Users) ExistUser(string un, string up, out string msg);
        List<Users> GetAllUsers(out string msg);
        Users GetUser(int id, out string msg);
        Users GetUserByUn(string un, out string msg);
        string GetUserHc(out string msg);
        int ReAddUser(Users us, out string msg);
        bool ResetIdColumn(out string msg);
        (bool isSuccess, string message) UserLogin(string un, string up, out string msg);
    }
}