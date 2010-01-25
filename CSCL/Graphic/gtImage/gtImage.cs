using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using CSCL;

namespace CSCL.Graphic
{
	public partial class gtImage
	{
		static ImageCodecInfo jpegImageCodecInfo=GetEncoderInfo("image/jpeg");
		static ImageCodecInfo pngImageCodecInfo=GetEncoderInfo("image/png");

		static ImageCodecInfo GetEncoderInfo(string mimeType)
		{
			ImageCodecInfo[] encoders;
			encoders=ImageCodecInfo.GetImageEncoders();
			foreach(ImageCodecInfo encoder in encoders) if(encoder.MimeType==mimeType) return encoder;
			return null;
		}

		public enum Format
		{
			GRAY=0,
			GRAY_Alpha=1,
			RGB=2,
			RGBA=3,
			BGR=4,
			BGRA=5,
		}

		uint width;
		uint height;
		public byte[] imageData;
		Format channelFormat;

		#region Konstruktoren
		public gtImage()
		{
			width=height=0;
			imageData=null;
			channelFormat=Format.GRAY;
		}

		public gtImage(uint w, uint h)
		{
			width=height=0;
			imageData=null;
			channelFormat=Format.GRAY;

			width=w;
			height=h;
			if(w*h>0) imageData=new byte[w*h];
		}

		public gtImage(uint w, uint h, Format format)
		{
			width=height=0;
			imageData=null;
			channelFormat=Format.GRAY;

			width=w;
			height=h;
			channelFormat=format;
			if(w*h>0) imageData=new byte[w*h*ConvertToBytePerPixel(format)];
		}

		public gtImage(uint w, uint h, Format format, byte[] bits)
		{
			width=height=0;
			imageData=null;
			channelFormat=Format.GRAY;

			width=w;
			height=h;
			channelFormat=format;
			if(w*h>0&&w*h*ConvertToBytePerPixel(format)==bits.Length) imageData=bits;
		}
		#endregion

		public uint BytePerPixel { get { return ConvertToBytePerPixel(channelFormat); } }

		public uint Width
		{
			get { return width; }
		}

		public uint Height
		{
			get { return height; }
		}

		public Format ChannelFormat { get { return channelFormat; } }

		#region GetBitsWithGranularity & Get/SetPixel
		public byte[] GetBitsWithGranularity(uint granularity)
		{
			uint bpp=BytePerPixel;
			uint wb=bpp*width;
			uint granwidth=Align(wb, granularity);
			if(granwidth*height==0) return null;

			if(wb==granwidth) return imageData;

			byte[] ret=new byte[granwidth*height];
			if(ret==null) return ret;

			byte[] dst=ret;
			uint ind=0;
			byte[] src=imageData;
			uint inds=0;

			uint granspare=(granwidth-wb);
			for(uint y=0; y<height; y++)
			{
				for(uint i=0; i<wb; i++)
					dst[ind++]=src[inds++];

				for(uint i=0; i<granspare; i++)
					dst[ind++]=0;
			}

			return ret;
		}

		public Color GetPixel(uint x, uint y)
		{
			if(x>=width||y>=height) return Color.FromArgb(0, 0, 0, 0);

			ulong pos=y*width+x;
			pos*=BytePerPixel;

			switch(channelFormat)
			{
				case Format.GRAY:
					{
						int col=imageData[pos];
						return Color.FromArgb(col, col, col);
					}
				case Format.GRAY_Alpha:
					{
						int col=imageData[pos];
						return Color.FromArgb(imageData[pos+1], col, col, col);
					}
				case Format.BGR:
					{
						int r=imageData[pos];
						int g=imageData[pos+1];
						int b=imageData[pos+2];
						return Color.FromArgb(r, g, b);
					}
				case Format.BGRA:
					{
						int r=imageData[pos];
						int g=imageData[pos+1];
						int b=imageData[pos+2];
						int a=imageData[pos+3];
						return Color.FromArgb(a, r, g, b);
					}
				case Format.RGB:
					{
						int b=imageData[pos];
						int g=imageData[pos+1];
						int r=imageData[pos+2];
						return Color.FromArgb(r, g, b);
					}
				case Format.RGBA:
					{
						int b=imageData[pos];
						int g=imageData[pos+1];
						int r=imageData[pos+2];
						int a=imageData[pos+3];
						return Color.FromArgb(a, r, g, b);
					}
			}

			return Color.FromArgb(0, 0, 0, 0);
		}

		public Color GetPixel(int x, int y)
		{
			if(x<0||y<0) return Color.FromArgb(0, 0, 0, 0);
			if(x>=width||y>=height) return Color.FromArgb(0, 0, 0, 0);

			return GetPixel((uint)x, (uint)y);
		}

