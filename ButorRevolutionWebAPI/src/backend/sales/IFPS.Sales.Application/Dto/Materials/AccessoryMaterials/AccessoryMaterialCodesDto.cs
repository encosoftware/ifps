using IFPS.Sales.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Sales.Application.Dto
{
    public class AccessoryMaterialCodesDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }

        public AccessoryMaterialCodesDto()
        {

        }

        public AccessoryMaterialCodesDto(AccessoryMaterial accessoryMaterial)
        {
            Id = accessoryMaterial.Id;
            Code = accessoryMaterial.Code;
        }

        public static Expression<Func<AccessoryMaterial, AccessoryMaterialCodesDto>> Projection
        {
            get
            {
                return ent => new AccessoryMaterialCodesDto
                {
                    Id = ent.Id,
                    Code = ent.Code
                };
            }
        }
    }
}
