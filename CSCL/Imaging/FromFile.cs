//
//  FromFile.cs
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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using CSCL;

namespace CSCL.Imaging
{
	public partial class Graphic
	{
		public static string FromFileFilter
		{
			get
			{
				return "Alle Dateien|*.png;*.jpg;*.jpeg;*.jpe;*.jif;*.jfi;*.jfif;*.psi;*.pmi;*.ljpg;*.tga;*.bmp;*.dib;"
				+"|PNG Dateien|*.png"
				+"|JPEG Dateien|*.jpg;*.jpeg;*.jpe;*.jif;*.jfi;*.jfif;*.psi;*.pmi"
				+"|TGA Dateien|*.tga"
				+"|BMP Dateien|*.bmp"
				+"|DIB Dateien|*.dib";
			}
		}

		public static List<string> FromFileFilterList
		{
			get
			{
				List<string> ret=new List<string>();

				ret.Add("png");
				ret.Add("jpg");
				ret.Add("jpeg");
				ret.Add("jpe");
				ret.Add("jif");
				ret.Add("jfi");
				ret.Add("jfif");
				ret.Add("psi");
				ret.Add("pmi");
				ret.Add("ljpg");
				ret.Add("tga");
				ret.Add("bmp");
				ret.Add("dib");
				return ret;
			}
		}

		#region FromFile
		public static Graphic FromFile(string filename)
		{
			if(filename.Length<5) return null;

			string ext=FileSystem.GetExtension(filename).ToLower();

			if(ext=="tga") return FromTGAFile(filename);
			if(ext=="bmp") return FromBMPFile(filename);

			return FromGDIFile(filename);
		}
		#endregion

