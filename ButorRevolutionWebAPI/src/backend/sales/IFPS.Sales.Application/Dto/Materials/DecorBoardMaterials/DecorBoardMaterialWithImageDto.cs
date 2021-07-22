using IFPS.Sales.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Sales.Application.Dto
{
    public class DecorBoardMaterialWithImageDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public ImageListDto Image { get; set; }
        public string Description { get; set; }

        public DecorBoardMaterialWithImageDto() { }

        public static Expression<Func<DecorBoardMaterial, DecorBoardMaterialWithImageDto>> Projection
        {
            get
            {
                return ent => new DecorBoardMaterialWithImageDto
                {
                    Id = ent.Id,
                    Code = ent.Code,
                    Image = new ImageListDto(ent.Image),
                    Description = ent.Description
                };
            }
        }
    }
}
