using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Domain.Seed.GroupingCategories
{
    public class GroupingCategorySeed : IEntitySeed<GroupingCategory>
    {
        public GroupingCategory[] Entities => new[]
        {
            new GroupingCategory(GroupingCategoryEnum.MaterialType) {Id = 1},        
            new GroupingCategory(GroupingCategoryEnum.DivisionType) {Id = 3},

            new GroupingCategory(GroupingCategoryEnum.MenuType) {Id = 9},
            new GroupingCategory(GroupingCategoryEnum.MenuType) {Id = 10, ParentGroupId = 9},
            new GroupingCategory(GroupingCategoryEnum.MenuType) {Id = 11, ParentGroupId = 9},
            new GroupingCategory(GroupingCategoryEnum.MenuType) {Id = 12, ParentGroupId = 9},
            new GroupingCategory(GroupingCategoryEnum.MenuType) {Id = 13, ParentGroupId = 9},

            new GroupingCategory(GroupingCategoryEnum.AppointmentType) {Id = 14},
            new GroupingCategory(GroupingCategoryEnum.AppointmentType) {Id = 15, ParentGroupId = 14},
            new GroupingCategory(GroupingCategoryEnum.AppointmentType) {Id = 16, ParentGroupId = 14},
            new GroupingCategory(GroupingCategoryEnum.AppointmentType) {Id = 42, ParentGroupId = 14},
            new GroupingCategory(GroupingCategoryEnum.AppointmentType) {Id = 43, ParentGroupId = 14},
            new GroupingCategory(GroupingCategoryEnum.AppointmentType) {Id = 44, ParentGroupId = 14},

            new GroupingCategory(GroupingCategoryEnum.WorktopBoard) { Id = 17, ParentGroupId = 1 },
            new GroupingCategory(GroupingCategoryEnum.DecorBoard) { Id = 18, ParentGroupId = 1 },
            new GroupingCategory(GroupingCategoryEnum.Accessories) { Id = 19, ParentGroupId = 1 },
            new GroupingCategory(GroupingCategoryEnum.Appliances) { Id = 20, ParentGroupId = 1 },
            new GroupingCategory(GroupingCategoryEnum.FoilMaterial) { Id = 21, ParentGroupId = 1 },

            new GroupingCategory(GroupingCategoryEnum.FurnitureUnitType) {Id = 2},
            new GroupingCategory(GroupingCategoryEnum.FurnitureUnitType) {Id = 32, ParentGroupId = 22, ImageId = new Guid("a07c074f-38ca-428e-ae53-7780b7012713")},
            new GroupingCategory(GroupingCategoryEnum.FurnitureUnitType) {Id = 33, ParentGroupId = 22, ImageId = new Guid("371fcae6-5196-4c35-818b-c9f5fafb09c9")},
            new GroupingCategory(GroupingCategoryEnum.FurnitureUnitType) {Id = 34, ParentGroupId = 22, ImageId = new Guid("556ca249-26bd-42ea-b41e-c68e216282ed")},

            // Webshop
            //new GroupingCategory(GroupingCategoryEnum.FurnitureUnitType) { Id = 22 },
            new GroupingCategory(GroupingCategoryEnum.Kitchen) { Id = 22, ParentGroupId = 2, ImageId = new Guid("12d668ca-cec4-43fb-9f7b-945fa92bbaac") },
            new GroupingCategory(GroupingCategoryEnum.Office) { Id = 23, ParentGroupId = 2, ImageId = new Guid("da4532c2-43fd-420f-b93a-c11afc44e9e5") },
            new GroupingCategory(GroupingCategoryEnum.Bathroom) { Id = 24, ParentGroupId = 2, ImageId = new Guid("3341a5ad-98b0-4e55-8ebe-f34c2439ed15") },

            new GroupingCategory(GroupingCategoryEnum.Islands) { Id = 25, ParentGroupId = 22, ImageId = new Guid("7cf3d233-da47-4f58-978e-bab8b9c4b118") },
            new GroupingCategory(GroupingCategoryEnum.KnobsHandles) { Id = 26, ParentGroupId = 22, ImageId = new Guid("95537a5a-11d7-4cf7-8c05-73e4a34f3097") },
            new GroupingCategory(GroupingCategoryEnum.Lightning) { Id = 27, ParentGroupId = 22, ImageId = new Guid("bc71cbf8-edb7-4ec4-8675-108e146ff40b") },

            new GroupingCategory(GroupingCategoryEnum.Appliances) {Id = 30, ParentGroupId = 20},
            new GroupingCategory(GroupingCategoryEnum.Appliances) {Id = 31, ParentGroupId = 30},

            new GroupingCategory(GroupingCategoryEnum.LivingRoom) { Id = 35, ParentGroupId = 2, ImageId = new Guid("703d5ae3-c1bc-43dd-bc46-9dd549ce2a7e") },

            new GroupingCategory(GroupingCategoryEnum.KnobsHandles) { Id = 36, ParentGroupId = 23, ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c") },
            new GroupingCategory(GroupingCategoryEnum.Lightning) { Id = 37, ParentGroupId = 24, ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c") },
            new GroupingCategory(GroupingCategoryEnum.Lightning) { Id = 38, ParentGroupId = 35, ImageId = new Guid("515b4ab1-a087-4373-96f1-25e6c058d02c") }


        };
    }
}
