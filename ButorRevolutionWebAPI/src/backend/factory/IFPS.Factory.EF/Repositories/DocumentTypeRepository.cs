using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Factory.EF.Repositories
{
    public class DocumentTypeRepository : EFCoreRepositoryBase<IFPSFactoryContext, DocumentType>, IDocumentTypeRepository
    {
        public DocumentTypeRepository(IFPSFactoryContext context) : base(context)
        {
        }

        protected override List<Expression<Func<DocumentType, object>>> DefaultIncludes => new List<Expression<Func<DocumentType, object>>>
        {
            ent=>ent.Translations
        };
    }
}
