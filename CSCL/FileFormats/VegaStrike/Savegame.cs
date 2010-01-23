using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Globalization;

namespace CSCL.FileFormats.VegaStrike
{
	/// <summary>
	/// Klasse zum Laden, Speichern und Verändern von
	/// Vega Strike Savegames
	/// </summary>
	public class Savegame
	{
		List<string> RestOfFile=new List<string>(); //Der Rest der Datei welcher nicht verändert wird

		Position CurrentLocation;
		Int64 CurrentMoney;

		double UnknownValue1;
		double UnknownValue2;
		double UnknownValue3;

		List<Ship> Ships;

		#region Private Funktionen
		/// <summary>
		/// Gibt das aktive Schiff zurück
		/// </summary>
		/// <returns></returns>
		private Ship GetActiveShip()
		{
			foreach (Ship i in Ships)
			{
				if (i.ActiveShip) return i;
			}

			throw new Exception("Kein aktives Schiff vorhanden!");
		}
		#endregion

		public void Load(string filename)
		{
			StreamReader sr=new StreamReader(filename);

			//Headerline wird gelesen
			//Beispiele:
			//Crucible/Cephid_17^990000500.000000^Llama.begin 119990000070.852110 -8999928.352009 -109989999927.620540
			//Crucible/Cephid_17^1082579456.000000^tesla|Llama.begin|Crucible/Cephid_17|Progeny.rgspec|Crucible/Cephid_17|Llama.stock|Crucible/Cephid_17 119995804019.487700 -8662679.368102 -109995814603.707200
			//Mom System		Geld
			string header=sr.ReadLine();
			string[] SplitedHeader=header.Split('^');

			//System und geld
			CurrentLocation=new Position(SplitedHeader[0]);
			CurrentMoney=Convert.ToInt64(Convert.ToDouble(SplitedHeader[1]));

			//Schiffe
			string[] SplitedRest=SplitedHeader[2].Split(' ');
			string[] SplitedShips=SplitedRest[0].Split('|');

			Ships=new List<Ship>();

			for(int i=0; i<SplitedShips.Length; i++)
			{
				Ship tmpShip=new Ship();

				if(i==0)
				{
					tmpShip.ActiveShip=true;
					tmpShip.Name=SplitedShips[i];
					tmpShip.Location=CurrentLocation;
				}
				else
				{
					tmpShip.ActiveShip=false;
					tmpShip.Name=SplitedShips[i];
					tmpShip.Location=new Position(SplitedShips[i+1]);
					i++;
				}

				Ships.Add(tmpShip);
			}

			//Unbekannte Werte
			UnknownValue3=Convert.ToDouble(SplitedRest[SplitedRest.Length-1]);
			UnknownValue2=Convert.ToDouble(SplitedRest[SplitedRest.Length-2]);
			UnknownValue1=Convert.ToDouble(SplitedRest[SplitedRest.Length-3]);

			//Rest der Datei
			RestOfFile=new List<string>();
			while(!sr.EndOfStream)
			{
				RestOfFile.Add(sr.ReadLine());
			}

			sr.Close();
		}

		public void Save(string filename)
		{
			//Crucible/Cephid_17^990000500.000000^Llama.begin 119990000070.852110 -8999928.352009 -109989999927.620540
			//Crucible/Cephid_17^1082579456.000000^tesla|Llama.begin|Crucible/Cephid_17|Progeny.rgspec|Crucible/Cephid_17|Llama.stock|Crucible/Cephid_17 119995804019.487700 -8662679.368102 -109995814603.707200
			//Mom System		Geld
			CultureInfo ci=Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture=CultureInfo.InvariantCulture;

			Ship ActiveShip=GetActiveShip();
			string header=String.Format("{0}/{1}^{2:F6}^{3}", CurrentLocation.System, CurrentLocation.Sector, CurrentMoney, ActiveShip.Name);

			if (Ships.Count>1)
			{
				//Restliche Schiffe hinzufügen
				foreach (Ship i in Ships)
				{
					if (i.ActiveShip!=true)
					{
						header+=String.Format("|{0}|{1}/{2}", i.Name, i.Location.System, i.Location.Sector);
					}
				}

				header+=String.Format(" {0:F6} {1:F6} {2:F6}", UnknownValue1, UnknownValue2, UnknownValue3);
			}
			else if (Ships.Count==1) //Nur ein Schiff
			{
				header+=String.Format(" {0:F6} {1:F6} {2:F6}", UnknownValue1, UnknownValue2, UnknownValue3);
			}

			//In die Datei schreiben
			StreamWriter sw=new StreamWriter(filename);
			sw.WriteLine(header);

			//Rest schreiben
			foreach (string i in RestOfFile)
			{
				sw.WriteLine(i);
			}

			sw.Close();

			Thread.CurrentThread.CurrentCulture=ci;
		}
	}
}
