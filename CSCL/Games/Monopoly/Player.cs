using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Games.Monopoly
{
	public partial class Monopoly
	{
		public class Player
		{
			string i_Name;
			int i_Money;
			Enums.Figure i_Figure;
			int i_BoardPosition;
			bool i_Active;

			public int BoardPosition
			{
				get
				{
					return i_BoardPosition;
				}
				internal set
				{
					i_BoardPosition=value;
				}
			}

			public Player(string Name, Enums.Figure Figure)
			{
				i_Name=Name;
				i_Money=Constants.MONEY_GAMESTART;
				i_Figure=Figure;
				i_BoardPosition=0;
				i_Active=true;
			}
		}
	}
}
