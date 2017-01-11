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

		public StravaAuthController(IOptions<StravaAppConfig> stravaAppConfig, Database db)
		{
			this.stravaAppConfig = stravaAppConfig.Value;
			this.db = db;
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
			var response = await Api.StravaAuth.GetToken(stravaAppConfig.ClientId, stravaAppConfig.ClientSectret, code);
			this.db.AccessTokens.Add(new AccessToken
			{
				UserId = response.Athlete.Id,
				Token = response.AccessToken
			});
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