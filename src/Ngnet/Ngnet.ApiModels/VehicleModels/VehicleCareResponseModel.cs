using AutoMapper;
using Ngnet.ApiModels.CompanyModels;
using Ngnet.Database.Models;
using Ngnet.Mapper;
using System;

namespace Ngnet.ApiModels.VehicleModels
{
    public class VehicleCareResponseModel : IMapFrom<VehicleCare>, IHaveCustomMappings
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
            configuration.CreateMap<VehicleCare, VehicleCareResponseModel>()
                .ForMember(x => x.StartDate, opt => opt.MapFrom(x => x.StartDate.ToString().Substring(0, 10)))
                .ForMember(x => x.EndDate, opt => opt.MapFrom(x => x.EndDate.ToString().Substring(0, 10)))
                .ForMember(x => x.PaidEndDate, opt => opt.MapFrom(x => x.PaidEndDate.ToString().Substring(0, 10)))
                .ForMember(x => x.Reminder, opt => opt.MapFrom(x => x.Reminder.ToString().Substring(0, 10)))
                .ForMember(x => x.DeletedOn, opt => opt.MapFrom(x => x.DeletedOn.ToString().Substring(0, 10)));
        }
    }
}
