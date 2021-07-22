using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Factory.EF.Repositories
{
    public class DocumentRepository : EFCoreRepositoryBase<IFPSFactoryContext, Document, Guid>, IDocumentRepository
    {
        public DocumentRepository(IFPSFactoryContext context) : base(context)
        { }

        protected override List<Expression<Func<Document, object>>> DefaultIncludes => new List<Expression<Func<Document, object>>>
        {

        };
    }
}
