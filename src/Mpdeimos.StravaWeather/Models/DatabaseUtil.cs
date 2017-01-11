using System;
using Npgsql;

namespace Mpdeimos.StravaWeather.Models
{
	public static class DatabaseUtil
	{
		public static string ConvertDatabaseConnectionString(string herokuDatabaseUrl)
		{
			var databaseUri = new Uri(herokuDatabaseUrl);
			var builder = new NpgsqlConnectionStringBuilder
			{
				Host = databaseUri.Host,
				Database = databaseUri.AbsolutePath.Trim('/')
			};

			if (databaseUri.Port > 0) {
				builder.Port = databaseUri.Port;
			}

			var userInfo = databaseUri.UserInfo.Split(new[] { ':' }, 2);
			if (userInfo.Length > 0)
			{
				builder.Username = userInfo[0];
			}

			if (userInfo.Length > 1)
			{
				builder.Password = userInfo[1];
			}

			return builder.ToString();
		}
	}
}