using System.ComponentModel.DataAnnotations;

namespace Mpdeimos.StravaWeather.Models
{
	public class AccessToken
	{
		[Key]
		public int UserId { get; set; }

		public string Token { get; set; }
	}
}