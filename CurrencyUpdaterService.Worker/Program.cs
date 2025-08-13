using CurrencyUpdaterService.Application;
using CurrencyUpdaterService.Infrastructure.External;
using CurrencyUpdaterService.Infrastructure.Persistence;
using CurrencyUpdaterService.Worker.Services;
using Microsoft.EntityFrameworkCore;

namespace CurrencyUpdaterService.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<Worker>();
            builder.Services.AddScoped<ICurrencyApiClient, CurrencyApiClient>();
            builder.Services.AddScoped<ICurrencyUpdateService, CurrencyUpdateService>();
            builder.Services.AddSingleton<CustomServiceScopeFactory>();
            builder.Services.AddHttpClient();
            builder.Services.AddDbContext<CurrencyDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            var host = builder.Build();
            host.Run();
        }
    }
}