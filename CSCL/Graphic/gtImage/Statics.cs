//
//  Statics.cs
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

namespace CSCL.Graphic
{
	public partial class gtImage
	{
		/////////////////////////////////////////////////////////////////
		// Statics
		/////////////////////////////////////////////////////////////////
		#region Sonstiges
		public static uint ConvertToBytePerPixel(Format format)
		{
			switch(format)
			{
				default: return 1;
				case Format.GRAY: return 1;
				case Format.GRAY_Alpha: return 2;
				case Format.RGB: return 3;
				case Format.BGR: return 3;
				case Format.RGBA: return 4;
				case Format.BGRA: return 4;
			}
		}

		public static uint Align(uint x, uint gran)
		{
			if(gran==1) return x;
			if(gran==0) return x;
			return ((x+(gran-1))/gran)*gran;
		}

		public static string ToFormatString(Format format)
		{
			switch(format)
			{
				default: return "Gray";
				case Format.GRAY: return "Gray";
				case Format.GRAY_Alpha: return "GrayAlpha";
				case Format.RGB: return "RGB";
				case Format.BGR: return "BGR";
				case Format.RGBA: return "RGBA";
				case Format.BGRA: return "BGRA";
			}
		}
		#endregion

		/////////////////////////////////////////////////////////////////
		// RGB to ... 
		/////////////////////////////////////////////////////////////////
		#region RGBToHSI/HSL
		// RGB to HSI (Gonzalez and Woods)
		public static byte[] RGB2HSIByte(byte r, byte g, byte b)
		{
			byte[] ret=new byte[3];
			int l=r+g+b;
			ret[0]=(byte)(l/3);

			if(l==0)
			{
				ret[1]=ret[2]=0;
				return ret;
			}

            int s=255-(765*System.Math.Min(System.Math.Min(r, g), b))/l;
			ret[1]=(byte)s;

			double rg=r-g, rb=r-b;
            double h=System.Math.Acos((rg+rb)/(2*System.Math.Sqrt(rg*rg+rb*(g-b))))*40.74366543;
			if(b>g) h=255-h;
			ret[2]=(byte)h;

			return ret;
		}

		// RGB to HSL (Foley and VanDam)
		public static byte[] RGB2HSLByte(byte r, byte g, byte b)
		{
			byte[] ret=new byte[3];

            byte mx=System.Math.Max(System.Math.Max(r, g), b), mi=System.Math.Min(System.Math.Min(r, g), b);
			int l=(mx+mi)/2;
			ret[0]=(byte)l;

			int s, delta=mx-mi;
			if(delta==0)
			{
				ret[1]=ret[2]=0;
				return ret;
			}

			if(l<128) s=255*delta/(mx+mi);
			else s=255*delta/(510-mx-mi);
			ret[1]=(byte)s;

			int h;
			if(r==mx) h=(g-b)*256/delta;
			else if(g==mx) h=512+(b-r)*256/delta;
			else h=1024+(r-g)*256/delta;
			if(h<0) h=h+1536;
			ret[2]=(byte)(h/6);

			return ret;
		}

		// RGB to HSL (Foley and VanDam)
		public static void RGB2HSLDouble(byte r, byte g, byte b, out double lit, out double sat, out double h)
		{
            byte mx=System.Math.Max(System.Math.Max(r, g), b), mi=System.Math.Min(System.Math.Min(r, g), b);
			lit=mx+mi;

			double delta=mx-mi;
			if(delta==0)
			{
				sat=h=0;
				return;
			}

			if(lit<=255) sat=255*delta/lit;
			else sat=255*delta/(510-lit);

			if(r==mx) h=(g-b)*60/delta;
			else if(g==mx) h=120+(b-r)*60/delta;
			else h=240+(r-g)*60/delta;
			if(h<0) h=h+360;
		}

		// HSL to RGB (Foley and VanDam)
		public static void HSL2RGBDouble(out byte r, out byte g, out byte b, double lit, double sat, double h)
		{
			if(sat==0)
			{
				r=g=b=(byte)(lit/2);
				return;
			}

			double m1, m2;
			if(lit<=255) m2=lit*(255+sat)/510;
			else m2=lit*(255-sat)/510+sat;
			m1=lit-m2;
			double hue=h-120;
			if(hue<0) hue=hue+360;
			if(hue<60) b=(byte)(m1+(m2-m1)*hue/60);
			else if(hue<180) b=(byte)(m2);
			else if(hue<240) b=(byte)(m1+(m2-m1)*(240-hue)/60);
			else b=(byte)(m1);
			hue=h;
			if(hue<60) g=(byte)(m1+(m2-m1)*hue/60);
			else if(hue<180) g=(byte)(m2);
			else if(hue<240) g=(byte)(m1+(m2-m1)*(240-hue)/60);
			else g=(byte)(m1);
			hue=h+120;
			if(hue>360) hue=hue-360;
			if(hue<60) r=(byte)(m1+(m2-m1)*hue/60);
			else if(hue<180) r=(byte)(m2);
			else if(hue<240) r=(byte)(m1+(m2-m1)*(240-hue)/60);
			else r=(byte)(m1);
		}
		#endregion
	}
}
