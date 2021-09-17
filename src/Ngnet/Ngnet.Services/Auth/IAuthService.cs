namespace Ngnet.Services.Auth
{
    public interface IAuthService
    {
        public string CreateJwtToken(string userId, string username, string secret);
    }
}
