using Microsoft.AspNetCore.Mvc;

namespace Mpdeimos.StravaWeather
{
    [Route("/")]
	public class Controller
	{
		[HttpGet]
		public string Get()
		{
			return "Hello World";
		}
	}
}