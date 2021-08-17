using Microsoft.AspNetCore.Mvc;

namespace Ngnet.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public abstract class ApiController : ControllerBase
    {
    }
}
