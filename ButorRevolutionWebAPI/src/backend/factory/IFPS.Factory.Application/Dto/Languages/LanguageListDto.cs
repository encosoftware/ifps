using ENCO.DDD.Domain.Model.Enums;

namespace IFPS.Factory.Application.Dto
{
    public class LanguageListDto
    {
        public LanguageTypeEnum LanguageType { get; set; }
        public string Translation { get; set; }
    }
}