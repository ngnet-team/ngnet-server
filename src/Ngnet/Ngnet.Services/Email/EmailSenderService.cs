using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration configuration;

        public EmailSenderService(IConfiguration configuration)
        {
            this.configuration = configuration;

            string key = this.configuration.GetSection("EmailSender:Key").ToString();
            this.sender = new SendGridClient(key);
        }

        public async Task<Response> SendEmailAsync(string type, EmailSenderModel model)
        {
            if (type == "Email confirmation") // needs to be put in types model
            {
                model = EmailConfirmation(model);
            }

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

        private EmailSenderModel EmailConfirmation(EmailSenderModel model)
        {
            var admin = this.configuration.GetSection("Admin");

            return new EmailSenderModel()
            {
                FromAddress = admin.GetSection("Email").Value,
                FromName = admin.GetSection("FirstName").Value + " " + admin.GetSection("LastName").Value,
                ToAddress = model.ToAddress,
                Subject = "Email confirmation message",
                Content = "Please confirm your email by clicking the button below"
            };
        }
    }
}
