using IFPS.Factory.Domain.Model;
using System;
using System.Linq.Expressions;

namespace IFPS.Factory.Application.Dto
{
    public class MaterialListForDropdownDto
    {
        public Guid Id { get; set; }
        public string MaterialCode { get; set; }

        public static Expression<Func<Material, MaterialListForDropdownDto>> Projection
        {
            get
            {
                return ent => new MaterialListForDropdownDto
                {
                    Id = ent.Id,
                    MaterialCode = ent.Code
                };
            }
        }
    }
}