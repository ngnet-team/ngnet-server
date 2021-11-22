using System.ComponentModel.DataAnnotations;

namespace Ngnet.ApiModels.AuthModels
{
    public class ChangeRequestModel
    {
        public string UserId { get; set; }

        [Required]
        public string Old { get; set; }

        [Required]
        public string New { get; set; }

        [Required]
        public string RepeatNew { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
