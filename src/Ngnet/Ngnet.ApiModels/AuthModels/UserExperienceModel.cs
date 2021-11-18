using Ngnet.Database.Models;
using Ngnet.Mapper;

namespace Ngnet.ApiModels.AuthModels
{
    public class UserExperienceModel : IMapFrom<UserExperience>
    {
        public string LoggedIn { get; set; }

        public string LoggedOut { get; set; }
    }
}
