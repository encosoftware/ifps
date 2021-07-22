using IFPS.Sales.Domain.Enums;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class DocumentListDto
    {
        public Guid Id { get; set; }
        public FileExtensionTypeEnum FileExtensionType { get; set; }
        public string DisplayName { get; set; }
    }
}
