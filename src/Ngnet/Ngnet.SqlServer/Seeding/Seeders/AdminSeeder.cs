using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Ngnet.Common;
using Ngnet.DbModels.Entities;
using System;
using System.Threading.Tasks;

namespace Ngnet.SqlServer.Seeding.Seeders
{
    public class AdminSeeder : ISeeder
    {
        private readonly AdminSeederModel adminSeederModel;

        public AdminSeeder(AdminSeederModel adminSeederModel)
        {
            if (adminSeederModel?.Email == null || adminSeederModel?.UserName == null || adminSeederModel?.Password == null)
            {
                throw new DataMisalignedException(ValidationMessages.RequiredAppSettingDevelopment, new Exception(ValidationMessages.ByAuthor("Dimitar Sotirov")));
            }

            this.adminSeederModel = adminSeederModel;
        }

        public async Task SeedAsync(NgnetDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            var user = await userManager.FindByNameAsync(this.adminSeederModel.UserName);

            if (user == null)
            {
                user = new User() 
                { 
                    Email = this.adminSeederModel.Email,
                    UserName = this.adminSeederModel.UserName,
                    FirstName = this.adminSeederModel?.FirstName,
                    LastName = this.adminSeederModel?.LastName,
                    CreatedOn = DateTime.UtcNow,
                };
                var result = await userManager.CreateAsync(user, this.adminSeederModel.Password);
                
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.ToString());
                }

                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
