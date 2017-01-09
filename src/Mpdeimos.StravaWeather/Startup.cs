using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using Mpdeimos.StravaWeather.Model;

namespace Mpdeimos.StravaWeather
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<Database>(options =>
			{
				var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
				if (string.IsNullOrEmpty(databaseUrl))
				{
					Console.WriteLine("Using in-memory db");
					options.UseInMemoryDatabase();
				}
				else
				{
					Console.WriteLine($"Using postgres db");
					options.UseNpgsql(DatabaseUtil.ConvertDatabaseConnectionString(databaseUrl));
				}
			});
			services.AddMvc();
		}

		public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole();
			loggerFactory.AddDebug();

			app.UseMvc();
		}
	}
}
