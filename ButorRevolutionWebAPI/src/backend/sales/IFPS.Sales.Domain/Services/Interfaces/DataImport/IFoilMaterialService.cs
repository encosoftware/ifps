using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Services.Interfaces
{
    public interface IFoilMaterialService
    {
        Task CreateFoilMaterialsFromCsv(string containerName, string fileName);
    }
}
