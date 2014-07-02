//
//  ExceptionInfo.cs
//
//  Copyright (c) 2011, 2012 by seeseekey <seeseekey@gmail.com>
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
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace CSCL
{
	/// <summary>
	/// Stellt Informationen �ber eine Exception bereit.
	/// </summary>
	[Serializable]
	public class ExceptionInfo
	{
		#region Private Variables
		private Exception m_exception;
		[NonSerialized]
		private Thread m_thread;
		private string m_appDomainName="";
		private string m_assemblyName="";
		private int m_threadId=0;
		private string m_threadUser="";
		private string m_productName="";
		private string m_productVersion="";
		private string m_executablePath="";
		private string m_companyName="";
		private OperatingSystem m_operatingSystem;
		private Version m_frameworkVersion;
		private long m_workingSet=0;
		#endregion

		#region Constructors
		public ExceptionInfo()
		{
		}

		public ExceptionInfo(Exception ex)
		{
			if (null!=ex)
			{
				m_exception=ex;
				m_thread=System.Threading.Thread.CurrentThread;
				InitClass();
			}
		}

		public ExceptionInfo(Exception ex, Thread threadObject)
		{
			if (null!=ex)
			{
				m_exception=ex;
				if (threadObject!=null) m_thread=threadObject;
				else m_thread=System.Threading.Thread.CurrentThread;

				InitClass();
			}
		}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return string.Format("{0}\r\n"+
				"AppDomain: {1}\r\n"+
				"ThreadId {2}\r\n"+
				"ThreadUser: {3}\r\n"+
				"Productname: {4}\r\n"+
				"Productversion: {5}\r\n"+
				"Dateipfad: {6}\r\n"+
				"Betriebssystem: {7}\r\n"+
				".NET-Framework-Version: {8}\r\n",
				this.Exception.ToString(),
				this.AppDomainName,
				this.ThreadId,
				this.ThreadUser,
				this.ProductName,
				this.ProductVersion,
				this.ExecutablePath,
				this.OperatingSystem.ToString(),
				this.FrameworkVersion.ToString());
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Gibt die Exception an, f�r die Informationen ermittelt werden sollen.
		/// </summary>
		public Exception Exception
		{
			get { return m_exception; }
			set { m_exception=value; }
		}

		/// <summary>
		/// Gibt den Namen der AppDomain zur�ck, in der die Exception eingetreten ist.
		/// </summary>
		public string AppDomainName
		{
			get { return m_appDomainName; }
		}

		/// <summary>
		/// Gibt den Namen der aufrufenden Assembly zur�ck.
		/// </summary>
		public string AssemblyName
		{
			get { return m_assemblyName; }
		}

		/// <summary>
		/// Gibt die ID des Threads zur�ck, in dem die Exception eingetreten ist.
		/// </summary>
		public int ThreadId
		{
			get { return m_threadId; }
		}

		/// <summary>
		/// Gibt den Benutzer des Threads zur�ck, in dem die Exception eingetreten ist.
		/// </summary>
		public string ThreadUser
		{
			get { return m_threadUser; }
		}

		/// <summary>
		/// Gibt den Titel der Anwendung zur�ck.
		/// </summary>
		public string ProductName
		{
			get { return m_productName; }
		}

		/// <summary>
		/// Gibt die Version der Anwendung zur�ck.
		/// </summary>
		public string ProductVersion
		{
			get { return m_productVersion; }
		}

		/// <summary>
		/// Gibt das Ausf�hrungsverzeichnis der Anwendung zur�ck.
		/// </summary>
		public string ExecutablePath
		{
			get { return m_executablePath; }
		}

		/// <summary>
		/// Gibt den zugeh�rigen Firmennamen zur�ck.
		/// </summary>
		public string CompanyName
		{
			get { return m_companyName; }
		}

		/// <summary>
		/// Gibt das Informationen �ber das installierte Betriebssystem zur�ck.
		/// </summary>
		public OperatingSystem OperatingSystem
		{
			get { return m_operatingSystem; }
		}

		/// <summary>
		/// Gibt eine Versionsinformation �ber das installierte .NET-Framework zur�ck.
		/// </summary>
		public Version FrameworkVersion
		{
			get { return m_frameworkVersion; }
		}

		/// <summary>
		/// Gibt die Summe des physischen Speichers, der dem aktiven Prozess zugewiesen wurde, zur�ck.
		/// </summary>
		public long WorkingSet
		{
			get { return m_workingSet; }
		}
		#endregion

		#region Private Functions
		/// <summary>
		/// Ermittelt das erste StackFrame aus dem StackTrace der aktuellen Exception.
		/// </summary>
		/// <returns></returns>
		private StackFrame GetFirstStackFrame(Exception ex)
		{
			StackFrame frame;

			try
			{
				// StackTrace aus Exception erstellen
				StackTrace trace=new StackTrace(ex, true);

				// StackFrame aus StackTrace ermitteln
				frame=trace.GetFrame(0);
				return frame;
			}
			catch
			{
				return null;
			}
		}
		#endregion

		#region Public Functions
		///// <summary>
		///// Serialisiert das ExceptionInfo-Objekt unter dem angegebenen Dateinamen.
		///// </summary>
		///// <param name="fileName"></param>
		//public void Save(string fileName)
		//{
		//    FileStream fs=new FileStream(fileName, FileMode.Create);
		//    try
		//    {
		//        SoapFormatter sf=new SoapFormatter();
		//        sf.Serialize(fs, this);
		//    }
		//    catch(Exception ex)
		//    {
		//        MessageBox.Show(ex.Message);
		//    }
		//    finally
		//    {
		//        fs.Close();
		//    }
		//}

		///// <summary>
		///// Deserialisiert das ExceptionInfo-Objekt vom angegebenen Dateinamen.
		///// </summary>
		///// <param name="fileName"></param>
		//public ExceptionInfo Open(string fileName)
		//{
		//    ExceptionInfo exInfo=null;
		//    if(File.Exists(fileName))
		//    {
		//        FileStream fs=new FileStream(fileName, FileMode.Open, FileAccess.Read);
		//        try
		//        {
		//            SoapFormatter sf=new SoapFormatter();
		//            exInfo=(ExceptionInfo)sf.Deserialize(fs);
		//        }
		//        catch
		//        {
		//            throw;
		//        }
		//        finally
		//        {
		//            fs.Close();
		//        }
		//    }
		//    else
		//        throw (new ArgumentException("Die angegebene Datei konnte nicht gefunden werden.", "fileName"));

		//    return exInfo;
		//}

		/// <summary>
		/// Extrahiert den Methodennamen aus dem Stack Trace der aktuellen Exception.
		/// </summary>
		/// <returns></returns>
		public string GetMethodName(Exception ex)
		{
			string methodName="";

			try
			{
				// Erstes StackFrame aus StackTrace ermitteln
				StackFrame frame=GetFirstStackFrame(ex);

				// Klassen- und Funktionsnamen aus StackFrame ermitteln
				if (frame!=null)
				{
					MethodBase method=frame.GetMethod();
					methodName=method.Name+"(";

					// Methoden-Parameter ermitteln
					ParameterInfo[] paramInfos=method.GetParameters();
					string methodParams="";
					foreach (ParameterInfo paramInfo in paramInfos)
					{
						methodParams+=", "+paramInfo.ParameterType.Name+" "+paramInfo.Name;
					}
					if (methodParams.Length>2)
						methodName+=methodParams.Substring(2);
					methodName+=")";
				}
				else
					methodName=GetMethodNameFromStack(ex.StackTrace);
			}
			catch
			{
				methodName="";
			}

			return methodName;
		}

		/// <summary>
		/// Extrahiert den Klassennamen aus dem Stack Trace der aktuellen Exception.
		/// </summary>
		/// <returns></returns>
		public string GetClassName(Exception ex)
		{
			string className="";

			try
			{
				// Erstes StackFrame aus StackTrace ermitteln
				StackFrame frame=GetFirstStackFrame(ex);

				// Klassennamen aus StackFrame ermitteln
				if (frame!=null)
				{
					MethodBase method=frame.GetMethod();
					className=method.DeclaringType.ToString();
				}
				else
					className=GetClassNameFromStack(ex.StackTrace);
			}
			catch
			{
				className="";
			}

			return className;
		}

		/// <summary>
		/// Extrahiert den Dateinamen aus dem Stack Trace der aktuellen Exception.
		/// </summary>
		/// <returns></returns>
		public string GetFileName(Exception ex)
		{
			string fileName="";

			try
			{
				// Erstes StackFrame aus StackTrace ermitteln
				StackFrame frame=GetFirstStackFrame(ex);

				// Dateinamen aus StackFrame ermitteln
				if (frame!=null)
				{
					fileName=frame.GetFileName();
					fileName=fileName.Substring(fileName.LastIndexOf("\\")+1);
				}
				else
					fileName=GetFileNameFromStack(ex.StackTrace);
			}
			catch
			{
				fileName="";
			}

			return fileName;
		}

		/// <summary>
		/// Extrahiert den Fehler-Spaltenposition in der Datei aus dem Stack Trace der aktuellen Exception.
		/// </summary>
		/// <returns></returns>
		public int GetFileColumnNumber(Exception ex)
		{
			int column=0;

			try
			{
				// Erstes StackFrame aus StackTrace ermitteln
				StackFrame frame=GetFirstStackFrame(ex);

				// Dateinamen aus StackFrame ermitteln
				column=frame.GetFileColumnNumber();
			}
			catch
			{
				column=0;
			}

			return column;
		}

		/// <summary>
		/// Extrahiert den Fehler-Zeilenposition in der Datei aus dem Stack Trace der aktuellen Exception.
		/// </summary>
		/// <returns></returns>
		public int GetFileLineNumber(Exception ex)
		{
			int line=0;

			try
			{
				// Erstes StackFrame aus StackTrace ermitteln
				StackFrame frame=GetFirstStackFrame(ex);

				// Dateinamen aus StackFrame ermitteln
				if (frame!=null)
					line=frame.GetFileLineNumber();
				else
					line=Convert.ToInt32(GetLineNumberFromStack(ex.StackTrace));
			}
			catch
			{
				line=0;
			}

			return line;
		}

		/// <summary>
		/// Extrahiert den IL-Offset im Code aus dem Stack Trace der aktuellen Exception.
		/// </summary>
		/// <returns></returns>
		public int GetILOffset(Exception ex)
		{
			int ilOffset=0;

			try
			{
				// Erstes StackFrame aus StackTrace ermitteln
				StackFrame frame=GetFirstStackFrame(ex);

				// Dateinamen aus StackFrame ermitteln
				ilOffset=frame.GetILOffset();
			}
			catch
			{
				ilOffset=0;
			}

			return ilOffset;
		}

		/// <summary>
		/// Extrahiert den Native-Offset im Code aus dem Stack Trace der aktuellen Exception.
		/// </summary>
		/// <returns></returns>
		public int GetNativeOffset(Exception ex)
		{
			int nativeOffset=0;

			try
			{
				// Erstes StackFrame aus StackTrace ermitteln
				StackFrame frame=GetFirstStackFrame(ex);

				// Dateinamen aus StackFrame ermitteln
				nativeOffset=frame.GetNativeOffset();
			}
			catch
			{
				nativeOffset=0;
			}

			return nativeOffset;
		}

		/// <summary>
		/// Gibt das StackTrace der aktuellen Exception zur�ck.
		/// </summary>
		/// <returns></returns>
		public string GetStackTrace(Exception ex)
		{
			string stackTrace="";

			try
			{
				if (ex.StackTrace!=null)
					stackTrace=ex.StackTrace;
				else
					stackTrace="";
			}
			catch
			{
				stackTrace="";
			}

			return stackTrace;
		}

		/// <summary>
		/// Extrahiert die Klassennamen aus dem Stack Trace.
		/// </summary>
		/// <returns></returns>
		public string GetClassNameFromStack(string stackTrace)
		{
			string className="";

			try
			{
				Regex regClass=new Regex(@" at ([_a-zA-Z0-9\.]+\({1})");
				Match matchClass=regClass.Match(stackTrace);
				if (matchClass!=null)
				{
					className=matchClass.Value.Replace(" at ", "");
					className=className.Substring(0, className.LastIndexOf(".")).Trim();
				}
			}
			catch
			{
			}

			return className;
		}

		/// <summary>
		/// Extrahiert die Methodennamen aus dem Stack Trace.
		/// </summary>
		/// <returns></returns>
		public string GetMethodNameFromStack(string stackTrace)
		{
			string methodName="";
			try
			{
				Regex regMethod=new Regex(@"\.[_a-zA-Z0-9\, ]+\({1}[_a-zA-Z0-9\, ]+\)");
				Match matchMethod=regMethod.Match(stackTrace);
				if (matchMethod!=null)
					methodName=matchMethod.Value.Replace(".", "").Trim();
			}
			catch
			{
			}

			return methodName;
		}

		/// <summary>
		/// Extrahiert den Dateinamen aus dem Stack Trace.
		/// </summary>
		/// <returns></returns>
		public string GetFileNameFromStack(string stackTrace)
		{
			string fileName="";
			try
			{
				Regex regFile=new Regex(@"(\\{1}[_a-zA-Z0-9\.]+\:)");
				Match matchFile=regFile.Match(stackTrace);
				if (matchFile!=null)
					fileName=matchFile.Value.Trim().Replace("\\", "").Replace(":", "");
			}
			catch
			{
			}

			return fileName;
		}

		/// <summary>
		/// Extrahiert die Zeilennummer aus dem Stack Trace.
		/// </summary>
		/// <returns></returns>
		public string GetLineNumberFromStack(string stackTrace)
		{
			string lineNumber="";
			try
			{
				Regex regLine=new Regex(@"(\:{1}line [1-9]+)");
				Match matchLine=regLine.Match(stackTrace);
				if (matchLine!=null)
					lineNumber=matchLine.Value.Replace(":line", "").Trim();
			}
			catch
			{
			}

			return lineNumber;
		}
		#endregion

		#region Private Functions
		/// <summary>
		/// Initialisiert die Eigenschaften der Klasse.
		/// </summary>
		private void InitClass()
		{
			if (m_exception!=null)
			{
				m_appDomainName=AppDomain.CurrentDomain.FriendlyName;
				m_assemblyName=GetAssemblyName();
				m_threadId=Thread.CurrentThread.ManagedThreadId;
				m_threadUser=GetThreadUser();
				m_productName=Application.ProductName;
				m_productVersion=Application.ProductVersion;
				m_executablePath=Application.ExecutablePath;
				m_companyName=Application.CompanyName;
				m_operatingSystem=Environment.OSVersion;
				m_frameworkVersion=Environment.Version;
				m_workingSet=Environment.WorkingSet;
			}
		}

		/// <summary>
		/// Gibt den Thread-Benutzer zur�ck.
		/// </summary>
		/// <returns></returns>
		private string GetThreadUser()
		{
			return Thread.CurrentPrincipal.Identity.Name;
		}

		/// <summary>
		/// Ermittelt den Assemblynamen.
		/// </summary>
		/// <returns></returns>
		private string GetAssemblyName()
		{
			string assemblyName="";

			try
			{
				assemblyName=System.Reflection.AssemblyName.GetAssemblyName(Application.ExecutablePath).ToString();
				assemblyName=assemblyName.Substring(0, assemblyName.IndexOf(",", assemblyName.IndexOf(",")+1));
			}
			catch
			{
				assemblyName="";
			}

			return assemblyName;
		}
		#endregion
	}
}
