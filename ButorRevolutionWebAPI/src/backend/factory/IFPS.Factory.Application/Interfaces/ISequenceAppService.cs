
using IFPS.Factory.Application.Dto;
using System;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface ISequenceAppService
    {
        Task<int> CreateRectangleBySequenceAsync(Guid componentId, RectangleBySequenceCreateDto dto);
        Task<int> CreateDrillBySequenceAsync(Guid componentId, DrillBySequenceCreateDto dto);
    }
}
