using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ngnet.Database.Models;
using Ngnet.Common.Json.Models;
using Ngnet.Common.Json.Service;
using Ngnet.Web.Infrastructure;
using System.Threading.Tasks;
using Ngnet.Common;
using Ngnet.ApiModels.CareModels;
using Ngnet.Web.Controllers.Base;
using Ngnet.Services.Cares.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Ngnet.Web.Controllers
{
    [Authorize]
    public class HealthCareController : CareController
    {
        private readonly IHealthCareService healthCareService;

        public HealthCareController
            (IHealthCareService healthCareService,
            UserManager<User> userManager,
            JsonService jsonService)
            : base(jsonService, userManager)
        {
            this.healthCareService = healthCareService;
        }

        [HttpPost]
        [Route(nameof(Save))]
        public async Task<ActionResult> Save(CareRequestModel model)
        {
            var errors = this.NoPermissions(model);
            if (errors != null)
            {
                return this.Unauthorized(errors);
            }

            model.UserId = model.UserId == null ? this.User.GetId() : model.UserId;
            CRUD result = await this.healthCareService.SaveAsync(model);

            LanguagesModel msg = this.GetCrudMsg(result);
            return this.Ok(msg);
        }

        [HttpPost]
        [Route(nameof(ById))]
        public ActionResult<CareResponseModel> ById(CareRequestModel model)
        {
            return this.healthCareService.GetById<CareResponseModel>(model.Id);
        }

        [HttpPost]
        [Route(nameof(ByUserId))]
        public ActionResult<CareResponseModel[]> ByUserId(CareRequestModel model)
        {
            return this.healthCareService.GetByUserId<CareResponseModel>(model.UserId);
        }

        [HttpGet]
        [Route(nameof(Self))]
        public ActionResult<CareResponseModel[]> Self()
        {
            return this.healthCareService.GetByUserId<CareResponseModel>(this.User.GetId());
        }

        [Authorize(/*Roles = "Admin"*/)]
        [HttpPost]
        [Route(nameof(Delete))]
        public async Task<ActionResult> Delete(CareResponseModel model)
        {
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
            var result = this.healthCareService.GetDropdown<LanguagesModel>(Paths.HealthCareNames);

            if (result == null)
            {
                var errors = this.GetErrors().HealthCareNamesNotFound;
                return this.NotFound(errors);
            }

            return result;
        }
    }
}
