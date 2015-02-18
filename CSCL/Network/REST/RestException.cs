using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace CSCL.Network.REST
{
	public class RestException : Exception
	{
		public HttpStatusCode StatusCode { get; private set; }

		public RestException(string message, HttpStatusCode statusCode)
			: base(message)
		{
			StatusCode=statusCode;
		}
	}
}