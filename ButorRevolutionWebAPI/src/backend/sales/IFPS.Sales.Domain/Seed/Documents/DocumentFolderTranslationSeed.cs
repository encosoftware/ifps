using ENCO.DDD.Domain.Model.Enums;
using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Domain.Seed.Documents
{
    public class DocumentFolderTranslationSeed : IEntitySeed<DocumentFolderTranslation>
    {
        public DocumentFolderTranslation[] Entities => new[]
        {
            new DocumentFolderTranslation(1, "3D Render", LanguageTypeEnum.EN) { Id = 1},
            new DocumentFolderTranslation(1, "3D Látványterv", LanguageTypeEnum.HU) { Id = 2},

            new DocumentFolderTranslation(2, "Offer", LanguageTypeEnum.EN) { Id = 3},
            new DocumentFolderTranslation(2, "Árajánlat", LanguageTypeEnum.HU) { Id = 4},

            new DocumentFolderTranslation(3, "Contract", LanguageTypeEnum.EN) { Id = 5},
            new DocumentFolderTranslation(3, "Szerződés", LanguageTypeEnum.HU) { Id = 6},

            new DocumentFolderTranslation(4, "Customer documents", LanguageTypeEnum.EN) { Id = 7},
            new DocumentFolderTranslation(4, "Ügyfél dokumentumok", LanguageTypeEnum.HU) { Id = 8},

            new DocumentFolderTranslation(5, "Production documents", LanguageTypeEnum.EN) { Id = 9},
            new DocumentFolderTranslation(5, "Gyártás dokumentáció", LanguageTypeEnum.HU) { Id = 10},

            new DocumentFolderTranslation(6, "Partner documents", LanguageTypeEnum.EN) { Id = 11 },
            new DocumentFolderTranslation(6, "Partner dokumentumok", LanguageTypeEnum.HU) { Id = 12 }

        };
    }
}
