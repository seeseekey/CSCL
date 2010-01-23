using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CSCL.Games.Manasource
{
	public class Attributes
	{
		public int HP { get; private set; }
		public double Size { get; private set; }
		public double Speed { get; private set; }
		public int AttackMin { get; private set; }
		public int AttackDelta { get; private set; }
		public int AttackMagic { get; private set; }
		public int Hit { get; private set; }
		public int Evade { get; private set; }
		public int PhysicalDefence { get; private set; }
		public int MagicalDefence { get; private set; }
		public int Mutation { get; private set; }

		public Attributes(XmlNode node)
		{
			foreach(XmlAttribute i in node.Attributes)
			{
				switch(i.Name.ToLower())
				{
					case "hp":
						{
							HP=Convert.ToInt32(i.Value);
							break;
						}
					case "size":
						{
							Size=Convert.ToDouble(i.Value);
							break;
						}
					case "speed":
						{
							Speed=Convert.ToDouble(i.Value);
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
					case "attack-magic":
						{
							AttackMagic=Convert.ToInt32(i.Value);
							break;
						}
					case "hit":
						{
							Hit=Convert.ToInt32(i.Value);
							break;
						}
					case "evade":
						{
							Evade=Convert.ToInt32(i.Value);
							break;
						}
					case "physical-defence":
						{
							PhysicalDefence=Convert.ToInt32(i.Value);
							break;
						}
					case "magical-defence":
						{
							MagicalDefence=Convert.ToInt32(i.Value);
							break;
						}
					case "mutation":
						{
							Mutation=Convert.ToInt32(i.Value);
							break;
						}
					default:
						{
							throw new NotImplementedException();
						}
				}
			}
		}
	}
}
