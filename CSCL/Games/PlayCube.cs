using System;
using System.Collections.Generic;
using System.Text;
using CSCL;
using CSCL.Helpers;

namespace CSCL.Games
{
	/// <summary>
	/// Diese Klasse stellt einen Spielwürfel bereit
	/// </summary>
	public static class PlayCube
	{
		public static int GetEye()
		{
			return GetEye(1);
		}

		public static int GetEye(int countCubes)
		{
			int ret=0;

			for(int i=0; i<countCubes; i++)
			{
				ret+=RandomHelpers.GetRandomInt(6);
			}

			return ret;
		}
	}
}
