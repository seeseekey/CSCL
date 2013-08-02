//
//  Transform.cs
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
		#region Flip & Co.
		public gtImage ToFlippedHorizontal()
		{
			gtImage ret=new gtImage(width, height, channelFormat);
			if(ret.imageData==null) return ret;

			uint bw=width*BytePerPixel;

			uint src=0;
			uint dst=height*bw;

			for(uint y=0; y<height; y++)
			{
				dst-=bw;
				for(uint x=0; x<bw; x++) ret.imageData[dst++]=imageData[src++];
				dst-=bw;
			}

			return ret;
		}

		public gtImage ToFlippedVertical()
		{
			gtImage ret=new gtImage(width, height, channelFormat);
			if(ret.imageData==null) return ret;

			uint bpp=BytePerPixel;
			uint bw=width*bpp;

			uint src=0;
			uint dst=bw;
			dst-=bpp;

			for(uint y=0; y<height; y++)
			{
				for(uint x=0; x<width; x++)
				{
					for(uint i=0; i<bpp; i++) ret.imageData[dst++]=imageData[src++];
					dst-=2*bpp;
				}
				dst+=2*bw;
			}

			return ret;
		}

		public gtImage ToRot90()
		{
			gtImage ret=new gtImage(height, width, channelFormat);
			if(ret.imageData==null) return ret;

			uint bpp=BytePerPixel;
			uint bw=height*bpp;

			uint src=0;
			uint dst_=(width-1)*bw;

			for(uint y=0; y<height; y++)
			{
				uint dst=dst_;
				for(uint x=0; x<width; x++)
				{
					for(uint i=0; i<bpp; i++) ret.imageData[dst++]=imageData[src++];
					dst-=bw+bpp;
				}
				dst_+=bpp;
			}
			return ret;
		}

		public gtImage ToRot180()
		{
			gtImage ret=new gtImage(width, height, channelFormat);
			if(ret.imageData==null) return ret;

			uint bpp=BytePerPixel;
			uint bw=width*bpp;

			uint src=0;
			uint dst=height*bw;
			dst-=bpp;

			for(uint y=0; y<height; y++)
			{
				for(uint x=0; x<width; x++)
				{
					for(uint i=0; i<bpp; i++) ret.imageData[dst++]=imageData[src++];
					dst-=2*bpp;
				}
			}

			return ret;
		}

		public gtImage ToRot270()
		{
			gtImage ret=new gtImage(height, width, channelFormat);
			if(ret.imageData==null) return ret;

			uint bpp=BytePerPixel;
			uint bw=width*bpp;

			uint dst=0;
			uint src_=(height-1)*bw;

			for(uint y=0; y<width; y++)
			{
				uint src=src_;
				for(uint x=0; x<height; x++)
				{
					for(uint i=0; i<bpp; i++) ret.imageData[dst++]=imageData[src++];
					src-=bw+bpp;
				}
				src_+=bpp;
			}
			return ret;
		}
		#endregion
	}
}
