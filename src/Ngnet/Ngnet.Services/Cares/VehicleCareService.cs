using Ngnet.ApiModels.CareModels;
using Ngnet.Database;
using Ngnet.Database.Models;
using Ngnet.Mapper;
using System.Linq;
using System.Threading.Tasks;
using System;
using Ngnet.Common;
using Ngnet.Common.Json.Service;
using Ngnet.Services.Companies;
using Ngnet.Services.Cares.Interfaces;

namespace Ngnet.Services.Cares
{
    public class VehicleCareService : CareBaseService, IVehicleCareService
    {
        public VehicleCareService(NgnetDbContext database, JsonService jsonService, ICompanyService companyService)
            : base(database, jsonService, companyService)
        {
        }

        public async Task<CRUD> DeleteAsync(string vehicleCareId, bool hardDelete = false)
        {
            this.response = CRUD.None;

            var vehicleCare = this.database.VehicleCares.FirstOrDefault(x => x.Id == vehicleCareId);

            if (vehicleCare == null)
            {
                this.response = CRUD.NotFound;
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

            this.response = CRUD.Deleted;
            var result = await this.database.SaveChangesAsync();
            if (result == 0)
            {
                this.response = CRUD.None;
            }

            return this.response;
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

        public async Task<CRUD> SaveAsync(CareRequestModel apiModel)
        {
            this.response = CRUD.None;

            VehicleCare vehicleCare = this.database.VehicleCares.FirstOrDefault(x => x.Id == apiModel.Id);

            //Create new entity
            if (vehicleCare == null)
            {
                this.response = CRUD.Created;

                vehicleCare = MappingFactory.Mapper.Map<VehicleCare>(apiModel);
                await this.database.VehicleCares.AddAsync(vehicleCare);
            }
            else
            {
                this.response = apiModel.IsDeleted ? CRUD.Deleted : CRUD.Updated;

                bool companyReceived = apiModel?.Company != null;

                if (companyReceived)
                {
                    apiModel.Company.Id = await this.companyService.SaveAsync(apiModel.Company);
                }

                //Modify existing entity
                vehicleCare = (VehicleCare)this.ModifyEntity<CareRequestModel>(apiModel, vehicleCare);
            }

            this.result = await this.database.SaveChangesAsync();
            if (this.result == 0)
            {
                this.response = CRUD.None;
            }

            return this.response;
        }
    }
}
