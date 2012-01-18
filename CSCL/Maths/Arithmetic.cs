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
		/// Macht eine Zahl Power of To
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public static uint MakePowerOf2(uint size)
		{
			uint potenceOf2Size=1;
			while (potenceOf2Size<size) potenceOf2Size*=2;
			if (potenceOf2Size>size) potenceOf2Size/=2;
			return potenceOf2Size;
		}

        /// <summary>
        /// Berechnet den gr��ten gemeinsamen Teiler zweier Zahlen.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static int LeastCommonMultiple(int n, int m)
        {
            //TODO: ? Operator durch If ersetzen
            return n == m ? n : n < m ? LeastCommonMultiple(n, m - n) : LeastCommonMultiple(n - m, m);
        }

        /// <summary>
        /// Klasse gibt nach �bergabe eines Integer an "Teiler.Summe" die Summe
        /// der echten Teiler aus.
        /// </summary>
        public static int Summe(int zahl)
        {
            int intSumme = 0;
            int intmax = 0;
            //Zahl wird bis zur H�lfte durchsucht
            for(int i = 1; i < (zahl / 2); i++)
            {
                if(zahl % i == 0)
                {
                    if(zahl / i == intmax)
                    {
                        break;
                        //Abbruchkriterium, falls die Zahlen sich wiederholen
                    }//of if
                    intmax = i;
                    intSumme = intSumme + i;
                    intSumme = intSumme + zahl / i;
                }//of if

            }//of for
            intSumme = intSumme - zahl;
            //Um Summe der Echten Teiler zu erhalten wird die Zahl von
            //der Teilersumme abgezogen			
            return intSumme;
        }//of Summe()

        /// <summary>
        /// Auf die Idee brachte mich der ber�hmte Carl Friedrich Gauss. Eine Geschichte �ber ihn erz�hlt, 
        /// dass sein Lehrer in der Schule die Aufgabe stelle, alle ganzen Zahlen von 1 bis 100 zu addieren, 
        /// in der Hoffnung, f�r die n�chste Stunde seine Ruhe zu haben.
        /// 
        /// Aber der Sch�ler Gauss stellte fest, dass sich die Summe der ersten und letzten Zahl immer 
        /// wieder ergibt, also beim ersten Zahlenpaar 1 + 100 = 101, beim zweiten Zahlenpaar 2 + 99 = 
        /// 101 u.s.w. Nun brauchte er nur noch die Anzahl der Paare ermitteln und mit 101 multiplizieren.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double SummeDerGanzenZahlenVonBis(int a, int b)
        {
            return (b - a) / 2 * (b + a) + (b + a) / 2;
        }

        /// <summary>
        /// Calculates the Greatest Common Dividor of two integer
        /// </summary>
        /// <param name="Number1">the first integer</param>
        /// <param name="Number2">the second integer</param>
        /// <returns>the Greatest Common Dividor</returns>
        public static long GetGCD(long Number1, long Number2)
        {
            long remainder = 0;
            do
            {
                remainder = Number1 % Number2;
                Number1 = Number2;
                Number2 = remainder;
            } while(remainder != 0);
            return Number1;
        }

        /// <summary>
        /// Calculates the Smallest Common Multiple
        /// </summary>
        /// <param name="Number1">the first integer</param>
        /// <param name="Number2">the first integer</param>
        /// <returns>the Smallest Common Multiple</returns>
        public static long GetSCM(long Number1, long Number2)
        {
            return (Number1 * Number2) / GetGCD(Number1, Number2);
        }

        /// <summary>
        /// Checks if an integer divides another
        /// </summary>
        /// <param name="Number1">the Divident</param>
        /// <param name="Number2">the Dividor</param>
        /// <returns>true if Number2 ist dividor of Number1, else false</returns>
        public static bool IsDivider(long Number1, long Number2)
        {
            return (Number1 % Number2) == 0;
        }
    }
}
