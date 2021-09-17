using Ngnet.ApiModels;
using Ngnet.Data.DbModels;
using System.Threading.Tasks;

namespace Ngnet.Services.Contracts
{
    public interface IVehicleCareService
    {
        public Task<int> SaveAsync(VehicleCareRequestModel apiModel);

        public T GetByVehicleCareId<T>(string vehicleCareId);

        public T[] GetByUserId<T>(string userId);
    }
}
