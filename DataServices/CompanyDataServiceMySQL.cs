using Dapper;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataServices
{
    public class CompanyDataServiceMySql : DbBaseMySql, ICompanyDataService
    {


        public bool FillCompanyCheck(out string msg)
        {
            msg = "";
            CompanyCheck cc = new CompanyCheck();
            string sql = "SELECT id, sgscenc FROM companycheck WHERE id = 1 AND sgscenc LIKE @p1";
            try
            {
                cc = ConLocal.QueryAsync< CompanyCheck>(sql, new { p1 = GlobalVars.CmpCheck + "%"}).Result.FirstOrDefault();
                if (cc == null)
                {
                    sql = "INSERT INTO companycheck( id, sgscenc) VALUES (1, @p1)";
                    ConLocal.ExecuteAsync(sql, new { p1 = GlobalVars.CmpCheck });
                }
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }
        public int AddSgscEnc(string sgsc, out string msg)
        {
            msg = "";
            string sql = "UPDATE companycheck SET sgscenc = CONCAT(sgscenc, @p1)";
            try
            {
                int result = ConLocal.ExecuteAsync(sql, new { p1 = sgsc }).Result;
                return result;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }
        public int UpdateSgscEnc(string sgsc, out string msg)
        {
            msg = "";
            string sql = "UPDATE companycheck SET sgscenc = @p1";
            try
            {
                int result = ConLocal.ExecuteAsync(sql, new { p1 = sgsc }).Result;
                return result;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }
        public string GetSgscEnc(out string msg) {
            msg = "";
            try
            {
                return ConLocal.QueryAsync<string>("SELECT sgscenc FROM companycheck WHERE id = 1").Result.First();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }
        public List<Company> GetCompanies(out string msg)
        {
            List<Company> lstCom = new List<Company>();
            msg = ""; 
            try
            {
                lstCom = ConLocal.QueryAsync<Company>("SELECT id, CompanyName, CompanyID, CompanyID2, SystemPassword, CompanyPassword, fm, gun, gp, gs, sgsc, unvan, adres, sgm, kka, kkc, sc1, sc2, sc3, sc4, sc5, cu, cd FROM company").Result.ToList();
                if (lstCom != null) lstCom = EncryptDs.DecryptCompany(lstCom);
                return lstCom;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }
        public List<Company> GetCompaniesByFm(int fm, out string msg)
        {
            List<Company> lstCom = new List<Company>();
            msg = ""; 
            try
            {
                string sql = "SELECT id, CompanyName, CompanyID, CompanyID2, SystemPassword, CompanyPassword, fm, gun, gp, gs, sgsc, unvan, adres, sgm, kka, kkc, sc1, sc2, sc3, sc4, sc5, cu, cd FROM company WHERE fm = @p1 AND id = @p2";
                lstCom = ConLocal.QueryAsync<Company>(sql, new { p1 = fm, p2 = fm}).Result.ToList();
                if (lstCom != null) lstCom = EncryptDs.DecryptCompany(lstCom);
                return lstCom;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public int GetCompanyId(string cid, string cid2, out string msg)
        {
            msg = "";
            //cid = EncryptDS.EncryptString(cid, GlobalVars.passPhrase);
            //cid2 = EncryptDS.EncryptString(cid2, GlobalVars.passPhrase);
            try
            {
                string sql = "SELECT id FROM company WHERE CompanyID = @p1 AND CompanyID2 = @p2";
                Company cx = ConLocal.QueryAsync<Company>(sql, new { p1 = cid, p2 = cid2 }).Result.FirstOrDefault();
                if (cx == null)
                {
                    return 0;
                }
                return cx.Id;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public Company ExistCompanyByRegNo(string regNo, out string msg)
        {
            msg = "";
            if (regNo == string.Empty)
            {
                msg = "Sicil no gelmedi"; return null;
            }
            //regNo = EncryptDS.EncryptString(regNo, GlobalVars.passPhrase);
            List<Company> comps = GetCompanies(out msg);
            try
            {
                if (comps != null)
                {
                    foreach (Company c in comps)
                    {
                        if (c.Sgsc == regNo)
                        {
                            return c;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
            return null;
        }
        public Company GetCompanyByNameAndRegNo(string cn, string regNo, out string msg)
        {
            msg = ""; Company c;
            try
            {
                if (regNo == string.Empty)
                {
                    string sql = "SELECT Id, CompanyName, CompanyId, CompanyId2, SystemPassword, CompanyPassword, fm, gun, gp, gs, sgsc, unvan, adres, sgm, kka, kkc, sc1, sc2, sc3, sc4, sc5, cu, cd FROM Company WHERE CompanyName= @cn Order By Id";

                    c = ConLocal.QueryAsync<Company>(sql, new { cn = cn}).Result.SingleOrDefault();
                }
                else
                {
                    string sql = "SELECT Id, CompanyName, CompanyId, CompanyId2, SystemPassword, CompanyPassword, fm, gun, gp, gs, sgsc, unvan, adres, sgm, kka, kkc, sc1, sc2, sc3, sc4, sc5, cu, cd FROM Company WHERE CompanyName= @p1 AND sgsc= @p2 Order By Id";
                    c = ConLocal.QueryAsync<Company>(sql, new { p1 = cn, p2 = regNo }).Result.SingleOrDefault();
                }
                if (c != null) c = EncryptDs.DecryptCompany(c);
                return c;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public Company GetCompanyById(int id, out string msg)
        {
            msg = "";
            try
            {
                string sql = "SELECT id, CompanyName, CompanyID, CompanyID2, SystemPassword, CompanyPassword, fm, gun, gp, gs, sgsc, unvan, adres, sgm, kka, kkc, sc1, sc2, sc3, sc4, sc5, cu, cd FROM company WHERE id = @p1 ORDER BY id";
                Company c = ConLocal.QueryAsync<Company>(sql, new { p1 = id }).Result.FirstOrDefault();
                if (c != null) c = EncryptDs.DecryptCompany(c);
                return c;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();

                return null;
            }
        }
        public Company GetCompanyBySgkIds(string cid, string cid2, out string msg)
        {
            msg = "";
            cid = EncryptDs.EncryptString(cid, GlobalVars.PassPhrase);
            cid2 = EncryptDs.EncryptString(cid2, GlobalVars.PassPhrase);
            try
            {
                string sql = "SELECT id, CompanyName, CompanyID, CompanyID2, SystemPassword, CompanyPassword, fm, gun, gp, gs, sgsc, unvan, adres, sgm, kka, kkc, sc1, sc2, sc3, sc4, sc5, cu, cd FROM company WHERE CompanyId = @p1 AND CompanyID2 = @p2 ORDER BY id";
                Company c = ConLocal.QueryAsync<Company>(sql, new { p1 = cid, p2 = cid2 }).Result.FirstOrDefault();
                if (c != null) c = EncryptDs.DecryptCompany(c);
                return c;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();

                return null;
            }
        }
        public int AddCompany(Company comp, int mcc, out string msg)
        {
            int result = 0;
            comp = EncryptDs.EncryptCompany(comp); Company compq = ExistCompanyByRegNo(comp.Sgsc, out msg);
            try
            {
                if (compq != null)
                {
                    //comp.Id = GetCompanyId(comp.CompanyId, comp.CompanyId2, out msg);
                    //if (comp.Id == -1) { return 0; }

                    string sql = "UPDATE company SET CompanyName = @p1, CompanyID = @p2, CompanyID2 = @p3, SystemPassword = @p4, CompanyPassword = @p5, fm = @p6, gun = @p7, gp = @p8, gs = @p9, sgsc = @p10, unvan = @p11, adres = @p12, sgm = @p13, kka = @p14, kkc = @p15, sc1 = @p16, sc2 = @p17, sc3 = @p18, sc4 = @p19, sc5 = @p20, cu = @p21, cd = @p22 WHERE id = @compId";
                    result = ConLocal.ExecuteAsync(sql, new { p1 = comp.CompanyName, p2 = comp.CompanyId, p3 = comp.CompanyId2, p4 = comp.SystemPassword, p5 = comp.CompanyPassword, p6 = comp.Fm, p7 = comp.Gun, p8 = comp.Gp, p9 = comp.Gs, p10 = comp.Sgsc, p11 = comp.Unvan, p12 = comp.Adres, p13 = comp.Sgm, p14 = comp.Kka, p15 = comp.Kkc, p16 = comp.Sc1, p17 = comp.Sc2, p18 = comp.Sc3, p19 = comp.Sc4, p20 = comp.Sc5, p21 = comp.Cu , p22 = comp.Cd, compId = compq.Id }).Result;
                    result = result > 0 ? 2 : 0;    // güncelleme başarılıysa result değerini 2 yap
                }
                else
                {
                    int count = ConLocal.Execute("SELECT COUNT(*) FROM company");
                    if (count >= mcc) { msg = "Daha fazla firma eklemek için bir üst pakete geçmelisiniz"; return 3; }
                    string sql = "INSERT INTO company VALUES(null, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20, @p21, @p22)";
                    
                    result = ConLocal.Execute(sql, new { p1 = comp.CompanyName, p2 = comp.CompanyId, p3 = comp.CompanyId2, p4 = comp.SystemPassword, p5 = comp.CompanyPassword, p6 = comp.Fm, p7 = comp.Gun, p8 = comp.Gp, p9 = comp.Gs, p10 = comp.Sgsc, p11 = comp.Unvan, p12 = comp.Adres, p13 = comp.Sgm, p14 = comp.Kka, p15 = comp.Kkc, p16 = comp.Sc1, p17 = comp.Sc2, p18 = comp.Sc3, p19 = comp.Sc4, p20 = comp.Sc5, p21 = comp.Cu, p22 = comp.Cd });
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString(); 
                return 0;
            }

            return result;
        }
        public int DeleteCompany(Company c, string sgscEnc, out string msg)
        {
            msg = "";
            int result;
            c = EncryptDs.EncryptCompany(c); 
            try
            {
                string sql = "DELETE FROM company WHERE CompanyID = @p1 AND CompanyID2 = @p2";
                result = ConLocal.ExecuteAsync(sql, new { p1 = c.CompanyId, p2 = c.CompanyId2 }).Result;
                    
                if (result == 1 && c.Id == c.Fm)
                {
                    sql = "UPDATE Company SET fm = 1 WHERE fm = @fm";
                    ConLocal.ExecuteAsync(sql, new { fm = c.Id });
                    sql = "UPDATE companycheck SET sgscenc = REPLACE(sgscenc, @p1, '')";
                    ConRemote.ExecuteAsync(sql, new { p1 = sgscEnc });
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return 0;
            }
            return result;
        }
        public void UpdateFm(Company c, int fm, out string msg)
        {
            msg = "";
            c = EncryptDs.EncryptCompany(c);
            try
            {
                string sql = "UPDATE Company SET fm = @fmx WHERE((CompanyID = @wcid) AND(CompanyID2 = @wcid2))";
                int result = ConLocal.ExecuteAsync(sql, new { fmx = fm , wcid = c.CompanyId, wcid2 = c.CompanyId2 }).Result;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }
        public int GetLastId(out string msg)
        {
            msg = ""; 
            try
            {
                string sql = "SELECT * FROM company ORDER BY id DESC LIMIT 1";
                Company c = ConLocal.QueryAsync<Company>(sql).Result.FirstOrDefault();
                if (c != null)
                {
                    return c.Id;
                }
                return 1;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return 0;
            }
        }
        public string GetCompanyNameById(int id, out string msg)
        {
            try
            {
                return GetCompanyById(id, out msg).CompanyName;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
            return string.Empty;
        }
        public Dictionary<string, int> GetCompanyCenters(out string msg)
        {
            msg = "";
            List<Company> c;
            Dictionary<string, int> dic = new Dictionary<string, int>();
            try
            {
                string sql = "SELECT id, CompanyName, CompanyID, CompanyID2, SystemPassword, CompanyPassword, fm, gun, gp, gs, sgsc, unvan, adres, sgm, kka, kkc, sc1, sc2, sc3, sc4, sc5, cu, cd FROM company WHERE fm = id";
                c = ConLocal.QueryAsync<Company>(sql).Result.ToList();  

                if (c != null && c.Count > 0)
                {
                    foreach (Company item in c)
                    {
                        dic.Add(item.CompanyName, item.Id);
                    }
                    return dic;
                }
                return null;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();

                return null;
            }
        }
        public void ResetCompanyId(out string msg)
        {
            msg = "";
            try
            {
                string sql = "TRUNCATE TABLE company";
                int c = ConLocal.Execute(sql);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }
        public void Copycomp() { }
    }
}
