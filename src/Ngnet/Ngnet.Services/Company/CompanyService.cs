using Ngnet.Common;
using Ngnet.Common.Json.Service;
using Ngnet.Data;

namespace Ngnet.Services.Company
{
    public class CompanyService : ICompanyService
    {
        private readonly NgnetDbContext database;
        private readonly JsonService jsonService;

        public CompanyService(NgnetDbContext database, JsonService jsonService)
        {
            this.database = database;
            this.jsonService = jsonService;
        }

        public T GetNames<T>()
        {
            var result = this.jsonService.Deserialiaze<T>(Paths.CompanyNames);
            return result;
        }
    }
}
