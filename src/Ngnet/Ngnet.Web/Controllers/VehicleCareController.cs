using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ngnet.Web.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Ngnet.ApiModels.VehicleModels ;
using Ngnet.Services.Vehicle;

namespace Ngnet.Web.Controllers
{
    [Authorize]
    public class VehicleCareController : ApiController
    {
        private readonly IVehicleCareService vehicleCareService;

        public VehicleCareController(IVehicleCareService vehicleCareService)
        {
            this.vehicleCareService = vehicleCareService;
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

            model.UserId = userId;
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
                return this.Unauthorized(errors);
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
                return this.Unauthorized(errors);
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
                return this.Unauthorized(errors);
            }

            return response;
        }
    }
}
