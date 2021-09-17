using Ngnet.Data.DbModels;
using Ngnet.Mapper;

namespace Ngnet.ApiModels.CompanyModels
{
    public class CompanyResponseModel : IMapFrom<Company>
    {
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string WebSite { get; set; }

        public string Address { get; set; }
    }
}
