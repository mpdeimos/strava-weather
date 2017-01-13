// Taken from https://github.com/ashimoon/dotnetliberty-dependencyinjection
// MIT Licensed

namespace Mpdeimos.StravaWeather.Utils
{
    public interface IServiceFactory<T> where T : class
    {
        T Build();
    }
}