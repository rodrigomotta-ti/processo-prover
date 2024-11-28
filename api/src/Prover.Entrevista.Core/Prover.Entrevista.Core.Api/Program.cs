
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Prover.Entrevista.Core.Application;
using Prover.Entrevista.Core.DependencyInjections;
using Prover.Entrevista.Core.Infrastructure.Authentication;
using System.Text;

namespace Prover.Entrevista.Core.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add Enviroment Variables
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddEnvironmentVariables();

        // Configurar mapeamento do JwtSettings
        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

        Console.WriteLine(builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>());

        // Configurar autenticação JWT
        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings!.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
            };
        });

        builder.Services.AddAuthorization();

        // Add Cors
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        });

        builder.Services.AddSwaggerGen(options =>
        {
            var titleBase = "Entrevista - Prover";
            var descriptionBase = "Esta API foi feita para a entrevista da Prover";

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = titleBase + " v1",
                Description = descriptionBase
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization using the Bearer scheme. \r\n\r\n Por favor, insira o token JWT com o prefixo 'Bearer' no campo abaixo.\r\n\r\nExemplo: \"Bearer tokenstring\"",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });

        // Add Dependencies
        builder.Services.DependencyInjections(builder.Configuration);

        // Add services to the container.
        builder.Services.AddControllers();
        
        builder.Services.AddApplication();
        builder.Services.AddDateOnlyTimeOnlyStringConverters();

        var app = builder.Build();
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });

            app.UseHttpsRedirection();
            app.UseDeveloperExceptionPage();
            app.UseCors("CorsPolicy");
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.MapControllers();

            app.MapGet("/", () => "Welcome to running ASP.NET Core API");
            app.Run();
        }
    }
}