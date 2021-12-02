using AutoMapper;
using Ngnet.ApiModels.CompanyModels;
using Ngnet.Database.Models;
using Ngnet.Mapper;

namespace Ngnet.ApiModels.CareModels
{
    public class CareResponseModel : IMapFrom<VehicleCare>, IMapFrom<HealthCare>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string PaidEndDate { get; set; }

        public string Reminder { get; set; }

        public decimal? Price { get; set; }

        public CompanyResponseModel Company { get; set; }

        public string Notes { get; set; }

        public string UserId { get; set; }

        public bool IsDeleted { get; set; }

        public string DeletedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<VehicleCare, CareResponseModel>()
                .ForMember(x => x.StartDate, opt => opt.MapFrom(x => x.StartDate != null ? x.StartDate.ToString().Substring(0, 10) : null))
                .ForMember(x => x.EndDate, opt => opt.MapFrom(x => x.EndDate != null ? x.EndDate.ToString().Substring(0, 10) : null))
                .ForMember(x => x.PaidEndDate, opt => opt.MapFrom(x => x.PaidEndDate != null ? x.PaidEndDate.ToString().Substring(0, 10) : null))
                .ForMember(x => x.Reminder, opt => opt.MapFrom(x => x.Reminder != null ? x.Reminder.ToString().Substring(0, 10) : null))
                .ForMember(x => x.DeletedOn, opt => opt.MapFrom(x => x.DeletedOn != null ? x.DeletedOn.ToString().Substring(0, 10) : null));

            configuration.CreateMap<HealthCare, CareResponseModel>()
                .ForMember(x => x.StartDate, opt => opt.MapFrom(x => x.StartDate != null ? x.StartDate.ToString().Substring(0, 10) : null))
                .ForMember(x => x.EndDate, opt => opt.MapFrom(x => x.EndDate != null ? x.EndDate.ToString().Substring(0, 10) : null))
                .ForMember(x => x.PaidEndDate, opt => opt.MapFrom(x => x.PaidEndDate != null ? x.PaidEndDate.ToString().Substring(0, 10) : null))
                .ForMember(x => x.Reminder, opt => opt.MapFrom(x => x.Reminder != null ? x.Reminder.ToString().Substring(0, 10) : null))
                .ForMember(x => x.DeletedOn, opt => opt.MapFrom(x => x.DeletedOn != null ? x.DeletedOn.ToString().Substring(0, 10) : null));
        }
    }
}
