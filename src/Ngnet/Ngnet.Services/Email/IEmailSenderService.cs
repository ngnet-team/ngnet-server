using SendGrid;
using System;
using System.Threading.Tasks;

namespace Ngnet.Services.Email
{
    public interface IEmailSenderService
    {
        public Task<Response> SendEmailAsync(EmailSendetModel model);
    }
}
