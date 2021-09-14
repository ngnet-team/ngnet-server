using AutoMapper;
using Ngnet.ApiModels;
using Ngnet.Data;
using Ngnet.Data.DbModels;
using Ngnet.Services.Contracts;
using System.Threading.Tasks;

namespace Ngnet.Services
{
    public class VehicleCareService : IVehicleCareService
    {
        private readonly NgnetDbContext database;
        private readonly IMapper mapper;

        public VehicleCareService(NgnetDbContext database, IMapper mapper)
        {
            this.database = database;
            this.mapper = mapper;
        }

        public async Task SaveAsync(VehicleCareRequestModel model)
        {
            //var vehicle = mapper.Map<VehicleCare>(model);

            await this.database.VehicleCares.AddAsync(new VehicleCare() { Name = model.Name });
            await this.database.SaveChangesAsync();
        }
    }
}
