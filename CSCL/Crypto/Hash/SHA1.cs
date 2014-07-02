//
//  SHA1.cs
//
//  Copyright (c) 2011, 2012 by seeseekey <seeseekey@gmail.com>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
            public static string HashString(string HashString)
            {
                SHA1CryptoServiceProvider SHA1 = new SHA1CryptoServiceProvider();
                    
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
                SHA1CryptoServiceProvider SHA1 = new SHA1CryptoServiceProvider();
               
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
}
