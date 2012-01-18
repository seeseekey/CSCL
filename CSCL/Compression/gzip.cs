//
//  gzip.cs
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
using System.IO;
using System.IO.Compression;

namespace CSCL.Compression
{
	public class gzip
	{
		public static void Compress(Stream sourceStream, Stream destinationStream)
		{
			using(GZipStream gzipStream=new GZipStream(destinationStream, CompressionMode.Compress))
			{
				CopyData(sourceStream, gzipStream);
			}
		}

		public static byte[] Compress(byte[] data)
		{
			byte[] res=null;

			using(MemoryStream sourceStream=new MemoryStream(data, false))
			{
				using(MemoryStream destinationStream=new MemoryStream())
				{
					Compress(sourceStream, destinationStream);
					res=destinationStream.ToArray();
				}
			}

			return res;
		}

		public static void Decompress(System.IO.Stream sourceStream, System.IO.Stream destinationStream)
		{
			using(GZipStream gzipStream=new GZipStream(sourceStream, CompressionMode.Decompress))
			{
				CopyData(gzipStream, destinationStream);
			}
		}

		public static byte[] Decompress(byte[] data)
		{
			byte[] res=null;

			using(System.IO.MemoryStream sourceStream=new System.IO.MemoryStream(data, false))
			{
				using(System.IO.MemoryStream destinationStream=new System.IO.MemoryStream())
				{
					Decompress(sourceStream, destinationStream);
					res=destinationStream.ToArray();
				}
			}

			return res;
		}

		private static void CopyData(Stream sourceStream, Stream destinationStream)
		{
			byte[] buffer=new byte[4096];
			Int32 bytesRead=0;
			do
			{
				bytesRead=sourceStream.Read(buffer, 0, buffer.Length);
				if(bytesRead!=0)
				{
					destinationStream.Write(buffer, 0, bytesRead);
				}
			}
			while(bytesRead!=0);
		}
	}
}
