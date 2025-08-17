using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserService.Application.Interfaces;
using UserService.Application.UseCases;
using UserService.Infrastructure;
using UserService.Infrastructure.Persistence;

namespace UserService.Api;

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
            c.SwaggerDoc("v1", new() { Title = "User API", Version = "v1" });
            
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

            var jwtScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Вставьте только токен (без 'Bearer ')",
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };
            c.AddSecurityDefinition("Bearer", jwtScheme);
            c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                { jwtScheme, System.Array.Empty<string>() }
            });
        });

        builder.Services.AddSingleton<ITokenRevocationStore, InMemoryTokenRevocationStore>();
        builder.Services
            .AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                var jwtConfig = builder.Configuration.GetSection("Jwt");
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfig["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtConfig["Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtConfig["Key"]!)),
                    ValidateLifetime = true
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async ctx =>
                    {
                        var store = ctx.HttpContext.RequestServices.GetRequiredService<ITokenRevocationStore>();
                        var jti = ctx.Principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                        if (!string.IsNullOrEmpty(jti) && await store.IsRevokedAsync(jti))
                        {
                            ctx.Fail("Token has been revoked");
                        }
                    }
                };
            });

        builder.Services.AddAuthorization();
        builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        builder.Services.AddScoped<UserLoginUseCase>();
        builder.Services.AddScoped<UserRegistrationUseCase>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddDbContext<UserDbContext>(opt =>
            opt.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));
        

        builder.Services.AddScoped<IFavoritesRepository, FavoritesRepository>();
        builder.Services.AddScoped<AddFavoriteUseCase>();
        builder.Services.AddScoped<ClearFavoritesUseCase>();
        

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        
        
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}