using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Games.Monopoly
{
	public partial class Monopoly
	{
		#region SpecialRules
		bool DoubleSumWhenGoOfStartField=false; //Doppelte summe wenn man direkt auf Los landet
		#endregion
		//        * Würfeln und die Figur bewegen
		//* Je nach Art des erreichten Feldes bestimmte Aktionen ausführen: Miete oder Steuern bezahlen bzw. Einnahmen erhalten, evtl. das eigene Grundstück durch Gebäude oder Hotel aufwerten, An- und Verkäufe; die von den Karten bestimmte Aktionen ausführen.
		//* Auch wenn ein Spieler im Gefängnis sitzt, kann er weiterhin Häuser bauen, Grundstücke kaufen oder verkaufen und sogar Miete kassieren.

		List<Player> i_Players;
		int i_CurrentPlayer;

		public void NextPlayer()
		{
			//i_Active

			if(i_CurrentPlayer==i_Players.Count-1)
			{
				i_CurrentPlayer=0;
			}
			else i_CurrentPlayer+=1;
		}

		public Monopoly(List<Player> Players)
		{
			i_Players=Players;
			i_CurrentPlayer=0;
		}
	}
}
