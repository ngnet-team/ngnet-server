using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ngnet.Database;
using Ngnet.Database.Seeding;

namespace Ngnet.Web.Infrastructure
{
    public static class ConfigureExtension
    {
        public static void ApplyMigrations(this IApplicationBuilder app, IConfiguration configuration)
        {
            using var servicesScope = app.ApplicationServices.CreateScope();

            var dbContext = servicesScope.ServiceProvider.GetService<NgnetDbContext>();

            dbContext.Database.Migrate();

            var adminSeederModel = configuration.GetSection("Admin").Get<AdminSeederModel>();
            new DatabaseSeeder(adminSeederModel).SeedAsync(dbContext, servicesScope.ServiceProvider).GetAwaiter().GetResult();
        }
    }
}