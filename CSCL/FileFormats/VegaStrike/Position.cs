using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.FileFormats.VegaStrike
{
	public class Position
	{
		public string System;
		public string Sector;

		/// <summary>
		/// Crucible/Cephid_17 solche Paramter kommen
		/// </summary>
		/// <param name="MergendPosition"></param>
		public Position(string MergedPosition)
		{
			string[] SplitedPosition=MergedPosition.Split('/');
			System=SplitedPosition[0];
			Sector=SplitedPosition[1];
		}
	}
}
