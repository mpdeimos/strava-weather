using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using Mpdeimos.StravaWeather.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Mpdeimos.StravaWeather.Config;
using Mpdeimos.StravaWeather.Utils;
using Microsoft.Extensions.Options;
using Mpdeimos.StravaWeather.WebApi;

namespace Mpdeimos.StravaWeather.Core
{
	public class Startup
	{
		public IConfiguration Configuration { get; private set; }
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddOptions();
			services.Configure<StravaAppConfig>(Configuration.GetSection("StravaApp"));
			services.Configure<DarkSkyConfig>(Configuration.GetSection("DarkSky"));

			services.AddDbContext<Database>(options =>
			{
				var databaseUrl = this.Configuration["DATABASE_URL"];
				if (string.IsNullOrEmpty(databaseUrl))
				{
					Console.WriteLine("Using in-memory db");
					options.UseInMemoryDatabase("strava-weather");
				}
				else
				{
					Console.WriteLine($"Using postgres db");
					options.UseNpgsql(DatabaseUtil.ConvertDatabaseConnectionString(databaseUrl));
				}
			});

			services.AddMvc(config => { config.Filters.Add(typeof(ExceptionHandler)); });

			services.AddSingletonFactory<StravaAuthApi, StravaAuthApiFactory>();
			services.AddSingletonFactory<StravaApi, StravaApiFactory>();
			services.AddSingletonFactory<DarkSkyApi, DarkSkyApiFactory>();
		}

		public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole();
			loggerFactory.AddDebug();

			app.UseMvc();
		}
	}
}
