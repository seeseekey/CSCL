﻿using System;
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
