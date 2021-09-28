using System.Threading.Tasks;

namespace Ngnet.Services.Auth
{
    public interface IAuthService
    {
        public string CreateJwtToken(string userId, string username, string secret);

        public Task<int> Update<T>(T model);
    }
}
