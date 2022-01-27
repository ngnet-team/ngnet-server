using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ngnet.Database;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Ngnet.Mapper;
using Ngnet.Common.Json.Service;
using Ngnet.Services.Companies;
using Ngnet.Services.Cares.Interfaces;
using Ngnet.Services.Cares;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Ngnet.Web.Infrastructure
{
    public static class ConfigureServicesExtension
    {
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
                options.UseSqlServer(configuration.GetValue<string>("Database:SqlServer:ConnectionString")));
        }

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            //chain the services
            return services
                .AddSingleton<JsonService>()
                .AddTransient<ICareBaseService, CareBaseService>()
                .AddTransient<IVehicleCareService, VehicleCareService>()
                .AddTransient<IHealthCareService, HealthCareService>()
                .AddTransient<ICompanyService, CompanyService>();
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes("demo");

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
                        ValidateAudience = false,
                    };
                });

            return services;
        }
    }
}