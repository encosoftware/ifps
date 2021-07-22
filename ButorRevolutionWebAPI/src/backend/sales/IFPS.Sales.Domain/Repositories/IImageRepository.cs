using ENCO.DDD.Repositories;
using IFPS.Sales.Domain.Model;
using System;

namespace IFPS.Sales.Domain.Repositories
{
    public interface IImageRepository : IRepository<Image, Guid>
    {
        
    }
}
