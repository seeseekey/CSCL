//
//  Drop.cs
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
using System.Threading;
using System.Globalization;

namespace CSCL.Games.Manasource
{
	public class Drop
	{
		public int Item { get; private set; }
		public double Percent { get; private set; }

		public Drop(XmlNode node)
		{
			CultureInfo nc=new CultureInfo("");
			CultureInfo oldCult=Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture=nc;
			
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
			
			Thread.CurrentThread.CurrentCulture=oldCult;
		}
	}
}
