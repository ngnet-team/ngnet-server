using AutoMapper;
using Ngnet.ApiModels.CompanyModels;
using Ngnet.Data.DbModels;
using Ngnet.Mapper;

namespace Ngnet.ApiModels.HealthModels
{
    public class HealthCareResponseModel : IMapFrom<HealthCare>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Date { get; set; }

        public string Reminder { get; set; }

        public decimal? Price { get; set; }

        public CompanyResponseModel Company { get; set; }

        public string Notes { get; set; }

        public string UserId { get; set; }

        public bool IsDeleted { get; set; }

        public string DeletedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<HealthCare, HealthCareResponseModel>()
                .ForMember(x => x.Date, opt => opt.MapFrom(x => x.Date.ToString().Substring(0, 10)))
                .ForMember(x => x.Reminder, opt => opt.MapFrom(x => x.Reminder.ToString().Substring(0, 10)))
                .ForMember(x => x.DeletedOn, opt => opt.MapFrom(x => x.DeletedOn.ToString().Substring(0, 10)));
        }
    }
}
