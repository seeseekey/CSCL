using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Games.Manasource
{
	public class TilesetInfo
	{
		public bool Animated { get; private set; }
		public int TileWidth { get; private set; }
		public int TileHeight { get; private set; }

		public TilesetInfo(bool animated, int tileWidth, int tileHeight)
		{
			Animated=animated;
			TileWidth=tileWidth;
			TileHeight=tileHeight;
		}
	}
}
