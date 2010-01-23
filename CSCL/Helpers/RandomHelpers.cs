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
