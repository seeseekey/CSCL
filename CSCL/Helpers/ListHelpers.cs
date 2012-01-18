//
//  ListHelpers.cs
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
