using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MigrationsService.Infrastructure;

namespace MigrationsService.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddDbContext<MigrationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

        builder.Services.AddScoped<IMigrationRunner, MigrationRunner>();

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHealthChecks()
            .AddCheck<MigrationHealthCheck>("migration_applied");


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        //TODO вынести эту мишуру в контроллер
        app.MapPost("/migrate", async (IMigrationRunner migrationRunner, CancellationToken cancellationToken) =>
            {
                await migrationRunner.ApplyMigrationsAsync(cancellationToken);
                return Results.Ok("Миграции успешно применены.");
            })
            //.RequireAuthorization()
            .WithOpenApi();

        app.MapHealthChecks("/health");
        
        //this is debug stuff yo
        app.MapGet("/healthz", async (HealthCheckService hc) =>
            {
                var report = await hc.CheckHealthAsync();
                return report.Status == HealthStatus.Healthy
                    ? Results.Ok("Healthy")
                    : Results.StatusCode(503);
            })
            .WithName("Healthz")
            .WithOpenApi();

        app.Run();
    }
}