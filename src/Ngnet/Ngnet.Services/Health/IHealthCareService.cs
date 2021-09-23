using Ngnet.ApiModels.HealthModels;
using System.Threading.Tasks;

namespace Ngnet.Services.Health
{
    public interface IHealthCareService
    {
        public Task<int> SaveAsync(HealthCareRequestModel apiModel);

        public T GetById<T>(string id);

        public T[] GetByUserId<T>(string userId);

        public Task<int> DeleteAsync(string vehicleCareId, bool hardDelete);

        public T GetNames<T>();
    }
}
