using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Ngnet.ApiModels;
using Ngnet.Common;
using Ngnet.Common.Json.Service;

namespace Ngnet.Web.Controllers
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
    }
}