		public void SetPixel(uint x, uint y, Color color)
		{
			if(x>=width||y>=height) return;

			ulong pos=y*width+x;
			pos*=BytePerPixel;

			switch(channelFormat)
			{
				case Format.GRAY:
					{
						imageData[pos]=color.R;
						break;
					}
				case Format.GRAY_Alpha:
					{
						imageData[pos]=color.R;
						imageData[pos+1]=color.A;
						break;
					}
				case Format.BGR:
					{
						imageData[pos]=color.R;
						imageData[pos+1]=color.G;
						imageData[pos+2]=color.B;
						break;
					}
				case Format.BGRA:
					{
						imageData[pos]=color.R;
						imageData[pos+1]=color.G;
						imageData[pos+2]=color.B;
						imageData[pos+3]=color.A;
						break;
					}
				case Format.RGB:
					{
						imageData[pos]=color.B;
						imageData[pos+1]=color.G;
						imageData[pos+2]=color.R;
						break;
					}
				case Format.RGBA:
					{
						imageData[pos]=color.B;
						imageData[pos+1]=color.G;
						imageData[pos+2]=color.R;
						imageData[pos+3]=color.A;
						break;
					}
			}
		}

		public void SetPixel(int x, int y, Color color)
		{
			if(x<0||y<0) return;
			if(x>=width||y>=height) return;
			SetPixel((uint)x, (uint)y, color);
		}
		#endregion

		#region GetImage & GetSubImage & ToBitmap
		public gtImage GetImage()
		{
			return GetSubImage(0, 0, width, height);
		}

		public gtImage GetSubImage(uint x, uint y, uint w, uint h)
		{
			if(x>=width||y>=height) throw new ArgumentOutOfRangeException("x or y", "Out of image.");

			gtImage ret=new gtImage(w, h, channelFormat);

			ret.Draw(-(int)x, -(int)y, this);

			return ret;
		}

		/// <summary>
		/// Erzeugt ein Thumbnail 
		/// EXPERIMENTEL Muss getestet werdne
		/// Quadrates Thumbnail unter beibehaltung der Seitenverhälntisse
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		public gtImage ToThumbnail(uint size)
		{
			gtImage bmp=null;
			gtImage crapped=null;
			uint x=0, y=0;
			double prop=0;

			if (width>size)
			{
				// compute proportation
				prop=(double)width/(double)height;

				if (width>height)
				{
                    x=(uint)System.Math.Round(size*prop, 0);
					y=size;
				}
				else
				{
					x=size;
                    y=(uint)System.Math.Round(size/prop, 0);
				}

				//TESTEN

				bmp=new gtImage(x, y);
				//bmp=new System.Drawing.Bitmap((Image)image, new Size(x, y));

				crapped=new gtImage(size, size, channelFormat);
				crapped.Draw(0, 0, bmp);

				//crapped=new System.Drawing.Bitmap(75, 75);
				//Graphics g=Graphics.FromImage(crapped);
				//g.DrawImage(bmp,
				//    new Rectangle(0, 0, 75, 75),
				//    new Rectangle(0, 0, 75, 75),
				//    GraphicsUnit.Pixel);

				bmp=crapped;
			}
			else
			{
				crapped=this;
			}

			return bmp;
		}

