using Ngnet.DbModels.Entities;
using Ngnet.Mapper;
using System.ComponentModel.DataAnnotations;

namespace Ngnet.ApiModels.AuthModels
{
    public class UserRequestModel : IMapTo<User>
    {
        public string Id { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string UserName { get; set; }

        [MinLength(6)]
        public string Password { get; set; }

        [MinLength(6)]
        public string RepeatPassword { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Age { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
    }
}
