using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class WorktopBoardMaterialListDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public CategoryListDto Category { get; set; }
        public ImageThumbnailListDto Image { get; set; }
        public DimensionDetailsDto Dimension { get; set; }
        public bool HasFiberDirection { get; set; }
        public PriceListDto Price { get; set; }
        public int TransactionMultiplier { get; set; }

        public WorktopBoardMaterialListDto(WorktopBoardMaterial worktopBoardMaterial)
        {
            Id = worktopBoardMaterial.Id;
            Code = worktopBoardMaterial.Code;
            Description = worktopBoardMaterial.Description;
            Category = new CategoryListDto(worktopBoardMaterial.Category);
            Image = new ImageThumbnailListDto(worktopBoardMaterial.Image);
            Dimension = new DimensionDetailsDto(worktopBoardMaterial.Dimension);
            HasFiberDirection = worktopBoardMaterial.HasFiberDirection;
            Price = new PriceListDto(worktopBoardMaterial.CurrentPrice.Price);
            TransactionMultiplier = worktopBoardMaterial.TransactionMultiplier;
        }

        public WorktopBoardMaterialListDto() { }

        public static Func<WorktopBoardMaterial, WorktopBoardMaterialListDto> FromEntity = entity => new WorktopBoardMaterialListDto(entity);
    }
}
