using System;
using System.Threading.Tasks;

namespace Ngnet.Database.Seeding
{
    public interface ISeeder
    {
        Task SeedAsync(NgnetDbContext dbContext, IServiceProvider serviceProvider);
    }
}
