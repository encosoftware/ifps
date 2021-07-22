using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Services.Interfaces
{
    public interface IWorktopBoardMaterialService
    {
        Task CreateWorktopBoardMaterialsFromCsv(string containerName, string fileName);
    }
}
