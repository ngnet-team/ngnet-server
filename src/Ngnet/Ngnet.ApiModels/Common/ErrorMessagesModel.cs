using Ngnet.Common.Json.Models;

namespace Ngnet.ApiModels.Common
{
    public class ErrorMessagesModel
    {
        public LanguagesModel VehicleCareNotFound { get; set; }

        public LanguagesModel VehicleCaresNotFound { get; set; }

        public LanguagesModel VehicleCareNamesNotFound { get; set; }

        public LanguagesModel CompanyNamesNotFound { get; set; }

        public LanguagesModel HealthCareNamesNotFound { get; set; }

        public LanguagesModel HealthCareNotFound { get; set; }

        public LanguagesModel HealthCaresNotFound { get; set; }
    }
}
