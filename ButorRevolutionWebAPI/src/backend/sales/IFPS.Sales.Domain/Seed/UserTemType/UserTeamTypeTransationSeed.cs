using ENCO.DDD.Domain.Model.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed
{
    public class UserTeamTypeTransationSeed : IEntitySeed<UserTeamTypeTranslation>
    {
        public UserTeamTypeTranslation[] Entities => new[]
        {
            new UserTeamTypeTranslation("Shipping group", 1, LanguageTypeEnum.EN) { Id = 1 },
            new UserTeamTypeTranslation("Kiszállítócsapat", 1, LanguageTypeEnum.HU) { Id = 2 },
            new UserTeamTypeTranslation("Installation group", 2, LanguageTypeEnum.EN) { Id = 3 },
            new UserTeamTypeTranslation("Beszerelőcsapat", 2, LanguageTypeEnum.HU) { Id = 4 },
            new UserTeamTypeTranslation("Survey group", 3, LanguageTypeEnum.EN) { Id = 5 },
            new UserTeamTypeTranslation("Helyszíni felmérőcsapat", 3, LanguageTypeEnum.HU) { Id = 6 }
        };
    }
}
