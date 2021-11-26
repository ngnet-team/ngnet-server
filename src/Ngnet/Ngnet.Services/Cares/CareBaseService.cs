using Ngnet.ApiModels;
using Ngnet.Common;
using Ngnet.Common.Json.Service;
using Ngnet.Database;
using Ngnet.Database.Models.Interfaces;
using Ngnet.Mapper;
using Ngnet.Services.Cares.Interfaces;
using Ngnet.Services.Companies;
using System;
using System.Linq;

namespace Ngnet.Services.Cares
{
    public class CareBaseService : ICareBaseService
    {
        protected readonly NgnetDbContext database;
        protected readonly JsonService jsonService;
        protected readonly ICompanyService companyService;

        protected CRUD response;
        protected int result;

        public CareBaseService(NgnetDbContext database, JsonService jsonService, ICompanyService companyService)
        {
            this.database = database;
            this.jsonService = jsonService;
            this.companyService = companyService;
        }

        public T[] GetReminders<T>(TimeModel model, string userId)
        {
            T[] cares = new T[1];

            var healthCares = this.database.HealthCares
                .Where(x => x.UserId == userId)
                .Where(x => !x.IsDeleted)
                .Where(x => x.Reminder >= this.GetTime(model))
                .To<T>()
                .ToArray();

            if (healthCares != null)
            {
                cares = cares.Concat(cares).ToArray();
            }

            var vehicleCares = this.database.VehicleCares
                .Where(x => x.UserId == userId)
                .Where(x => !x.IsDeleted)
                .Where(x => x.Reminder >= this.GetTime(model))
                .To<T>()
                .ToArray();

            if (vehicleCares != null)
            {
                cares = cares.Concat(vehicleCares).ToArray();
            }

            return cares.Where(x => x != null).ToArray();
        }

        public T GetDropdown<T>(string path)
        {
            return this.jsonService.Deserialiaze<T>(path);
        }

        protected ICare ModifyEntity<T>(T apiModel, ICare care)
        {
            var mappedModel = MappingFactory.Mapper.Map<ICare>(apiModel);

            care.Name = mappedModel.Name == null ? care.Name : mappedModel.Name;
            care.StartDate = mappedModel.StartDate == null ? care.StartDate : mappedModel.StartDate;
            care.EndDate = mappedModel.EndDate == null ? care.EndDate : mappedModel.EndDate;
            care.PaidEndDate = mappedModel.PaidEndDate == null ? care.PaidEndDate : mappedModel.PaidEndDate;
            care.Reminder = mappedModel.Reminder == null ? care.Reminder : mappedModel.Reminder;
            care.Price = mappedModel.Price == null ? care.Price : mappedModel.Price;
            care.CompanyId = mappedModel?.Company?.Id == null ? care.CompanyId : mappedModel.Company.Id;
            care.Notes = mappedModel.Notes == null ? care.Notes : mappedModel.Notes;
            care.ModifiedOn = DateTime.UtcNow;
            care.IsDeleted = mappedModel.IsDeleted == true ? mappedModel.IsDeleted : care.IsDeleted;
            care.DeletedOn = mappedModel.IsDeleted == true ? DateTime.UtcNow : care.DeletedOn;

            return care;
        }

        private DateTime GetTime(TimeModel model)
        {
            return DateTime.UtcNow
                .AddHours(model.Hours)
                .AddDays(model.Days)
                .AddDays(model.Weeks * 7)
                .AddDays(model.Months * 30);
        }

        private void SandBox()
        {
            Type baseType = typeof(ICare);
            Type type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => baseType.IsAssignableFrom(x))
                .FirstOrDefault(x => x.Name == "care");
            ICare care = (ICare)Activator.CreateInstance(type);
        }
    }
}
