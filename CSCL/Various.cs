//
//  Various.cs
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
using System.Reflection;

namespace CSCL
{
	/// <summary>
	/// Verschiedende nicht zuordedbare Funktionen
	/// </summary>
	public static class Various
	{
		static System.Random rndOne=new System.Random(Environment.TickCount);
		static System.Random rndTwo=new System.Random((int)Environment.WorkingSet);
		static uint counter=0;

		/// <summary>
		/// Diese Funtion liefert eine eindeutige ID zur�ck
		/// </summary>
		/// <returns></returns>
		public static string GetUniqueID()
		{
			if(counter>4294967200) counter=0;
			counter++;

			DateTime now=DateTime.Now;

			return now.Ticks.ToString().PadLeft(20, '0')
				+"-"
				+counter.ToString().PadLeft(10, '0')
				+"-"
				+rndTwo.Next(999999999).ToString().PadLeft(9, '0')
				+"-"
				+rndOne.Next(999999999).ToString().PadLeft(9, '0');
		}

		/// <summary>
		/// Diese Funktion liefert die momentane Zeit als ID zur�ck
		/// </summary>
		/// <returns></returns>
		public static string GetTimeID()
		{
			return DateTime.Now.Ticks.ToString().PadLeft(20, '0');
		}

		public static string GetReadableTimeID()
		{
			DateTime now=DateTime.Now;
			return String.Format("[{0:D4}.{1:D2}.{2:D2}] -> [{3:D2}:{4:D2}:{5:D2}:{6:D3}]", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);
		}

		public static string AssemblyVersion
		{
			get
			{
				string versionstring=Assembly.GetEntryAssembly().GetName().Version.ToString();
				if(versionstring.EndsWith(".0"))
				{
					versionstring=versionstring.Substring(0, versionstring.Length-2);
					if(versionstring.EndsWith(".0")) versionstring=versionstring.Substring(0, versionstring.Length-2);
				}
				return versionstring;
			}
		}
	}
}
