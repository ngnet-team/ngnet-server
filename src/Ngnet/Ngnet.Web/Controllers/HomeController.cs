using Microsoft.AspNetCore.Mvc;
using Ngnet.Common.Json.Service;
using Ngnet.Web.Controllers.Base;

namespace Ngnet.Web.Controllers
{
    public class HomeController : ApiController
    {
        public HomeController(JsonService jsonService) 
            : base(jsonService)
        {
        }

        public IActionResult Get()
        {
            return Ok("Home works");
        }
    }
}
