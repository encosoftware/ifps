using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories.Material;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Sales.EF.Repositories.Material
{
    public class BoardMaterialRepository : EFCoreRepositoryBase<IFPSSalesContext, BoardMaterial, Guid>, IBoardMaterialRepository
    {
        public BoardMaterialRepository(IFPSSalesContext context) : base(context)
        { }

        protected override List<Expression<Func<BoardMaterial, object>>> DefaultIncludes => new List<Expression<Func<BoardMaterial, object>>> { };
    }
}
