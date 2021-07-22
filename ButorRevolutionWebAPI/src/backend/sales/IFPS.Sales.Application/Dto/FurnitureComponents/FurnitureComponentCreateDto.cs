using IFPS.Sales.Domain.Enums;
using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class FurnitureComponentCreateDto
    {
        public Guid FurnitureUnitId { get; set; }
        public string Name { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public FurnitureComponentTypeEnum Type { get; set; }
        public int Amount { get; set; }
        public Guid MaterialId { get; set; }
        public Guid? TopFoilId { get; set; }
        public Guid? BottomFoilId { get; set; }
        public Guid? LeftFoilId { get; set; }
        public Guid? RightFoilId { get; set; }

        public FurnitureComponent CreateModelObject()
        {
            return new FurnitureComponent(Name, Width, Height, Amount)
            {
                FurnitureUnitId = FurnitureUnitId,
                Type = Type,
                BoardMaterialId = MaterialId,
                TopFoilId = TopFoilId,
                BottomFoilId = BottomFoilId,
                LeftFoilId = LeftFoilId,
                RightFoilId = RightFoilId
            };
        }
    }
}
