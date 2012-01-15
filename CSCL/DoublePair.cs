using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCL
{
	public class DoublePair<T, U>
	{
		public DoublePair()
		{
		}

		public DoublePair(T first, U second)
		{
			First=first;
			Second=second;
		}

		public T First { get; set; }
		public U Second { get; set; }
	}
}
