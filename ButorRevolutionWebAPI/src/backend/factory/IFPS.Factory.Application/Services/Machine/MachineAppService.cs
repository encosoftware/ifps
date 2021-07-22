using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class MachineAppService : ApplicationService, IMachineAppService
    {
        private readonly IMachineRepository machineRepository;

        public MachineAppService(
            IMachineRepository machineRepository,
            IApplicationServiceDependencyAggregate aggregate) : base(aggregate)
        {
            this.machineRepository = machineRepository;
        }         

        public async Task<List<MachinesDropdownDto>> GetMachinesDropdownAsync()
        {
            return (await machineRepository.GetAllListAsync()).Select(ent => new MachinesDropdownDto(ent)).ToList();
        }       
    }
}
