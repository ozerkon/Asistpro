using Dapper;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataServices
{
    public class PkcDataServiceMySql : DbBaseMySql, IPkcDataService
    {
        public class PrmCommands : DbBaseMySql
        {

            public int Insert(UserPrm p, out string msg)
            {
                msg = "";
                int result = -1;
                string query = "INSERT INTO userprm VALUES(null,  @sid, @fs, @cd, @hbrwsr, @acpt, @dbrwsr, @kbrwsr, @clsBrowsers, @sira, @cid, @cid2, @sp, @cp, @gun, @gp, @gs, @scd1, @scd2, @scd3, @scd4, @scd5)";
                try
                {
                    result = ConLocal.Execute(query, new { sid = p.Sid, fs = p.Fs, cd = p.Cd, hbrwsr = p.HideBrowser, acpt = p.AutoCaptcha, dbrwsr = p.DefaultBrowser, kbrwsr = p.KeepBrowser, clsBrowsers = p.CloseAllBrowsers, sira = p.Sira, cid = p.Cid, cid2 = p.Cid2, sp = p.Sp, cp = p.Cp, gun = p.Gun, gp = p.Gp, gs = p.Gs, scd1 = p.Scd1, scd2 = p.Scd2, scd3 = p.Scd3, scd4 = p.Scd4, scd5 = p.Scd5 });
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    return -1;
                }
                return result;
            }
            public int Delete(int id, out string msg)
            {
                msg = ""; 
                int result = -1;
                string query = "DELETE FROM userprm WHERE id = @id";
                try
                {
                    result  = ConLocal.Execute(query, new { id = id });
                }
                catch (Exception ex )
                {
                    msg = ex.Message;
                    return -1;
                }
                return result;

            }
            public int Update(UserPrm p, out string msg)
            {
                msg = "";
                string query = @"UPDATE userprm SET sid = @sid,  fs = @fs, cd = @cd, hideBrowser = @hbrwsr, autoCaptcha = @acpt, defaultBrowser = @dbrwsr, keepBrowser = @kbrwsr, closeAllBrowsers = @clsBrowsers, sira = @sira, cid = @cid, cid2 = @cid2, sp = @sp, cp = @cp, gun = @gun, gp = @gp, gs = @gs, scd1 = @scd1, scd2 = @scd2, scd3 = @scd3, scd4 = @scd4, scd5 = @scd5 WHERE id = @conditionId";
                int result = -1;
                try
                {
                    result = ConLocal.Execute(query, new { sid = p.Sid, fs = p.Fs, cd = p.Cd, hbrwsr = p.HideBrowser, acpt = p.AutoCaptcha, dbrwsr = p.DefaultBrowser, clsBrowsers = p.CloseAllBrowsers, kbrwsr = p.KeepBrowser, sira = p.Sira, cid = p.Cid, cid2 = p.Cid2, sp = p.Sp, cp = p.Cp, gun = p.Gun, gp = p.Gp, gs = p.Gs, scd1 = p.Scd1, scd2 = p.Scd2, scd3 = p.Scd3, scd4 = p.Scd4, scd5 = p.Scd5, conditionId = p.Id });
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    return -1;
                }
                return result;
            }
        }
        public class PckCommands : DbBaseMySql
        {

            public int Insert(Package p)
            {
                string query = "INSERT INTO package VALUES(null, @psc,@pep,@pps,@ptr,@pla)";
                return ConLocal.Execute(query, new
                {
                    psc = p.Psc,
                    pep = p.Pep,
                    pps = p.Pps,
                    ptr = p.Ptr,
                    pla = p.Pla
                });
            }
            public int Delete(int id)
            {
                string query = "DELETE FROM package WHERE id = @id";
                return ConLocal.Execute(query, new { id = id });

            }
            public int Update(Package p)
            {
                string query = @"UPDATE userprm SET 
                        psc = @psc,
                        pep = @pep,
                        pps = @pps,
                        ptr = @ptr,
                        pla = @pla
                    WHERE id = @conditionId";
                return ConLocal.Execute(query, new
                {
                    id = p.Id,
                    psc = p.Psc,
                    pep = p.Pep,
                    pps = p.Pps,
                    ptr = p.Ptr,
                    pla = p.Pla,
                    conditionId = p.Id
                });

            }
        }

        PrmCommands prmcmd = new PrmCommands();
        PckCommands pckcmd = new PckCommands();
        public List<Package> GetAllPackage()
        {
            try
            {
                string query = "SELECT id,psc,pep,pps,ptr,pla from package";
                List<Package> lst = ConLocal.Query<Package>(query).ToList();
                return lst;
            }
            catch (Exception )
            {
                return null;
            }
        }

        public int AddPkc(Package pk, out string msg)
        {
            int t = -1;
            Package p = GetPackageInfo(out msg);
            try
            {
                if (p == null)
                {
                    t = pckcmd.Insert(pk);
                }
                else
                {
                    t = pckcmd.Update(pk) > 0 ? 1 : 0;
                }

                return t;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return 0;
            }
        }
        public (Sapkt, int) CheckPkc(string e, string p, string h, out string msg)
        {
            msg = ""; Sapkt paket = new Sapkt();
            try
            {
                List<Sapkt> xx = GetAllPackage(out msg);
                paket = ConRemote.QueryFirst<Sapkt>("Select * From sapkt where hc = @hc", new { hc = h });
                string pass = DecPass(xx[5].Pass);
                ConRemote.Close();
                paket.Pass = DecPass(paket.Pass);

                if (paket.Email != e || paket.Pass != p)
                {
                    return (null, 2);
                }

                return (paket, 1);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                if (msg.Contains("içermiyor")) { return (null, 3); }
                else return (null, 4);
            }
        }
        public Package GetPackageInfo(out string msg)
        {
            msg = "";
            try
            {
                List<Package> lst = GetAllPackage();
                Package pck = lst.OrderByDescending(x => x.Id).FirstOrDefault();
                return pck;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public List<Sapkt> GetAllPackage(out string msg)
        {
            msg = "";

            try
            {
                List<Sapkt> r = new List<Sapkt>();
                r = ConRemote.Query<Sapkt>("Select * From sapkt").ToList();
                ConRemote.Close();
                return r;

            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                ConRemote.Close();
                return null;
            }

        }
        public int UpdatePkc(Sapkt pkc, out string msg)
        {
            msg = "";
            int sonuc = 0;
            try
            {
                int active = Convert.ToInt32(pkc.Active);
                string sql = "UPDATE sapkt SET active = @p1, discid = @p2, mac = @p3, sat = @p4, ubt = @p5 WHERE id = @p6";
                sonuc = ConRemote.Execute(sql, new { p1 = active, p2 = pkc.Discid, p3 = pkc.Mac, p4 = pkc.Sat, p5 = pkc.Ubt, p6 = pkc.Id });
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return sonuc;
        }

        public Demo CheckDemoPkc(string discid, out string msg)
        {
            msg = ""; Demo paket = new Demo();
            try
            {
                paket = ConRemote.QueryFirstOrDefault<Demo>("Select * From demo where discid = @discid", new { discid = discid });
                ConRemote.Close();
                if (paket == null) { return new Demo(); }
                return paket;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public int UpdateDemoPkc(Demo pkc, out string msg)
        {
            msg = "";
            int sonuc = 0;
            try
            {
                string sql = "UPDATE demo SET sat = @sat, ubt = @ubt WHERE discid = @discid";
                sonuc = ConRemote.Execute(sql, new { sat = pkc.Sat, ubt = pkc.Ubt, discid = pkc.Discid });
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return sonuc;
        }
        public int AddDemoPkc(Demo pkc, out string msg)
        {
            msg = "";
            int sonuc = 0;
            try
            {
                string sql = "INSERT INTO demo(sat, ubt, discid, mac, inuse) VALUES(@sat, @ubt, @discid, @mac, NULL)";
                sonuc = ConRemote.Execute(sql, new { sat = pkc.Sat, ubt = pkc.Ubt, discid = pkc.Discid, mac = pkc.Mac });
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return sonuc;
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
        
        public Surum GetNewVersion(out string msg)
        {
            msg = "";
            try
            {
                Surum srm = ConRemote.Query<Surum>("Select * From surum").SingleOrDefault();
                return srm;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return new Surum() { V = "error" };
            }
            finally { ConRemote.Close(); }
        }

        public List<UserPrm> GetUserPrms(out string msg)
        {
            msg = ""; List<UserPrm> prm = new List<UserPrm>();
            try
            {
                string sql = $"SELECT * from userprm";
                prm = ConLocal.Query<UserPrm>(sql).ToList();
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
            return prm;

        }
        public UserPrm GetUserPrm(int fs, out string msg)
        {
            msg = ""; UserPrm prm = new UserPrm();
            try
            {
                string sql = $"SELECT * from userprm WHERE fs = @p1";
                prm = ConLocal.Query<UserPrm>(sql, new { p1 = fs }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
            return prm;

        }
        public int SetUserPrm(UserPrm prm, out string msg)
        {
            int t = -1;
            UserPrm p = GetUserPrm(prm.Fs, out msg);
            try
            {
                if (p == null)
                {
                    t = prmcmd.Insert(prm, out msg);
                    t = t >= 1 ? 1 : 0;
                }
                else
                {
                    prm.Id = p.Id;
                    return prmcmd.Update(prm, out msg) > 0 ? 1 : 0;
                }

                return t;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return 0;
            }
        }
        public bool DeleteUserPrm(long id, out string msg)
        {
            msg = ""; id = Convert.ToInt32(id);
            try
            {
                return prmcmd.Delete(Convert.ToInt16(id), out msg) > 0 ? true : false;

            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }

        }
    }
}
