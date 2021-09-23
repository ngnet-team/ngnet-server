using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ngnet.Common.Json.Models;
using Ngnet.Data.DbModels;
using Ngnet.Services.Company;
using Ngnet.Web.Infrastructure;

namespace Ngnet.Web.Controllers
{
    public class CompanyController : ApiController
    {
        private readonly ICompanyService companyService;
        private readonly UserManager<User> userManager;
        private object jsonService;

        public CompanyController(ICompanyService companyService, UserManager<User> userManager)
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
                var errors = this.GetErrors(ValidationMessages.CompanyNamesNotFound);
                return this.NotFound(errors);
            }

            return result;
        }
    }
}
