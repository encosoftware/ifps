using IFPS.Sales.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Services.Interfaces
{
    public interface IAccessoryMaterialService
    {
        Task CreateAccessoryMaterialsFromCsv(string containerName, string fileName);
    }
}
