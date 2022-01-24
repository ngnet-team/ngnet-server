using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Ngnet.ApiModels.CareModels;
using Ngnet.Common.Json.Models;
using Ngnet.Common.Json.Service;
using Ngnet.Common;
using Ngnet.Web.Controllers.Base;
using Ngnet.Services.Cares.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Ngnet.Web.Controllers
{
    [Authorize]
    public class VehicleCareController : CareController
    {
        private readonly IVehicleCareService vehicleCareService;

        public VehicleCareController
            (IVehicleCareService vehicleCareService,
            ICareBaseService careBaseService,
            JsonService jsonService,
            IConfiguration configuration)
            : base(careBaseService, jsonService, configuration)
        {
            this.vehicleCareService = vehicleCareService;
        }

        [HttpPost]
        [Route(nameof(Save))]
        public async Task<ActionResult> Save(CareRequestModel model)
        {
            CRUD result = await this.vehicleCareService.SaveAsync(model);

            LanguagesModel msg = this.GetCrudMsg(result);
            return this.Ok(msg);
        }

        [HttpPost]
        [Route(nameof(ById))]
        public ActionResult<CareResponseModel> ById(CareRequestModel model)
        {
            return this.vehicleCareService.GetById<CareResponseModel>(model.Id);
        }

        [HttpPost]
        [Route(nameof(ByUserId))]
        public ActionResult<CareResponseModel[]> ByUserId(CareRequestModel model)
        {
            return this.vehicleCareService.GetByUserId<CareResponseModel>(model.UserId);
        }

        [HttpPost]
        [Route(nameof(Self))]
        public ActionResult<CareResponseModel[]> Self(CareRequestModel model)
        {
            return this.vehicleCareService.GetByUserId<CareResponseModel>(model.UserId);
        }

        [Authorize(/*Roles = "Admin"*/)]
        [HttpPost]
        [Route(nameof(Delete))]
        public async Task<ActionResult> Delete(CareRequestModel model)
        {
            //TODO
            return this.Ok(this.GetSuccessMsg().Deleted);
        }

        [HttpGet]
        [Route(nameof(Names))]
        public ActionResult<LanguagesModel> Names()
        {
            var result = this.vehicleCareService.GetDropdown<LanguagesModel>(Paths.VehicleCareNames);

            if (result == null)
            {
                var errors = this.GetErrors().VehicleCareNamesNotFound;
                return this.NotFound(errors);
            }

            return result;
        }
    }
}
