using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace IFPS.Sales.EF.Repositories
{
    public class DocumentStateRepository : EFCoreRepositoryBase<IFPSSalesContext, DocumentState>, IDocumentStateRepository
    {
        protected override List<Expression<Func<DocumentState, object>>> DefaultIncludes =>
            new List<Expression<Func<DocumentState, object>>>() { ent => ent.Translations };
        public DocumentStateRepository(IFPSSalesContext context) : base(context)
        {
        }

    }
}
