using Ngnet.Common.Json.Service;
using Ngnet.Database;

namespace Ngnet.Services
{
    public abstract class BaseService
    {
        protected readonly NgnetDbContext database;
        protected readonly JsonService jsonService;

        protected BaseService(NgnetDbContext database, JsonService jsonService)
        {
            this.database = database;
            this.jsonService = jsonService;
        }
    }
}
