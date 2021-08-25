using Microsoft.AspNetCore.Mvc;
using System;

namespace Ngnet.Web.Controllers
{
    public class HomeController : ApiController
    {
        public IActionResult Get()
        {
            return Ok("Home works");
        }
    }
}
