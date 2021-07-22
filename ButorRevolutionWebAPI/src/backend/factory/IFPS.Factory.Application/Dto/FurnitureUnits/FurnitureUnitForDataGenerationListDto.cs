using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Application.Dto
{
    public class FurnitureUnitForDataGenerationListDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public FurnitureUnitForDataGenerationListDto(FurnitureUnit furnitureUnit)
        {
            Id = furnitureUnit.Id;
            Name = furnitureUnit.Code;
        }
    }
}
