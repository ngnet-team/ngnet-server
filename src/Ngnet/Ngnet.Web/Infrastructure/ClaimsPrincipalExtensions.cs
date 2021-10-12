﻿using Microsoft.AspNetCore.Identity;
using Ngnet.DbModels.Entities;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ngnet.Web.Infrastructure
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetId(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.NameIdentifier).Value;

        public async static Task<string> GetRoleAsync(this ClaimsPrincipal user, UserManager<User> userManager)
        {
            User u = await userManager.FindByIdAsync(GetId(user));
            string role = userManager.GetRolesAsync(u).GetAwaiter().GetResult().FirstOrDefault();

            return role;
        }
    }
}
