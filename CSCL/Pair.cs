//
//  Pair.cs
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

namespace CSCL
{
	public class Pair<T>
	{
		public T First;
		public T Second;

		public Pair()
		{
		}

		public Pair(T first, T second)
		{
			First=first;
			Second=second;
		}

		public override bool Equals(object obj)
		{
			if(obj==null) return false;
			Pair<T> o=(Pair<T>)obj;
			return First.Equals(o.First)&&Second.Equals(o.Second);
		}

		public override int GetHashCode()
		{
			if((object)First==null)
			{
				if((object)Second==null) return 0;
				return (-1)^Second.GetHashCode();
			}

			if((object)Second==null) return First.GetHashCode();
			return First.GetHashCode()^Second.GetHashCode();
		}
	}
}
