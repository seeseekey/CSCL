using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Graphic
{
	public partial class gtImage
	{
		/////////////////////////////////////////////////////////////////
		// ReduceByN
		/////////////////////////////////////////////////////////////////
		#region ReduceByN
		public gtImage ReduceBy(uint m, uint n)
		{
			if(m==1&&n==1) return this;
			if((m*n)==0) return new gtImage(0, 0, channelFormat);
			if((width*height)==0) return new gtImage(0, 0, channelFormat);

			uint wr=width%m, hr=height%n;
			uint wm=width/m, hn=height/n;

			if(wm==0) wm=1;

			if(hn==0) hn=1;

			if(wr!=0)
			{
				if(hr!=0) return Downsample(wm, hn);
				if(n==1) return DownsampleH(wm);
				return ReduceByV(n).DownsampleH(wm); // ReduceBy is usually faster, so ist down first
			}
			if(hr!=0)
			{
				if(m==1) return DownsampleV(hn);
				return ReduceByH(m).DownsampleV(hn); // ReduceBy is usually faster, so ist down first
			}

			if(m==1) return ReduceByV(n);
			if(n==1) return ReduceByH(m);

			if((hn*width)>(wm*height)) return ReduceByH(m).ReduceByV(n);
			return ReduceByV(n).ReduceByH(m);
		}

		public gtImage ReduceBy(uint m)
		{
			if(m==1) return this;
			if(m==0) return new gtImage(0, 0, channelFormat);
			if((width*height)==0) return new gtImage(0, 0, channelFormat);

			uint wr=width%m, hr=height%m;
			uint wm=width/m, hn=height/m;

			if(wm==0) wm=1;

			if(hn==0) hn=1;

			if(wr!=0)
			{
				if(hr!=0) return Downsample(wm, hn);
				if(m==1) return DownsampleH(wm);
				return ReduceByV(m).DownsampleH(wm); // ReduceBy is usually faster, so ist down first
			}
			if(hr!=0)
			{
				if(m==1) return DownsampleV(hn);
				return ReduceByH(m).DownsampleV(hn); // ReduceBy is usually faster, so ist down first
			}

			if((hn*width)>(wm*height)) return ReduceByH(m).ReduceByV(m);
			return ReduceByV(m).ReduceByH(m);
		}

		gtImage ReduceByV(uint n)
		{
			if((width*height)==0) return new gtImage(0, 0, channelFormat);
			uint h=height/n;

			if(channelFormat==Format.GRAY)
			{
				gtImage ret=new gtImage(width, h, channelFormat);
				if(ret.imageData==null) return ret;

				for(uint x=0; x<width; x++)
				{
					byte[] dst=ret.imageData;
					uint ind=x;
					byte[] src=imageData;
					uint inds=x;
					for(uint y=0; y<h; y++)
					{
						uint sum=0;
						for(uint z=0; z<n; z++) { sum+=src[inds]; inds+=width; }

						dst[ind]=(byte)(sum/n);
						ind+=width;
					}
				}
				return ret;
			}

			if(channelFormat==Format.RGB||channelFormat==Format.BGR)
			{
				gtImage ret=new gtImage(width, h, channelFormat);
				if(ret.imageData==null) return ret;

				uint wb=3*width-2;
				for(uint x=0; x<width; x++)
				{
					byte[] dst=ret.imageData;
					uint ind=x*3;
					byte[] src=imageData;
					uint inds=x*3;
					for(uint y=0; y<h; y++)
					{
						uint sumr=0, sumg=0, sumb=0;
						for(uint z=0; z<n; z++)
						{
							sumr+=src[inds++];
							sumg+=src[inds++];
							sumb+=src[inds];
							inds+=wb;
						}

						dst[ind++]=(byte)(sumr/n);
						dst[ind++]=(byte)(sumg/n);
						dst[ind]=(byte)(sumb/n);
						ind+=wb;
					}
				}
				return ret;
			}

			if(channelFormat==Format.RGBA||channelFormat==Format.BGRA)
			{
				gtImage ret=new gtImage(width, h, channelFormat);
				if(ret.imageData==null) return ret;

				uint wb=4*width-3;
				for(uint x=0; x<width; x++)
				{
					byte[] dst=ret.imageData;
					uint ind=x*4;
					byte[] src=imageData;
					uint inds=x*4;
					for(uint y=0; y<h; y++)
					{
						uint sumr=0, sumg=0, sumb=0, suma=0;
						for(uint z=0; z<n; z++)
						{
							byte r=src[inds++];
							byte g=src[inds++];
							byte b=src[inds++];
							uint a=src[inds];
							inds+=wb;
							sumr+=r*a;
							sumg+=g*a;
							sumb+=b*a;
							suma+=a;
						}

						if(suma==0)
						{
							dst[ind++]=0;
							dst[ind++]=0;
							dst[ind++]=0;
							dst[ind]=0;
						}
						else
						{
							dst[ind++]=(byte)(sumr/suma);
							dst[ind++]=(byte)(sumg/suma);
							dst[ind++]=(byte)(sumb/suma);
							dst[ind]=(byte)(suma/n);
						}

						ind+=wb;
					}
				}
				return ret;
			}

			if(channelFormat==Format.GRAY_Alpha)
			{
				gtImage ret=new gtImage(width, h, channelFormat);
				if(ret.imageData==null) return ret;

				uint wb=2*width-1;
				for(uint x=0; x<width; x++)
				{
					byte[] dst=ret.imageData;
					uint ind=x*2;
					byte[] src=imageData;
					uint inds=x*2;
					for(uint y=0; y<h; y++)
					{
						uint sumg=0, suma=0;
						for(uint z=0; z<n; z++)
						{
							byte g=src[inds++];
							uint a=src[inds];
							inds+=wb;
							sumg+=g*a;
							suma+=a;
						}

						if(suma==0)
						{
							dst[ind++]=0;
							dst[ind]=0;
						}
						else
						{
							dst[ind++]=(byte)(sumg/suma);
							dst[ind]=(byte)(suma/n);
						}

						ind+=wb;
					}
				}
				return ret;
			}

			return new gtImage(0, 0, channelFormat);
		}

		gtImage ReduceByH(uint m)
		{
			if((width*height)==0) return new gtImage(0, 0, channelFormat);
			uint w=width/m;
			uint wh=w*height;

			if(channelFormat==Format.GRAY)
			{
				gtImage ret=new gtImage(w, height, channelFormat);
				if(ret.imageData==null) return ret;

				byte[] dst=ret.imageData;
				uint ind=0;
				byte[] src=imageData;
				uint inds=0;
				for(uint y=0; y<wh; y++)
				{
					uint sum=0;
					for(uint z=0; z<m; z++)
						sum+=src[inds++];
					dst[ind++]=(byte)(sum/m);
				}
				return ret;
			}

			if(channelFormat==Format.RGB||channelFormat==Format.BGR)
			{
				gtImage ret=new gtImage(w, height, channelFormat);
				if(ret.imageData==null) return ret;

				byte[] dst=ret.imageData;
				uint ind=0;
				byte[] src=imageData;
				uint inds=0;
				for(uint y=0; y<wh; y++)
				{
					uint sumr=0, sumg=0, sumb=0;
					for(uint z=0; z<m; z++)
					{
						sumr+=src[inds++];
						sumg+=src[inds++];
						sumb+=src[inds++];
					}
					dst[ind++]=(byte)(sumr/m);
					dst[ind++]=(byte)(sumg/m);
					dst[ind++]=(byte)(sumb/m);
				}
				return ret;
			}

			if(channelFormat==Format.RGBA||channelFormat==Format.BGRA)
			{
				gtImage ret=new gtImage(w, height, channelFormat);
				if(ret.imageData==null) return ret;

				byte[] dst=ret.imageData;
				uint ind=0;
				byte[] src=imageData;
				uint inds=0;
				for(uint y=0; y<wh; y++)
				{
					uint sumr=0, sumg=0, sumb=0, suma=0;
					for(uint z=0; z<m; z++)
					{
						byte r=src[inds++];
						byte g=src[inds++];
						byte b=src[inds++];
						uint a=src[inds++];
						sumr+=r*a;
						sumg+=g*a;
						sumb+=b*a;
						suma+=a;
					}

					if(suma==0)
					{
						dst[ind++]=0;
						dst[ind++]=0;
						dst[ind++]=0;
						dst[ind++]=0;
					}
					else
					{
						dst[ind++]=(byte)(sumr/suma);
						dst[ind++]=(byte)(sumg/suma);
						dst[ind++]=(byte)(sumb/suma);
						dst[ind++]=(byte)(suma/m);
					}
				}
				return ret;
			}

			if(channelFormat==Format.GRAY_Alpha)
			{
				gtImage ret=new gtImage(w, height, channelFormat);
				if(ret.imageData==null) return ret;

				byte[] dst=ret.imageData;
				uint ind=0;
				byte[] src=imageData;
				uint inds=0;
				for(uint y=0; y<wh; y++)
				{
					uint sumg=0, suma=0;
					for(uint z=0; z<m; z++)
					{
						byte g=src[inds++];
						uint a=src[inds++];
						sumg+=g*a;
						suma+=a;
					}

					if(suma==0)
					{
						dst[ind++]=0;
						dst[ind++]=0;
					}
					else
					{
						dst[ind++]=(byte)(sumg/suma);
						dst[ind++]=(byte)(suma/m);
					}
				}
				return ret;
			}

			return new gtImage(0, 0, channelFormat);
		}
		#endregion

		/////////////////////////////////////////////////////////////////
		// DownSampling
		/////////////////////////////////////////////////////////////////
		#region Downsampling
		public gtImage Downsample(uint w, uint h)
		{
			if(width<w||height<h)
				throw new Exception("Don't upsample an image using 'Downsample()'");

			if(width==w&&height==h) return this;
			if((width*height)==0) return new gtImage(0, 0, channelFormat);
			if((w*h)==0) return new gtImage(0, 0, channelFormat);

			if(width!=w)
			{
				if(height!=h)
				{
					if((height*w)>(width*h)) return DownsampleV(h).DownsampleH(w);
					return DownsampleH(w).DownsampleV(h);
				}
				return DownsampleH(w);
			}
			return DownsampleV(h);
		}

		unsafe gtImage DownsampleV(uint h)
		{
			if((width*height)==0) return new gtImage(0, 0, channelFormat);
			if(h==0) return new gtImage(0, 0, channelFormat);

			if(height>h&&height%h==0) return ReduceByV(height/h);
			double delta=((double)height)/h;

			if(channelFormat==Format.GRAY)
			{
				gtImage ret=new gtImage(width, h, channelFormat);
				if(ret.imageData==null) return ret;

				for(uint x=0; x<width; x++)
				{
					fixed(byte* _dst=ret.imageData, _src=imageData)
					{
						byte* dst=_dst+x;
						byte* src=_src+x;

						for(uint y=0; y<h; y++)
						{
							double deltay=y*delta;
							double dy=1-(deltay-((uint)deltay));
							byte* s=src+((uint)deltay)*width;
							double deltasum=dy;

							double gsum=*s*dy; s+=width;

							while((delta-deltasum)>0.0001)
							{
								dy=delta-deltasum;
								if(dy>=1)
								{
									deltasum+=1;
									gsum+=*s; s+=width;
								}
								else
								{
									gsum+=*s*dy;
									break;
								}
							}

							*dst=(byte)(gsum/delta+0.5); dst+=width;
						}
					}
				}
				return ret;
			}

			if(channelFormat==Format.RGB||channelFormat==Format.BGR)
			{
				gtImage ret=new gtImage(width, h, channelFormat);
				if(ret.imageData==null) return ret;

				int wb=(int)width*3;
				int wb2=wb-2;
				for(uint x=0; x<width; x++)
				{
					fixed(byte* _dst=ret.imageData, _src=imageData)
					{
						byte* dst=_dst+x*3;
						byte* src=_src+x*3;

						for(uint y=0; y<h; y++)
						{
							double deltay=y*delta;
							double dy=1-(deltay-((uint)deltay));
							byte* s=src+((uint)deltay)*wb;
							double deltasum=dy;

							double rsum=*(s++)*dy;
							double gsum=*(s++)*dy;
							double bsum=*s*dy; s+=wb2;

							while((delta-deltasum)>0.0001)
							{
								dy=delta-deltasum;
								if(dy>=1)
								{
									deltasum+=1;
									rsum+=*(s++);
									gsum+=*(s++);
									bsum+=*s; s+=wb2;
								}
								else
								{
									rsum+=*(s++)*dy;
									gsum+=*(s++)*dy;
									bsum+=*s*dy;
									break;
								}
							}

							*(dst++)=(byte)(rsum/delta+0.5);
							*(dst++)=(byte)(gsum/delta+0.5);
							*dst=(byte)(bsum/delta+0.5); dst+=wb2;
						}
					}
				}
				return ret;
			}

			if(channelFormat==Format.RGBA||channelFormat==Format.BGRA)
			{
				gtImage ret=new gtImage(width, h, channelFormat);
				if(ret.imageData==null) return ret;

				int wb=(int)width*4;
				int wb3=wb-3;
				for(uint x=0; x<width; x++)
				{
					fixed(byte* _dst=ret.imageData, _src=imageData)
					{
						byte* dst=_dst+x*4;
						byte* src=_src+x*4;

						for(uint y=0; y<h; y++)
						{
							double deltay=y*delta;
							double dy=1-(deltay-((uint)deltay));
							byte* s=src+((uint)deltay)*wb;
							double deltasum=dy;

							byte r=*(s++), g=*(s++), b=*(s++);
							uint a=*s; s+=wb3;

							double ady=a*dy;
							double rsum=r*ady;
							double gsum=g*ady;
							double bsum=b*ady;
							double asum=ady;

							while((delta-deltasum)>0.0001)
							{
								r=*(s++); g=*(s++); b=*(s++); a=*s; s+=wb3;

								dy=delta-deltasum;
								if(dy>=1)
								{
									deltasum+=1;
									rsum+=r*a;
									gsum+=g*a;
									bsum+=b*a;
									asum+=a;
								}
								else
								{
									ady=a*dy;
									rsum+=r*ady;
									gsum+=g*ady;
									bsum+=b*ady;
									asum+=ady;
									break;
								}
							}

							*(dst++)=(byte)(rsum/asum+0.5);
							*(dst++)=(byte)(gsum/asum+0.5);
							*(dst++)=(byte)(bsum/asum+0.5);
							*dst=(byte)(asum/delta+0.5); dst+=wb3;
						}
					}
				}
				return ret;
			}

			if(channelFormat==Format.GRAY_Alpha)
			{
				gtImage ret=new gtImage(width, h, channelFormat);
				if(ret.imageData==null) return ret;

				int wb=(int)width*2;
				int wb1=wb-1;
				for(uint x=0; x<width; x++)
				{
					fixed(byte* _dst=ret.imageData, _src=imageData)
					{
						byte* dst=_dst+x*2;
						byte* src=_src+x*2;

						for(uint y=0; y<h; y++)
						{
							double deltay=y*delta;
							double dy=1-(deltay-((uint)deltay));
							byte* s=src+((uint)deltay)*wb;
							double deltasum=dy;

							byte g=*(s++); uint a=*s; s+=wb1;

							double ady=a*dy;
							double gsum=g*ady;
							double asum=ady;

							while((delta-deltasum)>0.0001)
							{
								g=*(s++); a=*s; s+=wb1;

								dy=delta-deltasum;
								if(dy>=1)
								{
									deltasum+=1;
									gsum+=g*a;
									asum+=a;
								}
								else
								{
									ady=a*dy;
									gsum+=g*ady;
									asum+=ady;
									break;
								}
							}

							*(dst++)=(byte)(gsum/asum+0.5);
							*dst=(byte)(asum/delta+0.5); dst+=wb1;
						}
					}
				}
				return ret;
			}

			return new gtImage(0, 0, channelFormat);
		}

		unsafe gtImage DownsampleH(uint w)
		{
			if((width*height)==0) return new gtImage(0, 0, channelFormat);
			if(w==0) return new gtImage(0, 0, channelFormat);

			if(width>w&&width%w==0) return ReduceByH(width/w);

			double delta=((double)width)/w;

			if(channelFormat==Format.GRAY)
			{
				gtImage ret=new gtImage(w, height, channelFormat);
				if(ret.imageData==null) return ret;

				fixed(byte* _dst=ret.imageData, _src=imageData)
				{
					byte* dst=_dst;
					for(uint y=0; y<height; y++)
					{
						byte* src=_src+y*width;

						for(uint x=0; x<w; x++)
						{
							double deltax=x*delta;
							double dx=1-(deltax-((uint)deltax));
							byte* s=src+((uint)deltax);
							double deltasum=dx;

							double gsum=*(s++)*dx;

							while((delta-deltasum)>0.0001)
							{
								dx=delta-deltasum;
								if(dx>=1)
								{
									deltasum+=1;
									gsum+=*(s++);
								}
								else
								{
									gsum+=*s*dx;
									break;
								}
							}

							*(dst++)=(byte)(gsum/delta+0.5);
						}
					}
				}
				return ret;
			}

			if(channelFormat==Format.RGB||channelFormat==Format.BGR)
			{
				gtImage ret=new gtImage(w, height, channelFormat);
				if(ret.imageData==null) return ret;

				fixed(byte* _dst=ret.imageData, _src=imageData)
				{
					byte* dst=_dst;
					uint wb=width*3;
					for(uint y=0; y<height; y++)
					{
						byte* src=_src+y*wb;

						for(uint x=0; x<w; x++)
						{
							double deltax=x*delta;
							double dx=1-(deltax-((uint)deltax));
							byte* s=src+((uint)deltax)*3;
							double deltasum=dx;

							double rsum=*(s++)*dx;
							double gsum=*(s++)*dx;
							double bsum=*(s++)*dx;

							while((delta-deltasum)>0.0001)
							{
								dx=delta-deltasum;
								if(dx>=1)
								{
									deltasum+=1;
									rsum+=*(s++);
									gsum+=*(s++);
									bsum+=*(s++);
								}
								else
								{
									rsum+=*(s++)*dx;
									gsum+=*(s++)*dx;
									bsum+=*s*dx;
									break;
								}
							}

							*(dst++)=(byte)(rsum/delta+0.5);
							*(dst++)=(byte)(gsum/delta+0.5);
							*(dst++)=(byte)(bsum/delta+0.5);
						}
					}
				}
				return ret;
			}

			if(channelFormat==Format.RGBA||channelFormat==Format.BGRA)
			{
				gtImage ret=new gtImage(w, height, channelFormat);
				if(ret.imageData==null) return ret;

				fixed(byte* _dst=ret.imageData, _src=imageData)
				{
					byte* dst=_dst;
					uint wb=width*4;
					for(uint y=0; y<height; y++)
					{
						byte* src=_src+y*wb;

						for(uint x=0; x<w; x++)
						{
							double deltax=x*delta;
							double dx=1-(deltax-((uint)deltax));
							byte* s=src+((uint)deltax)*4;
							double deltasum=dx;

							byte r=*(s++), g=*(s++), b=*(s++);
							uint a=*(s++);

							double adx=a*dx;
							double rsum=r*adx;
							double gsum=g*adx;
							double bsum=b*adx;
							double asum=adx;

							while((delta-deltasum)>0.0001)
							{
								dx=delta-deltasum;
								r=*(s++); g=*(s++); b=*(s++); a=*(s++);
								if(dx>=1)
								{
									deltasum+=1;
									rsum+=r*a;
									gsum+=g*a;
									bsum+=b*a;
									asum+=a;
								}
								else
								{
									adx=a*dx;
									rsum+=r*adx;
									gsum+=g*adx;
									bsum+=b*adx;
									asum+=adx;
									break;
								}
							}

							*(dst++)=(byte)(rsum/asum+0.5);
							*(dst++)=(byte)(gsum/asum+0.5);
							*(dst++)=(byte)(bsum/asum+0.5);
							*(dst++)=(byte)(asum/delta+0.5);
						}
					}
				}
				return ret;
			}

			if(channelFormat==Format.GRAY_Alpha)
			{
				gtImage ret=new gtImage(w, height, channelFormat);
				if(ret.imageData==null) return ret;

				fixed(byte* _dst=ret.imageData, _src=imageData)
				{
					byte* dst=_dst;
					uint wb=width*2;
					for(uint y=0; y<height; y++)
					{
						byte* src=_src+y*wb;

						for(uint x=0; x<w; x++)
						{
							double deltax=x*delta;
							double dx=1-(deltax-((uint)deltax));
							byte* s=src+((uint)deltax)*2;
							double deltasum=dx;

							byte g=*(s++);
							uint a=*(s++);

							double gsum=g*dx*a;
							double asum=a*dx;

							while((delta-deltasum)>0.0001)
							{
								dx=delta-deltasum;
								g=*(s++); a=*(s++);
								if(dx>=1)
								{
									deltasum+=1;
									gsum+=g*a;
									asum+=a;
								}
								else
								{
									double adx=a*dx;
									gsum+=g*adx;
									asum+=adx;
									break;
								}
							}

							*(dst++)=(byte)(gsum/asum+0.5);
							*(dst++)=(byte)(asum/delta+0.5);
						}
					}
				}
				return ret;
			}

			return new gtImage(0, 0, channelFormat);
		}
		#endregion
	}
}
