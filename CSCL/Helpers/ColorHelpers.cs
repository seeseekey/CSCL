//
//  ColorHelpers.cs
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
using System.Drawing;

namespace CSCL.Helpers
{
    public class ColorHelpers
    {
        /// <summary>
        /// Gets the color by percent.
        /// 
        /// Diese Methode gibt eine System.Drawing.Color (von gelb bis rot) in Abh�ngigkeit von dem �bergebenen Prozentwert zur�ck.
        /// 
        /// Beispiel:
        /// 0% gr�n
        /// 50% gelb
        /// 75% orange
        /// 100% rot
        /// 
        /// Ich habe die Funktion genutzt um ein Oberfl�chenelement in Abh�ngigkeit von der Prozessorlast einzuf�rben.
        /// </summary>
        /// <param name="percent">The value 0 - 100%</param>
        /// <returns>System.Drawing.Color dependent on the value</returns>
        public static Color GetColorByValue(double percent)
        {
            if (percent > 100.0)
                throw new ArgumentException("percent cannot be larger than 100.0");

            double red;
            double green;

            if (percent > 50.0)
            {
                green = (100.0 - percent) * 5.1;
                red = 255.0;
            }
            else
            {
                green = 255.0;
                red = percent * 5.1;
            }
            return Color.FromArgb((int)red, (int)green, 0);
        }

		/// <summary>
		/// Gibt eine zuf�llige Farbe zur�ck
		/// </summary>
		/// <returns></returns>
        public static Color GetRandomColor()
        {
			System.Random random=new System.Random();
            return Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
        }
    }
}
