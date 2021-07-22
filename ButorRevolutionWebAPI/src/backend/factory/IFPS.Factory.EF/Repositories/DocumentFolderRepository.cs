using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Factory.EF.Repositories
{
    public class DocumentFolderRepository : EFCoreRepositoryBase<IFPSFactoryContext, DocumentFolder>, IDocumentFolderRepository
    {
        public DocumentFolderRepository(IFPSFactoryContext context) : base(context)
        {
        }

        protected override List<Expression<Func<DocumentFolder, object>>> DefaultIncludes => new List<Expression<Func<DocumentFolder, object>>>
        {
            ent => ent.Translations
        };
    }
}
