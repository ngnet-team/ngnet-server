using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ngnet.Database.Models;

namespace Ngnet.Database
{
    public class NgnetDbContext : IdentityDbContext<User, Role, string>
    {
        public NgnetDbContext(DbContextOptions<NgnetDbContext> options)
            :base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<VehicleCare> VehicleCares { get; set; }

        public DbSet<HealthCare> HealthCares { get; set; }

        public DbSet<UserExperience> UserExperiences { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //add custom builders
            builder.Entity<VehicleCare>().Property(p => p.Price).HasColumnType("decimal");
            builder.Entity<HealthCare>().Property(p => p.Price).HasColumnType("decimal");

            builder.Entity<Role>(x => 
            {
                x.Property(p => p.ConcurrencyStamp).HasMaxLength(1000000);
            });

            builder.Entity<User>(x =>
            {
                x.Property(p => p.PasswordHash).HasMaxLength(1000000);
                x.Property(p => p.SecurityStamp).HasMaxLength(1000000);
                x.Property(p => p.ConcurrencyStamp).HasMaxLength(1000000);
                x.Property(p => p.PhoneNumber).HasMaxLength(1000000);
            });

            base.OnModelCreating(builder);
        }
    }
}
