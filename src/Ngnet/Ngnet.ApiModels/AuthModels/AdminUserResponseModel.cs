using AutoMapper;
using Ngnet.Database.Models;
using Ngnet.Mapper;
using System;
using System.Collections.Generic;

namespace Ngnet.ApiModels.AuthModels
{
    public class AdminUserResponseModel : IMapFrom<User>
    {
        public AdminUserResponseModel()
        {
            this.Experiances = new HashSet<UserExperienceModel>();
        }

        public string Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public int? Age { get; set; }

        public string RoleName { get; set; }

        public string CreatedOn { get; set; }

        public ICollection<UserExperienceModel> Experiances { get; set; }

        public string ModifiedOn { get; set; }

        public string DeletedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
