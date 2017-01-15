using Microsoft.EntityFrameworkCore;

namespace Mpdeimos.StravaWeather.Models
{
    public class Database : DbContext
	{
		public DbSet<AccessToken> AccessTokens { get; set; }

		public Database(DbContextOptions<Database> options)
			: base(options)
		{
			Database.EnsureCreated();
		}
	}
}