using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace IFPS.Sales.EF.Repositories
{
    public class DocumentTypeRepository : EFCoreRepositoryBase<IFPSSalesContext, DocumentType>, IDocumentTypeRepository
    {
        public DocumentTypeRepository(IFPSSalesContext context) : base(context)
        {
        }

        protected override List<Expression<Func<DocumentType, object>>> DefaultIncludes => new List<Expression<Func<DocumentType, object>>>
        {
            ent=>ent.Translations
        };
    }
}
