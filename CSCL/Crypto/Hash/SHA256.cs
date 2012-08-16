using System;
using System.Security.Cryptography;
using System.IO;

namespace CSCL.Crypto
{
    public static class SHA256
    {
        public static string HashString(string HashString)
        {
            SHA256CryptoServiceProvider SHA1 = new SHA256CryptoServiceProvider();
            
            byte[] arrayData;
            byte[] arrayResult;
            string result = null;
            string temp = null;
            
            arrayData = System.Text.Encoding.UTF8.GetBytes(HashString);
            arrayResult = SHA1.ComputeHash(arrayData);
            
            for (int i = 0; i < arrayResult.Length; i++)
            {
                temp = Convert.ToString(arrayResult [i], 16);
                if (temp.Length == 1)
                    temp = "0" + temp;
                result += temp;
            }
            
            return result;
        }
        
        public static string HashFile(string filename)
        {
            SHA256CryptoServiceProvider SHA1 = new SHA256CryptoServiceProvider();
            
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            
            byte[] textToHash = br.ReadBytes((int)fs.Length);
            br.Close();
            byte[] arrayResult = SHA1.ComputeHash(textToHash);
            
            string result = null;
            string temp = null;
            
            for (int i = 0; i < arrayResult.Length; i++)
            {
                temp = Convert.ToString(arrayResult [i], 16);
                if (temp.Length == 1)
                    temp = "0" + temp;
                result += temp;
            }
            
            return result;
        }
    }
}

