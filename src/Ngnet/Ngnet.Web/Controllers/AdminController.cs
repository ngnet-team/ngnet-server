using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ngnet.ApiModels.AdminModels;
using Ngnet.Common;
using Ngnet.Common.Json.Service;
using Ngnet.Database.Models;
using Ngnet.Database.Seeding;
using Ngnet.Services.Auth;
using Ngnet.Services.Email;
using Ngnet.Web.Controllers.Base;
using Ngnet.Web.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ngnet.Web.Controllers
{
    [Authorize(/*Roles = "Admin"*/)]
    public class AdminController : UserController
    {
        public AdminController
            (IAuthService userService,
             UserManager<User> userManager,
             RoleManager<Role> roleManager,
             IConfiguration configuration,
             JsonService jsonService,
             IEmailSenderService emailSenderService)
            : base(userService, userManager, roleManager, configuration, jsonService, emailSenderService)
        {
        }

        [HttpGet]
        [Route(nameof(GetAllUsers))]
        public async Task<ActionResult<AdminUserResponseModel[]>> GetAllUsers()
        {
            var users = await this.userManager.Users
                .OrderByDescending(u => u.CreatedOn)
                .ToArrayAsync();

            if (users.Length == 0)
            {
                this.errors = this.GetErrors().UsersNotFound;
                return this.BadRequest(this.errors);
            }

            var result = users.Select(u => new AdminUserResponseModel()
            {
                Id = u.Id,
                RoleName = this.userManager.GetRolesAsync(u).GetAwaiter().GetResult().FirstOrDefault(),
                Email = u.Email,
                UserName = u.UserName,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Age = u.Age,
                CreatedOn = u.CreatedOn.ToShortDateString(),
                ModifiedOn = u.ModifiedOn != null ? u.ModifiedOn.Value.ToShortDateString() : null,
                DeletedOn = u.DeletedOn != null ? u.DeletedOn.Value.ToShortDateString() : null,
                IsDeleted = u.IsDeleted,
                Experiances = this.userService.GetExperiences(u.Id),
            }).ToArray();

            return result;
        }

        [HttpPost]
        [Route(nameof(ChangeRole))]
        public async Task<ActionResult> ChangeRole(AdminUserRequestModel model)
        {
            //needs better check
            if (model.RoleName != RoleType.Admin.ToString() && model.RoleName != RoleType.User.ToString())
            {
                this.errors = this.GetErrors().NoPermissions;
                return this.BadRequest(this.errors);
            }

            var user = await this.userManager.Users
                .FirstOrDefaultAsync(u => u.Id == model.Id);

            if (user == null)
            {
                this.errors = this.GetErrors().UserNotFound;
                return this.BadRequest(this.errors);
            }

            //The initial Admin can't change his role
            if (this.Admin.UserName == this.Admin.UserName)
            {
                this.errors = this.GetErrors().NoPermissions;
                return this.BadRequest(this.errors);
            }

            string role = await this.User.GetRoleAsync(this.userManager, user);
            if (role == model.RoleName)
            {
                return Ok();
            }

            this.result = await this.userManager.RemoveFromRoleAsync(user, role);
            if (!this.result.Succeeded)
            {
                this.errors = this.GetErrors().NoPermissions;
                return this.Unauthorized(this.errors);
            }

            this.result = await this.userManager.AddToRoleAsync(user, model.RoleName);
            if (!this.result.Succeeded)
            {
                this.errors = this.GetErrors().NoPermissions;
                return this.Unauthorized(this.errors);
            }

            return this.Ok(this.GetSuccessMsg().UserUpdated);
        }

        [Authorize]
        [HttpPost]
        [Route(nameof(ResetPassword))]
        public async Task<ActionResult> ResetPassword(AdminUserRequestModel model)
        {
            User user = await this.userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                this.errors = GetErrors().UserNotFound;
                return this.BadRequest(errors);
            }

            string resetToken = await this.userManager.GeneratePasswordResetTokenAsync(user);

            string newPassword = Guid.NewGuid().ToString().Substring(0, 7);
            this.result = await this.userManager.ResetPasswordAsync(user, resetToken, newPassword);
            if (!result.Succeeded)
            {
                this.errors = GetErrors().NoPermissions;
                return this.BadRequest(errors);
            }

            var response = new
            {
                NewPassword = newPassword,
                Msg = this.GetSuccessMsg().UserUpdated
            };
            return this.Ok(response);
        }

        [Authorize]
        [HttpPost]
        [Route(nameof(Update))]
        public async Task<ActionResult> Update(AdminUserRequestModel model)
        {
            return await this.UpdateBase<AdminUserRequestModel>(model);
        }
    }
}
