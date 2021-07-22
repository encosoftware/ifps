using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class CategoryDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public CategoryDetailsDto()
        {

        }

        public CategoryDetailsDto(GroupingCategory groupingCategory)
        {
            Id = groupingCategory.Id;
            Name = groupingCategory.CurrentTranslation.GroupingCategoryName;
        }
    }
}
