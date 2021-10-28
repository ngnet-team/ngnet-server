using System.ComponentModel.DataAnnotations;

namespace Ngnet.Database.Models
{
    public class Company : BaseModel<int>
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string PhoneNumber { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string WebSite { get; set; }

        [MaxLength(300)]
        public string Address { get; set; }
    }
}
