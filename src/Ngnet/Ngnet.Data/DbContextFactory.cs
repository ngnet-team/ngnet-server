using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Ngnet.Data
{
    public class DbContextFactory : IDesignTimeDbContextFactory<NgnetDbContext>
    {
        public NgnetDbContext CreateDbContext(string[] args)
        {
            var connectionString = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build()
                .GetSection("ConnectionStrings");

            var builder = new DbContextOptionsBuilder<NgnetDbContext>();
            builder.UseSqlServer(connectionString["DefaultConnection"]);

            return new NgnetDbContext(builder.Options);
        }
    }
}
