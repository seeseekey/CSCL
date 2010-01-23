using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace CSCL.Crypto
{
    public static partial class Hash
    {
        public static class SHA1
        {
            public static string HashStringToSHA1(string HashString)
            {
                SHA1CryptoServiceProvider SHA1 = new SHA1CryptoServiceProvider();

                byte[] textToHash = System.Text.Encoding.Default.GetBytes(HashString);
                byte[] result = SHA1.ComputeHash(textToHash);

                return System.BitConverter.ToString(result);
            }

            public static string HashFileToSHA1(string filename)
            {
                SHA1CryptoServiceProvider SHA1=new SHA1CryptoServiceProvider();
               
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);

                byte[] textToHash=br.ReadBytes((int)fs.Length);
                br.Close();
                byte[] result=SHA1.ComputeHash(textToHash);

                return System.BitConverter.ToString(result);
            }
        }
    }
}
