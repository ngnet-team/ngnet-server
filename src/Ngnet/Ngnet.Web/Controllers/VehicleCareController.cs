using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ngnet.Web.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Ngnet.ApiModels.VehicleModels ;
using Ngnet.Services.Vehicle;
using Microsoft.AspNetCore.Identity;
using Ngnet.Data.DbModels;
using Ngnet.Common.Json.Models;

namespace Ngnet.Web.Controllers
{
    [Authorize]
    public class VehicleCareController : ApiController
    {
        private readonly IVehicleCareService vehicleCareService;
        private readonly UserManager<User> userManager;

        public VehicleCareController(IVehicleCareService vehicleCareService, UserManager<User> userManager)
        {
            this.vehicleCareService = vehicleCareService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route(nameof(Save))]
        public async Task<ActionResult> Save(VehicleCareRequestModel model)
        {
            string userId = this.User.GetId();
            if (userId == null)
            {
                var errors = this.GetErrors(ValidationMessages.UserNotFound);
                return this.Unauthorized(errors);
            }

            var role = await this.User.GetRoleAsync(this.userManager);

            if (model.UserId == null)
            {
                model.UserId = userId;
            }
            else if (model.UserId != userId && role != "Admin")
            {
                var errors = this.GetErrors(ValidationMessages.NoPermissions);
                return this.Unauthorized(errors);
            }

            var result = await this.vehicleCareService.SaveAsync(model);

            return this.Ok(result);
        }

        [HttpPost]
        [Route(nameof(ById))]
        public ActionResult<VehicleCareResponseModel> ById(VehicleCareRequestModel model)
        {
            VehicleCareResponseModel response = this.vehicleCareService.GetByVehicleCareId<VehicleCareResponseModel>(model.Id);

            if (response == null)
            {
                var errors = this.GetErrors(ValidationMessages.VehicleCareNotFound);
                return this.NotFound(errors);
            }

            return response;
        }

        [HttpPost]
        [Route(nameof(ByUserId))]
        public ActionResult<VehicleCareResponseModel[]> ByUserId(VehicleCareRequestModel model)
        {
            VehicleCareResponseModel[] response = this.vehicleCareService.GetByUserId<VehicleCareResponseModel>(model.UserId);

            if (response.Length == 0)
            {
                var errors = this.GetErrors(ValidationMessages.VehicleCaresNotFound);
                return this.NotFound(errors);
            }

            return response;
        }

        [HttpGet]
        [Route(nameof(Self))]
        public ActionResult<VehicleCareResponseModel[]> Self()
        {
            VehicleCareResponseModel[] response = this.vehicleCareService.GetByUserId<VehicleCareResponseModel>(this.User.GetId());

            if (response.Length == 0)
            {
                var errors = this.GetErrors(ValidationMessages.VehicleCaresNotFound);
                return this.NotFound(errors);
            }

            return response;
        }

        [HttpPost]
        [Route(nameof(Delete))]
        public async Task<ActionResult> Delete(VehicleCareRequestModel model)
        {
            var role = await this.User.GetRoleAsync(this.userManager);

            if (role != "Admin")
            {
                var errors = this.GetErrors(ValidationMessages.NoPermissions);
                return this.Unauthorized(errors);
            }

            var result = await this.vehicleCareService.DeleteAsync(model.Id, true);

            if (result == 0)
            {
                var errors = this.GetErrors(ValidationMessages.VehicleCareNotFound);
                return this.NotFound(errors);
            }

            return this.Ok(result);
        }

        [HttpGet]
        [Route(nameof(Names))]
        public ActionResult<VehicleCareName> Names()
        {
            var result = this.vehicleCareService.GetNames<VehicleCareName>();

            if (result == null)
            {
                var errors = this.GetErrors(ValidationMessages.VehicleCareNamesNotFound);
                return this.NotFound(errors);
            }

            return result;
        }
    }
}
