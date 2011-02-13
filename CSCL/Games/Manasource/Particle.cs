using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Globalization;
using System.Threading;

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
