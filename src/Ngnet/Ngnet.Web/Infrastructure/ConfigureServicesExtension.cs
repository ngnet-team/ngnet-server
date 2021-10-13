using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Ngnet.Database;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Ngnet.Database.Models;
using Ngnet.Services;
using AutoMapper;
using Ngnet.Mapper;
using Ngnet.Services.Email;
using Ngnet.Services.Auth;
using Ngnet.Services.Vehicle;
using Ngnet.Common.Json.Service;
using Ngnet.Services.Health;
using Ngnet.Services.Companies;

namespace Ngnet.Web.Infrastructure
{
    public static class ConfigureServicesExtension
    {
        public static ApplicationSettingsModel GetApplicationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var applicationSettings = configuration.GetSection("ApplicationSettings");
            services.Configure<ApplicationSettingsModel>(applicationSettings);
            return applicationSettings.Get<ApplicationSettingsModel>();
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(c =>
            {
                c.AddProfile(new MappingProfile());
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddDbContext<NgnetDbContext>(options => 
            {
                if (configuration.GetValue<bool>("Database:SqlServer:Use"))
                {
                     options.UseSqlServer(configuration.GetValue<string>("Database:SqlServer:ConnectionString"));
                }
                else if (configuration.GetValue<bool>("Database:SqLite:Use"))
                {
                    options.UseSqlite(configuration.GetValue<string>("Database:SqLite:ConnectionString"));
                }
            });
        }

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<User, Role>(options => 
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<NgnetDbContext>();

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, ApplicationSettingsModel appSettings)
        {
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            //chain the services
            return services
                .AddTransient<IAuthService, AuthService>()
                .AddTransient<ICompanyService, CompanyService>()
                .AddTransient<IVehicleCareService, VehicleCareService>()
                .AddTransient<IHealthCareService, HealthCareService>()
                .AddSingleton<IEmailSenderService, EmailSenderService>(x => new EmailSenderService(configuration.GetSection("EmailSender:Key").ToString()))
                .AddSingleton<JsonService>();
        }
    }
}