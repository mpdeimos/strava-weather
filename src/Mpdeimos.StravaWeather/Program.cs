using System.IO;
using System.Linq.Expressions;
using Microsoft.AspNetCore;
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

			BuildWebHost(args, configurations).Run();
		}

		private static IWebHost BuildWebHost(string[] args, IConfigurationRoot configurations)
			=> WebHost.CreateDefaultBuilder(args)
				.UseConfiguration(configurations)
				.UseStartup<Mpdeimos.StravaWeather.Core.Startup>()
				.Build();

	}
}
