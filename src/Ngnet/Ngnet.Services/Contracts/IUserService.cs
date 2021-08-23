namespace Ngnet.Services.Contracts
{
    public interface IUserService
    {
        public string CreateJwtToken(string userId, string username, string secret);
    }
}
