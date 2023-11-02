using System;

namespace Models.Common
{
    public class SiteUser : ICloneable
    {
        public string Sid { get; set; } = "";
        public long Ux { get; set; } = 0;//UserID
        public string Uxn { get; set; } = String.Empty; //UserName
        public int Tx { get; set; } = 0;  //UserType
        public string Knwrt { get; set; } = String.Empty;
        public string Knwrtt { get; set; } = String.Empty;
        public string Ph { get; set; } = String.Empty;   //PasswordHash
        public string Ps { get; set; } = String.Empty;   //PasswordSalt

        public string Em { get; set; } = String.Empty;
        public DateTime Cd { get; set; } = DateTime.MinValue;
        public DateTime Ldt { get; set; } = DateTime.MinValue;  //LastLoginDate
        public string Lip { get; set; } = String.Empty;   //LastLoginIP
        public int Acs { get; set; }   //ActiveStatus
        //public DateTime ActiveStatusDate { get; set; }
        //public string ActiveStatusNotes { get; set; }
        public string ArcBase { get; set; } = "";
        public long Abn { get; set; } = 0;
        public string Abnm { get; set; } = "";
        public string Fya1 { get; set; } = "";  //filtrelerde varsayılan yilay başlangıc
        public string Fya2 { get; set; } = "";
        public int Dmusr { get; set; }
        public long Skm { get; set; }
        public int Tcv { get; set; }
        public int Scdv { get; set; }
        public string Wrkdir { get; set; } = "";
        public int Allcx { get; set; } = 0;
        public string Acscd { get; set; } = "";
        public long Lcx { get; set; }
        public SiteUser()
        {

        }
        public SiteUser(string ds, string dsk)
        {
            this.Uxn = ds;
            this.Knwrt = dsk;
        }

        public const int UserTypeAbnAdmin = 10;
        public const int UserTypeAdmin = 1;
        public const int UserTypeStandard = 0;

        public const string Invc = "invc";
        public const string Acpt = "acpt";
        object ICloneable.Clone()
        {
            throw new NotImplementedException();
        }
        public SiteUser Clone()
        {
            return (SiteUser)this.MemberwiseClone();
        }
        public bool Ia()
        {
            return this.Tx == SiteUser.UserTypeAdmin;
        }
        public bool Isa()
        {
            return this.Tx == SiteUser.UserTypeAdmin; //SiteUser.UserTypeSuperAdmin;
        }
        public long Uxf()
        {
            return (this.Ia() || (this.Allcx == 1)) ? 0 : this.Ux;
        }
    }
    public class SiteUserValidator
    {

        public (bool isValid, string message) AddValidator(SiteUser u)
        {
            string message = "";
            bool isValid = true;
            if (u.Uxn.Trim().Length == 0) { isValid = false; message += "Kullanıcı adı boş olamaz.\n"; }
            if (u.Em.Trim().Length == 0) { isValid = false; message += "Kullanıcı e-mail boş olamaz.\n"; }
            if (u.Knwrt.Trim().Length == 0) { isValid = false; message += "Kullanıcı şifresi boş olamaz.\n"; }
            else
            {
                if (u.Knwrt.Equals(u.Knwrtt) == false)
                {
                    isValid = false; message = "Şifre ve şifre tekrarı aynı olmalıdır";
                }
            }
            if ((u.Tx != 0) && (u.Tx != 1) && (u.Tx != 10)) { isValid = false; message += "Kullanıcı tipi hatalı.\n"; }

            return (isValid, message);
        }
        public (bool isValid, string message) UpdateValidator(SiteUser u)
        {
            string message = "";
            bool isValid = true;
            if (u.Uxn.Trim().Length == 0) { isValid = false; message += "Kullanıcı adı boş olamaz.\n"; }
            if (u.Em.Trim().Length == 0) { isValid = false; message += "Kullanıcı e-mail boş olamaz.\n"; }
            if (u.Tx < 0 || u.Tx > 1) { isValid = false; message += "Kullanıcı tipi hatalı.\n"; }

            return (isValid, message);
        }
        public (bool isValid, string message) PasswordChangeValidator(SiteUser u)
        {
            string message = "";
            bool isValid = true;
            if (u.Knwrt.Trim().Length == 0) { isValid = false; message += "Kullanıcı şifresi boş olamaz.\n"; }
            else
            {
                if (u.Knwrt.Equals(u.Knwrtt) == false)
                {
                    isValid = false; message = "Şifre ve şifre tekrarı aynı olmalıdır";
                }
            }
            return (isValid, message);
        }
    }
}
