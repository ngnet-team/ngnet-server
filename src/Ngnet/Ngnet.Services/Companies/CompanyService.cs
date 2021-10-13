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
    public class CompanyService : ICompanyService
    {
        private readonly NgnetDbContext database;
        private readonly JsonService jsonService;

        public CompanyService(NgnetDbContext database, JsonService jsonService)
        {
            this.database = database;
            this.jsonService = jsonService;
        }

        public T GetNames<T>()
        {
            var result = this.jsonService.Deserialiaze<T>(Paths.CompanyNames);
            return result;
        }
        public async Task<int> SaveAsync(CompanyRequestModel apiModel)
        {
            Company company;
            if (apiModel?.Id == null)
            {
                company = MappingFactory.Mapper.Map<Company>(apiModel);
                await this.database.Companies.AddAsync(company);
            }
            else
            {
                company = this.database.Companies.FirstOrDefault(x => x.Id == apiModel.Id);
                company = this.ModifyEntity<CompanyRequestModel>(apiModel, company);
            }

            await this.database.SaveChangesAsync();
            return company.Id;
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
