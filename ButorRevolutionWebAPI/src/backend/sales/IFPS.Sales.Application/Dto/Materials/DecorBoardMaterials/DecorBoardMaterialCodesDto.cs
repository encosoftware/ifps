using IFPS.Sales.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Sales.Application.Dto
{
    public class DecorBoardMaterialCodesDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string ParentName { get; set; }

        public DecorBoardMaterialCodesDto()
        {
        }

        public static Expression<Func<DecorBoardMaterial, DecorBoardMaterialCodesDto>> Projection = x => new DecorBoardMaterialCodesDto
        {
            Id = x.Id,
            Code = x.Code,
            //ParentName = x.Category.ParentGroup.CurrentTranslation.GroupingCategoryName
        };
    }
}

