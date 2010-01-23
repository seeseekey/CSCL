using System;
using System.Collections.Generic;
using System.Text;
using CSCL;
using CSCL.Helpers;

namespace CSCL.Games
{
	public partial class Winemaker
	{
		public class Vineyard
		{
			int i_VinePerHa=200; //Rebenanzahl pro Hektar
			long i_Area; //Gebietsgröße in Ha
			double i_Temperature; //Temperatur
			int i_State; //Status des Weinberges (0-100)
			int i_StateWater; //Bewässerungsstatus

			Dictionary<Wine, int> i_Vines; //Weinreben

			CultivationArea i_CultivationArea; //Anbaugebiet

			Winemaker i_this;

			public Dictionary<Wine, int> Vines
			{
				get
				{
					return i_Vines;
				}
			}

			/// <summary>
			/// Freie Reben
			/// </summary>
			public long FreeVines
			{
				get
				{
					int TakenWine=0;

					foreach (int i in i_Vines.Values)
					{
						TakenWine+=i;
					}

					return i_VinePerHa*i_Area-TakenWine;
				}
			}

			public long Area
			{
				get
				{
					return i_Area;
				}
			}

			public double Temperature
			{
				get
				{
					return i_Temperature;
				}
			}

			public int State
			{
				get
				{
					return i_State;
				}
			}

			public int StateWater
			{
				get
				{
					return i_StateWater;
				}
			}

			public CultivationArea CultivationArea
			{
				get
				{
					return i_CultivationArea;
				}
			}

			public Vineyard(Winemaker wm, CultivationArea cultivationArea)
			{
				i_Area=3;
				i_Temperature=-2;
				i_State=100;
				i_StateWater=79;
				i_CultivationArea=cultivationArea;

				i_this=wm;

				i_Vines=new Dictionary<Wine, int>();

				foreach (Wine i in i_this.i_VineAssortment.Assortment)
				{
					i_Vines.Add(i, 0);
				}
			}

			/// <summary>
			/// Reben anbauen
			/// </summary>
			/// <param name="wine"></param>
			/// <param name="count"></param>
			/// <returns></returns>
			public GameStateReturnCode GrowWine(Wine wine, int count)
			{
				if (i_this.i_DateTime.Month>7) return GameStateReturnCode.GrowWineOnlyBetweenJanuaryAndJuly;
				if (count>FreeVines) return GameStateReturnCode.NotEnoughFreeVines;
				if (i_this.PlayerInfo.Money<count*wine.PricePerVine) return GameStateReturnCode.NotEnoughMoney;

				i_Vines[wine]+=count; //Rebanzahl +
				i_this.PlayerInfo.Money-=count*wine.PricePerVine; //Money

				return GameStateReturnCode.Okay;
			}

			public void NextMonth()
			{
				//Temperatur
				i_Temperature=i_this.i_Weather.Temperature;

				//Weinbergstatus
				if (RandomHelpers.GetRandomBool()) //Wenn true;
				{
					i_State-=RandomHelpers.GetRandomInt(4);
					if (i_State<0) i_State=0;
				}

				//Bewässerungsstatus
				int rainfall=i_this.i_Weather.Rainfall; //Liter pro Quadratmeter
				int rainfallPerHa=rainfall*10000; //Regenfall pro Hektar
				int rainfallAll = rainfallPerHa*(int)i_Area;

				int waterNeeded=(int)i_Area*26250; //Liter Wasser benötigt werden

				int prozentRainfall=rainfallAll/(waterNeeded/100);

				if (prozentRainfall<100)
				{
					i_StateWater-=(100-prozentRainfall);
				}

				//78750/100 = 1% also 1% sind 787,5 das musst du wissen und
				//80000/787,5 = 101,5873% wenn ich mich nicht verrechnet habe

				//315 Kubikmeter je Hektar für wein (pro jahr)
				//315000 Liter je Hektar pro Jahr
				//26250 Liter je Hektar pro Monat
			}
		}
	}
}
