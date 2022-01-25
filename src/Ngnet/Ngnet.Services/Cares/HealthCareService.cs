using Ngnet.ApiModels.CareModels;
using Ngnet.Common;
using Ngnet.Common.Json.Service;
using Ngnet.Database;
using Ngnet.Database.Models;
using Ngnet.Database.Models.Interfaces;
using Ngnet.Mapper;
using Ngnet.Services.Cares.Interfaces;
using Ngnet.Services.Companies;
using System.Linq;
using System.Threading.Tasks;

namespace Ngnet.Services.Cares
{
    public class HealthCareService : CareBaseService, IHealthCareService
    {
        public HealthCareService(ICompanyService companyService, NgnetDbContext database, JsonService jsonService)
            : base(companyService, database, jsonService)
        {
        }

        public T[] GetByUserId<T>(string userId)
        {
            return this.database.HealthCares
                .Where(x => x.UserId == userId && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedOn)
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

        public async Task<CRUD> SaveAsync(CareRequestModel apiModel)
        {
            this.response = CRUD.None;

            HealthCare healthCare = this.database.HealthCares.FirstOrDefault(x => x.Id == apiModel.Id);
            if (healthCare == null)
    {
                this.response = await this.CreateAsync(apiModel);
            }
            else
            {
                this.response = await this.UpdateAsync(healthCare, apiModel);
            }

            await this.database.SaveChangesAsync();
            return this.response;
        }

        private async Task<CRUD> CreateAsync(CareRequestModel apiModel)
        {
            HealthCare healthCare = MappingFactory.Mapper.Map<HealthCare>(apiModel);
            await this.database.HealthCares.AddAsync(healthCare);

            return CRUD.Created;
        }

        private async Task<CRUD> UpdateAsync(HealthCare healthCare, CareRequestModel apiModel)
        {
            this.response = CRUD.Updated;

            //Modify company
            int companyId = await this.companyService.SaveAsync(apiModel.Company);
            if (companyId != 0)
            {
                apiModel.Company.Id = companyId;
            }

            //Modify existing care entity
            healthCare = (HealthCare)this.ModifyEntity<CareRequestModel>(apiModel, healthCare);

            return CRUD.Updated;
        }

        public async Task<CRUD> DeleteAsync(ICare care)
        {
            if (care == null)
            {
                return CRUD.NotFound;
            }

            this.response = await this.companyService.DeleteAsync(care?.CompanyId);
            var result = this.database.HealthCares.Remove((HealthCare)care);
            await this.database.SaveChangesAsync();

            return CRUD.Deleted;
        }
    }
}
