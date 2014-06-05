//
//  FileSystem.cs
//
//  Copyright (c) 2011, 2012 by seeseekey <seeseekey@googlemail.com>
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
using System.IO;
using System.Collections.Generic;

namespace CSCL
{
    /// <summary>
    /// Class for file systems operations
    /// </summary>
    public class FileSystem
	{
		#region Statische Variablen
		static char pathDelimiter=System.IO.Path.DirectorySeparatorChar;

		public static char PathDelimiter
		{
			get
			{
				return pathDelimiter;
			}
		}

        /// <summary>
        /// Feld nicht erlaubter Zeichen im Dateinamen
        /// </summary>
        static char[] illegalChars=new char[] { '<', '>', ':', '"', '|', '\0', '\x1', '\x2', '\x3', '\x4', '\x5',
			'\x6', '\x7', '\x8', '\x9', '\xA', '\xB', '\xC', '\xD', '\xE', '\xF', '\x10', '\x11', '\x12', '\x13',
			'\x14', '\x15', '\x16', '\x17', '\x18', '\x19', '\x1A', '\x1B', '\x1C', '\x1D', '\x1E', '\x1F', '\xfffd' };
        #endregion

        #region Eigenschaften
        /// <summary>
        /// Gibt den Tempfad des Benutzers zur�ck
        /// </summary>
        public static string TempPath
        {
            get
            {
				return Path.GetTempPath().TrimEnd(pathDelimiter)+pathDelimiter;
            }
        }

        /// <summary>
        /// Ermittelt den Applikationspfad
        /// </summary>
        /// <returns></returns>
        public static string ApplicationPath
        {
            get
            {
                FileInfo fi=new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location);
				return fi.DirectoryName+pathDelimiter;
            }
        }

