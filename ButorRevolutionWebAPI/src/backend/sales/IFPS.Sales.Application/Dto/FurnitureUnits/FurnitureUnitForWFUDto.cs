using IFPS.Sales.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Sales.Application.Dto
{
    public class FurnitureUnitForWFUDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsDeletable { get; set; }
        public ImageThumbnailDetailsDto ImageDetailsDto { get; set; }

        public static Expression<Func<FurnitureUnit, FurnitureUnitForWFUDto>> Projection
        {
            get
            {
                return ent => new FurnitureUnitForWFUDto
                {
                    Code = ent.Code,
                    Description = ent.Description,
                    ImageDetailsDto = new ImageThumbnailDetailsDto(ent.Image)
                };
            }
        }
    }
}
