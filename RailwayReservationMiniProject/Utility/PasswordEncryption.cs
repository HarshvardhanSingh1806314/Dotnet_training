using System;
using System.Security.Cryptography;

namespace Utility
{
    public class PasswordEncryption
    {
        public static (string hashedPassword, string salt) HashPassword(string password)
        {
            // generating a random salt value
            using (var rngProvider = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[16];
                rngProvider.GetBytes(salt);

                // hashing the password with salt value generated above using PBKDF2 slow hashing algorithm
                using(var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
                {
                    byte[] hash = pbkdf2.GetBytes(32);
                    string hashString = Convert.ToBase64String(hash);
                    string saltString = Convert.ToBase64String(salt);

                    return (hashString, saltString);
                }
            }
        }

        public static bool VerifyPassword(string enteredPassword, string storedHashedPassword, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);
            byte[] storedHashedPasswordBytes = Convert.FromBase64String(storedHashedPassword);

            using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 10000))
            {
                byte[] computedHash = pbkdf2.GetBytes(32);
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHashedPasswordBytes[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
