using Ngnet.ApiModels;
using System.Threading.Tasks;

namespace Ngnet.Services.Contracts
{
    public interface IVehicleCareService
    {
        public Task SaveAsync(VehicleCareRequestModel model);
    }
}
