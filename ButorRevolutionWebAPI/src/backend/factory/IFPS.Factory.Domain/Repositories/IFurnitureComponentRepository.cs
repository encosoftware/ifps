using ENCO.DDD.Repositories;
using IFPS.Factory.Domain.Model;
using System;

namespace IFPS.Factory.Domain.Repositories
{
    public interface IFurnitureComponentRepository : IRepository<FurnitureComponent, Guid>
    {
    }
}