		#region FromTGAFile
		public static Graphic FromTGAFile(string filename)
		{
			Graphic ret=null;

			using(FileStream fs=new FileStream(filename, FileMode.Open, FileAccess.Read))
			{
				uint fsize=(uint)fs.Length;
				if(fsize<19) throw new EndOfStreamException("File to small to be a Truevision Image.");

				BinaryReader br=new BinaryReader(fs);

				// Read Header (18 bytes)
				byte ID_Length=br.ReadByte();
				byte Color_Map_Type=br.ReadByte();
				byte Image_Type=br.ReadByte();
				ushort First_Entry_Index=br.ReadUInt16();
				ushort Color_Map_Length=br.ReadUInt16();
				byte Color_Map_Entry_Size=br.ReadByte();
				ushort X_Origin=br.ReadUInt16();
				ushort Y_Origin=br.ReadUInt16();
				ushort Width=br.ReadUInt16();
				ushort Height=br.ReadUInt16();
				byte Pixel_Depth=br.ReadByte();
				byte Image_Descriptor=br.ReadByte();

				// Offsets
				//uint pImage_ID=0;
				uint pImage_Data=0;
				uint pColor_Map_Data=0;
				uint pDeveloper_Directory=0;
				uint pExtension_Area=0;
				uint pScan_Line_Table=0;			// *Field 25 == Field 23 // Height*4 bytes

				// Vars
				uint width=0, height=0;
				uint bpp=0;	// 8 | 16 | 24 | 32
				bool isAlpha=false;
				bool hor_flipped=false, ver_flipped=false, rle=false;

				uint[] scan_line_table;

				//if(ID_Length!=0)
				//	pImage_ID=18;
				pImage_Data=18u+ID_Length;
				if(Color_Map_Type!=0)
				{
					pColor_Map_Data=18u+ID_Length;
					if(Color_Map_Length==0) throw new Exception("Color Map Type=1, but Length=0.");

					if(Color_Map_Entry_Size==15||Color_Map_Entry_Size==16) pImage_Data+=Color_Map_Length*2u;
					else if(Color_Map_Entry_Size==24) pImage_Data+=Color_Map_Length*3u;
					else if(Color_Map_Entry_Size==32) pImage_Data+=Color_Map_Length*4u;
					else throw new Exception("Illegal Color Map Entry Size.");
				}

				rle=false;
				switch(Image_Type)
				{
					case 10:
						{
							rle=true;
							if(Pixel_Depth==16) bpp=2;
							else if(Pixel_Depth==24) bpp=3;
							else if(Pixel_Depth==32) { bpp=4; isAlpha=true; }
							else throw new Exception("Illegal or unsupported Pixel Depth (RGB).");
						}
						break;
					case 2:
						{
							if(Pixel_Depth==16) bpp=2;
							else if(Pixel_Depth==24) bpp=3;
							else if(Pixel_Depth==32) { bpp=4; isAlpha=true; }
							else throw new Exception("Illegal or unsupported Pixel Depth (RGB).");
						}
						break;
					case 9:
						{
							rle=true;
							if(Pixel_Depth==8) bpp=1;
							else if(Pixel_Depth==16) { bpp=2; isAlpha=true; }
							else throw new Exception("Illegal or unsupported Pixel Depth (GRAY).");
						}
						break;
					case 11:
						{
							rle=true;
							if(Pixel_Depth==8) bpp=1;
							else if(Pixel_Depth==16) { bpp=2; isAlpha=true; }
							else throw new Exception("Illegal or unsupported Pixel Depth (GRAY).");
						}
						break;
					case 1:
						{
							if(Pixel_Depth==8) bpp=1;
							else if(Pixel_Depth==16) { bpp=2; isAlpha=true; }
							else throw new Exception("Illegal or unsupported Pixel Depth (GRAY).");
						}
						break;
					case 3:
						{
							if(Pixel_Depth==8) bpp=1;
							else if(Pixel_Depth==16) { bpp=2; isAlpha=true; }
							else throw new Exception("Illegal or unsupported Pixel Depth (GRAY).");
						}
						break;
					default:
						throw new Exception("Illegal or unsupported Image Type.");
				}

				if(Width==0) throw new Exception("Illegal Image Width.");
				if(Height==0) throw new Exception("Illegal Image Height.");
				width=Width;
				height=Height;

				if((Image_Descriptor&0x0F)!=(isAlpha?8:0)) throw new Exception("Illegal Alpha Channel Bits Count.");
				if((Image_Descriptor&0xC0)!=0) throw new Exception("Unsed Bits in Image Description not zero.");

				hor_flipped=(Image_Descriptor&0x20)!=0;
				ver_flipped=(Image_Descriptor&0x10)!=0;

				// checken ob pImage_Data-Offsets in File
				if((pImage_Data+height)>=fsize) throw new Exception("File to small to fold the complete Image Data.");

				// Extension Format
				fs.Seek(fsize-26, SeekOrigin.Begin);
				pExtension_Area=br.ReadUInt32();
				pDeveloper_Directory=br.ReadUInt32();
				String sig=br.ReadChars(17).ToString();

				scan_line_table=new uint[height];

				if(sig=="TRUEVISION-XFILE.")	// in der Hoffung, dass nicht zufällig
				{ // diese Zeichenfolge in der Pixeldaten steht
					if(pExtension_Area!=0)
					{
						fs.Seek(pExtension_Area, SeekOrigin.Begin);
						pScan_Line_Table=0;
						ushort Extension_Size=br.ReadUInt16();

						if(Extension_Size>=495)
						{
							fs.Seek(pExtension_Area+490, SeekOrigin.Begin);
							pScan_Line_Table=br.ReadUInt32();
						}

						if(pScan_Line_Table!=0)
						{
							fs.Seek(pScan_Line_Table, SeekOrigin.Begin);
							for(int i=0; i<height; i++) scan_line_table[i]=br.ReadUInt32();
						}
					}
				}
				else pExtension_Area=pDeveloper_Directory=0;

				if(pScan_Line_Table==0)
				{
					if(!rle)
					{
						for(uint i=0; i<height; i++) scan_line_table[i]=pImage_Data+i*width*bpp;
					}
					else
					{
						fs.Seek(pImage_Data, SeekOrigin.Begin);

						for(uint i=0; i<height; i++)
						{
							scan_line_table[i]=(uint)fs.Position;

							uint internalwidth=0;
							while(width>internalwidth)
							{
								try
								{
									byte ph=br.ReadByte();
									uint count=(uint)((ph&0x7F)+1);
									if((ph&0x80)>0)
									{ // rle packet
										if(br.ReadBytes((int)bpp).Length<bpp) throw new Exception("Error reading rle-packed Image Data.");
									}
									else
									{ // raw packet
										if(br.ReadBytes((int)(count*bpp)).Length<(count*bpp)) throw new Exception("Error reading rle-packed Image Data.");
									}
									internalwidth+=count;
								}
								catch(Exception)
								{
									throw new EndOfStreamException("Error reading rle-packed Image Data.");
								}
							}
							if(internalwidth>width) throw new Exception("Error reading rle-packed Image Data. (Line too long.)");
						}
					}
				}

				uint bpp_=bpp;

				if(!isAlpha&&bpp==2) bpp_=3; // RGB24 statt RGB15

				uint lineLen=width*bpp_;

				byte[] bits=new byte[height*lineLen];

				//for(int i=0; i<lineLen*height; i++) bits[i]=0; // Ziel säubern

				for(uint y=0; y<height; y++)
				{
					uint p=y*lineLen;
					uint dest=p;

					uint end=width;

					if(ver_flipped) dest-=(width+1)*bpp_;

					uint startOfLine;
					if(hor_flipped) startOfLine=scan_line_table[y];
					else startOfLine=scan_line_table[height-y-1];

					if(!rle)
					{
						uint blocksize=end*bpp;
						fs.Seek(startOfLine, SeekOrigin.Begin);
						byte[] buffer=br.ReadBytes((int)blocksize);
						if(buffer.Length<blocksize) throw new EndOfStreamException();

						int ind=0;
						for(uint myX=0; myX<end; myX++)
						{
							if(bpp<3)
							{ // 8 oder 16 Bit
								if(bpp==1||isAlpha)
								{// GRAY8
									bits[dest++]=buffer[ind++];
									if(isAlpha) bits[dest++]=buffer[ind++];
								}
								else
								{
									ushort w1=buffer[ind++];
									ushort w2=buffer[ind++];
									ushort w=(ushort)((w2<<8)&w1);
									ushort r=(ushort)(w&0x7c00); r>>=7;
									ushort g=(ushort)(w&0x03e0); g>>=2;
									ushort b=(ushort)(w&0x001f); b<<=3;
									bits[dest++]=(byte)b;
									bits[dest++]=(byte)g;
									bits[dest++]=(byte)r;
								}
							}
							else
							{
								bits[dest++]=buffer[ind++];
								bits[dest++]=buffer[ind++];
								bits[dest++]=buffer[ind++];
								if(bpp>3) bits[dest++]=buffer[ind++];
							}

							if(ver_flipped) dest-=2*bpp_;
						}
					}
					else
					{
						fs.Seek(startOfLine, SeekOrigin.Begin);

						byte[] buffer=new byte[width*bpp];
						uint ind=0;

						uint internalwidth=0;
						while(width>internalwidth)
						{
							try
							{
								byte ph=br.ReadByte();
								uint count=(uint)((ph&0x7F)+1);
								if((ph&0x80)>0)
								{ // rle packet
									byte[] tbuffer=br.ReadBytes((int)bpp);
									if(tbuffer.Length<bpp) throw new Exception("Error reading rle-packed Image Data.");

									for(uint i=0; i<count; i++)
									{
										tbuffer.CopyTo(buffer, ind);
										ind+=bpp;
									}
								}
								else
								{ // raw packet
									byte[] tbuffer=br.ReadBytes((int)(count*bpp));
									if(tbuffer.Length<(count*bpp)) throw new Exception("Error reading rle-packed Image Data.");
									tbuffer.CopyTo(buffer, ind);
									ind+=count*bpp;
								}
								internalwidth+=count;
							}
							catch(Exception)
							{
								throw new EndOfStreamException("Error reading rle-packed Image Data.");
							}
						}

						if(internalwidth>width) throw new EndOfStreamException("Error reading rle-packed Image Data. (Line too long.)");

						ind=0;
						for(uint myX=0; myX<end; myX++)
						{
							if(bpp<3)
							{ // 8 oder 16 Bit
								if(bpp==1||isAlpha)
								{// GRAY8
									bits[dest++]=buffer[ind++];
									if(isAlpha) bits[dest++]=buffer[ind++];
								}
								else
								{
									ushort w1=buffer[ind++];
									ushort w2=buffer[ind++];
									ushort w=(ushort)((w2<<8)&w1);
									ushort r=(ushort)(w&0x7c00); r>>=7;
									ushort g=(ushort)(w&0x03e0); g>>=2;
									ushort b=(ushort)(w&0x001f); b<<=3;
									bits[dest++]=(byte)b;
									bits[dest++]=(byte)g;
									bits[dest++]=(byte)r;
								}
							}
							else
							{
								bits[dest++]=buffer[ind++];
								bits[dest++]=buffer[ind++];
								bits[dest++]=buffer[ind++];
								if(bpp>3)
									bits[dest++]=buffer[ind++];
							}

							if(ver_flipped) dest-=2*bpp_;
						}
					}
				}

				ret=new Graphic();

				switch(bpp_)
				{
					case 1: ret.channelFormat=Format.GRAY; break;
					case 2: ret.channelFormat=Format.GRAYAlpha; break;
					case 3: ret.channelFormat=Format.RGB; break;
					case 4: ret.channelFormat=Format.RGBA; break;
					default: ret.channelFormat=Format.GRAY; break;
				}

				ret.imageData=bits;
				ret.width=width;
				ret.height=height;

				br.Close();
				fs.Close();
			} // using(br)

			return ret;
		}
		#endregion

