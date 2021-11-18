using AutoMapper;
using Ngnet.Database.Models;
using Ngnet.Mapper;

namespace Ngnet.ApiModels.AuthModels
{
    public class UserExperienceModel : IMapFrom<UserExperience>
    {
        public string LoggedIn { get; set; }

        public string LoggedOut { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<UserExperience, UserExperienceModel>()
                .ForMember(x => x.LoggedIn, opt => opt.MapFrom(x => x.LoggedIn.GetValueOrDefault().ToShortDateString()))
                .ForMember(x => x.LoggedOut, opt => opt.MapFrom(x => x.LoggedOut.GetValueOrDefault().ToShortDateString()));
        }
    }
}
