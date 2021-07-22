using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Factory.EF.Repositories
{
    public class CameraRepository : EFCoreRepositoryBase<IFPSFactoryContext, Camera>, ICameraRepository
    {
        public CameraRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<Camera, object>>> DefaultIncludes => new List<Expression<Func<Camera, object>>>
        {

        };
    }
}
