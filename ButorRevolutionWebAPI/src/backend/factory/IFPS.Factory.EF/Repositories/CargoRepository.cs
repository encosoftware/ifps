using ENCO.DDD.EntityFrameworkCore.Relational.Repositories;
using ENCO.DDD.Paging;
using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.EF.Repositories
{
    public class CargoRepository : EFCoreRepositoryBase<IFPSFactoryContext, Cargo>, ICargoRepository
    {
        public CargoRepository(IFPSFactoryContext context) : base(context) { }

        protected override List<Expression<Func<Cargo, object>>> DefaultIncludes => new List<Expression<Func<Cargo, object>>> { };

        public async Task<IPagedList<Cargo>> GetPagedCargoAsync
            (List<CargoStatusEnum> statuses,
            Expression<Func<Cargo, bool>> predicate = null,
            Func<IQueryable<Cargo>, IOrderedQueryable<Cargo>> orderBy = null,
            int pageIndex = 0,
            int pageSize = 20)
        {
            var cargos = GetAll()
                .Include(ent => ent.Vat)
                    .ThenInclude(ent => ent.Currency)
                .Include(ent => ent.Status)
                    .ThenInclude(ent => ent.Translations)
                .Include(ent => ent.Supplier)
                .Include(ent => ent.BookedBy)
                    .ThenInclude(ent => ent.CurrentVersion)
                .Where(ent => statuses.Contains(ent.Status.Status));

            return await GetPagedListAsync(cargos, predicate, orderBy, pageIndex, pageSize);
        }

        public async Task<Cargo> GetCargoByIdWithIncludes(int cargoId)
        {
            var result = await GetAll()
                .Include(ent => ent.BookedBy)
                    .ThenInclude(ent => ent.Company)
                        .ThenInclude(ent => ent.CurrentVersion)
                            .ThenInclude(ent => ent.ContactPerson)
                .Include(ent => ent.BookedBy)
                    .ThenInclude(ent => ent.CurrentVersion)
                .Include(ent => ent.Status)
                    .ThenInclude(status => status.Translations)
                .Include(ent => ent.Supplier)
                    .ThenInclude(ent => ent.CurrentVersion)
                        .ThenInclude(ent => ent.ContactPerson)
                            .ThenInclude(ent => ent.CurrentVersion)
                .Include(ent => ent.OrderedPackages)
                    .ThenInclude(ent => ent.MaterialPackage)
                        .ThenInclude(package => package.Material)
                .Include(ent => ent.OrderedPackages)
                    .ThenInclude(ent => ent.UnitPrice)
                        .ThenInclude(package => package.Currency)
                .Include(ent => ent.NetCost)
                    .ThenInclude(cost => cost.Currency)
                .SingleOrDefaultAsync(ent => ent.Id == cargoId);

            return result ?? throw new EntityNotFoundException(typeof(Cargo), cargoId);
        }
    }
}
