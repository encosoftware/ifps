using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class DecorBoardMaterialUpdateDto
    {
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public ImageUpdateDto ImageUpdateDto { get; set; }
        public DimensionUpdateDto Dimension { get; set; }
        public bool HasFiberDirection { get; set; }
        public PriceUpdateDto PriceUpdateDto { get; set; }
        public int TransactionMultiplier { get; set; }
        public DecorBoardMaterial UpdateModelObject(DecorBoardMaterial decorBoardMaterial)
        {
            decorBoardMaterial.Description = Description;
            decorBoardMaterial.CategoryId = CategoryId;
            decorBoardMaterial.HasFiberDirection = HasFiberDirection;
            decorBoardMaterial.Dimension = Dimension.CreateModelObject();
            decorBoardMaterial.TransactionMultiplier = TransactionMultiplier;
            if (decorBoardMaterial.CurrentPrice.Price != PriceUpdateDto.CreateModelObject())
            {
                decorBoardMaterial.CurrentPrice.ValidTo = DateTime.Now;
                decorBoardMaterial.AddPrice(new MaterialPrice(decorBoardMaterial.Id, PriceUpdateDto.CreateModelObject()));
            }
            return decorBoardMaterial;
        }
    }
}
