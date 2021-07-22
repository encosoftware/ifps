using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class FurnitureComponentCreateDto
    {
        public Guid FurnitureUnitId { get; set; }
        public string Name { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }
        public int TypeId { get; set; }
        public int Amount { get; set; }
        public Guid MaterialId { get; set; }
        public Guid TopFoilId { get; set; }
        public Guid BottomFoilId { get; set; }
        public Guid LeftFoilId { get; set; }
        public Guid RightFoilId { get; set; }
        public ImageCreateDto ImageCreateDto { get; set; }

        public FurnitureComponent CreateModelObject()
        {
            return new FurnitureComponent(Name, Width, Length, Amount)
            {
                FurnitureUnitId = FurnitureUnitId,
                Type = (FurnitureComponentTypeEnum)TypeId,
                BoardMaterialId = MaterialId,
                TopFoilId = TopFoilId,
                BottomFoilId = BottomFoilId,
                LeftFoilId = LeftFoilId,
                RightFoilId = RightFoilId
            };
        }
    }
}