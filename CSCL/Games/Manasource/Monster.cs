﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

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
		public string TargetCursor { get; private set; }
		public string Sprite { get; private set; }
		public List<Sound> Sounds { get; private set; }
		public Attributes Attributes { get; private set; }
		public Behavior Behavior { get; private set; }
		public int Exp { get; private set; }
		public List<Attack> Attacks { get; private set; }
		public List<Drop> Drops { get; private set; }
		public List<Vulnerability> Vulnerabilities { get; private set; }

		public int FightingStrength
		{
			get
			{
				return Attributes.HP*(Attributes.AttackMin+(Attributes.AttackMin+Attributes.AttackDelta));
			}
		}

		public Monster(XmlNode node)
		{
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
							//TODO: In Objekt überführen
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
	}
}
