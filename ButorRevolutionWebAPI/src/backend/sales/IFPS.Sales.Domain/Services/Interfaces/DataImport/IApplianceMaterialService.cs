using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Services.Interfaces
{
    public interface IApplianceMaterialService
    {
        Task CreateApplianceMaterialsFromCsv(string containerName, string fileName);
    }
}
