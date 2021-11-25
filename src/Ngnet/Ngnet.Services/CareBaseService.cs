using Ngnet.ApiModels;
using Ngnet.Common.Json.Service;
using Ngnet.Database;
using Ngnet.Mapper;
using Ngnet.Services.Companies;
using System;
using System.Linq;

namespace Ngnet.Services
{
    public class CareBaseService : ICareBaseService
    {
        protected readonly NgnetDbContext database;
        protected readonly JsonService jsonService;
        protected readonly ICompanyService companyService;

        public CareBaseService(NgnetDbContext database, JsonService jsonService, ICompanyService companyService)
        {
            this.database = database;
            this.jsonService = jsonService;
            this.companyService = companyService;
        }

        public T[] GetReminders<T>(TimeModel model, string userId)
        {
            var dateToremind = DateTime.UtcNow;
            dateToremind = dateToremind.AddHours(model.Hours);
            dateToremind = dateToremind.AddDays(model.Days);
            dateToremind = dateToremind.AddDays(model.Weeks * 7);
            dateToremind = dateToremind.AddDays(model.Months * 30);

            T[] cares = new T[1];

            var healthCares = this.database.HealthCares
                .Where(x => x.UserId == userId)
                .Where(x => !x.IsDeleted)
                .Where(x => x.Reminder >= dateToremind)
                .To<T>()
                .ToArray();

            if (healthCares != null)
            {
                cares = cares.Concat(healthCares).ToArray();
            }

            var vehicleCares = this.database.VehicleCares
                .Where(x => x.UserId == userId)
                .Where(x => !x.IsDeleted)
                .Where(x => x.Reminder <= dateToremind)
                .To<T>()
                .ToArray();

            if (vehicleCares != null)
            {
                cares = cares.Concat(vehicleCares).ToArray();
            }

            return cares.Where(x => x != null).ToArray();
        }
    }
}
