using Ngnet.ApiModels.AuthModels;
using Ngnet.Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ngnet.Services.Auth
{
    public interface IAuthService
    {
        public string CreateJwtToken(string userId, string username, string secret);

        public Task<int> Update<T>(T model);

        public Task<int> AddExperience(UserExperience exp);

        public ICollection<UserExperienceModel> GetExperiences(string UserId);
    }
}
