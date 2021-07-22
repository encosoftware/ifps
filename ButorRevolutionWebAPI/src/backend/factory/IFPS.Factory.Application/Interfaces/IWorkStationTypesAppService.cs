using IFPS.Factory.Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IWorkStationTypesAppService
    {
        Task<List<WorkStationTypeListDto>> GetWorkStationTypesAsync();
        Task<List<WorkStationTypeListWithMachinesDto>> GetWorkStationTypesWithMachinesAsync();
    }
}
