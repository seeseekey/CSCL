using System;

namespace CSCL.Controls.SourceGrid.Utils
{
	public interface IPerformanceCounter : IDisposable
	{
		double GetSeconds();
		double GetMilisec();
	}
}
