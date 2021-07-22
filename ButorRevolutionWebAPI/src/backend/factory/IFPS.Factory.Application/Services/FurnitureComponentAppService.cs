using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Repositories;
using IFPS.Factory.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class FurnitureComponentAppService : ApplicationService, IFurnitureComponentAppService
    {
        private readonly IFurnitureComponentRepository furnitureComponentRepository;
        private readonly IFurnitureUnitRepository furnitureUnitRepository;
        private readonly ITriDCorpusComponentLoaderService componentLoaderService;

        public FurnitureComponentAppService(IApplicationServiceDependencyAggregate aggregate,
           IFurnitureComponentRepository furnitureComponentRepository,
           IFurnitureUnitRepository furnitureUnitRepository,
           ITriDCorpusComponentLoaderService componentLoaderService)
            : base(aggregate)
        {
            this.furnitureComponentRepository = furnitureComponentRepository;
            this.furnitureUnitRepository = furnitureUnitRepository;
            this.componentLoaderService = componentLoaderService;
        }

        public async Task<FurnitureComponentWithSequenceDetailsDto> GetComponentWithSequenceAsync(Guid id)
        {
            var component = await furnitureComponentRepository.SingleIncludingAsync(ent => ent.Id == id);

            return new FurnitureComponentWithSequenceDetailsDto(component);
        }

        public async Task<List<FurnitureUnitWithComponentsDto>> GetComponentsByUnitIdAsync(Guid furnitureUnitId)
        {
            var unit = await furnitureUnitRepository.SingleIncludingAsync(ent => ent.Id == furnitureUnitId, x => x.Components);
            return unit.Components.Select(ent => new FurnitureUnitWithComponentsDto(ent)).ToList();
        }

        public async Task<Guid> CreateFurnitureComponentWithSequencesFromXxlFile(Guid furnitureComponentId, string fileContent)
        {
            var component = await furnitureComponentRepository.SingleIncludingAsync(ent => ent.Id == furnitureComponentId);
            componentLoaderService.LoadComponentFromXXLFile(ref component, fileContent);
            await furnitureComponentRepository.InsertAsync(component);

            await unitOfWork.SaveChangesAsync();

            return component.Id;
        }
    }
}
