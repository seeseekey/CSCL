using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CSCL.Games.Manasource
{
	public class Drop
	{
		public int Item { get; private set; }
		public double Percent { get; private set; }

		public Drop(XmlNode node)
		{
			foreach(XmlAttribute i in node.Attributes)
			{
				switch(i.Name.ToLower())
				{
					case "item":
						{
							Item=Convert.ToInt32(i.Value);
							break;
						}
					case "percent":
						{
							Percent=Convert.ToDouble(i.Value);
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
