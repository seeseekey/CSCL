﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CSCL.Games.Manasource
{
	public class Imageset
	{
		public string Name { get; private set; }
		public string Src { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }

		public Imageset(XmlNode node)
		{
			foreach(XmlAttribute i in node.Attributes)
			{
				switch(i.Name.ToLower())
				{
					case "name":
						{
							Name=i.Value.ToString();
							break;
						}
					case "src":
						{
							Src=i.Value.ToString();
							break;
						}
					case "width":
						{
							Width=Convert.ToInt32(i.Value);
							break;
						}
					case "height":
						{
							Height=Convert.ToInt32(i.Value);
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