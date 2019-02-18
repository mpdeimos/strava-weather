using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestEase;

namespace Mpdeimos.StravaWeather.WebApi
{
	public interface StravaApi
	{
		[Get("/activities/{id}")]
		Task<Activity> GetActivity([Path] long id);

		[Get("athlete/activities")]
		Task<Activity[]> GetActivities([Query("access_token")] string accessToken);

		[Put("/activities/{id}")]
		Task<Activity> SetActivityName([Path] long id, [Query] string name, [Query("access_token")] string accessToken);
	}

	public class Activity
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public Athlete Athlete { get; set; }

		[JsonProperty("start_date")]
		public DateTimeOffset StartDate { get; set; }

		[JsonProperty("elapsed_time")]
		public int ElapsedSeconds { get; set; }

		[JsonProperty("start_latlng")]
		public decimal[] StartLocation { get; set; }

		[JsonProperty("end_latlng")]
		public decimal[] EndLocation { get; set; }

		public DateTimeOffset MeanDate => StartDate.AddSeconds(ElapsedSeconds / 2);

		public decimal[] MeanLocation
		{
			get
			{
				if (StartLocation == null && EndLocation == null)
				{
					return null;
				}

				if (EndLocation == null)
				{
					return StartLocation;
				}

				return new decimal[] { (StartLocation[0] + EndLocation[0]) / 2, (StartLocation[1] + EndLocation[1]) / 2 };
			}
		}
	}

	public class Athlete
	{
		public int Id { get; set; }
	}
}
