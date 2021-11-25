﻿using Ngnet.ApiModels.CareModels;
using Ngnet.Database;
using Ngnet.Database.Models;
using Ngnet.Mapper;
using System.Linq;
using System.Threading.Tasks;
using System;
using Ngnet.Common;
using Ngnet.Common.Json.Service;
using Ngnet.Services.Companies;
using Ngnet.ApiModels;

namespace Ngnet.Services.Vehicle
{
    public class VehicleCareService : CareBaseService, IVehicleCareService
    {
        public VehicleCareService(NgnetDbContext database, JsonService jsonService, ICompanyService companyService)
            : base(database, jsonService, companyService)
        {
        }

        public async Task<int> DeleteAsync(string vehicleCareId, bool hardDelete = false)
        {
            var vehicleCare = this.database.VehicleCares.FirstOrDefault(x => x.Id == vehicleCareId);

            if (vehicleCare == null)
            {
                return 0;
            }

            if (hardDelete)
            {
                this.database.VehicleCares.Remove(vehicleCare);
            }
            else
            {
                vehicleCare.IsDeleted = true;
                vehicleCare.DeletedOn = DateTime.UtcNow;
            }

            return await this.database.SaveChangesAsync();
        }

        public T[] GetByUserId<T>(string userId)
        {
            return this.database.VehicleCares
                .Where(x => x.UserId == userId && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedOn)
                .To<T>().ToArray();
        }

        public T GetById<T>(string vehicleCareId)
        {
            return this.database.VehicleCares
                .Where(x => x.Id == vehicleCareId)
                .To<T>()
                .FirstOrDefault();
        }

        public T GetNames<T>()
        {
            return this.jsonService.Deserialiaze<T>(Paths.VehicleCareNames);
        }

        public async Task<CRUD> SaveAsync(CareRequestModel apiModel)
        {
            CRUD response = CRUD.None;

            VehicleCare vehicleCare = this.database.VehicleCares.FirstOrDefault(x => x.Id == apiModel.Id);

            //Create new entity
            if (vehicleCare == null)
            {
                response = CRUD.Created;

                vehicleCare = MappingFactory.Mapper.Map<VehicleCare>(apiModel);
                await this.database.VehicleCares.AddAsync(vehicleCare);
            }
            else
            {
                response = apiModel.IsDeleted ? CRUD.Deleted : CRUD.Updated;

                bool companyReceived = apiModel?.Company != null;

                if (companyReceived)
                {
                    apiModel.Company.Id = await this.companyService.SaveAsync(apiModel.Company);
                }

                //Modify existing entity
                vehicleCare = this.ModifyEntity<CareRequestModel>(apiModel, vehicleCare);
            }

            await this.database.SaveChangesAsync();

            return response;
        }

        private VehicleCare ModifyEntity<T>(T apiModel, VehicleCare vehicleCare)
        {
            var mappedModel = MappingFactory.Mapper.Map<VehicleCare>(apiModel);

            vehicleCare.Name = mappedModel.Name == null ? vehicleCare.Name : mappedModel.Name;
            vehicleCare.StartDate = mappedModel.StartDate == null ? vehicleCare.StartDate : mappedModel.StartDate;
            vehicleCare.EndDate = mappedModel.EndDate == null ? vehicleCare.EndDate : mappedModel.EndDate;
            vehicleCare.PaidEndDate = mappedModel.PaidEndDate == null ? vehicleCare.PaidEndDate : mappedModel.PaidEndDate;
            vehicleCare.Reminder = mappedModel.Reminder == null ? vehicleCare.Reminder : mappedModel.Reminder;
            vehicleCare.Price = mappedModel.Price == null ? vehicleCare.Price : mappedModel.Price;
            vehicleCare.CompanyId = mappedModel?.Company?.Id == null ? vehicleCare.CompanyId : mappedModel.Company.Id;
            vehicleCare.Notes = mappedModel.Notes == null ? vehicleCare.Notes : mappedModel.Notes;
            vehicleCare.ModifiedOn = DateTime.UtcNow;
            vehicleCare.IsDeleted = mappedModel.IsDeleted == true ? mappedModel.IsDeleted : vehicleCare.IsDeleted;
            vehicleCare.DeletedOn = mappedModel.IsDeleted == true ? DateTime.UtcNow : vehicleCare.DeletedOn;

            return vehicleCare;
        }
    }
}
