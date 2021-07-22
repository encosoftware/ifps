using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class RoleTranslationTestSeed : IEntitySeed<RoleTranslation>
    {
        public RoleTranslation[] Entities => new[]
        {
            new RoleTranslation(coreId: 10000, name: "Admin", LanguageTypeEnum.EN) { Id = 10000 },
            new RoleTranslation(coreId: 10000, name: "Rendszergazda", LanguageTypeEnum.HU) { Id = 10001 },
            new RoleTranslation(coreId: 10000, name: "Admin Expert", LanguageTypeEnum.EN) { Id = 10012 },
            new RoleTranslation(coreId: 10000, name: "Legfőbb Rendszergazda", LanguageTypeEnum.HU) { Id = 10013 },
            new RoleTranslation(coreId: 10002, name: "Production", LanguageTypeEnum.EN) { Id = 10002 },
            new RoleTranslation(coreId: 10002, name: "Gyártás", LanguageTypeEnum.HU) { Id = 10003 },
            new RoleTranslation(coreId: 10004, name: "Financial", LanguageTypeEnum.EN) { Id = 10004 },
            new RoleTranslation(coreId: 10004, name: "Pénzügy", LanguageTypeEnum.HU) { Id = 10005 },
            new RoleTranslation(coreId: 10006, name: "Supply", LanguageTypeEnum.EN) { Id = 10006 },
            new RoleTranslation(coreId: 10006, name: "Beszerzés", LanguageTypeEnum.HU) { Id = 10007 },
            new RoleTranslation(coreId: 10008, name: "Warehouse", LanguageTypeEnum.EN) { Id = 10008 },
            new RoleTranslation(coreId: 10008, name: "Raktár", LanguageTypeEnum.HU) { Id = 10009 },

            new RoleTranslation(coreId: 10005, name: "Financial Expert", LanguageTypeEnum.EN) { Id = 10010 },
            new RoleTranslation(coreId: 10005, name: "Pénzügyi főmufti", LanguageTypeEnum.HU) { Id = 10011 }
        };
    }
}