using System.Threading.Tasks;
using RestEase;

namespace Mpdeimos.StravaWeather.WebApi
{
	public interface DarkSkyApi
	{
		[Get("{latitude},{longitude}")]
		Task<Forecast> GetWeatherNow([Path] decimal latitude, [Path] decimal longitude, [Query] string units = Forecast.UnitsSi);

		[Get("{latitude},{longitude},{timestamp}")]
		Task<Forecast> GetWeatherInTime([Path] decimal latitude, [Path] decimal longitude, [Path] long timestamp, [Query] string units = Forecast.UnitsSi);
	}

	public class Forecast
	{
		public const string UnitsSi = "si";
		public const string UnitsUs = "us";

		public decimal Latitude { get; set; }
		public decimal Longitude { get; set; }
		public DataPoint Currently { get; set; }
	}

	public class DataPoint
	{
		public int Time { get; set; }

		public string Summary { get; set; }

		public string Icon { get; set; }

		public decimal Temperature { get; set; }

		public string PrecipType { get; set; }
	}
}