using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataServices
{
    public class PersonalDataServiceLDB : DbBaseLiteDb, IPersonalDataService
    {
        public PersonalDataServiceLDB()
        {
            PersonalConnect = LdbConnect.GetCollection<Personal>("personals");
            PersonalConnect.EnsureIndex(x => x.Id);
        }
        public int AddPersonal(Personal P, out string msg)
        {
            int result = -1; bool isOK; msg = "";
            Personal p = GetPersonalByTcno(P.Tcno, out msg); 
            try
            {
                if (p.Cid > 0)
                {
                    P.Id = p.Id;
                    isOK = PersonalConnect.Update(P);
                    result = isOK ? 2 : 0; // güncelleme başarılıysa result değerini 2 yap
                }
                else
                {
                    result = PersonalConnect.Insert(P).AsInt32;
                    result = (result >= 1) ? 1 : 0;
                }

                return result;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }

        public int AddPersonals(List<Personal> P, out string msg)
        {
            int result = 0; msg = "";
            try
            {
                foreach (Personal p in P)
                {
                    result += AddPersonal(p, out msg);
                }
                return result;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }

        public int DeletePersonal(long id, out string msg)
        {
            msg = "";
            try
            {
                return (PersonalConnect.Delete(id)) ? 1 : 0;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();

                return 0;
            }
        }

        public List<Personal> GetAllPersonals(out string msg)
        {
            msg = "";
            List<Personal> P = null;
            try
            {
                P = PersonalConnect.FindAll().ToList();
                return P;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();

                return null;
            }
        }

        public Personal GetPersonalById(long id, out string msg)
        {
            msg = ""; Personal p;
            try
            {
                p = PersonalConnect.FindById(id);
                return p;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }

        public Personal GetPersonalByTcno(string tcno, out string msg)
        {
            msg = ""; Personal p;
            try
            {
                p = PersonalConnect.FindOne(x => x.Tcno == tcno);
                if (p == null) return new Personal();
                return p;
            }
            catch (System.Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }

        public List<Personal> GetPersonalsByCompany(int cid, out string msg)
        {
            msg = "";
            List<Personal> P = null;
            try
            {
                P = PersonalConnect.Find(q => q.Cid == cid).ToList();
                return P;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
    }
}
