using System;
using System.Security.Cryptography;

namespace CSCL
{
    public static class SHA256
    {
        public static string HashStringToSHA256(string HashString)
        {
            SHA256CryptoServiceProvider SHA256 = new SHA256CryptoServiceProvider();
                
            byte[] textToHash = System.Text.Encoding.Default.GetBytes(HashString);
            byte[] result = SHA256.ComputeHash(textToHash);
                
            return System.BitConverter.ToString(result);
        }
    }
}

