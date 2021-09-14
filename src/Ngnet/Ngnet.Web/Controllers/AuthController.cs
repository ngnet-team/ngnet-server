using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ngnet.ApiModels;
using Ngnet.ApiModels.UserModels;
using Ngnet.Data.DbModels;
using Ngnet.Services.Contracts;
using Ngnet.Web.Infrastructure;
using Ngnet.Web.Models.UserModels;
using System.Collections.Generic;
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
        private readonly IMapper mapper;

        public AuthController
            (
             IAuthService userService, 
             UserManager<User> userManager, 
             RoleManager<Role> roleManager, 
             IConfiguration configuration,
             IMapper mapper
            )
        {
            this.userService = userService;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route(nameof(Register))]
        public async Task<ActionResult> Register(RegisterRequestModel model)
        {
            if (model.Password != model.RepeatPassword)
            {
                var errors = this.GetErrors(ValidationMessages.NotEquealPasswords);
                return this.BadRequest(errors);
            }

            var user = new User 
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

            return this.Ok();
        }

        [HttpPost]
        [Route(nameof(Login))]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel model)
        {
            var user = await this.userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                var errors = this.GetErrors(ValidationMessages.InvalidUsername);
                return this.Unauthorized(errors);
            }

            var validPassword = await this.userManager.CheckPasswordAsync(user, model.Password);

            if (!validPassword)
            {
                var errors = this.GetErrors(ValidationMessages.InvalidPasswords);
                return this.Unauthorized(errors);
            }

            string token = this.userService.CreateJwtToken(user.Id, user.UserName, this.configuration["ApplicationSettings:Secret"]);

            return new LoginResponseModel { Token = token };
        }

        [Authorize]
        [HttpPost]
        [Route(nameof(All))]
        public async Task<ActionResult<UserResponseModel[]>> All()
        {
            var users = await this.userManager.Users.ToArrayAsync();

            if (users.Length == 0)
            {
                var errors = this.GetErrors(ValidationMessages.UsersNotFound);
                return this.BadRequest(errors);
            }

            return users.Select(u => this.mapper.Map<UserResponseModel>(u)).ToArray();
        }

        [Authorize]
        [HttpGet]
        [Route(nameof(Profile))]
        public async Task<ActionResult<UserResponseModel>> Profile()
        {
            User user = await this.userManager.FindByIdAsync(this.User.GetId());

            if (user == null)
            {
                var errors = this.GetErrors(ValidationMessages.UserNotFound);
                return this.Unauthorized(errors);
            }

            var response =  this.mapper.Map<UserResponseModel>(user);
            response.RoleName = this.userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();
            return response;
        }

        private List<AuthErrorModel> GetErrors(string error)
        {
            return new List<AuthErrorModel> { new AuthErrorModel(error) };
        }
    }
}
