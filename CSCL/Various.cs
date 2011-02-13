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
		static System.Random TmpRandom=new System.Random(Environment.TickCount);
		static System.Random TmpRandom2=new System.Random((int)Environment.WorkingSet);

		static uint TmpCounter=0;

		/// <summary>
		/// Diese Funtion liefert eine eindeutige ID zurück
		/// </summary>
		/// <returns></returns>
		public static string GetUniqueID()
		{
			if(TmpCounter>4294967200) TmpCounter=0;
			TmpCounter++;

			DateTime TmpDateTime=DateTime.Now;

			return TmpDateTime.Ticks.ToString().PadLeft(20, '0')
				+"-"
				+TmpCounter.ToString().PadLeft(10, '0')
				+"-"
				+TmpRandom2.Next(999999999).ToString().PadLeft(9, '0')
				+"-"
				+TmpRandom.Next(999999999).ToString().PadLeft(9, '0');
		}

		/// <summary>
		/// Diese Funktion liefert die momentane Zeit als ID zurück
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
	}
}
