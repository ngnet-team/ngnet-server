﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Ngnet.Data.DbModels;
using System;
using System.Threading.Tasks;

namespace Ngnet.Data.Seeding
{
    public class RoleSeeder : ISeeder
    {
        public async Task SeedAsync(NgnetDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

            foreach (var roleName in Common.Seeding.RoleNames)
            {
                await SeedRoleAsync(roleManager, roleName);
            }
        }

        private async Task SeedRoleAsync(RoleManager<Role> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var result = await roleManager.CreateAsync(new Role(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.ToString());
                }
            }
        }
    }
}
