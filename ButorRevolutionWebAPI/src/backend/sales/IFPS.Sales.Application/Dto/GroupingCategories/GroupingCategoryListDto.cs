using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Application.Dto
{
    public class GroupingCategoryListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }     
        public GroupingCategoryEnum GroupingCategoryType { get; set; }

        public int? ParentId { get; set; }
        public string ParentName { get; set; }

        public List<GroupingCategoryListDto> Children { get; set; }

        public GroupingCategoryListDto()
        {

        }

        public GroupingCategoryListDto(GroupingCategory modelObject, bool loadChildren)
        {
            this.Id = modelObject.Id;
            this.Name = modelObject.CurrentTranslation.GroupingCategoryName;
            
            this.GroupingCategoryType = modelObject.CategoryType;
            this.ParentId = modelObject.ParentGroupId;            

            if (loadChildren)
            {
                this.Children = modelObject.Children.Select(x => new GroupingCategoryListDto(x, true)).ToList();
            }
        }

        public GroupingCategoryListDto(GroupingCategory modelObject)
        {
            this.Id = modelObject.Id;
            this.Name = modelObject.CurrentTranslation.GroupingCategoryName;

            this.GroupingCategoryType = modelObject.CategoryType;
            this.ParentId = modelObject.ParentGroupId;
            this.ParentName = modelObject.ParentGroup.CurrentTranslation.GroupingCategoryName;
        }
    }
}