		#region FromBMPFile
		enum BMPBiCompression { BI_RGB=0, BI_RLE8=1, BI_RLE4=2, BI_BITFIELDS=3, BI_JPEG=4, BI_PNG=5, BI_ALPHABITFIELDS=6 }

		public static Graphic FromBMPFile(string filename)
		{
			#region Variablen
			// Header
			int bfSize;			// Größe der BMP-Datei in Byte. (unzuverlässig)
			int bfOffBits;		// Offset der Bilddaten in Byte vom Beginn der Datei an.

			// Infoblock
			uint biSize;			// Größe des Informationsblocks in Byte
			int biWidth;			// Breite der Bitmap in Pixel.
			int biHeight;			// Höhe der Bitmap in Pixel
			short biPlanes;			// Bei PCX die Anzahl der Farbebenen bei BMP nicht verwendet (immer 1)
			short biBitCount;		// Farbtiefe (1, 4, 8, 16, 24, 32 Bit)
			BMPBiCompression biCompression;		// Compressionsmethod
			uint biSizeImage;		// Größe der Bilddaten (oder 0)
			int biXPelsPerMeter;	// Horizontale Auflösung des Zielausgabegerätes in Pixel pro Meter 
			int biYPelsPerMeter;	// Vertikale Auflösung des Zielausgabegerätes in Pixel pro Meter 
			uint biClrUsed;			// Colors
			uint biClrImportant;	// Important Color

			// Weitere Dinge für den Infoblock
			uint bmRed=0;	// Farbmaske Rot
			uint bmGreen=0;	// Farbmaske Grün
			uint bmBlue=0;	// Farbmaske Blau
			uint bmAlpha=0;	// Farbmaske Alpha

			// Farbtabelle
			List<Color> ColorTable=new List<Color>();
			#endregion

			// Datei öffnen
			BinaryReader fileReader=new BinaryReader(File.OpenRead(filename));

			try
			{
				#region Header auslesen
				byte[] buffer=new byte[2];
				fileReader.Read(buffer, 0, 2);
				if(buffer[0]!='B'&&buffer[0]!='M') return new Graphic(); // Signatur überprüfen

				bfSize=fileReader.ReadInt32();						// (unzuverlässig)
				fileReader.BaseStream.Seek(10, SeekOrigin.Begin);	// bfReserved überspringen
				bfOffBits=fileReader.ReadInt32();
				#endregion

				#region Informationsblock
				biSize=fileReader.ReadUInt32();

				// 108: Typ Windows V4 wird nicht unterstützt
				// 124: Typ Windows V5 wird nicht unterstützt
				if(biSize==12)
				{ // OS/2 1.x
					biWidth=fileReader.ReadInt16();
					biHeight=fileReader.ReadInt16();
					biPlanes=fileReader.ReadInt16();
					biBitCount=fileReader.ReadInt16();

					// Fest definiert
					biCompression=BMPBiCompression.BI_RGB;
					biSizeImage=(uint)(bfSize-26);
					biXPelsPerMeter=2835; // 1000 DPI
					biYPelsPerMeter=2835; // 1000 DPI

					if(biBitCount==1||biBitCount==4||biBitCount==8)
					{
						int CountColors=1<<biBitCount; // 2^biBitCount

						for(int i=0; i<CountColors; i++)
						{
							byte blue=fileReader.ReadByte();
							byte green=fileReader.ReadByte();
							byte red=fileReader.ReadByte();
							ColorTable.Add(Color.FromArgb(red, green, blue));
						}
					}
					else if(biBitCount==16||biBitCount==24||biBitCount==32)
					{
						// nix
					}
					else return new Graphic();
				}
				else if(biSize==40||biSize==56||biSize==64)
				{
					// 40: Windows 3.1x, 95, NT
					// 56: Adobe Photoshop BMP mit Bitmaske (nicht Standardkonform?)
					// 64: OS/2 2.x (weitere Bytes werden einfach ignoriert)

					biWidth=fileReader.ReadInt32();
					biHeight=fileReader.ReadInt32();
					biPlanes=fileReader.ReadInt16();
					biBitCount=fileReader.ReadInt16();

					biCompression=(BMPBiCompression)fileReader.ReadUInt32();

					biSizeImage=fileReader.ReadUInt32();
					biXPelsPerMeter=fileReader.ReadInt32();
					biYPelsPerMeter=fileReader.ReadInt32();

					biClrUsed=fileReader.ReadUInt32();
					biClrImportant=fileReader.ReadUInt32();

					// 56 Byte Header ist ein 40 Byte Header + BitFields (16 byte): fehlerhaft geschrieben von Adobe PhotoShop
					if(biSize==64) fileReader.BaseStream.Seek(24, SeekOrigin.Current); // Rest des OS/2 2.x Headers

					if(biBitCount==16||biBitCount==24||biBitCount==32)
					{
						if(biCompression==BMPBiCompression.BI_BITFIELDS&&(biBitCount==16||biBitCount==32))
						{
							bmRed=fileReader.ReadUInt32();
							bmGreen=fileReader.ReadUInt32();
							bmBlue=fileReader.ReadUInt32();
						}
						if(biCompression==BMPBiCompression.BI_ALPHABITFIELDS&&(biBitCount==16||biBitCount==32))
						{
							bmRed=fileReader.ReadUInt32();
							bmGreen=fileReader.ReadUInt32();
							bmBlue=fileReader.ReadUInt32();
							bmAlpha=fileReader.ReadUInt32(); // ich hoffe mal, dass der Alpha am Anfang ist
						}
					}
					else if(biBitCount==1||biBitCount==4||biBitCount==8)
					{
						uint CountColors=biClrUsed;
						if(biClrUsed==0) CountColors=1u<<biBitCount; // 2^biBitCount

						for(uint i=0; i<CountColors; i++)
						{
							byte blue=fileReader.ReadByte();
							byte green=fileReader.ReadByte();
							byte red=fileReader.ReadByte();
							fileReader.BaseStream.Seek(1, SeekOrigin.Current);
							ColorTable.Add(Color.FromArgb(red, green, blue));
						}
					}
					else return new Graphic();
				}
				else return new Graphic();

				// Konsistenz Check
				if(biPlanes!=1||biWidth<=0||biHeight==0) return new Graphic();
				if(biBitCount!=16&&biBitCount!=32&&(biCompression==BMPBiCompression.BI_ALPHABITFIELDS||
					biCompression==BMPBiCompression.BI_BITFIELDS)) return new Graphic();

				// Fall auf Standardbehandlung zurück, wenn Bitfields dem Standardfällen entsprechen
				if(biCompression==BMPBiCompression.BI_BITFIELDS)
				{
					if(biBitCount==32&&bmRed==0xFF0000&&bmGreen==0xFF00&&bmBlue==0xFF) biCompression=BMPBiCompression.BI_RGB;
				}
				if(biCompression==BMPBiCompression.BI_ALPHABITFIELDS)
				{
					if(biBitCount==32&&bmRed==0xFF0000&&bmGreen==0xFF00&&bmBlue==0xFF&&bmAlpha==0xFF000000) biCompression=BMPBiCompression.BI_RGB;
				}

				int absHeight=System.Math.Abs(biHeight);
				#endregion

				#region pImage anlegen
				Graphic ret;

				if(biBitCount==32||(biBitCount==16&&(biCompression==BMPBiCompression.BI_ALPHABITFIELDS||
					biCompression==BMPBiCompression.BI_BITFIELDS)))
					ret=new Graphic((uint)biWidth, (uint)absHeight, Format.RGBA);
				else //Andere Bildern
					ret=new Graphic((uint)biWidth, (uint)absHeight, Format.RGB);
				#endregion

				#region Bilddaten in pImage einlesen
				fileReader.BaseStream.Seek(bfOffBits, SeekOrigin.Begin); // zu Bilddaten springen

				if(biCompression==BMPBiCompression.BI_RGB)
				{
					#region Unkomprimierte Bilddaten
					if(biBitCount==1) // 1 Bit
					{
						#region 1 Bit
						int BytesPerRow=(int)Align((uint)(biWidth+7)/8, 4);

						int ind=0;
						buffer=new byte[BytesPerRow];

						while(ColorTable.Count<2) ColorTable.Add(Color.Black);

						if(biHeight>0) // buttom-up Bild
						{
							for(int i=absHeight-1; i>=0; i--)
							{
								ind=i*biWidth*3;
								fileReader.Read(buffer, 0, buffer.Length);
								byte pixel=0;
								for(int a=0; a<biWidth; a++)
								{
									if(a%8==0) pixel=buffer[a/8];
									int bit=(pixel&0x80)==0x80?1:0;
									ret.imageData[ind++]=ColorTable[bit].B;
									ret.imageData[ind++]=ColorTable[bit].G;
									ret.imageData[ind++]=ColorTable[bit].R;
									pixel<<=1;
								}
							}
						}
						else if(biHeight<0) // top-down Bild
						{
							for(int i=0; i<absHeight; i++)
							{
								fileReader.Read(buffer, 0, buffer.Length);
								byte pixel=0;
								for(int a=0; a<biWidth; a++)
								{
									if(a%8==0) pixel=buffer[a/8];
									int bit=(pixel&0x80)==0x80?1:0;
									ret.imageData[ind++]=ColorTable[bit].B;
									ret.imageData[ind++]=ColorTable[bit].G;
									ret.imageData[ind++]=ColorTable[bit].R;
									pixel<<=1;
								}
							}
						}
						#endregion
					}
					else if(biBitCount==4) // 4 Bit
					{
						#region 4 Bit
						int BytesPerRow=(int)Align((uint)(biWidth*4+7)/8, 4);

						int ind=0;
						buffer=new byte[BytesPerRow];

						while(ColorTable.Count<16) ColorTable.Add(Color.Black);

						if(biHeight>0) // buttom-up Bild
						{
							for(int i=absHeight-1; i>=0; i--)
							{
								ind=i*biWidth*3;
								fileReader.Read(buffer, 0, buffer.Length);
								byte pixel=0;
								for(int a=0; a<biWidth; a++)
								{
									if(a%2==0) pixel=buffer[a/2];
									int bit=(pixel&0xF0)>>4;
									ret.imageData[ind++]=ColorTable[bit].B;
									ret.imageData[ind++]=ColorTable[bit].G;
									ret.imageData[ind++]=ColorTable[bit].R;
									pixel<<=4;
								}
							}
						}
						else if(biHeight<0) // top-down Bild
						{
							for(int i=0; i<absHeight; i++)
							{
								fileReader.Read(buffer, 0, buffer.Length);
								byte pixel=0;
								for(int a=0; a<biWidth; a++)
								{
									if(a%2==0) pixel=buffer[a/2];
									int bit=(pixel&0xF0)>>4;
									ret.imageData[ind++]=ColorTable[bit].B;
									ret.imageData[ind++]=ColorTable[bit].G;
									ret.imageData[ind++]=ColorTable[bit].R;
									pixel<<=4;
								}
							}
						}
						#endregion
					}
					else if(biBitCount==8) // 8 Bit
					{
						#region 8 Bit
						int BytesPerRow=(int)Align((uint)biWidth, 4);

						int ind=0;
						buffer=new byte[BytesPerRow];

						while(ColorTable.Count<256) ColorTable.Add(Color.Black);

						if(biHeight>0) // buttom-up Bild
						{
							for(int i=absHeight-1; i>=0; i--)
							{
								ind=i*biWidth*3;
								fileReader.Read(buffer, 0, buffer.Length);
								for(int a=0; a<biWidth; a++)
								{
									byte pixel=buffer[a];
									ret.imageData[ind++]=ColorTable[pixel].B;
									ret.imageData[ind++]=ColorTable[pixel].G;
									ret.imageData[ind++]=ColorTable[pixel].R;
								}
							}
						}
						else if(biHeight<0) // top-down Bild
						{
							for(int i=0; i<absHeight; i++)
							{
								fileReader.Read(buffer, 0, buffer.Length);
								for(int a=0; a<biWidth; a++)
								{
									byte pixel=buffer[a];
									ret.imageData[ind++]=ColorTable[pixel].B;
									ret.imageData[ind++]=ColorTable[pixel].G;
									ret.imageData[ind++]=ColorTable[pixel].R;
								}
							}
						}
						#endregion
					}
					else if(biBitCount==16) // 16 Bit
					{
						#region 16 Bit
						int BytesPerRow=(biWidth*2);
						int rest=(int)(Align((uint)BytesPerRow, 4)-BytesPerRow);

						int ind=0;
						buffer=new byte[BytesPerRow];

						if(biHeight>0) // buttom-up Bild
						{
							for(int i=absHeight-1; i>=0; i--)
							{
								ind=i*biWidth*3;
								fileReader.Read(buffer, 0, buffer.Length);
								for(int a=0; a<biWidth; a++)
								{
									byte pixelA=buffer[a*2];
									byte pixelB=buffer[a*2+1];

									int b=(pixelA&0x1F)<<3;
									int g=((pixelB&0x3)<<6)+((pixelA&0xE0)>>2);
									int r=(pixelB&0x7C)<<1;

									ret.imageData[ind++]=(byte)(b+b/32);
									ret.imageData[ind++]=(byte)(g+g/32);
									ret.imageData[ind++]=(byte)(r+r/32);
								}
							}
						}
						else if(biHeight<0) // top-down Bild
						{
							for(int i=absHeight-1; i>=0; i--)
							{
								fileReader.Read(buffer, 0, buffer.Length);
								for(int a=0; a<biWidth; a++)
								{
									byte pixelA=buffer[a*2];
									byte pixelB=buffer[a*2+1];

									int b=(pixelA&0x1F)<<3;
									int g=((pixelB&0x3)<<6)+((pixelA&0xE0)>>2);
									int r=(pixelB&0x7C)<<1;

									ret.imageData[ind++]=(byte)(b+b/32);
									ret.imageData[ind++]=(byte)(g+g/32);
									ret.imageData[ind++]=(byte)(r+r/32);
								}
							}
						}
						#endregion
					}
					else if(biBitCount==24) // 24 Bit
					{
						#region 24 Bit
						int BytesPerRow=(biWidth*3);
						int rest=(int)(Align((uint)BytesPerRow, 4)-BytesPerRow);

						if(biHeight>0) // buttom-up Bild
						{
							for(int i=0; i<absHeight; i++)
							{
								fileReader.Read(ret.imageData, ret.imageData.Length-(i+1)*BytesPerRow, BytesPerRow);
								if(rest!=0) fileReader.BaseStream.Seek(rest, SeekOrigin.Current);
							}
						}
						else if(biHeight<0) // top-down Bild
						{
							if(rest==0)
							{
								fileReader.Read(ret.imageData, 0, BytesPerRow*absHeight); // ganze bild auf einmal laden
							}
							else
							{
								for(int i=0; i<absHeight; i++)
								{
									fileReader.Read(ret.imageData, i*BytesPerRow, BytesPerRow);
									if(rest!=0) fileReader.BaseStream.Seek(rest, SeekOrigin.Current);
								}
							}
						}
						#endregion
					}
					else if(biBitCount==32) // 32 Bit
					{
						#region 32 Bit
						int BytesPerRow=(biWidth*4);
						buffer=new byte[BytesPerRow];

						if(biHeight>0) // buttom-up Bild
						{
							for(int i=0; i<absHeight; i++)
							{
								fileReader.Read(ret.imageData, ret.imageData.Length-(i+1)*BytesPerRow, BytesPerRow);
							}
						}
						else if(biHeight<0) // top-down Bild
						{
							fileReader.Read(ret.imageData, 0, BytesPerRow*absHeight); // ganze bild auf einmal laden
						}
						#endregion
					}
					#endregion Unkomprimierte Bilddaten
				}
				else if(biCompression==BMPBiCompression.BI_RLE8) // RLE 8 Kodierte Daten
				{
					#region RLE 8
					if(biBitCount!=8) return new Graphic();	// nur 8 Bit Bilder erlaubt

					int BytesPerRow=(int)Align((uint)biWidth, 4);
					int rest=BytesPerRow-biWidth;
					byte[] bufferRLE=new byte[absHeight*BytesPerRow];
					int index=0;
					int lineCount=0;

					while(ColorTable.Count<256) ColorTable.Add(Color.Black);

					#region Dekomprimiere RLE 8
					while(fileReader.BaseStream.Position<fileReader.BaseStream.Length&&index<(absHeight*BytesPerRow))
					{
						if(rest!=0&&(index/BytesPerRow)!=lineCount) throw new Exception("Bad RLE 8 coding.");
						if(rest==0&&(index/BytesPerRow)!=lineCount)
							if(index>BytesPerRow*(lineCount+1)) throw new Exception("Bad RLE 8 coding.");

						byte cByte=fileReader.ReadByte();

						if(cByte==0) // Kommando steht in Byte 2
						{
							byte scByte=fileReader.ReadByte();
							switch(scByte)
							{
								case 0: // Ende der Bildzeile
									{
										lineCount++;
										index=lineCount*BytesPerRow;
										break;
									}
								case 1: // Ende der Bitmap
									{
										index=absHeight*BytesPerRow;
										lineCount=absHeight;
										fileReader.BaseStream.Position=fileReader.BaseStream.Length;
										break;
									}
								case 2: // Verschiebung der aktuellen Pixelposition
									{
										byte vRight=fileReader.ReadByte();	// Verschiebung nach Rechts
										if(vRight>=biWidth) throw new Exception("Bad RLE 8 coding.");

										byte vDown=fileReader.ReadByte();	// Verschiebung nach Unten

										int currentRow=index-lineCount*BytesPerRow;
										if((currentRow+vRight)>=biWidth) throw new Exception("Bad RLE 8 coding.");
										if((lineCount+vDown)>=absHeight) throw new Exception("Bad RLE 8 coding.");

										lineCount+=vDown;
										index=lineCount*BytesPerRow+currentRow+vRight;
										break;
									}
								default: // 3-255 Byte unverändert übernehmen
									{
										// Die folgenden n Bytes werden direkt übernommen;
										// der nächste Datensatz findet sich am darauffolgenden
										// geraden Offset (vom Start der Bilddaten aus gezählt).

										fileReader.Read(bufferRLE, index, scByte);
										index+=scByte;

										// Wenn Offset nicht grade dann ein Byte nach vorne
										if(scByte%2!=0) fileReader.BaseStream.Seek(1, SeekOrigin.Current);

										break;
									}
							}
						}
						else // Daten so oft wie im cByte angegeben reinschreiben
						{
							byte dByte=fileReader.ReadByte();
							for(int i=0; i<cByte; i++) bufferRLE[index++]=dByte;
						}
					}
					#endregion

					#region pImage erstellen und füllen
					if(biHeight>0) // buttom-up Bild
					{
						int fIndex=0; // RLE Buffer Index
						index=0;
						for(int i=absHeight-1; i>=0; i--)
						{
							index=i*biWidth*3;
							for(int a=0; a<biWidth; a++)
							{
								byte pixel=bufferRLE[fIndex++];
								ret.imageData[index++]=ColorTable[pixel].B;
								ret.imageData[index++]=ColorTable[pixel].G;
								ret.imageData[index++]=ColorTable[pixel].R;
							}
							fIndex+=rest;
						}
					}
					else if(biHeight<0) // top-down Bild
					{
						int fIndex=0;	// RLE Buffer Index
						index=0;
						for(int i=0; i<absHeight; i++)
						{
							for(int a=0; a<biWidth; a++)
							{
								byte pixel=bufferRLE[fIndex++];
								ret.imageData[index++]=ColorTable[pixel].B;
								ret.imageData[index++]=ColorTable[pixel].G;
								ret.imageData[index++]=ColorTable[pixel].R;
							}
							fIndex+=rest;
						}
					}
					#endregion
					#endregion
				}
				else if(biCompression==BMPBiCompression.BI_RLE4) // RLE 4 Kodierte Daten
				{
					#region RLE 4
					if(biBitCount!=4) return new Graphic(); //Nur 4 Bit Bilder erlaubt

					int BytesPerRow=(int)Align((uint)biWidth, 4);
					int rest=BytesPerRow-biWidth;
					byte[] bufferRLE=new byte[absHeight*BytesPerRow];
					int index=0;
					int lineCount=0;

					while(ColorTable.Count<16) ColorTable.Add(Color.Black);

					#region Dekomprimiere RLE 4
					while(fileReader.BaseStream.Position<fileReader.BaseStream.Length&&index<(absHeight*BytesPerRow))
					{
						if(rest!=0&&(index/BytesPerRow)!=lineCount) throw new Exception("Bad RLE 4 coding.");
						if(rest==0&&(index/BytesPerRow)!=lineCount)
							if(index>BytesPerRow*(lineCount+1)) throw new Exception("Bad RLE 4 coding.");

						byte cByte=fileReader.ReadByte();

						if(cByte==0) // Kommando steht in Byte 2
						{
							byte scByte=fileReader.ReadByte();
							switch(scByte)
							{
								case 0: // Ende der Bildzeile
									{
										lineCount++;
										index=lineCount*BytesPerRow;
										break;
									}
								case 1: // Ende der Bitmap
									{
										index=absHeight*BytesPerRow;
										lineCount=absHeight;
										fileReader.BaseStream.Position=fileReader.BaseStream.Length;
										break;
									}
								case 2: // Verschiebung der aktuellen Pixelposition
									{
										byte vRight=fileReader.ReadByte();	// Verschiebung nach Rechts
										if(vRight>=biWidth) throw new Exception("Bad RLE 4 coding.");

										byte vDown=fileReader.ReadByte();	// Verschiebung nach Unten

										int currentRow=index-lineCount*BytesPerRow;
										if((currentRow+vRight)>=biWidth) throw new Exception("Bad RLE 4 coding.");
										if((lineCount+vDown)>=absHeight) throw new Exception("Bad RLE 4 coding.");

										lineCount+=vDown;
										index=lineCount*BytesPerRow+currentRow+vRight;
										break;
									}
								default: // 3-255 Nibbles unverändert übernehmen
									{
										// Die folgenden n Nibbles werden direkt übernommen;
										// der nächste Datensatz findet sich am darauffolgenden
										// geraden Offset (vom Start der Bilddaten aus gezählt).

										byte[] nibbles=new byte[(scByte+1)/2];
										fileReader.Read(nibbles, 0, nibbles.Length);

										for(int i=0; i<nibbles.Length-(cByte%2); i++)
										{
											bufferRLE[index++]=(byte)(nibbles[i]>>4);
											bufferRLE[index++]=(byte)(nibbles[i]&0xF);
										}
										if(cByte%2!=0) bufferRLE[index++]=(byte)(nibbles[nibbles.Length-1]>>4);

										// Wenn Offset nicht grade dann ein Byte nach vorne
										if(((scByte+1)/2)%2!=0) fileReader.BaseStream.Seek(1, SeekOrigin.Current);

										break;
									}
							}
						}
						else // Daten so oft wie im cByte angegeben reinschreiben
						{
							byte dByte=fileReader.ReadByte();
							byte aByte=(byte)(dByte>>4);
							byte bByte=(byte)(dByte&0xF);
							for(int i=0; i<cByte/2; i++)
							{
								bufferRLE[index++]=aByte;
								bufferRLE[index++]=bByte;
							}
							if(cByte%2!=0) bufferRLE[index++]=aByte;
						}
					}
					#endregion

					#region pImage erstellen und füllen
					if(biHeight>0) // buttom-up Bild
					{
						int fIndex=0; // RLE Buffer Index
						index=0;
						for(int i=absHeight-1; i>=0; i--)
						{
							index=i*biWidth*3;
							for(int a=0; a<biWidth; a++)
							{
								byte pixel=bufferRLE[fIndex++];
								ret.imageData[index++]=ColorTable[pixel].B;
								ret.imageData[index++]=ColorTable[pixel].G;
								ret.imageData[index++]=ColorTable[pixel].R;
							}
							fIndex+=rest;
						}
					}
					else if(biHeight<0) // top-down Bild
					{
						int fIndex=0;	// RLE Buffer Index
						index=0;
						for(int i=0; i<absHeight; i++)
						{
							for(int a=0; a<biWidth; a++)
							{
								byte pixel=bufferRLE[fIndex++];
								ret.imageData[index++]=ColorTable[pixel].B;
								ret.imageData[index++]=ColorTable[pixel].G;
								ret.imageData[index++]=ColorTable[pixel].R;
							}
							fIndex+=rest;
						}
					}
					#endregion
					#endregion
				}
				else if(biCompression==BMPBiCompression.BI_BITFIELDS||biCompression==BMPBiCompression.BI_ALPHABITFIELDS)
				{ // Bitmasken
					#region BI_BITFIELDS
					if(biBitCount==16)
					{
						#region Bitmasken checken
						bmRed&=0xffff; // nur 16 bittig
						bmGreen&=0xffff;
						bmBlue&=0xffff;
						bmAlpha&=0xffff;

						bool doRed=true;
						bool doGreen=true;
						bool doBlue=true;
						bool doAlpha=true;

						if((bmRed&bmGreen)>0) throw new Exception("Bad bit fields");
						if((bmRed&bmBlue)>0) throw new Exception("Bad bit fields");
						if((bmGreen&bmBlue)>0) throw new Exception("Bad bit fields");

						int rshifta=0;
						while(((bmRed>>rshifta)&0x1)==0) rshifta++;
						int rshiftb=rshifta;
						while(((bmRed>>rshiftb)&0x1)!=0) rshiftb++;
						for(int i=rshiftb; i<16; i++) if((bmRed&1u<<i)>0) throw new Exception("Bad bit fields");

						int gshifta=0;
						while(((bmGreen>>gshifta)&0x1)==0) gshifta++;
						int gshiftb=gshifta;
						while(((bmGreen>>gshiftb)&0x1)!=0) gshiftb++;
						for(int i=gshiftb; i<16; i++) if((bmGreen&1u<<i)>0) throw new Exception("Bad bit fields");

						int bshifta=0;
						while(((bmBlue>>bshifta)&0x1)==0) bshifta++;
						int bshiftb=bshifta;
						while(((bmBlue>>bshiftb)&0x1)!=0) bshiftb++;
						for(int i=bshiftb; i<16; i++) if((bmBlue&1u<<i)>0) throw new Exception("Bad bit fields");

						int ashifta=0;
						int ashiftb=0;
						if(biCompression==BMPBiCompression.BI_BITFIELDS)
						{
							bmAlpha=~(bmRed|bmGreen|bmBlue);
							if(bmAlpha!=0xffff0000)
							{
								bmAlpha&=0xffff;
								ashifta=0;
								while(((bmAlpha>>ashifta)&0x1)==0) ashifta++;

								ashiftb=ashifta;
								while(((bmAlpha>>ashiftb)&0x1)!=0) ashiftb++;

								bmAlpha=~bmAlpha;
								for(int i=ashiftb; i<16; i++) bmAlpha|=1u<<i;
								bmAlpha=~bmAlpha;
							}
							else bmAlpha=0;
						}
						else
						{
							if((bmAlpha&bmRed)>0) throw new Exception("Bad bit fields");
							if((bmAlpha&bmGreen)>0) throw new Exception("Bad bit fields");
							if((bmAlpha&bmBlue)>0) throw new Exception("Bad bit fields");

							ashifta=0;
							while(((bmAlpha>>ashifta)&0x1)==0) ashifta++;
							ashiftb=ashifta;
							while(((bmAlpha>>ashiftb)&0x1)!=0) ashiftb++;
							for(int i=ashiftb; i<16; i++) if((bmAlpha&1u<<i)>0) throw new Exception("Bad bit fields");
						}

						if(bmRed==0) doRed=false;
						if(bmGreen==0) doGreen=false;
						if(bmBlue==0) doBlue=false;
						if(bmAlpha==0) doAlpha=false;

						if(!doRed&&!doGreen&&!doBlue&&!doAlpha) throw new Exception("Bad bit fields");

						int redSize=rshiftb-rshifta;
						int greenSize=gshiftb-gshifta;
						int blueSize=bshiftb-bshifta;
						int alphaSize=ashiftb-ashifta;
						#endregion

						#region 16 Bit
						int BytesPerRow=(biWidth*2);
						buffer=new byte[BytesPerRow*absHeight];

						if(biHeight>0) // buttom-up Bild
						{
							for(int i=0; i<absHeight; i++)
							{
								fileReader.Read(buffer, buffer.Length-(i+1)*BytesPerRow, BytesPerRow);
							}
						}
						else if(biHeight<0) // top-down Bild
						{
							fileReader.Read(buffer, 0, BytesPerRow*absHeight); // ganze bild auf einmal laden
						}

						uint redDiv=0, greenDiv=0, blueDiv=0, alphaDiv=0;
						if(doRed&&redSize<8) redDiv=1u<<redSize;
						if(doGreen&&greenSize<8) greenDiv=1u<<greenSize;
						if(doBlue&&blueSize<8) blueDiv=1u<<blueSize;
						if(doAlpha&&alphaSize<8) alphaDiv=1u<<alphaSize;

						// Bitmaske anwenden
						for(int i=0; i<biWidth*absHeight; i++)
						{
							int start=i*4;
							uint color=((uint)buffer[i*2+1]<<8)+(uint)buffer[i*2];

							if(doRed)
							{
								uint red=(color&bmRed)>>rshifta;
								if(redSize>8) red>>=redSize-8;
								if(redSize<8)
								{
									red<<=8-redSize;
									red+=red/redDiv;
								}
								ret.imageData[start+2]=(byte)red;
							}
							else ret.imageData[start+2]=0;

							if(doGreen)
							{
								uint green=(color&bmGreen)>>gshifta;
								if(greenSize>8) green>>=greenSize-8;
								if(greenSize<8)
								{
									green<<=8-greenSize;
									green+=green/greenDiv;
								}
								ret.imageData[start+1]=(byte)green;
							}
							else ret.imageData[start+1]=0;

							if(doBlue)
							{
								uint blue=(color&bmBlue)>>bshifta;
								if(blueSize>8) blue>>=blueSize-8;
								if(blueSize<8)
								{
									blue<<=8-blueSize;
									blue+=blue/blueDiv;
								}
								ret.imageData[start]=(byte)blue;
							}
							else ret.imageData[start]=0;

							if(doAlpha)
							{
								uint alpha=(color&bmAlpha)>>ashifta;
								if(alphaSize>8) alpha>>=alphaSize-8;
								if(alphaSize<8)
								{
									alpha<<=8-alphaSize;
									alpha+=alpha/alphaDiv;
								}
								ret.imageData[start+3]=(byte)alpha;
							}
							else ret.imageData[start+3]=0;
						}
						#endregion
					}
					else if(biBitCount==32)
					{
						#region Bitmasken checken
						bool doRed=true;
						bool doGreen=true;
						bool doBlue=true;
						bool doAlpha=true;

						if((bmRed&bmGreen)>0) throw new Exception("Bad bit fields");
						if((bmRed&bmBlue)>0) throw new Exception("Bad bit fields");
						if((bmGreen&bmBlue)>0) throw new Exception("Bad bit fields");

						int rshifta=0;
						while(((bmRed>>rshifta)&0x1)==0) rshifta++;
						int rshiftb=rshifta;
						while(((bmRed>>rshiftb)&0x1)!=0) rshiftb++;
						for(int i=rshiftb; i<32; i++) if((bmRed&1u<<i)>0) throw new Exception("Bad bit fields");

						int gshifta=0;
						while(((bmGreen>>gshifta)&0x1)==0) gshifta++;
						int gshiftb=gshifta;
						while(((bmGreen>>gshiftb)&0x1)!=0) gshiftb++;
						for(int i=gshiftb; i<32; i++) if((bmGreen&1u<<i)>0) throw new Exception("Bad bit fields");

						int bshifta=0;
						while(((bmBlue>>bshifta)&0x1)==0) bshifta++;
						int bshiftb=bshifta;
						while(((bmBlue>>bshiftb)&0x1)!=0) bshiftb++;
						for(int i=bshiftb; i<32; i++) if((bmBlue&1u<<i)>0) throw new Exception("Bad bit fields");

						int ashifta=0;
						int ashiftb=0;
						if(biCompression==BMPBiCompression.BI_BITFIELDS)
						{
							bmAlpha=~(bmRed|bmGreen|bmBlue);
							if(bmAlpha!=0)
							{
								ashifta=0;
								while(((bmAlpha>>ashifta)&0x1)==0) ashifta++;

								ashiftb=ashifta;
								while(((bmAlpha>>ashiftb)&0x1)!=0) ashiftb++;

								bmAlpha=~bmAlpha;
								for(int i=ashiftb; i<32; i++) bmAlpha|=1u<<i;
								bmAlpha=~bmAlpha;
							}
						}
						else
						{
							if((bmAlpha&bmRed)>0) throw new Exception("Bad bit fields");
							if((bmAlpha&bmGreen)>0) throw new Exception("Bad bit fields");
							if((bmAlpha&bmBlue)>0) throw new Exception("Bad bit fields");

							ashifta=0;
							while(((bmAlpha>>ashifta)&0x1)==0) ashifta++;
							ashiftb=ashifta;
							while(((bmAlpha>>ashiftb)&0x1)!=0) ashiftb++;
							for(int i=ashiftb; i<32; i++) if((bmAlpha&1u<<i)>0) throw new Exception("Bad bit fields");
						}

						if(bmRed==0) doRed=false;
						if(bmGreen==0) doGreen=false;
						if(bmBlue==0) doBlue=false;
						if(bmAlpha==0) doAlpha=false;

						if(!doRed&&!doGreen&&!doBlue&&!doAlpha) throw new Exception("Bad bit fields");

						int redSize=rshiftb-rshifta;
						int greenSize=gshiftb-gshifta;
						int blueSize=bshiftb-bshifta;
						int alphaSize=ashiftb-ashifta;
						#endregion

						#region 32 Bit
						int BytesPerRow=(biWidth*4);

						if(biHeight>0) // buttom-up Bild
						{
							for(int i=0; i<absHeight; i++)
							{
								fileReader.Read(ret.imageData, ret.imageData.Length-(i+1)*BytesPerRow, BytesPerRow);
							}
						}
						else if(biHeight<0) // top-down Bild
						{
							fileReader.Read(ret.imageData, 0, BytesPerRow*absHeight); // ganze bild auf einmal laden
						}

						uint redDiv=0, greenDiv=0, blueDiv=0, alphaDiv=0;
						if(doRed&&redSize<8) redDiv=1u<<redSize;
						if(doGreen&&greenSize<8) greenDiv=1u<<greenSize;
						if(doBlue&&blueSize<8) blueDiv=1u<<blueSize;
						if(doAlpha&&alphaSize<8) alphaDiv=1u<<alphaSize;

						// Bitmaske anwenden
						for(int i=0; i<biWidth*absHeight; i++)
						{
							int start=i*4;
							uint color=((uint)ret.imageData[start+3]<<24)+((uint)ret.imageData[start+2]<<16)+((uint)ret.imageData[start+1]<<8)+(uint)ret.imageData[start];

							if(doRed)
							{
								uint red=(color&bmRed)>>rshifta;
								if(redSize>8) red>>=redSize-8;
								if(redSize<8)
								{
									red<<=8-redSize;
									red+=red/redDiv;
								}
								ret.imageData[start+2]=(byte)red;
							}
							else ret.imageData[start+2]=0;

							if(doGreen)
							{
								uint green=(color&bmGreen)>>gshifta;
								if(greenSize>8) green>>=greenSize-8;
								if(greenSize<8)
								{
									green<<=8-greenSize;
									green+=green/greenDiv;
								}
								ret.imageData[start+1]=(byte)green;
							}
							else ret.imageData[start+1]=0;

							if(doBlue)
							{
								uint blue=(color&bmBlue)>>bshifta;
								if(blueSize>8) blue>>=blueSize-8;
								if(blueSize<8)
								{
									blue<<=8-blueSize;
									blue+=blue/blueDiv;
								}
								ret.imageData[start]=(byte)blue;
							}
							else ret.imageData[start]=0;

							if(doAlpha)
							{
								uint alpha=(color&bmAlpha)>>ashifta;
								if(alphaSize>8) alpha>>=alphaSize-8;
								if(alphaSize<8)
								{
									alpha<<=8-alphaSize;
									alpha+=alpha/alphaDiv;
								}
								ret.imageData[start+3]=(byte)alpha;
							}
							else ret.imageData[start+3]=0;
						}
						#endregion
					}

					return ret;
					#endregion
				}
				#endregion

				return ret;
			}
			catch(Exception)
			{
				return new Graphic();
			}
			finally
			{
				fileReader.Close();
			}
		}
		#endregion

