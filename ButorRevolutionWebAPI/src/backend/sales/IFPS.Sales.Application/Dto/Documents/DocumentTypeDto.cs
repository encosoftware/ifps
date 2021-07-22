using ENCO.DDD.Extensions;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Sales.Application.Dto
{
    public class DocumentTypeDto
    {
        public int Id { get; set; }
        public DocumentTypeEnum DocumentType { get; set; }
        public string Translation { get; set; }

        public static DocumentTypeDto FromModel(DocumentType type) => new DocumentTypeDto()
        {
            Id = type.Id,
            DocumentType = type.Type,
            Translation = type.Translations.GetCurrentTranslation().Name,
        };

        public static Expression<Func<DocumentType, DocumentTypeDto>> Projection => 
            x => new DocumentTypeDto
            {
                Id = x.Id,
                DocumentType = x.Type,
                Translation = x.Translations.GetCurrentTranslation().Name,
            };
    }
}
