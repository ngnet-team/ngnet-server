using Ngnet.Common;
using Ngnet.ApiModels.Common;
using Microsoft.AspNetCore.Mvc;
using Ngnet.Common.Json.Service;
using Microsoft.Extensions.Configuration;

namespace Ngnet.Web.Controllers.Base
{
    [ApiController]
    [Route("[controller]")]
    public abstract class ApiController : ControllerBase
    {
        protected readonly JsonService jsonService;
        protected readonly IConfiguration configuration;

        protected ApiController(JsonService jsonService, IConfiguration configuration)
        {
            this.jsonService = jsonService;
            this.configuration = configuration;
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