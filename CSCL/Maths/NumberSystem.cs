//
//  NumberSystem.cs
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

namespace CSCL.Maths
{
    public static class NumberSystem
    {
		const uint MIN_RADIX=1;	// Bin�r
		const uint MAX_RADIX=36;// 0-10 + a-z

		/// <summary>
		/// Wandelt es Dezimalzeichen (radix 10) in seine
		/// neue Radixrepresentation (radix x) um
		/// </summary>
		/// <param name="digit"></param>
		/// <param name="radix"></param>
		/// <returns></returns>
		public static char FromDecimalDigit(uint digit, uint radix)
		{
			if (digit>=radix) return '\0';
			if (radix<MIN_RADIX||radix>MAX_RADIX) return '\0';

			if (digit<10) return (char)('0'+digit);

			return (char)('a'-10+digit);
		}


		/// <summary>
		/// Wandelt ein Zeichen (ch) anhand seines radix
		/// in seine Dezimalrepresentation (radix 10) um
		/// </summary>
		/// <param name="ch"></param>
		/// <param name="radix"></param>
		/// <returns></returns>
		public static int ToDecimalDigit(char ch, uint radix)
		{
			int value=-1;

			if (radix>=MIN_RADIX&&radix<=MAX_RADIX)
			{
				if (ch>='0'&&ch<='9') value=ch-'0';
				if (ch>='a'&&ch<='z') value=10+ch-'a';
				if (ch>='A'&&ch<='Z') value=10+ch-'A';
			}

			return (value<(int)radix)?value:-1;
		}

		/// <summary>
		/// Wandelt eine Dezimalzahl (radix 10) in seine
		/// neue Radixrepresentation (radix x) um
		/// </summary>
		/// <param name="number"></param>
		/// <param name="radix"></param>
		/// <returns></returns>
		public static string FromDecimal(uint number, uint radix)
		{
			string numberS=number.ToString();
			string ret="";

			foreach (char sign in numberS)
			{
				ret+=FromDecimalDigit(Convert.ToUInt32(sign), radix);
			}

			return ret;
		}

		/// <summary>
		/// Wandelt einen String (ch) anhand seines radix
		/// in seine Dezimalrepresentation (radix 10) um
		/// </summary>
		/// <param name="number"></param>
		/// <param name="radix"></param>
		/// <returns></returns>
		public static uint ToDecimal(string number, uint radix)
		{
			string ret="";

			foreach (char sign in number)
			{
				ret+=ToDecimalDigit(sign, radix);
			}

			return Convert.ToUInt32(ret);
		}

		/// <summary>
		/// Konvertiert eine Zahl
		/// </summary>
		/// <param name="number"></param>
		/// <param name="sourceRadix"></param>
		/// <param name="targetRadix"></param>
		/// <returns></returns>
		public static string ConvertNumber(string number, uint sourceRadix,  uint targetRadix)
		{
			uint v1=ToDecimal(number, sourceRadix);
			return FromDecimal(v1, targetRadix);
		}


        /// <summary>
        /// Hexadezimal nach Dezimal
        /// </summary>
        /// <param name="hexValue"></param>
        /// <returns></returns>
		public static int HexToDec(string hexValue)
		{
			return Convert.ToInt32(ConvertNumber(hexValue, 16, 10));
		}

        /// <summary>
        /// Dezimal nach Hexadezimal
        /// </summary>
        /// <param name="decValue"></param>
        /// <returns></returns>
        public static string DecToHex(int decValue)
        {
			return ConvertNumber(decValue.ToString(), 10, 16);
        }
    }
}
