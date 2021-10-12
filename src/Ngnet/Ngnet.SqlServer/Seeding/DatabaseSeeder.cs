using Ngnet.SqlServer.Seeding.Seeders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ngnet.SqlServer.Seeding
{
    public class DatabaseSeeder : ISeeder
    {
        private readonly AdminSeederModel adminSeederModel;

        public DatabaseSeeder(AdminSeederModel adminSeederModel)
        {
            this.adminSeederModel = adminSeederModel;
        }

        public async Task SeedAsync(NgnetDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var seeders = new List<ISeeder>
                          {
                              new RoleSeeder(),
                              new AdminSeeder(adminSeederModel),
                          };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, serviceProvider);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
