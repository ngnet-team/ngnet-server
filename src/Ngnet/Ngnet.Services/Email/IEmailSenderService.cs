using SendGrid;
using System.Threading.Tasks;

namespace Ngnet.Services.Email
{
    public interface IEmailSenderService
    {
        public Task<Response> SendEmailAsync(string type, EmailSenderModel model);
    }
}
