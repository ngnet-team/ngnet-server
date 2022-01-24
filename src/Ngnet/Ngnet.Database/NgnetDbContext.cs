using Microsoft.EntityFrameworkCore;

using Ngnet.Database.Models;

namespace Ngnet.Database
{
    public class NgnetDbContext : DbContext
    {
        public NgnetDbContext(DbContextOptions<NgnetDbContext> options)
            :base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<VehicleCare> VehicleCares { get; set; }

        public DbSet<HealthCare> HealthCares { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //add custom builders
            builder.Entity<VehicleCare>().Property(p => p.Price).HasColumnType("decimal");
            builder.Entity<HealthCare>().Property(p => p.Price).HasColumnType("decimal");

            base.OnModelCreating(builder);
        }
    }
}
