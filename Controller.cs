using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using static Mpdeimos.StravaWeather.Database;

namespace Mpdeimos.StravaWeather
{
	[Route("/")]
	public class Controller
	{
		private Database db;

		public Controller(Database db)
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