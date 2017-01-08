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
			    .UseConfiguration(configurations)
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();

			host.Run();
		}
	}
}
