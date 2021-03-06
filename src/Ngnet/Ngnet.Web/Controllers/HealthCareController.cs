using Microsoft.AspNetCore.Mvc;
using Ngnet.Common.Json.Models;
using Ngnet.Common.Json.Service;
using System.Threading.Tasks;
using Ngnet.Common;
using Ngnet.ApiModels.CareModels;
using Ngnet.Web.Controllers.Base;
using Ngnet.Services.Cares.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Ngnet.ApiModels.Common;

namespace Ngnet.Web.Controllers
{
    public class HealthCareController : CareController
    {
        private readonly IHealthCareService healthCareService;

        public HealthCareController
            (IHealthCareService healthCareService,
            ICareBaseService careBaseService,
            JsonService jsonService,
            IConfiguration configuration)
            : base(careBaseService, jsonService, configuration)
        {
            this.healthCareService = healthCareService;
        }

        [HttpPost]
        [Route(nameof(Save))]
        public async Task<ActionResult> Save(CareRequestModel model)
        {
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

        [HttpPost]
        [Route(nameof(Self))]
        public ActionResult<CareResponseModel[]> Self(SimpleRequestModel model)
        {
            return this.healthCareService.GetByUserId<CareResponseModel>(model.UserId);
        }

        [Authorize(/*Roles = "Admin"*/)]
        [HttpPost]
        [Route(nameof(Delete))]
        public async Task<ActionResult> Delete(CareResponseModel model)
        {
            //TODO
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
