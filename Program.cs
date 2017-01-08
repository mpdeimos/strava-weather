using Microsoft.AspNetCore.Hosting;

namespace Mpdeimos.StravaWeather
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
               .UseKestrel()
               .UseStartup<Startup>()
               .UseUrls("http://localhost:5005")
               .Build();
 
           host.Run();
        }
    }
}
