using Microsoft.AspNetCore.Mvc;
using Ngnet.ApiModels;
using System.Collections.Generic;

namespace Ngnet.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public abstract class ApiController : ControllerBase
    {
        protected List<AuthErrorModel> GetErrors(string error)
        {
            return new List<AuthErrorModel> { new AuthErrorModel(error) };
        }
    }
}
