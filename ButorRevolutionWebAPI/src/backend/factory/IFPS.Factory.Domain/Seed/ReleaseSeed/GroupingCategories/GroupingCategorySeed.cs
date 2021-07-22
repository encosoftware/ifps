using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Seed
{
    public class GroupingCategorySeed : IEntitySeed<GroupingCategory>
    {
        public GroupingCategory[] Entities => new[]
        {
            new GroupingCategory(GroupingCategoryEnum.MaterialType) {Id = 1},
            new GroupingCategory(GroupingCategoryEnum.FurnitureUnitType) {Id = 2},
            new GroupingCategory(GroupingCategoryEnum.DivisionType) {Id = 3},
            new GroupingCategory(GroupingCategoryEnum.RoleType) {Id = 4},
            new GroupingCategory(GroupingCategoryEnum.RoleType) {Id = 5, ParentGroupId = 4},
            new GroupingCategory(GroupingCategoryEnum.RoleType) {Id = 6, ParentGroupId = 4},
            new GroupingCategory(GroupingCategoryEnum.RoleType) {Id = 7, ParentGroupId = 4},
            new GroupingCategory(GroupingCategoryEnum.RoleType) {Id = 8, ParentGroupId = 4},

            new GroupingCategory(GroupingCategoryEnum.MenuType) {Id = 9},
            new GroupingCategory(GroupingCategoryEnum.MenuType) {Id = 10, ParentGroupId = 9},
            new GroupingCategory(GroupingCategoryEnum.MenuType) {Id = 11, ParentGroupId = 9},
            new GroupingCategory(GroupingCategoryEnum.MenuType) {Id = 12, ParentGroupId = 9},
            new GroupingCategory(GroupingCategoryEnum.MenuType) {Id = 13, ParentGroupId = 9},

            new GroupingCategory(GroupingCategoryEnum.AppointmentType) {Id = 14},
            new GroupingCategory(GroupingCategoryEnum.AppointmentType) {Id = 15, ParentGroupId = 14},
            new GroupingCategory(GroupingCategoryEnum.AppointmentType) {Id = 16, ParentGroupId = 14},

            new GroupingCategory(GroupingCategoryEnum.WorktopBoard) { Id = 17, ParentGroupId = 1 },
            new GroupingCategory(GroupingCategoryEnum.DecorBoard) { Id = 18, ParentGroupId = 1 },
            new GroupingCategory(GroupingCategoryEnum.Accessories) { Id = 19, ParentGroupId = 1 },
            new GroupingCategory(GroupingCategoryEnum.Appliances) { Id = 20, ParentGroupId = 1 },
            new GroupingCategory(GroupingCategoryEnum.FoilMaterial) { Id = 21, ParentGroupId = 1 },

            new GroupingCategory(GroupingCategoryEnum.Kitchen) { Id = 22, ParentGroupId = 2, ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c") },
            new GroupingCategory(GroupingCategoryEnum.Office) { Id = 23, ParentGroupId = 2, ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c") },
            new GroupingCategory(GroupingCategoryEnum.Bathroom) { Id = 24, ParentGroupId = 2, ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c") },

            new GroupingCategory(GroupingCategoryEnum.Islands) { Id = 25, ParentGroupId = 22, ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c") },
            new GroupingCategory(GroupingCategoryEnum.KnobsHandles) { Id = 26, ParentGroupId = 22, ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c") },
            new GroupingCategory(GroupingCategoryEnum.Lightning) { Id = 27, ParentGroupId = 22, ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c") }
        };
    }
}
