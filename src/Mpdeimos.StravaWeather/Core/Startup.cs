using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using Mpdeimos.StravaWeather.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Mpdeimos.StravaWeather.Config;
using Mpdeimos.StravaWeather.WebApi;

namespace Mpdeimos.StravaWeather.Core
{
	public class Startup
	{
		public IConfigurationRoot Configuration { get; private set; }
		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

			if (env.IsDevelopment())
			{
				builder.AddUserSecrets();
			}

			builder.AddEnvironmentVariables();
			Configuration = builder.Build();
		}
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddOptions();
			services.Configure<StravaAppConfig>(Configuration.GetSection("StravaAppConfig"));

			services.AddDbContext<Database>(options =>
			{
				var databaseUrl = this.Configuration["DATABASE_URL"];
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

			services.AddMvc(config => { config.Filters.Add(typeof(ExceptionHandler)); });

			services.AddTransient<Api>();
		}

		public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole();
			loggerFactory.AddDebug();

			app.UseMvc();
		}
	}
}
