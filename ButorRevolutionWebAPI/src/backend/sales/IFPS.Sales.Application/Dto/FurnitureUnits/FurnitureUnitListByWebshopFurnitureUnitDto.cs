using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class FurnitureUnitListByWebshopFurnitureUnitDto
    {
        public Guid FurnitureUnitId { get; set; }
        public string Name { get; set; }

        public FurnitureUnitListByWebshopFurnitureUnitDto(FurnitureUnit furnitureUnit)
        {
            FurnitureUnitId = furnitureUnit.Id;
            Name = furnitureUnit.Code;
        }
    }
}
