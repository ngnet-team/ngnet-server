using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ngnet.Data.DbModels;

namespace Ngnet.Data
{
    public class NgnetDbContext : IdentityDbContext<User, Role, string>
    {
        public NgnetDbContext(DbContextOptions<NgnetDbContext> options)
            :base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<CarService> CarServices { get; set; }

        public DbSet<HealthService> HealthServices { get; set; }

        public DbSet<CarServiceName> CarServiceNames { get; set; }

        public DbSet<HealthServiceName> HealthServiceNames { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //add custom builders

            base.OnModelCreating(builder);
        }
    }
}
