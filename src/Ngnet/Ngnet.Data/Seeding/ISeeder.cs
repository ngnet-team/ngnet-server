using System;
using System.Threading.Tasks;

namespace Ngnet.Data.Seeding
{
    public interface ISeeder
    {
        Task SeedAsync(NgnetDbContext dbContext, IServiceProvider serviceProvider);
    }
}
