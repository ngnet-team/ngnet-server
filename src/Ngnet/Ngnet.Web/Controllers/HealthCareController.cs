using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ngnet.ApiModels;
using Ngnet.ApiModels.HealthModels;
using Ngnet.Common;
using Ngnet.Common.Json.Models;
using Ngnet.Common.Json.Service;
using Ngnet.Data.DbModels;
using Ngnet.Services.Health;
using Ngnet.Web.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace Ngnet.Web.Controllers
{
    public class HealthCareController : ApiController
    {
        private readonly IHealthCareService healthCareService;
        private readonly UserManager<User> userManager;

        public HealthCareController
            (IHealthCareService healthCareService,
            UserManager<User> userManager,
            JsonService jsonService)
            : base(jsonService)
        {
            this.healthCareService = healthCareService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route(nameof(Save))]
        public async Task<ActionResult> Save(HealthCareRequestModel model)
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
                var errors = this.GetError("NoPermissions");
                return this.Unauthorized(errors);
            }

            var result = await this.healthCareService.SaveAsync(model);

            return this.Ok(result);
        }

        [HttpPost]
        [Route(nameof(ById))]
        public ActionResult<HealthCareResponseModel> ById(HealthCareRequestModel model)
        {
            HealthCareResponseModel response = this.healthCareService.GetById<HealthCareResponseModel>(model.Id);

            if (response == null)
            {
                var error = this.GetError("VehicleCareNotFound");
                return this.NotFound(error);
            }

            return response;
        }

        [HttpPost]
        [Route(nameof(ByUserId))]
        public ActionResult<HealthCareResponseModel[]> ByUserId(HealthCareRequestModel model)
        {
            HealthCareResponseModel[] response = this.healthCareService.GetByUserId<HealthCareResponseModel>(model.UserId);

            if (response.Length == 0)
            {
                var error = this.GetError("VehicleCaresNotFound");
                return this.NotFound(error);
            }

            return response;
        }

        [HttpGet]
        [Route(nameof(Self))]
        public ActionResult<HealthCareResponseModel[]> Self()
        {
            HealthCareResponseModel[] response = this.healthCareService.GetByUserId<HealthCareResponseModel>(this.User.GetId());

            if (response.Length == 0)
            {
                var error = this.GetError("HealthCaresNotFound");
                return this.NotFound(error);
            }

            return response;
        }

        [HttpPost]
        [Route(nameof(Delete))]
        public async Task<ActionResult> Delete(HealthCareResponseModel model)
        {
            var role = await this.User.GetRoleAsync(this.userManager);

            if (role != "Admin")
            {
                var error = this.GetError("NoPermissions");
                return this.Unauthorized(error);
            }

            var result = await this.healthCareService.DeleteAsync(model.Id, true);

            if (result == 0)
            {
                var error = this.GetError("HealthCareNotFound");
                return this.NotFound(error);
            }

            return this.Ok(result);
        }

        [HttpGet]
        [Route(nameof(Names))]
        public ActionResult<SimpleDropDownModel> Names()
        {
            var result = this.healthCareService.GetNames<SimpleDropDownModel>();

            if (result == null)
            {
                var error = this.GetError("HealthCareNamesNotFound");
                return this.NotFound(error);
            }

            return result;
        }
    }
}
