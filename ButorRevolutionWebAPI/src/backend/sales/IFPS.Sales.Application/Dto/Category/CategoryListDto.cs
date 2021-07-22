using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class CategoryListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public CategoryListDto()
        {

        }

        public CategoryListDto(GroupingCategory groupingCategory)
        {
            Id = groupingCategory.Id;
            Name = groupingCategory.CurrentTranslation.GroupingCategoryName;
        }

        public static CategoryListDto FromEntity(GroupingCategory ent)
        {
            return new CategoryListDto()
            {
                Id = ent.Id,
                Name = ent.CurrentTranslation.GroupingCategoryName,
            };
        }
    }
}
