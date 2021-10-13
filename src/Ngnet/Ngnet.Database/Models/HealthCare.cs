using System;
using System.ComponentModel.DataAnnotations;

namespace Ngnet.Database.Models
{
    public class HealthCare : BaseModel<string>
    {
        public HealthCare()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string Name { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? Reminder { get; set; }

        [Range(0, 2147483647)]
        public decimal? Price { get; set; }

        public int? CompanyId { get; set; }

        public Company Company { get; set; }

        public string Notes { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
