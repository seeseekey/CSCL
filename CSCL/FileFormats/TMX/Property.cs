//
//  Property.cs
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
	public class Property
	{
		public string Name { get; private set; }
		public string Value { get; set; }

		public Property(string name, string value)
		{
			Name=name;
			Value=value;
		}

		public Property(XmlNode node)
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
					case "value":
						{
							Value=i.Value.ToString();
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
