using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Domain.Repositories.Material
{
    public interface IBoardMaterialRepository : IRepository<BoardMaterial, Guid>
    {
    }
}
