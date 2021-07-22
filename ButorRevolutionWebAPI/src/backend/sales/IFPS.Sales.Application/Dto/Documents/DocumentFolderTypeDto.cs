using ENCO.DDD.Extensions;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace IFPS.Sales.Application.Dto
{
    public class DocumentFolderTypeDto
    {
        public int FolderId { get; set; }
        public string FolderName { get; set; }
        public DocumentFolderTypeEnum DocumentFolderType { get; set; }
        public List<DocumentTypeDto> SupportedTypes { get; set; }

        public static DocumentFolderTypeDto FromModel(DocumentFolder folder) => new DocumentFolderTypeDto()
        {
            FolderName = folder.Translations.GetCurrentTranslation().Name,
            DocumentFolderType = folder.DocumentFolderType,
            SupportedTypes = folder.DocumentTypes.Select(x => DocumentTypeDto.FromModel(x)).ToList(),
        };

        public static Expression<Func<DocumentFolder, DocumentFolderTypeDto>> Projection => x => new DocumentFolderTypeDto
        {
            FolderId = x.Id,
            FolderName = x.Translations.GetCurrentTranslation().Name,
            DocumentFolderType = x.DocumentFolderType,
            SupportedTypes = x.DocumentTypes.Select(y => new DocumentTypeDto
            {
                Id = y.Id,
                DocumentType = y.Type,
                Translation = y.Translations.GetCurrentTranslation().Name,
            }).ToList(),
        };
    }
}

