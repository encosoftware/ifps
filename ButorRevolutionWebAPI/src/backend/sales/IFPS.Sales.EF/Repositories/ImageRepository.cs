using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace IFPS.Sales.EF.Repositories
{
    public class ImageRepository : EFCoreRepositoryBase<IFPSSalesContext, Image, Guid>, IImageRepository
    {
        public ImageRepository(IFPSSalesContext context) : base(context)
        { }

        protected override List<Expression<Func<Image, object>>> DefaultIncludes => new List<Expression<Func<Image, object>>>
        {            
        };
    }
}
