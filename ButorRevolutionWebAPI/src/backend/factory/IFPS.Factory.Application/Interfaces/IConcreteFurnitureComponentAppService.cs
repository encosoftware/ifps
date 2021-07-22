using IFPS.Factory.Application.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IConcreteFurnitureComponentAppService
    {
        Task GenerateQRCodeAsync(int cfcId);
        Task<List<ConcreteFurnitureComponentInformationListDto>> GetConcreteFurnitureComponentsAsync(Guid orderId);
    }
}
