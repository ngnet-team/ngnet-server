using System;
using System.ComponentModel.DataAnnotations;
using Ngnet.Database.Models.Interfaces;

namespace Ngnet.Database.Models.Base
{
    public class Care : BaseModel<string>, ICare
    {
        public Care()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? PaidEndDate { get; set; }

        public DateTime? Reminder { get; set; }

        public bool Remind { get; set; } = true;

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
