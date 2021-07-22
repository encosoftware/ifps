using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class FurnitureComponentDetailsDto
    {
        public string Name { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public int Amount { get; set; }
        public Guid MaterialId { get; set; }
        public Guid TopFoilId { get; set; }
        public Guid BottomFoilId { get; set; }
        public Guid LeftFoilId { get; set; }
        public Guid RightFoilId { get; set; }
        public ImageThumbnailDetailsDto ImageThumbnailDetailsDto { get; set; }

        public FurnitureComponentDetailsDto(FurnitureComponent furnitureComponent)
        {
            Name = furnitureComponent.Name;
            Width = furnitureComponent.Width;
            Length = furnitureComponent.Length;
            Amount = furnitureComponent.Amount;
            MaterialId = furnitureComponent.BoardMaterialId.GetValueOrDefault();
            TopFoilId = furnitureComponent.TopFoilId.GetValueOrDefault();
            BottomFoilId = furnitureComponent.BottomFoilId.GetValueOrDefault();
            LeftFoilId = furnitureComponent.LeftFoilId.GetValueOrDefault();
            RightFoilId = furnitureComponent.RightFoilId.GetValueOrDefault();
            ImageThumbnailDetailsDto = new ImageThumbnailDetailsDto(furnitureComponent.Image);
        }
    }
}