using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Mpdeimos.StravaWeather
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var configurations = new ConfigurationBuilder()
				.AddCommandLine(args)
				.Build();

			var host = new WebHostBuilder()
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseConfiguration(configurations)
				.UseKestrel()
				.UseStartup<Mpdeimos.StravaWeather.Core.Startup>()
				.Build();

			host.Run();
		}
	}
}
