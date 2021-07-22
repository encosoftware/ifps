using ENCO.DDD.Domain.Model.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed.GroupingCategories
{
    public class GroupingCategoryTranslationSeed : IEntitySeed<GroupingCategoryTranslation>
    {

        public GroupingCategoryTranslation[] Entities => new[]
           {
            new GroupingCategoryTranslation(1, "Material types", LanguageTypeEnum.EN) {Id = 1},
            new GroupingCategoryTranslation(1, "Alapanyag típusok", LanguageTypeEnum.HU) {Id = 2},

            new GroupingCategoryTranslation(2, "Furniture types", LanguageTypeEnum.EN) {Id = 3},
            new GroupingCategoryTranslation(2, "Bútorelem típusok", LanguageTypeEnum.HU) {Id = 4},

            new GroupingCategoryTranslation(3, "Divisions", LanguageTypeEnum.EN) {Id = 5},
            new GroupingCategoryTranslation(3, "Jogosultságok típusai", LanguageTypeEnum.HU) {Id = 6},
            
            new GroupingCategoryTranslation(9, "Menus", LanguageTypeEnum.EN) {Id = 17},
            new GroupingCategoryTranslation(9, "Menük", LanguageTypeEnum.HU) {Id = 18},

            new GroupingCategoryTranslation(10, "Admin", LanguageTypeEnum.EN) {Id = 19},
            new GroupingCategoryTranslation(10, "Admin", LanguageTypeEnum.HU) {Id = 20},

            new GroupingCategoryTranslation(11, "Sales", LanguageTypeEnum.EN) {Id = 21},
            new GroupingCategoryTranslation(11, "Értékesítés", LanguageTypeEnum.HU) {Id = 22},

            new GroupingCategoryTranslation(12, "Finances", LanguageTypeEnum.EN) {Id = 23},
            new GroupingCategoryTranslation(12, "Pénzügyek", LanguageTypeEnum.HU) {Id = 24},

            new GroupingCategoryTranslation(13, "Customers", LanguageTypeEnum.EN) {Id = 25},
            new GroupingCategoryTranslation(13, "Vevők", LanguageTypeEnum.HU) {Id = 26},

            new GroupingCategoryTranslation(14, "Appointments", LanguageTypeEnum.EN) {Id = 27},
            new GroupingCategoryTranslation(14, "Időpontok", LanguageTypeEnum.HU) {Id = 28},

            new GroupingCategoryTranslation(15, "Meeting", LanguageTypeEnum.EN) {Id = 29},
            new GroupingCategoryTranslation(15, "Megbeszélés", LanguageTypeEnum.HU) {Id = 30},

            new GroupingCategoryTranslation(16, "Contracting", LanguageTypeEnum.EN) {Id = 31},
            new GroupingCategoryTranslation(16, "Szerződéskötés", LanguageTypeEnum.HU) {Id = 32},

            new GroupingCategoryTranslation(17, "Worktop board", LanguageTypeEnum.EN) {Id = 33},
            new GroupingCategoryTranslation(17, "Munkalap", LanguageTypeEnum.HU) {Id = 34},

            new GroupingCategoryTranslation(18, "Decor board", LanguageTypeEnum.EN) {Id = 35},
            new GroupingCategoryTranslation(18, "Bútorlap", LanguageTypeEnum.HU) {Id = 36},

            new GroupingCategoryTranslation(19, "Accessories", LanguageTypeEnum.EN) {Id = 37},
            new GroupingCategoryTranslation(19, "Vasalatok, kiegészítők", LanguageTypeEnum.HU) {Id = 38},

            new GroupingCategoryTranslation(20, "Appliances", LanguageTypeEnum.EN) {Id = 39},
            new GroupingCategoryTranslation(20, "Konyhagépek", LanguageTypeEnum.HU) {Id = 40},

            new GroupingCategoryTranslation(21, "Foil material", LanguageTypeEnum.EN) {Id = 41},
            new GroupingCategoryTranslation(21, "Élzáró fóliák", LanguageTypeEnum.HU) {Id = 42},

            new GroupingCategoryTranslation(22, "Kitchen", LanguageTypeEnum.EN) { Id = 43 },
            new GroupingCategoryTranslation(22, "Konyha", LanguageTypeEnum.HU) { Id = 44 },

            new GroupingCategoryTranslation(23, "Office", LanguageTypeEnum.EN) { Id = 45 },
            new GroupingCategoryTranslation(23, "Irodai bútorok", LanguageTypeEnum.HU) { Id = 46 },

            new GroupingCategoryTranslation(24, "Bathroom", LanguageTypeEnum.EN) { Id = 47 },
            new GroupingCategoryTranslation(24, "Fürdőszobai elemek", LanguageTypeEnum.HU) { Id = 48 },

            new GroupingCategoryTranslation(25, "Islands", LanguageTypeEnum.EN) { Id = 49 },
            new GroupingCategoryTranslation(25, "Konyhaszigetek", LanguageTypeEnum.HU) { Id = 50 },

            new GroupingCategoryTranslation(26, "Knobs & Handles", LanguageTypeEnum.EN) { Id = 51 },
            new GroupingCategoryTranslation(26, "Kilincsek, fogantyúk", LanguageTypeEnum.HU) { Id = 52 },

            new GroupingCategoryTranslation(27, "Ligthning", LanguageTypeEnum.EN) { Id = 53 },
            new GroupingCategoryTranslation(27, "Világítás", LanguageTypeEnum.HU) { Id = 54 },

            new GroupingCategoryTranslation(30, "Mosogatógép", LanguageTypeEnum.EN) { Id = 55 },
            new GroupingCategoryTranslation(30, "Dishwasher machine", LanguageTypeEnum.HU) { Id = 56 },

            new GroupingCategoryTranslation(31, "Mosogatógép 60 cm", LanguageTypeEnum.EN) { Id = 57 },
            new GroupingCategoryTranslation(31, "Dishwasher machine 60 cm", LanguageTypeEnum.HU) { Id = 58 },

            new GroupingCategoryTranslation(32, "Tall", LanguageTypeEnum.EN) { Id = 59 },
            new GroupingCategoryTranslation(32, "Magas", LanguageTypeEnum.HU) { Id = 60 },

            new GroupingCategoryTranslation(33, "Base", LanguageTypeEnum.EN) { Id = 61 },
            new GroupingCategoryTranslation(33, "Alsó", LanguageTypeEnum.HU) { Id = 62 },

            new GroupingCategoryTranslation(34, "Top", LanguageTypeEnum.EN) { Id = 63 },
            new GroupingCategoryTranslation(34, "Felső", LanguageTypeEnum.HU) { Id = 64 },

            new GroupingCategoryTranslation(35, "Living room", LanguageTypeEnum.EN) { Id = 65 },
            new GroupingCategoryTranslation(35, "Nappali", LanguageTypeEnum.HU) { Id = 66 },

            new GroupingCategoryTranslation(36, "Knobs & Handles", LanguageTypeEnum.EN) { Id = 67 },
            new GroupingCategoryTranslation(36, "Kilincsek, fogantyúk", LanguageTypeEnum.HU) { Id = 68 },

            new GroupingCategoryTranslation(37, "Ligthning", LanguageTypeEnum.EN) { Id = 69 },
            new GroupingCategoryTranslation(37, "Világítás", LanguageTypeEnum.HU) { Id = 70 },

            new GroupingCategoryTranslation(38, "Ligthning", LanguageTypeEnum.EN) { Id = 71 },
            new GroupingCategoryTranslation(38, "Világítás", LanguageTypeEnum.HU) { Id = 72 },



            new GroupingCategoryTranslation(42, "Shipping", LanguageTypeEnum.EN) { Id = 73 },
            new GroupingCategoryTranslation(42, "Kiszállítás", LanguageTypeEnum.HU) { Id = 74 },

            new GroupingCategoryTranslation(43, "Installation", LanguageTypeEnum.EN) { Id = 75 },
            new GroupingCategoryTranslation(43, "Beszerelés", LanguageTypeEnum.HU) { Id = 76 },

            new GroupingCategoryTranslation(44, "Survey", LanguageTypeEnum.EN) { Id = 77 },
            new GroupingCategoryTranslation(44, "Helyszíni felmérés", LanguageTypeEnum.HU) { Id = 78 }

        };
    }
}
