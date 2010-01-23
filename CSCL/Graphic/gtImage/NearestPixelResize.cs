﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Graphic
{
	public partial class gtImage
	{
		/////////////////////////////////////////////////////////////////
		// NearestPixelResize
		/////////////////////////////////////////////////////////////////
		#region NearestPixelResize
		public gtImage NearestPixelResize(uint w, uint h)
		{
			if(width==w&&height==h) return this;
			if((width*height)==0) return new gtImage(0, 0, channelFormat);
			if((w*h)==0) return new gtImage(0, 0, channelFormat);

			if(width==w) return NearestPixelResizeV(h);
			if(height==h) return NearestPixelResizeH(w);
			return NearestPixelResizeVH(w, h);
		}

		gtImage NearestPixelResizeV(uint h)
		{
			double delta=(double)height/h;

			gtImage ret=new gtImage(width, h, channelFormat);
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

		gtImage NearestPixelResizeH(uint w)
		{
			double delta=(double)width/w;

			uint bpp=BytePerPixel;

			uint[] dx=new uint[w];
			for(uint x=0; x<w; x++) dx[x]=(uint)(x*delta+delta/2);

			if(bpp==1)
			{
				gtImage ret=new gtImage(w, height, channelFormat);
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
				gtImage ret=new gtImage(w, height, channelFormat);
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
				gtImage ret=new gtImage(w, height, channelFormat);
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
				gtImage ret=new gtImage(w, height, channelFormat);
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

			return new gtImage(0, 0, channelFormat);
		}

		gtImage NearestPixelResizeVH(uint w, uint h)
		{
			double deltah=(double)height/h;
			double deltaw=(double)width/w;

			uint bpp=BytePerPixel;

			uint[] dx=new uint[w];
			for(uint x=0; x<w; x++) dx[x]=(uint)(x*deltaw+deltaw/2);

			if(bpp==1)
			{
				gtImage ret=new gtImage(w, h, channelFormat);
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
				gtImage ret=new gtImage(w, h, channelFormat);
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
				gtImage ret=new gtImage(w, h, channelFormat);
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
				gtImage ret=new gtImage(w, h, channelFormat);
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

			return new gtImage(0, 0, channelFormat);
		}
		#endregion
	}
}
