using System;
using Npgsql;
using NUnit.Framework;

namespace Mpdeimos.StravaWeather.Models
{
	[TestFixture]
	public class DatabaseUtilTest
	{
		[TestCase("postgres://user:pass@host:1234/name", ExpectedResult="Host=host;Database=name;Port=1234;Username=user;Password=pass")]
		[TestCase("postgres://user@host:1234/name", ExpectedResult="Host=host;Database=name;Port=1234;Username=user")]
		[TestCase("postgres://host:1234/name", ExpectedResult="Host=host;Database=name;Port=1234;Username=")]
		[TestCase("postgres://host/name", ExpectedResult="Host=host;Database=name;Username=")]
		public string TestConvertDatabaseConnectionString(string herokuDatabaseConnection)
		{
			return DatabaseUtil.ConvertDatabaseConnectionString(herokuDatabaseConnection);
		}
	}
}