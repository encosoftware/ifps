using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class DocumentDetailsDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string ContainerName { get; set; }
        public FileExtensionTypeEnum FileExtensionType { get; set; }
        public string DisplayName { get; set; }

        public static DocumentDetailsDto FromModel(Document document)
        {
            return new DocumentDetailsDto()
            {
                Id = document.Id,
                ContainerName = document.ContainerName,
                FileName = document.FileName,
                DisplayName = document.DisplayName,
                FileExtensionType = document.FileExtensionType,
            };
        }
    }
}
