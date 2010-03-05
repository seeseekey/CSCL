using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

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
				XmlNode mapNode=xml.AddElement(mapsnode, "map", "");
				xml.AddAttribute(mapNode, "id", i.ID);
				xml.AddAttribute(mapNode, "name", i.Name);
			}

			xml.Save();
		}
		#endregion

		#region Hilfsfunktionen
		private int GetCoord(string coord)
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
