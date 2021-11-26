using Ngnet.ApiModels.CareModels;
using Ngnet.Common;
using System.Threading.Tasks;

namespace Ngnet.Services.Cares.Interfaces
{
    public interface ICareService
    {
        public Task<CRUD> SaveAsync(CareRequestModel apiModel);

        public T GetById<T>(string id);

        public T[] GetByUserId<T>(string userId);

        public Task<CRUD> DeleteAsync(string vehicleCareId, bool hardDelete);

        public T GetDropdown<T>(string path);
    }
}
