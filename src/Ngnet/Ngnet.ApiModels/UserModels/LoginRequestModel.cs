using System.ComponentModel.DataAnnotations;

namespace Ngnet.Web.Models.UserModels
{
    public class LoginRequestModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}