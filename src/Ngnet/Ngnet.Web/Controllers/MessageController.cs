using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Ngnet.ApiModels;
using Ngnet.ApiModels.CareModels;
using Ngnet.Common.Json.Service;
using Ngnet.Services.Cares.Interfaces;

namespace Ngnet.Web.Controllers.Base
{
    public class MessageController : ApiController
    {
        private ICareBaseService careBaseService;

        public MessageController
            (JsonService jsonService,
            ICareBaseService careBaseService,
            IConfiguration configuration)
            : base(jsonService, configuration)
        {
            this.careBaseService = careBaseService;
        }

        [HttpPost]
        [Route(nameof(GetReminders))]
        public ActionResult<CareResponseModel[]> GetReminders(TimeModel model)
        {
            return this.careBaseService
                .GetReminders<CareResponseModel>(model);
        }
    }
}
