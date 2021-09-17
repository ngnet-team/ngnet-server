namespace Ngnet.ApiModels.AuthModels
{
    public class AuthErrorModel
    {
        public AuthErrorModel(string error)
        {
            this.Description = error;
        }

        public string Description { get; set; }
    }
}
