using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Mpdeimos.StravaWeather.Config;
using Mpdeimos.StravaWeather.Utils;
using RestEase;

namespace Mpdeimos.StravaWeather.WebApi
{
    public class StravaApiFactory : IServiceFactory<StravaApi>
	{
		private readonly StravaAppConfig stravaAppConfig;
		public StravaApiFactory(IOptions<StravaAppConfig> stravaAppConfig)
		{
			this.stravaAppConfig = stravaAppConfig.Value;
		}

		public StravaApi Build()
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