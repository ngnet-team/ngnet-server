using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Ngnet.Database.Models
{
    public class User : IdentityUser, IBaseModel
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();

            this.CarNotes = new HashSet<VehicleCare>();
            this.HealthNotes = new HashSet<HealthCare>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Age { get; set; }

        public ICollection<VehicleCare> CarNotes { get; set; }

        public ICollection<HealthCare> HealthNotes { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
