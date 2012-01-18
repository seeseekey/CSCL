//
//  Sound.cs
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
using System.Threading;
using System.Globalization;

namespace CSCL.Games.Manasource
{
	public class Sound
	{
		public string Event { get; private set; }
		public string Filename { get; private set; }

		public Sound(XmlNode node)
		{
			CultureInfo nc=new CultureInfo("");
			CultureInfo oldCult=Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture=nc;
			
			foreach(XmlAttribute i in node.Attributes)
			{
				switch(i.Name.ToLower())
				{
					case "event":
						{
							//TODO: In Objekt umbauen
							Event=i.Value.ToString();
							break;
						}
					default:
						{
							throw new NotImplementedException();
						}
				}
			}

			Filename=node.InnerText;
			
			Thread.CurrentThread.CurrentCulture=oldCult;
		}
	}
}
