using System;
using System.Security.Cryptography;
using System.Text;

namespace DistanceEducation
{
    public class utils
    {
        public static string hashPassword(string password)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            byte[] password_bytes = Encoding.ASCII.GetBytes(password);
            byte[] encripted_bytes = sha1.ComputeHash(password_bytes);
            return Convert.ToBase64String(encripted_bytes);
        }
    }
}
