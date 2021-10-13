using Ngnet.ApiModels.CompanyModels;
using Ngnet.Database.Models;
using System.Threading.Tasks;

namespace Ngnet.Services.Companies
{
    public interface ICompanyService
    {
        public T GetNames<T>();

        public Task<int> SaveAsync(CompanyRequestModel model);
    }
}
