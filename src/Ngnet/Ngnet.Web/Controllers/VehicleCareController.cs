using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ngnet.Web.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Ngnet.ApiModels.CareModels;
using Microsoft.AspNetCore.Identity;
using Ngnet.Database.Models;
using Ngnet.Common.Json.Models;
using Ngnet.Common.Json.Service;
using Ngnet.Common;
using Ngnet.Web.Controllers.Base;
using Ngnet.Services.Cares.Interfaces;

namespace Ngnet.Web.Controllers
{
    [Authorize]
    public class VehicleCareController : CareController
    {
        private readonly IVehicleCareService vehicleCareService;

        public VehicleCareController
            (IVehicleCareService vehicleCareService,
            UserManager<User> userManager,
            JsonService jsonService)
            : base(jsonService, userManager)
        {
            this.vehicleCareService = vehicleCareService;
        }

        [HttpPost]
        [Route(nameof(Save))]
        public async Task<ActionResult> Save(CareRequestModel model)
        {
            var errors = await this.NoPermissions(model);
            if (errors != null)
            {
                return this.Unauthorized(errors);
            }

            model.UserId = model.UserId == null ? this.User.GetId() : model.UserId;
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

        [HttpGet]
        [Route(nameof(Self))]
        public ActionResult<CareResponseModel[]> Self()
        {
            return this.vehicleCareService.GetByUserId<CareResponseModel>(this.User.GetId());
        }

        [Authorize(/*Roles = "Admin"*/)]
        [HttpPost]
        [Route(nameof(Delete))]
        public async Task<ActionResult> Delete(CareRequestModel model)
        {
            var result = await this.vehicleCareService.DeleteAsync(model.Id, true);

            if (result == 0)
            {
                var errors = this.GetErrors().VehicleCareNotFound;
                return this.NotFound(errors);
            }

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
