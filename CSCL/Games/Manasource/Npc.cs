//
//  Npc.cs
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
	public class Npc //: IComparable
	{
		#region Statische Funktionen
		public static List<Npc> GetNpcsFromXml(string filename)
		{
			List<Npc> ret=new List<Npc>();
			XmlData npcfile=new XmlData(filename);
			List<XmlNode> npcs=npcfile.GetElements("npcs.npc");

			foreach(XmlNode i in npcs)
			{
				ret.Add(new Npc(i));
			}

			return ret;
		}
		#endregion

		public int ID { get; private set; }
		public string SpriteFilename { get; private set; }
		public string ParticleFxFilename { get; private set; }

		public Npc(XmlNode node)
		{
			CultureInfo nc=new CultureInfo("");
			CultureInfo oldCult=Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture=nc;
			
			foreach(XmlAttribute i in node.Attributes)
			{
				switch(i.Name.ToLower())
				{
					case "id":
						{
							ID=Convert.ToInt32(i.Value);
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
					case "sprite":
						{
							SpriteFilename=i.ChildNodes[0].Value;
							break;
						}
					case "particlefx":
						{
							ParticleFxFilename=i.ChildNodes[0].Value;
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
