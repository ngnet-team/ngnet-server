using Ngnet.ApiModels;
using Ngnet.Data;
using Ngnet.Data.DbModels;
using Ngnet.Mapper;
using Ngnet.Services.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Ngnet.Services
{
    public class VehicleCareService : IVehicleCareService
    {
        private readonly NgnetDbContext database;

        public VehicleCareService(NgnetDbContext database)
        {
            this.database = database;
        }

        public T[] GetByUserId<T>(string userId)
        {
            return this.database.VehicleCares
                .Where(x => x.UserId == userId)
                .To<T>()
                .ToArray();
        }

        public T GetByVehicleCareId<T>(string vehicleCareId)
        {
            return this.database.VehicleCares
                .Where(x => x.Id == vehicleCareId)
                .To<T>()
                .FirstOrDefault();
        }

        public async Task<int> SaveAsync(VehicleCareRequestModel apiModel)
        {
            VehicleCare vehicleCare = this.database.VehicleCares.FirstOrDefault(x => x.Id == apiModel.Id);

            if (vehicleCare != null)
            {
                vehicleCare = this.ModifyEntity<VehicleCareRequestModel>(apiModel, vehicleCare);
            }
            else
            {
                vehicleCare = MappingFactory.Mapper.Map<VehicleCare>(apiModel);
                await this.database.VehicleCares.AddAsync(vehicleCare);
            }

            return await this.database.SaveChangesAsync();
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
            vehicleCare.Notes = mappedModel.Notes == null ? vehicleCare.Notes : mappedModel.Notes;
            vehicleCare.ModifiedOn = System.DateTime.UtcNow;
            vehicleCare.IsDeleted = mappedModel.IsDeleted == true ? mappedModel.IsDeleted : vehicleCare.IsDeleted;
            vehicleCare.DeletedOn = mappedModel.IsDeleted == true ? System.DateTime.UtcNow : vehicleCare.DeletedOn;

            return vehicleCare;
        }
    }
}
