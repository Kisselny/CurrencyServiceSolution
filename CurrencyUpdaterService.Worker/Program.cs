using CurrencyUpdaterService.Infrastructure.External;
using Microsoft.Extensions.DependencyInjection;


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