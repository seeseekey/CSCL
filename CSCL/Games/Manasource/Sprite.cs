using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CSCL.Games.Manasource
{
	public class Sprite //: IComparable
	{
		#region Statische Funktionen
		public static Sprite GetSpriteFromXml(string filename)
		{
			List<Monster> ret=new List<Monster>();

			XmlData monsterFile=new XmlData(filename);
			List<XmlNode> sprite=monsterFile.GetElements("sprite");
			return new Sprite(sprite[0]);	
		}
		#endregion

		public List<Imageset> Imagesets { get; private set; }
		public List<Action> Actions { get; private set; }

		public Sprite(XmlNode node)
		{
			Actions=new List<Action>();
			Imagesets=new List<Imageset>();

			foreach(XmlAttribute i in node.Attributes)
			{
				switch(i.Name.ToLower())
				{
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
					case "action":
						{
							Actions.Add(new Action(i));
							break;
						}
					case "imageset":
						{
							Imagesets.Add(new Imageset(i));
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
