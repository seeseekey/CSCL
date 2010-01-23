using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CSCL.Games.Manasource
{
	public class Behavior
	{
		public bool Aggressive { get; private set; }
		public bool Cowardly { get; private set; }
		public int TrackRange { get; private set; }
		public int StrollRange { get; private set; }
		public int AttackDistance { get; private set; }

		public Behavior(XmlNode node)
		{
			foreach(XmlAttribute i in node.Attributes)
			{
				switch(i.Name.ToLower())
				{
					case "aggressive":
						{
							Aggressive=Convert.ToBoolean(i.Value);
							break;
						}
					case "cowardly":
						{
							Cowardly=Convert.ToBoolean(i.Value);
							break;
						}
					case "track-range":
						{
							TrackRange=Convert.ToInt32(i.Value);
							break;
						}
					case "stroll-range":
						{
							StrollRange=Convert.ToInt32(i.Value);
							break;
						}
					case "attack-distance":
						{
							AttackDistance=Convert.ToInt32(i.Value);
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
