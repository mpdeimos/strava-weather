using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Microsoft.Extensions.Logging;
using System;

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
					var databaseUri = new Uri(databaseUrl);
					Console.WriteLine($"Using postgres db {databaseUri.Host}");
					var userInfo = databaseUri.UserInfo.Split(new []{':'}, 2);
					var connection = new NpgsqlConnectionStringBuilder
					{
						Username = userInfo[0],
						Password = userInfo[1],
						Host = databaseUri.Host,
						Port = databaseUri.Port,
						Database = databaseUri.AbsolutePath.Trim('/')
					};
					options.UseNpgsql(connection.ToString());
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
