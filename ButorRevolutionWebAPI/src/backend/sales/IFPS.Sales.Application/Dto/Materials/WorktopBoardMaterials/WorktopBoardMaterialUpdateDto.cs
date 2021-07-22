using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class WorktopBoardMaterialUpdateDto
    {
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public ImageUpdateDto ImageUpdateDto { get; set; }
        public DimensionUpdateDto Dimension { get; set; }
        public bool HasFiberDirection { get; set; }
        public PriceUpdateDto PriceUpdateDto { get; set; }
        public int TransactionMultiplier { get; set; }

        public WorktopBoardMaterial UpdateModelObject(WorktopBoardMaterial worktopBoardMaterial)
        {
            worktopBoardMaterial.Description = Description;
            worktopBoardMaterial.CategoryId = CategoryId;
            worktopBoardMaterial.Dimension = Dimension.CreateModelObject();
            worktopBoardMaterial.HasFiberDirection = HasFiberDirection;
            worktopBoardMaterial.TransactionMultiplier = TransactionMultiplier;
            if (worktopBoardMaterial.CurrentPrice.Price != PriceUpdateDto.CreateModelObject())
            {
                worktopBoardMaterial.CurrentPrice.ValidTo = DateTime.Now;
                worktopBoardMaterial.AddPrice(new MaterialPrice(worktopBoardMaterial.Id, PriceUpdateDto.CreateModelObject()));
            }
            return worktopBoardMaterial;
        }
    }
}