		#region FromGDIFile
		public static Graphic FromGDIFile(string filename)
		{
			Image img=Image.FromFile(filename);
			uint width=(uint)img.Size.Width;
			uint height=(uint)img.Size.Height;

			Graphic ret=null;

			if((img.PixelFormat&PixelFormat.Alpha)==PixelFormat.Alpha)
			{
				ret=new Graphic(width, height, Format.RGBA);

				Bitmap bmp=new Bitmap(img);
				BitmapData data=bmp.LockBits(new Rectangle(0, 0, (int)width, (int)height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
				Marshal.Copy(data.Scan0, ret.imageData, 0, (int)(width*height*4));
				bmp.UnlockBits(data);
			}
			else
			{
				ret=new Graphic(width, height, Format.RGB);

				Bitmap bmp=new Bitmap(img);
				BitmapData data=bmp.LockBits(new Rectangle(0, 0, (int)width, (int)height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
				if(((int)width*3)==data.Stride)
				{
					Marshal.Copy(data.Scan0, ret.imageData, 0, (int)(width*height*3));
				}
				else
				{
					if(IntPtr.Size==4)
					{
						for(uint i=0; i<height; i++)
						{
							Marshal.Copy((IntPtr)(data.Scan0.ToInt32()+(int)(i*data.Stride)), ret.imageData, (int)(width*3*i), (int)(width*3));
						}
					}
					else if(IntPtr.Size==8)
					{
						for(uint i=0; i<height; i++)
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

			img.Dispose();
			img=null;

			return ret;
		}
		#endregion
	}
}