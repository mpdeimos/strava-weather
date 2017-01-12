using System;
using System.Net;

namespace Mpdeimos.StravaWeather.Core
{
	public class HttpException : Exception
	{
		public HttpStatusCode StatusCode { get; private set; }

		public HttpException(HttpStatusCode statusCode, string message)
		: base(message)
		{
			this.StatusCode = statusCode;
		}
	}
}