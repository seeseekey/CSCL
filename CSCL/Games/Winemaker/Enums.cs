using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Games
{
	public partial class Winemaker
	{
		/// <summary>
		/// Anbaugebiet
		/// </summary>
		public enum CultivationArea
		{
			Burgenland,
			Steiermark,
			Wachau,
			Weinviertel
		}

		public enum GameStateReturnCode
		{
			Okay, //Alles in Ordnung
			GrowWineOnlyBetweenJanuaryAndJuly, //Reben anpflanzen nur zwischen Januar und Juli
			ReadWineOnlyBetweenAugustAndDecember, //Reben lesen nur zwischen August und Dezember
			NotEnoughFreeVines, //Nicht genug Reben
			NotEnoughMoney //Nicht genug Geld
		}

	}
}
