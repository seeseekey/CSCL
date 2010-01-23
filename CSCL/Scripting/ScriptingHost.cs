using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.CodeDom.Compiler;
using System.CodeDom;
using Microsoft.CSharp;

namespace CSCL.Scripting
{
	/// <summary>
	/// Stellt externe Skriptingfunktionalität zur Verfügung
	/// </summary>
	public class ScriptingHost
	{
		#region Variablen und Exceptions
		private List<Assembly> i_LoadedScripts; //Geladenen Skripte
		private CompilerParameters i_CompilerParams=new CompilerParameters();

		public class CompilerException : Exception
		{
			public CompilerException() { }
			public CompilerException(string message) : base(message) { }
			public CompilerException(string message, Exception inner) : base(message, inner) { }
		}
		#endregion

		public ScriptingHost()
		{
			i_LoadedScripts=new List<Assembly>();

			i_CompilerParams.GenerateInMemory=true;
			i_CompilerParams.ReferencedAssemblies.Add("System.dll");
			i_CompilerParams.ReferencedAssemblies.Add("System.Windows.Forms.dll");
			i_CompilerParams.ReferencedAssemblies.Add("SCL.dll");
		}

		public void AddAssemblyReference(string assembly)
		{
			i_CompilerParams.ReferencedAssemblies.Add(assembly);
		}

		/// <summary>
		/// Liest das Skript ein und compiliert es 
		/// </summary>
		/// <param name="filename"></param>
		public void LoadScript(string code)
		{
			CSharpCodeProvider cs=new CSharpCodeProvider();
			CompilerResults res=cs.CompileAssemblyFromSource(i_CompilerParams, code);

			if(res.Errors.Count>0) //Wenn ein Fehler aufgetreten ist
			{
				foreach(CompilerError error in res.Errors)
				{
					throw new CompilerException(error.ToString());
				}

				return;
			}

			i_LoadedScripts.Add(res.CompiledAssembly);
		}

		public void LoadScriptFromFile(string filename)
		{
			string src=String.Empty;
			TextReader reader=new StreamReader(filename);
			src=reader.ReadToEnd();

			LoadScript(src);
		}

		public void LoadScripts(string path)
		{
			LoadScripts(path, "*.cs", false);
		}

		public void LoadScripts(string path, string filter)
		{
			LoadScripts(path, filter, false);
		}

		public void LoadScripts(string path, string filter, bool rekursiv)
		{
			foreach(string i in FileSystem.GetFiles(path, rekursiv, filter))
			{
				LoadScript(i);
			}
		}

		public void Start()
		{
			Start(null);
		}

		public void Start(object data)
		{
			if(i_LoadedScripts.Count==0)
			{
				throw new Exception("No scripts loaded!");
			}

			foreach(Assembly al in i_LoadedScripts)
			{
				foreach(Type t in al.GetTypes())
				{
					bool hasInterface=false;

					foreach(Type interfaceType in t.GetInterfaces())
					{
						if(interfaceType==typeof(IScript)) hasInterface=true;
					}

					if(hasInterface)
					{
						IScript script=Activator.CreateInstance(t) as IScript;
						script.Run(data);
					}
				}
			}
		}
	}
}
