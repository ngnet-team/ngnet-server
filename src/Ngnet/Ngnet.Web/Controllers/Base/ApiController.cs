using Ngnet.Common;
using Ngnet.ApiModels;
using Microsoft.AspNetCore.Mvc;
using Ngnet.Common.Json.Service;

namespace Ngnet.Web.Controllers.Base
{
    [ApiController]
    [Route("[controller]")]
    public abstract class ApiController : ControllerBase
    {
        private readonly JsonService jsonService;

        public ApiController(JsonService jsonService)
        {
            this.jsonService = jsonService;
        }

        protected ErrorMessagesModel GetErrors()
        {
            return this.jsonService.Deserialiaze<ErrorMessagesModel>(Paths.ErrorMessages);
        }

        protected SuccessMessagesModel GetSuccessMsg()
        {
            return this.jsonService.Deserialiaze<SuccessMessagesModel>(Paths.SuccessMessages);
        }
    }
}