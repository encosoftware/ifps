using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed.EmailDatas
{
    public class EmailDataSeed : IEntitySeed<EmailData>
    {
        public EmailData[] Entities => new[]
        {
            new EmailData(EmailTypeEnum.ConfirmEmail) { Id = 1 },
            new EmailData(EmailTypeEnum.ResetPassword) { Id = 2 },
            new EmailData(EmailTypeEnum.ConfirmPurchase) { Id = 3 },
            new EmailData(EmailTypeEnum.ConfirmNewUser) { Id = 4 },
            new EmailData(EmailTypeEnum.DocumentUploadSuccess) { Id = 5 },
            new EmailData(EmailTypeEnum.OfferUploaded) { Id = 6 },
            new EmailData(EmailTypeEnum.ContractSignedState) { Id = 7 },
            new EmailData(EmailTypeEnum.WaitingForContract) { Id = 8 },
            new EmailData(EmailTypeEnum.WaitingForOffer) { Id = 9 },
            new EmailData(EmailTypeEnum.ContractUploaded) { Id = 10 },
        };
    }
}
