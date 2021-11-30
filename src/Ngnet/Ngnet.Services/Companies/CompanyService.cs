using Ngnet.ApiModels.CompanyModels;
using Ngnet.Common;
using Ngnet.Common.Json.Service;
using Ngnet.Database;
using Ngnet.Database.Models;
using Ngnet.Mapper;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ngnet.Services.Companies
{
    public class CompanyService : BaseService, ICompanyService
    {
        private Company company;

        public CompanyService(NgnetDbContext database, JsonService jsonService)
            : base(database, jsonService)
        {
        }

        public T GetNames<T>()
        {
            var result = this.jsonService.Deserialiaze<T>(Paths.CompanyNames);
            return result;
        }

        public async Task<int> SaveAsync(CompanyRequestModel apiModel)
        {
            if (apiModel == null)
            {
                return 0;
            }

            this.company = this.database.Companies.FirstOrDefault(x => x.Id == apiModel.Id);
            if (this.company == null)
            {
                this.company = MappingFactory.Mapper.Map<Company>(apiModel);
                await this.database.Companies.AddAsync(this.company);
            }

            this.company = this.ModifyEntity<CompanyRequestModel>(apiModel, this.company);
            await this.database.SaveChangesAsync();

            return this.company.Id;
        }

        public async Task<CRUD> DeleteAsync(int? companyId)
        {
            this.company = this.database.Companies.FirstOrDefault(x => x.Id == companyId);
            if (this.company == null)
            {
                return CRUD.NotFound;
            }

            var result = this.database.Companies.Remove(this.company);
            await this.database.SaveChangesAsync();

            return CRUD.Deleted;
        }

        private Company ModifyEntity<T>(T apiModel, Company company)
        {
            var mappedModel = MappingFactory.Mapper.Map<Company>(apiModel);

            company.Name = mappedModel.Name == null ? company.Name : mappedModel.Name;
            company.PhoneNumber = mappedModel.PhoneNumber == null ? company.PhoneNumber : mappedModel.PhoneNumber;
            company.Email = mappedModel.Email == null ? company.Email : mappedModel.Email;
            company.WebSite = mappedModel.WebSite == null ? company.WebSite : mappedModel.WebSite;
            company.Address = mappedModel.Address == null ? company.Address : mappedModel.Address;
            company.ModifiedOn = DateTime.UtcNow;
            company.IsDeleted = mappedModel.IsDeleted == true ? mappedModel.IsDeleted : company.IsDeleted;
            company.DeletedOn = mappedModel.IsDeleted == true ? DateTime.UtcNow : company.DeletedOn;

            return company;
        }
    }
}
