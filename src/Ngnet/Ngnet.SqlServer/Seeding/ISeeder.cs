using System;
using System.Threading.Tasks;

namespace Ngnet.SqlServer.Seeding
{
    public interface ISeeder
    {
        Task SeedAsync(NgnetDbContext dbContext, IServiceProvider serviceProvider);
    }
}
