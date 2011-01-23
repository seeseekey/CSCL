using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CSCL.Games.Manasource
{
	public class Npc //: IComparable
	{
		#region Statische Funktionen
		public static List<Npc> GetNpcsFromXml(string filename)
		{
			List<Npc> ret=new List<Npc>();
			XmlData npcfile=new XmlData(filename);
			List<XmlNode> npcs=npcfile.GetElements("npcs.npc");

			foreach(XmlNode i in npcs)
			{
				ret.Add(new Npc(i));
			}

			return ret;
		}
		#endregion

		public int ID { get; private set; }
		public string SpriteFilename { get; private set; }
		public string ParticleFxFilename { get; private set; }

		public Npc(XmlNode node)
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
							SpriteFilename=i.Value;
							break;
						}
					case "particlefx":
						{
							ParticleFxFilename=i.Value;
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
