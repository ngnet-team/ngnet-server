using Microsoft.AspNetCore.Identity;
using Ngnet.Database.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ngnet.Database.Models
{
    public class User : IdentityUser, IBaseModel
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();

            this.VehicleCares = new HashSet<VehicleCare>();
            this.HealthCares = new HashSet<HealthCare>();
            this.Experiences = new HashSet<UserExperience>();
        }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        public string Gender { get; set; }

        public int? Age { get; set; }

        public ICollection<VehicleCare> VehicleCares { get; set; }

        public ICollection<HealthCare> HealthCares { get; set; }

        public DateTime CreatedOn { get; set; }

        public ICollection<UserExperience> Experiences { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
