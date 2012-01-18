//
//  Sprite.cs
//
//  Copyright (c) 2011, 2012 by seeseekey <seeseekey@googlemail.com>
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
	public class Sprite //: IComparable
	{
		#region Statische Funktionen
		public static Sprite GetSpriteFromXml(string filename)
		{
			List<Monster> ret=new List<Monster>();

			XmlData monsterFile=new XmlData(filename);
			List<XmlNode> sprite=monsterFile.GetElements("sprite");
			return new Sprite(sprite[0]);	
		}
		#endregion

		public List<Imageset> Imagesets { get; private set; }
		public List<Action> Actions { get; private set; }
		public List<Include> Includes { get; private set; }
		public string Name { get; private set; }
		public string Action { get; private set; }
		public int Variants { get; private set; }
		public int VariantOffset { get; private set; }

		public Sprite(XmlNode node)
		{
			CultureInfo nc=new CultureInfo("");
			CultureInfo oldCult=Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture=nc;
			
			Actions=new List<Action>();
			Imagesets=new List<Imageset>();
			Includes=new List<Include>();

			foreach(XmlAttribute i in node.Attributes)
			{
				switch(i.Name.ToLower())
				{
					case "name":
						{
							Name=i.Value;
							break;
						}
					case "action":
						{
							Action=i.Value;
							break;
						}
					case "variants":
						{
							Variants=Convert.ToInt32(i.Value);
							break;
						}
					case "variant_offset":
						{
							VariantOffset=Convert.ToInt32(i.Value);
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
					case "action":
						{
							Actions.Add(new Action(i));
							break;
						}
					case "imageset":
						{
							Imagesets.Add(new Imageset(i));
							break;
						}
					case "include":
						{
							Includes.Add(new Include(i));
							break;
						}
					case "#text": //wird ignoriert
						{
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
