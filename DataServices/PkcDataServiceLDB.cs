using Dapper;
using LiteDB;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataServices
{
    public class PkcDataServiceLdb : DbBaseLiteDb, IPkcDataService
    {
        public PkcDataServiceLdb()
        {

            PackageConnect = LdbConnect.GetCollection<Package>("package");
            PackageConnect.EnsureIndex(x => x.Id);

            PrmConnect = LdbConnect.GetCollection<UserPrm>("userprm");
            PrmConnect.EnsureIndex(x => x.Id);
            PrmConnect.EnsureIndex(x => x.Fs);

        }

        public int AddPkc(Package pk, out string msg)
        {
            int t = -1; pk.Id = 1;
            Package p = GetPackageInfo(out msg);
            try
            {
                if (p == null)
                {
                    t = PackageConnect.Insert(pk);
                }
                else
                {
                    t = PackageConnect.Update(pk) ? 1 : 0;
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
            msg = ""; Sapkt remote = new Sapkt();
            try
            {
                remote = ConRemote.QueryFirst<Sapkt>("Select * From sapkt where hc = @hc", new { hc = h });
                ConRemote.Close();
                remote.Pass = DecPass(remote.Pass);

                if (remote.Email != e || remote.Pass != p)
                {
                    return (null, 2);
                }

                return (remote, 1);
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
                Package pkc = PackageConnect.FindById(1);
                return pkc;
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
                if(paket == null) { return new Demo(); }
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
                prm = PrmConnect.FindAll().ToList();
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
                prm = PrmConnect.FindOne(p=> p.Fs == fs);
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
                    t = PrmConnect.Insert(prm);
                    t = t >= 1 ? 1 : 0;
                }
                else
                {
                    return PrmConnect.Update(prm) ? 1 : 0;
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
                return PrmConnect.Delete(id);

            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }

        }
    }
}
