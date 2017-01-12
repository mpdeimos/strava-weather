using System;
using System.Threading.Tasks;
using Mpdeimos.StravaWeather.Models;
using Newtonsoft.Json;
using RestEase;

namespace Mpdeimos.StravaWeather.WebApi
{
	public interface StravaApi
	{
		[Get("/activities/{id}")]
		Task<Activity> GetActivity([Path] int id);

		[Put("/activities/{id}")]
		Task<Activity> SetActivityName([Path] int id, [Query] string name, [Query("access_token")] string accessToken);
	}

	public class Activity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public Athlete Athlete { get; set; }
		public DateTime StartDate { get; set; }
		public int ElapsedTime { get; set; }

		[JsonProperty("start_latlng")]
		public decimal[] StartLocation { get; set; }

		[JsonProperty("end_latlng")]
		public decimal[] EndLocation { get; set; }

	}

	public class Athlete
	{
		public int Id { get; set; }
	}
}