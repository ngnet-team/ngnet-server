using System;
using System.ComponentModel.DataAnnotations;

namespace Ngnet.Data.DbModels
{
    public class HealthNotes
    {
        [Required]
        public string Name { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? Reminder { get; set; }

        [Range(0, 2147483647)]
        public decimal? Price { get; set; }

        public Company Company { get; set; }

        public string Notes { get; set; }
    }
}
