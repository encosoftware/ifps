using IFPS.Sales.Domain.Enums;

namespace IFPS.Sales.Application.Dto
{
    public class GroupingCategoryFilterDto
    {
        public GroupingCategoryEnum CategoryType { get; set; }
        public bool LoadOnlyRootObjects { get; set; }
    }
}
