using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class FurnitureComponentListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int Amount { get; set; }
        public string Code { get; set; }
        public string TopFoilName { get; set; }
        public string BottomFoilName { get; set; }
        public string LeftFoilName { get; set; }
        public string RightFoilName { get; set; }
        public ImageThumbnailListDto ImageThumbnailListDto { get; set; }

        public FurnitureComponentListDto(FurnitureComponent furnitureComponent)
        {
            Id = furnitureComponent.Id;
            Name = furnitureComponent.Name;
            Width = furnitureComponent.Width;
            Height = furnitureComponent.Length;
            Amount = furnitureComponent.Amount;
            Code = furnitureComponent.BoardMaterial.Code;
            TopFoilName = furnitureComponent.TopFoil?.Code;
            BottomFoilName = furnitureComponent.BottomFoil?.Code;
            LeftFoilName = furnitureComponent.LeftFoil?.Code;
            RightFoilName = furnitureComponent.RightFoil?.Code;
            ImageThumbnailListDto = new ImageThumbnailListDto(furnitureComponent.Image);
        }
    }
}
