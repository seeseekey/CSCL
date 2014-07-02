//
//  Channels.cs
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
		public Graphic SwapChannelsToBRG()
		{
            if(channelFormat!=Format.RGB) return this.ConvertToRGB().SwapChannelsToBRG();
			Graphic ret=new Graphic(width, height, Format.RGB);
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

		public Graphic SwapChannelsToRBG()
		{
            if(channelFormat!=Format.RGB) return this.ConvertToRGB().SwapChannelsToBRG();
            Graphic ret=new Graphic(width, height, Format.RGB);
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

		public Graphic SwapChannelsToGRB()
		{
            if(channelFormat!=Format.RGB) return this.ConvertToRGB().SwapChannelsToBRG();
            Graphic ret=new Graphic(width, height, Format.RGB);
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

		public Graphic SwapChannelsToBGR()
		{
            if(channelFormat!=Format.RGB) return this.ConvertToRGB().SwapChannelsToBGR();
            Graphic ret=new Graphic(width, height, Format.RGB);
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

		public Graphic SwapChannelsToGBR()
		{
            if(channelFormat!=Format.RGB) return this.ConvertToRGB().SwapChannelsToBRG();
            Graphic ret=new Graphic(width, height, Format.RGB);
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
