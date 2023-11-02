using LiteDB;
using Models.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataServices
{
    public class CompanyDataServiceLdb : DbBaseLiteDb, ICompanyDataService
    {
        public CompanyDataServiceLdb()
        {
            CompanyConnect = LdbConnect.GetCollection<Company>("company");
            CompanyConnect.EnsureIndex(x => x.CompanyName);
            CompanyConnect.EnsureIndex(x => x.CompanyId);
            CompanyConnect.EnsureIndex(x => x.CompanyId2);

            CcConnect = LdbConnect.GetCollection<CompanyCheck>("companycheck");
            CcConnect.EnsureIndex(x => x.Id);
        }
        public bool FillCompanyCheck(out string msg)
        {
            msg = "";
            CompanyCheck ccNew = new CompanyCheck() { SgscEnc = GlobalVars.CmpCheck };
            CompanyCheck cc = new CompanyCheck();
            try
            {
                cc = CcConnect.FindAll().FirstOrDefault();//.Where(x => x.sgscEnc.StartsWith($"'{GlobalVars.cmpCheck}'")).FirstOrDefault();
                if (cc == null)
                {
                    CcConnect.Insert(ccNew);
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
            msg = ""; bool result;
            try
            {
                string sgscEnc = $"{GetSgscEnc(out msg)}{sgsc}";
                //result = ldbConnect.Execute("UPDATE companycheck SET sgscenc = @p1", sgscEnc).Single();
                CompanyCheck cc = new CompanyCheck() { Id = 1, SgscEnc = sgscEnc };
                result = CcConnect.Update(cc);
                if (result) return 1;
                else return -1;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }
        public int UpdateSgscEnc(string sgsc, out string msg)
        {
            msg = ""; int result;
            try
            {
                CompanyCheck cc = new CompanyCheck() { Id = 1, SgscEnc = sgsc };
                result = CcConnect.Insert(cc).AsInt32;
                if (result == 1) return 1;
                else return -1;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return -1;
            }
        }
        public string GetSgscEnc(out string msg)
        {
            msg = "";
            try
            {
                CompanyCheck cc = CcConnect.FindById(1);
                return cc.SgscEnc;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }
        public List<Company> GetCompanies(out string msg)
        {
            msg = "";
            try
            {
                List<Company> lstCom = CompanyConnect.FindAll().ToList();
                if (lstCom != null) lstCom = EncryptDs.DecryptCompany(lstCom);
                return lstCom;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }
        public List<Company> GetCompaniesByFm(int fm, out string msg)
        {
            msg = "";
            try
            {
                List<Company> lstCom = CompanyConnect.Find(Query.And(Query.EQ("fm", fm), Query.EQ("Id", fm))).OrderBy(i => i.Id).ToList();
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
                Company cx = CompanyConnect.FindOne(c => c.CompanyId == cid && c.CompanyId2 == cid2);
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
                    foreach (var c in comps)
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
                    c = CompanyConnect.FindAll().Where(i => i.CompanyName == cn).OrderBy(x => x.Id).FirstOrDefault();
                }
                else
                {
                    c = CompanyConnect.FindAll().Where(i => i.CompanyName == cn && i.Sgsc == regNo).OrderBy(x => x.Id).FirstOrDefault();
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
                Company c = CompanyConnect.FindAll().Where(i => i.Id == id).OrderBy(x => x.Id).FirstOrDefault();
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
                Company c = CompanyConnect.FindAll().Where(i => i.CompanyId == cid && i.CompanyId2 == cid2).OrderBy(x => x.Id).FirstOrDefault();
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
                    comp.Id = compq.Id;
                    //if (comp.Id == -1) { return 0; }
                    result = CompanyConnect.Update(comp) ? 2 : 0; // güncelleme başarılıysa result değerini 2 yap 
                    if (compq.Id == compq.Fm && comp.Fm != compq.Fm && result == 2) result = 3; // firma merkez iken şube konumuna düşürüldüyse
                }
                else
                {
                    int count = CompanyConnect.Count();
                    if (count >= mcc) { msg = "Daha fazla firma eklemek için bir üst pakete geçmelisiniz"; return 3; }
                    string sql = $"INSERT INTO company:INT VALUES {{ CompanyName:'{comp.CompanyName }' , CompanyID:'{comp.CompanyId}', CompanyID2:'{comp.CompanyId2}', SystemPassword:'{comp.SystemPassword}', CompanyPassword:'{comp.CompanyPassword}', fm:{comp.Fm}, gun:'{comp.Gun}', gp:'{comp.Gp}', gs:'{comp.Gs}', sgsc:'{comp.Sgsc}', unvan: '{comp.Unvan}' , adres: '{comp.Adres}' , sgm: '{comp.Sgm}', kka:{{'$date':'{comp.Kka:yyyy-MM-ddThh:mm:ssZ}'}}, kkc: {{'$date':'{comp.Kkc:yyyy-MM-ddThh:mm:ssZ}'}}, sc1: '{comp.Sc1}' , sc2 : '{comp.Sc2}' ,sc3 : '{comp.Sc3}' ,sc4 : '{comp.Sc4}' ,sc5 : '{comp.Sc5}' ,cu: {comp.Cu} ,  cd:{{'$date':'{comp.Cd:yyyy-MM-ddThh:mm:ssZ}'}} }}";
                    result = LdbConnect.Execute(sql).Single();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString(); return 0;
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

                result = CompanyConnect.DeleteMany(Query.And(Query.EQ("CompanyID", c.CompanyId), Query.EQ("CompanyID2", c.CompanyId2)));
                if (result == 1 && c.Id == c.Fm)
                {
                    IBsonDataReader ibdr1 = LdbConnect.Execute("UPDATE Company SET fm = 1 WHERE fm = @fm", c.Id);
                    IBsonDataReader ibdr2 = LdbConnect.Execute($"UPDATE companycheck SET sgscenc = REPLACE(sgscenc, '{sgscEnc}', '')");
                }
                else if (result == 1)
                {
                    IBsonDataReader ibdr2 = LdbConnect.Execute($"UPDATE companycheck SET sgscenc = REPLACE(sgscenc, '{sgscEnc}', '')");
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
            msg = ""; int result;
            c = EncryptDs.EncryptCompany(c);
            try
            {
                var ddd = LdbConnect.Execute("UPDATE Company SET fm = @fmx WHERE((CompanyID = @wcid) AND(CompanyID2 = @wcid2))", fm, c.CompanyId, c.CompanyId2);
                result = ddd.Single();
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
                Company c = CompanyConnect.FindOne(Query.All(Query.Descending));
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
            msg = "";
            try
            {
                return CompanyConnect.FindById(id).CompanyName;
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
            Dictionary<string, int> d = new Dictionary<string, int>();
            try
            {
                c = CompanyConnect.FindAll().Where(i => i.Fm == i.Id).ToList();

                if (c != null && c.Count > 0)
                {
                    foreach (var item in c)
                    {
                        d.Add(item.CompanyName, item.Id);
                    }
                    return d;
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
                LdbConnect.DropCollection("company");
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
            }
        }

        public void Copycomp()
        {
            string databasePath = "C:\\ProgramData\\SgkAsistan\\Data\\sgkasistan.db";

            // Veritabanı bağlantısını oluşturun
            using (var db = new LiteDatabase(databasePath))
            {
                // LiteDB koleksiyonunu alın
                var collection = db.GetCollection<BsonDocument>("company");

                // JSON dosyasını okuyun ve koleksiyona ekleyin
                string json = File.ReadAllText("c:\\company.json");
                var jsonBsonArray = JsonSerializer.Deserialize(json).AsArray;

                foreach (BsonValue bsonValue in jsonBsonArray)
                {
                    collection.Insert(bsonValue.AsDocument);
                }

                // LiteDB koleksiyonunu alın
                collection = db.GetCollection<BsonDocument>("companycheck");

                // JSON dosyasını okuyun ve koleksiyona ekleyin
                json = File.ReadAllText("c:\\companycheck.json");
                jsonBsonArray = JsonSerializer.Deserialize(json).AsArray;

                foreach (BsonValue bsonValue in jsonBsonArray)
                {
                    collection.Insert(bsonValue.AsDocument);
                }
            }
        }
    }
}
