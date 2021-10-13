using System.ComponentModel.DataAnnotations;

namespace Ngnet.Database.Models
{
    public class Company : BaseModel<int>
    {
        [Required]
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string WebSite { get; set; }

        public string Address { get; set; }
    }
}
