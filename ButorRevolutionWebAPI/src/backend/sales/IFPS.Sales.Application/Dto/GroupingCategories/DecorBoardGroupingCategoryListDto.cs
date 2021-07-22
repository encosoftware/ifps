using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class DecorBoardGroupingCategoryListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DecorBoardGroupingCategoryListDto(GroupingCategory groupingCategory)
        {
            Id = groupingCategory.Id;
            Name = groupingCategory.CurrentTranslation.GroupingCategoryName;
        }
    }
}
