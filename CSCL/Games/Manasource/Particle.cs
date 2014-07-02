//
//  Particle.cs
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
