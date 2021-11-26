using System;

namespace Ngnet.Database.Models.Interfaces
{
    public interface ICare : IBaseModel
    {
        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? PaidEndDate { get; set; }

        public DateTime? Reminder { get; set; }

        public decimal? Price { get; set; }

        public int? CompanyId { get; set; }

        public Company Company { get; set; }

        public string Notes { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
