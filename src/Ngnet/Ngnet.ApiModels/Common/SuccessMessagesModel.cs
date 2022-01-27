using Ngnet.Common.Json.Models;

namespace Ngnet.ApiModels.Common
{
    public class SuccessMessagesModel
    {
        public LanguagesModel Created { get; set; }

        public LanguagesModel Updated { get; set; }

        public LanguagesModel Deleted { get; set; }
    }
}
