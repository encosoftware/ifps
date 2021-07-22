using IFPS.Factory.Domain.Enums;

namespace IFPS.Factory.Application.Dto
{
    public class GroupingCategoryFilterDto
    {
        public GroupingCategoryEnum CategoryType { get; set; }
        public bool LoadOnlyRootObjects { get; set; }
    }
}