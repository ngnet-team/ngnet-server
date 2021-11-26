using SendGrid;
using System.Threading.Tasks;

namespace Ngnet.Services.Email
{
    public interface IEmailSenderService
    {
        public Task<Response> SendEmailAsync(EmailSenderModel model);

        public Task<Response> EmailConfirmation(EmailSenderModel model);

        public string GetTemplate(string fileName);
    }
}
