using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Mpdeimos.StravaWeather.Config;
using Mpdeimos.StravaWeather.Models;
using Refit;

namespace Mpdeimos.StravaWeather.WebApi
{
    public class Api
	{
		private readonly RefitSettings refitSettings;
		public Api(IOptions<StravaAppConfig> stravaAppConfig)
		{
			this.refitSettings = new RefitSettings
			{
				HttpMessageHandlerFactory = () => new StravaHttpMessageHandler(stravaAppConfig.Value)
			};
		}

		public StravaAuthApi StravaAuth
		{
			get
			{
				return RestService.For<StravaAuthApi>("https://www.strava.com/oauth");
			}
		}

		public StravaApi Strava
		{
			get
			{
				return RestService.For<StravaApi>("https://www.strava.com/api/v3", refitSettings);
			}
		}

		private class StravaHttpMessageHandler : MessageProcessingHandler
		{
			private readonly StravaAppConfig stravaAppConfig;
			public StravaHttpMessageHandler(StravaAppConfig stravaAppConfig)
				: base(new HttpClientHandler())
			{
				this.stravaAppConfig = stravaAppConfig;
			}

			protected override HttpRequestMessage ProcessRequest(HttpRequestMessage request, CancellationToken cancellationToken)
			{
				var query = QueryHelpers.ParseQuery(request.RequestUri.Query);
				if (!query.ContainsKey("access_token"))
				{
					request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", stravaAppConfig.AccessToken);
				}
				return request;
			}

			protected override HttpResponseMessage ProcessResponse(HttpResponseMessage response, CancellationToken cancellationToken)
			{
				return response;
			}
		}
	}
}