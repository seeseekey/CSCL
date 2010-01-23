using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Games
{
	public partial class Winemaker
	{
		public class Player
		{
			string i_Name; //Playername
			uint i_StartAge; //Startalter des Spielers
			uint i_Age; //Alter des Player
			long i_Money; //Geld des Spielers in €

			public string Name
			{
				get
				{
					return i_Name;
				}
			}

			public uint Age
			{
				get
				{
					return i_Age;
				}
			}

			public long Money
			{
				get
				{
					return i_Money;
				}
				internal set
				{
					i_Money=value;
				}
			}

			public Player(string name, uint age)
			{
				i_Name=name;
				i_Age=age;
				i_StartAge=age;
				i_Money=55000;
			}
		}
	}
}
