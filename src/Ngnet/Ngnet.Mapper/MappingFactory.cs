using AutoMapper;
using Ngnet.ApiModels;
using Ngnet.ApiModels.UserModels;
using Ngnet.Data.DbModels;

namespace Ngnet.Mapper
{
    public class MappingFactory : Profile
    {
        public MappingFactory()
        {
            CreateMap<User, UserResponseModel>();
            CreateMap<VehicleCare, VehicleCareRequestModel>();
        }
    }
}
