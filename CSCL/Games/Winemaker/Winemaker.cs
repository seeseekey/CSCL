using System;
using System.Collections.Generic;
using System.Text;
using CSCL.Games;

namespace CSCL.Games
{
	public partial class Winemaker
	{
		DateTime i_DateTime;
		Player i_Player;
		Vineyard i_Vineyard;
		Weather i_Weather;

		VineAssortment i_VineAssortment;

		Bank i_Bank;

		public Bank BankInfo
		{
			get
			{
				return i_Bank;
			}
		}

		public DateTime DateTime
		{
			get
			{
				return i_DateTime;
			}
		}

		public Vineyard VineyardInfo
		{
			get
			{
				return i_Vineyard;
			}
		}

		public VineAssortment VineAssortmentInfo
		{
			get
			{
				return i_VineAssortment;
			}
		}

		public Player PlayerInfo
		{
			get
			{
				return i_Player;
			}
		}

		public Winemaker(string name, uint age, CultivationArea cultivationArea)
		{
			i_VineAssortment=new VineAssortment();
			i_Weather=new Weather(this);

			DateTime now = DateTime.Now;
			i_DateTime=new DateTime(now.Year, 1, 1); //Aktuelles Jahr, Monat Januar

			i_Player=new Player(name, age);
			i_Vineyard=new Vineyard(this, cultivationArea);

			i_Bank=new Bank(this);
		}

		public void NextMonth()
		{
			//Winemaker Klasse
			i_DateTime=i_DateTime.AddMonths(1); //Nächster Monat

			//Andere Klassen
			i_Bank.NextMonth(); //Kreditbezahlen
			i_Vineyard.NextMonth(); //Wineberg status
		}
	}
}
