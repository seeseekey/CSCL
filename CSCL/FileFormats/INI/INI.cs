using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CSCL.FileFormats.INI
{
	public class INISection
	{
		public string Name;
		public Dictionary<string, string> Values;

		public INISection()
		{
			Values=new Dictionary<string, string>();
		}
	}

	public class INI
	{
		public List<INISection> Sections=new List<INISection>();

		public INI()
		{
		}

		public INI(string filename)
		{
			string[] lines=File.ReadAllLines(filename);

			INISection currentSection=null;

			for(int i=0; i<lines.Length; i++)
			{
				string tmp=lines[i].Trim();

				if(tmp=="") continue;

				if(tmp[0]=='[')
				{
					char[] trimchars= { '[', ']' };

					INISection section=new INISection();
					section.Name=tmp.Trim(trimchars);

					Sections.Add(section);
					currentSection=section;
				}
				else
				{
					if(currentSection!=null)
					{
						string[] splited = tmp.Split('=');

						currentSection.Values.Add(splited[0], splited[1]);
					}
				}
			}
		}
	}
}
