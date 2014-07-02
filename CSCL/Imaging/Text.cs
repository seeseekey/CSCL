//
//  Text.cs
//
//  Copyright (c) 2011, 2012 by seeseekey <seeseekey@gmail.com>
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
using System.Drawing;
using System.Drawing.Text;

namespace CSCL.Imaging
{
	public partial class Graphic
	{
		public static Graphic RenderText(Font font, string text, Color color)
		{
			//Bilddimensionen ermitteln
			Bitmap bmp=new Bitmap(1, 1);
			Graphics _g=Graphics.FromImage(bmp);
			SizeF textSize=_g.MeasureString(text, font);
			// This is where the bitmap size is determined.
			int _imageWidth=Convert.ToInt32(textSize.Width)+1;
			int _imageHeight=Convert.ToInt32(textSize.Height)+1;
			_g=null;
			bmp=null;

			//Bitmap und Graphics Initialisieren
			bmp=new Bitmap(_imageWidth, _imageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			_g=Graphics.FromImage(bmp);
			_g.Clear(Color.Transparent);

			//Einstellungen
			_g.CompositingQuality=System.Drawing.Drawing2D.CompositingQuality.AssumeLinear;
			_g.InterpolationMode=System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			_g.TextRenderingHint=System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
			_g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			//Text zeichnen
			_g.DrawString(text, font, new SolidBrush(color), new Point(0, 0));

			return Graphic.FromBitmap(bmp);
		}

		public static Graphic RenderText(string filename, int size, string text, Color color)
		{
			PrivateFontCollection pfc = new PrivateFontCollection();
			pfc.AddFontFile(filename);
			FontFamily fontfam=pfc.Families[0];

			Font font=new Font(fontfam, size);

			return RenderText(font, text, color);
		}
	}
}
