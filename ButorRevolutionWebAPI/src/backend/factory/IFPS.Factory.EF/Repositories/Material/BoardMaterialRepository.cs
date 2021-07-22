using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using ENCO.DDD.Paging;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.EF.Repositories
{
    public class BoardMaterialRepository : EFCoreRepositoryBase<IFPSFactoryContext, BoardMaterial, Guid>, IBoardMaterialRepository
    {
        public BoardMaterialRepository(IFPSFactoryContext context) : base(context)
        { }

        protected override List<Expression<Func<BoardMaterial, object>>> DefaultIncludes =>
            new List<Expression<Func<BoardMaterial, object>>>
        {
            ent => ent.Translations
        };
    }
}