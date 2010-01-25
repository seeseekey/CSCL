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
        /// Berechnet den größten gemeinsamen Teiler zweier Zahlen.
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
        /// Klasse gibt nach Übergabe eines Integer an "Teiler.Summe" die Summe
        /// der echten Teiler aus.
        /// </summary>
        public static int Summe(int zahl)
        {
            int intSumme = 0;
            int intmax = 0;
            //Zahl wird bis zur Hälfte durchsucht
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
        /// Auf die Idee brachte mich der berühmte Carl Friedrich Gauss. Eine Geschichte über ihn erzählt, 
        /// dass sein Lehrer in der Schule die Aufgabe stelle, alle ganzen Zahlen von 1 bis 100 zu addieren, 
        /// in der Hoffnung, für die nächste Stunde seine Ruhe zu haben.
        /// 
        /// Aber der Schüler Gauss stellte fest, dass sich die Summe der ersten und letzten Zahl immer 
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
