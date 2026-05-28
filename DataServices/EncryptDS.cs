using Models.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DataServices
{
    public static class EncryptDs
    {
        private const int SaltByteSize = 24;
        private const int HashByteSize = 24;
        private const int HasingIterationsCount = 10101;

        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private const string InitVector = "pemgail9uzpgzl88";
        // This constant is used to determine the keysize of the encryption algorithm
        private const int Keysize = 256;
        //Encrypt
        public static string EncryptString(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(InitVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null))
            {
                byte[] keyBytes = password.GetBytes(Keysize / 8);
                using (RijndaelManaged symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                byte[] cipherTextBytes = memoryStream.ToArray();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }
        //Decrypt
        public static string DecryptString(string cipherText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(InitVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            using (PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null))
            {
                byte[] keyBytes = password.GetBytes(Keysize / 8);
                using (RijndaelManaged symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                    {
                        using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        internal static byte[] GenerateSalt(int saltByteSize = SaltByteSize)
        {
            using (RNGCryptoServiceProvider saltGenerator = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[saltByteSize];
                saltGenerator.GetBytes(salt);
                return salt;
            }
        }
        public static string GenerateSalt()
        {
            return System.Text.Encoding.Default.GetString(GenerateSalt(SaltByteSize));
        }
        public static string GenerateSaltVl(int vls)
        {
            return System.Text.Encoding.Default.GetString(GenerateSalt(vls));
        }

        public static byte[] ComputeHash(string password, byte[] salt, int iterations = HasingIterationsCount, int hashByteSize = HashByteSize)
        {
            using (Rfc2898DeriveBytes hashGenerator = new Rfc2898DeriveBytes(password, salt))
            {
                hashGenerator.IterationCount = iterations;
                return hashGenerator.GetBytes(hashByteSize);
            }
        }
        public static string GenerateHash(string password, string salt)
        {
            return System.Text.Encoding.Default.GetString(ComputeHash(password, Encoding.ASCII.GetBytes(salt), HasingIterationsCount, HashByteSize));
        }


        //Length constant verification - prevents timing attack
        private static bool AreHashesEqual(byte[] firstHash, byte[] secondHash)
        {
            int minHashLenght = firstHash.Length <= secondHash.Length ? firstHash.Length : secondHash.Length;
            var xor = firstHash.Length ^ secondHash.Length;
            for (int i = 0; i < minHashLenght; i++)
                xor |= firstHash[i] ^ secondHash[i];
            return 0 == xor;
        }
        internal static bool VerifyPassword(string password, byte[] passwordSalt, byte[] passwordHash)
        {
            byte[] computedHash = ComputeHash(password, passwordSalt);
            return AreHashesEqual(computedHash, passwordHash);
        }
        public static bool VerifyUserPassword(string password, string passwordSalt, string passwordHash)
        {
            return VerifyPassword(password, Encoding.ASCII.GetBytes(passwordSalt), Encoding.ASCII.GetBytes(passwordHash));
        }

        public static string Rsglmt(int b)
        {
            Random rs = new Random((int)DateTime.Now.Ticks);
            Random rs2 = new Random((int)DateTime.Now.Ticks - 2361);
            string input = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Range(0, b).Select(x => input[rs.Next(0, input.Length)]).ToArray());
        }
        public static string Ecdc(string szPlainText, int szEncryptionKey)
        {
            StringBuilder szInputStringBuild = new StringBuilder(szPlainText);
            StringBuilder szOutStringBuild = new StringBuilder(szPlainText.Length);
            char textch;
            for (int iCount = 0; iCount < szPlainText.Length; iCount++)
            {
                textch = szInputStringBuild[iCount];
                textch = (char)(textch ^ szEncryptionKey);
                szOutStringBuild.Append(textch);
            }
            return szOutStringBuild.ToString();
        }

        public static Company EncryptCompany(Company c)
        {
            c.CompanyId = EncryptString(c.CompanyId, GlobalVars.PassPhrase);
            c.CompanyId2 = EncryptString(c.CompanyId2, GlobalVars.PassPhrase);
            c.SystemPassword = EncryptString(c.SystemPassword, GlobalVars.PassPhrase);
            c.CompanyPassword = EncryptString(c.CompanyPassword, GlobalVars.PassPhrase);
            c.Gun = EncryptString(c.Gun, GlobalVars.PassPhrase);
            c.Gs = EncryptString(c.Gs, GlobalVars.PassPhrase);
            c.Gp = EncryptString(c.Gp, GlobalVars.PassPhrase);
            return c;
        }
        public static List<Company> EncryptCompany(List<Company> lstCm)
        {
            Company cm = new Company();
            List<Company> list = new List<Company>();
            foreach (Company c in lstCm)
            {
                cm = DecryptCompany(c);
                list.Add(cm);
            }
            return list;
        }
        public static Company DecryptCompany(Company c)
        {
            c.CompanyId = DecryptString(c.CompanyId, GlobalVars.PassPhrase);
            c.CompanyId2 = DecryptString(c.CompanyId2, GlobalVars.PassPhrase);
            c.SystemPassword = DecryptString(c.SystemPassword, GlobalVars.PassPhrase);
            c.CompanyPassword = DecryptString(c.CompanyPassword, GlobalVars.PassPhrase);
            c.Gun = DecryptString(c.Gun, GlobalVars.PassPhrase);
            c.Gs = DecryptString(c.Gs, GlobalVars.PassPhrase);
            c.Gp = DecryptString(c.Gp, GlobalVars.PassPhrase);
            return c;
        }
        public static List<Company> DecryptCompany(List<Company> lstCm)
        {
            Company cm = new Company();
            List<Company> list = new List<Company>();
            foreach (Company c in lstCm)
            {
                cm = DecryptCompany(c);
                list.Add(cm);
            }
            return list;
        }
    }
}
