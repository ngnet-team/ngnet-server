using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ngnet.Common.Json.Models;
using Ngnet.Common.Json.Service;
using Ngnet.Data.DbModels;
using Ngnet.Services.Companies;

namespace Ngnet.Web.Controllers
{
    public class CompanyController : ApiController
    {
        private readonly ICompanyService companyService;
        private readonly UserManager<User> userManager;

        public CompanyController(ICompanyService companyService, UserManager<User> userManager, JsonService jsonService)
            :base (jsonService)
        {
            this.companyService = companyService;
            this.userManager = userManager;
        }

        [HttpGet]
        [Route(nameof(Names))]
        public ActionResult<CompanyDropDownModel> Names()
        {
            var result = this.companyService.GetNames<CompanyDropDownModel>();

            if (result == null)
            {
                var error = this.GetError("CompanyNamesNotFound");
                return this.NotFound(error);
            }

            return result;
        }
    }
}
