using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Ngnet.ApiModels.CareModels;
using Ngnet.Common;
using Ngnet.Common.Json.Models;
using Ngnet.Common.Json.Service;
using Ngnet.Services.Cares.Interfaces;
using System.Threading.Tasks;

namespace Ngnet.Web.Controllers.Base
{
    public class CareController : ApiController
    {
        private readonly ICareBaseService careBaseService;

        public CareController(
            ICareBaseService careBaseService,
            JsonService jsonService,
            IConfiguration configuration)
            : base(jsonService, configuration)
        {
            this.careBaseService = careBaseService;
        }

        [HttpPost]
        [Route(nameof(RemindToggle))]
        public async Task<ActionResult> RemindToggle(CareRequestModel model)
        {
            CRUD response = await this.careBaseService.RemindToggle(model);
            if (response != CRUD.Updated)
            {
                return null;
            }

            return this.Ok(this.GetSuccessMsg().Updated);
        }

        protected LanguagesModel GetCrudMsg(CRUD result)
        {
            return result == CRUD.Created ? this.GetSuccessMsg().Created :
                   result == CRUD.Updated ? this.GetSuccessMsg().Updated :
                   result == CRUD.Deleted ? this.GetSuccessMsg().Deleted : null;
        }
    }
}
