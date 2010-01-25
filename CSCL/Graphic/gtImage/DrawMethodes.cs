using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace CSCL.Graphic
{
	public partial class gtImage
	{
		/////////////////////////////////////////////////////////////////
		// Zeichenroutinen
		/////////////////////////////////////////////////////////////////
		#region Draw
		public void Draw(int x, int y, gtImage source)
		{
			if(x>=width||y>=height) throw new ArgumentOutOfRangeException("x or y", "Out of image.");
			if(x+source.width<0||y+source.height<0) throw new ArgumentOutOfRangeException("x or y", "Out of image.");

			gtImage srcimg=source.ConvertTo(channelFormat);
			if(srcimg==null) return;

			uint bpp=ConvertToBytePerPixel(channelFormat);

			unsafe
			{
				fixed(byte* src_=srcimg.imageData, dst_=imageData)
				{
                    uint start=(uint)System.Math.Max(-x, 0)*bpp;
                    uint end=(uint)System.Math.Min(source.width, width-x)*bpp;

                    uint jstart=(uint)System.Math.Max(-y, 0);
                    uint jend=(uint)System.Math.Min(source.height, height-y);

					byte* src__=src_+start;
					byte* dst__=dst_+x*bpp+start;

					uint sw=source.width*bpp;
					uint dw=width*bpp;

					for(uint j=jstart; j<jend; j++)
					{
						byte* src=src__+sw*j;
						byte* dst=dst__+dw*(y+j);

						for(uint i=start; i<end; i++) *dst++=*src++;
					}
				}
			}
		}

		public void Draw(int x, int y, gtImage source, bool considerAlpha)
		{
			if(!considerAlpha||source.channelFormat==Format.BGR||
				source.channelFormat==Format.RGB||source.channelFormat==Format.GRAY)
			{
				Draw(x, y, source); return;
			}

			if(x>=width||y>=height) throw new ArgumentOutOfRangeException("x or y", "Out of image.");
			if(x+source.width<0||y+source.height<0) throw new ArgumentOutOfRangeException("x or y", "Out of image.");

			gtImage srcimg=null;

			switch(channelFormat)
			{
				case Format.GRAY: srcimg=source.ConvertToGrayAlpha(); break;
				case Format.GRAY_Alpha: srcimg=source.ConvertToGrayAlpha(); break;
				case Format.RGB: srcimg=source.ConvertToRGBA(); break;
				case Format.RGBA: srcimg=source.ConvertToRGBA(); break;
				case Format.BGR: srcimg=source.ConvertToBGRA(); break;
				case Format.BGRA: srcimg=source.ConvertToBGRA(); break;
			}

			uint bpp=ConvertToBytePerPixel(source.channelFormat);

			unsafe
			{
				fixed(byte* src_=srcimg.imageData, dst_=imageData)
				{
                    uint start=(uint)System.Math.Max(-x, 0);
                    uint end=(uint)System.Math.Min(source.width, width-x);

                    uint jstart=(uint)System.Math.Max(-y, 0);
                    uint jend=(uint)System.Math.Min(source.height, height-y);

					if(channelFormat==Format.BGR||channelFormat==Format.RGB||channelFormat==Format.GRAY)
					{
						uint dbpp=ConvertToBytePerPixel(channelFormat);

						byte* src__=src_+start*bpp;
						byte* dst__=dst_+x*dbpp+start*dbpp;

						uint sw=source.width*bpp;
						uint dw=width*dbpp;

						if(channelFormat==Format.BGR||channelFormat==Format.RGB)
						{
							for(uint j=jstart; j<jend; j++)
							{
								byte* src=src__+sw*j;
								byte* dst=dst__+dw*(y+j);

								for(uint i=start; i<end; i++)
								{
									byte sr=*src++; byte sg=*src++; byte sb=*src++; byte sa=*src++;

									if(sa!=0)
									{
										byte dr=*dst++; byte dg=*dst++; byte db=*dst++;
										dst-=3;

										double a2=sa/255.0;
										double a1=1-a2;
										*dst++=(byte)(dr*a1+sr*a2);
										*dst++=(byte)(dg*a1+sg*a2);
										*dst++=(byte)(db*a1+sb*a2);
									}
									else dst+=3;
								}
							}
						}
						else // GRAY
						{
							for(uint j=jstart; j<jend; j++)
							{
								byte* src=src__+sw*j;
								byte* dst=dst__+dw*(y+j);

								for(uint i=start; i<end; i++)
								{
									byte sg=*src++; byte sa=*src++;

									if(sa!=0)
									{
										byte dg=*dst;

										double a2=sa/255.0;
										double a1=1-a2;
										*dst++=(byte)(dg*a1+sg*a2);
									}
									else dst++;
								}
							}
						} // end if RGB || BGR
					}
					else // 2x Alpha-Bild
					{
						byte* src__=src_+start*bpp;
						byte* dst__=dst_+x*bpp+start*bpp;

						uint sw=source.width*bpp;
						uint dw=width*bpp;

						if(channelFormat==Format.BGRA||channelFormat==Format.RGBA)
						{
							for(uint j=jstart; j<jend; j++)
							{
								byte* src=src__+sw*j;
								byte* dst=dst__+dw*(y+j);

								for(uint i=start; i<end; i++)
								{
									byte sr=*src++; byte sg=*src++; byte sb=*src++; byte sa=*src++;

									if(sa!=0)
									{
										byte dr=*dst++; byte dg=*dst++; byte db=*dst++; byte da=*dst++;
										dst-=4;

										double a2=sa/255.0;
										double a1=1-a2;
										*dst++=(byte)(dr*a1+sr*a2);
										*dst++=(byte)(dg*a1+sg*a2);
										*dst++=(byte)(db*a1+sb*a2);
										*dst++=da;
									}
									else dst+=4;
								}
							}
						}
						else // GRAYALPHA
						{
							for(uint j=jstart; j<jend; j++)
							{
								byte* src=src__+sw*j;
								byte* dst=dst__+dw*(y+j);

								for(uint i=start; i<end; i++)
								{
									byte sg=*src++; byte sa=*src++;

									if(sa!=0)
									{
										byte dg=*dst++; byte da=*dst++;
										dst-=2;

										double a2=sa/255.0;
										double a1=1-a2;
										*dst++=(byte)(dg*a1+sg*a2);
										*dst++=da;
									}
									else dst+=2;
								}
							}
						} // end if RGBA || BGRA
					} // end if 2x Alpha-Bild
				} // fixed
			} // unsafe
		}
		#endregion

		#region Fill
		public void Fill(Color color)
		{
			unsafe
			{
				fixed(byte* dst_=imageData)
				{
					byte* dst=dst_;
					if(channelFormat==Format.GRAY)
					{
						byte g=color.R;
						int len=imageData.Length;
						for(int i=0; i<len; i++) *dst++=g;
					}
					else if(channelFormat==Format.RGB)
					{
						byte r=color.R;
						byte g=color.G;
						byte b=color.B;
						int len=imageData.Length/3;
						for(int i=0; i<len; i++)
						{
							*dst++=b;
							*dst++=g;
							*dst++=r;
						}
					}
					else if(channelFormat==Format.BGR)
					{
						byte r=color.R;
						byte g=color.G;
						byte b=color.B;
						int len=imageData.Length/3;
						for(int i=0; i<len; i++)
						{
							*dst++=r;
							*dst++=g;
							*dst++=b;
						}
					}
					else if(channelFormat==Format.RGBA)
					{
						byte r=color.R;
						byte g=color.G;
						byte b=color.B;
						byte a=color.A;
						int len=imageData.Length/4;
						for(int i=0; i<len; i++)
						{
							*dst++=b;
							*dst++=g;
							*dst++=r;
							*dst++=a;
						}
					}
					else if(channelFormat==Format.BGRA)
					{
						byte r=color.R;
						byte g=color.G;
						byte b=color.B;
						byte a=color.A;
						int len=imageData.Length/4;
						for(int i=0; i<len; i++)
						{
							*dst++=r;
							*dst++=g;
							*dst++=b;
							*dst++=a;
						}
					}
					else if(channelFormat==Format.GRAY_Alpha)
					{
						byte g=color.R;
						byte a=color.A;
						int len=imageData.Length/2;
						for(int i=0; i<len; i++)
						{
							*dst++=g;
							*dst++=a;
						}
					}
				}
			}
		}

		public void Fill(byte r, byte g, byte b)
		{
			Fill(Color.FromArgb(r, g, b));
		}

		public void Fill(byte alpha, byte r, byte g, byte b)
		{
			Fill(Color.FromArgb(alpha, r, g, b));
		}
		#endregion

		#region FillWithRaster
		public void FillWithRaster(Color color1, Color color2, byte rasterSize)
		{
			if(rasterSize==0)
			{
				Fill(color1);
				return;
			}

			unsafe
			{
				fixed(byte* dst_=imageData)
				{
					byte* dst=dst_;
					if(channelFormat==Format.GRAY)
					{
						byte g1=color1.R;
						byte g2=color2.R;
						bool doColor1=true;
						for(uint y=0; y<height; y++)
						{
							if((y%rasterSize)==0) doColor1=!doColor1;
							bool lastDoColor1=doColor1;
							for(int x=0; x<width; x++)
							{
								if((x%rasterSize)==0) doColor1=!doColor1;
								*dst++=(doColor1?g1:g2);
							}
							doColor1=lastDoColor1;
						}
					}
					else if(channelFormat==Format.RGB)
					{
						byte r1=color1.R;
						byte g1=color1.G;
						byte b1=color1.B;
						byte r2=color2.R;
						byte g2=color2.G;
						byte b2=color2.B;
						bool doColor1=true;
						for(uint y=0; y<height; y++)
						{
							if((y%rasterSize)==0) doColor1=!doColor1;
							bool lastDoColor1=doColor1;
							for(int x=0; x<width; x++)
							{
								if((x%rasterSize)==0) doColor1=!doColor1;
								*dst++=(doColor1?b1:b2);
								*dst++=(doColor1?g1:g2);
								*dst++=(doColor1?r1:r2);
							}
							doColor1=lastDoColor1;
						}
					}
					else if(channelFormat==Format.BGR)
					{
						byte r1=color1.R;
						byte g1=color1.G;
						byte b1=color1.B;
						byte r2=color2.R;
						byte g2=color2.G;
						byte b2=color2.B;
						bool doColor1=true;
						for(uint y=0; y<height; y++)
						{
							if((y%rasterSize)==0) doColor1=!doColor1;
							bool lastDoColor1=doColor1;
							for(int x=0; x<width; x++)
							{
								if((x%rasterSize)==0) doColor1=!doColor1;
								*dst++=(doColor1?r1:r2);
								*dst++=(doColor1?g1:g2);
								*dst++=(doColor1?b1:b2);
							}
							doColor1=lastDoColor1;
						}
					}
					else if(channelFormat==Format.RGBA)
					{
						byte r1=color1.R;
						byte g1=color1.G;
						byte b1=color1.B;
						byte a1=color1.A;
						byte r2=color2.R;
						byte g2=color2.G;
						byte b2=color2.B;
						byte a2=color2.A;
						bool doColor1=true;
						for(uint y=0; y<height; y++)
						{
							if((y%rasterSize)==0) doColor1=!doColor1;
							bool lastDoColor1=doColor1;
							for(int x=0; x<width; x++)
							{
								if((x%rasterSize)==0) doColor1=!doColor1;
								*dst++=(doColor1?b1:b2);
								*dst++=(doColor1?g1:g2);
								*dst++=(doColor1?r1:r2);
								*dst++=(doColor1?a1:a2);
							}
							doColor1=lastDoColor1;
						}
					}
					else if(channelFormat==Format.BGRA)
					{
						byte r1=color1.R;
						byte g1=color1.G;
						byte b1=color1.B;
						byte a1=color1.A;
						byte r2=color2.R;
						byte g2=color2.G;
						byte b2=color2.B;
						byte a2=color2.A;
						bool doColor1=true;
						for(uint y=0; y<height; y++)
						{
							if((y%rasterSize)==0) doColor1=!doColor1;
							bool lastDoColor1=doColor1;
							for(int x=0; x<width; x++)
							{
								if((x%rasterSize)==0) doColor1=!doColor1;
								*dst++=(doColor1?r1:r2);
								*dst++=(doColor1?g1:g2);
								*dst++=(doColor1?b1:b2);
								*dst++=(doColor1?a1:a2);
							}
							doColor1=lastDoColor1;
						}
					}
					else if(channelFormat==Format.GRAY_Alpha)
					{
						byte g1=color1.R;
						byte a1=color1.A;
						byte g2=color2.R;
						byte a2=color2.A;
						bool doColor1=true;
						for(uint y=0; y<height; y++)
						{
							if((y%rasterSize)==0) doColor1=!doColor1;
							bool lastDoColor1=doColor1;
							for(int x=0; x<width; x++)
							{
								if((x%rasterSize)==0) doColor1=!doColor1;
								*dst++=(doColor1?g1:g2);
								*dst++=(doColor1?a1:a2);
							}
							doColor1=lastDoColor1;
						}
					}
				}
			}
		}

		public void FillWithRaster(byte r1, byte g1, byte b1, byte r2, byte g2, byte b2, byte rasterSize)
		{
			FillWithRaster(Color.FromArgb(r1, g1, b1), Color.FromArgb(r2, g2, b2), rasterSize);
		}

		public void FillWithRaster(byte alpha1, byte r1, byte g1, byte b1, byte alpha2, byte r2, byte g2, byte b2, byte rasterSize)
		{
			FillWithRaster(Color.FromArgb(alpha1, r1, g1, b1), Color.FromArgb(alpha2, r2, g2, b2), rasterSize);
		}
		#endregion

		#region FillWithMandelbrot
		public void FillWithMandelbrot()
		{
			FillWithMandelbrot(-2.0, -1.6, 1, 1.6, 255);
		}

		public void FillWithMandelbrot(double x1, double y1, double x2, double y2, byte depth)
		{
			//Ausschnitt der Grafik
			//x1, y1, x2, y2
			//Berechnungstiefe
			//depth

			int d; //Laufvariable für Tiefe
			double dx, dy; //Schrittweite pro Pixel
			double px, py; //aktuelle Weltkoordinate
			double u, v; //Berechnungsvariablen
			double ax, ay; //Berechnungsvariablen

			Color[] c=new Color[256];

			//Zufallsfarben erzeugen
			System.Random myRandom=new System.Random();
			for(int i=0; i<256; i++)
			{
				c[i]=Color.FromArgb(myRandom.Next(255), myRandom.Next(255), myRandom.Next(255));
			}

			dx=(x2-x1)/Width;
			dy=(y2-y1)/Height;

			//Erzeugung des Bitmap
			for(uint y=0; y<Height; y++)
			{
				for(uint x=0; x<Width; x++)
				{
					px=x1+x*dx;
					py=y1+y*dy;
					d=0;
					ax=0;
					ay=0;

					do
					{
						u=ax*ax-ay*ay+px;
						v=2*ax*ay+py;
						ax=u;
						ay=v;
						d++;
					}
					while(!(ax*ax+ay*ay>8||d==depth));

					SetPixel(x, y, c[d]);
				}
			}
		}
		#endregion

		#region FillWithTestPattern
		public void FillWithTestPattern()
		{
			Fill(Color.Red);
			CircleFilled((int)(width/2), (int)(height/2), (uint)(width/2-width/32), Color.Green);
			CircleFilled((int)(width/2), (int)(height/2), (uint)(width/2-width/16), Color.Blue);
			CircleFilled((int)(width/2), (int)(height/2), (uint)(width/2-width/8), Color.Yellow);

			Line((int)(width/2-width/8), (int)(height/2-height/8), (int)(width/2+width/8), (int)(height/2-height/8), Color.Red);
			Line((int)(width/2-width/8), (int)(height/2-height/16), (int)(width/2+width/8), (int)(height/2-height/16), Color.Green);
			Line((int)(width/2-width/8), (int)(height/2-height/24), (int)(width/2+width/8), (int)(height/2-height/24), Color.Blue);
			Line((int)(width/2-width/8), (int)(height/2-height/32), (int)(width/2+width/8), (int)(height/2-height/32), Color.Black);
			Line((int)(width/2-width/8), (int)(height/2-height/40), (int)(width/2+width/8), (int)(height/2-height/40), Color.White);
		}
		#endregion

		#region Line, PolyLine & Polygon
		public void Line(int xstart, int ystart, int xend, int yend, Color color)
		{
			//Initialisierung
			int x, y, t, dist, xerr, yerr, dx, dy, incx, incy;

			// Entfernung in beiden Dimensionen berechnen
			dx=xend-xstart;
			dy=yend-ystart;

			// Vorzeichen des Inkrements bestimmen
			if(dx<0)
			{
				incx=-1;
				dx=-dx;
			}
			else if(dx>0) incx=1;
			else incx=0;

			if(dy<0)
			{
				incy=-1;
				dy=-dy;
			}
			else if(dy>0) incy=1;
			else incy=0;

			// feststellen, welche Entfernung größer ist
			dist=(dx>dy)?dx:dy;

			// Initialisierungen vor Schleifenbeginn
			x=xstart;
			y=ystart;
			xerr=dx;
			yerr=dy;

			// Pixel berechnen
			for(t=0; t<dist; ++t)
			{
				SetPixel(x, y, color);

				xerr+=dx;
				yerr+=dy;

				if(xerr>dist)
				{
					xerr-=dist;
					x+=incx;
				}

				if(yerr>dist)
				{
					yerr-=dist;
					y+=incy;
				}
			}

			SetPixel(xend, yend, color);
		}

		public void Line(int xstart, int ystart, int xend, int yend, byte r, byte g, byte b)
		{
			Line(xstart, ystart, xend, yend, Color.FromArgb(r, g, b));
		}

		public void Line(int xstart, int ystart, int xend, int yend, byte alpha, byte r, byte g, byte b)
		{
			Line(xstart, ystart, xend, yend, Color.FromArgb(alpha, r, g, b));
		}

		//public void PolyLine(Tuple2iList points, Color color)
		//{
		//    for(int i=0; i<points.Count-1; i++)
		//    {
		//        Line(points[i].x, points[i].y, points[i+1].x, points[i+1].y, color);
		//    }
		//}

		//public void PolyLine(Tuple2iList points, byte r, byte g, byte b)
		//{
		//    PolyLine(points, Color.FromArgb(r, g, b));
		//}

		//public void PolyLine(Tuple2iList points, byte alpha, byte r, byte g, byte b)
		//{
		//    PolyLine(points, Color.FromArgb(alpha, r, g, b));
		//}

		//public void Polygon(Tuple2iList points, Color color)
		//{
		//    PolyLine(points, color);
		//    Line(points[points.Count-1].x, points[points.Count-1].y, points[0].x, points[0].y, color);
		//}

		//public void Polygon(Tuple2iList points, byte r, byte g, byte b)
		//{
		//    Polygon(points, Color.FromArgb(r, g, b));
		//}

		//public void Polygon(Tuple2iList points, byte alpha, byte r, byte g, byte b)
		//{
		//    Polygon(points, Color.FromArgb(alpha, r, g, b));
		//}
		#endregion

		#region Rect & RectFilled
		public void Rect(int x, int y, uint w, uint h, Color color)
		{
			Line(x, y, x+(int)w, y, color); // von 1 zu 2
			Line(x+(int)w, y, x+(int)w, y+(int)h, color); // von 2 zu 3
			Line(x+(int)w, y+(int)h, x, y+(int)h, color); // von 3 zu 4
			Line(x, y+(int)h, x, y, color); // von 4 zu 1
		}

		public void Rect(int x, int y, uint w, uint h, byte r, byte g, byte b)
		{
			Rect(x, y, w, h, Color.FromArgb(r, g, b));
		}

		public void Rect(int x, int y, uint w, uint h, byte alpha, byte r, byte g, byte b)
		{
			Rect(x, y, w, h, Color.FromArgb(alpha, r, g, b));
		}

		public void RectFilled(int x, int y, uint w, uint h, Color color)
		{
			if(x>=width||y>=height) throw new ArgumentOutOfRangeException("x or y", "Out of image.");

			uint bpp=ConvertToBytePerPixel(channelFormat);

            uint start=(uint)System.Math.Max(-x, 0);
            uint end=(uint)System.Math.Min(w, width-x);

            uint jstart=(uint)System.Math.Max(-y, 0);
            uint jend=(uint)System.Math.Min(h, height-y);

			unsafe
			{
				fixed(byte* dst_=imageData)
				{
					byte* dst__=dst_+x*bpp+start;

					uint dw=width*bpp;

					if(channelFormat==Format.GRAY)
					{
						byte g=color.R;

						for(uint j=jstart; j<jend; j++)
						{
							byte* dst=dst__+dw*(y+j);

							for(uint i=start; i<end; i++) *dst++=g;
						}
					}
					else if(channelFormat==Format.RGB)
					{
						byte r=color.R;
						byte g=color.G;
						byte b=color.B;

						for(uint j=jstart; j<jend; j++)
						{
							byte* dst=dst__+dw*(y+j);

							for(uint i=start; i<end; i++)
							{
								*dst++=b;
								*dst++=g;
								*dst++=r;
							}
						}
					}
					else if(channelFormat==Format.BGR)
					{
						byte r=color.R;
						byte g=color.G;
						byte b=color.B;

						for(uint j=jstart; j<jend; j++)
						{
							byte* dst=dst__+dw*(y+j);

							for(uint i=start; i<end; i++)
							{
								*dst++=r;
								*dst++=g;
								*dst++=b;
							}
						}
					}
					else if(channelFormat==Format.RGBA)
					{
						byte r=color.R;
						byte g=color.G;
						byte b=color.B;
						byte a=color.A;

						for(uint j=jstart; j<jend; j++)
						{
							byte* dst=dst__+dw*(y+j);

							for(uint i=start; i<end; i++)
							{
								*dst++=b;
								*dst++=g;
								*dst++=r;
								*dst++=a;
							}
						}
					}
					else if(channelFormat==Format.BGRA)
					{
						byte r=color.R;
						byte g=color.G;
						byte b=color.B;
						byte a=color.A;

						for(uint j=jstart; j<jend; j++)
						{
							byte* dst=dst__+dw*(y+j);

							for(uint i=start; i<end; i++)
							{
								*dst++=r;
								*dst++=g;
								*dst++=b;
								*dst++=a;
							}
						}
					}
					else if(channelFormat==Format.GRAY_Alpha)
					{
						byte g=color.R;
						byte a=color.A;

						for(uint j=jstart; j<jend; j++)
						{
							byte* dst=dst__+dw*(y+j);

							for(uint i=start; i<end; i++)
							{
								*dst++=g;
								*dst++=a;
							}
						}
					}
				}
			}

			//pImage tmpImage=pImage.GenImage(w, h, pImage.Format.RGBA);
			//tmpImage.SetPixel(0, 0, color);
			//tmpImage.Fill(color);
			//tmpImage=tmpImage.NearestPixelResize(w, h);
			//Draw(x, y, tmpImage);
		}

		public void RectFilled(int x, int y, uint w, uint h, byte r, byte g, byte b)
		{
			RectFilled(x, y, w, h, Color.FromArgb(r, g, b));
		}

		public void RectFilled(int x, int y, uint w, uint h, byte alpha, byte r, byte g, byte b)
		{
			RectFilled(x, y, w, h, Color.FromArgb(alpha, r, g, b));
		}
		#endregion

		#region Circle & CircleFilled
		public void Circle(int x0, int y0, uint radius, Color color)
		{
			int f=1-(int)radius;
			int ddF_x=0;
			int ddF_y=-2*(int)radius;
			int x=0;
			int y=(int)radius;

			SetPixel(x0, y0+(int)radius, color);
			SetPixel(x0, y0-(int)radius, color);
			SetPixel(x0+(int)radius, y0, color);
			SetPixel(x0-(int)radius, y0, color);

			while(x<y)
			{
				if(f>=0)
				{
					y--;
					ddF_y+=2;
					f+=ddF_y;
				}
				x++;
				ddF_x+=2;
				f+=ddF_x+1;

				SetPixel(x0+x, y0+y, color);
				SetPixel(x0-x, y0+y, color);
				SetPixel(x0+x, y0-y, color);
				SetPixel(x0-x, y0-y, color);
				SetPixel(x0+y, y0+x, color);
				SetPixel(x0-y, y0+x, color);
				SetPixel(x0+y, y0-x, color);
				SetPixel(x0-y, y0-x, color);
			}
		}

		public void Circle(int x0, int y0, uint radius, byte r, byte g, byte b)
		{
			Circle(x0, y0, radius, Color.FromArgb(r, g, b));
		}

		public void Circle(int x0, int y0, uint radius, byte alpha, byte r, byte g, byte b)
		{
			Circle(x0, y0, radius, Color.FromArgb(alpha, r, g, b));
		}

		public void CircleFilled(int x0, int y0, uint radius, byte r, byte g, byte b)
		{
			CircleFilled(x0, y0, radius, Color.FromArgb(r, g, b));
		}

		public void CircleFilled(int x0, int y0, uint radius, byte alpha, byte r, byte g, byte b)
		{
			CircleFilled(x0, y0, radius, Color.FromArgb(alpha, r, g, b));
		}

		public void CircleFilled(int x0, int y0, uint radius, Color color)
		{
			for (int i=x0-(int)radius; i<=x0+radius; i++)
			{
				for (int k=y0-(int)radius; k<=y0+radius; k++)
				{
					//Wenn Punkt aushalb des Bilder so wird er nicht gezeichnet
					if (i<1|i>width-1) continue;
					if (k<1|k>height-1) continue;

					//Berechne Distanz
					if (radius>CSCL.Maths.Geometry.Distanz(x0, y0, i, k))
					{
						SetPixel(i, k, color);
					}
				}
			}
		}
		#endregion

		#region Bezier
		//public void Bezier(Tuple2dList p, int iter, Color color)
		//{
		//    int n=p.Count-1;

		//    List<Tuple2dList> q=new List<Tuple2dList>();

		//    q.Add(p);

		//    for(int j=1; j<=n; j++)
		//    {
		//        Tuple2dList q0=q[j-1];
		//        Tuple2dList q1=new Tuple2dList();
		//        for(int i=0; i<=(n-j); i++) q1.Add((q0[i]+q0[i+1])*0.5);
		//        q.Add(q1);
		//    }

		//    if(iter>=0)
		//    {
		//        //Arrays für die Punkte der geteilten Kontrollpolygone C1, C2)
		//        Tuple2dList c1=new Tuple2dList(), c2=new Tuple2dList();

		//        for(int i=0; i<n+1; i++)
		//        {
		//            c1.Add(q[i][0]);
		//            c2.Add(q[n-i][i]);
		//        }

		//        Bezier(c1, --iter, color);
		//        Bezier(c2, --iter, color);
		//        return;
		//    }

		//    for(int i=0; i<n; i++)
		//    {
		//        //Kurve C1 zur Linie vereinfacht
		//        int b1x1=(int)Math.Round(q[i][0].x);
		//        int b1y1=(int)Math.Round(q[i][0].y);
		//        int b1x2=(int)Math.Round(q[i+1][0].x);
		//        int b1y2=(int)Math.Round(q[i+1][0].y);

		//        //Kurve C2 zur Linie vereinfacht
		//        int b2x1=(int)Math.Round(q[n-i][i].x);
		//        int b2y1=(int)Math.Round(q[n-i][i].y);
		//        int b2x2=(int)Math.Round(q[n-i-1][i+1].x);
		//        int b2y2=(int)Math.Round(q[n-i-1][i+1].y);

		//        Line(b1x1, b1y1, b1x2, b1y2, color);
		//        Line(b2x1, b2y1, b2x2, b2y2, color);
		//    }
		//}

		//public void Bezier(Tuple2dList p, int iter, byte r, byte g, byte b)
		//{
		//    Bezier(p, iter, Color.FromArgb(r, g, b));
		//}

		//public void Bezier(Tuple2dList p, int iter, byte alpha, byte r, byte g, byte b)
		//{
		//    Bezier(p, iter, Color.FromArgb(alpha, r, g, b));
		//}
		#endregion
	}
}