        /// <summary>
        /// Ermittelt den Applikationspfad mit der Anwendung
        /// </summary>
        /// <returns></returns>
        public static string ApplicationPathWithFilename
        {
            get
            {
                FileInfo fi=new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location);
                return fi.FullName;
            }
        }

        /// <summary>
        /// Gibt die Position des Ordners Anwendungsdaten zur�ck
        /// </summary>
        /// <returns></returns>
        public static string ApplicationDataDirectory
        {
            get
            {
				return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).TrimEnd(pathDelimiter)+pathDelimiter;
            }
        }
        #endregion

        #region Directories & Files
        /// <summary>
        /// �berpr�ft ob eine Datei oder ein Verzeichniss existiert
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool Exists(string filename)
        {
            return File.Exists(filename)||Directory.Exists(filename);
        }
        #endregion

        #region Directories
        public static bool ExistsDirectory(string directory)
        {
            return System.IO.Directory.Exists(directory);
        }

        /// <summary>
        /// �berpr�ft ob der �bergebene Dateiname ein Verzeichnis ist
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool IsDirectory(string filename)
        {
            try
            {
                FileAttributes attr=File.GetAttributes(filename);
                return (attr&FileAttributes.Directory)!=0;
            }
            catch(Exception) { }
            return false;
        }

        /// <summary>
        /// Legt ein Verzeichnis an
        /// </summary>
        /// <param name="dir">Path des anzulegenden Verzeichnisses</param>
        /// <returns>True, wenn erfolgreich.</returns>
		public static bool CreateDirectory(string dir)
        {
			return CreateDirectory(dir, false);
        }

        /// <summary>
        /// Legt ein Verzeichnis und s�mtliche Unterverzeichnisse an
        /// </summary>
        /// <param name="dir">Path des anzulegenden Verzeichnisses</param>
        /// <param name="force">Erzwingt das Anlegen des Verzeichnisses</param>
        /// <returns>True, wenn erfolgreich.</returns>
		public static bool CreateDirectory(string dir, bool throwException)
		{
			bool ret=true;

			char[] sep=new char[] { pathDelimiter };
			string[] pathParts=dir.Split(sep, StringSplitOptions.RemoveEmptyEntries);

			string path="";

			foreach(string i in pathParts)
			{
				switch(Environment.OSVersion.Platform)
				{
					case PlatformID.Win32NT:
					case PlatformID.Win32S:
					case PlatformID.Win32Windows:
						{
							path+=i+pathDelimiter;
							if(IsRoot(i)) continue;
							break;
						}
					case PlatformID.MacOSX:
					case PlatformID.Unix:
						{
							if(dir.Length>0&&dir[0]=='/') //Root
							{
								if(path.Length>0)
								{
									path+=i+pathDelimiter;
								}
								else
								{
									path+=pathDelimiter+i+pathDelimiter;
								}
							}
							else
							{
								path+=i+pathDelimiter; //Relativ
							}
							break;
						}
					default:
						{
							throw new NotImplementedException();
						}
				}

				if(!Directory.Exists(path))
				{
					try
					{
						Directory.CreateDirectory(path);
					}
					catch(Exception e)
					{
						if(throwException) throw e;
						return false;
					}
				}
			}

			return ret;
		}

        public static bool CopyDirectory(string source, string dest)
        {
            return CopyDirectory(source, dest, true, new List<string>());
        }

        public static bool CopyDirectory(string source, string dest, bool recursiv)
        {
            return CopyDirectory(source, dest, recursiv, new List<string>(), new List<string>(), false);
        }

		public static bool CopyDirectory(string source, string dest, bool recursiv, List<string> ExcludeFolders)
		{
			return CopyDirectory(source, dest, recursiv, ExcludeFolders, new List<string>(), false);
		}

		public static bool CopyDirectory(string source, string dest, bool recursiv, List<string> ExcludeFolders, List<string> ExcludeFiles)
		{
			return CopyDirectory(source, dest, recursiv, ExcludeFolders, ExcludeFiles, false);
		}

        public static bool CopyDirectory(string source, string dest, bool recursiv, List<string> ExcludeFolders, List<string> ExcludeFiles, bool ignoreExistingFiles)
        {
            DirectoryInfo SourceDI=new DirectoryInfo(source);

            bool result=true;
            if(!Directory.Exists(dest))
                Directory.CreateDirectory(dest);


            foreach(FileInfo fi in SourceDI.GetFiles())
            {
				string destFile=dest+pathDelimiter+fi.Name;

				bool breakOut=false;

				foreach(string i in ExcludeFiles)
				{
					if(fi.Name==i)
					{
						breakOut=true;
						continue;
					}
				}

				if(breakOut) continue;

				if(ignoreExistingFiles)
				{
					if(!ExistsFile(destFile))
					{
						result=result&&(fi.CopyTo(destFile, false)!=null);
					}
				}
				else
				{
					result=result&&(fi.CopyTo(destFile, false)!=null);
				}
            }

            if(recursiv)
            {
                foreach(DirectoryInfo subDir in SourceDI.GetDirectories())
                {
                    bool make=true;

                    foreach(string i in ExcludeFolders)
                    {
                        if(subDir.Name==i)
                        {
                            make=false;
                            break;
                        }
                    }

                    if(make)
                    {
                        string destTmp=subDir.FullName.Replace(SourceDI.FullName, dest);
						result=result&&CopyDirectory(subDir.FullName, destTmp, true, ExcludeFolders, ExcludeFiles, ignoreExistingFiles);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Verschiebt ein Verzeichniss �ber Volumegrenzen hinaus
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static bool MoveDirectory(string source, string dest)
        {
            if(CopyDirectory(source, dest)&&RemoveDirectory(source, true)) return true;
            else return false;
        }

        /// <summary>
        /// L�scht ein !leeres! Verzeichnis 
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static bool RemoveDirectory(string dir)
        {
            return RemoveDirectory(dir, false, false);
        }

		public static bool RemoveDirectory(string dir, bool recursive)
		{
			return RemoveDirectory(dir, recursive, false);
		}

        /// <summary>
        /// L�scht ein leeres (recursive=false)
        /// oder nicht leeres (recursive=true) Verzeichnis
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
		public static bool RemoveDirectory(string dir, bool recursive, bool throwException)
		{
			try
			{ 
				Directory.Delete(dir, recursive);
			}
			catch(Exception e) 
			{
				if(throwException) throw e;
				else return false;
			}
			return true;
		}

        /// <summary>
        /// Gibt das aktuelle Verzeichnis zur�ck
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDir()
        {
			return Environment.CurrentDirectory.TrimEnd(pathDelimiter)+pathDelimiter;
        }

        /// <summary>
        /// Setzt das aktuelle Verzeichnis
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static bool SetCurrentDir(string dir)
        {
			Environment.CurrentDirectory=dir.TrimEnd(pathDelimiter)+pathDelimiter;
            return GetCurrentDir()==dir;
        }

        /// <summary>
        /// Gibt Unterverzeichnisse des Verzeichnisses
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static List<string> GetDirectories(string directory)
        {
            try
            {
                List<string> ret=new List<string>();
                ret.AddRange(Directory.GetDirectories(directory));
                return ret;
            }
            catch(Exception)
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// Gibt Unterverzeichnisse des Verzeichnisses (gefiltert)
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static List<string> GetDirectories(string directory, string filter)
        {
            try
            {
                List<string> ret=new List<string>();
                ret.AddRange(Directory.GetDirectories(directory, filter));
                return ret;
            }
            catch(Exception)
            {
                return new List<string>();
            }
        }
        #endregion

        #region Files
        public static bool ExistsFile(string filename)
        {
            return System.IO.File.Exists(filename);
        }

        /// <summary>
        /// �berpr�ft ob der �bergebene Dateiname eine Datei ist
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool IsFile(string filename)
        {
            try
            {
                FileAttributes attr=File.GetAttributes(filename);
                return (attr&FileAttributes.Directory)==0;
            }
            catch(Exception) { }
            return false;
        }

        /// <summary>
        /// Kopiert eine Datei
        /// ohne �berschreiben des Ziels, wenn bereits vorhanden
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        public static bool CopyFile(string src, string dst)
        {
            return CopyFile(src, dst, false);
        }

        /// <summary>
        /// Kopiert eine Datei
        /// overwrite = true zum �berschreiben des Ziels, wenn bereits vorhanden
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static bool CopyFile(string src, string dst, bool overwrite)
        {
			try { File.Copy(src, dst, overwrite); }
			catch(Exception) { return false; }
            return true;
        }

		public static bool CopyFiles(string source, string target, string filter)
		{
			return CopyFiles(source, target, filter, new List<string>(), false);
		}

		public static bool CopyFiles(string source, string target, string filter, List<string> ExcludeFiles)
		{
			return CopyFiles(source, target, filter, ExcludeFiles, false);
		}

        /// <summary>
        /// Kopiert mehrere Dateien anhand Filterkriterien
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
		public static bool CopyFiles(string source, string target, string filter, List<string> ExcludeFiles, bool throwException)
		{
			List<string> files=GetFiles(source, false, filter);

			foreach(string i in files)
			{
				string fn=GetFilename(i);

				try
				{
					bool breakOut=false;

					foreach(string j in ExcludeFiles)
					{
						if(fn==j)
						{
							breakOut=true;
							continue;
						}
					}

					if(breakOut) continue;

					File.Copy(i, target+fn);
				}
				catch(Exception e)
				{
					if(throwException) throw e;
					else return false;
				}
			}

			return true;
		}

        /// <summary>
        /// Verschiebt eine Datei
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        public static bool MoveFile(string src, string dst)
        {
            try { File.Move(src, dst); }
            catch(Exception) { return false; }
            return true;
        }

        /// <summary>
        /// Benennt eine Datei um
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        public static bool RenameFile(string src, string dst)
        {
            return MoveFile(src, dst);
        }

        public static bool RemoveFile(string filename)
        {
            try
            {
                File.Delete(filename);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool RemoveFiles(List<string> filenames)
        {
            bool ret=true;

            foreach(string i in filenames)
            {
                if(RemoveFile(i)==false) ret=false;
            }

            return ret;
        }

        /// <summary>
        /// �berpr�ft ob eine Datei ReadOnly ist
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool IsReadOnly(string filename)
        {
            try
            {
                FileAttributes attr=File.GetAttributes(filename);
                return (attr&FileAttributes.ReadOnly)!=0;
            }
            catch(Exception) { }
            return false;
        }

        /// <summary>
        /// Gibt die Dateigr��e zur�ck
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static long GetFilesize(string filename)
        {
            if(!IsFile(filename)) return -1;
            FileInfo fi=new FileInfo(filename);
            return fi.Length;
        }

        /// <summary>
        /// �berpr�ft ob der Dateiname nur g�ltige Zeichenenth�lt
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool IsFilenameValid(string filename)
        {
            if(filename.IndexOfAny(illegalChars)!=-1) return false;
            return true;
        }

        /// <summary>
        /// Macht aus einem Dateinamen mit ung�ltigen Zeichen einen Dateinamen
        /// mit g�ltigen Zeichen
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetValidFilename(string filename)
        {
            int ind;
            while((ind=filename.IndexOfAny(illegalChars))!=-1) filename=filename.Remove(ind, 1);
            return filename;
        }

        /// <summary>
        /// Creates a list which contains all filenames in a specific folder
        /// </summary>
        /// <param name="Root">Folder which contains files to be listed</param>
        /// <param name="SubFolders">True for scanning subfolders</param>
        /// <returns></returns>
        public static List<string> GetFiles(string Root, bool Recursiv)
        {
            return GetFiles(Root, Recursiv, "*");
        }

        /// <summary>
        /// Creates a list which contains all filenames in a specific folder
        /// </summary>
        /// <param name="Root">Folder which contains files to be listed</param>
        /// <param name="SubFolders">True for scanning subfolders</param>
        /// <returns></returns>
		public static List<string> GetFiles(string directory, bool recursiv, string filter)
		{
			try
			{
				List<string> ret=new List<string>();

				string[] subdirs=Directory.GetDirectories(directory);
				string[] files=Directory.GetFiles(directory, filter);

				ret.AddRange(files);

				if(recursiv)
				{
					foreach(string dir in subdirs)
					{
						List<string> subfiles=GetFiles(dir, recursiv, filter);
						ret.AddRange(subfiles);
					}
				}

				return ret;
			}
			catch(Exception)
			{
				return null;
			}
		}

		public static DateTime GetFileDateTime(string filename)
		{
			if(!IsFile(filename)) throw new FileNotFoundException("", filename);
			FileInfo fi=new FileInfo(filename);
			return fi.LastWriteTime;
		}
        #endregion

        #region Paths
        /// <summary>
        /// Ermittelt ob es sich bei dem Pfad um einen Root handelt
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
		public static bool IsRoot(string path)
		{
			path=path.TrimEnd(pathDelimiter).ToLower();

			switch(Environment.OSVersion.Platform)
			{
				case PlatformID.Win32NT:
				case PlatformID.Win32S:
				case PlatformID.Win32Windows:
					{
						if(path.Length==2&&path[0]>='a'&&path[0]<='z'&&path[1]==':') return true;
						break;
					}
				case PlatformID.MacOSX:
				case PlatformID.Unix:
					{
						if(path.Length>0&&path[0]=='/') return true;
						break;
					}
				default:
					{
						throw new NotImplementedException();
					}
			}

			return false;
		}

        /// <summary>
        /// �berpr�ft ob der �bergebende Pfad Absolut ist
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
		public static bool IsAbsolute(string path)
		{
			switch(Environment.OSVersion.Platform)
			{
				case PlatformID.Win32NT:
				case PlatformID.Win32S:
				case PlatformID.Win32Windows:
					{
						if(path.Length<3) return false;	// kein "c:\" oder �hnliches
						if(!Char.IsLetter(path[0]))
						{	//Test auf UNC Pfad
							if(path[0]!='\\') return false;	// z.B. "\\FOO\myMusic"
							if(path[1]!='\\') return false;
							return true;
						}
						if(path[1]!=':') return false;
						if(path[2]!='\\'&&path[2]!='/') return false;
						return true;
					}
				case PlatformID.MacOSX:
				case PlatformID.Unix:
					{
						if(path.Length>0&&path[0]=='/') return true;
						else return false;
					}
				default:
					{
						throw new NotImplementedException();
					}
			}
		}

        /// <summary>
        /// �berpr�ftob es sich bei dem filename um einen Pfad handelt
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsPath(string filename)
        {
            char[] trimchars=new char[2];
            trimchars[0]='/';
            trimchars[1]='\\';

            filename=filename.TrimEnd(trimchars);

            if(filename.IndexOf('\\')!=-1||filename.IndexOf('/')!=-1) return true;
            else return false;
        }

        /// <summary>
        /// �berpr�ft ob der �bergebende Pfad ein Netzwerkpfad ist
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsNetworkPath(string path)
        {
            if(path.Length<3) return false;	// kein "c:\" oder �hnliches
            if(!Char.IsLetter(path[0]))
            {	//Test auf UNC Pfad
                if(path[0]!='\\') return false;	// z.B. "\\FOO\myMusic"
                if(path[1]!='\\') return false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// �berpr�ft ob der �bergebende Pfad ein lokaler Pfad ist
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsLocalPath(string path)
        {
            if(IsNetworkPath(path)) return false;
            if(IsAbsolute(path)) return true;

            if(path.Length<1) return false;

            if(path.IndexOfAny(illegalChars)!=-1) return false;
            return true;
        }

        /// <summary>
        /// Gibt den Absoluten Pfad des �bergenen Pfades zur�ck
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetAbsolutePath(string path)
        {
            FileInfo ret=new FileInfo(path);
            return ret.FullName;
        }

		public static string GetRelativePath(string completePath, string basePath)
		{
			return GetRelativePath(completePath, basePath, false);
		}

        /// <summary>
        /// Gibt den relativen Pfad zur�ck
        /// </summary>
        /// <param name="completePath"></param>
        /// <param name="basePath"></param>
        /// <returns></returns>
        public static string GetRelativePath(string completePath, string basePath, bool CaseSensitive)
        {
            if(!IsAbsolute(completePath)&&!IsNetworkPath(completePath))
                throw new ArgumentException("completePath is not absolute.");

            if(!IsAbsolute(basePath)&&!IsNetworkPath(basePath))
                throw new ArgumentException("basePath is not absolute.");

			if (!CaseSensitive)
			{
				completePath=completePath.ToLower();
				basePath=basePath.ToLower();
			}

            completePath=GetAbsolutePath(completePath);
            basePath=GetAbsolutePath(basePath);

            // canonize
			if(basePath[basePath.Length-1]!=pathDelimiter) basePath+=pathDelimiter;

            string relativePath="";

            int baseStart=completePath.IndexOf(basePath);

            while(baseStart!=0&&basePath.Length>3)
            {
                basePath=GetPath(basePath.Substring(0, basePath.Length-1));

				if(basePath[basePath.Length-1]!=pathDelimiter) basePath+=pathDelimiter;

				relativePath+=".."+pathDelimiter;
                baseStart=completePath.IndexOf(basePath);
            }

            if(baseStart==0)
                relativePath+=completePath.Substring(basePath.Length);
            else relativePath=completePath;

            return relativePath;
        }

        public static string GetPath(string filename)
        {
            return GetPath(filename, false);
        }

		public static string GetPath(string filename, bool stringMethod)
		{
			return GetPath(filename, stringMethod, true);
		}

        /// <summary>
        /// Gibt Verzeichnisanteil des Strings zur�ck (incl. \)
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetPath(string filename, bool stringMethod, bool pathDelimiterAtEnd)
        {
            if(stringMethod)
            {
				if(filename[filename.Length-1]==pathDelimiter) return filename;

                int idx=-1;

                for(int i=filename.Length-1;i>=0;i--)
                {
                    if(filename[i]=='\\'||filename[i]=='/')
                    {
                        idx=i; 
                        break;
                    }
                }

                if(idx==-1) return "";
                string path=filename.Substring(0, idx);

				if(pathDelimiterAtEnd) return path+pathDelimiter;
				else return path;
            }
            else
            {
                FileInfo ret=new FileInfo(filename);
                if(ret.DirectoryName.Length==3) return ret.DirectoryName;

				if(pathDelimiterAtEnd) return ret.DirectoryName+pathDelimiter;
				else return ret.DirectoryName;
            }
        }

		public static string GetPathWithPathDelimiter(string path)
		{
			return path.TrimEnd(pathDelimiter)+pathDelimiter;
		}

        /// <summary>
        /// Gibt Dateinamenanteil des Strings zur�ck (rechts des letzten \ oder /)
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetFilename(string filename)
        {
            return GetFilename(filename, false);
        }

        public static string GetFilename(string filename, bool stringMethod)
        {
            if(stringMethod)
            {
				if(filename[filename.Length-1]==pathDelimiter) return filename;

                int idx=-1;

                for(int i=filename.Length-1;i>=0;i--)
                {
                    if(filename[i]=='\\'||filename[i]=='/')
                    {
                        idx=i;
                        break;
                    }
                }

                if(idx==-1) return filename;
                idx++;
                return filename.Substring(idx, filename.Length-idx);
            }
            else
            {
                FileInfo ret=new FileInfo(filename);
                return ret.Name;
            }
        }

        /// <summary>
        /// Gibt Dateinamenendung des Strings zur�ck (rechts des letzten '.' )
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetExtension(string filename)
        {
            FileInfo ret=new FileInfo(filename);
            return ret.Extension.TrimStart('.');
        }

        /// <summary>
        /// Gibt einen String mit neuer Dateierweiterung zur�ck
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="newExt"></param>
        /// <returns></returns>
        public static string ChangeExtension(string filename, string newExt)
        {
            string fnWithoutExt=GetFilenameWithoutExt(filename);
            newExt=newExt.Replace(".", "");
            return fnWithoutExt+"."+newExt;
        }

        /// <summary>
        /// Gibt Dateinamenanteil(ohne letzte Erweiterung) des Strings zur�ck
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetFilenameWithoutExt(string filename)
        {
            FileInfo ret=new FileInfo(filename);
            return ret.Name.Substring(0, ret.Name.Length-ret.Extension.Length);
        }

        /// <summary>
        /// Gibt Path und Dateinamenanteil(ohne letzte Erweiterung) des Strings zur�ck
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetPathFilenameWithoutExt(string filename)
        {
            return GetPath(filename)+GetFilenameWithoutExt(filename);
        }
        #endregion
    }
}
