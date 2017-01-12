using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Mpdeimos.StravaWeather.Config;
using RestEase;

namespace Mpdeimos.StravaWeather.WebApi
{
	public class Api
	{
		private readonly StravaAppConfig stravaAppConfig;
		public Api(IOptions<StravaAppConfig> stravaAppConfig)
		{
			this.stravaAppConfig = stravaAppConfig.Value;
		}

		public StravaAuthApi StravaAuth
		{
			get
			{
				return RestClient.For<StravaAuthApi>("https://www.strava.com/oauth");
			}
		}

		public StravaApi Strava
		{
			get
			{
				return RestClient.For<StravaApi>("https://www.strava.com/api/v3", (request, cancellationToken) =>
				{
					var query = QueryHelpers.ParseQuery(request.RequestUri.Query);
					if (!query.ContainsKey("access_token"))
					{
						request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", stravaAppConfig.AccessToken);
					}

					return Task.FromResult(request);
				});
			}
		}
	}
}