//
//  NearestPixelResize.cs
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

namespace CSCL.Imaging
{
	public partial class Graphic
	{
		#region NearestPixelResize
		public Graphic NearestPixelResize(uint w, uint h)
		{
			if(width==w&&height==h) return this;
			if((width*height)==0) return new Graphic(0, 0, channelFormat);
			if((w*h)==0) return new Graphic(0, 0, channelFormat);

			if(width==w) return NearestPixelResizeV(h);
			if(height==h) return NearestPixelResizeH(w);
			return NearestPixelResizeVH(w, h);
		}

		Graphic NearestPixelResizeV(uint h)
		{
			double delta=(double)height/h;

			Graphic ret=new Graphic(width, h, channelFormat);
			if(ret.imageData==null) return ret;

			uint bpp=BytePerPixel;
			uint bw=width*bpp;

			uint dst=0;
			for(uint y=0; y<h; y++)
			{
				uint src=((uint)(y*delta+delta/2))*bw;
				for(uint i=0; i<bw; i++) ret.imageData[dst++]=imageData[src++];
			}
			return ret;
		}

		Graphic NearestPixelResizeH(uint w)
		{
			double delta=(double)width/w;

			uint bpp=BytePerPixel;

			uint[] dx=new uint[w];
			for(uint x=0; x<w; x++) dx[x]=(uint)(x*delta+delta/2);

			if(bpp==1)
			{
				Graphic ret=new Graphic(w, height, channelFormat);
				if(ret.imageData==null) return ret;

				uint dst=0;
				for(uint y=0; y<height; y++)
				{
					uint src=y*width;
					for(uint x=0; x<w; x++) ret.imageData[dst++]=imageData[src+dx[x]];
				}
				return ret;
			}

			if(bpp==4)
			{
				Graphic ret=new Graphic(w, height, channelFormat);
				if(ret.imageData==null) return ret;

				uint bw=width*4;
				uint dst=0;
				for(uint y=0; y<height; y++)
				{
					uint src=y*bw;

					for(uint x=0; x<w; x++)
					{
						uint s=src+dx[x]*4;
						ret.imageData[dst++]=imageData[s++];
						ret.imageData[dst++]=imageData[s++];
						ret.imageData[dst++]=imageData[s++];
						ret.imageData[dst++]=imageData[s];
					}
				}
				return ret;
			}

			if(bpp==3)
			{
				Graphic ret=new Graphic(w, height, channelFormat);
				if(ret.imageData==null) return ret;

				uint bw=width*3;
				uint dst=0;
				for(uint y=0; y<height; y++)
				{
					uint src=y*bw;

					for(uint x=0; x<w; x++)
					{
						uint s=src+dx[x]*3;
						ret.imageData[dst++]=imageData[s++];
						ret.imageData[dst++]=imageData[s++];
						ret.imageData[dst++]=imageData[s];
					}
				}
				return ret;
			}

			if(bpp==2)
			{
				Graphic ret=new Graphic(w, height, channelFormat);
				if(ret.imageData==null) return ret;

				uint bw=width*2;
				uint dst=0;
				for(uint y=0; y<height; y++)
				{
					uint src=y*bw;

					for(uint x=0; x<w; x++)
					{
						uint s=src+dx[x]*2;
						ret.imageData[dst++]=imageData[s++];
						ret.imageData[dst++]=imageData[s];
					}
				}
				return ret;
			}

			return new Graphic(0, 0, channelFormat);
		}

		Graphic NearestPixelResizeVH(uint w, uint h)
		{
			double deltah=(double)height/h;
			double deltaw=(double)width/w;

			uint bpp=BytePerPixel;

			uint[] dx=new uint[w];
			for(uint x=0; x<w; x++) dx[x]=(uint)(x*deltaw+deltaw/2);

			if(bpp==1)
			{
				Graphic ret=new Graphic(w, h, channelFormat);
				if(ret.imageData==null) return ret;

				uint dst=0;
				for(uint y=0; y<h; y++)
				{
					uint src=(uint)(y*deltah+deltah/2)*width;
					for(uint x=0; x<w; x++) ret.imageData[dst++]=imageData[src+dx[x]];
				}
				return ret;
			}

			if(bpp==4)
			{
				Graphic ret=new Graphic(w, h, channelFormat);
				if(ret.imageData==null) return ret;

				uint dst=0;
				uint w4=width*4;
				for(uint y=0; y<h; y++)
				{
					uint src=(uint)(y*deltah+deltah/2)*w4;
					for(uint x=0; x<w; x++)
					{
						uint s=src+dx[x]*4;
						ret.imageData[dst++]=imageData[s++];
						ret.imageData[dst++]=imageData[s++];
						ret.imageData[dst++]=imageData[s++];
						ret.imageData[dst++]=imageData[s];
					}
				}
				return ret;
			}

			if(bpp==3)
			{
				Graphic ret=new Graphic(w, h, channelFormat);
				if(ret.imageData==null) return ret;

				uint dst=0;
				uint w3=width*3;
				for(uint y=0; y<h; y++)
				{
					uint src=(uint)(y*deltah+deltah/2)*w3;
					for(uint x=0; x<w; x++)
					{
						uint s=src+dx[x]*3;
						ret.imageData[dst++]=imageData[s++];
						ret.imageData[dst++]=imageData[s++];
						ret.imageData[dst++]=imageData[s];
					}
				}
				return ret;
			}

			if(bpp==2)
			{
				Graphic ret=new Graphic(w, h, channelFormat);
				if(ret.imageData==null) return ret;

				uint dst=0;
				uint w2=width*2;
				for(uint y=0; y<h; y++)
				{
					uint src=(uint)(y*deltah+deltah/2)*w2;
					for(uint x=0; x<w; x++)
					{
						uint s=src+dx[x]*2;
						ret.imageData[dst++]=imageData[s++];
						ret.imageData[dst++]=imageData[s];
					}
				}
				return ret;
			}

			return new Graphic(0, 0, channelFormat);
		}
		#endregion
	}
}
