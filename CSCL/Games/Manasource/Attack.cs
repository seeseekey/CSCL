using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CSCL.Games.Manasource
{
	public class Attack
	{
		public int ID { get; private set; }
		public int Priority { get; private set; }
		public string Type { get; private set; }
		public int PreDelay { get; private set; }
		public int AftDelay { get; private set; }
		public int DamageFactor { get; private set; }
		public int Range { get; private set; }
		public string Animation { get; private set; }
		public string ParticleEffect { get; private set; }
		public string Action { get; private set; }
		public Elements Element { get; private set; }

		public Attack(XmlNode node)
		{
			foreach(XmlAttribute i in node.Attributes)
			{
				switch(i.Name.ToLower())
				{
					case "id":
						{
							ID=Convert.ToInt32(i.Value);
							break;
						}
					case "priority":
						{
							Priority=Convert.ToInt32(i.Value);
							break;
						}
					case "type":
						{
							//TODO: Sollte in Objekt(bzw enum) umgebaut werden
							Type=i.Value.ToString();
							break;
						}
					case "pre-delay":
						{
							PreDelay=Convert.ToInt32(i.Value);
							break;
						}
					case "aft-delay":
						{
							AftDelay=Convert.ToInt32(i.Value);
							break;
						}
					case "damage-factor":
						{
							DamageFactor=Convert.ToInt32(i.Value);
							break;
						}
					case "range":
						{
							Range=Convert.ToInt32(i.Value);
							break;
						}
					case "animation":
						{
							//TODO: Sollte in Objekt(bzw enum) umgebaut werden
							Animation=i.Value.ToString();
							break;
						}
					case "particle-effect":
						{
							ParticleEffect=i.Value.ToString();
							break;
						}
					case "action":
						{
							Action=i.Value.ToString();
							break;
						}
					case "element":
						{
							Element=Enums.GetElementFromString(i.Value.ToString());
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
