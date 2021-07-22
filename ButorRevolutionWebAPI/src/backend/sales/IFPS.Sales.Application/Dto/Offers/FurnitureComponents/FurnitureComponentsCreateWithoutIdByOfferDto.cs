using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class FurnitureComponentsCreateWithoutIdByOfferDto
    {
        public string Name { get; set; }

        public Guid? BoardMaterialId { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public int Amount { get; set; }

        public Guid? TopFoilId { get; set; }

        public Guid? BottomFoilId { get; set; }

        public Guid? RightFoilId { get; set; }

        public Guid? LeftFoilId { get; set; }

        public Guid FurnitureUnitId { get; set; }

        public FurnitureComponentsCreateWithoutIdByOfferDto(FurnitureComponent component)
        {
            Name = component.Name;
            BoardMaterialId = component.BoardMaterialId;
            Width = component.Width;
            Height = component.Length;
            Amount = component.Amount;
            TopFoilId = component.TopFoilId;
            BottomFoilId = component.BottomFoilId;
            RightFoilId = component.RightFoilId;
            LeftFoilId = component.LeftFoilId;
            FurnitureUnitId = component.FurnitureUnitId;
        }

        public FurnitureComponent CreateModelObject(Guid furnitureUnitId, Guid imageId)
        {
            return new FurnitureComponent(Name, Width, Height, Amount)
            {
                BoardMaterialId = BoardMaterialId,
                TopFoilId = TopFoilId,
                BottomFoilId = BottomFoilId,
                RightFoilId = RightFoilId,
                LeftFoilId = LeftFoilId,
                FurnitureUnitId = furnitureUnitId,
                ImageId = imageId
            };
        }
    }
}
