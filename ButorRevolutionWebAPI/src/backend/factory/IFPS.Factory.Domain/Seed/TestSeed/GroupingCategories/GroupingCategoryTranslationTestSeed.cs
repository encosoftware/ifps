using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class GroupingCategoryTranslationTestSeed : IEntitySeed<GroupingCategoryTranslation>
    {
        public GroupingCategoryTranslation[] Entities => new[]
           {
            new GroupingCategoryTranslation(10000, "Material types", LanguageTypeEnum.EN) {Id = 10000},
            new GroupingCategoryTranslation(10000, "Alapanyag típusok", LanguageTypeEnum.HU) {Id = 10001},
            new GroupingCategoryTranslation(10001, "Furniture types", LanguageTypeEnum.EN) {Id = 10002},
            new GroupingCategoryTranslation(10001, "Bútorelem típusok", LanguageTypeEnum.HU) {Id = 10003},
            new GroupingCategoryTranslation(10002, "Divisions", LanguageTypeEnum.EN) {Id = 10004},
            new GroupingCategoryTranslation(10002, "Jogosultsági körök", LanguageTypeEnum.HU) {Id = 10005},
            new GroupingCategoryTranslation(10003, "Roles", LanguageTypeEnum.EN) {Id = 10006},
            new GroupingCategoryTranslation(10003, "Szerepkörök", LanguageTypeEnum.HU) {Id = 10007},
            new GroupingCategoryTranslation(10004, "Admin roles", LanguageTypeEnum.EN) {Id = 10008},
            new GroupingCategoryTranslation(10004, "Admin szerepkörök", LanguageTypeEnum.HU) {Id = 10009},

            new GroupingCategoryTranslation(10005, "Customer roles", LanguageTypeEnum.EN) {Id = 10010},
            new GroupingCategoryTranslation(10005, "Ügyfél szerepkörök", LanguageTypeEnum.HU) {Id = 10011},

            new GroupingCategoryTranslation(10006, "Sales roles", LanguageTypeEnum.EN) {Id = 10012},
            new GroupingCategoryTranslation(10006, "Értékesítői szerepkörök", LanguageTypeEnum.HU) {Id = 10013},

            new GroupingCategoryTranslation(10007, "Supply roles", LanguageTypeEnum.EN) {Id = 10014},
            new GroupingCategoryTranslation(10007, "Beszerzési szerepkörök", LanguageTypeEnum.HU) {Id = 10015},

            new GroupingCategoryTranslation(10008, "Menus", LanguageTypeEnum.EN) {Id = 10016},
            new GroupingCategoryTranslation(10008, "Menük", LanguageTypeEnum.HU) {Id = 10017},

            new GroupingCategoryTranslation(10009, "Admin", LanguageTypeEnum.EN) {Id = 10018},
            new GroupingCategoryTranslation(10009, "Admin", LanguageTypeEnum.HU) {Id = 10019},

            new GroupingCategoryTranslation(10010, "Sales", LanguageTypeEnum.EN) {Id = 10020},
            new GroupingCategoryTranslation(10010, "Értékesítés", LanguageTypeEnum.HU) {Id = 10021},

            new GroupingCategoryTranslation(10011, "Finances", LanguageTypeEnum.EN) {Id = 10022},
            new GroupingCategoryTranslation(10011, "Pénzügyek", LanguageTypeEnum.HU) {Id = 10023},

            new GroupingCategoryTranslation(10012, "Customers", LanguageTypeEnum.EN) {Id = 10024},
            new GroupingCategoryTranslation(10012, "Vevők", LanguageTypeEnum.HU) {Id = 10025}
        };
    }
}