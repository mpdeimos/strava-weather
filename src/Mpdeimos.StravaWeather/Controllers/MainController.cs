using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mpdeimos.StravaWeather.WebApi;
using Refit;
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

		[HttpGet]
		public List<Activity> Get()
		{
			return db.Activities.ToList();
		}

		[HttpGet("add/{id}")]
		public Activity Add(int id)
		{
			var activity = new Activity{Id=id};
			db.Activities.Add(activity);
			db.SaveChanges();
			return activity;
		}
	}
}