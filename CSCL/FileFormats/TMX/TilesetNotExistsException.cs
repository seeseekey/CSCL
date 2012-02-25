using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.FileFormats.TMX
{
	public class TilesetNotExistsException: Exception
	{
		public string Filename { get; set; }

		public TilesetNotExistsException(string filename)
		{
			Filename=filename;
		}
	}
}
