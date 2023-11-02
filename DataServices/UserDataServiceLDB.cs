using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataServices
{
    public class UserDataServiceLdb : DbBaseLiteDb, IUserDataService
    {
        public UserDataServiceLdb()
        {
            UsersConnect = LdbConnect.GetCollection<Users>("users");
            UsersConnect.EnsureIndex(x => x.Un);
        }

        public List<Users> GetAllUsers(out string msg)
        {
            msg = "";
            List<Users> u = null;
            try
            {
                u = UsersConnect.FindAll().ToList();
                return u;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();

                return null;
            }

        }

        public string GetUserHc(out string msg)
        {
            msg = "";
            try
            {
                Users u = UsersConnect.FindById(1);
                return u.Hc;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();

                return String.Empty;
            }
        }

        public bool ExistUser(int id, out string msg)
        {
            msg = "";
            try
            {
                List<Users> users = GetAllUsers(out msg);
                foreach (var u in users)
                {
                    if (u.Id == id)
                    {

                        return true;
                    }
                }

                return false;
            }
            catch (System.Exception ex)
            {
                msg = ex.Message.ToString();

                return false;
            }
        }

        public (int, int) ExistUser(string un, out string msg)
        {
            try
            {
                List<Users> users = GetAllUsers(out msg);
                foreach (var u in users)
                {
                    if (u.Un == un)
                    {

                        return (1, u.Id);
                    }
                }

                return (0, 0);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return (2, 0);
            }
        }

        public (bool, Users) ExistUser(string un, string up, out string msg)
        {

            msg = "";
            List<Users> lst = new List<Users>();
            try
            {
                lst = GetAllUsers(out msg);
                foreach (Users ux in lst)
                {
                    if (ux != null && ux.Un == un && DecPass(ux.Up) == up)
                    {

                        return (true, ux);
                    }
                }
                return (false, null);

            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();

                return (false, null);
            }
        }

        public Users GetUser(int id, out string msg)
        {
            msg = ""; Users u;
            try
            {
                u = UsersConnect.FindById(id);
                return u;
            }
            catch (System.Exception ex)
            {
                msg = ex.Message.ToString();

                return null;
            }
        }

        public Users GetUserByUn(string un, out string msg)
        {
            msg = ""; Users u;
            try
            {
                u = UsersConnect.FindOne(x => x.Un == un);
                return u;
            }
            catch (System.Exception ex)
            {
                msg = ex.Message.ToString();

                return null;
            }
        }

        public int AddUser(Users us, out string msg)
        {
            int result = -5; bool isOk;
            try
            {
                if (ExistUser(us.Id, out msg))
                {
                    isOk = UsersConnect.Update(us);
                    result = (isOk) ? 2 : 0; // güncelleme başarılıysa result değerini 2 yap
                }
                else
                {
                    result = UsersConnect.Insert(us).AsInt32;
                    result = (result >= 1) ? 1 : 0;
                }

                return result;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return 0;
            }

        }

        public int ReAddUser(Users us, out string msg)
        {
            msg = "";
            try
            {
                us.Id = 0;
                return (UsersConnect.Insert(us).AsInt32 > 0) ? 1 : 0;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return 0;
            }

        }

        public int DeleteUser(Users us, out string msg)
        {
            msg = "";
            try
            {
                return (UsersConnect.Delete(us.Id)) ? 1 : 0;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();

                return 0;
            }
        }

        public bool ResetIdColumn(out string msg)
        {
            try
            {
                List<Users> usr = GetAllUsers(out msg);

                LdbConnect.Execute("DROP COLLECTION temp");
                foreach (var u in usr)
                {
                    LdbConnect.Execute("INSERT INTO temp:INT VALUES {" + $"unm:'{u.Unm}', uln:'{u.Uln}', un:'{u.Un}', up:'{u.Up}', upp:'{u.Upp}', uy:'{u.Uy}', cd:" + "{" + $"\"$date\":\"{u.Cd}\" " + "}" + $", hc:'{u.Hc}', cu:{u.Cu}" + "}");

                }
                LdbConnect.Execute("DROP COLLECTION users");
                LdbConnect.Execute("RENAME COLLECTION temp TO users");
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();

                return false;
            }

            return true;
        }

        public (bool isSuccess, string message) UserLogin(string un, string up, out string msg)
        {
            msg = "";
            string message = "";
            bool isSuccess = true;
            try
            {
                if (un.Trim() != "" && up.Trim() != "")
                {
                    List<Users> lst = GetAllUsers(out msg);
                    foreach (Users us in lst)
                    {
                        if (un.Trim() != us.Un || up.Trim() != us.Up)
                        {
                            message = "Kullanıcı adı veya şifre hatalı";
                            isSuccess = false;
                        }
                    }
                }
                else
                {
                    message = "Kullanıcı adı ve şifresi boş olamaz";
                    isSuccess = false;
                }

                return (isSuccess, message);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();

                return (false, msg);
            }

        }

        public string EncPass(string ps)
        {
            Random rd = new Random();
            StringBuilder sb = new StringBuilder();
            for (int a = 0; a < 4; a++)
            {
                sb.Append(Convert.ToChar(rd.Next(33, 126)).ToString());
            }
            int i = 1;
            foreach (char c in ps)
            {
                sb.Append(c.ToString());
                for (int a = 0; a < 5; a++)
                {
                    sb.Append(Convert.ToChar(rd.Next(33, 126)).ToString());
                }
                i++;
            }

            return sb.ToString();
        }

        public string DecPass(string ps)
        {
            StringBuilder sb = new StringBuilder();
            int l = ps.Length;
            sb.Append(ps[4]);
            for (int i = 10; i < ps.Length; i += 6)
            {
                sb.Append(ps[i]);
                if (i == ps.Length - 1) break;

            }
            return sb.ToString();
        }
    }
}
