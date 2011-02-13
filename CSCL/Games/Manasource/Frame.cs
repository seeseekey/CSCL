using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Globalization;
using System.Threading;

namespace CSCL.Games.Manasource
{
	public class Frame //: IComparable
	{
		//public List<Animation> Animations { get; private set; }
		public int Index { get; private set; }
		public int OffsetY { get; private set; }
		public int OffsetX { get; private set; }
		public int Delay { get; private set; }

		public Frame(XmlNode node)
		{
			CultureInfo nc=new CultureInfo("");
			CultureInfo oldCult=Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture=nc;
			
			foreach(XmlAttribute i in node.Attributes)
			{
				switch(i.Name.ToLower())
				{
					case "index":
						{
							Index=Convert.ToInt32(i.Value);
							break;
						}
					case "offsety":
						{
							OffsetY=Convert.ToInt32(i.Value);
							break;
						}
					case "offsetx":
						{
							OffsetX=Convert.ToInt32(i.Value);
							break;
						}
					case "delay":
						{
							Delay=Convert.ToInt32(i.Value);
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
