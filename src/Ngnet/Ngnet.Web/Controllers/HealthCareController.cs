﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ngnet.Database.Models;
using Ngnet.Common.Json.Models;
using Ngnet.Common.Json.Service;
using Ngnet.Services.Health;
using Ngnet.Web.Infrastructure;
using System.Threading.Tasks;
using Ngnet.Common;
using Ngnet.ApiModels.CareModels;
using Ngnet.Web.Controllers.Base;

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
        public async Task<ActionResult> Save(CareRequestModel model)
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
        public ActionResult<CareResponseModel> ById(CareRequestModel model)
        {
            CareResponseModel response = this.healthCareService.GetById<CareResponseModel>(model.Id);

            if (response == null)
            {
                var errors = this.GetErrors().VehicleCareNotFound;
                return this.NotFound(errors);
            }

            return response;
        }

        [HttpPost]
        [Route(nameof(ByUserId))]
        public ActionResult<CareResponseModel[]> ByUserId(CareRequestModel model)
        {
            CareResponseModel[] response = this.healthCareService.GetByUserId<CareResponseModel>(model.UserId);

            return response;
        }

        [HttpGet]
        [Route(nameof(Self))]
        public ActionResult<CareResponseModel[]> Self()
        {
            CareResponseModel[] response = this.healthCareService.GetByUserId<CareResponseModel>(this.User.GetId());

            return response;
        }

        [HttpPost]
        [Route(nameof(Delete))]
        public async Task<ActionResult> Delete(CareResponseModel model)
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
