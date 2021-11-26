using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Ngnet.ApiModels.CareModels;
using Ngnet.Common;
using Ngnet.Common.Json.Models;
using Ngnet.Common.Json.Service;
using Ngnet.Database.Models;
using Ngnet.Web.Infrastructure;
using System.Threading.Tasks;

namespace Ngnet.Web.Controllers.Base
{
    public abstract class CareController : ApiController
    {
        protected readonly UserManager<User> userManager;

        protected CareController(
            JsonService jsonService,
            IConfiguration configuration,
            UserManager<User> userManager)
            : base(jsonService, configuration)
        {
            this.userManager = userManager;
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
