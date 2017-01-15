using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Mpdeimos.StravaWeather.Models;

namespace Mpdeimos.StravaWeather.Controllers
{
	[Route("/main")]
	public class MainController
	{
		private Database db;

		public MainController(Database db)
		{
			this.db = db;
		}
	}
}