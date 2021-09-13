using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Ngnet.Data.DbModels
{
    public class User : IdentityUser, IBaseModel
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();

            this.CarNotes = new HashSet<CarService>();
            this.HealthNotes = new HashSet<HealthService>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Age { get; set; }

        public ICollection<CarService> CarNotes { get; set; }

        public ICollection<HealthService> HealthNotes { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
