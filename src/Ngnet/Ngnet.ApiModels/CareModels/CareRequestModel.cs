﻿using AutoMapper;
using Ngnet.ApiModels.CompanyModels;
using Ngnet.Database.Models;
using Ngnet.Mapper;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ngnet.ApiModels.CareModels
{
    public class CareRequestModel : IMapTo<VehicleCare>, IMapTo<HealthCare>, IHaveCustomMappings
    {
        public string Id { get; set; }

        [Required]
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
            configuration.CreateMap<CareRequestModel, VehicleCare>().ForMember(x => x.Id, opt => opt.Condition(c => c.Id != null));
            configuration.CreateMap<CareRequestModel, HealthCare>().ForMember(x => x.Id, opt => opt.Condition(c => c.Id != null));
        }
    }
}