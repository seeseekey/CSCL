using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CSCL.Games.Manasource
{
	public class Action //: IComparable
	{
		public List<Animation> Animations { get; private set; }
		public string Name { get; private set; }
		public string Imageset { get; private set; }

		public Action(XmlNode node)
		{
			CultureInfo nc=new CultureInfo("");
			CultureInfo oldCult=Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture=nc;
			
			Animations=new List<Animation>();

			foreach(XmlAttribute i in node.Attributes)
			{
				switch(i.Name.ToLower())
				{
					case "name":
						{
							Name=i.Value.ToString();
							break;
						}
					case "imageset":
						{
							Imageset=i.Value.ToString();
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
					case "animation":
						{
							Animations.Add(new Animation(i));
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
	}
}
