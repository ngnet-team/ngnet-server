using Ngnet.Common;
using Ngnet.ApiModels;
using Ngnet.Common.Json.Models;
using Microsoft.AspNetCore.Mvc;
using Ngnet.Common.Json.Service;
using Elmah;

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

        protected SimpleDropDownModel GetError(string errorMessageName)
        {
            ErrorMessagesModel errors = this.jsonService.Deserialiaze<ErrorMessagesModel>(Paths.ErrorMessages);

            SimpleDropDownModel error = (SimpleDropDownModel)DataBinder.Eval(errors, errorMessageName);

            return error;
        }
    }
}