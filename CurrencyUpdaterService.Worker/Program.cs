using CurrencyUpdaterService.Application;
using CurrencyUpdaterService.Infrastructure.External;

namespace CurrencyUpdaterService.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<Worker>();
            builder.Services.AddSingleton<ICurrencyApiClient, CurrencyApiClient>();
            builder.Services.AddHttpClient();
            var host = builder.Build();
            host.Run();
        }
    }
}