using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Ngnet.ApiModels.AuthModels;
using Ngnet.Common;
using Ngnet.Common.Json.Service;
using Ngnet.Database.Models;
using Ngnet.Services.Auth;
using Ngnet.Services.Email;
using Ngnet.Web.Controllers.Base;
using Ngnet.Web.Infrastructure;
using Ngnet.Web.Models.AuthModels;
using System;
using System.Threading.Tasks;

namespace Ngnet.Web.Controllers
{
    public class AuthController : UserController
    {
        public AuthController
            (IAuthService userService,
             UserManager<User> userManager,
             RoleManager<Role> roleManager,
             IConfiguration configuration,
             JsonService jsonService,
             IEmailSenderService emailSenderService)
            : base(userService, userManager, roleManager, configuration, jsonService, emailSenderService)
        {
        }

        [HttpPost]
        [Route(nameof(Register))]
        public async Task<ActionResult> Register(RegisterRequestModel model)
        {
            if (model.Password != model.RepeatPassword)
            {
                this.errors = this.GetErrors().NotEqualPasswords;
                return this.BadRequest(this.errors);
            }

            var user = await this.userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                this.errors = this.GetErrors().ExistingUserName;
                return this.BadRequest(this.errors);
            }

            user = new User
            {
                Email = model.Email,
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreatedOn = DateTime.UtcNow
            };

            this.result = await this.userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
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
                this.errors = this.GetErrors().InvalidUsername;
                return this.Unauthorized(this.errors);
            }

            var validPassword = await this.userManager.CheckPasswordAsync(user, model.Password);

            if (!validPassword)
            {
                this.errors = this.GetErrors().InvalidPasswords;
                return this.Unauthorized(this.errors);
            }

            if (user.IsDeleted)
            {
                this.errors = this.GetErrors().UserNotFound;
                return this.NotFound(this.errors);
            }

            var result = this.userService.AddExperience(new UserExperience() 
            { 
                UserId = user.Id,
                LoggedIn = DateTime.UtcNow 
            });

            string token = this.userService.CreateJwtToken(user.Id, user.UserName, this.configuration["ApplicationSettings:Secret"]);

            var responseMessage = this.GetSuccessMsg().UserLoggedIn;

            return new LoginResponseModel { Token = token, ResponseMessage = responseMessage };
        }

        [HttpGet]
        [Route(nameof(Logout))]
        public async Task<ActionResult> Logout()
        {
            CRUD result = await this.userService.AddExperience(new UserExperience()
            {
                UserId = this.User.GetId(),
                LoggedOut = DateTime.UtcNow
            });

            if (result.HasFlag(CRUD.NotFound))
            {
                this.errors = this.GetErrors().UserNotFound;
                return this.Unauthorized(this.errors);
            }

            if (result.HasFlag(CRUD.None))
            {
                return this.Ok();
            }

            return this.Ok(this.GetSuccessMsg().UserUpdated);
        }

        [Authorize]
        [HttpGet]
        [Route(nameof(Profile))]
        public async Task<ActionResult<UserResponseModel>> Profile()
        {
            User user = await this.userManager.FindByIdAsync(this.User.GetId());

            if (user == null)
            {
                this.errors = this.GetErrors().UserNotFound;
                return this.Unauthorized(this.errors);
            }

            return new UserResponseModel()
            {
                RoleName = await this.User.GetRoleAsync(this.userManager),
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                Age = user.Age
            };
        }
    }
}
