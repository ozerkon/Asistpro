using Dapper;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataServices
{
    public class UserDataServiceMySql : DbBaseMySql, IUserDataService
    {
        public class UsersCommand : DbBaseMySql
        {

            public int Insert(Users u)
            {
                string query = "INSERT INTO users VALUES(null, @unm, @uln, @un, @up, @upp, @uy, @cd, @hc, @cu)";
                return ConLocal.Execute(query, new
                {
                    unm = u.Unm,
                    uln = u.Uln,
                    un = u.Un,
                    up = u.Up,
                    upp = u.Upp,
                    uy = u.Uy,
                    cd = u.Cd,
                    hc = u.Hc,
                    cu = u.Cu
                });
            }
            public int Delete(int id)
            {
                string query = "DELETE FROM users WHERE id = @id";
                return ConLocal.Execute(query, new { id = id });

            }
            public int Update(Users u)
            {
                string query = @"UPDATE users SET 
                        unm = @unm,
                        uln = @uln,
                        un = @un,
                        up = @up,
                        upp = @upp,
                        uy = @uy,
                        cd = @cd,
                        hc = @hc,
                        cu = @cu
                    WHERE id = @conditionId";
                return ConLocal.Execute(query, new
                {
                    unm = u.Unm,
                    uln = u.Uln,
                    un = u.Un,
                    up = u.Up,
                    upp = u.Upp,
                    uy = u.Uy,
                    cd = u.Cd,
                    hc = u.Hc,
                    cu = u.Cu,
                    conditionId = u.Id
                });

            }
        }

        UsersCommand ucmd = new UsersCommand();

        public List<Users> GetAllUsers(out string msg)
        {
            msg = "";
            List<Users> u = new List<Users>();
            try
            {
                string query = "SELECT id,unm,uln, un, up,upp,uy,cd,hc,cu from users";
                u = ConLocal.Query<Users>(query).ToList();
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
                Users u = GetAllUsers(out msg).OrderByDescending(x => x.Id).FirstOrDefault();
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
                u = GetAllUsers(out msg).Find(usr => usr.Id == id);
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

                u = GetAllUsers(out msg).Find(x => x.Un == un);
                //string query = "SELECT id, unm, uln, un, up, upp, uy, cd, hc, cu from users WHERE unm = @p1";
                //u = con.QueryAsync(query, new { p1 = un }).Result.FirstOrDefault();
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
                int res, uid;
                (res, uid) = ExistUser(us.Un, out msg);
                if (res > 0)
                {
                    us.Id = uid;
                    isOk = ucmd.Update(us) > 0 ? true : false;
                    result = (isOk) ? 2 : 0; // güncelleme başarılıysa result değerini 2 yap
                }
                else
                {
                    result = ucmd.Insert(us);
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
                return (ucmd.Insert(us) > 0) ? 1 : 0;
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
                return (ucmd.Delete(us.Id)) > 0 ? 1 : 0;
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
                ConLocal.Execute("TRUNCATE TABLE users");
                foreach (var u in usr)
                {
                    ucmd.Insert(u);
                }
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
                        if (un.Trim() != us.Unm || up.Trim() != us.Up)
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

