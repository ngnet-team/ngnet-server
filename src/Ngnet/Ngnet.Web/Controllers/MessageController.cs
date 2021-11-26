using Microsoft.AspNetCore.Mvc;
using Ngnet.ApiModels;
using Ngnet.ApiModels.CareModels;
using Ngnet.Common.Json.Service;
using Ngnet.Services.Cares.Interfaces;
using Ngnet.Web.Infrastructure;

namespace Ngnet.Web.Controllers.Base
{
    public class MessageController : ApiController
    {
        private readonly JsonService jsonService;
        private ICareBaseService careBaseService;

        public MessageController
            (JsonService jsonService,
            ICareBaseService careBaseService)
            : base(jsonService)
        {
            this.jsonService = jsonService;
            this.careBaseService = careBaseService;
        }

        [HttpPost]
        [Route(nameof(GetReminders))]
        public ActionResult<CareResponseModel[]> GetReminders(TimeModel model)
        {
            return this.careBaseService
                .GetReminders<CareResponseModel>(model, this.User.GetId());
        }
    }
}
