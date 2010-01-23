using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Games.Monopoly
{
	public partial class Monopoly
	{
		class Board
		{
			/// <summary>
			/// Ein Feld auf dem Board
			/// </summary>
			class Field
			{
				Enums.FieldType i_FieldType; //Feldtyp
				string i_StreetName;
				int i_BasePrice; //Basispreis
				Enums.StreetColor i_StreetColor;

				public Field(Enums.FieldType FieldType)
				{
					i_FieldType=FieldType;
					i_StreetName="";
					i_BasePrice=0;
				}

				public Field(Enums.FieldType FieldType, string StreetName, int BasePrice)
				{
					i_FieldType=FieldType;
					i_StreetName=StreetName;
					i_BasePrice=BasePrice;
				}

				public Field(Enums.FieldType FieldType, string StreetName, int BasePrice, Enums.StreetColor StreetColor)
				{
					i_FieldType=FieldType;
					i_StreetName=StreetName;
					i_BasePrice=BasePrice;
					i_StreetColor=StreetColor;
				}
			}

			List<Field> i_Fields;

			public Board()
			{
				i_Fields=new List<Field>();

				//Erzeuge Felder
				i_Fields.Add(new Field(Enums.FieldType.Start)); //LOS Feld
				i_Fields.Add(new Field(Enums.FieldType.Street, "Badstraße", 40, Enums.StreetColor.Mauve));
				i_Fields.Add(new Field(Enums.FieldType.Community));
				i_Fields.Add(new Field(Enums.FieldType.Street, "Turmstraße", 80, Enums.StreetColor.Mauve));
				i_Fields.Add(new Field(Enums.FieldType.IncomingTax));
				i_Fields.Add(new Field(Enums.FieldType.RailwayStation, "Südbahnhof", 500));
				i_Fields.Add(new Field(Enums.FieldType.Street, "Chausseestraße", 120, Enums.StreetColor.LightBlue));
				i_Fields.Add(new Field(Enums.FieldType.Event));
				i_Fields.Add(new Field(Enums.FieldType.Street, "Elisenstraße", 120, Enums.StreetColor.LightBlue));
				i_Fields.Add(new Field(Enums.FieldType.Street, "Poststraße", 160, Enums.StreetColor.LightBlue));
				i_Fields.Add(new Field(Enums.FieldType.Prison));
				i_Fields.Add(new Field(Enums.FieldType.Street, "Seestraße", 120, Enums.StreetColor.LightMauve));
				i_Fields.Add(new Field(Enums.FieldType.Special, "Elektrizitätswerk", 80));
				i_Fields.Add(new Field(Enums.FieldType.Street, "Hafenstraße", 200, Enums.StreetColor.LightMauve));
				i_Fields.Add(new Field(Enums.FieldType.Street, "Neue Straße", 240, Enums.StreetColor.LightMauve));
				i_Fields.Add(new Field(Enums.FieldType.RailwayStation, "Westbahnhof", 500));
				i_Fields.Add(new Field(Enums.FieldType.Street, "Münchner Straße", 280, Enums.StreetColor.Orange));
				i_Fields.Add(new Field(Enums.FieldType.Community));
				i_Fields.Add(new Field(Enums.FieldType.Street, "Wiener Straße", 280, Enums.StreetColor.Orange));
				i_Fields.Add(new Field(Enums.FieldType.Street, "Berliner Straße", 320, Enums.StreetColor.Orange));
				i_Fields.Add(new Field(Enums.FieldType.FreeParking));
				i_Fields.Add(new Field(Enums.FieldType.Street, "Theaterstraße", 360, Enums.StreetColor.Red));
				i_Fields.Add(new Field(Enums.FieldType.Event));
				i_Fields.Add(new Field(Enums.FieldType.Street, "Museumsstraße", 360, Enums.StreetColor.Red));
				i_Fields.Add(new Field(Enums.FieldType.Street, "Opernplatz", 400, Enums.StreetColor.Red));
				i_Fields.Add(new Field(Enums.FieldType.RailwayStation, "Nordbahnhof", 500));
				i_Fields.Add(new Field(Enums.FieldType.Street, "Lessingstraße", 440, Enums.StreetColor.Yellow));
				i_Fields.Add(new Field(Enums.FieldType.Street, "Schillerstraße", 440, Enums.StreetColor.Yellow));
				i_Fields.Add(new Field(Enums.FieldType.Special, "Wasserwerk", 80));
				i_Fields.Add(new Field(Enums.FieldType.Street, "Goethestraße", 440, Enums.StreetColor.Yellow));
				i_Fields.Add(new Field(Enums.FieldType.GoIntoPrison));
				i_Fields.Add(new Field(Enums.FieldType.Street, "Rathausplatz", 520, Enums.StreetColor.Green));
				i_Fields.Add(new Field(Enums.FieldType.Street, "Hauptstraße", 520, Enums.StreetColor.Green));
				i_Fields.Add(new Field(Enums.FieldType.Community));
				i_Fields.Add(new Field(Enums.FieldType.Street, "Bahnhofstraße", 560, Enums.StreetColor.Green));
				i_Fields.Add(new Field(Enums.FieldType.RailwayStation, "Hauptbahnhof", 500));
				i_Fields.Add(new Field(Enums.FieldType.Event));
				i_Fields.Add(new Field(Enums.FieldType.Street, "Parkstraße", 700, Enums.StreetColor.Blue));
				i_Fields.Add(new Field(Enums.FieldType.AdditionalTax));
				i_Fields.Add(new Field(Enums.FieldType.Street, "Schloßallee", 1000, Enums.StreetColor.Blue));
			}

			public void SetPlayer(Player Player, int Count)
			{
				for(int i=0; i<Count; i++)
				{
					if(Player.BoardPosition==i_Fields.Count-1)
					{
						Player.BoardPosition=0; //Los
					}
					else Player.BoardPosition+=1;
				}
			}
		}
	}
}
