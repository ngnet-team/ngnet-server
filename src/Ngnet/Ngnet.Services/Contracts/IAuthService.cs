namespace Ngnet.Services.Contracts
{
    public interface IAuthService
    {
        public string CreateJwtToken(string userId, string username, string secret);
    }
}
