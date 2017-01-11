using System.Threading.Tasks;
using Newtonsoft.Json;
using Refit;

namespace Mpdeimos.StravaWeather.WebApi
{
	public interface StravaAuthApi
	{
		[Post("/token")]
		Task<OAuthResponse> GetToken([AliasAs("client_id")] int clientId, [AliasAs("client_secret")] string clientSecret, string code);
	}

	public class OAuthResponse
	{
		[JsonProperty(PropertyName="access_token")]
		public string AccessToken { get; set; }

		public StravaAthlete Athlete { get; set; }
	}

	public class StravaAthlete
	{
		public int Id { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }
	}
}