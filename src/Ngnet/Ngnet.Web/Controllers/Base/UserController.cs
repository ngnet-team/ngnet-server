using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Ngnet.ApiModels.AuthModels;
using Ngnet.Common;
using Ngnet.Common.Json.Models;
using Ngnet.Common.Json.Service;
using Ngnet.Database.Models;
using Ngnet.Services.Auth;
using Ngnet.Services.Email;
using Ngnet.Web.Infrastructure;
using SendGrid;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ngnet.Web.Controllers.Base
{
    public abstract class UserController : ApiController
    {
        protected IAuthService userService;
        protected UserManager<User> userManager;
        protected RoleManager<Role> roleManager;
        protected IEmailSenderService emailSenderService;

        protected IdentityResult result;
        protected LanguagesModel errors;

        protected UserController
            (IAuthService userService,
             UserManager<User> userManager,
             RoleManager<Role> roleManager,
             IConfiguration configuration,
             JsonService jsonService,
             IEmailSenderService emailSenderService)
            : base(jsonService, configuration)
        {
            this.userService = userService;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.emailSenderService = emailSenderService;
        }

        [Authorize]
        [HttpPost]
        [Route(nameof(Change))]
        public async Task<ActionResult> Change(ChangeRequestModel model)
        {
            string userId = model.UserId == null ? this.User.GetId() : model.UserId;
            User user = await this.userManager.FindByIdAsync(userId);

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

        protected async Task<ActionResult> UpdateBase<T>(T model)
        {
            CRUD result = await this.userService.Update<T>(model);

            if (result.Equals(CRUD.NotFound))
            {
                this.errors = this.GetErrors().UserNotFound;
                return this.Unauthorized(this.errors);
            }


            if (result.Equals(CRUD.None))
            {
                return this.Ok();
            }

            return this.Ok(this.GetSuccessMsg().UserUpdated);
        }

        protected async Task<LanguagesModel> EmailValidator(string emailAddress)
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

            EmailSenderModel model = new EmailSenderModel(this.Admin.Email, emailAddress);
            Response response = await this.emailSenderService.EmailConfirmation(model);

            if (response == null || !response.IsSuccessStatusCode)
            {
                return this.GetErrors().InvalidEmail;
            }

            return null;
        }
    }
}
