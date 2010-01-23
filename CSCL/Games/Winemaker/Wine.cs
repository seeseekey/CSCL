using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Games
{
	public partial class Winemaker
	{
		public class Wine
		{
			string i_Name; //Weinname
			int i_PricePerVine; //Preis pro Rebe

			public string Name
			{
				get
				{
					return i_Name;
				}
			}

			public int PricePerVine
			{
				get
				{
					return i_PricePerVine;
				}
			}

			public Wine(string name, int pricePerWine)
			{
				i_Name=name;
				i_PricePerVine=pricePerWine;
			}
		}
	}
}
