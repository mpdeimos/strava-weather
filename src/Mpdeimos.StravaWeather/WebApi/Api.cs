using Refit;

namespace Mpdeimos.StravaWeather.WebApi
{
	// TODO (MP) solve this by dependency injection
	public static class Api
	{
		public static StravaAuthApi StravaAuth { get { return RestService.For<StravaAuthApi>("https://www.strava.com/oauth"); } }
	}
}