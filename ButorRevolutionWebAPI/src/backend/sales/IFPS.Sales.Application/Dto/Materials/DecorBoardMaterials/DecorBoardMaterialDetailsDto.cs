using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class DecorBoardMaterialDetailsDto
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

        public DecorBoardMaterialDetailsDto() { }

        public DecorBoardMaterialDetailsDto(DecorBoardMaterial decorBoardMaterial)
        {
            Id = decorBoardMaterial.Id;
            Code = decorBoardMaterial.Code;
            Description = decorBoardMaterial.Description;
            CategoryId = decorBoardMaterial.CategoryId.Value;
            Image = new ImageDetailsDto(decorBoardMaterial.Image);
            Dimension = new DimensionDetailsDto(decorBoardMaterial.Dimension);
            HasFiberDirection = decorBoardMaterial.HasFiberDirection;
            Price = new PriceDetailsDto(decorBoardMaterial.CurrentPrice.Price);
            TransactionMultiplier = decorBoardMaterial.TransactionMultiplier;
        }
    }
}
