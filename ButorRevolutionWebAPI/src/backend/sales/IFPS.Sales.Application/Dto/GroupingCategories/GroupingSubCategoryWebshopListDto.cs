using IFPS.Sales.Domain.Model;

namespace IFPS.Sales.Application.Dto
{
    public class GroupingSubCategoryWebshopListDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ImageDetailsDto Image { get; set; }

        public GroupingSubCategoryWebshopListDto(GroupingCategory category)
        {
            this.Id = category.Id;
            this.Name = category.CurrentTranslation?.GroupingCategoryName;
            this.Image = new ImageDetailsDto(category.Image);
        }
    }
}
