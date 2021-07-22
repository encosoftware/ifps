using ENCO.DDD.Service;
using Hangfire;
using IFPS.Factory.Domain.FileHandling;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using IFPS.Factory.Domain.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace IFPS.Factory.Application.BackgroundJobs
{
    public class EmailSenderJob : ApplicationService, IEmailSenderJob
    {
        private readonly IEmailService _emailService;
        private readonly IEmailRepository emailRepository;
        private readonly IEmailDataRepository emailDataRepository;
        private readonly EmailSettings emailSettings;
        private readonly UserManager<User> userManager;
        private readonly LocalFileStorageConfiguration localFiles;
        private readonly string htmlContainerFolder;

        private const int NUMBER_OF_ATTEMPTS = 5;

        public EmailSenderJob(IApplicationServiceDependencyAggregate aggregate) : base(aggregate) { }
        public EmailSenderJob(
            IApplicationServiceDependencyAggregate aggregate
            , IHostingEnvironment environment
            , IEmailService emailService
            , IOptions<EmailSettings> emailSettings
            , IEmailRepository emailRepository
            , UserManager<User> userManager
            , IOptions<LocalFileStorageConfiguration> localFiles
            , IEmailDataRepository emailDataRepository) : base(aggregate)
        {
            _emailService = emailService;
            this.emailRepository = emailRepository;
            this.userManager = userManager;
            this.emailDataRepository = emailDataRepository;
            this.localFiles = localFiles.Value;
            htmlContainerFolder = Path.Combine(environment.ContentRootPath, "AppData");
            this.emailSettings = emailSettings.Value;
        }

        [AutomaticRetry(Attempts = NUMBER_OF_ATTEMPTS)]
        public async Task SendAllEmails()
        {
            string subject = string.Empty;
            BodyBuilder builder = new BodyBuilder();
            MimeMessage mimeMessage = new MimeMessage();
            TimeSpan elapsedTime;
            int elapsedHours;

            List<Email> emails = await emailRepository.GetAllListIncludingAsync(email => (!email.IsSuccess && email.SendCount < emailSettings.MaximumTrials),
                   email => email.User.CurrentVersion, email => email.EmailData, email => email.User);

            foreach (Email email in emails)
            {
                elapsedTime = email.TimeOfSent.Subtract(Clock.Now);
                elapsedHours = Math.Abs(elapsedTime.Hours);
                if (email.SendCount < emailSettings.MaximumTrials && elapsedHours > emailSettings.HangfireTimeInterval)
                {
                    switch (email.EmailData.Type)
                    {
                        case Domain.Enums.EmailTypeEnum.None:
                            break;
                        case Domain.Enums.EmailTypeEnum.ConfirmEmail:
                            await GenerateConfirmEmailAsync(builder, email);
                            break;
                        case Domain.Enums.EmailTypeEnum.ResetPassword:
                            await GenerateResetPasswordEmailAsync(builder, email);
                            break;
                        case Domain.Enums.EmailTypeEnum.Other:
                            break;
                        default:
                            builder.HtmlBody = string.Empty;
                            subject = string.Empty;
                            break;
                    }

                    try
                    {
                        await _emailService.SendEmailAsync(
                            email.User,
                            subject,
                            builder.ToMessageBody(),
                            email.EmailDataId);
                        await unitOfWork.SaveChangesAsync();
                    }
                    catch (SmtpCommandException)
                    {
                        email.IsSuccess = false;
                        email.TimeOfSent = Clock.Now;
                        email.SendCount++;
                        await emailRepository.UpdateAsync(email);
                        await unitOfWork.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        email.IsSuccess = false;
                        email.TimeOfSent = Clock.Now;
                        email.SendCount++;
                        await emailRepository.UpdateAsync(email);
                        await unitOfWork.SaveChangesAsync();
                    }
                }
            }
        }

        private async Task GenerateResetPasswordEmailAsync(BodyBuilder builder, Email email)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(email.User);
            token = HttpUtility.UrlEncode(token);
            string url = localFiles.BaseURL + $"/login?dialog=true&userId={email.User.Id}&passwordresettoken={token}";
            var emailData = await emailDataRepository.SingleAsync(ent => ent.Type == Domain.Enums.EmailTypeEnum.ResetPassword);
            string contents = System.IO.File.ReadAllText(Path.Combine(htmlContainerFolder, emailData.CurrentTranslation.FileName));
            var pathImage = Path.Combine(htmlContainerFolder, emailData.CurrentTranslation.ImageFileName);
            var image = builder.LinkedResources.Add(pathImage);
            image.ContentId = MimeUtils.GenerateMessageId();
            builder.HtmlBody = string.Format(contents, image.ContentId, email.User.CurrentVersion.Name, url, url);
        }

        private async Task GenerateConfirmEmailAsync(BodyBuilder builder, Email email)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(email.User);
            token = HttpUtility.UrlEncode(token);
            string url = localFiles.BaseURL + $"/login?userId={email.User.Id}&token={token}";
            var emailData = await emailDataRepository.SingleAsync(ent => ent.Type == Domain.Enums.EmailTypeEnum.ConfirmEmail);
            string contents = System.IO.File.ReadAllText(Path.Combine(htmlContainerFolder, emailData.CurrentTranslation.FileName));

            var pathImage = Path.Combine(htmlContainerFolder, emailData.CurrentTranslation.ImageFileName);
            var image = builder.LinkedResources.Add(pathImage);

            image.ContentId = MimeUtils.GenerateMessageId();
            builder.HtmlBody = string.Format(contents, email.User.CurrentVersion.Name, url, url);
        }
    }
}
