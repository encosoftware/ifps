using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace IFPS.Factory.EF.Repositories
{
    public class DocumentStateRepository : EFCoreRepositoryBase<IFPSFactoryContext, DocumentState>, IDocumentStateRepository
    {
        protected override List<Expression<Func<DocumentState, object>>> DefaultIncludes =>
            new List<Expression<Func<DocumentState, object>>>() { ent => ent.Translations };
        public DocumentStateRepository(IFPSFactoryContext context) : base(context)
        {
        }

    }
}
