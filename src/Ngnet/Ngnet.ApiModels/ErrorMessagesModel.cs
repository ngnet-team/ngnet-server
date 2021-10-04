using Ngnet.Common.Json.Models;

namespace Ngnet.ApiModels
{
    public class ErrorMessagesModel
    {
        public SimpleDropDownModel NotEqualPasswords { get; set; }

        public SimpleDropDownModel InvalidUsername { get; set; }

        public SimpleDropDownModel InvalidPasswords { get; set; }

        public SimpleDropDownModel UserNotFound { get; set; }

        public SimpleDropDownModel UsersNotFound { get; set; }

        public SimpleDropDownModel VehicleCareNotFound { get; set; }

        public SimpleDropDownModel VehicleCaresNotFound { get; set; }

        public SimpleDropDownModel NoPermissions { get; set; }

        public SimpleDropDownModel VehicleCareNamesNotFound { get; set; }

        public SimpleDropDownModel CompanyNamesNotFound { get; set; }

        public SimpleDropDownModel HealthCareNamesNotFound { get; set; }

        public SimpleDropDownModel HealthCareNotFound { get; set; }

        public SimpleDropDownModel HealthCaresNotFound { get; set; }
    }
}
