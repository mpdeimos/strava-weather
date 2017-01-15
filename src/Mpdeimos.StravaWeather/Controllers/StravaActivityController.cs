using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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


		public StravaActivityController(Database db, StravaApi stravaApi, DarkSkyApi darkSkyApi)
		{
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
				var match = Regex.Match(activityUrl, @"https://.+/(\d+)");
				if (!match.Success)
				{
					throw new HttpException(HttpStatusCode.BadRequest, "Provided url is not an Strava activity url");
				}

				var activity = await stravaApi.GetActivity(int.Parse(match.Groups[1].Value));
				if (activity.MeanLocation == null)
				{
					return null;
				}

				var token = db.AccessTokens.Where(t => t.UserId == activity.Athlete.Id).FirstOrDefault();
				if (token == null)
				{
					throw new HttpException(HttpStatusCode.BadRequest, "Provided athlete is not registered");
				}

				var forecast = await darkSkyApi.GetWeatherInTime(activity.MeanLocation[0], activity.MeanLocation[1], activity.MeanDate.ToUnixTimeSeconds());
				int temperature = (int)Math.Round(forecast.Currently.Temperature);
				return await stravaApi.SetActivityName(activity.Id, $"{activity.Name} ({temperature}°C {forecast.Currently.Summary})", token.Token);
			}
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