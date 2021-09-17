using Ngnet.Data.DbModels;
using Ngnet.Mapper;
using System.ComponentModel.DataAnnotations;

namespace Ngnet.Web.Models.AuthModels
{
    public class LoginRequestModel : IMapTo<User>
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}