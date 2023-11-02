using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Common
{
    public class Customer
    {
        public long Cx { get; set; }   //CompanyId
        public long Abn { get; set; }   //CompanyId
        public long Cxp { get; set; }   //ParentCompanyId
        public string Cnm { get; set; } = String.Empty;   //CompanyName
        public string Cco { get; set; } = String.Empty;  //CompanyCode
        public string Onm { get; set; } = String.Empty;  //OfficialName
        public string Ct { get; set; } = String.Empty;  //ContactPerson
        public string Ct2 { get; set; } = String.Empty;   //ContactPerson2
        public string Cm { get; set; } = String.Empty;  //CompanyEMail
        public string Cp { get; set; } = String.Empty;   //CompanyPhone
        public string Ct1Ml { get; set; } = String.Empty;  //Contact1EMail
        public string Ct2Ml { get; set; } = String.Empty;  //Contact2EMail

        public string Ct1P { get; set; } = String.Empty;  //Contact1Phone
        public string Ct1Px { get; set; } = String.Empty;   //Contact1PhoneExt
        public string Ct2P { get; set; } = String.Empty;////Contact2Phone
        public string Ct2Px { get; set; } = String.Empty;  //Contact2PhoneExt
        public string Fx { get; set; } = String.Empty;  //Fax
        public string Aa { get; set; } = String.Empty;  //Address
        public string Z { get; set; } = String.Empty;   //ZipCode 
        public string Ulc { get; set; } = String.Empty;   //CountryCode
        public string Uln { get; set; } = String.Empty;  //CountryName
        public int Cyx { get; set; }   //CityId
        public string Cynm { get; set; } = String.Empty;   //CityName
        public string Dsnm { get; set; } = String.Empty;  //DistrictName
        public string Tof { get; set; } = String.Empty;    //TaxOffice
        public string Tn { get; set; } = String.Empty;   //TaxNumber


        public string Sgsc { get; set; } = String.Empty;  //SGKSicilNo
        public string Sgmkd { get; set; } = String.Empty;   //SGMKodAd
        public string Tsctp { get; set; } = String.Empty;   //TescilTipi
        public int Ivuatn { get; set; } = 0;   //InvUseDiffTaxNumber
        public string Ivatn { get; set; } = String.Empty;   //InvAltTaxNumber
        public string Ivm { get; set; } = String.Empty;  //InvEMail
        public string Mvi { get; set; } = String.Empty;//InvEMail2
        public string Ivmn { get; set; } = String.Empty;//InvEMail3
        public string Ivmb { get; set; } = String.Empty;//InvEMail4
        public string Sl { get; set; } = String.Empty;
        public string Slinvc { get; set; } = String.Empty;
        public int Isac { get; set; }   //IsActive
        public DateTime Cd { get; set; } //CreateDate
        public long Cu { get; set; }  //CreateUserID
        public long Cip { get; set; }  //CreateUserID

        public DateTime? Opdt { get; set; }   //kanun kap al
        public DateTime? Csdt { get; set; }   //kanun kap cikis
        public string Scsdt { get; set; } = String.Empty;  //kann kap cik
        public DateTime Csdtchkdt { get; set; }
        public string Prmrt { get; set; } = "";
        public string Oprmrt { get; set; } = "";
        public string Izisldt { get; set; } = String.Empty;

        public string Cd1 { get; set; } = String.Empty;
        public string Cd2 { get; set; } = String.Empty;
        public string Cd3 { get; set; } = String.Empty;
        public string Scd1 { get; set; } = String.Empty;
        public string Scd2 { get; set; } = String.Empty;
        public string Scd3 { get; set; } = String.Empty;
        public string Muhent { get; set; } = String.Empty;

        public int Wya { get; set; } = 0;
        public int Wya2 { get; set; } = 0;
        public int Wgya { get; set; } = 0;
        public int Wgya2 { get; set; } = 0;
        public int Defhltype { get; set; } = 0;
        public int Appl18 { get; set; } = 0;

        public static int HlOnayli = 10;
        public static int HlOnaysiz = 20;
        public static int HlExcel = 30;
        public static int HlXml = 31;
        public static int HlBdpxml = 32;



        //public string a { get; set; } = "";
        //public string b { get; set; } = "";
        //public string c { get; set; } = "";
        //public string d { get; set; } = "";
        #region CompanyStatus
        public enum CompanyStatusEnum { Active = 1, Inactive = 0 }
        public CompanyStatusEnum CompanyStatus;
        public readonly Dictionary<CompanyStatusEnum, string> CompanyStatusTexts = new Dictionary<CompanyStatusEnum, string> { { CompanyStatusEnum.Active, "Aktif" }, { CompanyStatusEnum.Inactive, "Aktif Değil" } };
        public enum Dmsts { Dm = 0, Wk = 1 }
        public int Dm { get; set; } = 0;
        public int Dms { get; set; } = 0;
        public bool Isdm() => (Dms == (int)Dmsts.Dm);

        #endregion
        #region tenn
        public int Smrt { get; set; }
        #endregion
        public DateTime Gtr()
        {
            return (this.Opdt == null) ? DateTime.MinValue : Opdt.Value;
        }
        public DateTime Gtrm(DateTime tt)
        {
            return (this.Gtr() > tt) ? this.Gtr() : tt;
        }
        public DateTime Gtrm(int j, int m)
        {
            DateTime k = new DateTime(j, m, 1);
            return (this.Gtr() > k) ? this.Gtr() : k;
        }
        public int? Opdtym()
        {
            if (this.Opdt == null) return null;
            return 100 * this.Opdt.Value.Year + this.Opdt.Value.Month;
        }
        public string Opdtymstr()
        {
            if (this.Opdt == null) return "";
            return this.Opdt.Value.ToString("yyyy/MM").Replace(".", "/");
        }
        public int? Csdtym()
        {
            if (this.Csdt == null) return null;
            return 100 * this.Csdt.Value.Year + this.Csdt.Value.Month;
        }
        public string Csdtymstr()
        {
            if (this.Csdt == null) return "";
            return this.Csdt.Value.ToString("yyyy/MM").Replace(".", "/");
        }
        public bool Isbcn()
        {
            if (Sgsc.Length >= 26)
            {
                string tsrpart = Sgsc.Substring(23, 3);
                return (Convert.ToInt32(tsrpart) > 0);
            }
            return false;
        }
        public string Cntrc { get; set; } = String.Empty;
        public string Szno { get; set; } = String.Empty;

        public string Srn
        {
            get
            {
                string srn = (Sgsc.Length > 19) ? Sgsc.Substring(9, 7) : "";
                return srn;
            }
        }
        public static string Srn2(string sgsc)
        {
            string srn = (sgsc.Length > 19) ? sgsc.Substring(9, 7) : "";
            return srn;
        }
        public string Ysb
        {
            get
            {
                string srn = (Sgsc.Length >= 6) ? Sgsc.Substring(5, 2) : "";
                return srn;
            }
        }
        public string Esb
        {
            get
            {
                string srn = (Sgsc.Length >= 9) ? Sgsc.Substring(7, 2) : "";
                return srn;
            }
        }
        public string Iln
        {
            get
            {
                string srn = (Sgsc.Length >= 19) ? Sgsc.Substring(16, 3) : "";
                return srn;
            }
        }
        public string Subc
        {
            get
            {
                string srn = (Sgsc.Length >= 26) ? Sgsc.Substring(23, 3) : "";
                return srn;
            }
        }
        public Customer Clone()
        {
            Customer cln = new Customer();
            cln = (Customer)this.MemberwiseClone();
            return cln;
        }

        public static string Arcnm(Customer c)
        {
            return $"{c.Cnm} ({c.Cx.ToString().PadLeft(8, '0')})";
        }
    }

}
