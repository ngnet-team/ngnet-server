using Microsoft.AspNetCore.Identity;
using System;

namespace Ngnet.Data.DbModels
{
    public class User : IdentityUser
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
