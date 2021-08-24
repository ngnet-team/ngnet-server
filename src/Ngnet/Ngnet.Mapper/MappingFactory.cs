using AutoMapper;
using Ngnet.Data.DbModels;
using Ngnet.Web.Models.UserModels;

namespace Ngnet.Mapper
{
    public class MappingFactory : Profile
    {
        public MappingFactory()
        {
            CreateMap<User, UsersResponseModel>();
        }
    }
}
