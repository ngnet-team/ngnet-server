using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Ngnet.Common.Json.Service;
using Ngnet.Web.Controllers.Base;

namespace Ngnet.Web.Controllers
{
    public class HomeController : ApiController
    {
        public HomeController(
            JsonService jsonService,
            IConfiguration configuration) 
            : base(jsonService, configuration)
        {
        }

        public IActionResult Get()
        {
            return Ok("Home works");
        }
    }
}
