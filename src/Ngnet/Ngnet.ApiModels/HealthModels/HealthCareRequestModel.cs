using AutoMapper;
using Ngnet.ApiModels.CompanyModels;
using Ngnet.Data.DbModels;
using Ngnet.Mapper;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ngnet.ApiModels.HealthModels
{
    public class HealthCareRequestModel : IMapTo<HealthCare>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? Reminder { get; set; }

        [Range(0, 2147483647)]
        public decimal? Price { get; set; }

        public CompanyRequestModel Company { get; set; }

        public string Notes { get; set; }

        public string UserId { get; set; }

        public bool IsDeleted { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<HealthCareRequestModel, HealthCare>().ForMember(x => x.Id, opt => opt.Condition(c => c.Id != null));
        }
    }
}
