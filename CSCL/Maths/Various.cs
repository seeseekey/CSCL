using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Maths
{
    public static class Various
    {
        /// <summary>
        /// Converts an integer to a string containing a roman number
        /// </summary>
        /// <param name="Number">The integer</param>
        /// <returns>The string containing the roman</returns>
        public static string ToRoman(long Number)
        {
            if (Number == 0)
            {
                return string.Empty;
            }
            if (Number >= 1000)
            {
                return "M" + ToRoman(Number - 1000);
            }
            if (Number >= 900)
            {
                return "CM" + ToRoman(Number - 900);
            }
            if (Number >= 500)
            {
                return "D" + ToRoman(Number - 500);
            }
            if (Number >= 400)
            {
                return "CD" + ToRoman(Number - 400);
            }
            if (Number >= 100)
            {
                return "C" + ToRoman(Number - 100);
            }
            if (Number >= 90)
            {
                return "XC" + ToRoman(Number - 90);
            }
            if (Number >= 50)
            {
                return "L" + ToRoman(Number - 50);
            }
            if (Number >= 40)
            {
                return "XL" + ToRoman(Number - 40);
            }
            if (Number >= 10)
            {
                return "X" + ToRoman(Number - 10);
            }
            if (Number >= 9)
            {
                return "IX" + ToRoman(Number - 9);
            }
            if (Number >= 5)
            {
                return "V" + ToRoman(Number - 5);
            }
            if (Number >= 4)
            {
                return "IV" + ToRoman(Number - 4);
            }
            if (Number >= 1)
            {
                return "I" + ToRoman(Number - 1);
            }
            return string.Empty;
        }

        /// <summary>
        /// Converts a string containing a roman number to an integer
        /// </summary>
        /// <param name="Roman">The string containing a roman number</param>
        /// <returns>The integer</returns>
        public static long FromRoman(string Roman)
        {
            if (Roman.Length == 0)
            {
                return 0;
            }
            Roman = Roman.ToUpper();
            long intern = 0;
            for (int i = 0; i < Roman.Length; i++)
            {
                int value = 0;
                switch (Roman[i])
                {
                    case 'M':
                        value = +1000;
                        break;
                    case 'D':
                        value = +500;
                        for (int j = i + 1; j < Roman.Length; j++)
                        {
                            if (("M").IndexOf(Roman[j]) != -1)
                            {
                                value = -500;
                                break;
                            }
                        }
                        break;
                    case 'C':
                        value = +100;
                        for (int j = i + 1; j < Roman.Length; j++)
                        {
                            if (("MD").IndexOf(Roman[j]) != -1)
                            {
                                value = -100;
                                break;
                            }
                        }
                        break;
                    case 'L':
                        value = +50;
                        for (int j = i + 1; j < Roman.Length; j++)
                        {
                            if (("MDC").IndexOf(Roman[j]) != -1)
                            {
                                value = -50;
                                break;
                            }
                        }
                        break;
                    case 'X':
                        value = +10;
                        for (int j = i + 1; j < Roman.Length; j++)
                        {
                            if (("MDCL").IndexOf(Roman[j]) != -1)
                            {
                                value = -10;
                                break;
                            }
                        }
                        break;
                    case 'V':
                        value = +5;
                        for (int j = i + 1; j < Roman.Length; j++)
                        {
                            if (("MDCLX").IndexOf(Roman[j]) != -1)
                            {
                                value = -5;
                                break;
                            }
                        }
                        break;
                    case 'I':
                        value = +1;
                        for (int j = i + 1; j < Roman.Length; j++)
                        {
                            if (("MDCLXV").IndexOf(Roman[j]) != -1)
                            {
                                value = -1;
                                break;
                            }
                        }
                        break;
                    default:
                        throw new ArgumentException("Roman number string contains an illegal character at index " + i.ToString());
                }
                intern += Convert.ToInt64(value);
            }
            return Convert.ToInt64(intern);
        }
    }
}
