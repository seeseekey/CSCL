//
//  TilesetInfo.cs
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
