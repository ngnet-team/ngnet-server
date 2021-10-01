using Ngnet.ApiModels.HealthModels;
using Ngnet.Common;
using Ngnet.Common.Json.Service;
using Ngnet.Data;
using Ngnet.Data.DbModels;
using Ngnet.Mapper;
using Ngnet.Services.Companies;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ngnet.Services.Health
{
    public class HealthCareService : IHealthCareService
    {
        private readonly NgnetDbContext database;
        private readonly JsonService jsonService;
        private readonly ICompanyService companyService;

        public HealthCareService(NgnetDbContext database, JsonService jsonService, ICompanyService companyService)
        {
            this.database = database;
            this.jsonService = jsonService;
            this.companyService = companyService;
        }

        public async Task<int> DeleteAsync(string healthCareId, bool hardDelete)
        {
            var healthCare = this.database.HealthCares.FirstOrDefault(x => x.Id == healthCareId);

            if (healthCare == null)
            {
                return 0;
            }

            if (hardDelete)
            {
                this.database.HealthCares.Remove(healthCare);
            }
            else
            {
                healthCare.IsDeleted = true;
                healthCare.DeletedOn = DateTime.UtcNow;
            }

            return await this.database.SaveChangesAsync();
        }

        public T[] GetByUserId<T>(string userId)
        {
            return this.database.HealthCares
                .Where(x => x.UserId == userId)
                .To<T>()
                .ToArray();
        }

        public T GetById<T>(string id)
        {
            return this.database.HealthCares
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();
        }

        public T GetNames<T>()
        {
            return this.jsonService.Deserialiaze<T>(Paths.HealthCareNames);
        }

        public async Task<int> SaveAsync(HealthCareRequestModel apiModel)
        {
            HealthCare healthCare = this.database.HealthCares.FirstOrDefault(x => x.Id == apiModel.Id);

            //Create new entity
            if (healthCare == null)
            {
                healthCare = MappingFactory.Mapper.Map<HealthCare>(apiModel);
                await this.database.HealthCares.AddAsync(healthCare);
            }
            else
            {
                bool companyReceived = apiModel?.Company != null;

                if (companyReceived)
                {
                    apiModel.Company.Id = await this.companyService.SaveAsync(apiModel.Company);
                }

                //Modify existing entity
                healthCare = this.ModifyEntity<HealthCareRequestModel>(apiModel, healthCare);
            }

            return await this.database.SaveChangesAsync();
        }

        private HealthCare ModifyEntity<T>(T apiModel, HealthCare healthCare)
        {
            var mappedModel = MappingFactory.Mapper.Map<HealthCare>(apiModel);

            healthCare.Name = mappedModel.Name == null ? healthCare.Name : mappedModel.Name;
            healthCare.Date = mappedModel.Date == null ? healthCare.Date : mappedModel.Date;
            healthCare.Reminder = mappedModel.Reminder == null ? healthCare.Reminder : mappedModel.Reminder;
            healthCare.Price = mappedModel.Price == null ? healthCare.Price : mappedModel.Price;
            healthCare.CompanyId = mappedModel?.Company?.Id == null ? healthCare.CompanyId : mappedModel.Company.Id;
            healthCare.Notes = mappedModel.Notes == null ? healthCare.Notes : mappedModel.Notes;
            healthCare.ModifiedOn = DateTime.UtcNow;
            healthCare.IsDeleted = mappedModel.IsDeleted == true ? mappedModel.IsDeleted : healthCare.IsDeleted;
            healthCare.DeletedOn = mappedModel.IsDeleted == true ? DateTime.UtcNow : healthCare.DeletedOn;

            return healthCare;
        }
    }
}
