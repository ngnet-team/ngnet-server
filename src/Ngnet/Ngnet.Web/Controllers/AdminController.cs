using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ngnet.ApiModels.AdminModels;
using Ngnet.Common.Json.Models;
using Ngnet.Common.Json.Service;
using Ngnet.Database.Models;
using Ngnet.Services.Auth;
using Ngnet.Services.Email;
using System.Linq;
using System.Threading.Tasks;

namespace Ngnet.Web.Controllers
{
    [Authorize(/*Roles = "Admin"*/)]
    public class AdminController : ApiController
    {
        private IAuthService userService;
        private UserManager<User> userManager;
        private RoleManager<Role> roleManager;
        private IConfiguration configuration;
        private IEmailSenderService emailSenderService;
        private IdentityResult result;
        private LanguagesModel errors;

        public AdminController
            (IAuthService userService,
             UserManager<User> userManager,
             RoleManager<Role> roleManager,
             IConfiguration configuration,
             JsonService jsonService,
             IEmailSenderService emailSenderService)
            : base(jsonService)
        {
            this.userService = userService;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.emailSenderService = emailSenderService;
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
                ModifiedOn = u.ModifiedOn != null ? u.ModifiedOn.Value.ToShortTimeString() : null,
                DeletedOn = u.DeletedOn != null ? u.DeletedOn.Value.ToShortDateString() : null,
                IsDeleted = u.IsDeleted,
                Experiances = this.userService.GetExperiences(u.Id),
            }).ToArray();

            return result;
        }

        [HttpPost]
        [Route(nameof(Update))]
        public async Task<ActionResult> Update(AdminUserRequestModel model)
        {
            var user = await this.userManager.Users
                .FirstOrDefaultAsync(u => u.Id == model.Id);

            if (user == null)
            {
                this.errors = this.GetErrors().UserNotFound;
                return this.BadRequest(this.errors);
            }

            int result = await this.userService.Update<AdminUserRequestModel>(model);

            if (result == 0)
            {
                this.errors = this.GetErrors().UserNotFound;
                return this.Unauthorized(this.errors);
            }

            return this.Ok(this.GetSuccessMsg().UserUpdated);

            // ----- still not ready ----- 
            if (model.PermanentDeletion)
            {
                this.result = await this.userManager.DeleteAsync(user);
            }

            return null;
        }
    }
}
