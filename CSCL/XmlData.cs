using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace CSCL
{
	/// <summary>
	/// Klasse welche die Benutzung von XML im normalen Gebrauch vereinfacht
	/// </summary>
	public class XmlData
	{
		#region Variablen
		private XmlDocument InternalXmlDocument; //Interner Xml im Speicher
		private XmlDeclaration InternalXmlDeclaration; //Die XmlDekleniration oben im Header

		private string InternalFilename;
		#endregion

		#region Eigenschaften
		/// <summary>
		/// Das XML Document Objekt
		/// Für Bearbeitungen auf direkter XML Ebene
		/// </summary>
		public XmlDocument Document
		{
			get
			{
				return InternalXmlDocument;
			}
		}
		#endregion

		#region Hilfsfunktionen
		/// <summary>
		/// Macht den Pfad konfirm, z.B. wenn er ohne xml als Root angegeben wird.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		private string MakePathConfirm(string path)
		{
			string ret=path.Trim().TrimStart('/');
			ret='/'+ret.Replace('.', '/');
			return ret;
		}

		/// <summary>
		/// Erzeugt einen Pfad wenn er noch nicht vorhanden ist
		/// </summary>
		/// <param name="path"></param>
		private void CheckPathAndCreateMissingElements(string path)
		{
			path=MakePathConfirm(path);
			char[] splitchars=new char[] { '/' };
			string[] parts=path.Split(splitchars, StringSplitOptions.RemoveEmptyEntries);

			string checkpath="";

			foreach(string i in parts)
			{
				string ExistPath=checkpath+"."+i;
				ExistPath=ExistPath.TrimStart('.');

				if(ExistElement(ExistPath)==false)
				{
					path=MakePathConfirm(checkpath);

					XmlNode AddNode=InternalXmlDocument.CreateElement(i);
					XmlNodeList SelectedPath=InternalXmlDocument.SelectNodes(path);
					SelectedPath[0].AppendChild(AddNode);
				}

				checkpath+="."+i;
			}
		}
		#endregion

		#region Konstruktoren und Destruktoren
		/// <summary>
		/// Konstruktor
		/// </summary>
		public XmlData()
		{
			//XmlDocument initialisieren
			InternalXmlDocument=new XmlDocument();
			InternalXmlDeclaration=InternalXmlDocument.CreateXmlDeclaration("1.0", "utf-8", "yes");
			InternalXmlDocument.InsertBefore(InternalXmlDeclaration, InternalXmlDocument.DocumentElement);
		}

		public XmlData(string filename)
		{
			InitXmlDataWithFile(filename, false);
		}

		public XmlData(string filename, bool overwrite)
		{
			InitXmlDataWithFile(filename, overwrite);
		}

		/// <summary>
		/// Konstruktor welcher gleich eine Datei lädt
		/// </summary>
		/// <param name="filename">Name der Datei</param>
		public void InitXmlDataWithFile(string filename, bool overwrite)
		{
			//XmlDocument initialisieren
			InternalXmlDocument=new XmlDocument();
			InternalXmlDeclaration=InternalXmlDocument.CreateXmlDeclaration("1.0", "utf-8", "yes");
			InternalXmlDocument.InsertBefore(InternalXmlDeclaration, InternalXmlDocument.DocumentElement);

			//if(!FileSystem.ExistsFile(filename))
			//{
			//    InternalXmlDocument.AppendChild(InternalXmlDocument.CreateElement("xml"));
			//    Save(filename);
			//}

			InternalFilename=filename;
			InternalXmlDocument.XmlResolver=null;

			if(FileSystem.ExistsFile(filename)&&overwrite==false)
			{
				InternalXmlDocument.Load(InternalFilename);
			}
		}

		public XmlData(byte[] data)
		{
			//XmlDocument initialisieren
			InternalXmlDocument=new XmlDocument();
			InternalXmlDeclaration=InternalXmlDocument.CreateXmlDeclaration("1.0", "utf-8", "yes");
			InternalXmlDocument.InsertBefore(InternalXmlDeclaration, InternalXmlDocument.DocumentElement);

			MemoryStream ms=new MemoryStream(data);
			InternalFilename="";
			InternalXmlDocument.XmlResolver=null;
			InternalXmlDocument.Load(ms);
		}
		#endregion

		#region Add Funktionen
		public void AddAttribute(XmlNode addNode, string attributeName, int attributeValue)
		{
			AddAttribute(addNode, attributeName, attributeValue.ToString());
		}

		/// <summary>
		/// Fügt der Node ein Atribute hinzu
		/// </summary>
		/// <param name="addNode"></param>
		/// <param name="attributeName"></param>
		/// <param name="attributeValue"></param>
		public void AddAttribute(XmlNode addNode, string attributeName, string attributeValue)
		{
			XmlDocument TmpXmlDoc=addNode.OwnerDocument;
			XmlAttribute AddAttribute=TmpXmlDoc.CreateAttribute(attributeName);
			AddAttribute.Value=attributeValue;
			addNode.Attributes.SetNamedItem(AddAttribute);
		}

		/// <summary>
		/// Erstellt ein Root Element
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public XmlNode AddRoot(string name)
		{
			return InternalXmlDocument.AppendChild(InternalXmlDocument.CreateElement(name));
		}

		/// <summary>
		/// Fügt ein Element mit Inhalt hinzu
		/// </summary>
		/// <param name="path"></param>
		/// <param name="name"></param>
		/// <param name="content"></param>
		/// <returns></returns>
		public XmlNode AddElement(XmlNode node, string name, string content)
		{
			XmlNode AddNode=InternalXmlDocument.CreateElement(name);
			AddNode.InnerText=content;

			return node.AppendChild(AddNode);
		}

		public XmlNode WriteElement(string path, string content)
		{
			if(ExistElement(path)) //Pfad existiert
			{
				XmlNode EditNode=GetNodes(path)[0];
				EditNode.InnerText=content;

				return EditNode;
			}
			else
			{
				CheckPathAndCreateMissingElements(path);
				path=MakePathConfirm(path);

				XmlNodeList SelectedPath=InternalXmlDocument.SelectNodes(path);
				SelectedPath[0].InnerText=content;

				return SelectedPath[0];
			}
		}
		#endregion

		#region Get Funktionen
		public static string GetAttributeAsString(XmlNode node, string attName)
		{
			return node.Attributes.GetNamedItem("name").Value.ToString();
		}

		public List<XmlNode> GetNodes(string path)
		{
			return GetNodes(InternalXmlDocument, path);
		}

		/// <summary>
		/// Sucht nach einem dings
		/// </summary>
		/// <param name="node"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		private static List<XmlNode> GetNodes(XmlNode node, string path)
		{
			List<XmlNode> ret=new List<XmlNode>();

			string[] splitted=path.Split('.');

			string restPath="";

			if(splitted.Length>1)
			{
				for(int j=1; j<splitted.Length; j++)
				{
					if(j==splitted.Length-1) restPath+=splitted[j];
					else restPath+=splitted[j]+".";
				}
			}

			foreach(XmlNode i in node.ChildNodes)
			{
				if(i.LocalName==splitted[0])
				{
					if(splitted.Length==1)
					{
						ret.Add(i);
					}
					else
					{
						ret.AddRange(GetNodes(i, restPath));
					}
				}
			}

			return ret;
		}

		/// <summary>
		/// Gibt das erste Gefundende Element ruück welches dem Pfad entspricht
		/// als String zurück
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public string GetElementAsString(string path)
		{
			List<XmlNode> ret=GetNodes(path);
			if(ret.Count==0)
			{
				return "";
			}
			else if(ret[0]==null)
			{
				return "";
			}
			else
			{
				if(ret[0].InnerText==null)
				{
					return "";
				}
				else
				{
                    return ret[0].InnerText.ToString();
				}
			}
		}

		/// <summary>
		/// Gibt das erste Gefundende Element zurück welches dem Pfad entspricht
		/// als XmlNode zurück
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public XmlNode GetElementAsXmlNode(string path)
		{
			return GetElements(path)[0];
		}

		/// <summary>
		/// Gibt die Elemente eines bestimmten Pfades zurück
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public List<XmlNode> GetElements(string path)
		{
			return GetNodes(path);
		}

		/// <summary>
		/// Gibt Alle Elemente eines bestimmten 
		/// Pfades zurück welche ein bestimmtes Attribut 
		/// mit einem bestimmten Wert haben
		/// </summary>
		/// <param name="path"></param>
		/// <param name="attributeName"></param>
		/// <param name="attributeValue"></param>
		/// <returns></returns>
		public List<XmlNode> GetElementsWithSpecificItem(string path, string attributeName, string attributeValue)
		{
			path+="[@"+attributeName+"=\""+attributeValue+"\"]";
			return GetNodes(path);
		}
		#endregion

		#region Sonstige Funktionen
		public bool ExistElement(string path)
		{
			if(GetElements(path).Count==0) return false;
			return true;
		}

		/// <summary>
		/// Gibt die Anzahl der Tags des übergebenen Pfades zurück
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		//public int GetTagCount(string path)
		//{
		//    string xPathString=MakePathConfirm(path);
		//    XmlNodeList SelectedPath=InternalXmlDocument.SelectNodes(xPathString);
		//    return SelectedPath.Count;
		//}
		#endregion

		#region Filework
		/// <summary>
		/// Speichert das XML Dokument unter dem InternalFilename
		/// </summary>
		public void Save()
		{
			InternalXmlDocument.Save(InternalFilename);
		}

		/// <summary>
		/// Speichert ein XML Dokument
		/// </summary>
		/// <param name="filename"></param>
		public void Save(string filename)
		{
			InternalFilename=filename;
			InternalXmlDocument.Save(InternalFilename);
		}
		#endregion
	}
}