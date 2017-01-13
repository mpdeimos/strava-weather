using Mpdeimos.StravaWeather.Utils;
using RestEase;

namespace Mpdeimos.StravaWeather.WebApi
{
    public class StravaAuthApiFactory : IServiceFactory<StravaAuthApi>
	{
		public StravaAuthApi Build()
		{
			return RestClient.For<StravaAuthApi>("https://www.strava.com/oauth");
		}
	}
}