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
		private readonly StravaApi api;


		public StravaActivityController(Database db, StravaApi api)
		{
			this.db = db;
			this.api = api;
		}
		
		[HttpPost("register")]
		public async Task<WebApi.Activity> RegisterActivity()
		{
			using (var reader = new StreamReader(this.Request.Body))
			{
				var activityUrl = reader.ReadToEnd().Trim();
				var match = Regex.Match(activityUrl, @"https://.+/(\d+)");
				if (!match.Success)
				{
					throw new HttpException(HttpStatusCode.BadRequest, "Provided url is not an Strava activity url");
				}

				var activity = await api.GetActivity(int.Parse(match.Groups[1].Value));
				var token = db.AccessTokens.Where(t => t.UserId == activity.Athlete.Id).First();
				activity = await api.SetActivityName(activity.Id, activity.Name + ".", token.Token);
				return activity;
			}
		}
	}
}