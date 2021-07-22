using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class FurnitureUnitsForDropdownDto
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public FurnitureUnitsForDropdownDto(FurnitureUnit furnitureUnit)
        {
            Id = furnitureUnit.Id;
            Code = furnitureUnit.Code;
        }
    }
}
