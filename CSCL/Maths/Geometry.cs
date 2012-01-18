//
//  Geometry.cs
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
	public static class Geometry
	{
		/// <summary>
		/// Distanz zwischen 2 Punkten im 2d Raum
		/// </summary>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <returns></returns>
		public static int Distanz(int x1, int y1, int x2, int y2)
		{
			int xd=System.Math.Abs(x1-x2);
			xd*=xd;
			// schnelles Quadrieren
			int yd=System.Math.Abs(y1-y2);
			yd*=yd;
			return (int)System.Math.Sqrt(xd+yd);
		}
	}
}
