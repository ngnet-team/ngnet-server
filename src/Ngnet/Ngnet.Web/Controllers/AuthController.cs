using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Ngnet.ApiModels.AuthModels;
using Ngnet.Common.Json.Models;
using Ngnet.Common.Json.Service;
using Ngnet.Database.Models;
using Ngnet.Services.Auth;
using Ngnet.Services.Email;
using Ngnet.Web.Infrastructure;
using Ngnet.Web.Models.AuthModels;
using SendGrid;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ngnet.Web.Controllers
{
    public class AuthController : ApiController
    {
        private readonly IAuthService userService;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IConfiguration configuration;
        private readonly IEmailSenderService emailSenderService;
        private IdentityResult result;
        private LanguagesModel errors;

        public AuthController
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
        public async Task<int> Logout()
        {
            return await this.userService.AddExperience(new UserExperience()
            {
                UserId = this.User.GetId(),
                LoggedOut = DateTime.UtcNow
            });
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

        [Authorize]
        [HttpPost]
        [Route(nameof(Update))]
        public async Task<ActionResult> Update(UserRequestModel model)
        {
            model.Id = this.User.GetId();
            int result = await this.userService.Update<UserRequestModel>(model);

            if (result == 0)
            {
                this.errors = this.GetErrors().UserNotFound;
                return this.Unauthorized(this.errors);
            }

            return this.Ok(this.GetSuccessMsg().UserUpdated);
        }

        [Authorize]
        [HttpPost]
        [Route(nameof(Change))]
        public async Task<ActionResult> Change(ChangeRequestModel model)
        {
            User user = await this.userManager.FindByIdAsync(this.User.GetId());

            if (user == null)
            {
                this.errors = this.GetErrors().UserNotFound;
                return this.Unauthorized(this.errors);
            }

            if (model.New != model.RepeatNew)
            {
                this.errors = this.GetErrors().NotEqualFields;
                return this.Unauthorized(this.errors);
            }

            switch (model.Value.ToLower())
            {
                case "email":

                    if (model.Old != user.Email)
                    {
                        this.errors = GetErrors().InvalidEmail;
                        break;
                    }

                    this.errors = await this.EmailValidator(model.New);
                    if (this.errors != null)
                    {
                        this.errors = GetErrors().InvalidEmail;
                        break;
                    }

                    var token = await this.userManager.GenerateChangeEmailTokenAsync(user, model.New);
                    this.result = await this.userManager.ChangeEmailAsync(user, model.New, token);
                    break;

                case "password":

                    if (model.New.Length < 6)
                    {
                        this.errors = this.GetErrors().NotEqualPasswords;
                        break;
                    }

                    this.result = await this.userManager.ChangePasswordAsync(user, model.Old, model.New);
                    break;

                default:
                    break;
            }

            if (errors != null)
            {
                return this.BadRequest(errors);
            }

            if (!this.result.Succeeded)
            {
                return this.BadRequest(result.Errors.FirstOrDefault().Description);
            }

            return this.Ok(this.GetSuccessMsg().UserUpdated);
        }

        // ----------------- Private -----------------

        private async Task<LanguagesModel> EmailValidator(string emailAddress)
        {
            // ------- Local validation ------- 

            string pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"; //needs to be upgraded, copied from: regexr.com/3e48o
            var matching = Regex.IsMatch(emailAddress, pattern);

            if (!matching)
            {
                return this.GetErrors().InvalidEmail;
            }

            return null; // need valid send grid api key before code below...

            // ------- real email validation ------- 

            EmailSenderModel model = new EmailSenderModel() { ToAddress = emailAddress };
            Response response = await this.emailSenderService.SendEmailAsync("Email confirmation", model);

            if (response == null || !response.IsSuccessStatusCode)
            {
                return this.GetErrors().InvalidEmail;
            }

            return null;
        }
    }
}
