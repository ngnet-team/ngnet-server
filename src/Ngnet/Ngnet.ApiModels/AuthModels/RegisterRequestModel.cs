using Ngnet.Database.Models;
using Ngnet.Mapper;
using System.ComponentModel.DataAnnotations;

namespace Ngnet.Web.Models.AuthModels
{
    public class RegisterRequestModel : IMapTo<User>
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [MinLength(6)]
        public string RepeatPassword { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Age { get; set; }
    }
}
