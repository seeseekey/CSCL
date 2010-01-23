using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL
{
	/// <summary>
	/// Verschiedende nicht zuordedbare Funktionen
	/// </summary>
	public static class Various
	{
		/// <summary>
		/// Diese Funtion liefert eine eindeutige ID zurück
		/// </summary>
		/// <returns></returns>
		public static string GetUniqueID()
		{
			DateTime TmpDateTime = DateTime.Now;
			System.Random TmpRandom = new System.Random(Environment.TickCount);
			System.Random TmpRandom2 = new System.Random((int)Environment.WorkingSet);

			return TmpDateTime.Ticks.ToString().PadLeft(20, '0')
				 + '-'
				 + TmpRandom2.Next(999999999).ToString().PadLeft(9, '0')
				 + '-'
				 + TmpRandom.Next(999999999).ToString().PadLeft(9, '0');
		}

		/// <summary>
		/// Diese Funktion liefert die momentane Zeit als ID zurück
		/// </summary>
		/// <returns></returns>
		public static string GetTimeID()
		{
			return DateTime.Now.Ticks.ToString().PadLeft(20, '0');
		}
	}
}
