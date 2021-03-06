using IFPS.Factory.Application.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Interfaces
{
    public interface IApplianceMaterialAppService
    {
        Task<Guid> CreateApplianceFromFileAsync(ApplianceMaterialCreateFromGeneratedDataDto dto);
        Task<List<ApplianceMaterialLisForDataGenerationDto>> GetApplianceMaterialsAsync();
    }
}
