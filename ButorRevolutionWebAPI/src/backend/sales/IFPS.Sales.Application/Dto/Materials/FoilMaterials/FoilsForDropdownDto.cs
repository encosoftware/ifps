using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Application.Dto
{
    public class FoilsForDropdownDto
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public FoilsForDropdownDto(FoilMaterial foil)
        {
            Id = foil.Id;
            Code = foil.Code;
        }
    }
}
