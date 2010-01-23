using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Games.Monopoly
{
	public partial class Monopoly
	{
		public class Enums
		{
			/// <summary>
			/// Figur des Players
			/// </summary>
			public enum Figure
			{
				Car, //Auto
				Horse, //Pferd
				Skateboard //Skateboard
			}

			/// <summary>
			/// Feldtyp des Boardplatzes
			/// </summary>
			public enum FieldType
			{
				Start, //LOS Feld
				Street, //Normale Straße
				Community, //Gemeinschaftsfeld
				Event, //Ereignissfeld
				IncomingTax, //Einkommenssteuer
				RailwayStation, //Bahnhof
				Prison, //Gefängnis
				Special, //Wasserwerk etc.
				FreeParking, //Frei Parken
				GoIntoPrison, //Gehe ins Gefängnis
				AdditionalTax //Zusatzsteuer
			}

			/// <summary>
			/// Die Gamefarben der Straßen
			/// </summary>
			public enum StreetColor
			{
				Mauve, //Lila
				LightBlue, //Hellblau
				LightMauve, //HellLila
				Orange, //Orange
				Red, //Rot
				Yellow,
				Green,
				Blue
			}
		}
	}
}
