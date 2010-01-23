using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Math
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
