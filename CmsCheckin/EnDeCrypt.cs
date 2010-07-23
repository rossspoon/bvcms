using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CmsCheckin
{
    public static partial class Util
    {
        private const string cryptoKey = "fgsltw";
        private static readonly byte[] IV = new byte[8] { 240, 3, 45, 29, 0, 76, 173, 59 };
        public static string Encrypt(string s)
        {
            if (s == null || s.Length == 0)
                return string.Empty;
            string result = string.Empty;
            byte[] buffer = Encoding.ASCII.GetBytes(s);
            var des = new TripleDESCryptoServiceProvider();
            var MD5 = new MD5CryptoServiceProvider();
            des.Key = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(cryptoKey));
            des.IV = IV;
            result = Convert.ToBase64String(
                des.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length));
            return result;
        }
        public static string Decrypt(string s)
        {
            if (s == null || s.Length == 0)
                return string.Empty;
            try
            {
                string result = string.Empty;
                byte[] buffer = Convert.FromBase64String(s);
                var des = new TripleDESCryptoServiceProvider();
                var MD5 = new MD5CryptoServiceProvider();
                des.Key = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(cryptoKey));
                des.IV = IV;
                result = Encoding.ASCII.GetString(
                    des.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length));
                return result;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}