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
	}
}
