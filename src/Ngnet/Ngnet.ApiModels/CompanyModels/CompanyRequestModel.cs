using Ngnet.Data.DbModels;
using Ngnet.Mapper;
using System.ComponentModel.DataAnnotations;

namespace Ngnet.ApiModels.CompanyModels
{
    public class CompanyRequestModel : IMapTo<Company>
    {
        [Required]
        public string Name { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string WebSite { get; set; }

        public string Address { get; set; }
    }
}
