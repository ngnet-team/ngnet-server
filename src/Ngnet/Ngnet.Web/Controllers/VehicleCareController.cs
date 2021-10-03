using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ngnet.Web.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Ngnet.ApiModels.VehicleModels;
using Ngnet.Services.Vehicle;
using Microsoft.AspNetCore.Identity;
using Ngnet.Data.DbModels;
using Ngnet.Common.Json.Models;
using Ngnet.Common.Json.Service;

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
        public async Task<ActionResult> Save(VehicleCareRequestModel model)
        {
            string userId = this.User.GetId();
            if (userId == null)
            {
                var error = this.GetError("UserNotFound");
                return this.Unauthorized(error);
            }

            var role = await this.User.GetRoleAsync(this.userManager);

            if (model.UserId == null)
            {
                model.UserId = userId;
            }
            else if (model.UserId != userId && role != "Admin")
            {
                var error = this.GetError("NoPermissions");
                return this.Unauthorized(error);
            }

            var result = await this.vehicleCareService.SaveAsync(model);

            return this.Ok(result);
        }

        [HttpPost]
        [Route(nameof(ById))]
        public ActionResult<VehicleCareResponseModel> ById(VehicleCareRequestModel model)
        {
            VehicleCareResponseModel response = this.vehicleCareService.GetById<VehicleCareResponseModel>(model.Id);

            if (response == null)
            {
                var error = this.GetError("VehicleCareNotFound");
                return this.NotFound(error);
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
                var error = this.GetError("VehicleCaresNotFound");
                return this.NotFound(error);
            }

            return response;
        }

        [HttpGet]
        [Route(nameof(Self))]
        public ActionResult<VehicleCareResponseModel[]> Self()
        {
            VehicleCareResponseModel[] response = this.vehicleCareService.GetByUserId<VehicleCareResponseModel>(this.User.GetId());

            return response;
        }

        [HttpPost]
        [Route(nameof(Delete))]
        public async Task<ActionResult> Delete(VehicleCareRequestModel model)
        {
            var role = await this.User.GetRoleAsync(this.userManager);

            if (role != "Admin")
            {
                var error = this.GetError("NoPermissions");
                return this.Unauthorized(error);
            }

            var result = await this.vehicleCareService.DeleteAsync(model.Id, true);

            if (result == 0)
            {
                var error = this.GetError("VehicleCareNotFound");
                return this.NotFound(error);
            }

            return this.Ok(result);
        }

        [HttpGet]
        [Route(nameof(Names))]
        public ActionResult<SimpleDropDownModel> Names()
        {
            var result = this.vehicleCareService.GetNames<SimpleDropDownModel>();

            if (result == null)
            {
                var error = this.GetError("VehicleCareNamesNotFound");
                return this.NotFound(error);
            }

            return result;
        }
    }
}
