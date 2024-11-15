using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Filters;
using System.Text.RegularExpressions;
using System.Text;
using Newtonsoft.Json.Serialization;
using Meowgic.Data.Data;
using Meowgic.Shares;
using System.Security.Claims;
using Meowgic.Shares.Enum;
using Meowgic.Business.Services;
using System.Text.Json.Serialization;

namespace Meowgic.API.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddApiDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithConfigurations()
                    .AddDbContextWithConfigurations(configuration)
                    .AddAuthenticationServicesWithConfigurations(configuration)
                    .AddSwaggerConfigurations()
                    .AddCorsConfigurations();

            return services;
        }

        private static IServiceCollection AddDbContextWithConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("Default")!;
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString, b => b.EnableRetryOnFailure()));
            return services;
        }

        private static IServiceCollection AddSwaggerConfigurations(this IServiceCollection services)
        {

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
                {
                    Description =
                        @"JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345example'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                });
                //options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {

                //    Reference = new OpenApiReference
                //    {
                //        Id = "Bearer",
                //        Type = ReferenceType.SecurityScheme
                //    }
                //        },
                //        new List<string>()
                //    }
                //});


                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            return services;
        }

        private static IServiceCollection AddAuthenticationServicesWithConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                //options.SaveToken = true;
                //options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JwtAuth:Audience"],
                    ValidIssuer = configuration["JwtAuth:Issuer"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtAuth:Key"]!))
                };
            });
            services.AddAuthorizationBuilder()
                .AddPolicy("Admin", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, Roles.Admin.ToString());
                })
                .AddPolicy("Customer", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, Roles.Customer.ToString());
                })
                .AddPolicy("Staff", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, Roles.Staff.ToString());
                })
                .AddPolicy("Reader", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, Roles.Reader.ToString());
                });
            return services;
        }

        private static IServiceCollection AddControllersWithConfigurations(this IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            services.AddControllers()
                .AddJsonOptions(options =>
                 { 
          options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
                     options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });
            return services;
        }

        private static IServiceCollection AddCorsConfigurations(this IServiceCollection services)
        {
            services.AddCors(options =>
                options.AddPolicy("AllowAll", b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
            return services;
        }

    }
}
