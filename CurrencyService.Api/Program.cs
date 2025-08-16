using CurrencyService.Application.Interfaces;
using CurrencyService.Application.UseCases;
using CurrencyService.Infrastructure.External;
using CurrencyService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CurrencyService.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddDbContext<CurrencyDbContext>(opt =>
            opt.UseNpgsql(builder.Configuration.GetConnectionString("CurrencyDb")));
        
        builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        builder.Services.Configure<UserServiceClientOptions>(builder.Configuration.GetSection("UserService"));
        builder.Services.AddHttpClient<IUserFavoritesClient, UserFavoritesHttpClient>();
        builder.Services.AddScoped<GetUserFavoriteCurrenciesUseCase>();
        //builder.Services.AddScoped<GetRatesByCodesUseCase>();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}