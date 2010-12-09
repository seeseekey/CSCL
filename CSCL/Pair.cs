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
