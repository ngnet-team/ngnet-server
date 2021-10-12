﻿using Ngnet.DbModels.Entities;
using Ngnet.Mapper;

namespace Ngnet.ApiModels.AuthModels
{
    public class UserResponseModel : IMapFrom<User>
    {
        public string Email { get; set; }

        public string UserName { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Age { get; set; }

        public string RoleName { get; set; }
    }
}
