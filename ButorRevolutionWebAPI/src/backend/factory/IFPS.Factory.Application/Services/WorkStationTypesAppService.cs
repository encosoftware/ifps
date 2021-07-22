using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Repositories;

namespace IFPS.Factory.Application.Services
{
    public class WorkStationTypesAppService : ApplicationService, IWorkStationTypesAppService
    {
        private readonly IWorkStationTypeRepository workStationTypeRepository;

        public WorkStationTypesAppService(IWorkStationTypeRepository workStationTypeRepository,
            IApplicationServiceDependencyAggregate aggregate) : base(aggregate)
        {
            this.workStationTypeRepository = workStationTypeRepository;
        }

        public async Task<List<WorkStationTypeListDto>> GetWorkStationTypesAsync()
        {
            var workStationTypes = await workStationTypeRepository.GetAllListAsync();
            return workStationTypes.Select(ent => new WorkStationTypeListDto(ent)).ToList();
        }

        public async Task<List<WorkStationTypeListWithMachinesDto>> GetWorkStationTypesWithMachinesAsync()
        {
            //var workStationTypes = await workStationTypeRepository.GetAllListAsync();

            //var dtoList = new List<WorkStationTypeListWithMachinesDto>();
            //foreach (var workStationType in workStationTypes)
            //{
            //    var machines = await machineRepository.GetAllListAsync(ent => ent.WorkStationTypeId == workStationType.Id);
            //    var dto = new WorkStationTypeListWithMachinesDto(workStationType, machines);
            //    dtoList.Add(dto);
            //}
            //return dtoList;
            return null;
        }
    }
}
