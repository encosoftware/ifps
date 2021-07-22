using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class WorktopBoardMaterialDetailsDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public ImageDetailsDto Image { get; set; }
        public DimensionDetailsDto Dimension { get; set; }
        public bool HasFiberDirection { get; set; }
        public PriceDetailsDto Price { get; set; }
        public int TransactionMultiplier { get; set; }

        public WorktopBoardMaterialDetailsDto() { }

        public WorktopBoardMaterialDetailsDto(WorktopBoardMaterial worktopBoardMaterial)
        {
            Id = worktopBoardMaterial.Id;
            Code = worktopBoardMaterial.Code;
            Description = worktopBoardMaterial.Description;
            CategoryId = worktopBoardMaterial.CategoryId.Value;
            Image = new ImageDetailsDto(worktopBoardMaterial.Image);
            Dimension = new DimensionDetailsDto(worktopBoardMaterial.Dimension);
            HasFiberDirection = worktopBoardMaterial.HasFiberDirection;
            Price = new PriceDetailsDto(worktopBoardMaterial.CurrentPrice.Price);
            TransactionMultiplier = worktopBoardMaterial.TransactionMultiplier;
        }
    }
}
