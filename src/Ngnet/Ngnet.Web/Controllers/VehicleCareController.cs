using Microsoft.AspNetCore.Mvc;
using Ngnet.ApiModels;
using System.Threading.Tasks;
using Ngnet.Web.Infrastructure;
using System;
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
            Console.WriteLine(userId);
            if (userId == null)
            {
                return this.Unauthorized(ValidationMessages.UserNotFound);
                //var errors = this.GetErrors(ValidationMessages.UserNotFound);
                //return this.Unauthorized(errors);
            }

            model.UserId = userId;
            await this.vehicleCareService.SaveAsync(model);

            return this.Ok();
        }
    }
}
