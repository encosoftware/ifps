﻿using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace IFPS.Factory.EF.Repositories
{
    public class FurnitureComponentRepository : EFCoreRepositoryBase<IFPSFactoryContext, FurnitureComponent, Guid>, IFurnitureComponentRepository
    {
        public FurnitureComponentRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<FurnitureComponent, object>>> DefaultIncludes => new List<Expression<Func<FurnitureComponent, object>>> { };
    }
}
