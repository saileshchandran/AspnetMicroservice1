using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
        public EmailSettings emailSettings { get; set; }

        public ILogger<EmailService> Logger { get; }

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            this.emailSettings = emailSettings.Value ?? throw new ArgumentNullException(nameof(emailSettings));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> SendEmail(Email email)
        {
            var client = new SendGridClient(emailSettings.ApiKey);
            var from = new SendGrid.Helpers.Mail.EmailAddress { Email = emailSettings.FromAddress, Name = emailSettings.FromName }; 
            var to = new SendGrid.Helpers.Mail.EmailAddress { Email = email.To };
            var subject = email.Subject;
            var body = email.Body;
            var sendGridMessage = SendGrid.Helpers.Mail.MailHelper.CreateSingleEmail(from, to, subject, body, body);
            var response = await client.SendEmailAsync(sendGridMessage);

            Logger.LogInformation("Email sent.");

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted || response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                Logger.LogInformation("Email sending failed !!!");
                return false;
            }
        }
    }
}
