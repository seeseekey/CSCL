using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Globalization;
using System.Threading;
using CSCL.Exceptions;

namespace CSCL.Games.Manasource
{
	public class Monster : IComparable
	{
		#region Statische Funktionen
		public static List<Monster> GetMonstersFromMonsterXml(string filename)
		{
			List<Monster> ret=new List<Monster>();

			XmlData monsterFile=new XmlData(filename);
			List<XmlNode> monsters=monsterFile.GetElements("monsters.monster");

			foreach(XmlNode i in monsters)
			{
				ret.Add(new Monster(i));
			}

			return ret;
		}
		#endregion

		public int ID { get; private set; }
		public string Name { get; private set; }
		public string Script { get; private set; }
		public string TargetCursor { get; private set; }
		//public string SpriteFilename { get; private set; }
		public string Sprite { get; private set; }
		public List<Sound> Sounds { get; private set; }
		public Attributes Attributes { get; private set; }
		public Behavior Behavior { get; private set; }
		public int Exp { get; private set; }
		public List<Attack> Attacks { get; private set; }
		public List<Drop> Drops { get; private set; }
		public List<Vulnerability> Vulnerabilities { get; private set; }

		public Int64 FightingStrength
		{
			get
			{
				return (Int64)Attributes.HP*(Attributes.AttackMin+(Attributes.AttackMin+Attributes.AttackDelta));
			}
		}

		public double GetSaleDropMoneyValue(List<Item> items)
		{
			double ret=0;

			foreach(Drop drop in Drops)
			{
				Item dropitem=null;

				foreach(Item i in items)
				{
					if(drop.Item==i.ID)
					{
						dropitem=i;
						break;
					}
				}

				if(dropitem==null) throw new ItemNotFoundException();

				ret+=dropitem.Value*(drop.Percent/100.0);
			}

			return ret;
		}

		public Monster(XmlNode node)
		{
			CultureInfo nc=new CultureInfo("");
			CultureInfo oldCult=Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture=nc;
			
			Sounds=new List<Sound>();
			Attacks=new List<Attack>();
			Drops=new List<Drop>();
			Vulnerabilities=new List<Vulnerability>();

			foreach(XmlAttribute i in node.Attributes)
			{
				switch(i.Name.ToLower())
				{
					case "id":
						{
							ID=Convert.ToInt32(i.Value);
							break;
						}
					case "name":
						{
							Name=i.Value.ToString();
							break;
						}
					case "targetcursor":
						{
							//TODO In Objekt überführen
							TargetCursor=i.Value.ToString();
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
							//SpriteFilename=i.InnerText;
							Sprite=i.InnerText;
							break;
						}
					case "sound":
						{
							Sounds.Add(new Sound(i));
							break;
						}
					case "drop":
						{
							Drops.Add(new Drop(i));
							break;
						}
					case "attack":
						{
							Attacks.Add(new Attack(i));
							break;
						}
					case "attributes":
						{
							Attributes=new Attributes(i);
							break;
						}
					case "behavior":
						{
							Behavior=new Behavior(i);
							break;
						}
					case "exp":
						{
							Exp=Convert.ToInt32(i.InnerText);
							break;
						}
					case "vulnerability":
						{
							Vulnerabilities.Add(new Vulnerability(i));
							break;
						}
					case "#comment":
						{
							//Diese Tags werden ignoriert
							break;
						}
					case "script":
						{
							//SpriteFilename=i.InnerText;
							Script=i.InnerText;
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
			Monster tmp=(Monster)obj;
			if(tmp.FightingStrength<this.FightingStrength)
			{
				return 1;
			}
			else if(tmp.FightingStrength==this.FightingStrength)
			{
				return 0;
			}
			else
			{
				return -1;
			}
		}
		#endregion

		public string ToMediaWikiInfobox(List<Item> items)
		{
			string agressive="nicht definiert";

			if(Behavior!=null)
			{
				if(Behavior.Aggressive) agressive="Ja";
				else agressive="Nein";
			}

			string ret=String.Format("{{{{Infobox Monster");
			ret+=String.Format("| image = Monster-{0}.png", ID);
			ret+=String.Format("| name  = {0}", Name);
			ret+=String.Format("| id = {0}", ID);
			ret+=String.Format("| exp = {0}", Exp);
			ret+=String.Format("| hp = {0}", Attributes.HP);
			ret+=String.Format("| fighting-strength = {0}", FightingStrength);

			ret+=String.Format("| aggressive = {0}", agressive);

			if(Attributes.AttackDelta==0)
			{
				ret+=String.Format("| attack = {0}", Attributes.AttackMin);
			}
			else
			{
				ret+=String.Format("| attack = {0}-{1}", Attributes.AttackMin, Attributes.AttackMin+Attributes.AttackDelta);
			}

			ret+=String.Format("| defense-physical = {0}", Attributes.PhysicalDefence);
			ret+=String.Format("| defense-magical = {0}", Attributes.MagicalDefence);
			ret+=String.Format("| mutation = {0}", Attributes.Mutation);
			ret+=String.Format("| speed = {0} Tiles pro Sekunde", Attributes.Speed);
			ret+=String.Format("| sale-drop-money-value = {0} Aki", GetSaleDropMoneyValue(items));
			ret+=String.Format("}}}}");

			return ret;
		}
	}
}
