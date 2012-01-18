//
//  Helper.cs
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
	public class Helper
	{
		/// <summary>
		/// Gibt die Pixelkooridnate eines Tiles zur�ck
		/// </summary>
		/// <param name="tileCoord"></param>
		/// <returns></returns>
		public static int GetPixelCoord(int tileCoord)
		{
			return tileCoord*32;
		}

        /// <summary>
        /// Gibt die Tilekooridnate eines Pixels zur�ck
        /// </summary>
        /// <param name="pixelCoord"></param>
        /// <returns></returns>
        public static int GetTileCoord(int pixelCoord)
        {
            double ret=pixelCoord/32;
            return (int)System.Math.Round(ret);
        }

		/// <summary>
		/// Gibt die Valide Tileset H�he zur�ck
		/// </summary>
		/// <param name="tileheight"></param>
		/// <returns></returns>
		public static int GetValidTilesetHeight(int tileheight)
		{
			int value=tileheight;
			int tmp=1024/value;
			int height=tmp*value;

			return height;
		}

		public static TilesetInfo GetTilesetInfo(string filename)
		{
			string fn=FileSystem.GetFilenameWithoutExt(filename);
			string[] splited=fn.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);

			int height=32;

			try
			{
				height=Convert.ToInt32(splited[splited.Length-1]);
			}
			catch
			{
			}

			int width=32;

			try
			{
				width=Convert.ToInt32(splited[splited.Length-2]);
			}
			catch
			{
			}

			string animated=splited[splited.Length-3];

			if(animated=="ani")
			{
				return new TilesetInfo(true, width, height);
			}
			else
			{
				return new TilesetInfo(false, width, height);
			}
		}
	}
}
