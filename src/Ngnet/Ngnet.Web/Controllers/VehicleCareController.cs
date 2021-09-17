using Microsoft.AspNetCore.Mvc;
using Ngnet.ApiModels;
using System.Threading.Tasks;
using Ngnet.Web.Infrastructure;
using Ngnet.Services.Contracts;
using Microsoft.AspNetCore.Authorization;

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
        [Route(nameof(Single))]
        public VehicleCareResponseModel Single(VehicleCareRequestModel model)
        {
            return this.vehicleCareService.GetByVehicleCareId<VehicleCareResponseModel>(model.Id);
        }

        [HttpPost]
        [Route(nameof(Multiple))]
        public VehicleCareResponseModel[] Multiple(VehicleCareRequestModel model)
        {
            return this.vehicleCareService.GetByUserId<VehicleCareResponseModel>(model.UserId);
        }
    }
}
