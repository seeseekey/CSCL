using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CSCL.Helpers
{
	public class ListHelpers
	{
		/// <summary>
		/// Wirft eine generische Liste durcheinander
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="ilist"></param>
		public static void Shuffle<T>(IList<T> ilist)
		{
			int iIndex;
			T tTmp;
			for (int i=1; i<ilist.Count; ++i)
			{
				iIndex=RandomHelpers.RndGen.Next(i+1);
				tTmp=ilist[i];
				ilist[i]=ilist[iIndex];
				ilist[iIndex]=tTmp;
			}
		}

        /// <summary>
        /// Schreibt eine Liste in eine Datei
        /// </summary>
        /// <param name="liste"></param>
        /// <param name="filename"></param>
        public static void WriteListIntofile(List<string> liste, string filename)
        {
            StreamWriter sw=new StreamWriter(filename);

            foreach(string i in liste)
            {
                sw.WriteLine(i);
            }

            sw.Close();
        }
	}
}
