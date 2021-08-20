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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //add custom builders

            base.OnModelCreating(builder);
        }
    }
}
