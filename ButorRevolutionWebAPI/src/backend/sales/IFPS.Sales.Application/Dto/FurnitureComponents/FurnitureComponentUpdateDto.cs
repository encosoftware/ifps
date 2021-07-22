using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class FurnitureComponentUpdateDto
    {
        public string Name { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int Amount { get; set; }
        public Guid MaterialId { get; set; }
        public Guid? TopFoilId { get; set; }
        public Guid? BottomFoilId { get; set; }
        public Guid? LeftFoilId { get; set; }
        public Guid? RightFoilId { get; set; }
        public ImageUpdateDto ImageUpdateDto { get; set; }

        public FurnitureComponent UpdateModelObject(FurnitureComponent furnitureComponent)
        {
            furnitureComponent.Name = Name;
            furnitureComponent.Width = Width;
            furnitureComponent.Length = Height;
            furnitureComponent.Amount = Amount;
            furnitureComponent.BoardMaterialId = MaterialId;
            furnitureComponent.TopFoilId = TopFoilId;
            furnitureComponent.BottomFoilId = BottomFoilId;
            furnitureComponent.LeftFoilId = LeftFoilId;
            furnitureComponent.RightFoilId = RightFoilId;
            return furnitureComponent;
        }
    }
}
