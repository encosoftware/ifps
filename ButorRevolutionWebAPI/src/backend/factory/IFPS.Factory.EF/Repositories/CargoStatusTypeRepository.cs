using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IFPS.Factory.EF.Repositories
{
    public class CargoStatusTypeRepository : EFCoreRepositoryBase<IFPSFactoryContext, CargoStatusType>, ICargoStatusTypeRepository
    {
        public CargoStatusTypeRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<CargoStatusType, object>>> DefaultIncludes => new List<Expression<Func<CargoStatusType, object>>>
        {
            ent => ent.Translations
        };
    }
}
