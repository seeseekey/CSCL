using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Graphic
{
	public partial class gtImage
	{
		public gtImage SwapChannelsToBRG()
		{
            if(channelFormat!=Format.RGB) return this.ConvertToRGB().SwapChannelsToBRG();
			gtImage ret=new gtImage(width, height, Format.RGB);
			if(ret.imageData==null) return ret;

			uint count=width*height;
            byte[] src=imageData;
			uint ind=0;
            byte[] dst=ret.imageData;
			uint inds=0;

			//Channel Swap RGB
			for(uint i=0; i<count; i++)
			{
				byte r=src[inds++];
				byte g=src[inds++];
				byte b=src[inds++];
				dst[ind++]=b;
				dst[ind++]=r;
				dst[ind++]=g;
			}

			return ret;
		}

		public gtImage SwapChannelsToRBG()
		{
            if(channelFormat!=Format.RGB) return this.ConvertToRGB().SwapChannelsToBRG();
            gtImage ret=new gtImage(width, height, Format.RGB);
			if(ret.imageData==null) return ret;

			uint count=width*height;
			byte[] src=imageData;
			uint ind=0;
			byte[] dst=ret.imageData;
			uint inds=0;

			//Channel Swap RGB
			for(uint i=0; i<count; i++)
			{
				byte r=src[inds++];
				byte g=src[inds++];
				byte b=src[inds++];
				dst[ind++]=r;
				dst[ind++]=b;
				dst[ind++]=g;
			}

			return ret;
		}

		public gtImage SwapChannelsToGRB()
		{
            if(channelFormat!=Format.RGB) return this.ConvertToRGB().SwapChannelsToBRG();
            gtImage ret=new gtImage(width, height, Format.RGB);
			if(ret.imageData==null) return ret;

			uint count=width*height;
			byte[] src=imageData;
			uint ind=0;
			byte[] dst=ret.imageData;
			uint inds=0;

			//Channel Swap RGB
			for(uint i=0; i<count; i++)
			{
				byte r=src[inds++];
				byte g=src[inds++];
				byte b=src[inds++];
				dst[ind++]=g;
				dst[ind++]=r;
				dst[ind++]=b;
			}

			return ret;
		}

		public gtImage SwapChannelsToBGR()
		{
            if(channelFormat!=Format.RGB) return this.ConvertToRGB().SwapChannelsToBGR();
            gtImage ret=new gtImage(width, height, Format.RGB);
			if(ret.imageData==null) return ret;

			uint count=width*height;
			byte[] src=imageData;
			uint ind=0;
			byte[] dst=ret.imageData;
			uint inds=0;

			//Channel Swap RGB
			for(uint i=0; i<count; i++)
			{
				byte r=src[inds++];
				byte g=src[inds++];
				byte b=src[inds++];
				dst[ind++]=b;
				dst[ind++]=g;
				dst[ind++]=r;
			}

			return ret;
		}

		public gtImage SwapChannelsToGBR()
		{
            if(channelFormat!=Format.RGB) return this.ConvertToRGB().SwapChannelsToBRG();
            gtImage ret=new gtImage(width, height, Format.RGB);
			if(ret.imageData==null) return ret;

			uint count=width*height;
			byte[] src=imageData;
			uint ind=0;
			byte[] dst=ret.imageData;
			uint inds=0;

			//Channel Swap RGB
			for(uint i=0; i<count; i++)
			{
				byte r=src[inds++];
				byte g=src[inds++];
				byte b=src[inds++];
				dst[ind++]=g;
				dst[ind++]=b;
				dst[ind++]=r;
			}

			return ret;
		}
	}
}
