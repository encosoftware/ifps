using IFPS.Sales.Domain.Enums;

namespace IFPS.Sales.Application.Dto
{
    public class DocumentStateDto
    {
        public DocumentStateEnum State { get; set; }
        public string CurrentTranslation { get; set; }

        public DocumentStateDto(DocumentStateEnum state, string name)
        {
            State = state;
            CurrentTranslation = name;
        }
    }
}
