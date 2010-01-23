using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Text;

namespace CSCL.Graphic
{
	public partial class gtImage
	{
		public static gtImage RenderText(Font font, string text, Color color)
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

			return gtImage.FromBitmap(bmp);
		}

		public static gtImage RenderText(string filename, int size, string text, Color color)
		{
			PrivateFontCollection pfc = new PrivateFontCollection();
			pfc.AddFontFile(filename);
			FontFamily fontfam=pfc.Families[0];

			Font font=new Font(fontfam, size);

			return RenderText(font, text, color);
		}
	}
}
