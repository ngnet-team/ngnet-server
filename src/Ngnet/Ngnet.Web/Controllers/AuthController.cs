using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ngnet.ApiModels;
using Ngnet.Data.DbModels;
using Ngnet.Services;
using Ngnet.Web.Infrastructure;
using Ngnet.Web.Models.UserModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ngnet.Web.Controllers
{
    public class AuthController : ApiController
    {
        private readonly AuthService userService;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public AuthController
            (
             AuthService userService, 
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
                var errors = new List<AuthErrorModel> { new AuthErrorModel(ValidationMessages.NotEquealPasswords) };
                return BadRequest(errors);
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
                return BadRequest(action.Errors);
            }

            await this.userManager.AddToRoleAsync(user, "User");

            return Ok();
        }

        [HttpPost]
        [Route(nameof(Login))]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel model)
        {
            var user = await this.userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                var errors = new List<AuthErrorModel> { new AuthErrorModel(ValidationMessages.InvalidUsername) };
                return Unauthorized(errors);
            }

            var validPassword = await this.userManager.CheckPasswordAsync(user, model.Password);

            if (!validPassword)
            {
                var errors = new List<AuthErrorModel> { new AuthErrorModel(ValidationMessages.InvalidPasswords) };
                return Unauthorized(errors);
            }

            string token = this.userService.CreateJwtToken(user.Id, user.UserName, this.configuration["ApplicationSettings:Secret"]);

            return new LoginResponseModel { Token = token };
        }

        [Authorize]
        [HttpGet]
        [Route(nameof(All))]
        public async Task<UsersResponseModel[]> All()
        {
            var users = await this.userManager.Users.ToArrayAsync();

            return users.Select(u => this.mapper.Map<UsersResponseModel>(u)).ToArray();
        }
    }
}
