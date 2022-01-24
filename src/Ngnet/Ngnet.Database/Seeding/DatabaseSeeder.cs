using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ngnet.Database.Seeding
{
    public class DatabaseSeeder : ISeeder
    {
        public DatabaseSeeder()
        {
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
                              //Add Seeders
                          };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, serviceProvider);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
