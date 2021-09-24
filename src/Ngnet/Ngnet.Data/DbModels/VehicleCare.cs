using System;
using System.ComponentModel.DataAnnotations;

namespace Ngnet.Data.DbModels
{
    public class VehicleCare : BaseModel<string>
    {
        public VehicleCare()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? PaidEndDate { get; set; }

        public DateTime? Reminder { get; set; }

        [Range(0, 2147483647)]
        public decimal? Price { get; set; }

        public int? CompanyId { get; set; }

        public Company Company { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
