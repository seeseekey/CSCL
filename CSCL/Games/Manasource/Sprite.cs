using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

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
