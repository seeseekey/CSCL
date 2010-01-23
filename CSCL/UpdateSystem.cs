using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;
using CSCL.Helpers;

namespace CSCL
{
	/// <summary>
	/// Updatesystem für Windows?
	/// </summary>
    public class UpdateSystem
    {
        string i_url;
        string i_current_version;

        public UpdateSystem(string url, string currentVersion)
        {
            i_url=url;
            i_current_version=currentVersion;
        }

        public bool CheckVersion()
        {
            //Datei Downloaden
            WebClient client = new WebClient();
            byte[] padData=client.DownloadData(new Uri(i_url));

            //Datei auswerten
            XmlData xd=new XmlData(padData);
			string xmlver=xd.GetElementAsString("XML_DIZ_INFO.Program_Info.Program_Version");

			if(i_current_version!=xmlver) return true;
			else return false;
        }

		/// <summary>
		/// string updaterTxtURL="http://services.seeseekey.net/updater/updater.txt";
		/// string updaterURL="http://services.seeseekey.net/updater/updater.exe";
		/// </summary>
		/// <param name="updaterTxtURL"></param>
		/// <param name="updaterURL"></param>
		public void Update(string updaterTxtURL,string updaterURL)
		{
            //Pfade und URL Definitionen
            string updaterPath=FileSystem.ApplicationPath+"updater.exe";

            //Instanzen
            WebClient client=new WebClient();

            //Überprüfung ob der Updater existiert
            if(FileSystem.Exists(updaterPath)==false)
            {
                client.DownloadFile(new Uri(updaterURL), updaterPath);
            }

            #region Update holen
            //Pad Datei Downloaden
			byte[] padData=client.DownloadData(new Uri(i_url));

			//Datei auswerten
			XmlData xd=new XmlData(padData);
            string padurl=xd.GetElementAsString("XML_DIZ_INFO.Web_Info.Download_URLs.Secondary_Download_URL");

			//Update Zip downloaden
            string UpdateZipPath=FileSystem.TempPath+FileSystem.GetFilename(padurl, true);
            client.DownloadFile(new Uri(padurl), UpdateZipPath);
            #endregion

            //Überprüfung ob es eine neue Updater Version gibt
            byte[] UpdaterSHA1OnlineData=client.DownloadData(new Uri(updaterTxtURL));
            string UpdaterSHA1Online=Helpers.StringHelpers.ByteArrayToString(UpdaterSHA1OnlineData);
            string UpdaterSHA1Local=Crypto.Hash.SHA1.HashFileToSHA1(updaterPath);

            if(UpdaterSHA1Local!=UpdaterSHA1Online)
            {
                FileSystem.RemoveFile(updaterPath);
                client.DownloadFile(new Uri(updaterURL), updaterPath);
            }

            //Updater starten
            string cmdArgs=String.Format("-FilenameZip:\"{0}\" -PathApplication:\"{1}\" -FilenameApplication:\"{2}\"", UpdateZipPath, FileSystem.ApplicationPath.Trim('\\'), FileSystem.ApplicationPathWithFilename);

            ProcessHelpers.StartProcess(updaterPath, cmdArgs);
            Environment.Exit(0);
		}
    }
}
