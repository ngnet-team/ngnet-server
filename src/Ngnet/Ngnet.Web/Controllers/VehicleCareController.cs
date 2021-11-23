using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ngnet.Web.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Ngnet.ApiModels.CareModels;
using Ngnet.Services.Vehicle;
using Microsoft.AspNetCore.Identity;
using Ngnet.Database.Models;
using Ngnet.Common.Json.Models;
using Ngnet.Common.Json.Service;
using Ngnet.Common;
using Ngnet.Web.Controllers.Base;

namespace Ngnet.Web.Controllers
{
    [Authorize]
    public class VehicleCareController : ApiController
    {
        private readonly IVehicleCareService vehicleCareService;
        private readonly UserManager<User> userManager;

        public VehicleCareController
            (IVehicleCareService vehicleCareService,
            UserManager<User> userManager,
            JsonService jsonService)
            : base(jsonService)
        {
            this.vehicleCareService = vehicleCareService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route(nameof(Save))]
        public async Task<ActionResult> Save(CareRequestModel model)
        {
            string userId = this.User.GetId();
            if (userId == null)
            {
                var errors = this.GetErrors().UserNotFound;
                return this.Unauthorized(errors);
            }

            var role = await this.User.GetRoleAsync(this.userManager);

            if (model.UserId == null)
            {
                model.UserId = userId;
            }
            else if (model.UserId != userId && role != "Admin")
            {
                var errors = this.GetErrors().NoPermissions;
                return this.Unauthorized(errors);
            }

            CRUD result = await this.vehicleCareService.SaveAsync(model);

            LanguagesModel msg =
                result == CRUD.Created ? this.GetSuccessMsg().Created :
                result == CRUD.Updated ? this.GetSuccessMsg().Updated :
                result == CRUD.Deleted ? this.GetSuccessMsg().Deleted : null;

            return this.Ok(msg);
        }

        [HttpPost]
        [Route(nameof(ById))]
        public ActionResult<CareResponseModel> ById(CareRequestModel model)
        {
            CareResponseModel response = this.vehicleCareService.GetById<CareResponseModel>(model.Id);

            if (response == null)
            {
                var errors = this.GetErrors().VehicleCareNotFound;
                return this.NotFound(errors);
            }

            return response;
        }

        [HttpPost]
        [Route(nameof(ByUserId))]
        public ActionResult<CareResponseModel[]> ByUserId(CareRequestModel model)
        {
            CareResponseModel[] response = this.vehicleCareService.GetByUserId<CareResponseModel>(model.UserId);

            return response;
        }

        [HttpGet]
        [Route(nameof(Self))]
        public ActionResult<CareResponseModel[]> Self()
        {
            CareResponseModel[] response = this.vehicleCareService.GetByUserId<CareResponseModel>(this.User.GetId());

            return response;
        }

        [HttpPost]
        [Route(nameof(Delete))]
        public async Task<ActionResult> Delete(CareRequestModel model)
        {
            var role = await this.User.GetRoleAsync(this.userManager);

            if (role != "Admin")
            {
                var errors = this.GetErrors().NoPermissions;
                return this.Unauthorized(errors);
            }

            var result = await this.vehicleCareService.DeleteAsync(model.Id, true);

            if (result == 0)
            {
                var errors = this.GetErrors().VehicleCareNotFound;
                return this.NotFound(errors);
            }

            return this.Ok(this.GetSuccessMsg().Deleted);
        }

        [HttpGet]
        [Route(nameof(Names))]
        public ActionResult<LanguagesModel> Names()
        {
            var result = this.vehicleCareService.GetNames<LanguagesModel>();

            if (result == null)
            {
                var errors = this.GetErrors().VehicleCareNamesNotFound;
                return this.NotFound(errors);
            }

            return result;
        }
    }
}
