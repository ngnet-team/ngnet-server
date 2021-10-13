using AutoMapper;
using Ngnet.Database.Models;
using Ngnet.Mapper;
using System.ComponentModel.DataAnnotations;

namespace Ngnet.ApiModels.CompanyModels
{
    public class CompanyRequestModel : IMapTo<Company>, IHaveCustomMappings
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string WebSite { get; set; }

        public string Address { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<CompanyRequestModel, Company>().ForMember(x => x.Id, opt => opt.Condition(c => c.Id != null));
        }
    }
}
