using System.Threading.Tasks;
using Newtonsoft.Json;
using RestEase;

namespace Mpdeimos.StravaWeather.WebApi
{
	public interface StravaAuthApi
	{
		[Post("/token")]
		Task<OAuthResponse> GetToken([Query("client_id")] int clientId, [Query("client_secret")] string clientSecret, [Query] string code);
	}

	public class OAuthResponse
	{
		[JsonProperty("access_token")]
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