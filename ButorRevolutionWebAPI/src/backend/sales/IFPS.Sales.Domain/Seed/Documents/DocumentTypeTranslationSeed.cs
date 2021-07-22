using ENCO.DDD.Domain.Model.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed.Documents
{
    public class DocumentTypeTranslationSeed : IEntitySeed<DocumentTypeTranslation>
    {
        public DocumentTypeTranslation[] Entities => new[]
        {
            new DocumentTypeTranslation(1, "3D Render", LanguageTypeEnum.EN) { Id = 1},
            new DocumentTypeTranslation(1, "3D látványterv", LanguageTypeEnum.HU) { Id = 2},

            new DocumentTypeTranslation(2, "Offer", LanguageTypeEnum.EN) { Id = 3},
            new DocumentTypeTranslation(2, "Árajánlat", LanguageTypeEnum.HU) { Id = 4},

            new DocumentTypeTranslation(3, "Contract", LanguageTypeEnum.EN) { Id = 5},
            new DocumentTypeTranslation(3, "Szerződés", LanguageTypeEnum.HU) { Id = 6},

            new DocumentTypeTranslation(4, "Engineering drawing", LanguageTypeEnum.EN) { Id = 7},
            new DocumentTypeTranslation(4, "Gépészeti terv", LanguageTypeEnum.HU) { Id = 8},

            new DocumentTypeTranslation(5, "Technical drawing", LanguageTypeEnum.EN) { Id = 29},
            new DocumentTypeTranslation(5, "Műszaki rajz", LanguageTypeEnum.HU) { Id = 30},

            new DocumentTypeTranslation(6, "Floor plan", LanguageTypeEnum.EN) { Id = 9},
            new DocumentTypeTranslation(6, "Alaprajz", LanguageTypeEnum.HU) { Id = 10},

            new DocumentTypeTranslation(7, "On-site measurement", LanguageTypeEnum.EN) { Id = 11},
            new DocumentTypeTranslation(7, "Helyszíni felmérés", LanguageTypeEnum.HU) { Id = 12},

            new DocumentTypeTranslation(8, "Payment request", LanguageTypeEnum.EN) { Id = 13},
            new DocumentTypeTranslation(8, "Díjbekérő", LanguageTypeEnum.HU) { Id = 14},

            new DocumentTypeTranslation(9, "Production request", LanguageTypeEnum.EN) { Id = 15},
            new DocumentTypeTranslation(9, "Gyártásrendelő", LanguageTypeEnum.HU) { Id = 16},

            new DocumentTypeTranslation(10, "Repair form", LanguageTypeEnum.EN) { Id = 17},
            new DocumentTypeTranslation(10, "Javítási lap", LanguageTypeEnum.HU) { Id = 18},

            new DocumentTypeTranslation(11, "Delivery note", LanguageTypeEnum.EN) { Id = 19},
            new DocumentTypeTranslation(11, "Szállítólevél", LanguageTypeEnum.HU) { Id = 20},

            new DocumentTypeTranslation(12, "Certificate for installment", LanguageTypeEnum.EN) { Id = 21},
            new DocumentTypeTranslation(12, "Teljesítési igazolás beszerelésről", LanguageTypeEnum.HU) { Id = 22},
            
            new DocumentTypeTranslation(13, "Guarantee repair form", LanguageTypeEnum.EN) { Id = 23},
            new DocumentTypeTranslation(13, "Garanciális javítási lap", LanguageTypeEnum.HU) { Id = 24},
        };
    }
}
