using IFPS.Sales.Domain.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting.Internal;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;
        private readonly IHostingEnvironment _env;

        public EmailService(
            IOptions<EmailSettings> emailSettings,
            IHostingEnvironment env)
        {
            this.emailSettings = emailSettings.Value;
            _env = env;
        }

        public EmailService()
        {
            emailSettings = new EmailSettings();
            _env = new HostingEnvironment();
        }

        public async Task SendEmailAsync(User user, string subject, MimeEntity mimeEntity, int emailDataId)
        {
            bool isSuccess = false;
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(emailSettings.SenderName, emailSettings.Sender));
            mimeMessage.To.Add(new MailboxAddress(user.Email));
            mimeMessage.Subject = subject;
            mimeMessage.Body = mimeEntity;

            try
            {
                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    // The third parameter is useSSL (true if the client should make an SSL-wrapped
                    // connection to the server; otherwise, false).
                    await client.ConnectAsync(emailSettings.SmtpServer, emailSettings.SmtpPort, true);

                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(emailSettings.Sender, emailSettings.Password);

                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
                }
                isSuccess = true;
            }
            catch (SmtpCommandException ex)
            {
                isSuccess = false;
                throw new SmtpCommandException(ex.ErrorCode, ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                isSuccess = false;
                throw new Exception(ex.Message);
            }

            user.AddEmail(new Email(user.Id, emailDataId: emailDataId, timeOfSent: Clock.Now, isSuccess: isSuccess));
        }
    }
}
