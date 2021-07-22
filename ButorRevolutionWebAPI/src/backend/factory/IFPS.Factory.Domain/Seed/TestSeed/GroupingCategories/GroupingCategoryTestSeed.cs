using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;

namespace IFPS.Factory.Domain.Seed
{
    public class GroupingCategoryTestSeed : IEntitySeed<GroupingCategory>
    {
        public GroupingCategory[] Entities => new[]
        {
            new GroupingCategory(GroupingCategoryEnum.MaterialType) {Id = 10000},
            new GroupingCategory(GroupingCategoryEnum.FurnitureUnitType) {Id = 10001},
            new GroupingCategory(GroupingCategoryEnum.DivisionType) {Id = 10002},
            new GroupingCategory(GroupingCategoryEnum.RoleType) {Id = 10003},
            new GroupingCategory(GroupingCategoryEnum.RoleType) {Id = 10004, ParentGroupId = 10003},
            new GroupingCategory(GroupingCategoryEnum.RoleType) {Id = 10005, ParentGroupId = 10003},
            new GroupingCategory(GroupingCategoryEnum.RoleType) {Id = 10006, ParentGroupId = 10003},
            new GroupingCategory(GroupingCategoryEnum.RoleType) {Id = 10007, ParentGroupId = 10003},

            new GroupingCategory(GroupingCategoryEnum.MenuType) {Id = 10008},
            new GroupingCategory(GroupingCategoryEnum.MenuType) {Id = 10009, ParentGroupId = 10008},
            new GroupingCategory(GroupingCategoryEnum.MenuType) {Id = 10010, ParentGroupId = 10008},
            new GroupingCategory(GroupingCategoryEnum.MenuType) {Id = 10011, ParentGroupId = 10008},
            new GroupingCategory(GroupingCategoryEnum.MenuType) {Id = 10012, ParentGroupId = 10008}
        };
    }
}