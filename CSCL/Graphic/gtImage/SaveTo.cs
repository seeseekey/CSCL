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
		/////////////////////////////////////////////////////////////////
		// SaveToTGA
		/////////////////////////////////////////////////////////////////
		#region SaveToTGA
		public int SaveToTGA(string pathfilename)
		{
			if(pathfilename==null) return 2;
			if(pathfilename=="") return 2;
			if(width==0||height==0) return 3;
			if(width>0xFFFF||height>0xFFFF) return 4;

			if(channelFormat==Format.BGR) return ConvertToRGB().SaveToTGA(pathfilename);
			if(channelFormat==Format.BGRA) return ConvertToRGBA().SaveToTGA(pathfilename);

			bool isRGB=(channelFormat==Format.BGR||channelFormat==Format.RGB||channelFormat==Format.BGRA||channelFormat==Format.RGBA);
			bool isAlpha=(channelFormat==Format.BGRA||channelFormat==Format.RGBA||channelFormat==Format.GRAY_Alpha);

			ulong size=(ulong)(18+((isRGB)?(isAlpha?4:3):(isAlpha?2:1))*width*height);	// Länge der Daten
			if(size>0xFFFFFFFF) return 4;		// Übertrieben groß

			using(FileStream fs=new FileStream(pathfilename, FileMode.Create, FileAccess.Write))
			{
				BinaryWriter bw=new BinaryWriter(fs);

				byte Pixel_Depth=(byte)(isRGB?(isAlpha?32:24):(isAlpha?16:8));
				byte Image_Descriptor=(byte)(isAlpha?0x28:0x20);	// Field 5.6

				// Schreiben der Header (18 bytes)
				bw.Write((byte)0);				// ID_Length
				bw.Write((byte)0);				// Color_Map_Type
				bw.Write((byte)(isRGB?2:3));	// Image_Type
				bw.Write((ushort)0);			// First_Entry_Index
				bw.Write((ushort)0);			// Color_Map_Length
				bw.Write((byte)0);				// Color_Map_Entry_Size
				bw.Write((ushort)0);			// X_Origin
				bw.Write((ushort)0);			// Y_Origin
				bw.Write((ushort)width);		// Width
				bw.Write((ushort)height);		// Height
				bw.Write(Pixel_Depth);			// Pixel_Depth
				bw.Write(Image_Descriptor);		// Image_Descriptor

				bw.Write(imageData);
				bw.Close();
				fs.Close();
			}
			return 0;
		}
		#endregion

		/////////////////////////////////////////////////////////////////
		// SaveToJpeg
		/////////////////////////////////////////////////////////////////
		#region SaveToJpeg
		public void SaveToJpeg(string filename)
		{
			SaveToJpeg(filename, -1, -1);
		}

		public void SaveToJpeg(string filename, byte quality)
		{
			SaveToJpeg(filename, -1, -1, quality);
		}

		public void SaveToJpeg(string filename, int exifWidth, int exifHeight)
		{
			if(channelFormat==Format.RGBA)
			{
				ConvertToRGB().SaveToJpeg(filename, exifWidth, exifHeight);
				return;
			}

			if(channelFormat!=Format.RGB) throw new Exception("Only rgb images can be saved by SaveToJpeg.");

			Bitmap bmp=new Bitmap((int)width, (int)height, PixelFormat.Format24bppRgb);

			BitmapData data=bmp.LockBits(new Rectangle(0, 0, (int)width, (int)height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

			if(((int)width*3)==data.Stride)
			{
				Marshal.Copy(imageData, 0, data.Scan0, (int)(width*height*3));
			}
			else
			{
				if(IntPtr.Size==4)
				{
					for(uint i=0; i<height; i++)
					{
						Marshal.Copy(imageData, (int)(width*3*i), (IntPtr)(data.Scan0.ToInt32()+(int)(i*data.Stride)), (int)(width*3));
					}
				}
				else if(IntPtr.Size==8)
				{
					for(uint i=0; i<height; i++)
					{
						Marshal.Copy(imageData, (int)(width*3*i), (IntPtr)(data.Scan0.ToInt64()+(long)(i*data.Stride)), (int)(width*3));
					}
				}
			}
			bmp.UnlockBits(data);
			data=null;

			if(exifWidth>0&&exifHeight>0)
			{
				PropertyItem o=(PropertyItem)Activator.CreateInstance(typeof(PropertyItem), true);

				byte[] buff=Encoding.ASCII.GetBytes("ASCII\0\0\0"+exifWidth+"x"+exifHeight);
				o.Id=0x9286;
				o.Type=7;
				o.Len=buff.Length;
				o.Value=buff;
				bmp.SetPropertyItem(o);

				o=(PropertyItem)Activator.CreateInstance(typeof(PropertyItem), true);
				o.Id=0x100;
				o.Type=4;
				o.Len=4;
				o.Value=BitConverter.GetBytes((uint)exifWidth);
				bmp.SetPropertyItem(o);

				o=(PropertyItem)Activator.CreateInstance(typeof(PropertyItem), true);
				o.Id=0x101;
				o.Type=4;
				o.Len=4;
				o.Value=BitConverter.GetBytes((uint)exifHeight);
				bmp.SetPropertyItem(o);
			}

			bmp.Save(filename, ImageFormat.Jpeg);
			bmp.Dispose();
			bmp=null;
		}

		public void SaveToJpeg(string filename, int exifWidth, int exifHeight, byte quality)
		{
			if(channelFormat==Format.RGBA)
			{
				ConvertToRGB().SaveToJpeg(filename, exifWidth, exifHeight);
				return;
			}

			if(channelFormat!=Format.RGB) throw new Exception("Only rgb images can be saved by SaveToJpeg.");

			Bitmap bmp=new Bitmap((int)width, (int)height, PixelFormat.Format24bppRgb);

			BitmapData data=bmp.LockBits(new Rectangle(0, 0, (int)width, (int)height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

			if(((int)width*3)==data.Stride)
			{
				Marshal.Copy(imageData, 0, data.Scan0, (int)(width*height*3));
			}
			else
			{
				if(IntPtr.Size==4)
				{
					for(uint i=0; i<height; i++)
					{
						Marshal.Copy(imageData, (int)(width*3*i), (IntPtr)(data.Scan0.ToInt32()+(int)(i*data.Stride)), (int)(width*3));
					}
				}
				else if(IntPtr.Size==8)
				{
					for(uint i=0; i<height; i++)
					{
						Marshal.Copy(imageData, (int)(width*3*i), (IntPtr)(data.Scan0.ToInt64()+(long)(i*data.Stride)), (int)(width*3));
					}
				}
			}
			bmp.UnlockBits(data);
			data=null;

			if(exifWidth>0&&exifHeight>0)
			{
				PropertyItem o=(PropertyItem)Activator.CreateInstance(typeof(PropertyItem), true);

				byte[] buff=Encoding.ASCII.GetBytes("ASCII\0\0\0"+exifWidth+"x"+exifHeight);
				o.Id=0x9286;
				o.Type=7;
				o.Len=buff.Length;
				o.Value=buff;
				bmp.SetPropertyItem(o);

				o=(PropertyItem)Activator.CreateInstance(typeof(PropertyItem), true);
				o.Id=0x100;
				o.Type=4;
				o.Len=4;
				o.Value=BitConverter.GetBytes((uint)exifWidth);
				bmp.SetPropertyItem(o);

				o=(PropertyItem)Activator.CreateInstance(typeof(PropertyItem), true);
				o.Id=0x101;
				o.Type=4;
				o.Len=4;
				o.Value=BitConverter.GetBytes((uint)exifHeight);
				bmp.SetPropertyItem(o);
			}

			if(quality>100) quality=100;

			EncoderParameters encoderParameters=new EncoderParameters(1);
			encoderParameters.Param[0]=new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);
			bmp.Save(filename, jpegImageCodecInfo, encoderParameters);
			encoderParameters.Dispose();
			bmp.Dispose();
			bmp=null;
		}
		#endregion

		/////////////////////////////////////////////////////////////////
		// SaveToTiff
		/////////////////////////////////////////////////////////////////
		#region Save To Tiff
		public void SaveToTiffGDI(string filename)
		{
			if(channelFormat==Format.BGR) {ConvertToRGB().SaveToTiffGDI(filename); return; }
			if(channelFormat==Format.BGRA) { ConvertToRGBA().SaveToTiffGDI(filename); return; }

			if(channelFormat!=Format.RGB&&channelFormat!=Format.RGBA) // nur RGB(A) Bilder bitte
				throw new Exception("Only rgb(a) images can be saved by SaveToTiff.");

			if(channelFormat==Format.RGBA)
			{
				Bitmap bmp=new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);
				BitmapData data=bmp.LockBits(new Rectangle(0, 0, (int)width, (int)height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

				Marshal.Copy(imageData, 0, data.Scan0, (int)(width*height*4));

				bmp.UnlockBits(data);
				data=null;

				bmp.Save(filename, ImageFormat.Tiff);
				bmp.Dispose();
				bmp=null;
			}
			else
			{
				Bitmap bmp=new Bitmap((int)width, (int)height, PixelFormat.Format24bppRgb);
				BitmapData data=bmp.LockBits(new Rectangle(0, 0, (int)width, (int)height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

				if(((int)width*3)==data.Stride)
				{
					Marshal.Copy(imageData, 0, data.Scan0, (int)(width*height*3));
				}
				else
				{
					if(IntPtr.Size==4)
					{
						for(uint i=0; i<height; i++)
						{
							Marshal.Copy(imageData, (int)(width*3*i), (IntPtr)(data.Scan0.ToInt32()+(int)(i*data.Stride)), (int)(width*3));
						}
					}
					else if(IntPtr.Size==8)
					{
						for(uint i=0; i<height; i++)
						{
							Marshal.Copy(imageData, (int)(width*3*i), (IntPtr)(data.Scan0.ToInt64()+(long)(i*data.Stride)), (int)(width*3));
						}
					}
				}
				bmp.UnlockBits(data);
				data=null;

				bmp.Save(filename, ImageFormat.Tiff);
				bmp.Dispose();
				bmp=null;
			}
		}
		#endregion

		/////////////////////////////////////////////////////////////////
		// SaveToBMP
		/////////////////////////////////////////////////////////////////
		#region SaveToBMP
		public void SaveToBMP(string filename)
		{
			if(channelFormat!=Format.RGB&&channelFormat!=Format.RGBA) //nur RGB Bilder bitte
				throw new Exception("Only rgb or rgba images can be saved by SaveToBMP.");

			BinaryWriter fileWriter=new BinaryWriter(File.Open(filename, FileMode.Create));

			#region Header
			fileWriter.Write('B'); //Signatur
			fileWriter.Write('M'); //Signatur
			fileWriter.Write((uint)0); //Größe der Datei (TODO)
			fileWriter.Write((uint)0); //Reserved
			fileWriter.Write((uint)70); //Offset der Bilddaten
			#endregion

			#region Informationsblock
			fileWriter.Write((uint)40); //Größe des Infoblockes
			fileWriter.Write((int)width); //Bildbreite
			fileWriter.Write((int)-height); //Bildhöhe (Top Down Bild)
			fileWriter.Write((ushort)1); //Planes (immer 1)

			if(channelFormat==Format.RGB) fileWriter.Write((ushort)24); //24 Bit
			else if(channelFormat==Format.RGBA) fileWriter.Write((ushort)32); //32 Bit

			fileWriter.Write((uint)0); //Kompression (BI_RGB / Unkomprimiert)
			fileWriter.Write((uint)0); //Größe der Bilddaten (darf 0 sein)

			fileWriter.Write((int)0); //Horizontale Auflösung des Zielausgabegerätes in Pixel pro Meter 
			fileWriter.Write((int)0); //Vertikale Auflösung des Zielausgabegerätes in Pixel pro Meter 

			fileWriter.Write((uint)0); //biClrUsed (0 da keine Palette)
			fileWriter.Write((uint)0); //biClrImportant (0 da keine Palette)
			#endregion

			#region RGB Quad
			fileWriter.Write((uint)0); //Blau
			fileWriter.Write((uint)0); //Grün
			fileWriter.Write((uint)0); //Rot
			fileWriter.Write((uint)0); //Reserved
			#endregion

			fileWriter.Write(GetBitsWithGranularity(4));

			fileWriter.Close();
		}
		#endregion

		#region SaveToBMPGDI
		public void SaveToBMPGDI(string filename)
		{
			if(channelFormat!=Format.RGB) // nur RGB Bilder bitte
				throw new Exception("Only rgb images can be saved by SaveToBMP.");

			Bitmap bmp=new Bitmap((int)width, (int)height, PixelFormat.Format24bppRgb);
			BitmapData data=bmp.LockBits(new Rectangle(0, 0, (int)width, (int)height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

			if(((int)width*3)==data.Stride)
			{
				Marshal.Copy(imageData, 0, data.Scan0, (int)(width*height*3));
			}
			else
			{
				if(IntPtr.Size==4)
				{
					for(uint i=0; i<height; i++)
					{
						Marshal.Copy(imageData, (int)(width*3*i), (IntPtr)(data.Scan0.ToInt32()+(int)(i*data.Stride)), (int)(width*3));
					}
				}
				else if(IntPtr.Size==8)
				{
					for(uint i=0; i<height; i++)
					{
						Marshal.Copy(imageData, (int)(width*3*i), (IntPtr)(data.Scan0.ToInt64()+(long)(i*data.Stride)), (int)(width*3));
					}
				}
			}
			bmp.UnlockBits(data);
			data=null;

			bmp.Save(filename, ImageFormat.Bmp);
			bmp.Dispose();
			bmp=null;
		}
		#endregion

		/////////////////////////////////////////////////////////////////
		// SaveToPNG
		/////////////////////////////////////////////////////////////////
		#region SaveToPNGGDI
		public void SaveToPNGGDI(string filename)
		{
			if(channelFormat==Format.BGR) ConvertToRGB().SaveToPNGGDI(filename);
			if(channelFormat==Format.BGRA) ConvertToRGBA().SaveToPNGGDI(filename);

			if(channelFormat!=Format.RGB&&channelFormat!=Format.RGBA) // nur RGB(A) Bilder bitte
				throw new Exception("Only rgb(a) images can be saved by SaveToPNG.");

			if(channelFormat==Format.RGBA)
			{
				Bitmap bmp=new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);
				BitmapData data=bmp.LockBits(new Rectangle(0, 0, (int)width, (int)height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

				Marshal.Copy(imageData, 0, data.Scan0, (int)(width*height*4));

				bmp.UnlockBits(data);
				data=null;

				bmp.Save(filename, ImageFormat.Png);
				bmp.Dispose();
				bmp=null;
			}
			else
			{
				Bitmap bmp=new Bitmap((int)width, (int)height, PixelFormat.Format24bppRgb);
				BitmapData data=bmp.LockBits(new Rectangle(0, 0, (int)width, (int)height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

				if(((int)width*3)==data.Stride)
				{
					Marshal.Copy(imageData, 0, data.Scan0, (int)(width*height*3));
				}
				else
				{
					if(IntPtr.Size==4)
					{
						for(uint i=0; i<height; i++)
						{
							Marshal.Copy(imageData, (int)(width*3*i), (IntPtr)(data.Scan0.ToInt32()+(int)(i*data.Stride)), (int)(width*3));
						}
					}
					else if(IntPtr.Size==8)
					{
						for(uint i=0; i<height; i++)
						{
							Marshal.Copy(imageData, (int)(width*3*i), (IntPtr)(data.Scan0.ToInt64()+(long)(i*data.Stride)), (int)(width*3));
						}
					}
				}
				bmp.UnlockBits(data);
				data=null;

				bmp.Save(filename, ImageFormat.Png);
				bmp.Dispose();
				bmp=null;
			}
		}
		#endregion

		/////////////////////////////////////////////////////////////////
		// SaveToFile
		/////////////////////////////////////////////////////////////////
		#region SaveToFile
		public void SaveToFile(string filename)
		{
			string ext=FileSystem.GetExtension(filename).ToLower();

			switch(ext)
			{
				case "png": SaveToPNGGDI(filename); break;
				case "jpg":
				case "jpeg":
				case "jpe":
				case "jif":
				case "jfi":
				case "jfif":
				case "psi":
				case "pmi": SaveToJpeg(filename); break;
				case "tga": SaveToTGA(filename); break;
				case "bmp":
				case "dib": SaveToBMP(filename); break;
				case "tiff":
				case "tif": SaveToTiffGDI(filename); break;
				default: throw new Exception("Unknown Extension.");
			}
		}
		#endregion
	}
}
