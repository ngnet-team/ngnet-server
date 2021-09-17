using Ngnet.ApiModels.CompanyModels;
using Ngnet.Data.DbModels;
using Ngnet.Mapper;
using System;

namespace Ngnet.ApiModels.VehicleModels
{
    public class VehicleCareResponseModel : IMapFrom<VehicleCare>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? PaidEndDate { get; set; }

        public DateTime? Reminder { get; set; }

        public decimal? Price { get; set; }

        public CompanyResponseModel Company { get; set; }

        public string Notes { get; set; }

        public string UserId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
