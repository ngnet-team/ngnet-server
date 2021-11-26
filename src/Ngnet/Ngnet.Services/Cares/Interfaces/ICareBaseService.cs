using Ngnet.ApiModels;

namespace Ngnet.Services.Cares.Interfaces
{
    public interface ICareBaseService
    {
        public T[] GetReminders<T>(TimeModel model, string userId);
    }
}
