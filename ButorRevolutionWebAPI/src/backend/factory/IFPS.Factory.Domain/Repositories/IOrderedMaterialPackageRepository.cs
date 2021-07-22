using IFPS.Factory.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Repositories
{
    public interface IOrderedMaterialPackageRepository
    {
        Task<List<OrderedMaterialPackage>> GetOrderedMaterialPackagesWithInclude(int supplierId);
    }
}
