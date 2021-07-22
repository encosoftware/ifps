using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class GroupingCategoryDetailsDto
    {
        public int Id { get; set; }
        public GroupingCategoryEnum GroupingCategoryType { get; set; }
        public int? ParentId { get; set; }

        public List<GroupingCategoryTranslationListDto> Translations { get; set; }

        public GroupingCategoryDetailsDto()
        {
        }

        public GroupingCategoryDetailsDto(GroupingCategory modelObject)
        {
            this.Id = modelObject.Id;
            this.GroupingCategoryType = modelObject.CategoryType;
            this.ParentId = modelObject.ParentGroupId;

            this.Translations = modelObject.Translations.Select(x => new GroupingCategoryTranslationListDto(x)).ToList();
        }
    }
}