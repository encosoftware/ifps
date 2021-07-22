using ENCO.DDD.Extensions;
using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace IFPS.Factory.Application.Dto
{
    public class DocumentFolderTypeDto
    {
        public int FolderId { get; set; }
        public string FolderName { get; set; }
        public List<DocumentTypeDto> SupportedTypes { get; set; }

        public static DocumentFolderTypeDto FromModel(DocumentFolder folder) => new DocumentFolderTypeDto()
        {
            FolderName = folder.Translations.GetCurrentTranslation().Name,
            SupportedTypes = folder.DocumentTypes.Select(x => DocumentTypeDto.FromModel(x)).ToList(),
        };

        public static Expression<Func<DocumentFolder, DocumentFolderTypeDto>> Projection => x => new DocumentFolderTypeDto
        {
            FolderId = x.Id,
            FolderName = x.Translations.GetCurrentTranslation().Name,
            SupportedTypes = x.DocumentTypes.Select(y => new DocumentTypeDto
            {
                Id = y.Id,
                DocumentType = y.Type,
                Translation = y.Translations.GetCurrentTranslation().Name,
            }).ToList(),
        };
    }
}
