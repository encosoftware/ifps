using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Sales.EF.Repositories
{
    public class DocumentRepository : EFCoreRepositoryBase<IFPSSalesContext, Document, Guid>, IDocumentRepository
    {
        public DocumentRepository(IFPSSalesContext context) : base(context)
        { }

        protected override List<Expression<Func<Document, object>>> DefaultIncludes => new List<Expression<Func<Document, object>>>
        {
            
        };

        public virtual Task<int> GetCountOfSameTypeOfDocumentAsync(Guid orderId, int documentTypeId)
        {
            return context.Documents
                .Where(ent => ent.DocumentGroupVersion.Core.OrderId == orderId && ent.DocumentTypeId == documentTypeId)
                .CountAsync();
                
        }
    }
}
