using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Ngnet.Common.Json.Models;
using Ngnet.Common.Json.Service;
using Ngnet.Services.Companies;
using Ngnet.Web.Controllers.Base;

namespace Ngnet.Web.Controllers
{
    public class CompanyController : ApiController
    {
        private readonly ICompanyService companyService;

        public CompanyController(
            ICompanyService companyService,
            JsonService jsonService,
            IConfiguration configuration)
            :base (jsonService, configuration)
        {
            this.companyService = companyService;
        }

        [HttpGet]
        [Route(nameof(Names))]
        public ActionResult<CompanyDropDownModel> Names()
        {
            var result = this.companyService.GetNames<CompanyDropDownModel>();

            if (result == null)
            {
                var errors = this.GetErrors().CompanyNamesNotFound;
                return this.NotFound(errors);
            }

            return result;
        }
    }
}
