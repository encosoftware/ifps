using IFPS.Sales.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Application.Dto
{
    public class GroupingCategoryWebshopListDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ImageDetailsDto Image { get; set; }

        public List<GroupingSubCategoryWebshopListDto> SubCategories { get; set; }

        public GroupingCategoryWebshopListDto(GroupingCategory category)
        {
            this.Id = category.Id;
            this.Name = category.CurrentTranslation?.GroupingCategoryName;
            this.Image = new ImageDetailsDto(category.Image);
            this.SubCategories = category.Children.Select(ent => new GroupingSubCategoryWebshopListDto(ent)).ToList();
        }
    }
}
