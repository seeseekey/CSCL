//
//  FormatConverter.cs
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

namespace CSCL.Imaging
{
	public partial class Graphic
	{
		#region Formatconverter
		public Graphic ConvertToGray()
		{
			if(channelFormat==Format.GRAY) return this;
			Graphic ret=new Graphic(width, height, Format.GRAY);
			if(ret.imageData==null) return ret;

			uint count=width*height;
			byte[] src=imageData;
			uint ind=0;
			byte[] dst=ret.imageData;
			uint inds=0;

			switch(channelFormat)
			{
				case Format.GRAYAlpha:
					{
						for(uint i=0; i<count; i++) { dst[ind++]=src[inds++]; inds++; }
						break;
					}
				case Format.BGR:
					{
						for(uint i=0; i<count; i++) { byte r=src[inds++]; byte g=src[inds++]; byte b=src[inds++]; dst[ind++]=(byte)(0.299*r+0.587*g+0.114*b); }
						break;
					}
				case Format.RGB:
					{
						for(uint i=0; i<count; i++) { byte b=src[inds++]; byte g=src[inds++]; byte r=src[inds++]; dst[ind++]=(byte)(0.299*r+0.587*g+0.114*b); }
						break;
					}
				case Format.BGRA:
					{
						for(uint i=0; i<count; i++) { byte r=src[inds++]; byte g=src[inds++]; byte b=src[inds++]; inds++; dst[ind++]=(byte)(0.299*r+0.587*g+0.114*b); }
						break;
					}
				case Format.RGBA:
					{
						for(uint i=0; i<count; i++) { byte b=src[inds++]; byte g=src[inds++]; byte r=src[inds++]; inds++; dst[ind++]=(byte)(0.299*r+0.587*g+0.114*b); }
						break;
					}
			}

			return ret;
		}

		public Graphic ConvertToGrayAlpha()
		{
			if(channelFormat==Format.GRAYAlpha) return this;
			Graphic ret=new Graphic(width, height, Format.GRAYAlpha);
			if(ret.imageData==null) return ret;

			uint count=width*height;
			byte[] src=imageData;
			uint ind=0;
			byte[] dst=ret.imageData;
			uint inds=0;
			switch(channelFormat)
			{
				case Format.GRAY:
					{
						for(uint i=0; i<count; i++) { dst[ind++]=src[inds++]; dst[ind++]=255; }
						break;
					}
				case Format.BGR:
					{
						for(uint i=0; i<count; i++) { byte r=src[inds++]; byte g=src[inds++]; byte b=src[inds++]; dst[ind++]=(byte)(0.299*r+0.587*g+0.114*b); dst[ind++]=255; }
						break;
					}
				case Format.RGB:
					{
						for(uint i=0; i<count; i++) { byte b=src[inds++]; byte g=src[inds++]; byte r=src[inds++]; dst[ind++]=(byte)(0.299*r+0.587*g+0.114*b); dst[ind++]=255; }
						break;
					}
				case Format.BGRA:
					{
						for(uint i=0; i<count; i++) { byte r=src[inds++]; byte g=src[inds++]; byte b=src[inds++]; dst[ind++]=(byte)(0.299*r+0.587*g+0.114*b); dst[ind++]=src[inds++]; }
						break;
					}
				case Format.RGBA:
					{
						for(uint i=0; i<count; i++) { byte b=src[inds++]; byte g=src[inds++]; byte r=src[inds++]; dst[ind++]=(byte)(0.299*r+0.587*g+0.114*b); dst[ind++]=src[inds++]; }
						break;
					}
			}
			return ret;
		}

		public Graphic ConvertToRGB()
		{
			if(channelFormat==Format.RGB) return this;
			Graphic ret=new Graphic(width, height, Format.RGB);
			if(ret.imageData==null) return ret;

			uint count=width*height;
			byte[] src=imageData;
			uint ind=0;
			byte[] dst=ret.imageData;
			uint inds=0;
			switch(channelFormat)
			{
				case Format.GRAY:
					for(uint i=0; i<count; i++) { byte g=src[inds++]; dst[ind++]=g; dst[ind++]=g; dst[ind++]=g; }
					break;
				case Format.GRAYAlpha:
					for(uint i=0; i<count; i++) { byte g=src[inds++]; dst[ind++]=g; dst[ind++]=g; dst[ind++]=g; inds++; }
					break;
				case Format.BGR:
					for(uint i=0; i<count; i++) { byte b=src[inds++]; byte g=src[inds++]; dst[ind++]=src[inds++]; dst[ind++]=g; dst[ind++]=b; }
					break;
				case Format.BGRA:
					for(uint i=0; i<count; i++) { byte b=src[inds++]; byte g=src[inds++]; dst[ind++]=src[inds++]; dst[ind++]=g; dst[ind++]=b; inds++; }
					break;
				case Format.RGBA:
					for(uint i=0; i<count; i++) { dst[ind++]=src[inds++]; dst[ind++]=src[inds++]; dst[ind++]=src[inds++]; inds++; }
					break;
			}
			return ret;
		}

