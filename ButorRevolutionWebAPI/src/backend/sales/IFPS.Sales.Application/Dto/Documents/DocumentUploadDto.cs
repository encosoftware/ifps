using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class DocumentUploadDto
    {
        public int DocumentGroupId{ get; set; }
        public int? DocumentGroupVersionId{ get; set; }
        public int DocumentTypeId { get; set; }
        public int UploaderUserId { get; set; }
        public List<DocumentCreateDto> Documents { get; set; }
    }
}
