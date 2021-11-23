using Ngnet.ApiModels.AuthModels;
using Ngnet.Common;
using Ngnet.Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ngnet.Services.Auth
{
    public interface IAuthService
    {
        public string CreateJwtToken(string userId, string username, string secret);

        public Task<CRUD> Update<T>(T model);

        public Task<CRUD> AddExperience(UserExperience exp);

        public ICollection<UserExperienceModel> GetExperiences(string UserId);
    }
}
