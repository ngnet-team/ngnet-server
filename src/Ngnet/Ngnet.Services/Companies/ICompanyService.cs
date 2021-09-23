using Ngnet.ApiModels.CompanyModels;
using Ngnet.Data.DbModels;
using System.Threading.Tasks;

namespace Ngnet.Services.Companies
{
    public interface ICompanyService
    {
        public T GetNames<T>();

        public Task<int> SaveAsync(CompanyRequestModel model);
    }
}
