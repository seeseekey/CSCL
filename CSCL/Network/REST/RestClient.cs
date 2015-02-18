using System;
using System.Net;
using System.Text;
using System.IO;

namespace CSCL.Network.REST
{
    public class RestClient
    {
        public string ContentType { get; set; }
        public string EndPoint { get; set; }
        public HttpMethod Method { get; set; }
        public string PostData { get; set; }

        public RestClient(string endpoint="", HttpMethod method=HttpMethod.GET, string postData="")
        {
            EndPoint = endpoint;
            Method = method;
            ContentType = "application/json";
            PostData = postData;
        }

		public string Request(string parameters="")
		{
			HttpStatusCode statusCode;
			return Request(out statusCode, parameters);
		}

		public string Request(out HttpStatusCode statusCode, string parameters="")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(EndPoint + parameters);

            request.Method = Method.ToString();
            request.ContentLength = 0;
            request.ContentType = ContentType;

            if (!string.IsNullOrEmpty(PostData) && Method == HttpMethod.POST)
            {
                var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(PostData);
                request.ContentLength = bytes.Length;

                using (var writeStream = request.GetRequestStream())
                {
                    writeStream.Write(bytes, 0, bytes.Length);
                }
            }

            using(HttpWebResponse response=(HttpWebResponse)request.GetResponse())
            {
                string ret="";

				if(response.StatusCode==HttpStatusCode.OK)
				{
					//Recieve the response
					using(var responseStream=response.GetResponseStream())
					{
						if(responseStream!=null)
						{
							using(var reader=new StreamReader(responseStream))
							{
								ret=reader.ReadToEnd();
							}
						}
					}
				}

				statusCode=response.StatusCode;
				return ret;
            }
        }
    }
}