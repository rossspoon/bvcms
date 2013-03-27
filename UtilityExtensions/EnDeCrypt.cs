using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Linq;

namespace UtilityExtensions
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
        public static string EncryptForUrl(string s)
        {
            if (s == null || s.Length == 0)
                return string.Empty;
            string result = string.Empty;
            byte[] buffer = Encoding.ASCII.GetBytes(s);
            var des = new TripleDESCryptoServiceProvider();
            var MD5 = new MD5CryptoServiceProvider();
            des.Key = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(cryptoKey));
            des.IV = IV;
            result = HttpServerUtility.UrlTokenEncode(
                des.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length));
            return result;
        }

        public static string DecryptFromUrl(string s)
        {
            if (s == null || s.Length == 0)
                return string.Empty;
            try
            {
                string result = string.Empty;
                byte[] buffer = HttpServerUtility.UrlTokenDecode(s);
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

        // ------------ New Encrpytion Code ----------------

        public static string Encrypt(string sText, string sPrefix)
        {
            if (sText == null || sText.Length == 0 || sPrefix == null || sPrefix.Length == 0)
                return string.Empty;

            string result = string.Empty;
            byte[] buffer = Encoding.ASCII.GetBytes(sText);

            var des = new TripleDESCryptoServiceProvider();
            var MD5 = new MD5CryptoServiceProvider();
            
            var sSalt = getSalt( sPrefix + "Salt" );

            des.Key = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes( getKey( sPrefix + "Key" ) ) );
            des.IV = sSalt.Split(' ').Select(s => Convert.ToByte(s, 10)).ToArray();

            result = Convert.ToBase64String( des.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length) );
            return result;
        }

        public static string Decrypt(string sText, string sPrefix)
        {
            if (sText == null || sText.Length == 0 || sPrefix == null || sPrefix.Length == 0)
                return string.Empty;

            try
            {
                string result = string.Empty;
                byte[] buffer = Convert.FromBase64String(sText);

                var des = new TripleDESCryptoServiceProvider();
                var MD5 = new MD5CryptoServiceProvider();

                des.Key = MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes( getKey( sPrefix + "Key" ) ) );
                des.IV = getSalt(sPrefix + "Salt").Split(' ').Select(s => Convert.ToByte(s, 10)).ToArray();

                result = Encoding.ASCII.GetString( des.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length) );
                return result;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string getKey( string sKeyName )
        {
            return System.Configuration.ConfigurationManager.AppSettings[sKeyName] ?? "";
        }

        public static string getSalt( string sSaltName )
        {
            return System.Configuration.ConfigurationManager.AppSettings[sSaltName] ?? "";
        }

        public static string getMasked(string sText, int iCount, bool bFromRight, string sMask)
        {
            if (sText == null || sText.Length == 0 || sText.Length < iCount) return "";

            if (bFromRight)
                return sMask + sText.Substring(sText.Length - iCount, iCount);
            else
                return sText.Substring(0, iCount) + sMask;
        }
    }
}