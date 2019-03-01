using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mpdeimos.StravaWeather.Core;
using Mpdeimos.StravaWeather.Models;
using Mpdeimos.StravaWeather.WebApi;

namespace Mpdeimos.StravaWeather.Controllers
{
	[Route("athlete")]
	public class StravaAthleteController
	{
		private readonly Database db;
		private readonly StravaApi stravaApi;
		private readonly DarkSkyApi darkSkyApi;
		private readonly ILogger<StravaAthleteController> logger;

		public StravaAthleteController(Database db, StravaApi stravaApi, DarkSkyApi darkSkyApi, ILogger<StravaAthleteController> logger)
		{
			this.logger = logger;
			this.db = db;
			this.stravaApi = stravaApi;
			this.darkSkyApi = darkSkyApi;
		}


		private async Task<dynamic> AttachForecast(Activity activity)
		{
			Forecast forecast = null;
			if (activity.MeanLocation != null)
			{
				forecast = await darkSkyApi.GetWeatherInTime(activity.MeanLocation[0], activity.MeanLocation[1], activity.MeanDate.ToUnixTimeSeconds());
			}
			return new { Activity = activity, Forecast = forecast };
		}

		[HttpGet("{athlete}")]
		public async Task<List<dynamic>> GetActivities(int athlete)
		{
			var token = db.AccessTokens.Where(t => t.UserId == athlete).FirstOrDefault();
			if (token == null)
			{
				throw new HttpException(HttpStatusCode.BadRequest, "Provided athlete is not registered");
			}

			var activities = await stravaApi.GetActivities(token.Token);
			var result = new List<dynamic>();
			foreach (var activity in activities)
			{
				result.Add(await AttachForecast(activity));
			}

			return result;
		}
	}
}