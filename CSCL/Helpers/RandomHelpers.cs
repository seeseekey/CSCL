//
//  RandomHelpers.cs
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

namespace CSCL.Helpers
{
    public class RandomHelpers
	{
		public static global::System.Random RndGen=new System.Random();

		/// <summary>
		/// Gibt einen zuf�lligen Int zur�ck
		/// </summary>
		/// <param name="max"></param>
		/// <returns></returns>
		public static int GetRandomInt(int max)
		{
			return RndGen.Next(max);
		}

        public static double GetRandomDouble()
        {
            return RndGen.NextDouble();
        }

		//Gibt einen zuf�lligen Bool zur�ck
		public static bool GetRandomBool()
		{
			int tmp=RndGen.Next(2);

			if (tmp==0) return false;
			else return true;
		}
    }
}
