using AutoMapper;
using Ngnet.ApiModels.CompanyModels;
using Ngnet.Data.DbModels;
using Ngnet.Mapper;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ngnet.ApiModels.VehicleModels
{
    public class VehicleCareRequestModel : IMapTo<VehicleCare>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? PaidEndDate { get; set; }

        public DateTime? Reminder { get; set; }

        [Range(0, 2147483647)]
        public decimal? Price { get; set; }

        public CompanyRequestModel Company { get; set; }

        public string Notes { get; set; }

        public string UserId { get; set; }

        public bool IsDeleted { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<VehicleCareRequestModel, VehicleCare>().ForMember(x => x.Id, opt => opt.Condition(c => c.Id != null));
        }
    }
}
