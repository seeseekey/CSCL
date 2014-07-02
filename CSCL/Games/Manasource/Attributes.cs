//
//  Attributes.cs
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
	public class Attributes
	{
		public int HP { get; private set; }
		public double Size { get; private set; }
		public double Speed { get; private set; }
		public int AttackMin { get; private set; }
		public int AttackDelta { get; private set; }
		public int AttackMagic { get; private set; }
		public int Hit { get; private set; }
		public int Evade { get; private set; }
		public int PhysicalDefence { get; private set; }
		public int MagicalDefence { get; private set; }
		public int Mutation { get; private set; }

		public Attributes(XmlNode node)
		{
			CultureInfo nc=new CultureInfo("");
			CultureInfo oldCult=Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture=nc;

			foreach(XmlAttribute i in node.Attributes)
			{
				switch(i.Name.ToLower())
				{
					case "hp":
						{
							HP=Convert.ToInt32(i.Value);
							break;
						}
					case "size":
						{
							Size=Convert.ToDouble(i.Value);
							break;
						}
					case "speed":
						{
							Speed=Convert.ToDouble(i.Value);
							break;
						}
					case "attack-min":
						{
							AttackMin=Convert.ToInt32(i.Value);
							break;
						}
					case "attack-delta":
						{
							AttackDelta=Convert.ToInt32(i.Value);
							break;
						}
					case "attack-magic":
						{
							AttackMagic=Convert.ToInt32(i.Value);
							break;
						}
					case "hit":
						{
							Hit=Convert.ToInt32(i.Value);
							break;
						}
					case "evade":
						{
							Evade=Convert.ToInt32(i.Value);
							break;
						}
					case "physical-defence":
						{
							PhysicalDefence=Convert.ToInt32(i.Value);
							break;
						}
					case "magical-defence":
						{
							MagicalDefence=Convert.ToInt32(i.Value);
							break;
						}
					case "mutation":
						{
							Mutation=Convert.ToInt32(i.Value);
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
