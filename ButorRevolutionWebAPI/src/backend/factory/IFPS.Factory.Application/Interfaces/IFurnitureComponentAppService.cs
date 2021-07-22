using IFPS.Factory.Application.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IFurnitureComponentAppService
    {
        Task<FurnitureComponentWithSequenceDetailsDto> GetComponentWithSequenceAsync(Guid id);
        Task<List<FurnitureUnitWithComponentsDto>> GetComponentsByUnitIdAsync(Guid furnitureUnitId);

        Task<Guid> CreateFurnitureComponentWithSequencesFromXxlFile(Guid furnitureComponentId, string fileContent);
    }
}
