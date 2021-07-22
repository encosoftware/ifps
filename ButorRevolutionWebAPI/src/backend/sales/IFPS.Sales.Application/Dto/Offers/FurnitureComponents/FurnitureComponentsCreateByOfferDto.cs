using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class FurnitureComponentsCreateByOfferDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid? BoardMaterialId { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public int Amount { get; set; }

        public Guid? TopFoilId { get; set; }

        public Guid? BottomFoilId { get; set; }

        public Guid? RightFoilId { get; set; }

        public Guid? LeftFoilId { get; set; }

        public FurnitureComponentsCreateByOfferDto()
        {

        }

        public FurnitureComponent CreateModelObject(Guid furnitureUnitId)
        {
            return new FurnitureComponent(Name, Width, Height, Amount)
            {
                BoardMaterialId = BoardMaterialId,
                TopFoilId = TopFoilId,
                BottomFoilId = BottomFoilId,
                RightFoilId = RightFoilId,
                LeftFoilId = LeftFoilId,
                FurnitureUnitId = furnitureUnitId
            };
        }       
    }
}
