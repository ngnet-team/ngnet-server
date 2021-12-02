using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Ngnet.ApiModels.CareModels;
using Ngnet.Common;
using Ngnet.Common.Json.Models;
using Ngnet.Common.Json.Service;
using Ngnet.Database.Models;
using Ngnet.Services.Cares.Interfaces;
using Ngnet.Web.Infrastructure;
using System.Threading.Tasks;

namespace Ngnet.Web.Controllers.Base
{
    public class CareController : ApiController
    {
        private readonly ICareBaseService careBaseService;
        protected readonly UserManager<User> userManager;

        public CareController(
            ICareBaseService careBaseService,
            JsonService jsonService,
            IConfiguration configuration,
            UserManager<User> userManager)
            : base(jsonService, configuration)
        {
            this.careBaseService = careBaseService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route(nameof(RemindToggle))]
        public async Task<ActionResult> RemindToggle(CareRequestModel model)
        {
            CRUD response = await this.careBaseService.RemindToggle(model);
            if (response != CRUD.Updated)
            {
                return null;
            }

            return this.Ok(this.GetSuccessMsg().Updated);
        }

        protected async Task<LanguagesModel> NoPermissions(CareRequestModel model)
        {
            string currUser = this.User.GetId();
            if (currUser == null)
            {
                return this.GetErrors().UserNotFound;
            }

            var role = await this.User.GetRoleAsync(this.userManager);
            if (model.UserId == null)
            {
                model.UserId = currUser;
            }
            else if (model.UserId != currUser && role != RoleType.Admin.ToString())
            {
                return this.GetErrors().NoPermissions;
            }

            return null;
        }

        protected LanguagesModel GetCrudMsg(CRUD result)
        {
            return result == CRUD.Created ? this.GetSuccessMsg().Created :
                   result == CRUD.Updated ? this.GetSuccessMsg().Updated :
                   result == CRUD.Deleted ? this.GetSuccessMsg().Deleted : null;
        }
    }
}
