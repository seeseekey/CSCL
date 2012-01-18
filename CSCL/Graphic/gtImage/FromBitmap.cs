//
//  FromBitmap.cs
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
		public static gtImage FromBitmap(Bitmap bmp)
		{
			gtImage ret=null;

			uint width=(uint)bmp.Width;
			uint height=(uint)bmp.Height;

			if ((bmp.PixelFormat&PixelFormat.Alpha)==PixelFormat.Alpha)
			{
				ret=new gtImage(width, height, Format.RGBA);

				BitmapData data=bmp.LockBits(new Rectangle(0, 0, (int)width, (int)height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
				Marshal.Copy(data.Scan0, ret.imageData, 0, (int)(width*height*4));
				bmp.UnlockBits(data);
			}
			else
			{
				ret=new gtImage(width, height, Format.RGB);

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

			return ret;
		}
	}
}
