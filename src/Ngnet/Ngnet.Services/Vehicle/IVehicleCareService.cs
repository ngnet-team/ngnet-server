using Ngnet.ApiModels.VehicleModels;
using System.Threading.Tasks;

namespace Ngnet.Services.Vehicle
{
    public interface IVehicleCareService
    {
        public Task<int> SaveAsync(VehicleCareRequestModel apiModel);

        public T GetByVehicleCareId<T>(string vehicleCareId);

        public T[] GetByUserId<T>(string userId);

        public Task<int> DeleteAsync(string vehicleCareId, bool hardDelete);
    }
}
