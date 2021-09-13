using System.ComponentModel.DataAnnotations;

namespace Ngnet.Data.DbModels
{
    public class Company : BaseModel<int>
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
