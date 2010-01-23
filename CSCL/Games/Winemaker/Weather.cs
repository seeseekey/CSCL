using System;
using System.Collections.Generic;
using System.Text;
using CSCL.Helpers;

namespace CSCL.Games
{
	public partial class Winemaker
	{
		public class Weather
		{
			//Temperaturen in °C (mittler Monatswert)
			double[] MeanTemperatures=new double[12];

			//Niederschlag in mm (mittler Monatswert in Liter pro Quadratmeter)
			int[] MeanRainfall=new int[12];

			//Variablen
			Winemaker i_this;
			double i_Temperature;
			int i_Rainfall;

			//Intern
			int lastMounth=0;

			public double Temperature
			{
				get
				{
					if(i_this.i_DateTime.Month!=lastMounth)	GetWeather();
					return i_Temperature;
				}
			}

			public int Rainfall
			{
				get
				{
					if (i_this.i_DateTime.Month!=lastMounth) GetWeather();
					return i_Rainfall;
				}
			}

			public Weather(Winemaker wm)
			{
				i_this=wm;

				MeanTemperatures[0]=0.6;
				MeanTemperatures[1]=0.0;
				MeanTemperatures[2]=4.2;
				MeanTemperatures[3]=8.7;
				MeanTemperatures[4]=14.1;
				MeanTemperatures[5]=16.9;
				MeanTemperatures[6]=18.9;
				MeanTemperatures[7]=18.7;
				MeanTemperatures[8]=14.1;
				MeanTemperatures[9]=8.9;
				MeanTemperatures[10]=3.6;
				MeanTemperatures[11]=-0.1;

				MeanRainfall[0]=36;
				MeanRainfall[1]=42;
				MeanRainfall[2]=61;
				MeanRainfall[3]=59;
				MeanRainfall[4]=85;
				MeanRainfall[5]=94;
				MeanRainfall[6]=96;
				MeanRainfall[7]=102;
				MeanRainfall[8]=76;
				MeanRainfall[9]=51;
				MeanRainfall[10]=59;
				MeanRainfall[11]=56;
			}

			private void GetWeather()
			{
				lastMounth=i_this.i_DateTime.Month;

				//Temperatur
				double tmp=MeanTemperatures[i_this.i_DateTime.Month-1];
                int posneg=RandomHelpers.GetRandomInt(1);
                double vDouble=RandomHelpers.GetRandomDouble();
                int vInt=RandomHelpers.GetRandomInt(5);
				double vSumme=vDouble+vInt;

				if (posneg==0) //Negativ
				{
					i_Temperature=tmp-vSumme;
				}
				else //Positiv
				{
					i_Temperature=tmp+vSumme;
				}

				//Niederschlag
				i_Rainfall=MeanRainfall[i_this.i_DateTime.Month-1];
			}
		}
	}
}
