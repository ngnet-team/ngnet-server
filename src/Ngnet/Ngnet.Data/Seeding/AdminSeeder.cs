using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Ngnet.Data.DbModels;
using System;
using System.Threading.Tasks;

namespace Ngnet.Data.Seeding
{
    public class AdminSeeder : ISeeder
    {
        private readonly AdminSeederModel adminSeederModel;

        public AdminSeeder(AdminSeederModel adminSeederModel)
        {
            if (adminSeederModel.UserName == null || adminSeederModel.Password == null)
            {
                throw new DataMisalignedException("You must create appsetting.Developer.json file with Admin { Username and Password } properties");
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
                    UserName = this.adminSeederModel.UserName,
                    FirstName = this.adminSeederModel?.FirstName,
                    LastName = this.adminSeederModel?.LastName,
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
