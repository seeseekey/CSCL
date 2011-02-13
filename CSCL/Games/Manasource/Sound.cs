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
