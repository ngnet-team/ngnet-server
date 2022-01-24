using Ngnet.ApiModels;
using Ngnet.ApiModels.CareModels;
using Ngnet.Common;
using System.Threading.Tasks;

namespace Ngnet.Services.Cares.Interfaces
{
    public interface ICareBaseService
    {
        public T[] GetReminders<T>(TimeModel model);

        public Task<CRUD> RemindToggle(CareRequestModel apiModel);
    }
}
