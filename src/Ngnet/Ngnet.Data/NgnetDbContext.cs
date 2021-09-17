﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

        public DbSet<VehicleCare> VehicleCares { get; set; }

        public DbSet<HealthCare> HealthCares { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //add custom builders
            builder.Entity<VehicleCare>().Property(b => b.Price).HasColumnType("decimal");
            builder.Entity<HealthCare>().Property(b => b.Price).HasColumnType("decimal");

            base.OnModelCreating(builder);
        }
    }
}
