//
//  Map.cs
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
	public class Map: IComparable
	{
		#region Statische Funktionen
		public static List<Map> GetMapsFromMapsXml(string filename)
		{
			List<Map> ret=new List<Map>();

			XmlData mapsFile=new XmlData(filename);
			List<XmlNode> maps=mapsFile.GetElements("maps.map");

			foreach(XmlNode i in maps)
			{
				ret.Add(new Map(i));
			}

			return ret;
		}

		public static string GetOuterWorldMapFilenameWithoutExtension(int x, int y, int z)
		{
			string vX="";
			if(x==0) vX="o";
			else if(x<0) vX="n";
			else if(x>0) vX="p";

			string vY="";
			if(y==0) vY="o";
			else if(y<0) vY="n";
			else if(y>0) vY="p";

			string vZ="";
			if(z==0) vZ="o";
			else if(z<0) vZ="n";
			else if(z>0) vZ="p";

			return String.Format("ow-{0:0000}{1:0000}-{2}{3:0000}-{4}{5:0000}", vX, Math.Abs(x), vY, Math.Abs(y), vZ, Math.Abs(z));
		}

		public static void SaveToMapsXml(string filename, List<Map> maps)
		{
			XmlData xml=new XmlData(filename, true);

			maps.Sort();

			XmlNode mapsnode=xml.AddRoot("maps");

			foreach(Map i in maps)
			{
				XmlNode mapNode=xml.AddElement(mapsnode, "map");
				xml.AddAttribute(mapNode, "id", i.ID);
				xml.AddAttribute(mapNode, "name", i.Name);
			}

			xml.Save();
		}

		public static Map GetMapFromName(List<Map> maps, string name)
		{
			foreach(Map i in maps)
			{
				if(i.Name.ToLower()==name.ToLower()) return i;
			}

			throw new Exception("No map found!");
		}

		public static string IncreaseArcofMap(string name, XYZ axis)
		{
			char[] splitChars= { '-' };
			string[] Splited=name.Split(splitChars);

			if(Splited[0]!="ow") throw new NotImplementedException();

			int x=GetCoord(Splited[1]);
			int y=GetCoord(Splited[2]);
			int z=GetCoord(Splited[3]);

			switch(axis)
			{
				case XYZ.X:
					{
						x++;
						break;
					}
				case XYZ.Y:
					{
						y++;
						break;
					}
				case XYZ.Z:
					{
						z++;
						break;
					}
			}

			return GetOuterWorldMapFilenameWithoutExtension(x, y, z);
		}

		public static string DecreaseArcofMap(string name, XYZ axis)
		{
			char[] splitChars= { '-' };
			string[] Splited=name.Split(splitChars);

			if(Splited[0]!="ow") throw new NotImplementedException();

			int x=GetCoord(Splited[1]);
			int y=GetCoord(Splited[2]);
			int z=GetCoord(Splited[3]);

			switch(axis)
			{
				case XYZ.X:
					{
						x--;
						break;
					}
				case XYZ.Y:
					{
						y--;
						break;
					}
				case XYZ.Z:
					{
						z--;
						break;
					}
			}

			return GetOuterWorldMapFilenameWithoutExtension(x, y, z);
		}
		#endregion

		#region Hilfsfunktionen
		private static int GetCoord(string coord)
		{
			string replace=coord.Replace("n", "-");
			replace=replace.Replace("p", "");
			replace=replace.Replace("o", "");

			return Convert.ToInt32(replace);
		}
		#endregion

		public int X
		{
			get
			{
				char[] splitChars= { '-' };
				string[] Splited=Name.Split(splitChars);

				if(Splited[0]!="ow") throw new NotImplementedException();

				return GetCoord(Splited[1]);
			}
		}

		public int Y
		{
			get
			{
				char[] splitChars= { '-' };
				string[] Splited=Name.Split(splitChars);

				if(Splited[0]!="ow") throw new NotImplementedException();

				return GetCoord(Splited[2]);
			}
		}

		public int Z
		{
			get
			{
				char[] splitChars= { '-' };
				string[] Splited=Name.Split(splitChars);

				if(Splited[0]!="ow") throw new NotImplementedException();

				return GetCoord(Splited[3]);
			}
		}

		public string MapType
		{
			get
			{
				char[] splitChars= { '-' };
				string[] Splited=Name.Split(splitChars);

				return Splited[0];
			}
		}

		public int ID { get; private set; }
		public string Name { get; private set; }

		public Map(int id, string name)
		{
			ID=id;
			Name=name;
		}

		public Map(XmlNode node)
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
					case "name":
						{
							Name=i.Value.ToString();
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

		#region IComparable Members
		public int CompareTo(object obj)
		{
			Map tmp=(Map)obj;
			if(tmp.ID<this.ID)
			{
				return 1;
			}
			else if(tmp.ID==this.ID)
			{
				return 0;
			}
			else
			{
				return -1;
			}
		}
		#endregion
	}
}
