//
//  OnlineHighscore.cs
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
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace CSCL.Games
{
	public class OnlineHighscore
	{
		public string GetPost(string url, string postData)
		{
			Uri requestURL=new Uri(url);

			HttpWebRequest HttpWRequest=(HttpWebRequest)WebRequest.Create(requestURL);
            HttpWRequest.UserAgent = "CSCL";
			HttpWRequest.KeepAlive=true;
			HttpWRequest.Headers.Set("Pragma", "no-cache");
			HttpWRequest.Timeout=300000;
			HttpWRequest.Method="POST";

			// add the content type so we can handle form data
			HttpWRequest.ContentType="application/x-www-form-urlencoded";

			// we need to store the data into a byte array
			byte[] PostData=System.Text.Encoding.ASCII.GetBytes(postData);
			HttpWRequest.ContentLength=PostData.Length;
			Stream tempStream=HttpWRequest.GetRequestStream();
			// write the data to be posted to the Request Stream
			tempStream.Write(PostData, 0, PostData.Length);
			tempStream.Close();

			//get the response. This is where we make the connection to the server
			HttpWebResponse HttpWResponse=(HttpWebResponse)HttpWRequest.GetResponse();
			string state=HttpWResponse.StatusCode.ToString();
			
			//Read the raw HTML from the request
			StreamReader sr=new StreamReader(HttpWResponse.GetResponseStream(), Encoding.ASCII);
			//Convert the stream to a string
			string s=sr.ReadToEnd();
			sr.Close();
			return s; /**/
		}
	}
}
