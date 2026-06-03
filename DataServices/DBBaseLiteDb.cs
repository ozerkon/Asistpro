using LiteDB;
using Models.Common;
using Models.Domain;
using MySql.Data.MySqlClient;
using System;

namespace DataServices
{
    public class DbBaseLiteDb : DbBase
    {
        // Tepedeki "= GlobalVars.GetMsqcsRemote();" kısımlarını kaldırıyoruz!
        public override string RemoteCs { get; set; }
        public override string LocalCs { get; set; }
        protected override MySqlConnection ConRemote { get; set; }
        protected LiteDatabase LdbConnect { get; set; }

        public ILiteCollection<Links> LinksConnect;
        public ILiteCollection<Users> UsersConnect;
        public ILiteCollection<Personal> PersonalConnect;
        public ILiteCollection<Leaves> LeavesConnect;
        public ILiteCollection<LeavePeriod> LeavesPeriodsConnect;
        public ILiteCollection<Company> CompanyConnect;
        public ILiteCollection<Package> PackageConnect;
        public ILiteCollection<SgkDb> SgkDbConnect;
        public ILiteCollection<SgkEt> SgkEtConnect;
        public ILiteCollection<SgkMe> SgkMeConnect;
        public ILiteCollection<SgkCr> SgkCrConnect;
        public ILiteCollection<Sgk6661> Sgk6661Connect;
        public ILiteCollection<UserPrm> PrmConnect;
        public ILiteCollection<SgkIgl> IglConnect;
        public ILiteCollection<SgkHl> HlConnect;
        public ILiteCollection<SgkHlp> HlpConnect;
        public ILiteCollection<SgkThkk> ThkkConnect;
        public ILiteCollection<CompanyCheck> CcConnect;

        public DbBaseLiteDb()
        {
            // Değerleri tam bu anda, nesne üretilirken güncel statik hafızadan güvenle çekiyoruz.
            // Eğer config'den gelen gizli karakter riski varsa her ihtimale karşı .Trim() de ekleyebilirsiniz.
            LocalCs = GlobalVars.GetliteDbCs();
            RemoteCs = GlobalVars.GetMsqcsRemote();

            LdbConnect = new LiteDatabase(LocalCs);
            SetConRemote();
        }

        public override void SetConLocal(string con)
        {
            LocalCs = con;
            LdbConnect = new LiteDatabase(con);
        }

        public override void SetConRemote()
        {
            // Eğer RemoteCs bir şekilde boş kaldıysa veya null ise nesneyi oluşturup patlatma koruması
            if (!string.IsNullOrEmpty(RemoteCs))
            {
                ConRemote = new MySqlConnection(RemoteCs);
            }
        }

        public override bool TestConnection(string cs)
        {
            string msg = "";
            LdbConnect = new LiteDatabase(cs);
            ILiteCollection<TestCl> testCon;
            try
            {
                testCon = LdbConnect.GetCollection<TestCl>("testcl");
                testCon.EnsureIndex(x => x.Id);
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }

        public override bool TruncateTables()
        {
            try
            {
                LdbConnect.Execute("DROP COLLECTION aaab");
                LdbConnect.Execute("DROP COLLECTION company");
                LdbConnect.Execute("DROP COLLECTION companycheck");
                LdbConnect.Execute("DROP COLLECTION crs");
                LdbConnect.Execute("DROP COLLECTION dbs");
                LdbConnect.Execute("DROP COLLECTION ets");
                LdbConnect.Execute("DROP COLLECTION hl");
                LdbConnect.Execute("DROP COLLECTION hlp");
                LdbConnect.Execute("DROP COLLECTION igl");
                LdbConnect.Execute("DROP COLLECTION leaves");
                LdbConnect.Execute("DROP COLLECTION links");
                LdbConnect.Execute("DROP COLLECTION mes");
                LdbConnect.Execute("DROP COLLECTION package");
                LdbConnect.Execute("DROP COLLECTION periods");
                LdbConnect.Execute("DROP COLLECTION personals");
                LdbConnect.Execute("DROP COLLECTION thkk");
                LdbConnect.Execute("DROP COLLECTION userprm");
                LdbConnect.Execute("DROP COLLECTION users");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}