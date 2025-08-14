using Microsoft.EntityFrameworkCore;
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

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.MapPost("/migrate", async (IMigrationRunner migrationRunner) =>
            {
                await migrationRunner.ApplyMigrationsAsync();
                return Results.Ok("Миграции успешно применены.");
            })
            //.RequireAuthorization()
            .WithOpenApi();

        app.Run();
    }
}