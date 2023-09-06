using System.Security.Cryptography;

namespace AppPFE.Helpers
{
    public class PasswordHasher
    {
        private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        private static readonly int saltSize = 16;
        private static readonly int HashSize = 20;
        private static readonly int Iterations = 10000;


        public static string Hashpassword(string password)
        {
            byte[] salt;
            rng.GetBytes(salt= new byte[saltSize]);
            var key = new Rfc2898DeriveBytes(password, salt,Iterations);
            var hash = key.GetBytes(HashSize);

            var hashBytes = new byte[saltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, saltSize);
            Array.Copy(hash, 0, hashBytes, saltSize,HashSize);

            var base64hash= Convert.ToBase64String(hashBytes);

            return base64hash;        
        
        }

        public static bool VerifyPassword(string password,string bas64hash)
        {
            var hashBytes = Convert.FromBase64String(bas64hash);

            var salt = new byte[saltSize];
            Array.Copy(hashBytes, 0, salt, 0, saltSize);

            var key = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = key.GetBytes(HashSize);

            for (var i = 0; i < HashSize; i++)
            {   
                    if (hashBytes[i+saltSize]!= hash[i])
                        return false;
               
            }
            return true;
        }


    }
}
