using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

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
		public string Image { get; private set; }
		public string Effect { get; private set; }
		public string Description { get; private set; }
		public int Defense { get; private set; }
		public int HP { get; private set; }
		public string Script { get; private set; }
		public string MissileParticle { get; private set; }
		public int View { get; private set; }

		public Item(XmlNode node)
		{
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
	}
}
