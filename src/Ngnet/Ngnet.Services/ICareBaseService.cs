using Ngnet.ApiModels;

namespace Ngnet.Services
{
    public interface ICareBaseService
    {
        public T[] GetReminders<T>(TimeModel model, string userId);
    }
}
