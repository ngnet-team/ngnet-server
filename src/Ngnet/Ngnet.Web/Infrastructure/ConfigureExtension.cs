using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ngnet.Data;

namespace Ngnet.Web.Infrastructure
{
    public static class ConfigureExtension
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var services = app.ApplicationServices.CreateScope();

            var dbContext = services.ServiceProvider.GetService<NgnetDbContext>();

            dbContext.Database.Migrate();
        }
    }
}
