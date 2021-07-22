using System;
using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class DocumentGroupVersionDto
    {
        public int Id { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public DocumentStateDto DocumentState { get; set; }
        public List<DocumentListDto> Documents  { get; set; }
    }
}
