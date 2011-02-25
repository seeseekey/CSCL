using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Games.Manasource
{
	public static class Enums
{
		public static Elements GetElementFromString(string element)
		{
			switch(element.ToLower())
			{
				case "fire":
					{
						return Elements.Fire;
					}
				case "earth":
					{
						return Elements.Earth;
					}
				case "ice":
					{
						return Elements.Ice;
					}
				default:
					{
						throw new NotImplementedException();
					}
			}
		}
}

	public enum Elements
	{
		Fire,
		Earth,
		Ice
	}
}
