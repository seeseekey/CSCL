using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Games
{
	public partial class Winemaker
	{
		/// <summary>
		/// Das Weinsortiment
		/// </summary>
		public class VineAssortment
		{
			List<Wine> i_Assortment;

			public List<Wine> Assortment
			{
				get
				{
					return i_Assortment;
				}
			}

			public VineAssortment()
			{
				i_Assortment=new List<Wine>();

				Wine aWine=new Wine("Samling 88", 10);
				i_Assortment.Add(aWine);

				aWine=new Wine("Muskat Ottonel", 12);
				i_Assortment.Add(aWine);

				aWine=new Wine("Chardonnay", 14);
				i_Assortment.Add(aWine);

				aWine=new Wine("Traminer", 8);
				i_Assortment.Add(aWine);

				aWine=new Wine("Bouvier", 9);
				i_Assortment.Add(aWine);

				aWine=new Wine("Welschriesling", 8);
				i_Assortment.Add(aWine);
			}

			/// <summary>
			/// Gibt den Wein anhand des Namens zurück
			/// </summary>
			/// <param name="name"></param>
			/// <returns></returns>
			public Wine GetWine(string name)
			{
				foreach (Wine i in i_Assortment)
				{
					if (i.Name==name) return i;
				}

				throw new Exception("WineNotFound!");
			}
		}
	}
}
