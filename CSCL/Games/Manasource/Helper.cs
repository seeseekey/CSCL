using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Games.Manasource
{
	public class Helper
	{
		/// <summary>
		/// Gibt die Pixelkooridnate eines Tiles zurück
		/// </summary>
		/// <param name="tileCoord"></param>
		/// <returns></returns>
		public static int GetPixelCoord(int tileCoord)
		{
			return tileCoord*32;
		}

        /// <summary>
        /// Gibt die Tilekooridnate eines Pixels zurück
        /// </summary>
        /// <param name="pixelCoord"></param>
        /// <returns></returns>
        public static int GetTileCoord(int pixelCoord)
        {
            double ret=pixelCoord/32;
            return (int)System.Math.Round(ret);
        }

		/// <summary>
		/// Gibt die Valide Tileset Höhe zurück
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
