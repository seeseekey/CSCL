//
//  Item.cs
//
//  Copyright (c) 2011, 2012 by seeseekey <seeseekey@googlemail.com>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Globalization;
using System.Threading;

namespace CSCL.Games.Manasource
{
	public class Item:IComparable
	{
		#region Statische Funktionen
		public static List<Item> GetItemsFromItemsXml(string filename)
		{
			List<Item> ret=new List<Item>();

			XmlData itemsFile=new XmlData(filename);
			List<XmlNode> items=itemsFile.GetElements("items.item");

			foreach(XmlNode i in items)
			{
				ret.Add(new Item(i));
			}

			return ret;
		}
		#endregion

		public int ID { get; private set; }
		public string Name { get; private set; }
		public double Weight { get; private set; }
		public string WeaponType { get; private set; }
		public string Sprite { get; private set; }
		public string Type { get; private set; }
		public List<Sound> Sounds { get; private set; }
		public int AttackMin { get; private set; }
		public int AttackDelta { get; private set; }
		public int MaxPerSlot { get; private set; }
		public int AttackRange { get; private set; }
		public int AttackAngle { get; private set; }
		public string AttackShape { get; private set; }
		public string AttackTarget { get; private set; }
		public string AttackAction { get; private set; }
		public string Image { get; private set; }
		public string Effect { get; private set; }
		public string Description { get; private set; }
		public int Defense { get; private set; }
		public int HP { get; private set; }
		public string Script { get; private set; }
		public string MissileParticle { get; private set; }
		public int View { get; private set; }
		public int Lifetime { get; private set; }
		public int Intelligence { get; private set; }
		public int Value { get; private set; }

		public Item(XmlNode node)
		{
			CultureInfo nc=new CultureInfo("");
			CultureInfo oldCult=Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture=nc;
			
			Sounds=new List<Sound>();

			foreach(XmlAttribute i in node.Attributes)
			{
				switch(i.Name.ToLower())
				{
					case "id":
						{
							ID=Convert.ToInt32(i.Value);
							break;
						}
					case "hp":
						{
							HP=Convert.ToInt32(i.Value);
							break;
						}
					case "defense":
						{
							Defense=Convert.ToInt32(i.Value);
							break;
						}
					case "attack-shape":
						{
							//TODO Objekt bzw enum drauß machen
							AttackShape=i.Value.ToString();
							break;
						}
					case "script":
						{
							//TODO Objekt bzw enum drauß machen
							Script=i.Value.ToString();
							break;
						}
					case "image":
						{
							//TODO Bild einlesen mit gtImage
							Image=i.Value.ToString();
							break;
						}
					case "description":
						{
							Description=i.Value.ToString();
							break;
						}
					case "effect":
						{
							//TODO Objekt bzw Klasse draus machen
							Effect=i.Value.ToString();
							break;
						}
					case "attack-target":
						{
							//TODO Objekt bzw enum drauß machen
							AttackTarget=i.Value.ToString();
							break;
						}
					case "attack-action":
						{
							//TODO Objekt bzw enum drauß machen
							AttackAction=i.Value.ToString();
							break;
						}
					case "name":
						{
							Name=i.Value.ToString();
							break;
						}
					case "max-per-slot":
						{
							MaxPerSlot=Convert.ToInt32(i.Value);
							break;
						}
					case "weapon-type":
						{
							WeaponType=i.Value.ToString();
							break;
						}
					case "weight":
						{
							Weight=Convert.ToDouble(i.Value);
							break;
						}
					case "type":
						{
							//TODO: In Objekt überführen
							Type=i.InnerText;
							break;
						}
					case "attack-min":
						{
							AttackMin=Convert.ToInt32(i.Value);
							break;
						}
					case "attack-delta":
						{
							AttackDelta=Convert.ToInt32(i.Value);
							break;
						}
					case "attack-range":
						{
							AttackRange=Convert.ToInt32(i.Value);
							break;
						}
					case "attack-angle":
						{
							AttackAngle=Convert.ToInt32(i.Value);
							break;
						}
					case "missile-particle":
						{
							MissileParticle=i.Value.ToString();
							break;
						}
					case "view":
						{
							View=Convert.ToInt32(i.Value);
							break;
						}
					case "lifetime":
						{
							Lifetime=Convert.ToInt32(i.Value);
							break;
						}
					case "intelligence":
						{
							Intelligence=Convert.ToInt32(i.Value);
							break;
						}
					case "value":
						{
							Value=Convert.ToInt32(i.Value);
							break;
						}
					default:
						{
							throw new NotImplementedException();
						}
				}
			}

			foreach(XmlNode i in node.ChildNodes)
			{
				switch(i.Name.ToLower())
				{
					case "sprite":
						{
							//TODO: In Objekt überführen
							Sprite=i.InnerText;
							break;
						}
					case "sound":
						{
							Sounds.Add(new Sound(i));
							break;
						}
					default:
						{
							throw new NotImplementedException();
						}
				}
			}
			
			Thread.CurrentThread.CurrentCulture=oldCult;
		}

		#region IComparable Members
		public int CompareTo(object obj)
		{
			Item tmp=(Item)obj;
			if(tmp.ID<this.ID)
			{
				return 1;
			}
			else if(tmp.ID==this.ID)
			{
				return 0;
			}
			else
			{
				return -1;
			}
		}
		#endregion

		public string ToMediaWikiInfobox()
		{
			string ret=String.Format("{{{{Infobox Item");
			ret+=String.Format("| image = Item-{0}.png", ID);
			ret+=String.Format("| name  = {0}", Name);
			ret+=String.Format("| id = {0}", ID);
			ret+=String.Format("| description = {0}", Description);
			ret+=String.Format("| weight = {0}", Weight);
			ret+=String.Format("| effect = {0}", Effect);
			ret+=String.Format("| salevalue = {0} Aki", Value);
			ret+=String.Format("| maxperslot = {0}", MaxPerSlot);
			ret+=String.Format("}}}}");

			return ret;
		}
	}
}
