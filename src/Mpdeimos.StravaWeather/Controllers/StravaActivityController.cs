using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mpdeimos.StravaWeather.Core;
using Mpdeimos.StravaWeather.Models;
using Mpdeimos.StravaWeather.WebApi;

namespace Mpdeimos.StravaWeather.Controllers
{
	[Route("activity")]
	public class StravaActivityController : ControllerBase
	{
		private readonly Database db;
		private readonly StravaApi stravaApi;
		private readonly DarkSkyApi darkSkyApi;
		private readonly ILogger<StravaActivityController> logger;

		public StravaActivityController(Database db, StravaApi stravaApi, DarkSkyApi darkSkyApi, ILogger<StravaActivityController> logger)
		{
			this.logger = logger;
			this.db = db;
			this.stravaApi = stravaApi;
			this.darkSkyApi = darkSkyApi;
		}

		[HttpPost("register")]
		public async Task<Activity> RegisterActivity()
		{
			using (var reader = new StreamReader(this.Request.Body))
			{
				var activityUrl = reader.ReadToEnd().Trim();
				logger.LogInformation($"Url: {activityUrl}");
				var match = Regex.Match(activityUrl, @"https?://.+/(\d+)");
				if (!match.Success)
				{
					throw new HttpException(HttpStatusCode.BadRequest, "Provided url is not an Strava activity url");
				}

				long id = long.Parse(match.Groups[1].Value);
				return await RegisterActivity(id);
			}
		}


		[HttpGet("register/{id}")]
		public async Task<Activity> RegisterActivity(long id)
		{
			var activity = await stravaApi.GetActivity(id);
			if (activity.MeanLocation == null)
			{
				throw new HttpException(HttpStatusCode.BadRequest, $"Activity '{id}' not found or not public");
			}

			var token = db.AccessTokens.Where(t => t.UserId == activity.Athlete.Id).FirstOrDefault();
			logger.LogInformation($"User: {token}");
			if (token == null)
			{
				throw new HttpException(HttpStatusCode.BadRequest, $"Provided athlete '{activity.Athlete.Id}' is not registered");
			}

			var forecast = await darkSkyApi.GetWeatherInTime(activity.MeanLocation[0], activity.MeanLocation[1], activity.MeanDate.ToUnixTimeSeconds());
			int temperature = (int)Math.Round(forecast.Currently.Temperature);
			logger.LogInformation($"Weather: {forecast.BestHourlyMatch.Summary}");
			return await stravaApi.SetActivityName(activity.Id, $"{activity.Name} ({temperature}°C {forecast.BestHourlyMatch.Summary})", token.Token);
		}
	}
}