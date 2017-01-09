using System.Threading.Tasks;
using Refit;

namespace Mpdeimos.StravaWeather.WebApi
{
		public interface ITestApi
		{
			[Get("/posts/{user}")]
			Task<string> GetUser(string user);
		}
}