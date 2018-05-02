using Microsoft.Extensions.Configuration;
using System.Threading;

namespace UserProfile.Api
{
    public class ConfigureSettings
    {
        public static IConfiguration CreateConfiguration()
        {
            var builder = new ConfigurationBuilder()
               // .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
        
            return builder.Build();
        }
    }
}