using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CSCL.Games.Manasource
{
	public class Particle //: IComparable
	{
		#region Statische Funktionen
		//public static Particle GetSpriteFromXml(string filename)
		//{
		//    List<Monster> ret=new List<Monster>();

		//    XmlData monsterFile=new XmlData(filename);
		//    List<XmlNode> sprite=monsterFile.GetElements("sprite");
		//    return new Particle(sprite[0]);
		//}
		#endregion

		//public List<Imageset> Imagesets { get; private set; }
		//public List<Action> Actions { get; private set; }
		//public List<Include> Includes { get; private set; }
		//public string Name { get; private set; }
		//public string Action { get; private set; }
		//public int Variants { get; private set; }
		//public int VariantOffset { get; private set; }

		public Particle(XmlNode node)
		{
			CultureInfo nc=new CultureInfo("");
			CultureInfo oldCult=Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture=nc;

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
					default:
						{
							throw new NotImplementedException();
						}
				}
			}
			
			Thread.CurrentThread.CurrentCulture=oldCult;
		}
	}
}
