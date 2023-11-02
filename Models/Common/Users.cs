using System;

namespace Models.Common
{
    public class Users : ICloneable
    {
        public int Id { get; set; }
        public string Unm { get; set; }
        public string Uln { get; set; }
        public string Un { get; set; }
        public string Up { get; set; } = String.Empty;
        public string Upp { get; set; } = String.Empty;
        public string Uy { get; set; } = String.Empty;
        public DateTime Cd { get; set; } = DateTime.Now;
        public string Hc { get; set; } = String.Empty;
        public int Cu { get; set; } = 1;

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public static class ActiveUser
        {
            public static int Id { get; set; } = 1;
            public static DateTime LoginTime { get; set; }
            public static int Yetki { get; set; }
            public static bool UserChangeRequest { get; set; } = false;
        }

        public class UserValidator
        {
            public (bool isValid, string message) AddValidator(Users u)
            {
                string message = "";
                bool isValid = true;
                if (u.Un.Trim().Length == 0) { isValid = false; message += "Kullanıcı adı boş olamaz.\n"; }
                if (u.Up.Trim().Length == 0) { isValid = false; message += "Kullanıcı şifresi boş olamaz.\n"; }
                else
                {
                    if (u.Up.Equals(u.Upp) == false)
                    {
                        isValid = false; message = "Şifre ve şifre tekrarı aynı olmalıdır";
                    }
                }

                return (isValid, message);
            }
            public (bool isValid, string message) UpdateValidator(Users u)
            {
                string message = "";
                bool isValid = true;
                if (u.Un.Trim().Length == 0) { isValid = false; message += "Kullanıcı adı boş olamaz.\n"; }
                return (isValid, message);
            }
            public (bool isValid, string message) PasswordChangeValidator(Users u)
            {
                string message = "";
                bool isValid = true;
                if (u.Up.Trim().Length == 0) { isValid = false; message += "Kullanıcı şifresi boş olamaz.\n"; }
                else
                {
                    if (u.Up.Equals(u.Upp) == false)
                    {
                        isValid = false; message = "Şifre ve şifre tekrarı aynı olmalıdır";
                    }
                }
                return (isValid, message);
            }
        }
    }
    public static class UserChanged
    {
        public static bool HasChanged { get; set; }
    }
}
