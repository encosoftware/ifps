using ENCO.DDD.Service;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Exceptions;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Services
{
    public class CustomerEmailService : ApplicationService, ICustomerEmailService
    {
        private readonly IEmailService emailService;
        private readonly EmailSettings emailSettings;
        private readonly string htmlContainerFolder;

        public CustomerEmailService(
            IEmailService emailService,
            IHostingEnvironment environment,
            IOptions<EmailSettings> options,
            IApplicationServiceDependencyAggregate aggregate) : base(aggregate)
        {
            this.emailService = emailService;
            this.emailSettings = options.Value;
            htmlContainerFolder = System.IO.Path.Combine(environment.ContentRootPath, @"AppData/Email");
        }

        public async Task SendEmailToCustomerAsync(User user, OrderState orderState, int emailDataId, string workingNumber)
        {
            string documentUploadENG = "Your file uploaded successfully!";
            string documentUploadHUN = "azonosítójú rendeléshez a dokumentum feltöltése sikeres volt!";

            string waitingForOrderENG = " is waiting for offer!";
            string waitingForOrderHUN = " azonosítójú rendelése ajánlattételre vár!";

            string contents = System.IO.File.ReadAllText(System.IO.Path.Combine(htmlContainerFolder, "orderStateUpdate.html"));
            var builder = new BodyBuilder();

            CreateEmailContent(user, orderState, workingNumber, documentUploadENG, documentUploadHUN, waitingForOrderENG, waitingForOrderHUN, contents, builder);

            await emailService.SendEmailAsync(user,
            subject: user.Language == ENCO.DDD.Domain.Model.Enums.LanguageTypeEnum.HU ?
            orderState.Translations.Where(ent => ent.Language == ENCO.DDD.Domain.Model.Enums.LanguageTypeEnum.HU).First().Name :
            orderState.Translations.Where(ent => ent.Language == ENCO.DDD.Domain.Model.Enums.LanguageTypeEnum.EN).First().Name,
            builder.ToMessageBody(),
            emailDataId);
        }

        private void CreateEmailContent(User user, OrderState orderState, string workingNumber, string documentUploadENG, string documentUploadHUN, string waitingForOrrderENG, string waitingForOrrderHUN, string contents, BodyBuilder builder)
        {
            if (orderState.State == OrderStateEnum.WaitingForOffer)
            {
                if (user.Language == ENCO.DDD.Domain.Model.Enums.LanguageTypeEnum.HU)
                    builder.HtmlBody = string.Format(contents, emailSettings.GreetingHUN, user.CurrentVersion.Name, workingNumber, System.Text.Encoding.UTF8.GetString(System.Text.Encoding.UTF8.GetBytes(waitingForOrrderHUN)));

                else builder.HtmlBody = string.Format(contents, emailSettings.GreetingENG, user.CurrentVersion.Name, workingNumber, System.Text.Encoding.UTF8.GetString(System.Text.Encoding.UTF8.GetBytes(waitingForOrrderENG)));
            }
            else
            {
                if (user.Language == ENCO.DDD.Domain.Model.Enums.LanguageTypeEnum.HU)
                    builder.HtmlBody = string.Format(contents, emailSettings.GreetingHUN, user.CurrentVersion.Name, workingNumber, System.Text.Encoding.UTF8.GetString(System.Text.Encoding.UTF8.GetBytes(documentUploadHUN)));

                else builder.HtmlBody = string.Format(contents, emailSettings.GreetingENG, user.CurrentVersion.Name, workingNumber, System.Text.Encoding.UTF8.GetString(System.Text.Encoding.UTF8.GetBytes(documentUploadENG)));
            }
        }
    }
}
