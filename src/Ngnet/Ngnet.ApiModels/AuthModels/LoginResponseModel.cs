using Ngnet.Common.Json.Models;

namespace Ngnet.Web.Models.AuthModels
{
    public class LoginResponseModel
    {
        public string Token { get; set; }

        public LanguagesModel ResponseMessage { get; set; }
    }
}
