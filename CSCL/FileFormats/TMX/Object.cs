//
//  Object.cs
//
//  Copyright (c) 2011, 2012 by seeseekey <seeseekey@gmail.com>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CSCL.FileFormats.TMX
{
	public class Object
	{
		public List<Property> Properties { get; private set; }
		public string Name { get; private set; }
		public string Type { get; private set; }
		public int X { get; set; }
		public int Y { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }

		public Object(string name, string type, int width, int height, int x, int y)
		{
			Properties=new List<Property>();

			Name=name;
			Type=type;
			Width=width;
			Height=height;
			X=x;
			Y=y;
		}

		public Object(XmlNode node)
		{
			Properties=new List<Property>();

			foreach(XmlAttribute i in node.Attributes)
			{
				switch(i.Name.ToLower())
				{
					case "name":
						{
							Name=i.Value.ToString();
							break;
						}
					case "type":
						{
							Type=i.Value.ToString();
							break;
						}
					case "x":
						{
							X=Convert.ToInt32(i.Value);
							break;
						}
					case "y":
						{
							Y=Convert.ToInt32(i.Value);
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
							int debug=555;
							break;
						}
				}
			}

			foreach(XmlNode i in node.ChildNodes)
			{
				switch(i.Name.ToLower())
				{
					case "properties":
						{
							foreach(XmlNode j in i.ChildNodes)
							{
								Properties.Add(new Property(j));
							}
							break;
						}
					default:
						{
							int debug=555;
							break;
						}
				}
			}
		}
	}
}
