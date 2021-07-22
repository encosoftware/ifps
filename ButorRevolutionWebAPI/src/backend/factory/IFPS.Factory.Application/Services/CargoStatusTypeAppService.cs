using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class CargoStatusTypeAppService : ApplicationService, ICargoStatusTypeAppService
    {
        private readonly ICargoStatusTypeRepository cargoStatusTypeRepository;

        public CargoStatusTypeAppService(
            IApplicationServiceDependencyAggregate aggregate,
            ICargoStatusTypeRepository cargoStatusTypeRepository) : base(aggregate)
        {
            this.cargoStatusTypeRepository = cargoStatusTypeRepository;
        }

        public async Task<List<CargoStatusTypeDropdownListDto>> GetCargoStatusTypeForDropdownAsync()
        {
            var statuses = await cargoStatusTypeRepository.GetAllListAsync();
            return statuses.Select(ent => new CargoStatusTypeDropdownListDto(ent)).ToList();
        }
    }
}
