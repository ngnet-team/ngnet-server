using System.ComponentModel.DataAnnotations;

namespace Ngnet.ApiModels
{
    public class CompanyRequestModel
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
