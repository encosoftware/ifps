using ENCO.DDD.Service;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class PlanAppService : ApplicationService, IPlanAppService
    {
        private readonly ICameraRepository cameraRepository;
        private readonly IPlanRepository planRepository;

        public PlanAppService(
              IApplicationServiceDependencyAggregate aggregate
            , ICameraRepository cameraRepository
            , IPlanRepository planRepository
            ) : base(aggregate)
        {
            this.cameraRepository = cameraRepository;
            this.planRepository = planRepository;
        }

        public async Task SetPlanProductionProcessTimeAsync(string ipAddress, int cfcId)
        {
            Camera camera = await GetCameraAsync(ipAddress);

            Expression<Func<Plan, bool>> filter = (Plan p) => p.ConcreteFurnitureComponentId == cfcId &&
            p.WorkStation.WorkStationType.StationType == camera.WorkStationCamera.WorkStation.WorkStationType.StationType;

            Plan plan = await GetPlanAsync(filter);

            if (camera.WorkStationCamera.CFCProductionState.State == CFCProductionStateEnum.Started)
                plan.ProductionProcess.StartTime = Clock.Now;

            if (camera.WorkStationCamera.CFCProductionState.State == CFCProductionStateEnum.Finished)
                plan.ProductionProcess.EndTime = Clock.Now;

            await unitOfWork.SaveChangesAsync();
        }

        private async Task<Plan> GetPlanAsync(Expression<Func<Plan, bool>> filter)
        {
            return await planRepository.SingleIncludingAsync(filter, p => p.ProductionProcess);
        }

        private async Task<Camera> GetCameraAsync(string ipAddress)
        {
            return await cameraRepository.SingleIncludingAsync(ent => ent.IPAddress == ipAddress, ent => ent.WorkStationCamera.WorkStation.WorkStationType, ent => ent.WorkStationCamera.CFCProductionState);
        }
    }
}
