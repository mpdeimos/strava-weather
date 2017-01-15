using Microsoft.Extensions.Options;
using Mpdeimos.StravaWeather.Config;
using Mpdeimos.StravaWeather.Utils;
using RestEase;

namespace Mpdeimos.StravaWeather.WebApi
{
    public class DarkSkyApiFactory : IServiceFactory<DarkSkyApi>
	{
		private readonly DarkSkyConfig darkSkyConfig;
		public DarkSkyApiFactory(IOptions<DarkSkyConfig> darkSkyConfig)
		{
			this.darkSkyConfig = darkSkyConfig.Value;
		}

		public DarkSkyApi Build()
		{
			return RestClient.For<DarkSkyApi>($"https://api.darksky.net/forecast/{this.darkSkyConfig.AccessToken}");
		}
	}
}