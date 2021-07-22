using ENCO.DDD.Extensions;
using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace IFPS.Sales.Application.Dto
{
    public class DocumentGroupDto
    {
        public int DocumentGroupId { get; set; }
        public bool IsVersionable { get; set; }
        public int FolderId { get; set; }
        public string FolderName { get; set; }
        public DocumentFolderTypeEnum DocumentFolderType { get; set; }

        public List<DocumentGroupVersionDto> Versions { get; set; }


        public static Expression<Func<DocumentGroup, DocumentGroupDto>> Projection => x => new DocumentGroupDto
        {
            DocumentGroupId = x.Id,
            FolderId = x.DocumentFolderId,
            FolderName = x.DocumentFolder.Translations.GetCurrentTranslation().Name,
            DocumentFolderType = x.DocumentFolder.DocumentFolderType,
            IsVersionable = x.IsHistorized,
            Versions = x.Versions.Select(y => new DocumentGroupVersionDto
            {
                Id = y.Id,
                DocumentState = new DocumentStateDto(y.State.State, y.State.Translations.GetCurrentTranslation().Name),
                LastModifiedOn = y.LastModificationTime,
                Documents = y.Documents.Select(z => new DocumentListDto
                {
                    Id = z.Id,
                    DisplayName = z.DisplayName,
                    FileExtensionType = z.FileExtensionType,
                }).ToList(),
            }).OrderByDescending(ent => ent.LastModifiedOn).ToList(),
        };
    }
}
