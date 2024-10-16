
using Meowgic.API.Extensions;
using Meowgic.Data.Data;
using Meowgic.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Microsoft.EntityFrameworkCore;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using Meowgic.Business.Extension;
using Meowgic.Data.Extension;
using Meowgic.API.Middlewares;
using Meowgic.Business.Mapper;
using Meowgic.Business.Services;
using Meowgic.Data.Models.Response.Account;
using Meowgic.Data.Models;

            var builder = WebApplication.CreateBuilder(args);
         
            //builder.Services.AddControllers();
            //builder.Services.AddHttpContextAccessor();

            var configuration = builder.Configuration;
builder.Services.AddIdentity<Account, Role>()
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddApiDependencies(configuration)
                            .AddBusinessLogicDependencies()
                            .AddDataAccessDependencies();

    var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
    builder.Services.AddSingleton(emailConfig);     
//Add serilog
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
            //builder.Services.AddDbContext<AppDbContext>(options =>
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
            
            // Add services to the container.

           //builder.Services.AddControllers();
            //builder.Services.Configure<IdentityOptions>(options =>
            //{
            //    options.Password.RequireDigit = true;
            //    options.Password.RequireLowercase = true;
            //    options.Password.RequireUppercase = true;
            //    options.Password.RequireNonAlphanumeric = true;
            //    options.Password.RequiredLength = 8;
            //    options.Password.RequiredUniqueChars = 1;
            //    options.User.RequireUniqueEmail = false;
            //    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
            //    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultProvider;
            //}).Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromMinutes(15));
            //builder.Services.AddDataProtection();

            //builder.Services.AddSignalR();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}
            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

app.UseAuthentication();
app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
