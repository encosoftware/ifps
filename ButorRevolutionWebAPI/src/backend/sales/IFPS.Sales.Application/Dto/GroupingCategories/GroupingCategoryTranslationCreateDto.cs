using ENCO.DDD.Domain.Model.Enums;

namespace IFPS.Sales.Application.Dto
{
    public class GroupingCategoryTranslationCreateDto
    {
        public string Name { get; set; }
        public LanguageTypeEnum Language { get; set; }  
    }
}
