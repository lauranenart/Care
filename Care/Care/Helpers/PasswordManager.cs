using System;
using System.Security.Cryptography;
using System.Text;

namespace Care.Helpers
{
    public class PasswordManager
    {
        private const int SALT_SIZE = 32;
      
        public static string HashPassword(string password, string salt)
        {
            SHA256 sha = SHA256.Create();
            var saltedPassword = string.Format("{0}{1}", salt, password);
             byte[] bytes = sha.ComputeHash(Encoding.ASCII.GetBytes(saltedPassword));

             var hashed = new StringBuilder();
             foreach (var b in bytes)
             {
                 hashed.Append(b.ToString("x2"));
             }

             return hashed.ToString();
        }

        public static bool VerifyHashedPassword(string password, string hash, string salt)
        {
            
             var hashOfPassword = HashPassword(password, salt);
             return hashOfPassword.CompareTo(hash) == 0;
        }

        public static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[SALT_SIZE];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }
    }
}
