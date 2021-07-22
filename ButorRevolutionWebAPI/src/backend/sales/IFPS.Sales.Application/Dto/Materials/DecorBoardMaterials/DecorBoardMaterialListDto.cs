using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class DecorBoardMaterialListDto
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

        public DecorBoardMaterialListDto(DecorBoardMaterial decorBoardMaterial)
        {
            Id = decorBoardMaterial.Id;
            Code = decorBoardMaterial.Code;
            Description = decorBoardMaterial.Description;
            Category = new CategoryListDto(decorBoardMaterial.Category);
            Image = new ImageThumbnailListDto(decorBoardMaterial.Image);
            Dimension = new DimensionDetailsDto(decorBoardMaterial.Dimension);
            HasFiberDirection = decorBoardMaterial.HasFiberDirection;
            Price = new PriceListDto(decorBoardMaterial.CurrentPrice.Price);
            TransactionMultiplier = decorBoardMaterial.TransactionMultiplier;
        }

        public DecorBoardMaterialListDto() { }

        public static Func<DecorBoardMaterial, DecorBoardMaterialListDto> FromEntity = entity => new DecorBoardMaterialListDto(entity);
    }
}
