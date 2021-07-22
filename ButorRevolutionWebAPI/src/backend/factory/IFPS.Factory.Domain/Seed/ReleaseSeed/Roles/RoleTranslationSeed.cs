using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class RoleTranslationSeed : IEntitySeed<RoleTranslation>
    {
        public RoleTranslation[] Entities => new[]
        {
            new RoleTranslation(coreId: 1, name: "Admin", LanguageTypeEnum.EN) { Id = 1 },
            new RoleTranslation(coreId: 1, name: "Rendszergazda", LanguageTypeEnum.HU) { Id = 2 },
            new RoleTranslation(coreId: 2, name: "Admin Expert", LanguageTypeEnum.EN) {Id = 11 },
            new RoleTranslation(coreId: 2, name: "Legfőbb Admin", LanguageTypeEnum.HU) {Id = 12 },
            new RoleTranslation(coreId: 3, name: "Production", LanguageTypeEnum.EN) { Id = 3 },
            new RoleTranslation(coreId: 3, name: "Gyártás", LanguageTypeEnum.HU) { Id = 4 },
            new RoleTranslation(coreId: 5, name: "Financial", LanguageTypeEnum.EN) { Id = 5 },
            new RoleTranslation(coreId: 5, name: "Pénzügy", LanguageTypeEnum.HU) { Id = 6 },
            new RoleTranslation(coreId: 7, name: "Supply", LanguageTypeEnum.EN) { Id = 7 },
            new RoleTranslation(coreId: 7, name: "Beszerzés", LanguageTypeEnum.HU) { Id = 8 },
            new RoleTranslation(coreId: 9, name: "Warehouse", LanguageTypeEnum.EN) { Id = 9 },
            new RoleTranslation(coreId: 9, name: "Raktár", LanguageTypeEnum.HU) { Id = 10 },
        };
    }
}
