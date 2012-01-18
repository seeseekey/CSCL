//
//  Animation.cs
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
	public class Animation //: IComparable
	{
		public List<Sequence> Sequences { get; private set; }
		public List<Frame> Frames { get; private set; }
		public string Direction { get; private set; }
		public string Stand { get; private set; }

		public Animation(XmlNode node)
		{
			CultureInfo nc=new CultureInfo("");
			CultureInfo oldCult=Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture=nc;
			
			Sequences=new List<Sequence>();
			Frames=new List<Frame>();

			foreach(XmlAttribute i in node.Attributes)
			{
				switch(i.Name.ToLower())
				{
					case "direction":
						{
							//TODO In Enum überführen
							Direction=i.Value.ToString();
							break;
						}
					case "stand":
						{
							//TODO In Enum überführen
							Stand=i.Value.ToString();
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
					case "sequence":
						{
							Sequences.Add(new Sequence(i));
							break;
						}
					case "frame":
						{
							Frames.Add(new Frame(i));
							break;
						}
					case "end":
						{
							//TODO wird zur zeit ignoriert, sollte geschaut werden ob der Tag Parameter enthält
							break;
						}
					case "#comment": //wird ignoriert
					case "#text":
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