		public Bitmap ToBitmap()
		{
			gtImage intern=null;
			switch(channelFormat)
			{
				case Format.GRAY:
					intern=ConvertToRGB(); break;
				case Format.GRAY_Alpha:
					intern=ConvertToRGBA(); break;
				case Format.RGB:
					intern=ConvertToRGB(); break;
				case Format.BGR:
					intern=ConvertToRGB(); break;
				case Format.RGBA:
					intern=ConvertToRGBA(); break;
				case Format.BGRA:
					intern=ConvertToRGBA(); break;
			}

			if(intern==null) throw new Exception("Null image can't be converted.");

			if(intern.channelFormat==Format.RGBA)
			{
				Bitmap bmp=new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);
				BitmapData data=bmp.LockBits(new Rectangle(0, 0, (int)width, (int)height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

				Marshal.Copy(intern.imageData, 0, data.Scan0, (int)(width*height*4));

				bmp.UnlockBits(data);
				data=null;
				return bmp;
			}
			else
			{
				Bitmap bmp=new Bitmap((int)width, (int)height, PixelFormat.Format24bppRgb);
				BitmapData data=bmp.LockBits(new Rectangle(0, 0, (int)width, (int)height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

				if(((int)width*3)==data.Stride)
				{
					Marshal.Copy(intern.imageData, 0, data.Scan0, (int)(width*height*3));
				}
				else
				{
					if(IntPtr.Size==4)
					{
						for(uint i=0; i<height; i++)
						{
							Marshal.Copy(intern.imageData, (int)(width*3*i), (IntPtr)(data.Scan0.ToInt32()+(int)(i*data.Stride)), (int)(width*3));
						}
					}
					else if(IntPtr.Size==8)
					{
						for(uint i=0; i<height; i++)
						{
							Marshal.Copy(intern.imageData, (int)(width*3*i), (IntPtr)(data.Scan0.ToInt64()+(long)(i*data.Stride)), (int)(width*3));
						}
					}
				}
				bmp.UnlockBits(data);
				data=null;
				return bmp;
			}
		}
		#endregion

		/////////////////////////////////////////////////////////////////
		// Resize
		/////////////////////////////////////////////////////////////////
		#region Resize
		public gtImage Resize(uint w, uint h)
		{
			if(width==w&&height==h) return this;
			if((width*height)==0) return new gtImage(0, 0, channelFormat);
			if((w*h)==0) return new gtImage(0, 0, channelFormat);

			if(width<=w&&height<=h) return NearestPixelResize(w, h);

			if(width>w&&height>h) return Downsample(w, h);

			if(width>w) return DownsampleH(w).NearestPixelResizeV(h);

			return DownsampleV(h).NearestPixelResizeH(w);
		}

		public gtImage ResizeToPowerOf2()
		{
			return Resize(CSCL.Maths.Arithmetic.MakePowerOf2(width), CSCL.Maths.Arithmetic.MakePowerOf2(height));
		}

		public gtImage ResizeByWidth(double newWidth)
		{
			double sizeFactor=newWidth/width;
			double newHeigth=sizeFactor*height;

			return Resize((uint)newWidth, (uint)newHeigth);
		}
		#endregion

		/////////////////////////////////////////////////////////////////
		// noch mehr Bildverarbeitung
		/////////////////////////////////////////////////////////////////
		public double Compare(gtImage ImageToCompare, uint Threshold)
		{
			if(ImageToCompare==null) throw new Exception("Image is null");
			if(Width!=ImageToCompare.Width) throw new Exception("Image have different sizes");
			if(Height!=ImageToCompare.Height) throw new Exception("Image have different sizes");
			if(channelFormat!=ImageToCompare.channelFormat) throw new Exception("Image have different formats");

			uint divergency=0;

			for(uint y=0; y<Width; y++)
			{
				for(uint x=0; x<Height; x++)
				{
					Color picA=GetPixel(x, y);
					Color picB=ImageToCompare.GetPixel(x, y);

					int dif=0;

                    dif+=System.Math.Abs(picA.R-picB.R);
                    dif+=System.Math.Abs(picA.G-picB.G);
                    dif+=System.Math.Abs(picA.B-picB.B);
					dif/=3;

					if(dif>Threshold) divergency++;
				}
			}

			//Gibt in % zurück wie stark die Bilder von einander abweichen
			//0 % == Bilder sind Identisch
			return (double)(100*(divergency/(double)(Width*Height)));
		}

		#region Inverted, InvertedAlpha
		public gtImage Inverted()
		{
			gtImage ret=new gtImage(width, height, channelFormat);
			if(ret.imageData==null) return ret;

			uint count=width*height*BytePerPixel;
			if(channelFormat==Format.BGR||channelFormat==Format.RGB||channelFormat==Format.GRAY)
			{
				for(uint i=0; i<count; i++) ret.imageData[i]=(byte)(255-imageData[i]);
			}
			else if(channelFormat==Format.BGRA||channelFormat==Format.RGBA)
			{
				for(uint i=0; i<count; i+=4)
				{
					ret.imageData[i]=(byte)(255-imageData[i]);
					ret.imageData[i+1]=(byte)(255-imageData[i+1]);
					ret.imageData[i+2]=(byte)(255-imageData[i+2]);
					ret.imageData[i+3]=imageData[i+3];
				}
			}
			else if(channelFormat==Format.GRAY_Alpha)
			{
				for(uint i=0; i<count; i+=2)
				{
					ret.imageData[i]=(byte)(255-imageData[i]);
					ret.imageData[i+1]=imageData[i+1];
				}
			}

			return ret;
		}

		/// <summary>
		/// Invertiert den Alphakanal
		/// </summary>
		/// <returns></returns>
		public gtImage InvertAlpha()
		{
			if (channelFormat!=Format.RGBA&&channelFormat!=Format.BGRA&&channelFormat!=Format.GRAY_Alpha) return this;

			gtImage ret=new gtImage(width, height, channelFormat);
			if (ret.imageData==null) return ret;

			uint count=width*height*BytePerPixel;
			if (channelFormat==Format.BGRA||channelFormat==Format.RGBA)
			{
				for (uint i=0; i<count; i+=4)
				{
					ret.imageData[i]=(byte)(imageData[i]);
					ret.imageData[i+1]=(byte)(imageData[i+1]);
					ret.imageData[i+2]=(byte)(imageData[i+2]);
					ret.imageData[i+3]=(byte)(255-imageData[i+3]);
				}
			}
			else if (channelFormat==Format.GRAY_Alpha)
			{
				for (uint i=0; i<count; i+=2)
				{
					ret.imageData[i]=(byte)(imageData[i]);
					ret.imageData[i+1]=(byte)(255-imageData[i+1]);
				}
			}

			return ret;
		}
		#endregion
	}
}
