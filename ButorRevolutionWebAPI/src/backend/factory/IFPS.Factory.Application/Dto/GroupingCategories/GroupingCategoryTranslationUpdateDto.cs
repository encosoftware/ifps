using ENCO.DDD.Domain.Model.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Application.Dto
{
    public class GroupingCategoryTranslationUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public LanguageTypeEnum Language { get; set; }

        public GroupingCategoryTranslationUpdateDto()
        {
        }

        public GroupingCategoryTranslationUpdateDto(GroupingCategoryTranslation modelObject)
        {
            this.Id = modelObject.Id;
            this.Name = modelObject.GroupingCategoryName;
            this.Language = modelObject.Language;
        }
    }
}