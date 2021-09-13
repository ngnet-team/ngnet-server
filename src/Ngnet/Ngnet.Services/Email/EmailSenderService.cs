using Ngnet.Common;
using Ngnet.Services.Email;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace Ngnet.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly SendGridClient sender;

        public EmailSenderService(string key)
        {
            this.sender = new SendGridClient(key);
        }

        public async Task<Response> SendEmailAsync(EmailSendetModel model)
        {
            if (string.IsNullOrWhiteSpace(model.FromAddress) && string.IsNullOrWhiteSpace(model.ToAddress))
            {
                throw new ArgumentException(ValidationMessages.EmptryEmailSenderAddresses);
            }

            var fromAddress = new EmailAddress(model.FromAddress, model.FromName);
            var toAddress = new EmailAddress(model.ToAddress);

            var mail = MailHelper.CreateSingleEmail(fromAddress, toAddress, model.Subject, null, model.Content);

            Response response = null;

            try
            {
                response = await this.sender.SendEmailAsync(mail);
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }

            return response;
        }
    }
}
