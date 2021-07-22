using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class EmailDataTranslationTestSeed : IEntitySeed<EmailDataTranslation>
    {
        public EmailDataTranslation[] Entities => new[]
        {
            new EmailDataTranslation(coreId: 1, "Email/confirmEmail.html", "Email/id-card.png", LanguageTypeEnum.HU){ Id = 1 },
            new EmailDataTranslation(coreId: 2, "Email/resetPassword.html","Email/id-card.png", LanguageTypeEnum.HU){ Id = 2 },
            new EmailDataTranslation(coreId: 3, "Email/createNewUserByAdmin.html","Email/id-card.png", LanguageTypeEnum.HU){ Id = 3 }
        };
    }
}
