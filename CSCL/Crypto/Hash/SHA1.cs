//
//  SHA1.cs
//
//  Copyright (c) 2011, 2012 by seeseekey <seeseekey@googlemail.com>
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
