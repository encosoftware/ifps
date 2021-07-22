using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace IFPS.Factory.Application.Dto
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
            Translation = type.CurrentTranslation.Name,
        };

        public static Expression<Func<DocumentType, DocumentTypeDto>> Projection
        {
            get
            {
                return x => new DocumentTypeDto
                {
                    Id = x.Id,
                    DocumentType = x.Type,
                    Translation = x.CurrentTranslation.Name,
                };
            }
        }
    }
}
