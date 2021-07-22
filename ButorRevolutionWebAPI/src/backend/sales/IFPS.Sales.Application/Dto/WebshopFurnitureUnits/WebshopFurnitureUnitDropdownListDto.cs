using IFPS.Sales.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Sales.Application.Dto
{
    public class WebshopFurnitureUnitDropdownListDto
    {
        public int Id { get; set; }
        public string FurnitureUnitCode { get; set; }

        public WebshopFurnitureUnitDropdownListDto() { }

        public static Expression<Func<WebshopFurnitureUnit, WebshopFurnitureUnitDropdownListDto>> Projection
        {
            get
            {
                return ent => new WebshopFurnitureUnitDropdownListDto
                {
                    Id = ent.Id,
                    FurnitureUnitCode = ent.FurnitureUnit.Code
                };
            }
        }
    }
}
