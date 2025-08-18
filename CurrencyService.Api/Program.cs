using System.Text;
using CurrencyService.Application.Interfaces;
using CurrencyService.Application.UseCases;
using CurrencyService.Infrastructure.External;
using CurrencyService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CurrencyService.Api;

///
public class Program
{
    ///
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            var jwtScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };
            c.AddSecurityDefinition("Bearer", jwtScheme);
            c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                { jwtScheme, Array.Empty<string>() }
            });
        });
        
        var jwt = builder.Configuration.GetSection("Jwt");
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidIssuer   = jwt["Issuer"],
                    ValidateAudience = true,
                    ValidAudience    = jwt["Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!)),
                    ValidateLifetime = true
                };
                // o.MapInboundClaims = false;
            });

        
        builder.Services.AddDbContext<CurrencyDbContext>(opt =>
            opt.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));
        
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