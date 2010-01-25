// 
// Copyright (C) 2006 Enterprise Distributed Technologies Ltd
// 
// www.enterprisedt.com
//

#region Change Log

// Change Log:
// 
// $Log$
// Revision 1.4  2009-10-01 01:32:00  hans
// Combine failed if pathRight was null.
//
// Revision 1.3  2009-09-30 03:34:13  hans
// Fixed numerous bugs.
//
// Revision 1.2  2009-09-25 05:56:25  bruceb
// remove Contains
//
// Revision 1.1  2009-09-25 05:21:27  hans
// Utility for dealing with FTP (i.e. UNIX) paths.
//
//
//

#endregion

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;

using CSCL.Util.Debug;

namespace CSCL.Util
{
    internal class PathUtil
    {
        //private static Logger log = Logger.GetLogger("PathUtil");
        private const char SEPARATOR_CHAR = '/';
        private const string SAMEDIR_STRING = ".";
        private const string UPDIR_STRING = "..";

        public static char SeparatorChar
        {
            get { return SEPARATOR_CHAR; }
        }

        public static string Separator
        {
            get { return SEPARATOR_CHAR.ToString(); }
        }

        public static string Fix(string path)
        {
            return Implode(Fix(Explode(path)));
        }

        public static StringCollection Fix(StringCollection path)
        {
            if (path.Count == 0)
                return path;
            StringCollection fixedPath = new StringCollection();
            for (int i = 0; i < path.Count; i++)
            {
                string p = path[i];
                if (p == Separator)
                {
                    // add separator unless it's a duplicate (i.e. follows another sep - "//")
                    if (i == 0 || (fixedPath.Count>0 && fixedPath[fixedPath.Count - 1] != Separator))
                        fixedPath.Add(p);
                }
                else if (p == UPDIR_STRING)
                {
                    if (fixedPath.Count == 1)
                        throw new ArgumentException("Cannot change up a directory from the root");

                    // only remove parent directory if there is one and it's not another ".."
                    if (fixedPath.Count >= 2 && fixedPath[fixedPath.Count - 2] != UPDIR_STRING)
                    {
                        fixedPath.RemoveAt(fixedPath.Count - 1);  // remove separator
                        fixedPath.RemoveAt(fixedPath.Count - 1);  // remove parent
                    }
                    else
                        fixedPath.Add(p);   // add the ".." 
                }
                else if (p != "" && p != SAMEDIR_STRING)  // ignore empty strings and "."
                    fixedPath.Add(p);
            }

            // remove trailing slashes
            while (fixedPath.Count > 1 && fixedPath[fixedPath.Count - 1] == Separator)
                fixedPath.RemoveAt(fixedPath.Count - 1);

            // if there's nothing then just return "."
            if (fixedPath.Count == 0)
                fixedPath.Add(SAMEDIR_STRING);

            return fixedPath;
        }

        public static bool IsAbsolute(string path)
        {
            return path!=null && path.StartsWith("/");
        }

        public static string GetFileName(string path)
        {
            if (path.IndexOf(SEPARATOR_CHAR.ToString()) >= 0)
                return path.Substring(path.LastIndexOf(SEPARATOR_CHAR) + 1);
            else
                return path;
        }

        public static string GetFolderPath(string path)
        {
            if (path.IndexOf(SEPARATOR_CHAR.ToString()) >= 0)
                return path.Substring(0, path.LastIndexOf(SEPARATOR_CHAR));
            else
                return path;
        }

        public static string Combine(string pathLeft, string pathRight)
        {
            if (pathLeft == null)
                pathLeft = "/";
            if (pathRight == null || pathRight == "" || pathRight == ".")
                return pathLeft;
            if (pathRight.StartsWith(Separator))
                throw new ArgumentException("Second argument cannot be absolute", "pathRight");
            if (pathLeft.EndsWith(Separator))
                pathLeft = pathLeft.Substring(0, pathLeft.Length - 1);
            return Fix(pathLeft + Separator + pathRight);
        }

        public static string Combine(string path1, string path2, params string[] pathN)
        {
            string path = Combine(path1, path2);
            foreach (string p in pathN)
                path = Combine(path, p);
            return path;
        }

        public static StringCollection Explode(string path)
        {
            StringCollection elements = new StringCollection();
            int pos1 = 0;
            while (true)
            {
                int pos2 = path.IndexOf(SeparatorChar, pos1);
                if (pos2 < 0)
                    break;
                if (pos1 == pos2)
                {
                    elements.Add(Separator);
                    pos1 = pos2 + 1;
                }
                else
                {
                    elements.Add(path.Substring(pos1, pos2 - pos1));
                    pos1 = pos2;
                }
            }
            if (pos1<path.Length)
                elements.Add(path.Substring(pos1));
            return elements;
        }

        public static string Implode(IEnumerable pathElements, int start, int length)
        {
            StringBuilder path = new StringBuilder();
            int i = 0;
            foreach (string p in pathElements)
            {
                if (i >= start && (length < 0 || i < start + length))
                    path.Append(p);
                i++;
            }
            return path.ToString();
        }

        public static string Implode(IEnumerable pathElements, int start)
        {
            return Implode(pathElements, start, -1);
        }

        public static string Implode(IEnumerable pathElements)
        {
            return Implode(pathElements, 0, -1);
        }
    }
}
