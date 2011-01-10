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
		// FromStream (BMP, DIB, JPG, GIF, PNG)
		/////////////////////////////////////////////////////////////////
		#region FromStream
		public static gtImage FromStream(Stream stream)
		{
			Image img=Image.FromStream(stream);
			uint width=(uint)img.Size.Width;
			uint height=(uint)img.Size.Height;

			gtImage ret=null;

			if ((img.PixelFormat&PixelFormat.Alpha)==PixelFormat.Alpha)
			{
				ret=new gtImage(width, height, Format.RGBA);

				Bitmap bmp=new Bitmap(img);
				BitmapData data=bmp.LockBits(new Rectangle(0, 0, (int)width, (int)height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
				Marshal.Copy(data.Scan0, ret.imageData, 0, (int)(width*height*4));
				bmp.UnlockBits(data);
			}
			else
			{
				ret=new gtImage(width, height, Format.RGB);

				Bitmap bmp=new Bitmap(img);
				BitmapData data=bmp.LockBits(new Rectangle(0, 0, (int)width, (int)height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
				if (((int)width*3)==data.Stride)
				{
					Marshal.Copy(data.Scan0, ret.imageData, 0, (int)(width*height*3));
				}
				else
				{
					if (IntPtr.Size==4)
					{
						for (uint i=0; i<height; i++)
						{
							Marshal.Copy((IntPtr)(data.Scan0.ToInt32()+(int)(i*data.Stride)), ret.imageData, (int)(width*3*i), (int)(width*3));
						}
					}
					else if (IntPtr.Size==8)
					{
						for (uint i=0; i<height; i++)
						{
							Marshal.Copy((IntPtr)(data.Scan0.ToInt64()+(long)(i*data.Stride)), ret.imageData, (int)(width*3*i), (int)(width*3));
						}
					}
				}

				bmp.UnlockBits(data);
				data=null;
				bmp.Dispose();
				bmp=null;
			}

			img.Dispose();
			img=null;

			return ret;
		}
		#endregion
	}
}