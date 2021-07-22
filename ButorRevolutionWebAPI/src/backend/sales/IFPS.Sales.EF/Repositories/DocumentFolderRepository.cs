using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace IFPS.Sales.EF.Repositories
{
    public class DocumentFolderRepository : EFCoreRepositoryBase<IFPSSalesContext, DocumentFolder>, IDocumentFolderRepository
    {
        public DocumentFolderRepository(IFPSSalesContext context) : base(context)
        {
        }

        protected override List<Expression<Func<DocumentFolder, object>>> DefaultIncludes => new List<Expression<Func<DocumentFolder, object>>>
        {
            ent => ent.Translations
        };
    }
}
