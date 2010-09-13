using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CSCL.Games.Manasource
{
	public class Include //: IComparable
	{
		//public List<Imageset> Imagesets { get; private set; }
		//public List<Action> Actions { get; private set; }
		public string File { get; private set; }
		//public string Action { get; private set; }

		public Include(XmlNode node)
		{
			//Actions=new List<Action>();
			//Imagesets=new List<Imageset>();

			foreach(XmlAttribute i in node.Attributes)
			{
				switch(i.Name.ToLower())
				{
					case "file":
						{
							File=i.Value;
							break;
						}
					//case "action":
					//    {
					//        Action=i.Value;
					//        break;
					//    }
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
					//case "action":
					//    {
					//        Actions.Add(new Action(i));
					//        break;
					//    }
					//case "imageset":
					//    {
					//        Imagesets.Add(new Imageset(i));
					//        break;
					//    }
					default:
						{
							throw new NotImplementedException();
						}
				}
			}
		}
	}
}