		public Graphic ConvertToRGBA()
		{
			if(channelFormat==Format.RGBA) return this;
			Graphic ret=new Graphic(width, height, Format.RGBA);
			if(ret.imageData==null) return ret;

			uint count=width*height;
			byte[] src=imageData;
			uint ind=0;
			byte[] dst=ret.imageData;
			uint inds=0;
			switch(channelFormat)
			{
				case Format.GRAY:
					for(uint i=0; i<count; i++) { byte g=src[inds++]; dst[ind++]=g; dst[ind++]=g; dst[ind++]=g; dst[ind++]=255; }
					break;
				case Format.GRAYAlpha:
					for(uint i=0; i<count; i++) { byte g=src[inds++]; dst[ind++]=g; dst[ind++]=g; dst[ind++]=g; dst[ind++]=src[inds++]; }
					break;
				case Format.BGR:
					for(uint i=0; i<count; i++) { byte b=src[inds++]; byte g=src[inds++]; dst[ind++]=src[inds++]; dst[ind++]=g; dst[ind++]=b; dst[ind++]=255; }
					break;
				case Format.BGRA:
					for(uint i=0; i<count; i++) { byte b=src[inds++]; byte g=src[inds++]; dst[ind++]=src[inds++]; dst[ind++]=g; dst[ind++]=b; dst[ind++]=src[inds++]; }
					break;
				case Format.RGB:
					for(uint i=0; i<count; i++) { dst[ind++]=src[inds++]; dst[ind++]=src[inds++]; dst[ind++]=src[inds++]; dst[ind++]=255; }
					break;
			}
			return ret;
		}

		public Graphic ConvertToBGR()
		{
			if(channelFormat==Format.BGR) return this;
			Graphic ret=new Graphic(width, height, Format.BGR);
			if(ret.imageData==null) return ret;

			uint count=width*height;
			byte[] src=imageData;
			uint ind=0;
			byte[] dst=ret.imageData;
			uint inds=0;
			switch(channelFormat)
			{
				case Format.GRAY:
					for(uint i=0; i<count; i++) { byte g=src[inds++]; dst[ind++]=g; dst[ind++]=g; dst[ind++]=g; }
					break;
				case Format.GRAYAlpha:
					for(uint i=0; i<count; i++) { byte g=src[inds++]; dst[ind++]=g; dst[ind++]=g; dst[ind++]=g; inds++; }
					break;
				case Format.RGB:
					for(uint i=0; i<count; i++) { byte r=src[inds++]; byte g=src[inds++]; dst[ind++]=src[inds++]; dst[ind++]=g; dst[ind++]=r; }
					break;
				case Format.RGBA:
					for(uint i=0; i<count; i++) { byte r=src[inds++]; byte g=src[inds++]; dst[ind++]=src[inds++]; dst[ind++]=g; dst[ind++]=r; inds++; }
					break;
				case Format.BGRA:
					for(uint i=0; i<count; i++) { dst[ind++]=src[inds++]; dst[ind++]=src[inds++]; dst[ind++]=src[inds++]; inds++; }
					break;
			}
			return ret;
		}

		public Graphic ConvertToBGRA()
		{
			if(channelFormat==Format.BGRA) return this;
			Graphic ret=new Graphic(width, height, Format.BGRA);
			if(ret.imageData==null) return ret;

			uint count=width*height;
			byte[] src=imageData;
			uint ind=0;
			byte[] dst=ret.imageData;
			uint inds=0;
			switch(channelFormat)
			{
				case Format.GRAY:
					for(uint i=0; i<count; i++) { byte g=src[inds++]; dst[ind++]=g; dst[ind++]=g; dst[ind++]=g; dst[ind++]=255; }
					break;
				case Format.GRAYAlpha:
					for(uint i=0; i<count; i++) { byte g=src[inds++]; dst[ind++]=g; dst[ind++]=g; dst[ind++]=g; dst[ind++]=src[inds++]; }
					break;
				case Format.RGB:
					for(uint i=0; i<count; i++) { byte r=src[inds++]; byte g=src[inds++]; dst[ind++]=src[inds++]; dst[ind++]=g; dst[ind++]=r; dst[ind++]=255; }
					break;
				case Format.RGBA:
					for(uint i=0; i<count; i++) { byte r=src[inds++]; byte g=src[inds++]; dst[ind++]=src[inds++]; dst[ind++]=g; dst[ind++]=r; dst[ind++]=src[inds++]; }
					break;
				case Format.BGR:
					for(uint i=0; i<count; i++) { dst[ind++]=src[inds++]; dst[ind++]=src[inds++]; dst[ind++]=src[inds++]; dst[ind++]=255; }
					break;
			}
			return ret;
		}

		public Graphic ConvertTo(Format trgformat)
		{
			switch(trgformat)
			{
				case Format.GRAY: return ConvertToGray();
				case Format.GRAYAlpha: return ConvertToGrayAlpha();
				case Format.RGB: return ConvertToRGB();
				case Format.RGBA: return ConvertToRGBA();
				case Format.BGR: return ConvertToBGR();
				case Format.BGRA: return ConvertToBGRA();
			}
			return null;
		}

		public Graphic ConvertToBW(byte threshold)
		{
			Graphic ret=ConvertToGray();
			if(ret.imageData==null)
				return ret;

			uint count=width*height;

			for(uint i=0; i<count; i++)
				ret.imageData[i]=(ret.imageData[i]<threshold)?(byte)0:(byte)255;

			return ret;
		}
		#endregion
	}
}
