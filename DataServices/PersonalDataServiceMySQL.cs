using Dapper;
using Models.Common;
using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataServices
{
    public class PersonalCommand : DbBaseMySql
    {
        public int Insert(Personal p)
        {
            string query = "INSERT INTO personal VALUES(null, @tcno, @ads, @cid , @dtr, @igt, @ict, @tih, @active)";
            return ConLocal.Execute(query, new
            {
                tcno = p.Tcno,
                ads = p.Ads,
                cid = p.Cid,
                dtr = p.Dtr,
                igt = p.Igt,
                ict = p.Ict,
                tih = p.Tih,
                active = p.Active
            });
        }
        public int Delete(long id)
        {
            string query = "DELETE FROM personal WHERE id = @id";
            return ConLocal.Execute(query, new { id = id });

        }
        public int Update(Personal p)
        {
            string query = @"UPDATE personal SET tcno = @tcno, ads = @ads, cid = @cid,  dtr = @dtr,  igt = @igt, ict = @ict,  tih = @tih,  active = @active WHERE id = @conditionId";

                return ConLocal.Execute(query, new
                {
                    tcno = p.Tcno,
                    ads = p.Ads,
                    cid = p.Cid,
                    dtr = p.Dtr,
                    igt = p.Igt,
                    ict = p.Ict,
                    tih = p.Tih,
                    active = p.Active,
                    conditionId = p.Id
                });
  

        }
    }
    public class PersonalDataServiceMySql : DbBaseMySql, IPersonalDataService
    {
        PersonalCommand lcmd = new PersonalCommand();
        public int AddPersonal(Personal p, out string msg)
        {
            try
            {
                Personal personal = GetPersonalByTcno(p.Tcno, out msg);
                if (personal == null) { lcmd.Insert(p); return 1; }
                else { lcmd.Update(p); return 2; }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
        }

        public int AddPersonals(List<Personal> p, out string msg)
        {
            msg = ""; int res = 0;
            try
            {
                foreach (Personal pers in p)
                {
                    res += lcmd.Insert(pers);
                }
                return res;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
        }

        public int DeletePersonal(long id, out string msg)
        {
            msg = "";
            try
            {
                return lcmd.Delete(id);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
        }

        public List<Personal> GetAllPersonals(out string msg)
        {
            msg = ""; List<Personal> prs = new List<Personal>();
            string query = "SELECT * FROM personal";
            try
            {
                prs = ConLocal.Query<Personal>(query).ToList();
                return prs;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }

        public Personal GetPersonalById(long id, out string msg)
        {
            msg = ""; Personal prs = new Personal();
            string query = "SELECT * FROM personal WHERE id = @id";
            try
            {
                prs = ConLocal.Query<Personal>(query, new { id = id }).FirstOrDefault();
                return prs;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }

        public Personal GetPersonalByTcno(string tcno, out string msg)
        {
            msg = ""; Personal prs = new Personal();
            string query = "SELECT * FROM personal WHERE tcno = @tcno";
            try
            {
                prs = ConLocal.Query<Personal>(query, new { tcno = tcno }).FirstOrDefault();
                return prs;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }

        public List<Personal> GetPersonalsByCompany(int cid, out string msg)
        {
            msg = ""; List<Personal> prs = new List<Personal>();
            string query = "SELECT * FROM personal WHERE cid = @cid";
            try
            {
                prs = ConLocal.Query<Personal>(query, new { cid = cid}).ToList();
                return prs;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }
       
    }
}
