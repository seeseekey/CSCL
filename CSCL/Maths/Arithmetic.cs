//
//  Arithmetic.cs
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

namespace CSCL.Maths
{
	public static class Arithmetic
	{
		/// <summary>
		/// Returns the power of 2 variant from size
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public static uint GetPowerOf2(uint size)
		{
			uint potenceOf2Size=1;
			while(potenceOf2Size<size) potenceOf2Size*=2;
			if(potenceOf2Size>size) potenceOf2Size/=2;
			return potenceOf2Size;
		}
	}
}
