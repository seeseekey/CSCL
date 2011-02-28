using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace CSCL.Services.Google.Translate
{
	public class Translator
	{
		public static string Translate(string stringToTranslate, string fromLanguage, string toLanguage)
		{
			// make sure that the passed string is not empty or null
			if(!String.IsNullOrEmpty(stringToTranslate))
			{
				// per Google's terms of use, we can only translate
				// a string of up to 5000 characters long
				if(stringToTranslate.Length<=5000)
				{
					const int bufSizeMax=65536;
					const int bufSizeMin=8192;

					try
					{
						// by default format? is text.  
						// so we don't need to send a format? key
						string requestUri="http://ajax.googleapis.com/ajax/services/language/translate?v=1.0&q="+stringToTranslate+"&langpair="+fromLanguage+"%7C"+toLanguage;

						// execute the request and get the response stream
						HttpWebRequest request=(HttpWebRequest)WebRequest.Create(requestUri);
						HttpWebResponse response=(HttpWebResponse)request.GetResponse();
						Stream responseStream=response.GetResponseStream();

						// get the length of the content returned by the request
						int length=(int)response.ContentLength;
						int bufSize=bufSizeMin;

						if(length>bufSize) bufSize=length>bufSizeMax?bufSizeMax:length;

						// allocate buffer and StringBuilder for reading response
						byte[] buf=new byte[bufSize];
						StringBuilder sb=new StringBuilder(bufSize);

						// read the whole response
						while((length=responseStream.Read(buf, 0, buf.Length))!=0)
						{
							sb.Append(Encoding.UTF8.GetString(buf, 0, length));
						}

						// the format of the response is like this
						// {"responseData": {"translatedText":"¿Cómo estás?"}, "responseDetails": null, "responseStatus": 200}
						// so now let's clean up the response by manipulating the string
						string translatedText=sb.Remove(0, 36).ToString();
						translatedText=translatedText.Substring(0,
						translatedText.IndexOf("\"},"));

						return translatedText;
					}
					catch
					{
						return "Cannot get the translation.  Please try again later.";
					}
				}
				else
				{
					return "String to translate must be less than 5000 characters long.";
				}
			}
			else
			{
				return "String to translate is empty.";
			}
		}
	}
}
