using IFPS.Sales.Domain.Model;
using System.Collections.Generic;

namespace IFPS.Sales.Application.Dto
{
    public class GroupingCategoryCreateDto
    {   
        public IList<GroupingCategoryTranslationCreateDto> Translations { get; set; }

        public int ParentId { get; set; }

        public GroupingCategoryCreateDto()
        {

        }

        public GroupingCategory ConvertToModelObject(GroupingCategory parent)
        {
            var groupingCategory = new GroupingCategory(parent);

            foreach (var translation in this.Translations)
            {
                groupingCategory.AddTranslation(new GroupingCategoryTranslation(translation.Name, translation.Language));
            }

            return groupingCategory;
        }
    }
}
