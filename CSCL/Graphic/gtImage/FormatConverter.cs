using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Graphic
{
	public partial class gtImage
	{
		/////////////////////////////////////////////////////////////////
		// Format-Converter
		/////////////////////////////////////////////////////////////////
		#region Format-Converter
		public gtImage ConvertToGray()
		{
			if(channelFormat==Format.GRAY) return this;
			gtImage ret=new gtImage(width, height, Format.GRAY);
			if(ret.imageData==null) return ret;

			uint count=width*height;
			byte[] src=imageData;
			uint ind=0;
			byte[] dst=ret.imageData;
			uint inds=0;
			switch(channelFormat)
			{
				case Format.GRAY_Alpha:
					for(uint i=0; i<count; i++) { dst[ind++]=src[inds++]; inds++; }
					break;
				case Format.BGR:
					for(uint i=0; i<count; i++) { byte r=src[inds++]; byte g=src[inds++]; byte b=src[inds++]; dst[ind++]=(byte)(0.299*r+0.587*g+0.114*b); }
					break;
				case Format.RGB:
					for(uint i=0; i<count; i++) { byte b=src[inds++]; byte g=src[inds++]; byte r=src[inds++]; dst[ind++]=(byte)(0.299*r+0.587*g+0.114*b); }
					break;
				case Format.BGRA:
					for(uint i=0; i<count; i++) { byte r=src[inds++]; byte g=src[inds++]; byte b=src[inds++]; inds++; dst[ind++]=(byte)(0.299*r+0.587*g+0.114*b); }
					break;
				case Format.RGBA:
					for(uint i=0; i<count; i++) { byte b=src[inds++]; byte g=src[inds++]; byte r=src[inds++]; inds++; dst[ind++]=(byte)(0.299*r+0.587*g+0.114*b); }
					break;
			}
			return ret;
		}

		public gtImage ConvertToGrayAlpha()
		{
			if(channelFormat==Format.GRAY_Alpha) return this;
			gtImage ret=new gtImage(width, height, Format.GRAY_Alpha);
			if(ret.imageData==null) return ret;

			uint count=width*height;
			byte[] src=imageData;
			uint ind=0;
			byte[] dst=ret.imageData;
			uint inds=0;
			switch(channelFormat)
			{
				case Format.GRAY:
					for(uint i=0; i<count; i++) { dst[ind++]=src[inds++]; dst[ind++]=255; }
					break;
				case Format.BGR:
					for(uint i=0; i<count; i++) { byte r=src[inds++]; byte g=src[inds++]; byte b=src[inds++]; dst[ind++]=(byte)(0.299*r+0.587*g+0.114*b); dst[ind++]=255; }
					break;
				case Format.RGB:
					for(uint i=0; i<count; i++) { byte b=src[inds++]; byte g=src[inds++]; byte r=src[inds++]; dst[ind++]=(byte)(0.299*r+0.587*g+0.114*b); dst[ind++]=255; }
					break;
				case Format.BGRA:
					for(uint i=0; i<count; i++) { byte r=src[inds++]; byte g=src[inds++]; byte b=src[inds++]; dst[ind++]=(byte)(0.299*r+0.587*g+0.114*b); dst[ind++]=src[inds++]; }
					break;
				case Format.RGBA:
					for(uint i=0; i<count; i++) { byte b=src[inds++]; byte g=src[inds++]; byte r=src[inds++]; dst[ind++]=(byte)(0.299*r+0.587*g+0.114*b); dst[ind++]=src[inds++]; }
					break;
			}
			return ret;
		}

		public gtImage ConvertToRGB()
		{
			if(channelFormat==Format.RGB) return this;
			gtImage ret=new gtImage(width, height, Format.RGB);
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
				case Format.GRAY_Alpha:
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

		public gtImage ConvertToRGBA()
		{
			if(channelFormat==Format.RGBA) return this;
			gtImage ret=new gtImage(width, height, Format.RGBA);
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
				case Format.GRAY_Alpha:
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

		public gtImage ConvertToBGR()
		{
			if(channelFormat==Format.BGR) return this;
			gtImage ret=new gtImage(width, height, Format.BGR);
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
				case Format.GRAY_Alpha:
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

		public gtImage ConvertToBGRA()
		{
			if(channelFormat==Format.BGRA) return this;
			gtImage ret=new gtImage(width, height, Format.BGRA);
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
				case Format.GRAY_Alpha:
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

		public gtImage ConvertTo(Format trgformat)
		{
			switch(trgformat)
			{
				case Format.GRAY: return ConvertToGray();
				case Format.GRAY_Alpha: return ConvertToGrayAlpha();
				case Format.RGB: return ConvertToRGB();
				case Format.RGBA: return ConvertToRGBA();
				case Format.BGR: return ConvertToBGR();
				case Format.BGRA: return ConvertToBGRA();
			}
			return null;
		}

		public gtImage ConvertToBW(byte threshold)
		{
			gtImage ret=ConvertToGray();
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
