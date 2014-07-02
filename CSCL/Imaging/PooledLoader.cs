//
//  PooledLoader.cs
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
		public class PooledLoader
		{
			int maxImages;
			Dictionary<string, Graphic> imgPool;
			List<string> imgLoadOrder;

			public PooledLoader()
			{
				maxImages=20;
				imgPool=new Dictionary<string, Graphic>();
				imgLoadOrder=new List<string>();
			}

			public PooledLoader(uint mxi)
			{
				maxImages=(int)mxi;
				imgPool=new Dictionary<string, Graphic>();
				imgLoadOrder=new List<string>();
			}

			public Graphic FromFile(string filename)
			{
				if (imgPool.ContainsKey(filename))
				{
					imgLoadOrder.Remove(filename);
					imgLoadOrder.Add(filename);
					return imgPool[filename];
				}

				while (maxImages<=imgPool.Count)
				{
					string del=imgLoadOrder[0];
					imgLoadOrder.RemoveAt(0);
					imgPool.Remove(del);
				}

				Graphic ret=Graphic.FromFile(filename);
				imgPool.Add(filename, ret);
				imgLoadOrder.Add(filename);

				return ret;
			}

			public void Clear()
			{
				imgPool.Clear();
				imgLoadOrder.Clear();
			}

			public int MaxImages
			{
				get
				{
					return maxImages;
				}
				set
				{
					maxImages=value;
				}
			}

			public int Count
			{
				get { return imgLoadOrder.Count; }
			}

			public bool RemoveFileFromPool(string filename)
			{
				string lower=filename.ToLower();

				if (imgPool.ContainsKey(lower))
				{
					imgLoadOrder.Remove(lower);
					imgPool.Remove(lower);
					return true;
				}

				return false;
			}

			public string[] ToArray()
			{
				return imgLoadOrder.ToArray();
			}
		}
	}
}
