using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Repositories;

namespace IFPS.Factory.Application.Services
{
    public class ApplianceMaterialAppService : ApplicationService, IApplianceMaterialAppService
    {
        private readonly IApplianceMaterialRepository applianceMaterialRepository;

        public ApplianceMaterialAppService(
            IApplicationServiceDependencyAggregate aggregate,
            IApplianceMaterialRepository applianceMaterialRepository)
            : base(aggregate)
        {
            this.applianceMaterialRepository = applianceMaterialRepository;
        }

        public async Task<Guid> CreateApplianceFromFileAsync(ApplianceMaterialCreateFromGeneratedDataDto dto)
        {
            var applianceMaterial = dto.CreateModelObject();

            var newAppMat = await applianceMaterialRepository.InsertAsync(applianceMaterial);
            await unitOfWork.SaveChangesAsync();

            return newAppMat.Id;
        }

        public async Task<List<ApplianceMaterialLisForDataGenerationDto>> GetApplianceMaterialsAsync()
        {
            var appliances = await applianceMaterialRepository.GetAllListAsync();
            return appliances.Select(ent => new ApplianceMaterialLisForDataGenerationDto(ent)).ToList();
        }
    }
}
