using Ngnet.ApiModels.CareModels;
using Ngnet.Database;
using Ngnet.Database.Models;
using Ngnet.Mapper;
using System.Linq;
using System.Threading.Tasks;
using Ngnet.Common;
using Ngnet.Services.Companies;
using Ngnet.Services.Cares.Interfaces;
using Ngnet.Database.Models.Interfaces;
using Ngnet.Common.Json.Service;

namespace Ngnet.Services.Cares
{
    public class VehicleCareService : CareBaseService, IVehicleCareService
    {
        public VehicleCareService(ICompanyService companyService, NgnetDbContext database, JsonService jsonService)
            : base(companyService, database, jsonService)
        {
        }

        public async Task<CRUD> DeleteAsync(ICare care)
        {
            if (care == null)
            {
                return CRUD.NotFound;
            }

            this.response = await this.companyService.DeleteAsync(care?.CompanyId);
            var result = this.database.VehicleCares.Remove((VehicleCare)care);
            await this.database.SaveChangesAsync();

            return CRUD.Deleted;
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
            //Create a new entity
            if (vehicleCare == null)
            {
                this.response = CRUD.Created;

                vehicleCare = MappingFactory.Mapper.Map<VehicleCare>(apiModel);
                await this.database.VehicleCares.AddAsync(vehicleCare);
            }
            //Modify an existing one
            else
            {
                //Permanently delete
                if (apiModel.IsDeleted)
                {
                    CRUD result = await this.DeleteAsync(vehicleCare);
                    if (result == CRUD.Deleted)
                    {
                        return CRUD.Deleted;
                    }
                }

                this.response = CRUD.Updated;

                //Modify company
                int companyId = await this.companyService.SaveAsync(apiModel.Company);
                if (companyId != 0)
                {
                    apiModel.Company.Id = companyId;
                }

                //Modify existing care entity
                vehicleCare = (VehicleCare)this.ModifyEntity<CareRequestModel>(apiModel, vehicleCare);
            }

            await this.database.SaveChangesAsync();
            return this.response;
        }
    }
}
