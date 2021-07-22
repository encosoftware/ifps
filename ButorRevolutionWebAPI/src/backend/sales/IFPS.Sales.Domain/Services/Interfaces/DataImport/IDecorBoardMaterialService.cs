using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Services.Interfaces
{
    public interface IDecorBoardMaterialService
    {
        Task CreateDecorBoardMaterialsFromCsv(string containerName, string fileName);
    }
}
