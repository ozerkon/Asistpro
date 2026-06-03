using Models.Common;
using MySql.Data.MySqlClient;
using Dapper;
using System;
using System.Collections.Generic;

namespace DataServices
{
    public class DbBaseMySql : DbBase
    {
        // Alanların otomatik atamalarını kaldırıp temiz bırakıyoruz
        public override string RemoteCs { get; set; }
        public override string LocalCs { get; set; }
        protected override MySqlConnection ConRemote { get; set; }
        protected MySqlConnection ConLocal { get; set; }

        public DbBaseMySql()
        {
            // Atamaları tam nesne türetilme anına (Constructor) taşıyoruz.
            // Bu sayede ana formdaki ayarlar çoktan çözülmüş ve temizlenmiş oluyor.
            LocalCs = GlobalVars.GetMsqcsLocal();
            RemoteCs = GlobalVars.GetMsqcsRemote();

            ConLocal = new MySqlConnection(LocalCs);
            SetConRemote();
        }

        public override void SetConLocal(string con)
        {
            LocalCs = con;
            ConLocal = new MySqlConnection(con);
        }

        public override void SetConRemote()
        {
            // Boş kalma ihtimaline karşı null/empty kontrolü koruması
            if (!string.IsNullOrEmpty(RemoteCs))
            {
                ConRemote = new MySqlConnection(RemoteCs);
            }
        }

        public override bool TestConnection(string cs)
        {
            string msg = "";
            try
            {
                // testcon nesnesini using bloğuna alarak işlem bittiğinde 
                // bağlantının sunucuda açık kalmamasını (Dispose edilmesini) sağlıyoruz.
                using (MySqlConnection testcon = new MySqlConnection(cs))
                {
                    string sqlEnc = "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意d";
                    string sql = Encrypt.DecryptString(sqlEnc, GlobalVars.PassPhrase);

                    testcon.Open();
                    testcon.Execute(sql);
                }
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
            List<string> tables = new List<string>() { "alv", "company", "companycheck", "links", "package", "periods", "personal", "sgk6661", "sgkcr", "sgkdb", "sgket", "sgkhl", "sgkhlp", "sgkigl", "sgkme", "sgkthkk", "userprm", "users" };
            try
            {
                foreach (string tbl in tables)
                {
                    ConLocal.Execute($"TRUNCATE TABLE {tbl};");
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}