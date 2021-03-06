using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Mpdeimos.StravaWeather.Config;
using Mpdeimos.StravaWeather.Models;
using Mpdeimos.StravaWeather.WebApi;

namespace Mpdeimos.StravaWeather.Controllers
{
	[Route("strava-connect")]
	public class StravaAuthController : ControllerBase
	{
		private readonly Database db;
		private readonly StravaAppConfig stravaAppConfig;
		private readonly StravaAuthApi api;

		public StravaAuthController(Database db, StravaAuthApi api, IOptions<StravaAppConfig> stravaAppConfig)
		{
			this.stravaAppConfig = stravaAppConfig.Value;
			this.db = db;
			this.api = api;
		}

		[HttpGet]
		public RedirectResult Connect()
		{
			var responseUri = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.Path}/connected";
			var redirectUri = $"https://www.strava.com/oauth/authorize?client_id={stravaAppConfig.ClientId}&response_type=code&redirect_uri={responseUri}&scope=write,view_private";
			return this.Redirect(redirectUri);
		}

		[HttpGet("connected")]
		public async Task<string> Connected(string code)
		{
			var response = await api.GetToken(stravaAppConfig.ClientId, stravaAppConfig.ClientSectret, code);
			var token = this.db.AccessTokens.FirstOrDefault(t => t.UserId == response.Athlete.Id);
			if (token == null)
			{
				token = new AccessToken
				{
					UserId = response.Athlete.Id,
				};
				this.db.AccessTokens.Add(token);
			}
			token.Token = response.AccessToken;

			this.db.SaveChanges();

			return $"Connected {response.Athlete.Firstname} {response.Athlete.Lastname} ({response.Athlete.Id})";
		}

		[HttpGet("users")]
		public List<int> ListConnectedUsers()
		{
			return this.db.AccessTokens.Select(token => token.UserId).ToList();
		}
	}
}