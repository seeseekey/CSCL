using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCL
{
	/// <summary>
	/// Timer with parameter
	/// </summary>
	public class AdvancedTimer : System.Timers.Timer
	{
		public object Parameter
		{
			get;
			set;
		}

		public AdvancedTimer() : base()
		{
		}
	}
}
