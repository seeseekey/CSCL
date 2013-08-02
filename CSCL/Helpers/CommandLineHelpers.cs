using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSCL.Helpers
{
	public static class CommandLineHelpers
	{
		public static Dictionary<string, string> GetCommandLine(string[] args)
		{
			Dictionary<string, string> ret=new Dictionary<string, string>();
			if(args.Length<1) return ret;	// nix Argumente

			int filecount=0;
			string key;
			foreach(string str in args)
			{
				if(str[0]=='-')
				{
					int idx=str.IndexOf(':');
					if(idx>=0)
					{
						key=str.Substring(1, idx-1);
						ret.Add(key, str.Substring(idx+1));
					}
					else
					{
						key=str.Substring(1);
						ret.Add(key, "true");
					}
				}
				else
				{
					key=String.Format("file{0}", filecount++);
					ret.Add(key, str);
				}
			}
			return ret;
		}

		public static List<string> GetFilesFromCommandline(Dictionary<string, string> line)
		{
			List<string> ret=new List<string>();

			foreach(string key in line.Keys)
			{
				if(key.StartsWith("file"))
				{
					ret.Add(line[key]);
				}
			}

			return ret;
		}
	}
}
