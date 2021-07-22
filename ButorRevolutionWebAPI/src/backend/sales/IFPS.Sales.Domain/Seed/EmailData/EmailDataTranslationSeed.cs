using ENCO.DDD.Domain.Model.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed.EmailDatas
{
    public class EmailDataTranslationSeed : IEntitySeed<EmailDataTranslation>
    {
        public EmailDataTranslation[] Entities => new[]
        {
            new EmailDataTranslation(coreId: 1, fileName: "Email/confirmEmail.html", imageFileName: "Email/id-card.png", LanguageTypeEnum.HU) { Id = 1 },
            new EmailDataTranslation(coreId: 2, fileName: "Email/resetPassword.html", imageFileName: "Email/id-card.png", LanguageTypeEnum.HU) { Id = 2 },
            new EmailDataTranslation(coreId: 3, fileName: "Email/confirmEmail.html", imageFileName: "Email/id-card.png", LanguageTypeEnum.HU) { Id = 3 },
            new EmailDataTranslation(coreId: 4, fileName: "Email/createNewUserByAdmin.html", imageFileName: "Email/id-card.png", LanguageTypeEnum.HU) { Id = 4 },
            new EmailDataTranslation(coreId: 5, fileName: "Email/orderStateUpdate.html", imageFileName: "Email/id-card.png", LanguageTypeEnum.HU) { Id = 5 },
            new EmailDataTranslation(coreId: 6, fileName: "Email/orderStateUpdate.html", imageFileName: "Email/id-card.png", LanguageTypeEnum.HU) { Id = 6 },
            new EmailDataTranslation(coreId: 7, fileName: "Email/orderStateUpdate.html", imageFileName: "Email/id-card.png", LanguageTypeEnum.HU) { Id = 7 },
            new EmailDataTranslation(coreId: 8, fileName: "Email/orderStateUpdate.html", imageFileName: "Email/id-card.png", LanguageTypeEnum.HU) { Id = 8 },
            new EmailDataTranslation(coreId: 9, fileName: "Email/orderStateUpdate.html", imageFileName: "Email/id-card.png", LanguageTypeEnum.HU) { Id = 9 },
            new EmailDataTranslation(coreId: 10, fileName: "Email/orderStateUpdate.html", imageFileName: "Email/id-card.png", LanguageTypeEnum.HU) { Id = 10 },
        };
    }
}
