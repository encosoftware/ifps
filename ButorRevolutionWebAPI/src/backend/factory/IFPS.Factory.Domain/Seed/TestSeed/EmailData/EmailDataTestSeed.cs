using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class EmailDataTestSeed : IEntitySeed<EmailData>
    {
        public EmailData[] Entities => new[]
        {
            new EmailData(EmailTypeEnum.ConfirmEmail) { Id = 1 },
            new EmailData(EmailTypeEnum.ResetPassword) { Id = 2 },
            new EmailData(EmailTypeEnum.ConfirmNewUser) { Id = 3 },
        };
    }
}
