using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ngnet.ApiModels.AuthModels;
using Ngnet.Common.Json.Service;
using Ngnet.DbModels.Entities;
using Ngnet.Services.Auth;
using Ngnet.Web.Infrastructure;
using Ngnet.Web.Models.AuthModels;
using System.Linq;
using System.Threading.Tasks;

namespace Ngnet.Web.Controllers
{
    public class AuthController : ApiController
    {
        private readonly IAuthService userService;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IConfiguration configuration;

        public AuthController
            (IAuthService userService,
             UserManager<User> userManager,
             RoleManager<Role> roleManager,
             IConfiguration configuration,
             JsonService jsonService)
            : base(jsonService)
        {
            this.userService = userService;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route(nameof(Register))]
        public async Task<ActionResult> Register(RegisterRequestModel model)
        {
            if (model.Password != model.RepeatPassword)
            {
                var errors = this.GetErrors().NotEqualPasswords;
                return this.BadRequest(errors);
            }

            var user = await this.userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                var errors = this.GetErrors().ExistingUserName;
                return this.BadRequest(errors);
            }

            user = new User
            {
                Email = model.Email,
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var action = await this.userManager.CreateAsync(user, model.Password);
            if (!action.Succeeded)
            {
                return this.BadRequest(action.Errors);
            }

            await this.userManager.AddToRoleAsync(user, "User");

            return this.Ok(this.GetSuccessMsg().UserRegistered);
        }

        [HttpPost]
        [Route(nameof(Login))]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel model)
        {
            var user = await this.userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                var errors = this.GetErrors().InvalidUsername;
                return this.Unauthorized(errors);
            }

            var validPassword = await this.userManager.CheckPasswordAsync(user, model.Password);

            if (!validPassword)
            {
                var errors = this.GetErrors().InvalidPasswords;
                return this.Unauthorized(errors);
            }

            if (user.IsDeleted)
            {
                var errors = this.GetErrors().UserNotFound;
                return this.NotFound(errors);
            }

            string token = this.userService.CreateJwtToken(user.Id, user.UserName, this.configuration["ApplicationSettings:Secret"]);

            var responseMessage = this.GetSuccessMsg().UserLoggedIn;

            return new LoginResponseModel { Token = token, ResponseMessage = responseMessage };
        }

        [Authorize]
        [HttpPost]
        [Route(nameof(All))]
        public async Task<ActionResult<UserResponseModel[]>> All()
        {
            var users = await this.userManager.Users.ToArrayAsync();

            if (users.Length == 0)
            {
                var errors = this.GetErrors().UsersNotFound;
                return this.BadRequest(errors);
            }

            return users.Select(u => new UserResponseModel()
            {
                RoleName = this.userManager.GetRolesAsync(u).GetAwaiter().GetResult().FirstOrDefault(),
                Email = u.Email,
                UserName = u.UserName,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Age = u.Age
            }).ToArray();
        }

        [Authorize]
        [HttpGet]
        [Route(nameof(Profile))]
        public async Task<ActionResult<UserResponseModel>> Profile()
        {
            User user = await this.userManager.FindByIdAsync(this.User.GetId());

            if (user == null)
            {
                var errors = this.GetErrors().UserNotFound;

                return this.Unauthorized(errors);
            }

            return new UserResponseModel()
            {
                RoleName = await this.User.GetRoleAsync(this.userManager),
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age
            };
        }

        [HttpPost]
        [Route(nameof(Update))]
        public async Task<ActionResult> Update(UserRequestModel model)
        {
            int result = await this.userService.Update<UserRequestModel>(model);

            if (result == 0)
            {
                var errors = this.GetErrors().UserNotFound;

                return this.Unauthorized(errors);
            }

            return this.Ok(this.GetSuccessMsg().UserUpdated);
        }
    }
}
