using ENCO.DDD.Domain.Model.Enums;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFPS.Sales.Domain.Seed
{
    public class DocumentStateTranslationSeed : IEntitySeed<DocumentStateTranslation>
    {
        public DocumentStateTranslation[] Entities => new[]
        {
            new DocumentStateTranslation(1, "Uploaded", LanguageTypeEnum.EN) { Id = 1},
            new DocumentStateTranslation(1, "Feltöltve", LanguageTypeEnum.HU) { Id = 2},

            new DocumentStateTranslation(2, "Waiting for Approval", LanguageTypeEnum.EN) { Id = 3},
            new DocumentStateTranslation(2, "Jóváhagyásra vár", LanguageTypeEnum.HU) { Id = 4},

            new DocumentStateTranslation(3, "Approved", LanguageTypeEnum.EN) { Id = 5},
            new DocumentStateTranslation(3, "Jóváhagyva", LanguageTypeEnum.HU) { Id = 6},

            new DocumentStateTranslation(4, "Declined", LanguageTypeEnum.EN) { Id = 7},
            new DocumentStateTranslation(4, "Elutasított", LanguageTypeEnum.HU) { Id = 8},

            new DocumentStateTranslation(5, "Empty", LanguageTypeEnum.EN) { Id = 9},
            new DocumentStateTranslation(5, "Üres", LanguageTypeEnum.HU) { Id = 10},


        };
    }
}
