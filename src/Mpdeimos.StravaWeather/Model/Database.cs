using Microsoft.EntityFrameworkCore;

namespace Mpdeimos.StravaWeather.Model
{
    public class Database : DbContext
	{
		public DbSet<Activity> Activities { get; set; }

		public Database(DbContextOptions<Database> options)
			: base(options)
		{
			Database.EnsureCreated();
		}
	}
}