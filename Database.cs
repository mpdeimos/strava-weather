using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Mpdeimos.StravaWeather
{
	public class Database : DbContext
	{
		public DbSet<Activity> Activities { get; set; }

		public Database(DbContextOptions<Database> options)
			: base(options)
		{
			Database.EnsureCreated();
		}

		public class Activity
		{
			[Key]
			public int Id { get; set; }
		}
	}
}