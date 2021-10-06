using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ngnet.Data.DbModels;
using Ngnet.ApiModels.HealthModels;
using Ngnet.Common.Json.Models;
using Ngnet.Common.Json.Service;
using Ngnet.Services.Health;
using Ngnet.Web.Infrastructure;
using System.Threading.Tasks;
using Ngnet.Common;

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

            CRUD result = await this.healthCareService.SaveAsync(model);

            LanguagesModel msg =
                result == CRUD.Created ? this.GetSuccessMsg().Created :
                result == CRUD.Updated ? this.GetSuccessMsg().Updated :
                result == CRUD.Deleted ? this.GetSuccessMsg().Deleted : null;

            return this.Ok(msg);
        }

        [HttpPost]
        [Route(nameof(ById))]
        public ActionResult<HealthCareResponseModel> ById(HealthCareRequestModel model)
        {
            HealthCareResponseModel response = this.healthCareService.GetById<HealthCareResponseModel>(model.Id);

            if (response == null)
            {
                var errors = this.GetErrors().VehicleCareNotFound;
                return this.NotFound(errors);
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
                var errors = this.GetErrors().VehicleCaresNotFound;
                return this.NotFound(errors);
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
                var errors = this.GetErrors().HealthCaresNotFound;
                return this.NotFound(errors);
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
                var errors = this.GetErrors().NoPermissions;
                return this.Unauthorized(errors);
            }

            var result = await this.healthCareService.DeleteAsync(model.Id, true);

            if (result == 0)
            {
                var errors = this.GetErrors().HealthCareNotFound;
                return this.NotFound(errors);
            }

            return this.Ok(this.GetSuccessMsg().Deleted);
        }

        [HttpGet]
        [Route(nameof(Names))]
        public ActionResult<LanguagesModel> Names()
        {
            var result = this.healthCareService.GetNames<LanguagesModel>();

            if (result == null)
            {
                var errors = this.GetErrors().HealthCareNamesNotFound;
                return this.NotFound(errors);
            }

            return result;
        }
    }
}
