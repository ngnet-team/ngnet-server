using Ngnet.Common.Json.Models;

namespace Ngnet.ApiModels
{
    public class SuccessMessagesModel
    {
        public LanguagesModel UserRegistered { get; set; }

        public LanguagesModel UserUpdated { get; set; }

        public LanguagesModel UserLoggedIn { get; set; }

        public LanguagesModel HealthCareSaved { get; set; }

        public LanguagesModel HealthCareDeleted { get; set; }

        public LanguagesModel VehicleCareSaved { get; set; }

        public LanguagesModel VehicleCareDeleted { get; set; }
    }
}
